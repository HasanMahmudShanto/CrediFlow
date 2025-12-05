
const API_BASE_URL = "https://localhost:44387/api";

// --- 1. Load Data Function (GET) ---
function loadCustomerData(id) {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/customer/customer/" + id, // GET api/customer/customer/{id}
        dataType: "json",
        success: function (customer) {
            // Populate form fields
            $("#Name").val(customer.Name);
            $("#Address").val(customer.Address);
            $("#Email").val(customer.Email);
            $("#Card_Number").val(customer.Card_Number);
            $("#Monthly_Income").val(customer.Monthly_Income);

            // NOTE: Status and Credit_Score fields should be hidden or disabled if they are system-generated
            // However, since you included them in the Edit form[cite: 5], we load them:
            $("#Status").val(customer.Status);
            $("#Credit_Score").val(customer.Credit_Score);

            // Set the correct radio button
            $(`input[name="GenderOption"][value="${customer.Gender}"]`).prop('checked', true);
        },
        error: function (xhr) {
            alert("Error loading customer data for editing. Check API status.");
            console.error("Load Error:", xhr.responseText);
        }
    });
}

// --- 2. Save Function (Handles CREATE/UPDATE POST) ---
// We pass the isEditMode flag and the customerId directly into this function
function saveCustomer(isEditMode, customerId) {

    // Determine API endpoint and data payload based on mode
    const apiEndpoint = isEditMode
        ? API_BASE_URL + "/customer/update"    // POST api/customer/update
        : API_BASE_URL + "/customer/create";   // POST api/customer/create

    // 1. Gather all required form data
    const customerData = {
        // Only required for UPDATE. If CREATE, it's ignored or 0.
        
        "Name": $("#Name").val(),
        "Address": $("#Address").val(),
        "Email": $("#Email").val(),
        "Gender": parseFloat($('input[name="GenderOption"]:checked').val()),
        "Card_Number": $("#Card_Number").val(),
        "Monthly_Income": parseFloat($("#Monthly_Income").val())
    };

    // If it's EDIT mode, the API might also need Status and Credit_Score (as they are in your update payload [cite: 5])
    if (isEditMode) {
        customerData["Customer_Id"] = $("#CustomerId").val();
        // Add the system-managed fields required for update payload
        customerData["Status"] = $("#Status").val();
        // Use parseFloat or ensure your API can handle the int if Credit_Score is float
        customerData["Credit_Score"] = parseFloat($("#Credit_Score").val());
    }

    // 2. Perform the AJAX call
    $.ajax({
        type: "POST",
        url: apiEndpoint,
        data: JSON.stringify(customerData), // Data must be stringified JSON
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            alert(`Customer ${response.Name} saved successfully!`);
            window.location.href = redirectUrl;
        },
        error: function (xhr) {
            alert("Error saving customer. Check browser console for network or 500 errors.");
            console.error("Save Error:", xhr.responseText);
        }
    });
}

// --- 3. DOM Ready Initialization ---
$(document).ready(function () {
    const customerId = $("#CustomerId").val();
    const isEditMode = customerId && parseInt(customerId) > 0;

    // Attach the correct CustomerId value to the hidden field, derived from ViewBag
    // NOTE: Your Upsert.cshtml uses C# if/else, so the ID must be placed in the hidden field in the Edit block
    // <div class="form-group"><label>Customer Id</label><input type="text" class="form-control" id="CustomerId" placeholder= "@CustomerId" disabled></div> 

    // 1. Check for Edit Mode and Load Data
    if (isEditMode) {
        // Load the existing data into the form fields
        loadCustomerData(customerId);
    } 

    // 2. Attach Submission Handler
    $("#customerUpsertForm").on("submit", function (e) {
        e.preventDefault();
        // Pass the calculated mode and ID to the save function
        saveCustomer(isEditMode, customerId);
    });
});