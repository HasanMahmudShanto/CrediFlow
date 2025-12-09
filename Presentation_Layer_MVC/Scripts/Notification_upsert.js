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
        "Date": $("#Date").val(),
        // Ensure Customer_Id is a number if API expects it
        "Customer_Id": parseInt($("#Customer_Id").val(), 10)
    };

    // 2. Perform AJAX call
    $.ajax({
        type: "POST",
        url: apiEndpoint,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(notificationData),
        success: function (response) {
            alert("Notification " + (isEditMode ? "updated" : "created") + " successfully!");
            window.location.href = window.redirectUrl; // Use window.redirectUrl
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

function CheckCustomerAvailability(customerId) {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/customer/" + customerId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            return true; // Customer exists
        },
        error: function (xhr) {
            return false; // Customer does not exist
        }
    });

}


// CheckValidation function now returns an array of errors
function CheckValidation() {
    let errors = [];

    // NOTE: Use the correct case-sensitive IDs from your Upsert.cshtml!
    const title = $("#Title").val().trim();
    const message = $("#Message").val().trim();
    const dateVal = $("#Date").val();
    const customerId = $("#Customer_Id").val();

    if (!title) {
        errors.push("Title is required.");
    }
    if (!message) {
        errors.push("Message cannot be empty.");
    }
    if (!isValidDate(dateVal)) {
        errors.push("Please select a valid Date and Time.");
    }
    // Check if ID is present and is a positive number
    if (CheckCustomerAvailability(customerId)) {
        errors.push("A valid Customer ID is required.");
    }

    return errors;
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

        const errors = CheckValidation(); // Run validation first

        if (errors.length > 0) {
            // Display errors in the modal
            const errorList = document.getElementById('errorList');
            errorList.innerHTML = "";
            errors.forEach(function (error) {
                let li = document.createElement('li');
                li.textContent = error;
                errorList.appendChild(li);
            });
            // Show the Bootstrap Modal (ID: validationModal)
            $('#validationModal').modal('show');

        } else {
            // Validation Passed: Proceed with AJAX call
            saveNotification(isEditMode, notificationId);
        }
    });
});