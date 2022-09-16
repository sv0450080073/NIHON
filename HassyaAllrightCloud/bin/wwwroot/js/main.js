function handleDeleteRow(r) {
    var i = r.parentNode.parentNode;
    i.parentNode.removeChild(i);
}

function handleAddRow() {
    $('#hiddenRow').clone().removeAttr('id').show().appendTo($('#hiddenRow').parent());
}

function fadeToggle() {
    $("#content").on('click', '.title-section', function () {
        $(this).find('i').toggleClass('fa-angle-up').toggleClass('fa-angle-down');
        $(this).next().slideToggle();
        onInit();
    });
}

function customScrollbar() {
    $(".mouse-event").each(function () {
        var div1 = $(this).find('.busData--normal');
        var div2 = $(this).find('.busData--rentalBus');
        var div3 = $(this).find('.busData--spareBus');

        div2.scrollLeft(div1.scrollLeft());
        div3.scrollLeft(div1.scrollLeft());

        div1.scroll(function () {
            div2.scrollLeft($(this).scrollLeft());
            div3.scrollLeft($(this).scrollLeft());
        });

        div2.scroll(function () {
            div1.scrollLeft($(this).scrollLeft());
            div3.scrollLeft($(this).scrollLeft());
        });

        div3.scroll(function () {
            div1.scrollLeft($(this).scrollLeft());
            div2.scrollLeft($(this).scrollLeft());
        });
    })
}

function setEventforTimeInput() {
    $(".customTime :input").attr('type', 'tel');

    // remove comma in currency field when focus
    $(".customTime :input").focus(function (e) {
        if (this.value == "--:--") {
            this.preValue = "--:--";
            this.value = "";
        }
        else if (this.value == "00:00") {
            this.preValue = "00:00";
            this.value = "";
        }
        else {
            this.value = this.value.replace(/:/g, "");
        }
    });
    // re-format currency with comma
    $(".customTime :input").blur(function (e) {
        this.value = formatCustomTimewithComma(this.value);
        if ((this.value == "") && ("--:--" == this.preValue)) {
            this.value = "--:--";
        }
        else if ((this.value == "") && ("00:00" == this.preValue)) {
            this.value = "00:00";
        }
    });
    $(".customTime :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 58) || x == 8 ||
            (x >= 35 && x <= 40) || x == 46))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
}

function setEventforCurrencyField() {
    $(".number :input").attr('type', 'tel');

    // remove comma in currency field when focus
    $(".number :input").focus(function (e) {
        if (this.value == "0") {
            this.preValue = "0";
            this.value = "";
        } else {
            this.value = this.value.replace(/,/g, "");
        }
    });
    // re-format currency with comma
    $(".number :input").blur(function (e) {
        this.value = formatCurrrencywithComma(this.value);
        if ((this.value == "") && ("0" == this.preValue)) {
            this.value = "0";
        }
    });
    // just allow input number
    $(".number :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
    //just allow input float
    $('.two-decimals').keyup(function (event) {
        this.value = this.value.replace(/[^0-9\.]/g,'');
        if($(this).val().indexOf('.')!=-1){         
            if($(this).val().split(".")[1].length > 2){                
                if( isNaN( parseFloat( this.value ) ) ) return;
                this.value = parseFloat(this.value).toFixed(2);
            }  
         }            
         return this; //for chaining
    });
    //disable drop text
    $(".number :input").bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable drop text
    $(".number :input").bind("drop", function (e) {
        var data = e.originalEvent.dataTransfer.getData("text");
        if (isNumber(data) == false)
            e.preventDefault();
    });
}

function setEventforCodeNumberField() {
    $(".code-number :input").attr('type', 'tel');

    // remove comma in currency field when focus
    $(".code-number :input").focus(function (e) {
        if (this.value.length > 0) {
            this.value = parseInt(this.value);
        }
    });
    // re-format currency with comma
    $(".code-number :input").blur(function (e) {
        if (this.value.length > 0) {
            this.value = this.value.padStart(8, '0');
        }
    });
    // just allow input number
    $(".code-number :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8 ||
            (x >= 35 && x <= 40) || x == 46))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
    //disable drop text
    $(".code-number :input").bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable drop text
    $(".code-number :input").bind("drop", function (e) {
        var data = e.originalEvent.dataTransfer.getData("text");
        if (isNumber(data) == false)
            e.preventDefault();
    });
}

