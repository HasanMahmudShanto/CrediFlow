
const API_BASE_URL = "https://localhost:44387/api";


function formatToDateTimeLocal(isoString) {
    if (!isoString) return '';
    // This finds the part before the seconds or timezone marker (Z or +)
    // Example: "2025-10-12T10:30:00Z" becomes "2025-10-12T10:30"
    return isoString.substring(0, 16);
}

function loadNotificationData(id) {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/notification/" + id, 
        dataType: "json",
        success: function (notification) {
            // Populate form fields
            $("#Title").val(notification.Title);
            $("#Message").val(notification.Message);
            $("#Recipient").val(notification.Recipient);
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

    // Determine API endpoint and data payload based on mode
    const apiEndpoint = isEditMode
        ? API_BASE_URL + "/notification/update"    // POST api/notification/update
        : API_BASE_URL + "/notification/create";   // POST api/notification/create
    // 1. Gather all required form data
    const notificationData = {
        // Only required for UPDATE. If CREATE, it's ignored or 0.
        "Notification_Id": isEditMode ? notificationId : 0,
        "Title": $("#Title").val(),
        "Message": $("#Message").val(),
        "Recipient": $("#Recipient").val(),
        "Is_Read": $("#Is_Read").is(':checked'),
        "Date": $("#Date").val(),
        "Customer_Id": parseFloat($("#Customer_Id").val())
    };
    $.ajax({
        type: "POST",
        url: apiEndpoint,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(notificationData),
        success: function (response) {
            alert("Notification " + (isEditMode ? "updated" : "created") + " successfully!");
            // Optionally redirect back to list or clear form
            window.location.href = redirectUrl; // Adjust URL as needed
        },
        error: function (xhr) {
            alert("Error saving notification. Check console.");
            console.error("Save Error:", xhr.responseText);
        }
    });
        
    

}







$(document).ready(function () {
    const notificationId = parseInt($("#Notification_Id").val(), 10);
    const isEditMode = notificationId  > 0;

    if (isEditMode) {
        loadNotificationData(notificationId);
            
    }

    $("#NotificationUpsertForm").submit(function (event) {
        event.preventDefault();
        saveNotification(isEditMode, notificationId);

    });

});