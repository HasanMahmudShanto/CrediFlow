const API_BASE_URL = "https://localhost:44387/api";

function displayCustomers(customers) {
    const $tableBody = $("#customerTableBody");
    $tableBody.empty();
    let rowNumber = 1;
    if (customers && customers.length > 0) {
        $.each(customers, function (index, customer) {
            const row = `
            <tr>
                <td>${rowNumber++}</td>
                <td>${customer.Customer_Id}</td>
                <td>${customer.Name}</td>
                <td>${customer.Credit_Score}</td>
                <td>${customer.Gender}</td>
                <td>${customer.Address}</td>
                <td>${customer.Card_Number}</td>
                <td>$${customer.Monthly_Income.toFixed(2)}</td>
                <td>${customer.Status}</td>
                <td>${customer.Email}</td>
                <td>
                    <a href="${upsertUrlBase}?id=${customer.Customer_Id}" class="btn btn-sm btn-info">Edit</a>
                    <button class="btn btn-sm btn-danger delete-button" onclick="DeleteCustomer(${customer.Customer_Id})" data-id="${customer.Customer_Id}">Delete</button>
                </td>
             </tr>`;
            $tableBody.append(row);
        });
    } else {
        $tableBody.append('<tr><td colspan="11" class="text-center">No customers found in the API.</td></tr>');
    }
}


function GetAllCustomers() {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/customer/all",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log("Data fetched successfully: ", data);
            displayCustomers(data);
        },
        error: function (error) {
            console.log("Error fetching customers: ", error);
        }
    });
}

function DeleteCustomer(customerId) {
    if (confirm("WARNING: Are you sure you want to delete customer ID " + customerId + "? This action cannot be undone.")) {

        // ** NOTE: Use the API_BASE_URL defined at the top of app.js **
        $.ajax({
            type: "POST",
            url: API_BASE_URL + "/customer/delete/" + customerId, // Endpoint with URL parameter
            success: function (response) {
                // Response is Success or error message
                alert("Customer ID " + customerId + " deleted successfully!");
                GetAllCustomers(); // Refresh the table
            },
            error: function (xhr) {
                alert("Error deleting customer. Check console.");
                console.error("Deletion Error:", xhr.responseText);
            }
        });
    }
}



$(document).ready(function () {
    GetAllCustomers();
});

