const API_BASE_URL = "https://localhost:44387/api";

// Global map to store customer IDs and Names
let customerIdToNameMap = {};

// --- 1. Load Customers and Populate Map ---
function loadCustomerMap(callback) {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/customer/all",
        dataType: "json",
        success: function (customers) {
            customers.forEach(customer => {
                customerIdToNameMap[customer.Customer_Id] = customer.Name;
            });
            console.log("Customer map loaded:", customerIdToNameMap);
            // Execute the callback function (which will be GetAllNotification)
            if (callback) {
                callback();
            }
        },
        error: function (error) {
            console.error("Error loading customer data:", error);
            // Even on error, still try to load notifications
            if (callback) {
                callback();
            }
        }
    });
}

// --- 2. Display Notification Table ---
function displayNotification(data) {
    const $tableBody = $("#notificationTableBody");
    $tableBody.empty();
    let rowNumber = 1;

    if (data && data.length > 0) {
        $.each(data, function (index, notification) {

            // LOOKUP STEP: Get the name from the map using the ID
            const customerName = customerIdToNameMap[notification.Customer_Id] || "Unknown Customer";

            const row = `
            <tr>
                <td>${rowNumber++}</td>
                <td>${notification.Notification_Id}</td>
                <td>${notification.Title}</td>
                <td>${notification.Date}</td>
                
                <td>${customerName}</td> 
                
                <td>${notification.Is_Read}</td>
                <td>
                    <a href="${upsertUrlBase}?id=${notification.Notification_Id}" class="btn btn-sm btn-info">Edit</a> |
                    <button class="btn btn-sm btn-danger delete-button" onclick="DeleteNotification(${notification.Notification_Id})" data-id="${notification.Notification_Id}">Delete</button>
                </td>
            </tr>`;
            $tableBody.append(row);
        });
    } else {
        $tableBody.append('<tr><td colspan="7" class="text-center">No notifications found in the API.</td></tr>');
    }
}


// --- 3. Get All Notifications (Runs after customer map is ready) ---
function GetAllNotification() {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/notification/allpartial",
        contentType: "application/json; charset = utf-8",
        dataType: "json",
        success: function (data) {
            console.log("Notification data fetched successfully: ", data);
            displayNotification(data);
        },
        error: function (error) {
            console.log("Error fetching notifications: ", error);
        }

    });

}


function DeleteNotification(Id) {
    if (confirm("WARNING: Are you sure you want to delete notification ID " + Id + "? This action cannot be undone.")) {

        $.ajax({
            type: "POST",
            url: API_BASE_URL + "/notification/delete/" + Id,
            success: function (response) {
                alert("Notification ID " + Id + " deleted successfully!");
                // Refresh by reloading the page's main data flow
                loadCustomerMap(GetAllNotification);
            },
            error: function (xhr) {
                alert("Error deleting notification. Check console.");
                console.error("Deletion Error:", xhr.responseText);
            }
        });
    }
}


// --- 4. Initialization (Load map, then load notifications) ---
$(document).ready(function () {
    // Start the process: Load the customer map first, then execute GetAllNotification as the callback
    loadCustomerMap(GetAllNotification);
});