//check text is number
function isNumber(num) {
    return (typeof num == 'string' || typeof num == 'number') && !isNaN(num - 0) && num !== '';
};
//set forcus input
function focusEditor(className) {
    document.getElementsByClassName(className)[0].querySelector("input").focus();
}

function formatCurrrencywithComma(x) {
    x = x.toString();
    var pattern = /(-?\d+)(\d{3})/;
    while (pattern.test(x))
        x = x.replace(pattern, "$1,$2");
    return x;
}

function formatCustomTimewithComma(x) {
    str = x.toString();
    switch (str.length) {
        case 1:
            return "0" + str + ":00";
        case 2:
            return str + ":00";
        case 3:
            return "0" + str[0] + ":" + str[1] + str[2];
        case 4:
            return str[0] + str[1] + ":" + str[2] + str[3];
    }
    return str;
}

//press enter to next tabindex and shift+enter in pre tabindex
function tabindexFix() {
    {
        // Setting focus on first textbox

        //$('input:text:first').focus();

        // binding keydown event to textbox

        $('input:text').bind('keydown', function (e) {
            // detecting keycode returned from keydown and comparing if its equal to 13 (enter key code)
            if (e.which == 13 && e.shiftKey) {
                // by default if you hit enter key while on textbox so below code will prevent that default behaviour

                e.preventDefault();

                // getting next index by getting current index and incrementing it by 1 to go to next textbox

                var preIndex = $('input:text').index(this) - 1;
                for (i = preIndex; i > 0; i--) {
                    var selectedIndex = $('input:text:eq(' + i + ')').attr('tabindex');
                    if (selectedIndex == -1) {
                        preIndex = preIndex - 1;
                    }
                    else {
                        preIndex = preIndex;
                        break;
                    }
                }
                if (preIndex > 0) {
                    // setting index to next textbox using CSS3 selector of nth child

                    $('input:text:eq(' + preIndex + ')').focus();
                }
            }
            else
                if (e.keyCode == 13) {
                    // by default if you hit enter key while on textbox so below code will prevent that default behaviour

                    e.preventDefault();

                    // getting next index by getting current index and incrementing it by 1 to go to next textbox

                    var nextIndex = $('input:text').index(this) + 1;

                    // getting total number of textboxes on the page to detect how far we need to go

                    var maxIndex = $('input:text').length;
                    for (i = nextIndex; i < maxIndex; i++) {
                        var selectedIndex = $('input:text:eq(' + i + ')').attr('tabindex');
                        if (selectedIndex == -1) {
                            nextIndex = nextIndex + 1;
                        }
                        else {
                            nextIndex = nextIndex;
                            break;
                        }
                    }

                    // check to see if next index is still smaller then max index
                    if (nextIndex < maxIndex) {
                        // setting index to next textbox using CSS3 selector of nth child

                        $('input:text:eq(' + nextIndex + ')').focus();
                    }
                }
        });
    }
}
//set maxleght
function addMaxLength(textBoxCss, maxlength) {
    var cssClassName = '.' + textBoxCss + maxlength;
    $(cssClassName + " :input").attr("maxlength", maxlength);
}
//disable tab index with class
function addtabindex(textBoxCss) {
    var cssClassName = '.' + textBoxCss;
    $(cssClassName + " :input").attr("tabindex", "-1");
}

//enable tab index with class
function enabletabindex(textBoxCss) {
    var cssClassName = '.' + textBoxCss;
    $(cssClassName + " :input").attr("tabindex", "0");
}

function focusSubmitButton(btnCss) {
    $('.' + btnCss).focus();
}

//disableButton
function disableButton(btnCss) {
    var cssClassName = '.' + btnCss;
    $(cssClassName).css('pointer-events', 'none');
}
//enableButton
function enableButton(btnCss) {
    var cssClassName = '.' + btnCss;
    $(cssClassName).css('pointer-events', 'auto');
}

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

