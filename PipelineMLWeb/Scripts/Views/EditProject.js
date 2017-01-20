var starting_value;
var cvs;
var ctx;
var sinewaveList;
var partList;

$(document).ready(function () {



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
    var editor = new JSONEditor(document.getElementById('editor_holder'), {
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


    if (typeof PipelineCanvas === 'undefined') { throw new Error('JavaScript requires PipelineCanvas.js'); }
    var pipelineCanvas = new PipelineCanvas();
    pipelineCanvas.startDrawing('main-canvas');

    // signalR

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("EditProjectHub");
    hub.on("OnGetAvailableClassTypes", function (data) {
        partList = data;
    });
    hub.on("OnGetProject", function (data) {
        // TODO: Put project setup code here
    });
    conn.start(function () {
        hub.invoke('GetAvailableClassTypes');
        hub.invoke('GetProject', '0'); // TODO: put project ID here.
    });


});
