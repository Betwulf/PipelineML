var starting_value;
var cvs;
var ctx;
var sinewaveList;
var editor; //JSON Editor
var project; // pipeline project view model
var pipelineCanvas; // javascript drawing functions
var editingData;

$(document).ready(function () {

    // the function to receive editing info for a pipeline part that the user selected to edit
    var editPipelinePart = function (data) {
        //TODO: Fix editor, dispose of old one if any?
        editingData = data;
        createEditor(data.schemaJSON, data.dataJSON);
    };


    var createEditor = function (schema, data) {
        starting_value = data;
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
            schema: schema,

            // Seed the form with a starting value
            startval: starting_value,

            // Disable additional properties
            no_additional_properties: true,

            // Require all properties by default
            required_by_default: true
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

    hub.on("OnEditPipelinePart", function (data) {
        console.log("OnEditPipelinePart: " + data.classType)
        editPipelinePart(data);
    });
    
    conn.start(function () {
        console.log('Got project id:' + projectId);
        hub.invoke('GetProject', projectId);
    });



    pipelineCanvas.getCreatePipelinePartFunction(function (data) {
        console.log('Create Pipeline Part:' + data.classType);
        hub.invoke('CreatePipelinePart', data);
    });

    pipelineCanvas.getEditPipelinePartFunction(function (data) {
        console.log('Edit Pipeline Part:' + data.classType);
        hub.invoke('GetPipelinePartAndSchema', data);
    });
});
