const API_BASE_URL = "https://localhost:44387/api";


function displayNotification(data) {
    const $tableBody = $("#notificationTableBody");
    $tableBody.empty();
    let rowNumber = 1;
    if (data && data.length > 0) {
        $.each(data, function (index, notification) {
            const row = `
            <tr>
                <td>${rowNumber++}</td>
                <td>${notification.Notification_Id}</td>
                <td>${notification.Title}</td>
                <td>${notification.Message}</td>
                <td>${notification.Date}</td>
                <td>${notification.Customer_Id}</td>
                <td>${notification.Is_Read}</td>
                <td>
                    <a href="${upsertUrlBase}?id=${notification.Notification_Id}" class="btn btn-sm btn-warning">Edit</a> |
                    <button class="btn btn-sm btn-danger delete-button" onclick="DeleteNotification(${notification.Notification_Id})" data-id="${notification.Notification_Id}">Delete</button>
                </td>
            </tr>`;
            $tableBody.append(row);
        });
    } else {
        $tableBody.append('<tr><td colspan="5" class="text-center">No notifications found in the API.</td></tr>');
    }
}


function GetAllNotification() {
    $.ajax({
        type: "GET",
        url: API_BASE_URL + "/notification/all",
        contentType: "application/json; charset = utf-8",
        dataType: "json",
        success: function (data) {
            console.log("Data fetched successfully: ", data);
            displayNotification(data);
        },
        error: function (error) {
            console.log("Error fetching notifications: ", error);
        }

    });

}


function DeleteNotification(Id) {
    if (confirm("WARNING: Are you sure you want to delete notification ID " + Id + "? This action cannot be undone.")) {

        // ** NOTE: Use the API_BASE_URL defined at the top of app.js **
        $.ajax({
            type: "POST",
            url: API_BASE_URL + "/notification/delete/" + Id, // Endpoint with URL parameter
            success: function (response) {
                // Response is Success or error message
                alert("Notification ID " + Id + " deleted successfully!");
                GetAllNotification(); // Refresh the table
            },
            error: function (xhr) {
                alert("Error deleting notification. Check console.");
                console.error("Deletion Error:", xhr.responseText);
            }
        });
    }
}




$(document).ready(function () {
    GetAllNotification();
});