window.browserResize = {
    getInnerWidth: function () {
        return window.innerWidth - $("#sidebar").width() - 30 - 15 - $(".busData-name").width() - 5;
    },

    getCallbackInnerWidth: function () {
        return window.innerWidth + parseFloat($("#sidebar").css("margin-left"), 10) - 30 - 15 - $(".busData-name").outerWidth() - 5;
    },

    registerWidthCallback: function () {
        document.getElementById("sidebarCollapse").addEventListener("click", function () {
            DotNet.invokeMethodAsync("HassyaAllrightCloud", 'OnBrowserCallbackWidth');
        });
    },
    registerResizeCallback: function () {
        window.addEventListener("resize", function () {
            DotNet.invokeMethodAsync("HassyaAllrightCloud", 'OnBrowserResize');
        });
    },
}

function setTooltip() {
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').each(function () {
            $(this).tooltip({
                sanitize: false,
                title: $(this).attr("data-original-title"),
                //delay: { show: 500, hide: 100 },
                trigger: "hover"
            });
        })
    })
}

function removeTooltip() {
    $('[data-toggle="tooltip"]').tooltip('hide');
}

function scrollToTop() {
    $("#table-wrapper").animate({ scrollTop: 0 }, "fast");
}

function setMouseUpAndMove(screenX, screenY, count, reference) {
    var chartArea = document.getElementById("sale-chart-area");
    var areaFrame = chartArea.getBoundingClientRect();
    var gridGroup = chartArea.querySelector(".dxc-grids-group");
    if (gridGroup == null) {
        return;
    }
    var chart = gridGroup.getBoundingClientRect();
    var frame = document.getElementById("frame-select");
    var WidthPerTime = chart.width / count;
    var scroll = document.documentElement.scrollTop;
    if (screenY < chart.top || screenY > chart.bottom
        || screenX < chart.left || screenX > chart.right) {
        return;
    }
    frame.style.visibility = "visible";
    frame.style.top = (chart.top + scroll) + "px";
    frame.style.left = screenX + "px";
    frame.style.height = chart.height + "px";
    frame.style.width = 0;
    frame.style.marginLeft = null;
    var currentLeft = frame.offsetLeft;
    document.onmouseup = closeDragElement;
    document.onmousemove = elementDrag;

    function elementDrag(e) {
        e = e || window.event;
        e.preventDefault();
        if (e.clientX < currentLeft) {
            frame.style.left = Math.max(e.clientX, chart.left) + "px";
            frame.style.width = Math.min(Math.abs(e.clientX - currentLeft), currentLeft - chart.left) + "px";
        } else {
            frame.style.left = currentLeft + "px";
            frame.style.width = Math.min(Math.abs(e.clientX - currentLeft), chart.right - currentLeft) + "px";
        }
    }
    function closeDragElement() {
        document.onmouseup = null;
        document.onmousemove = null;
        var currentFrame = frame.getBoundingClientRect();
        var adjustLeft = chart.left + WidthPerTime * Math.floor((currentFrame.left - chart.left) / WidthPerTime);
        frame.style.left = null;
        frame.style.marginLeft = (adjustLeft - areaFrame.left) + "px";
        frame.style.width = (WidthPerTime * Math.min(Math.ceil((currentFrame.right - chart.left) / WidthPerTime), count) + chart.left - adjustLeft) + "px";
        IndexSelectBegin = Math.max(0, Math.floor((currentFrame.left - chart.left) * count / chart.width));
        IndexSelectEnd = Math.min(count - 1, Math.floor((currentFrame.right - chart.left) * count / chart.width));
        reference.invokeMethodAsync("WhenDragComplete", IndexSelectBegin, IndexSelectEnd);
    }
}

