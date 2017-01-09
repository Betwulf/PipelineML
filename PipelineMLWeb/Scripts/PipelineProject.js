var starting_value;
var cvs;
var ctx;
var sinewaveList;

$(document).ready(function () {

    //$.getJSON("/Notes/Data", starting_value, loadedData);


    // Start the Canvas
    cvs = document.getElementById('main-canvas');
    ctx = cvs.getContext('2d');
    window.addEventListener('resize', resizeCanvas, false);
    resizeCanvas();

    // Create Canvas Background
    var sinewaveList = [getWave(1, cvs.width, 50, 'rgba(255, 255, 255, 0)', 0)];


    // Build Sinewave object
    function getWave(periodicity, width, height, color, offset)
    {
        var nowTime = new Date();
        var currentPhase = (nowTime.getSeconds() + nowTime.getMilliseconds()/1000.0)*6; // cycles from 0 to 360
        var sinewave;
        sinewave.offset = offset;
        sinewave.height = height;
        sinewave.color = color;
        sinewave.periodicity = periodicity;
        sinewave.points = [
            { x: 0, y: height * Math.sin(periodicity * currentPhase * Math.PI / 180) },
            { x: width*0.25, y: height * Math.sin(periodicity * (currentPhase+90) * Math.PI / 180) },
            { x: width*0.5, y: height * Math.sin(periodicity * (currentPhase+180) * Math.PI / 180) },
            { x: width*0.75, y: height * Math.sin(periodicity * (currentPhase+270) * Math.PI / 180) },
            { x: width, y: height * Math.sin(periodicity * (currentPhase+360) * Math.PI / 180) }]
    }

    // drawWave
    function drawWave(sinewave)
    {
        ctx.shadowOffsetX = 0;
        ctx.shadowOffsetY = 0;
        ctx.shadowBlur = 60;
        ctx.shadowColor = color;

        ctx.beginPath();
        ctx.moveTo(
         sinewave.points[0].x, this.points[0].y
        );
        ctx.bezierCurveTo(
         sinewave.points[1].x, this.points[1].y,
         sinewave.points[2].x, this.points[2].y,
         sinewave.points[3].x, this.points[2].y,
         sinewave.points[4].x, this.points[3].y
        );
        ctx.strokeStyle = color;
        ctx.stroke();
    }

    // Runs each time the DOM window resize event fires.
    // Resets the canvas dimensions to match window,
    // then draws the new borders accordingly.
    function resizeCanvas() {
        cvs.width = window.innerWidth;
        cvs.height = window.innerHeight;
        redraw();
    }



    // Draw loop
    function redraw() {
        //Background
        //ctx.fillStyle = '#114411';
        //ctx.fillRect(0,0,cvs.width,cvs.height);
        ctx.fillStyle = '#995555';
        ctx.fillRect(40, 40, 100, 100);
        for (point in sinewaveList) {
        }
        requestAnimationFrame(redraw);
    }

});
