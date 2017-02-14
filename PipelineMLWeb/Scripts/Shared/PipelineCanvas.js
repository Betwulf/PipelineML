﻿var PipelineCanvas = function () {

    var canvasId;
    var canvasOffsetX;
    var canvasOffsetY;
    var isDrawingProject = false;

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




    var projectModel; // the view model holding all details of the project
    var boxScale = 1.0;
    var boxMargin = 20;
    var boxOffsetX = 0;
    var boxOffsetY = 0;
    var boxHeight = 60;
    var boxWidth = 180;
    var boxFont = '12pt Arial';
    var boxFontHeight = 12;
    var boxColor = 'rgba(255, 255, 255, 1)';
    var boxTextColor = 'white';
    var hitBoxes = []; // used for mouse events
    var classTypes = null;


    // used to build the hitbox list
    function getHitBox(x, y, w, h, column, id, classname) {
        this.x = x;
        this.y = y;
        this.xw = x + w;
        this.yh = y + h;
        this.column = column;
        this.id = id;
        this.classname = classname;
    }



    function drawPlus(x,y,w,h)
    {
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


    function drawBox(x,y,w,h,boxText)
    {
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

        // DataGenerator Stack
        if (projectModel.DataGeneratorPart !== null) {
            projectModel.DataGeneratorPart.x = currX;
            projectModel.DataGeneratorPart.y = currY;
            projectModel.DataGeneratorPart.xw = currX + boxWidth;
            projectModel.DataGeneratorPart.yh = currY + boxHeight;
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.DataGeneratorPart.Name);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 0, projectModel.DataGeneratorPart.Id, projectModel.DataGeneratorPart.ClassName));
        }
        else {
            drawPlus(currX, currY, boxWidth, boxHeight);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 0, null, classTypes.DatasetGenerator));
        }

        // Preprocess Stack
        currX = currX + boxWidth + boxMargin;
        currY = boxOffsetY + boxMargin;
        for (part in projectModel.PreProcessParts) {
            projectModel.PreProcessParts[part].x = currX;
            projectModel.PreProcessParts[part].y = currY;
            projectModel.PreProcessParts[part].xw = currX + boxWidth;
            projectModel.PreProcessParts[part].yh = currY + boxHeight;
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.PreProcessParts[part].Name);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 1, projectModel.PreProcessParts[part].Id, projectModel.PreProcessParts[part].ClassName));
            currY = currY + boxHeight + boxMargin;
        }
        if (projectModel.PreProcessParts.length == 0)
        {
            drawPlus(currX, currY, boxWidth, boxHeight);
            hitBoxes.push(new getHitBox(currX, currY, boxWidth, boxHeight, 1, null, classTypes.DataTransform));
        }
        // then plus

    }



    function handleMouseDown(e) {
        e.preventDefault();
        e.stopPropagation();
        mouseX = parseInt(e.clientX - canvasOffsetX);
        mouseY = parseInt(e.clientY - canvasOffsetY);
        console.log("Testing (" + mouseX + ", " + mouseY + ")");
        for (boxnum in hitBoxes)
        {
            var box = hitBoxes[boxnum];
            console.log("box:" + box.x + ", " + box.y + " - " + box.xw + ", " + box.yh);
            if (mouseX >= box.x && mouseX <= box.xw && mouseY >= box.y && mouseY <= box.yh)
            {
                // THEN HIT!
                console.log("HIT: " + box.id)
            }
        }
    };



    function handleMouseMove(e) {
        e.preventDefault();
        e.stopPropagation();
        mouseX = parseInt(e.clientX - canvasOffsetX);
        mouseY = parseInt(e.clientY - canvasOffsetY);
        // TODO: capture hitboxes and test
    };


    // Starts drawing the project elements when data is loaded
    startDrawingProject = function (model) {
        projectModel = model;
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
        if (isDrawingProject)
        {
            //TODO: Project drawing stuff goes here
            drawBoxes();
        }

        requestAnimationFrame(redraw);
    }



    getClassTypes = function (aClassTypes) {
        classTypes = aClassTypes;
    };


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


    return {
        startDrawing: startDrawing,
        startDrawingProject: startDrawingProject,
        getClassTypes: getClassTypes
    };

};