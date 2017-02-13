var PipelineCanvas = function () {

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
        //console.log("w: " + txtSize.width + " h: " + txtSize.height);
        ctx.fillStyle = boxTextColor;
        ctx.fillText(boxText, x + (w / 2), y + (h / 2) + boxFontHeight / 2);
    }


    function drawBoxes()
    {
        var currX = boxOffsetX + boxMargin;
        var currY = boxOffsetY + boxMargin;
        boxHeight = boxHeight * boxScale;
        boxWidth = boxWidth * boxScale;

        // first get dimensions
        if (projectModel.DataGeneratorPart !== null)
        {
            drawBox(currX, currY, boxWidth, boxHeight, projectModel.DataGeneratorPart.Name);
        }
        else
        {
            drawPlus(currX, currY, boxWidth, boxHeight);
        }
    }



    function handleMouseDown(e) {
        e.preventDefault();
        e.stopPropagation();
        mouseX = parseInt(e.clientX - canvasOffsetX);
        mouseY = parseInt(e.clientY - canvasOffsetY);
        // TODO: capture hitboxes and test
    }



    function handleMouseMove(e) {
        e.preventDefault();
        e.stopPropagation();
        mouseX = parseInt(e.clientX - canvasOffsetX);
        mouseY = parseInt(e.clientY - canvasOffsetY);
        // TODO: capture hitboxes and test
    }


    // Starts drawing the project elements when data is loaded
    startDrawingProject = function(model)
    {
        projectModel = model;
        isDrawingProject = true;
    }







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
        startDrawingProject: startDrawingProject
    };

};
