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

            // --- FIX: Correct Gender Radio Button Selection ---
            const genderValueFromAPI = customer.Gender;

            // Use .toFixed(1) to force the number (e.g., 1) to be the string "1.0" 
            // to match the HTML attribute value="1.0"
            const genderString = parseFloat(genderValueFromAPI).toFixed(1);

            // Use the consistently formatted string in the selector
            $(`input[name="GenderOption"][value="${genderString}"]`).prop('checked', true);

            // Load Edit Mode Fields
            $("#Status").val(customer.Status);
            $("#Credit_Score").val(customer.Credit_Score);

        },
        error: function (xhr) {
            alert("Error loading customer data for editing. Check API status.");
            console.error("Load Error:", xhr.responseText);
        }
    });
}

// --- 2. Save Function (Handles CREATE/UPDATE POST) ---
function saveCustomer(isEditMode, customerId) {

    // Determine API endpoint and data payload based on mode
    const apiEndpoint = isEditMode
        ? API_BASE_URL + "/customer/update"    // POST api/customer/update
        : API_BASE_URL + "/customer/create";   // POST api/customer/create

    // 1. Gather all required form data
    const customerData = {
        "Name": $("#Name").val(),
        "Address": $("#Address").val(),
        "Email": $("#Email").val(),
        "Gender": parseFloat($('input[name="GenderOption"]:checked').val()),
        "Card_Number": $("#Card_Number").val(),
        "Monthly_Income": parseFloat($("#Monthly_Income").val())
    };

    // If it's EDIT mode, add required update fields
    if (isEditMode) {
        customerData["Customer_Id"] = $("#CustomerId").val();
        customerData["Status"] = $("#Status").val();
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

// --- 3. Validation Function (Synchronous only) ---
function CheckValidation(isEditMode) {
    let SyncError = [];

    // Local Variables
    const name = $("#Name").val().trim();
    const email = $("#Email").val().trim();
    const cardNumber = $("#Card_Number").val().trim();
    const monthlyIncome = $("#Monthly_Income").val().trim();
    const gender = $('input[name="GenderOption"]:checked').val();
    const address = $("#Address").val().trim();

    let status = null;
    let creditScore = null;

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (email && !emailPattern.test(email)) {
        SyncError.push("Email format is invalid.");
    }

    // Gather Edit Mode fields
    if (isEditMode) {
        status = $("#Status").val();
        creditScore = $("#Credit_Score").val().trim();
    }

    // --- Validation Checks ---
    if (!name) { SyncError.push("Name is required."); }
    if (!email) { SyncError.push("Email is required."); }
    if (!cardNumber || isNaN(cardNumber) || parseFloat(cardNumber) <= 0) {
        SyncError.push("A valid Card Number is required.");
    }
    if (!monthlyIncome || isNaN(monthlyIncome) || parseFloat(monthlyIncome) <= 0) {
        SyncError.push("Monthly Income must be a positive number.");
    }
    if (!gender) { SyncError.push("You must select a gender"); }
    if (!address) { SyncError.push("Address is required."); }

    if (isEditMode) {
        if (!status) {
            SyncError.push("Status is required.");
        }
        if (!creditScore || isNaN(creditScore) || parseFloat(creditScore) < 0) {
            SyncError.push("Credit Score must be a non-negative number.");
        }
    }

    // --- FIX: Return the error array directly (no Promise needed here) ---
    return SyncError;
}

// --- 4. DOM Ready Initialization ---
$(document).ready(function () {
    const customerId = $("#CustomerId").val();
    const isEditMode = customerId && parseInt(customerId) > 0;

    // 1. Check for Edit Mode and Load Data
    if (isEditMode) {
        loadCustomerData(customerId);
    }

    // 2. Attach Submission Handler
    $("#customerUpsertForm").on("submit", function (e) {
        e.preventDefault();

        // --- FIX: Call CheckValidation directly and handle result ---
        const errors = CheckValidation(isEditMode);

        if (errors.length > 0) {
            // Show Modal if errors exist
            const errorList = $("#errorList");
            errorList.empty();
            errors.forEach(function (error) {
                let li = document.createElement('li');
                li.textContent = error;
                errorList.append(li);
            })
            $('#validationModal').modal('show');
        }
        else {
            // Validation Passed: Proceed to save
            saveCustomer(isEditMode, customerId);
        }
    });
});