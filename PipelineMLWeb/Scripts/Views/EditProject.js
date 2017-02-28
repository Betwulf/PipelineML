var starting_value;
var cvs;
var ctx;
var sinewaveList;
var editor; //JSON Editor
var project; // pipeline project view model
var pipelineCanvas; // javascript drawing functions
var editingData;
var resetError;
var onError;

$(document).ready(function () {

    // the function to receive editing info for a pipeline part that the user selected to edit
    var editPipelinePart = function (data) {
        //TODO: Fix editor, dispose of old one if any?
        editingData = data;
        createEditor(JSON.parse(data.schemaJSON), JSON.parse(data.dataJSON));
    };

    onError = function (data) {
        console.log("OnError: " + data.ErrorMessage);
        $('#projectStateTooltip').attr('data-original-title', data.ErrorMessage);
        $('#projectStateTooltip').tooltip();
        $('#projectStateTooltip').text("details");
        $('#projectState').removeProp('hidden');
        $('#projectStateText').text(data.FriendlyMessage);
    }

    resetError = function () {
        console.log("resetError Called");
        $('#projectStateTooltip').attr('data-original-title', "");
        $('#projectStateTooltip').tooltip();
        $('#projectStateTooltip').text("details");
        $('#projectState').prop('hidden');
        $('#projectStateText').text('');
    }

    var createEditor = function (schema, data) {
        resetError();
        var $div = $('#editor_holder');
        $div.html('');

        starting_value = data;
        // JSONEditor Setup params
        JSONEditor.defaults.options.theme = 'bootstrap3';
        JSONEditor.defaults.options.iconlib = 'bootstrap3';
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
        resetError();
        project = data;
        $('#projectName').text('Project: ' + project.Name);
        pipelineCanvas.startDrawingProject(project);
    });

    hub.on("OnEditPipelinePart", function (data) {
        console.log("OnEditPipelinePart: " + data.classType);
        editPipelinePart(data);
    });
    hub.on("OnError", function (data) {
        onError(data);
    });

    
    conn.start(function () {
        console.log('Got project id:' + projectId);
        hub.invoke('GetProject', projectId);
    });



    pipelineCanvas.getCreatePipelinePartFunction(function (data) {
        resetError();
        console.log('Create Pipeline Part:' + data.classType);
        hub.invoke('CreatePipelinePart', data);
    });

    pipelineCanvas.getEditPipelinePartFunction(function (data) {
        resetError();
        console.log('Edit Pipeline Part:' + data.classType);
        hub.invoke('GetPipelinePartAndSchema', data);
    });

    pipelineCanvas.getOnErrorFunction(function (data) {
        onError(data);
    });

    
    pipelineCanvas.getResetErrorFunction(function (data) {
        resetError();
    });
});