function ChangeChartType(reference) {
    var frame = document.getElementById("frame-select");
    if (frame == null) {
        setTimeout(function () {
            ChangeChartType(reference);
        }, 500);
        return;
    }
    frame.style.visibility = "hidden";
    requestResetFrame();
    var chartLegendCheckbox = $(".dx-chart-legend-item .custom-checkbox");
    if (chartLegendCheckbox.length > 0) {
        chartLegendCheckbox.first().css("cursor", "auto");
    }
    var chartLengendControlLabel = $(".dx-chart-legend-item .custom-checkbox .custom-control-label");

    chartLengendControlLabel.click(function () {
        requestResetFrame();
    });

    window.addEventListener('resize', function () {
        if ($("#frame-select").is(":visible")) {
            frame.style.visibility = "hidden";
            requestResetFrame();
        }
    });

    $("#content").on('click', '#sale-per-day-title', function () {
        frame.style.visibility = "hidden";
        requestResetFrame();
    });

    $("#content").on('click', '#bar-staff-day .dxc-series .dxc-markers rect', function (e) {
        var gridGroup = document.getElementById("bar-staff-day").querySelector(".dxc-grids-group");
        var chart = gridGroup.getBoundingClientRect();
        var index = Math.floor($("#bar-staff-day .dxc-series .dxc-markers rect").length * (e.clientX - chart.left) / chart.width);
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 2);
        OnBrowserScroll(index, 2, reference);
    });

    $("#content").on('click', '#bar-customer-day .dxc-series .dxc-markers rect', function (e) {
        var gridGroup = document.getElementById("bar-customer-day").querySelector(".dxc-grids-group");
        var chart = gridGroup.getBoundingClientRect();
        var index = Math.floor($("#bar-customer-day .dxc-series .dxc-markers rect").length * (e.clientX - chart.left) / chart.width);
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 4);
        OnBrowserScroll(index, 4, reference);
    });

    $("#content").on('click', '#bar-group-classification-day .dxc-series .dxc-markers rect', function (e) {
        var gridGroup = document.getElementById("bar-group-classification-day").querySelector(".dxc-grids-group");
        var chart = gridGroup.getBoundingClientRect();
        var index = Math.floor($("#bar-group-classification-day .dxc-series .dxc-markers rect").length * (e.clientX - chart.left) / chart.width);
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 6);
        OnBrowserScroll(index, 6, reference);
    });

    $("#content").on('click', '#pie-group-classification-day .dxc-series .dxc-markers path', function (e) {
        var pieGroupClassificationDayContainer = document.getElementById("pie-group-classification-day").querySelector(".dxc-series .dxc-markers");
        var chart = pieGroupClassificationDayContainer.getBoundingClientRect();
        var centerX = (chart.left + chart.right) / 2;
        var centerY = (chart.top + chart.bottom) / 2;
        var beginX = chart.right;
        var beginY = centerY;
        var Radius = chart.width / 2;
        var lengthToBeginPoint = Math.sqrt(Math.pow(e.clientX - beginX, 2) + Math.pow(e.clientY - beginY, 2));
        var lengthToCenter = Math.sqrt(Math.pow(e.clientX - centerX, 2) + Math.pow(e.clientY - centerY, 2));
        var angle = Math.acos((Radius * Radius + lengthToCenter * lengthToCenter - lengthToBeginPoint * lengthToBeginPoint) / (2 * Radius * lengthToCenter));
        if (e.clientY < beginY) {
            angle = 2 * Math.PI - angle;
        }
        var selectedRate = angle / (2 * Math.PI);
        var values = $("#pie-group-classification-day .grid-area .grid-display tr:not(:last-child) td:last-child");
        var sum = 0;
        var rate = [0];
        values.each(function () {
            var currencyValue = $(this).html();
            var numberValue = Number(currencyValue.replace(/[^0-9.-]+/g, ""));
            sum += numberValue;
        });
        values.each(function () {
            var currencyValue = $(this).html();
            var numberValue = Number(currencyValue.replace(/[^0-9.-]+/g, ""));
            var rateBefore = rate[rate.length - 1];
            rate.push(rateBefore + numberValue / sum);
        });
        var index = 0;
        for (var i = 0; i < rate.length; i++) {
            if (rate[i] <= selectedRate && (rate[i + 1] > selectedRate || i == rate.length - 1)) {
                index = i;
                break;
            }
        }
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 8);
        OnBrowserScroll(index, 8, reference);
    });
}

function requestResetFrame() {
    var i = 1;
    var isDone = false;
    while (i < 100) {
        (function (i) {
            setTimeout(function () {
                if (!isDone) {
                    isDone = ResetFrame();
                }
            }, 500 * i);
        })(i++)
    }
}

function triggerToggle() {
    var lastTogger = $("#content .title-section").last();
    var iconClass = lastTogger.find('i').attr("class");
    if (iconClass.endsWith("down")) {
        lastTogger.trigger("click");
    }
}

