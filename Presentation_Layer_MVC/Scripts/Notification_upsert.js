// Scripts/Notification_upsert.js

const API_BASE_URL = "https://localhost:44387/api";

// Assuming redirectUrl is defined globally in Upsert.cshtml (window.redirectUrl)

function formatToDateTimeLocal(isoString) {
    if (!isoString) return '';
    return isoString.substring(0, 16);
}

function loadNotificationData(id) {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/notification/" + id,
        dataType: "json",
        success: function (notification) {
            $("#Title").val(notification.Title);
            $("#Message").val(notification.Message);
            // Assuming Recipient field is not used in your current form structure, skip or verify ID
            // $("#Recipient").val(notification.Recipient); 
            $("#Is_Read").prop('checked', notification.Is_Read);
            $("#Date").val(formatToDateTimeLocal(notification.Date));
            $("#Customer_Id").val(notification.Customer_Id);
        },
        error: function (xhr) {
            alert("Error loading notification data for editing. Check API status.");
            console.error("Load Error:", xhr.responseText);
        }
    });
}

function saveNotification(isEditMode, notificationId) {

    // Determine API endpoint
    const apiEndpoint = isEditMode
        ? API_BASE_URL + "/notification/update"
        : API_BASE_URL + "/notification/create";

    // 1. Gather all required form data (Using correct IDs from your CSHTML)
    const notificationData = {
        "Notification_Id": isEditMode ? notificationId : 0,
        "Title": $("#Title").val(),
        "Message": $("#Message").val(),
        // Note: Recipient is missing from the form/payload
        "Is_Read": $("#Is_Read").is(':checked'),
        
        // Ensure Customer_Id is a number if API expects it
        "Customer_Id": parseInt($("#Customer_Id").val(), 10)
    };
    if (isEditMode) {
        notificationData.Date = $("#Date").val();
    }
    // 2. Perform AJAX call
    $.ajax({
        type: "POST",
        url: apiEndpoint,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(notificationData),
        success: function (response) {
            alert("Notification " + (isEditMode ? "updated" : "created") + " successfully!");
            window.location.href = redirectUrl; // Use window.redirectUrl
        },
        error: function (xhr) {
            alert("Error saving notification. See console.");
            console.error("Save Error:", xhr.responseText);
        }
    });
}


function isValidDate(dateString) {
    if (!dateString) {
        return false;
    }

    // 1. Attempt to create a Date object from the string.
    const dateObject = new Date(dateString);

    // 2. Check if the result is NOT "Invalid Date".
    // getTime() returns NaN for an Invalid Date object.
    return !isNaN(dateObject.getTime());
}

// Scripts/Notification_upsert.js

function CheckCustomerAvailability(customerId) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "GET",
            url: API_BASE_URL + "/customer/customer/" + customerId, // Ensure the URL is correct
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            // --- CHECK THE DATA CONTENT ON SUCCESS ---
            success: function (data) {
                // If the data object is NOT null or NOT undefined, the customer exists.
                if (data && data.Customer_Id && data.Customer_Id === parseInt(customerId)) {
                    resolve(true); // Customer exists: SUCCESS
                } else {
                    // API returned 200 OK but with null/empty data (Customer not found)
                    resolve(false);
                }
            },

            // --- HANDLE TRUE ERROR (e.g., 404, 500) ---
            error: function (xhr) {
                // If we get an actual HTTP error (404, 500), assume the customer does not exist
                // or there's a problem with the API itself.
                resolve(false);
            }
        });
    });
}


// CheckValidation function now returns an array of errors
function CheckValidation(isEditMode) {
    let syncErrors = [];

    // --- SYNCHRONOUS CHECKS ---
    const title = $("#Title").val().trim();
    const message = $("#Message").val().trim();
    if (isEditMode){ const dateVal = $("#Date").val(); }
        
    
    const customerId = $("#Customer_Id").val();

    // ... (rest of synchronous checks: Message, isValidDate) ...
    if (!title) { syncErrors.push("Title is required."); }
    if (!message) { syncErrors.push("Message cannot be empty."); }
    if (isEditMode) {
        if (!isValidDate(dateVal)) { syncErrors.push("Please select a valid Date and Time."); }
    }
   
    if (!customerId || isNaN(parseInt(customerId)) || parseInt(customerId) <= 0) {
        syncErrors.push("A Customer ID is required.");
    }

    // --- ASYNCHRONOUS CHECK (Customer Existence) ---
    // Return a Promise that wraps the async customer availability check
    return new Promise((resolve) => {

        // If there are SYNC errors, resolve immediately with the errors array
        if (syncErrors.length > 0) {
            resolve(syncErrors);
            return;
        }

        // If no SYNC errors, run the ASYNC check
        CheckCustomerAvailability(customerId).then(customerExists => {
            if (!customerExists) {
                // If customer does NOT exist, add an error
                syncErrors.push("The entered Customer ID does not exist.");
            }
            // Resolve the promise with the final list of errors
            resolve(syncErrors);
        });
    });
}


$(document).ready(function () {
    const notificationId = parseInt($("#Notification_Id").val(), 10);
    const isEditMode = notificationId > 0;

    if (isEditMode) {
        loadNotificationData(notificationId);
    }

    // Attach form submission handler
    $("#NotificationUpsertForm").submit(function (event) {
        event.preventDefault(); // Stop default form submission

        // Call the validation function and wait for the Promise to resolve
        CheckValidation(isEditMode).then(errors => {
            if (errors.length > 0) {
                // Display errors in the modal (Same code as before)
                const errorList = document.getElementById('errorList');
                errorList.innerHTML = "";
                errors.forEach(function (error) {
                    let li = document.createElement('li');
                    li.textContent = error;
                    errorList.appendChild(li);
                });
                $('#validationModal').modal('show');

            } else {
                // Validation Passed: Proceed with AJAX call
                const notificationId = parseInt($("#Notification_Id").val(), 10);
                const isEditMode = notificationId > 0;
                saveNotification(isEditMode, notificationId);
            }
        });
    });
});