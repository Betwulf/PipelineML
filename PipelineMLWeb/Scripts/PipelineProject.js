var starting_value;
var cvs;
var ctx;
var sinewaveList;

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



    // Start the Canvas
    cvs = document.getElementById('main-canvas');
    ctx = cvs.getContext('2d');
    window.addEventListener('resize', resizeCanvas, false);
    resizeCanvas();

    // Create Canvas Background
    var halfHeight = cvs.height/2 - 30;
    sinewaveList = [new getWave(0.5, halfHeight / 2 + 10, 'rgba(120, 120, 120, 1)', 0, halfHeight),
                    new getWave(0.4, halfHeight / 2 + 20, 'rgba(120, 120, 120, 1)', 100, halfHeight),
                    new getWave(0.3, halfHeight / 2 + 30, 'rgba(120, 120, 120, 1)', 150, halfHeight)
];


    // Build Sinewave object
    function getWave(periodicity, height, color, xoffset, yoffset)
    {
        var nowTime = new Date();
        this.xoffset = xoffset;
        this.yoffset = yoffset;
        this.height = height;
        this.color = color;
        this.periodicity = periodicity;
        this.points = [];
    }

    function updateWave(sinewave)
    {
        var nowTime = new Date();
        var currentPhase = (nowTime.getMinutes()*60 + nowTime.getSeconds() + nowTime.getMilliseconds() / 1000.0) * 6;
        
        sinewave.points = [
            { x: 0, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset) * Math.PI / 180) + sinewave.yoffset },
            { x: cvs.width * 1 / 3, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset + 120) * Math.PI / 180) + sinewave.yoffset },
            { x: cvs.width * 2 / 3, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset + 240) * Math.PI / 180) + sinewave.yoffset },
            { x: cvs.width, y: sinewave.height * Math.sin((currentPhase * sinewave.periodicity + sinewave.xoffset + 360) * Math.PI / 180) + sinewave.yoffset }];

    }

    // drawWave
    function drawWave(sinewave)
    {
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

        //console.log(sinewave.points[0].x + ", " + sinewave.points[0].y);
        //console.log(sinewave.points[4].x + ", " + sinewave.points[4].y);
    }

    // Runs each time the DOM window resize event fires.
    // Resets the canvas dimensions to match window,
    // then draws the new borders accordingly.
    function resizeCanvas() {
        cvs.width = $("#main-canvas").parent().width();
        cvs.height = window.innerHeight - 76; // Adjustment for the navbar height
        redraw();
    }



    // Draw loop
    function redraw() {
        //Background
        ctx.beginPath();
        ctx.fillStyle = 'rgba(85, 85, 85, 1)';
        ctx.fillRect(0,0,cvs.width,cvs.height); // refresh background
        for (aWave in sinewaveList) {
            updateWave(sinewaveList[aWave]);
            drawWave(sinewaveList[aWave]);
        }
        requestAnimationFrame(redraw);
    }


    // signalR

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("CounterHub");
    hub.on("OnHit", function (count) {
        $('#counter').text(count);
    });
    conn.start(function () {
        hub.invoke('RecordHit');
    });


});
