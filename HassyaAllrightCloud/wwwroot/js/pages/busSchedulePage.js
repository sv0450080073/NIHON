//set height topbar
function onInit() {
    $(document).ready(function () {
        $(".topbar").each(function () {
            $(this).css("height", $(this).parent().parent().find('.listColumn').outerHeight());
        })
    });
    $(window).on('load resize', function () {
        $(".topbar").each(function () {
            $(this).css("height", $(this).parent().parent().find('.listColumn').outerHeight());
        })
    });
}
function unloadScrollBars() {
    var strLocation = document.location.pathname.split('/')[1];
    if (strLocation == "busschedule") {
       var style = document.createElement("style");
      style.innerHTML = `body::-webkit-scrollbar {display: none;}`;
      document.head.appendChild(style);
    }
    
}
//press Esc 
function pressEsc() {
    var strLocation = document.location.pathname.split('/')[1];
    if (strLocation == "busschedule") {
        document.addEventListener('keydown', function (event) {
            if (event.key === "Escape") {
                DotNet.invokeMethodAsync("HassyaAllrightCloud", 'pressKey');
            }
        });
    }
    else if (strLocation == "") {
        // To do
    }
}
//set zindex
function Setzindex(lineid, index) {
    var arr = jQuery.map(jQuery('.listTimeline__item'),function(n,i){
                return jQuery(n).attr('id');
     });
    arr.forEach(updatezindex);
    $("#" + lineid).css("z-index", index);
}
function updatezindex(item) {
   $("#" + item).css("z-index", 2);
}
//draw line
function creatediv(lineid,  iteminLineLst, dotnetInstance, minwidth, view) {
    if (!$('.editable:hover').length) {          
        $("#" + lineid).append("<div class='line' id='line_" + lineid + "_" + 1 + "' />");
        drawLineInsert(lineid, iteminLineLst, dotnetInstance, 100,minwidth, view);
        $(".linevisual").css("z-index", "99");
    }   
}
function drawLineInsert(lineid, iteminLineLst, dotnetInstance, startx,minwidth, view) {
    var lineiddiv = ".linevisual";
    var coordinate = [];
    var count_line = 1;
    var starting_coordinate_x = startx;
    var starting_coordinate_y = 0;
    var left = 0;
    var width = 0;
    var is_clicked = true;
    var minleft = 0;
    var maxwidth = 0;
    $(lineiddiv).mouseup(function (event) {
        if (event.which == 1) {
            if (is_clicked == true && width != 0) { dotnetInstance.invokeMethodAsync("ActualFocusCell", left, width); }
            coordinate = [];
            is_clicked = false;
            $(".linevisual").css("z-index", "0");
            $("#line_" + lineid + "_" + count_line).remove();
            coordinate = [];
        }
    });

    function drawLine(starting_coordinate_x, end_coordinate_x, maxwidth) {
        if (coordinate.length > 1) {
            var start_x = starting_coordinate_x;
            var end_x = end_coordinate_x;
            if (start_x < end_x) {
                var widthdiv = parseInt(end_x) - parseInt(start_x);
                left = start_x;
                if (widthdiv < minwidth) {
                    widthdiv = minwidth;
                }
                width = widthdiv;
                if (parseInt(end_x) >= maxwidth) {
                    width = maxwidth - start_x;
                }
                $("div #line_" + lineid + "_" + count_line).css("position", "absolute");
                $("div #line_" + lineid + "_" + count_line).css("left", start_x + "px");
                $("div #line_" + lineid + "_" + count_line).css("top", "0.3125rem");
                $("div #line_" + lineid + "_" + count_line).css("width", widthdiv + "px");
                $("div #line_" + lineid + "_" + count_line).css("border", "1px solid #FFF5BF");
                if (view == 1) {
                    $("div #line_" + lineid + "_" + count_line).css("height", "2rem");
                } else if (view == 2) {
                    $("div #line_" + lineid + "_" + count_line).css("height", "0.9375rem");
                } else {
                    $("div #line_" + lineid + "_" + count_line).css("height", "0.75rem");
                }                
                $("div #line_" + lineid + "_" + count_line).css("background", "#FFF5BF");
                $("div #line_" + lineid + "_" + count_line).css("max-width", maxwidth - start_x + "px");
                $("div #line_" + lineid + "_" + count_line).css("min-width", minleft + "px");
            }
            //coordinate = [];
        }
    }

    $(lineiddiv).mousemove(function (event) {
        var temp_array = [];
        temp_array['X'] = (event.pageX - $(this).offset().left);
        coordinate.push(temp_array);
        minleft = iteminLineLst[0]['minleft'];
        maxwidth = iteminLineLst[0]['maxwidth'];
        starting_coordinate_x = coordinate[0];
        if (minleft <= starting_coordinate_x) {
            drawLine(starting_coordinate_x['X'], temp_array['X'], maxwidth);
        }
    });
}