function ResetFrame() {
    var saleGridTr = $("#sale-grid-area .grid-display tr:not(:first)");
    var count = saleGridTr.length;
    var start = saleGridTr.index($("#sale-grid-area .grid-display tr.selected-area:first")[0]);
    var end = saleGridTr.index($("#sale-grid-area .grid-display tr.selected-area:last")[0]);
    var chartArea = document.getElementById("sale-chart-area");
    var areaFrame = chartArea.getBoundingClientRect();
    var gridGroup = chartArea.querySelector(".dxc-grids-group");
    if (gridGroup == null) {
        return false;
    }
    var scroll = document.documentElement.scrollTop;
    var chart = gridGroup.getBoundingClientRect();
    var frame = document.getElementById("frame-select");
    var WidthPerTime = chart.width / count;
    frame.style.visibility = "visible";
    frame.style.top = (chart.top + scroll) + "px";
    frame.style.height = chart.height + "px";
    var width = WidthPerTime * (end - start + 1);
    frame.style.width = width + "px";
    var adjustLeft = chart.left + start * WidthPerTime;
    frame.style.marginLeft = (adjustLeft - areaFrame.left) + "px";
    return true;
}

function SelectTimeByGrid(indexBegin, count, reference) {
    var saleGridTr = $("#sale-grid-area .grid-display tr:not(:first)");
    saleGridTr.each(function (index) {
        if (index == indexBegin) {
            $(this).addClass("selected-area");
        } else {
            $(this).removeClass("selected-area");
        }
    });
    document.onmouseup = closeDragElement;
    document.onmousemove = elementDrag;
    var fromIndex = indexBegin;
    var toIndex = indexBegin;
    function closeDragElement(e) {
        ResetFrame();
        reference.invokeMethodAsync("WhenDragComplete", fromIndex, toIndex);
        document.onmouseup = null;
        document.onmousemove = null;
    }
    function elementDrag(e) {
        var selection = window.getSelection();
        var parent = selection.extentNode.parentElement;
        if (parent.localName == "td") {
            parent = parent.parentElement;
        }
        var indexDrag = saleGridTr.index(parent);
        if (indexDrag < 0) {
            return;
        }
        fromIndex = Math.min(indexDrag, indexBegin);
        toIndex = Math.max(indexDrag, indexBegin);
        saleGridTr.each(function (index) {
            if (fromIndex <= index && index <= toIndex) {
                $(this).addClass("selected-area");
            } else {
                $(this).removeClass("selected-area");
            }
        });
    }
}

function OnBrowserScroll(index, graphType, reference) {
    var isHide = false;

    $(window).scroll(function () {
        if (!isHide) {
            reference.invokeMethodAsync("HideMenu", index, graphType);
            isHide = true;
        }
    });
}

function setHover() {
    $(document).ready(function () {
        $(".editable").hover(function () {
            $(".listTimeline").find("[data-bookingID='" + $(this).attr("data-bookingID") + "']").addClass("hover");
        }, function () {
            $(".listTimeline").find(".editable.hover").removeClass("hover");
        })
    });
}

function handleKeyPress() {
    $('#hyper-form input').bind("keydown", function (e) {
        if (e.keyCode == 13 && !e.shiftKey) {
            e.preventDefault();
            var nextIndex = $('input').index(this);
            var maxIndex = $('input').length;
            if (nextIndex < maxIndex - 1 && $('#hyper-form input:eq(' + nextIndex + ')').is(":visible")) {
                $('#hyper-form input:eq(' + nextIndex + ')').focus();
            } else {
                this.blur();
            }
        }
        if (e.keyCode == 13 && e.shiftKey) {
            e.preventDefault();
        }
    });
}

