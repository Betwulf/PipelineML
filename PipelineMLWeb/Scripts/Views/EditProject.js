var starting_value;
var cvs;
var ctx;
var sinewaveList;
var editor; //JSON Editor
var project; // pipeline project view model
var pipelineCanvas; // javascript drawing functions

$(document).ready(function () {



    var CreateEditor = function () {
        //$.getJSON("/Notes/Data", starting_value, loadedData);
        // JSONEditor Setup params
        JSONEditor.defaults.options.theme = 'bootstrap3';
        JSONEditor.defaults.options.disable_array_add = 'true';
        JSONEditor.defaults.options.disable_array_delete = 'true';
        JSONEditor.defaults.options.disable_array_reorder = 'true';
        JSONEditor.defaults.options.disable_collapse = 'true';
        JSONEditor.defaults.options.disable_edit_json = 'true';
        JSONEditor.defaults.options.disable_properties = 'true';
        // Initialize the json editor with a JSON schema
        editor = new JSONEditor(document.getElementById('editor_holder'), {
            schema: {
                type: "object",
                title: "Car",
                properties: {
                    make: {
                        type: "string",
                        enum: [
                          "Toyota",
                          "BMW",
                          "Honda",
                          "Ford",
                          "Chevy",
                          "VW"
                        ]
                    },
                    model: {
                        type: "string"
                    },
                    year: {
                        type: "integer",
                        enum: [
                          1995, 1996, 1997, 1998, 1999,
                          2000, 2001, 2002, 2003, 2004,
                          2005, 2006, 2007, 2008, 2009,
                          2010, 2011, 2012, 2013, 2014
                        ],
                        default: 2008
                    }
                }
            }
        });
    };


    if (typeof PipelineCanvas === 'undefined') { throw new Error('JavaScript requires PipelineCanvas.js'); }
    pipelineCanvas = new PipelineCanvas();
    pipelineCanvas.startDrawing('main-canvas');

    // signalR

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("EditProjectHub");


    hub.on("OnGetProject", function (data) {
        project = data;
        $('#projectName').text('Project: ' + project.Name);
        pipelineCanvas.startDrawingProject(project);
    });

    conn.start(function () {
        console.log('Got project id:' + projectId);
        hub.invoke('GetProject', projectId);
    });

    pipelineCanvas.getCreatePipelinePartFunction(function (data) {
        console.log('Create Pipeline Part:' + data.classType);
        hub.invoke('CreatePipelinePart', data);
    });
});
