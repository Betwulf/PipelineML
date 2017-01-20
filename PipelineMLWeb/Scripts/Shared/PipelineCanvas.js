var PipelineCanvas = function () {

    var canvasId;

    // Build Sinewave object
    function getWave(periodicity, height, color, xoffset, yoffset) {
        var nowTime = new Date();
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




    // Runs each time the DOM window resize event fires.
    // Resets the canvas dimensions to match window,
    // then draws the new borders accordingly.
    function resizeCanvas() {
        cvs.width = $("#" + canvasId).parent().width();
        cvs.height = window.innerHeight - 76; // Adjustment for the navbar height
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
        requestAnimationFrame(redraw);
    }

    // Exported method call
    startDrawing = function (aCanvasId) {
        canvasId = aCanvasId;
        // Start the Canvas
        cvs = document.getElementById(canvasId);
        ctx = cvs.getContext('2d');
        window.addEventListener('resize', resizeCanvas, false);
        resizeCanvas();

        // Create Canvas Background
        var halfHeight = cvs.height / 2 - 30;
        sinewaveList = [new getWave(0.5, halfHeight / 2 + 10, 'rgba(120, 120, 120, 1)', 0, halfHeight),
                        new getWave(0.4, halfHeight / 2 + 20, 'rgba(120, 120, 120, 1)', 100, halfHeight),
                        new getWave(0.3, halfHeight / 2 + 30, 'rgba(120, 120, 120, 1)', 150, halfHeight)
        ];



    };


    return {
        startDrawing: startDrawing // ,

    };

};