function Roll() {
    var sliderall = document.querySelectorAll('.busData--normal');
    var slidergreen = document.querySelector('.busData--rentalBus');
    var slidergray = document.querySelector('.busData--spareBus');
    let isDown = false;
    let startX;
    let scrollLeft;
    let classname;

    document.addEventListener('mouseover', function (e) {
        classname = e.target.className;
        classname = classname.trim();
    }, false);

    sliderall.forEach(function (slider) {
        slider.addEventListener('mousedown', (e) => {
            if (classname == "listTimeline__item") {
                isDown = true;
                slider.classList.add('active');
                startX = e.pageX - slider.offsetLeft;
                scrollLeft = slider.scrollLeft;
            }
        });
        slider.addEventListener('mouseleave', () => {
            if (classname == "listTimeline__item") {
                isDown = false;
                slider.classList.remove('active');
            }
        });
        slider.addEventListener('mouseup', () => {
            if (classname == "listTimeline__item") {
                isDown = false;
                slider.classList.remove('active');
            }
        });
        slider.addEventListener('mousemove', (e) => {
            if (classname == "listTimeline__item") {
                if (!isDown) return;
                e.preventDefault();
                const x = e.pageX - slider.offsetLeft;
                const walk = (x - startX) * 1.5; //scroll-fast
                slider.scrollLeft = scrollLeft - walk;
            }
        });

        if (slidergreen != null) {
            slidergreen.addEventListener('mousedown', (e) => {
                if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                    isDown = true;
                    slidergreen.classList.add('active');
                    startX = e.pageX - slider.offsetLeft;
                    scrollLeft = slider.scrollLeft;
                }
            });
            slidergreen.addEventListener('mouseleave', () => {
                if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                    isDown = false;
                    slidergreen.classList.remove('active');
                }
            });
            slidergreen.addEventListener('mouseup', () => {
                if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                    isDown = false;
                    slidergreen.classList.remove('active');
                }
            });
            slidergreen.addEventListener('mousemove', (e) => {
                if (classname == "listTimeline__item  listTimeline__item--rentalBus") {
                    if (!isDown) return;
                    e.preventDefault();
                    const x = e.pageX - slider.offsetLeft;
                    const walk = (x - startX) * 1.5; //scroll-fast
                    slider.scrollLeft = scrollLeft - walk;
                }
            });
        }

        if (slidergray != null) {
            slidergray.addEventListener('mousedown', (e) => {
                if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                    isDown = true;
                    slidergray.classList.add('active');
                    startX = e.pageX - slidergray.offsetLeft;
                    scrollLeft = slider.scrollLeft;
                }
            });
            slidergray.addEventListener('mouseleave', () => {
                if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                    isDown = false;
                    slidergray.classList.remove('active');
                }
            });
            slidergray.addEventListener('mouseup', () => {
                if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                    isDown = false;
                    slidergray.classList.remove('active');
                }
            });
            slidergray.addEventListener('mousemove', (e) => {
                if (classname == "listTimeline__item   listTimeline__item--spareBus") {
                    if (!isDown) return;
                    e.preventDefault();
                    const x = e.pageX - slider.offsetLeft;
                    const walk = (x - startX) * 1.5; //scroll-fast
                    slider.scrollLeft = scrollLeft - walk;
                }
            });
        }
    });
}

function fadeToggleWidthAdjustHeight() {
    AdjustHeight();
    $("#content").off('click').on('click', '.title-section', function () {
        $(this).find('i').toggleClass('fa-angle-up').toggleClass('fa-angle-down');
        $(this).next().slideToggle();
        setTimeout(function () {
            AdjustHeight();
        }, 500);
    });
    window.addEventListener("resize", function () {
        setTimeout(function () {
            AdjustHeight();
        }, 500);
    });
}

function AdjustHeight() {
    var browserHeight = $(window).height();
    var conditionDiv = $('.express-condition.mb-2');
    var tableArea = $('#table-wrapper');
    var tableAreaHeight = tableArea[0].offsetHeight;
    var totalArea = $('#total-area');
    var totalAreaRect = totalArea[0].getBoundingClientRect();
    var adjustHeightValue = 0;
    var scroll = document.documentElement.scrollTop;
    if (conditionDiv.is(":visible")) {
        adjustHeightValue = tableAreaHeight + (browserHeight - totalAreaRect.top - scroll);
    } else {
        adjustHeightValue = tableAreaHeight - (totalAreaRect.bottom - browserHeight + scroll);
    }
    tableArea.css('max-height', adjustHeightValue + "px");
    tableArea.css('min-height', adjustHeightValue + "px");
}

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

