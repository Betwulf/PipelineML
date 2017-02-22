var PipelineCanvas = function () {

    // Canvas Variables 
    var canvasId;
    var canvasOffsetX;
    var canvasOffsetY;
    var isDrawingProject = false;

    // Data and Functions
    var projectModel; // the view model holding all details of the project
    var classTypes = null; //  for convenience - part of the project model that provides class types.
    var createPipelinePart = null; // passed in function to call when creating a pipeline part
    var editPipelinePart = null; // passed in function to call when a user wants to edit a pipeline part

    // Box Variables
    var boxScale = 1.0;
    var boxMargin = 20;
    var boxOffsetX = 0;
    var boxOffsetY = 0;
    var boxHeight = 60;
    var boxWidth = 180;
    var boxFont = '12pt Arial';
    var boxFontHeight = 12;
    var boxColor = 'rgba(200, 200, 200, 1)';
    var boxTextColor = 'white';
    var hitBoxes = []; // used for mouse events



    // Build Sinewave object
    function getWave(periodicity, height, color, xoffset, yoffset) {
        this.xoffset = xoffset;
        this.yoffset = yoffset;
        this.height = height;
        this.color = color;
        this.periodicity = periodicity;
        this.points = [];
    }


    // change wave shapes
    function updateWave(sinewave) {
        var nowTime = new Date();
        var currentPhase = (nowTime.getMinutes() * 60 + nowTime.getSeconds() + nowTime.getMilliseconds() / 1000.0) * 6;

        sinewave.points = [
            { x: 0, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset) * Math.PI / 180) + sinewave.yoffset },
            { x: cvs.width * 1 / 3, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset + 120) * Math.PI / 180) + sinewave.yoffset },
            { x: cvs.width * 2 / 3, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset + 240) * Math.PI / 180) + sinewave.yoffset },
            { x: cvs.width, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset + 360) * Math.PI / 180) + sinewave.yoffset }];
    }



    // drawWave
    function drawWave(sinewave) {
        ctx.lineWidth = 1;
        ctx.shadowOffsetX = 0;
        ctx.shadowOffsetY = 0;
        ctx.shadowBlur = 10;
        ctx.shadowColor = 'rgba(255, 255, 255, 0.4)';

        ctx.beginPath();
        ctx.moveTo(sinewave.points[0].x, sinewave.points[0].y);
        ctx.bezierCurveTo(
         sinewave.points[1].x, sinewave.points[1].y,
         sinewave.points[2].x, sinewave.points[2].y,
         sinewave.points[3].x, sinewave.points[3].y
        );
        ctx.strokeStyle = sinewave.color;
        ctx.stroke();
        ctx.shadowColor = 'rgba(255, 255, 255, 0)';
        ctx.closePath();
    }




    // used to build the hitbox list
    function getHitBox(x, y, w, h, column, id, classType) {
        this.x = x;
        this.y = y;
        this.xw = x + w;
        this.yh = y + h;
        this.column = column;
        this.id = id;
        this.classType = classType;
    }



    function drawPlus(x, y, w, h) {
        ctx.beginPath();
        ctx.strokeStyle = boxColor;
        ctx.lineWidth = 6;
        ctx.moveTo(x + (w / 2), y);
        ctx.lineTo(x + (w / 2), y + h);
        ctx.moveTo(x + (w / 2) - (h / 2), y + (h / 2));
        ctx.lineTo(x + (w / 2) + (h / 2), y + (h / 2));
        ctx.stroke();
        ctx.closePath();
    }


    function drawBox(x, y, w, h, boxText) {
        ctx.beginPath();
        ctx.strokeStyle = boxColor;
        ctx.lineWidth = 2;
        ctx.moveTo(x, y);
        ctx.lineTo(x + w, y);
        ctx.lineTo(x + w, y + h);
        ctx.lineTo(x, y + h);
        ctx.lineTo(x, y);
        ctx.stroke();
        ctx.closePath();
        ctx.font = boxFont;
        ctx.textAlign = "center";
        var txtSize = ctx.measureText(boxText);
        ctx.fillStyle = boxTextColor;
        ctx.fillText(boxText, x + (w / 2), y + (h / 2) + boxFontHeight / 2);
    }


    function drawBoxes() {
        if (classTypes === null) return;
        hitBoxes = [];
        // first get dimensions
        var currX = boxOffsetX + boxMargin;
        var currY = boxOffsetY + boxMargin;
        boxHeight = boxHeight * boxScale;
        boxWidth = boxWidth * boxScale;

        // DataGenerator Column
        if (projectModel.DataGeneratorPart !== null) {
            projectModel.DataGeneratorPart.x = currX;
            projectModel.DataGeneratorPart.y = currY;
            projectModel.DataGeneratorPart.xw = currX + boxWidth;
            projectModel.DataGeneratorPart.yh = currY + boxHeight;
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.DataGeneratorPart.Name);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 0, projectModel.DataGeneratorPart.Id, projectModel.DataGeneratorPart.ClassType));
        }
        else {
            drawPlus(currX, currY, boxWidth, boxHeight);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 0, null, classTypes.DatasetGenerator));
        }

        // Preprocess Column
        currX = currX + boxWidth + boxMargin;
        currY = boxOffsetY + boxMargin;
        for (part in projectModel.PreProcessParts) {
            projectModel.PreProcessParts[part].x = currX;
            projectModel.PreProcessParts[part].y = currY;
            projectModel.PreProcessParts[part].xw = currX + boxWidth;
            projectModel.PreProcessParts[part].yh = currY + boxHeight;
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.PreProcessParts[part].Name);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 1, projectModel.PreProcessParts[part].Id, projectModel.PreProcessParts[part].ClassType));
            currY = currY + boxHeight + boxMargin;
        }
        if (projectModel.PreProcessParts.length === 0) {
            drawPlus(currX, currY, boxWidth, boxHeight);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 1, null, classTypes.DataTransform));
        }


        // Machine Learning Column
        currX = currX + boxWidth + boxMargin;
        currY = boxOffsetY + boxMargin;
        for (part in projectModel.MLParts) {
            projectModel.MLParts[part].x = currX;
            projectModel.MLParts[part].y = currY;
            projectModel.MLParts[part].xw = currX + boxWidth;
            projectModel.MLParts[part].yh = currY + boxHeight;
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.MLParts[part].Name);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 2, projectModel.MLParts[part].Id, projectModel.MLParts[part].ClassType));
            currY = currY + boxHeight + boxMargin;
        }
        if (projectModel.MLParts.length === 0) {
            drawPlus(currX, currY, boxWidth, boxHeight);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 2, null, classTypes.MachineLearning));
        }


        // Postprocess Column
        currX = currX + boxWidth + boxMargin;
        currY = boxOffsetY + boxMargin;
        for (part in projectModel.PostProcessParts) {
            projectModel.PostProcessParts[part].x = currX;
            projectModel.PostProcessParts[part].y = currY;
            projectModel.PostProcessParts[part].xw = currX + boxWidth;
            projectModel.PostProcessParts[part].yh = currY + boxHeight;
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.PostProcessParts[part].Name);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 3, projectModel.PostProcessParts[part].Id, projectModel.PostProcessParts[part].ClassType));
            currY = currY + boxHeight + boxMargin;
        }
        if (projectModel.PostProcessParts.length === 0) {
            drawPlus(currX, currY, boxWidth, boxHeight);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 3, null, classTypes.DataTransform));
        }



        // Evaluator Column
        currX = currX + boxWidth + boxMargin;
        currY = boxOffsetY + boxMargin;
        for (part in projectModel.EvalutorParts) {
            projectModel.EvalutorParts[part].x = currX;
            projectModel.EvalutorParts[part].y = currY;
            projectModel.EvalutorParts[part].xw = currX + boxWidth;
            projectModel.EvalutorParts[part].yh = currY + boxHeight;
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.EvalutorParts[part].Name);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 4, projectModel.EvalutorParts[part].Id, projectModel.EvalutorParts[part].ClassType));
            currY = currY + boxHeight + boxMargin;
        }
        if (projectModel.EvalutorParts.length === 0) {
            drawPlus(currX, currY, boxWidth, boxHeight);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 4, null, classTypes.Evaluator));
        }
    }

    function handleCreatePipelinePart(event)
    {
        console.log("Create: " + event.data.id);
        var $div = $('#editor_holder');
        $div.html('');
        createPipelinePart( { projectId: projectModel.Id, classType: event.data.id, columNumber: event.data.column });

    }


    function handleMouseDown(e) {
        e.preventDefault();
        e.stopPropagation();
        mouseX = parseInt(e.clientX - canvasOffsetX);
        mouseY = parseInt(e.clientY - canvasOffsetY);
        for (boxnum in hitBoxes) {
            var box = hitBoxes[boxnum];
            if (mouseX >= box.x && mouseX <= box.xw && mouseY >= box.y && mouseY <= box.yh) {
                // THEN HIT!
                console.log("HIT: " + box.classType);
                if (box.id === null) {
                    // no object here, just a plus sign, so let them add!
                    var $div = $('#editor_holder');
                    $div.html('');
                    var form = $('<div></div>').attr("id", 'selectType').attr("class", 'list-group');
                    $.each(classTypes.PipelineParts[box.classType], function (key, value) {
                        console.log("Adding: " + value.FriendlyName);
                        $("<a href='#' class='list-group-item' value='" + value.FriendlyName + "' >")
                        .attr("id", value.ClassType)
                        .attr("name", value.FriendlyName)
                        .text(value.FriendlyName)
                        .on('click', { id: value.ClassType, column: box.column }, handleCreatePipelinePart)
                        .appendTo(form);

                    });

                    $div.append('<h3>Select Type:</h3>');
                    $(form).appendTo($div);
                }
                else {
                    // they want to edit an object
                    editPipelinePart({ projectId: projectModel.Id, pipelinePartId: box.id, classType: box.classType, columNumber: box.column });
                }
            }
        }
    }



    function handleMouseMove(e) {
        e.preventDefault();
        e.stopPropagation();
        mouseX = parseInt(e.clientX - canvasOffsetX);
        mouseY = parseInt(e.clientY - canvasOffsetY);
        // TODO: capture hitboxes and test
    }


    // Starts drawing the project elements when data is loaded
    startDrawingProject = function (model) {
        projectModel = model;
        classTypes = projectModel.ClassTypes;
        isDrawingProject = true;
    };







    // Runs each time the DOM window resize event fires.
    // Resets the canvas dimensions to match window,
    // then draws the new borders accordingly.
    function resizeCanvas() {
        cvs.width = $("#" + canvasId).parent().width();
        cvs.height = window.innerHeight - 96; // Adjustment for the navbar and project name height 
        redraw();
    }




    // Draw loop
    function redraw() {
        //Background
        ctx.beginPath();
        ctx.fillStyle = 'rgba(85, 85, 85, 1)';
        ctx.fillRect(0, 0, cvs.width, cvs.height); // refresh background
        for (aWave in sinewaveList) {
            updateWave(sinewaveList[aWave]);
            drawWave(sinewaveList[aWave]);
        }

        // draw project
        if (isDrawingProject) {
            //TODO: Project drawing stuff goes here
            drawBoxes();
        }

        requestAnimationFrame(redraw);
    }




    // Exported method call
    startDrawing = function (aCanvasId) {
        canvasId = aCanvasId;
        // Start the Canvas
        cvs = document.getElementById(canvasId);
        ctx = cvs.getContext('2d');
        var canvasOffset = $("#" + canvasId).offset();
        canvasOffsetX = canvasOffset.left;
        canvasOffsetY = canvasOffset.top;
        window.addEventListener('resize', resizeCanvas, false);
        $("#" + canvasId).mousedown(function (e) { handleMouseDown(e); });
        $("#" + canvasId).mousemove(function (e) { handleMouseMove(e); });
        resizeCanvas();

        // Create Canvas Background
        var halfHeight = cvs.height / 2 - 30;
        sinewaveList = [new getWave(0.5, halfHeight / 2 + 10, 'rgba(120, 120, 120, 1)', 0, halfHeight),
                        new getWave(0.4, halfHeight / 2 + 20, 'rgba(120, 120, 120, 1)', 100, halfHeight),
                        new getWave(0.3, halfHeight / 2 + 30, 'rgba(120, 120, 120, 1)', 150, halfHeight)
        ];
    };

    // Get the function we would need to call to create a new pipeline part that the user requested
    getCreatePipelinePartFunction = function (fn) 
    {
        createPipelinePart = fn;
    }

    // get the function to call when a user clicks on an existing pipeline part for editing
    getEditPipelinePartFunction = function (fn)
    {
        editPipelinePart = fn;
    }

    return {
        startDrawing: startDrawing,
        startDrawingProject: startDrawingProject,
        getCreatePipelinePartFunction: getCreatePipelinePartFunction,
        getEditPipelinePartFunction: getEditPipelinePartFunction
    };

};
