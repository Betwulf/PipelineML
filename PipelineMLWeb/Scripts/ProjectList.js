
// TODO: Take this out
var gViewModel; // for debugging in the browser

$(document).ready(function () {





    // signalR

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("ProjectListHub");
    hub.on("OnGetProjects", function (viewModel)
    {
        gViewModel = viewModel;
        if (viewModel !== null && viewModel.length > 0)
        {
            console.log("viewModel: " + viewModel);
            var row = "";
            for (var i = 0; i < viewModel.length ; i++) {
                row = row + "<tr><td>" + viewModel[i].Name + "</td>" +
                    "<td>" + viewModel[i].Description + "</td>" +
                    '<td><a href="/PipelineProject/Edit/' + viewModel[i].Id + '">Edit</a></td>' +
                    "</tr>";
            }
            $('#projectTable > tbody:last-child').after(row);
        }
        else {
            $('#projectTable > tbody:last-child').after('<tr><td colspan="6">No projects yet...</td></tr>');
        }

    });
    conn.start(function () {
        hub.invoke("GetProjects");
    });


});