function handleSelectByKeyUp(reference) {
    document.onkeydown = handleKeyDown
    document.onkeyup = handleKeyUp;
    function handleKeyDown(e) {
        var isTargetBody = $(e.target).is("body");
        if (isTargetBody) {
            if (e.keyCode == 38 || e.keyCode == 40) {
                e.preventDefault();
            }
            reference.invokeMethodAsync("OnKeyDown", e.keyCode || e.which, e.shiftKey).then(data => {
                if (data >= 0) {
                    var container = $("#table-wrapper");
                    var headerHeight = $("#table-wrapper thead").height();
                    var contHeight = container[0].clientHeight;
                    var childRow = $("#table-wrapper .body-row-1:eq(" + data + ")");
                    if (e.keyCode == 40) {
                        childRow = $("#table-wrapper .body-row-3:eq(" + data + ")");
                    }
                    var childRowTop = childRow.offset().top - container.offset().top - headerHeight;
                    var childRowBottom = childRowTop + childRow.height();
                    var currentScroll = container.scrollTop();
                    if (childRowBottom > contHeight - headerHeight) {
                        $("#table-wrapper").animate({ scrollTop: currentScroll + childRowBottom - contHeight + headerHeight }, 200);
                    } else if (childRowTop < 0) {
                        $("#table-wrapper").animate({ scrollTop: currentScroll + childRowTop }, 200);
                    }
                }
            });
        }
    }
    function handleKeyUp(e) {
        var isTargetBody = $(e.target).is("body");
        if (isTargetBody && (e.keyCode == 16 || e.keyCode == 17)) {
            reference.invokeMethodAsync("KeyUpComplete");
        }
    }
}
function settextinlinetextarea(len) {
    $("#txtJourneys").keydown(function (e) {
        //get Textearea text
        var text = $(this).val();

        //Split with \n carriage return
        var lines = text.split("\n"); 

        for (var i = 0; i < lines.length; i++) {
            if (lines[i].length > len) {
                lines[i] = lines[i].substring(0, len);
                lines[i] = lines[i]+"\n";
            }     
        }
        //Join with \n.
        //Set textarea
        $(this).val(lines.join("\n"));

    });
     $("#txtJourneys").bind("paste", function (e) {
        //get Textearea text
        var text =window.event.clipboardData.getData('text');;

        //Split with \n carriage return
        var lines = text.split("\n"); 

        for (var i = 0; i < lines.length; i++) {
            if (lines[i].length > len) {
                lines[i] = lines[i].substring(0, len);
                lines[i] = lines[i]+"\n";
                console.log(lines[i]);
            }     
        }
        //Join with \n.
        //Set textarea
        $(this).val(lines.join("\n"));
      });
}

function scroll() {
    $('.scrollbar-macosx').scrollbar();
    $(".scrollbar-outer").scrollbar();
}

function openNewUrlInNewTab(url) {
    window.open(url, '_blank');
}

function displayEditDeleteIcon(iconCss, buttonCss) {
    var iconCssName = '.' + iconCss;
    var buttonCssName = '.' + buttonCss;
    
    $(iconCssName + ' .fa-edit').click(function () {
        var button = $(iconCssName).find(buttonCssName)
        if (button.hasClass('hide')) {
            button.removeClass('hide').addClass('show')
        } else {
            button.removeClass('show').addClass('hide')
        }
    })
    
}

function adjustHyperAreaWidth() {
    resetHyperAreaWidth();
    window.addEventListener("resize", function () {
        resetHyperAreaWidth();
    });
}

function resetHyperAreaWidth() {
    var rowWidth = $('.condition-display-row').width();
    var minConditionRowWidth = parseFloat($('.left-search-condition').css('min-width'));
    if (rowWidth < (minConditionRowWidth + 2) * 2) {
        $('.left-search-condition, .right-search-condition').width("calc(100% - 2px)");
    } else {
        $('.left-search-condition, .right-search-condition').width("calc(50% - 2px)");
    }
}

function setEventforRateField() {
    // just allow input number
    $(".rate :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8 || x == 46)) {
            let index = $(this).val().indexOf('.');
            let lengthDec = 0;
            if (index == 1) {
                lengthDec = $(this).val().split('.')[1].length;
            }
            let lengthStr = $(this).val().length;
            if (x == 46 && index == 1 || lengthStr > 3 || (this.selectionStart == 3 && lengthDec == 1)) {
                return false;
            }
            return true;
        }
        else {
            e.preventDefault();
            return false;
        }
    });
    //disable drop text
    $(".rate :input").bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable drop text
    $(".rate :input").bind("drop", function (e) {
        var data = e.originalEvent.dataTransfer.getData("text");
        if (isNumber(data) == false)
            e.preventDefault();
    });
    // prevent jap
    $(".rate :input, .number-int :input").blur(function (e) {
        if (isNumber(this.value) == false) {
            this.value = 0;
        }
        this.value -= 0;
    });
}

window.browserResizeStaff = {
    getInnerWidth: function () {
        return (window.innerWidth - $("#sidebar").width() - 45) / 2 - $(".busData-name").outerWidth() - 2;
    },

    getCallbackInnerWidth: function () {
        return (window.innerWidth + parseFloat($("#sidebar").css("margin-left"), 10) - 45) / 2 - $(".busData-name").outerWidth() - 2;
    },
}

function zoomTabBooking() {
    $(".zoom-icon--expand").click(function () {
        $(".col-6+.zoom").addClass("active");
    })
    $(".zoom-icon--compress").click(function () {
        $(".col-6+.zoom").removeClass("active");
    })
}

function handleRadioPopupSignUp() {
    $("#radioCSV input.form-control").prop("disabled", true);
    $("input[name='customRadioPopupSignup']").click(function () {
        if ($(this).val() == 0) {
            $("#radioLayout input.form-control").prop("disabled", false);
            $("#radioCSV input.form-control").prop("disabled", true);
        } else {
            $("#radioLayout input.form-control").prop("disabled", true);
            $("#radioCSV input.form-control").prop("disabled", false);
        }
    })
}

function CustomInputFile() {
    $('.custom-file-input').on('change', function () {
        var fileName = document.getElementById("inputFile").files[0].name;
        $(this).next('.custom-file-label').html(fileName);
    })
}

function closeDropdown() {
    $(document).on("click", function (event) {
        var $trigger = $(".custom-multi-combobox");
        if ($trigger !== event.target && !$trigger.has(event.target).length) {
            $(".custom-multi-combobox.show").removeClass("show");
        }
    });
}

//Load script of specific page
function loadPageScript(pageName, functionName) {
    let url = `js/pages/${pageName}.js`;
    let params = Array.prototype.slice.call(arguments).splice(2)
    let completion = function () { console.log(`${url} is loaded`) }
    if (functionName) {
        completion = function () { window[functionName].apply(this, params);}
    }
    loadScript(url, functionName, completion)
}

//Load script of specific component
function loadComponentScript(componentName, functionName) {
    let url = `js/components/${componentName}.js`;
    let params = Array.prototype.slice.call(arguments).splice(2)
    let completion = function () { console.log(`${url} is loaded`) }
    if (functionName) {
        completion = function () { window[functionName].apply(this, params); }
    }
    loadScript(url, functionName, completion)
}

//Load script from specific url
function loadLibraryScript(url, functionName) {
    let params = Array.prototype.slice.call(arguments).splice(2)
    let completion = function () { console.log(`${url} is loaded`) }
    if (functionName) {
        completion = function () { window[functionName].apply(this, params); }
    }
    loadScript(url, functionName, completion)
}

//Load script from specific url
//If script is loaded and function is existing => executing callback
//If script is not load but alread existed in head of html document => add to waitingFunction for executing after script is loaded
//If script is not load and not in head of html document => load script then executing callback and waiting functions
function loadScript(url, functionName, callback) {
    if (isScriptLoaded(url) && (typeof window[functionName] === "function")) {
        callback();
    } else {
        if (isScriptAppended(url)) {
            if (functionName) {
                let functionMapping = {};
                if (waitingFunctions[url] != undefined) {
                    functionMapping = waitingFunctions[url];
                }
                functionMapping[functionName] = callback
                waitingFunctions[url] = functionMapping;
            }
        } else {
            var head = document.head;
            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = url;

            /* Then bind the event to the callback function.
            There are several events for cross browser compatibility.*/
            script.onreadystatechange = function () { callback(); loadedScripts.push(url); runQueueFunction(url); };
            script.onload = function () { callback(); loadedScripts.push(url); runQueueFunction(url); };

            // Fire the loading
            head.appendChild(script);
        }
    }
}

var waitingFunctions = {} // storing functions which is executed after a specified script is loaded
var loadedScripts = [] // storing scripts url which is loaded

//Check if script is loaded into Source 
function isScriptLoaded(url) {
    return loadedScripts.includes(url);
}

//Check if script is appened to head of html document
function isScriptAppended(url) {
    var scripts = document.getElementsByTagName('script');
    for (var i = scripts.length; i--;) {
        if (scripts[i].attributes["src"].nodeValue == url) return true;
    }
    return false;
}

//Run waiting functions
function runQueueFunction(url) {
    let functions = waitingFunctions[url];
    for (var key in functions) {
        functions[key]();
    }
    waitingFunctions[url] = {};
}