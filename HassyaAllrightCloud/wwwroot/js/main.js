function setColorFromDb(cssColors) {
    var style = document.querySelector('head style');
    for (var i = style.sheet.cssRules.length - 1; i >= 0; i--) {
        style.sheet.deleteRule(i);
    }
    for (const property in cssColors.result) {
        console.log(`Load color: ${property} - ${cssColors.result[property]}`);
        style.sheet.insertRule(`.${property} {background-color: ${cssColors.result[property]}}`);
    }
}

function setNumberRate(selector = '.number-rate') {
    let inputFilter = function (value) {
        return /^[0-9]*\.?[0-9]*$/.test(value);
    };
    let textboxes = $(`${selector} input`);
    for (let i = 0; i < textboxes.length; i++) {
        ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
            textboxes[i].addEventListener(event, function () {
                if (inputFilter(this.value)) {
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                } else {
                    this.value = "aaa";
                }
            });
        });
    }
}

function setColorTextFromDb(cssColors) {
    var style = document.querySelector('head style');
    for (var i = style.sheet.cssRules.length - 1; i >= 0; i--) {
        style.sheet.deleteRule(i);
    }
    for (const property in cssColors.result) {
        console.log(`Load color: ${property} - ${cssColors.result[property]}`);
        style.sheet.insertRule(`.${property} {color: ${cssColors.result[property]}}`);
    }
}

function setEventForTelAndFaxInput() {
    $(".customTelFax :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 45))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });

    $(".customTelFax :input").bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        let regex = /[^0-9-]+/;
        let found = clipboarddata.match(regex);

        if (found == null) {
            return;
        }
        if (found.length > 0) {
            clipboarddata = clipboarddata.substring(0, 14);
            e.preventDefault();
        }
    });
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
        if (((x >= 48 && x <= 57) || x == 8))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
}

function setEventforCurrencyField(isClearZero = true) {
    $(".number :input").attr('type', 'tel');

    // remove comma in currency field when focus
    $(".number :input").focus(function (e) {
        if (this.value == "0" && isClearZero) {
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
        this.value = this.value.replace(/[^0-9\.]/g, '');
        if ($(this).val().indexOf('.') != -1) {
            if ($(this).val().split(".")[1].length > 2) {
                if (isNaN(parseFloat(this.value))) return;
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

function setEventforCodeNumberField(padLength = 10) {
    $(".code-number :input").attr('type', 'tel');

    // Remove leading Zeroes and Tenant when focus
    $(".code-number :input").focus(function (e) {
        if (this.value.length > 0) {
            this.value = parseInt(this.value);
        }
    });
    // re-format with leading Zeroes and Tenant
    $(".code-number :input").blur(function (e) {
        if (this.value.length > 0) {
            this.value = this.value.padStart(padLength, '0');
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

function setEventforIsNumberField() {
    $(".Is-number :input").attr('type', 'tel');

    // remove comma in currency field when focus
    $(".Is-number :input").focus(function (e) {
        if (this.value.length > 0) {
            this.value = parseInt(this.value);
        }
    });
    // re-format currency with comma
    $(".Is-number :input").blur(function (e) {
        if (this.value.length > 0) {
            this.value = this.value.padStart(10, '0');
        }
    });
    // just allow input number
    $(".Is-number :input").keypress(function (e) {
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
    $(".Is-number :input").bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable drop text
    $(".Is-number :input").bind("drop", function (e) {
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
            if ((e.which == 13 || e.keyCode == 8) && e.shiftKey) {
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
                if (e.keyCode == 13 || e.keyCode == 8) {
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
                    console.log(nextIndex);
                    // check to see if next index is still smaller then max index
                    if (nextIndex < maxIndex) {
                        // setting index to next textbox using CSS3 selector of nth child

                        $('input:text:eq(' + nextIndex + ')').focus();
                    }
                    else {
                        $('input:text:eq(' + 0 + ')').focus();
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
    var scroll = window.scrollY
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
    document.body.style.cursor = "crosshair";
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
        if (chart.left <= e.clientX && chart.right >= e.clientX && chart.top <= e.clientY && chart.bottom >= e.clientY) {
            document.body.style.cursor = "crosshair";
        } else {
            document.body.style.cursor = "";
        }
    }
    function closeDragElement(e) {
        document.onmouseup = null;
        document.onmousemove = null;
        document.body.style.cursor = "";
        var currentFrame = frame.getBoundingClientRect();
        var adjustLeft = chart.left + WidthPerTime * Math.floor(Math.max(currentFrame.left - chart.left, 0) / WidthPerTime);
        frame.style.left = null;
        frame.style.marginLeft = (adjustLeft - areaFrame.left) + "px";
        frame.style.width = (WidthPerTime * Math.min(Math.ceil((currentFrame.right - chart.left) / WidthPerTime), count) + chart.left - adjustLeft) + "px";
        IndexSelectBegin = Math.max(0, Math.floor((currentFrame.left - chart.left) * count / chart.width));
        IndexSelectEnd = Math.min(count - 1, Math.floor((currentFrame.right - chart.left) * count / chart.width));
        var x = e.clientX;
        var y = e.clientY;
        if (x < chart.left) {
            x = chart.left;
        } else if (x > chart.right) {
            x = chart.right;
        }

        if (y < chart.top) {
            y = chart.top;
        } else if (y > chart.bottom) {
            y = chart.bottom;
        }
        reference.invokeMethodAsync("WhenDragComplete", IndexSelectBegin, IndexSelectEnd, x, y);
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
    requestResetFrameWCount();
    var chartLegendCheckbox = $(".dx-chart-legend-item .custom-checkbox");
    if (chartLegendCheckbox.length > 0) {
        chartLegendCheckbox.first().css("cursor", "auto");
    }
    var chartLengendControlLabel = $(".dx-chart-legend-item .custom-checkbox .custom-control-label");

    chartLengendControlLabel.click(function () {
        requestResetFrame();
        requestResetFrameWCount();
    });

    window.addEventListener('resize', function () {
        if ($("#frame-select").is(":visible")) {
            frame.style.visibility = "hidden";
            requestResetFrame();
            requestResetFrameWCount();
        }
    });

    $("#content").on('click', '#sale-per-day-title', function () {
        frame.style.visibility = "hidden";
        requestResetFrame();
    });
    $("#content").on('click', '#bar-sale-count-day-title', function () {
        frame.style.visibility = "hidden";
        requestResetFrameWCount();
    });

    $("#content").on('click', '#bar-staff-day .dxc-series .dxc-markers rect', function (e) {
        var gridGroup = document.getElementById("bar-staff-day").querySelector(".dxc-grids-group");
        var chart = gridGroup.getBoundingClientRect();
        var index = Math.floor($("#bar-staff-day .dxc-series .dxc-markers rect").length * (e.clientX - chart.left) / chart.width);
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 1, false);
        OnBrowserScroll(index, 2, reference);
    });

    $("#content").on('click', '#bar-customer-day .dxc-series .dxc-markers rect', function (e) {
        var gridGroup = document.getElementById("bar-customer-day").querySelector(".dxc-grids-group");
        var chart = gridGroup.getBoundingClientRect();
        var index = Math.floor($("#bar-customer-day .dxc-series .dxc-markers rect").length * (e.clientX - chart.left) / chart.width);
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 2, false);
        OnBrowserScroll(index, 4, reference);
    });

    $("#content").on('click', '#bar-group-classification-day .dxc-series .dxc-markers rect', function (e) {
        var gridGroup = document.getElementById("bar-group-classification-day").querySelector(".dxc-grids-group");
        var chart = gridGroup.getBoundingClientRect();
        var index = Math.floor($("#bar-group-classification-day .dxc-series .dxc-markers rect").length * (e.clientX - chart.left) / chart.width);
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 3, false);
        OnBrowserScroll(index, 6, reference);
    });

    $("#content").on('click', '#pie-group-classification-day .this-year-pie .dxc-series .dxc-markers path', function (e) {
        var pieGroupClassificationDayContainer = document.getElementById("pie-group-classification-day").querySelector(".this-year-pie .dxc-series .dxc-markers");
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
        var values = $("#pie-group-classification-day .grid-area .grid-display tr:not(:last-child) .value-this-year");
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
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 4, false);
        OnBrowserScroll(index, 8, reference);
    });

    $("#content").on('click', '#pie-group-classification-day .last-year-pie .dxc-series .dxc-markers path', function (e) {
        var pieGroupClassificationDayContainer = document.getElementById("pie-group-classification-day").querySelector(".last-year-pie .dxc-series .dxc-markers");
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
        var values = $("#pie-group-classification-day .grid-area .grid-display tr:not(:last-child) .value-last-year");
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
        reference.invokeMethodAsync("ShowMenu", e.clientX, e.clientY, index, 5, false);
        OnBrowserScroll(index, 8, reference);
    });

    $("#content").on('click', '#LineSaleSeriesMenu #line-sale-staff', function () {
        reference.invokeMethodAsync("DisplayStaffBySale");
        hiddenContextMenu("LineSaleSeriesMenu");
    });

    $("#content").on('click', '#LineSaleSeriesMenu #line-sale-customer', function () {
        reference.invokeMethodAsync("DisplayCustomerBySale");
        hiddenContextMenu("LineSaleSeriesMenu");
    });

    $("#content").on('click', '#LineSaleSeriesMenu #line-sale-group-bar', function () {
        reference.invokeMethodAsync("DisplayGroupClassificationBarBySale");
        hiddenContextMenu("LineSaleSeriesMenu");
    });

    $("#content").on('click', '#LineSaleSeriesMenu #line-sale-group-pie', function () {
        reference.invokeMethodAsync("DisplayGroupClassificationPieBySale");
        hiddenContextMenu("LineSaleSeriesMenu");
    });

    $("#content").on('click', '#BarStaffSeriesMenu #bar-staff-customer', function () {
        reference.invokeMethodAsync("DisplayCustomerByStaff");
        hiddenContextMenu("BarStaffSeriesMenu");
    });

    $("#content").on('click', '#BarStaffSeriesMenu #bar-staff-group-bar', function () {
        reference.invokeMethodAsync("DisplayGroupClassificationBarByStaff");
        hiddenContextMenu("BarStaffSeriesMenu");
    });

    $("#content").on('click', '#BarStaffSeriesMenu #bar-staff-group-pie', function () {
        reference.invokeMethodAsync("DisplayGroupClassificationPieByStaff");
        hiddenContextMenu("BarStaffSeriesMenu");
    });

    $("#content").on('click', '#BarCustomerSeriesMenu #bar-customer-staff', function () {
        reference.invokeMethodAsync("DisplayStaffByCustomer");
        hiddenContextMenu("BarCustomerSeriesMenu");
    });

    $("#content").on('click', '#BarCustomerSeriesMenu #bar-customer-group-bar', function () {
        reference.invokeMethodAsync("DisplayGroupClassificationBarByCustomer");
        hiddenContextMenu("BarCustomerSeriesMenu");
    });

    $("#content").on('click', '#BarCustomerSeriesMenu #bar-customer-group-pie', function () {
        reference.invokeMethodAsync("DisplayGroupClassificationPieByCustomer");
        hiddenContextMenu("BarCustomerSeriesMenu");
    });

    $("#content").on('click', '#GroupClassificationSeriesMenu #group-staff', function () {
        reference.invokeMethodAsync("DisplayStaffByGroupClassification");
        hiddenContextMenu("GroupClassificationSeriesMenu");
    });

    $("#content").on('click', '#GroupClassificationSeriesMenu #group-customer', function () {
        reference.invokeMethodAsync("DisplayCustomerByGroupClassification");
        hiddenContextMenu("GroupClassificationSeriesMenu");
    });

    function hiddenContextMenu(id) {
        $("#" + id + ":first").addClass("blazor-context-menu--hidden");
    }
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
function requestResetFrameWCount() {
    var i = 1;
    var isDone = false;
    while (i < 100) {
        (function (i) {
            setTimeout(function () {
                if (!isDone) {
                    isDone = ResetFrameWCount();
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
    var beginSelected = $("#sale-grid-area .grid-display tr.selected-area:first");
    var endSelected = $("#sale-grid-area .grid-display tr.selected-area:last");
    if (beginSelected.length == 0) {
        return true;
    }
    var start = saleGridTr.index(beginSelected[0]);
    var end = saleGridTr.index(endSelected[0]);
    var chartArea = document.getElementById("sale-chart-area");
    var areaFrame = chartArea.getBoundingClientRect();
    var gridGroup = chartArea.querySelector(".dxc-grids-group");
    if (gridGroup == null) {
        return false;
    }
    var scroll = window.scrollY
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
function ResetFrameWCount() {
    var saleGridTr = $("#sale-grid-area-wcount .grid-display-wcount tr:not(:first)");
    var count = saleGridTr.length / 2;
    var beginSelected = $("#sale-grid-area-wcount .grid-display-wcount tr.selected-area-wcount:first");
    var endSelected = $("#sale-grid-area-wcount .grid-display-wcount tr.selected-area-wcount:last");
    if (beginSelected.length == 0) {
        return true;
    }
    var start = Math.floor(saleGridTr.index(beginSelected[0]) / 2);
    var end = Math.floor(saleGridTr.index(endSelected[0]) / 2);
    var chartArea = document.getElementById("sale-chart-area");
    var areaFrame = chartArea.getBoundingClientRect();
    var gridGroup = chartArea.querySelector(".dxc-grids-group");
    if (gridGroup == null) {
        return false;
    }
    var scroll = window.scrollY
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
        if (window.getSelection) {
            if (window.getSelection().empty) {
                window.getSelection().empty();
            } else if (window.getSelection().removeAllRanges) {
                window.getSelection().removeAllRanges();
            }
        } else if (document.selection) {
            document.selection.empty();
        }
        ResetFrame();
        reference.invokeMethodAsync("WhenDragComplete", fromIndex, toIndex, e.clientX, e.clientY);
        document.onmouseup = null;
        document.onmousemove = null;
    }
    function elementDrag(e) {
        var selection = window.getSelection();
        var parent = getParentElement(selection.extentNode, "tr");
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

function SelectTimeByGridWCount(indexBegin, count, reference) {
    var saleGridTr = $("#sale-grid-area-wcount .grid-display-wcount tr:not(:first)");
    saleGridTr.each(function () {
        $(this).removeClass("selected-area-wcount");
    });
    saleGridTr.each(function (index) {
        if (index % 2 != 0) {
            return;
        }
        if (index == indexBegin) {
            $(this).addClass("selected-area-wcount");
            $(this).next().addClass("selected-area-wcount");
        } else {
            $(this).removeClass("selected-area-wcount");
            $(this).next().removeClass("selected-area-wcount");
        }
    });
    document.onmouseup = closeDragElement;
    document.onmousemove = elementDrag;
    var fromIndex = indexBegin;
    var toIndex = indexBegin;

    function closeDragElement(e) {
        if (window.getSelection) {
            if (window.getSelection().empty) {
                window.getSelection().empty();
            } else if (window.getSelection().removeAllRanges) {
                window.getSelection().removeAllRanges();
            }
        } else if (document.selection) {
            document.selection.empty();
        }
        ResetFrameWCount();
        reference.invokeMethodAsync("WhenDragComplete", Math.floor(fromIndex / 2), Math.floor(toIndex / 2), e.clientX, e.clientY);
        document.onmouseup = null;
        document.onmousemove = null;
    }
    function elementDrag(e) {
        var selection = window.getSelection();
        var parent = getParentElement(selection.extentNode, "tr");
        var indexDrag = saleGridTr.index(parent);
        if (indexDrag < 0) {
            return;
        }
        fromIndex = Math.min(indexDrag, indexBegin);
        toIndex = Math.max(indexDrag, indexBegin);
        if (fromIndex % 2 != 0) {
            fromIndex = fromIndex - 1;
        }
        if (toIndex % 2 == 0) {
            toIndex = toIndex + 1;
        }
        saleGridTr.each(function (index) {
            if (fromIndex <= index && index <= toIndex) {
                $(this).addClass("selected-area-wcount");
            } else {
                $(this).removeClass("selected-area-wcount");
            }
        });
    }
}

function getParentElement(element, type) {
    if (element == null || element.localName == type) {
        return element;
    }
    return getParentElement(element.parentElement, type);
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

function fadeToggleWidthAdjustHeight() {
    AdjustHeight();
    $("#content").on('click', '.supermenu-title-section', function () {
        setTimeout(function () {
            AdjustHeight();
        }, 500);
    });
    window.addEventListener("resize", function () {
        if ($('.supermenu-title-section').length > 0) {
            setTimeout(function () {
                AdjustHeight();
            }, 500);
        }
    });
}

function AdjustKobanTableHeight() {
    $("#content").on('click', '.supermenu-title-section', function () {
        setTimeout(function () {
            var browserHeight = $(window).height();
            var scroll = window.scrollY
            var conditionDiv = $('.express-condition.mb-2');
            var saveKoban = $('#save-koban');
            var saveKobanAreaRect = saveKoban[0].getBoundingClientRect();
            var tableArea = $('#koban-table-wrapper');
            var tableAreaHeight = tableArea[0].offsetHeight;
            var filterAreaHeight = conditionDiv[0].offsetHeight;
            if (conditionDiv.is(":visible")) {
                var adjustHeightValue = tableAreaHeight + (browserHeight - saveKobanAreaRect.top  - 20) - 15;
                tableArea.css('max-height', adjustHeightValue + "px");
                tableArea.css('min-height', adjustHeightValue + "px");
            } else {
                var adjustHeightValue = tableAreaHeight - (saveKobanAreaRect.bottom - browserHeight  - filterAreaHeight + 15) - 10;
                tableArea.css('min-height', adjustHeightValue + "px");
                tableArea.css('max-height', adjustHeightValue + "px");
            }
        }, 500);
    });
}

function AdjustHeight() {
    var browserHeight = $(window).height();
    var conditionDiv = $('.express-condition.mb-2');
    var tableArea = $('#table-wrapper');
    var gridArea = $('.super-grid-size');
    var tableAreaHeight = tableArea[0].offsetHeight;
    var totalArea = $('#total-area');
    var totalAreaRect = totalArea[0].getBoundingClientRect();
    var pagingArea = $('#super-pagination-area');
    var pagingAreaRect = pagingArea[0].getBoundingClientRect();
    var adjustHeightValue = 0;
    var scroll = window.scrollY
    if (conditionDiv.is(":visible")) {
        adjustHeightValue = tableAreaHeight + (browserHeight - totalAreaRect.top - scroll);
    } else {
        adjustHeightValue = tableAreaHeight - (totalAreaRect.bottom - browserHeight + scroll);
    }
    tableArea.css('max-height', adjustHeightValue + "px");
    tableArea.css('min-height', adjustHeightValue + "px");
    gridArea.css('max-height', (adjustHeightValue - pagingAreaRect.height) + "px");
    gridArea.css('min-height', (adjustHeightValue - pagingAreaRect.height) + "px");
}

function handleSelectByKeyUp(reference) {
    document.onkeydown = handleKeyDown
    document.onkeyup = handleKeyUp;
    function handleKeyDown(e) {
        var isTargetBody = $(e.target).is("body") || e.target.type == 'checkbox';
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
        var isTargetBody = $(e.target).is("body") || e.target.type == 'checkbox';
        if (isTargetBody && (e.keyCode == 16 || e.keyCode == 17)) {
            reference.invokeMethodAsync("KeyUpComplete");
        }
    }
}
function formatNumber(number, unit) {
    return String(number).replace(regexNumberGroup, ',') + ' ' + unit + (number == 1 ? '' : 's');
}

function encode(string) {
    // URL-encode some more characters to avoid issues when using permalink URLs in Markdown
    return encodeURIComponent(string).replace(/['()_*]/g, function (character) {
        return '%' + character.charCodeAt().toString(16);
    });
}
function settextinlinetextarea(len) {
    $("#txtJourneys").keyup(function (e) {
        var arrayForEachLine = this.value.split(/\r\n|\r|\n/g);
        var newVal = [];
        for (var i = 0; i < arrayForEachLine.length; i++) {
            encodedValue = encode(arrayForEachLine[i]),
                characterCount = [...arrayForEachLine[i]].length;
            var offset = arrayForEachLine[i];
            if ([...offset].length >= 37) {
                byteCounts = new Blob([offset]).size;
                while ([...offset].length >= 37) {
                    newVal.push(slice(offset, 0, 37));
                    offset = slice(offset, 37, [...offset].length);
                }
                if (offset != "") {
                    newVal.push(offset);
                }
            } else {
                newVal.push(arrayForEachLine[i]);
            }
        }
        this.value = newVal.join('\n');

    })
}
function charAt(string, index) {
    var first = string.charCodeAt(index);
    var second;
    if (first >= 0xD800 && first <= 0xDBFF && string.length > index + 1) {
        second = string.charCodeAt(index + 1);
        if (second >= 0xDC00 && second <= 0xDFFF) {
            return string.substring(index, index + 2);
        }
    }
    return string[index];
}
function toNumber(value, fallback) {
    if (value === undefined) {
        return fallback;
    } else {
        return Number(value);
    }
}
function slice(string, start, end) {
    var accumulator = "";
    var character;
    var stringIndex = 0;
    var unicodeIndex = 0;
    var length = string.length;

    while (stringIndex < length) {
        character = charAt(string, stringIndex);
        if (unicodeIndex >= start && unicodeIndex < end) {
            accumulator += character;
        }
        stringIndex += character.length;
        unicodeIndex += 1;
    }
    return accumulator;
}

function substrBytes(str, start, length) {
    var buf = new Buffer(str);
    return buf.slice(start, start + length).toString();
}

function openNewUrlInNewTab(url) {
    window.open(url, '_blank');
}
function adjustHyperAreaWidth() {
    resetHyperAreaWidth();
    window.addEventListener("resize", function () {
        resetHyperAreaWidth();
    });
}
function resetHyperAreaWidth() {
    var rowWidth = $('.condition-display-row').width();
    if (rowWidth == undefined) {
        return;
    }
    var minConditionRowWidth = parseFloat($('.left-search-condition').css('min-width'));
    if (rowWidth < (minConditionRowWidth + 2) * 2) {
        $('.left-search-condition, .right-search-condition').width("calc(100% - 2px)");
        $('.top-left-condition').width("calc(56% - 2px)");
        $('.top-right-condition').width("calc(44% - 2px)");
        $('#right-top-label').attr("style", "min-width: 60px !important; width: 60px !important");
        $('#item-condition').width("calc(100% - 68px)");
    } else {
        $('.left-search-condition, .right-search-condition, .top-left-condition, .top-right-condition').width("calc(50% - 2px)");
        $('#right-top-label').attr("style", "margin-left: 2px");
        $('#item-condition').width("calc(100% - 106px)");
    }
}
//Load script of specific page
function loadPageScript(pageName, functionName) {
    let url = `js/pages/${pageName}.js`;
    let params = Array.prototype.slice.call(arguments).splice(2)
    let completion = function () { console.log(`${url} is loaded`) }
    if (functionName) {
        completion = function () { window[functionName].apply(this, params); }
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

// Ly write
window.browserResize = {
    getInnerWidth: function () {
        let element = $(".busData__wrap");
        let scrollbar = 17; //element[0].offsetWidth - element[0].clientWidth;
        let width = element.width() - $(".busData-name").outerWidth() - scrollbar;
        return width;
    },

    getCallbackInnerWidth: function () {
        let width = null;
        let element = $(".busData__wrap");
        let scrollbar = element[0].offsetWidth - element[0].clientWidth;
        if (parseFloat($("#kobo-vertical-menu").css("margin-left"), 10) < 0) {
            let outerWidth = $(".busData-name").outerWidth();
            if (isNumber(outerWidth)) {
                width = element.width() - $(".busData-name").outerWidth() - scrollbar - $("#kobo-vertical-menu").outerWidth();
            }
            else {
                width = element.width() - scrollbar - $("#kobo-vertical-menu").outerWidth();
            }
        }
        else {
            let outerWidth = $(".busData-name").outerWidth();
            if (isNumber(outerWidth)) {
                width = element.width() - $(".busData-name").outerWidth() - scrollbar + $("#kobo-vertical-menu").outerWidth();
            }
            else {
                width = element.width() - scrollbar + $("#kobo-vertical-menu").outerWidth();
            }
        }
        return width;
    },

    collapseButtonFixedBottom: function() {
        document.getElementById("kobo-menu-btn").addEventListener("click", function () {
            let isCollapse = !document.getElementById('content').classList.contains('collapse');
            if(isCollapse) {
                document.getElementsByClassName("button-fixed-bottom")[0].classList.add("bottom-expand");
            } else {
                document.getElementsByClassName("button-fixed-bottom")[0].classList.remove("bottom-expand");
            }
        });
    },
    registerWidthCallback: function () {
        document.getElementById("kobo-menu-btn").addEventListener("click", function () {
            console.log('click');
            DotNet.invokeMethodAsync("HassyaAllrightCloud", 'OnBrowserCallbackWidth');
        });
    },
    registerResizeCallback: function () {
        window.addEventListener("resize", function () {
            DotNet.invokeMethodAsync("HassyaAllrightCloud", 'OnBrowserResize');
        });
    },
}

window.browserResizeStaff = {
    getInnerWidth: function () {
        let outerWidth = $(".busData-name").outerWidth();
        if (isNumber(outerWidth)) {
            return (window.innerWidth - $("#kobo-vertical-menu").width() - 45) / 2 - $(".busData-name").outerWidth() - 2;
        }
        else {
            return (window.innerWidth - $("#kobo-vertical-menu").width() - 45) / 2 - 2;
        }
    },

    getCallbackInnerWidth: function () {
        let outerWidth = $(".busData-name").outerWidth();
        if (isNumber(outerWidth)) {
            return (window.innerWidth + parseFloat($("#kobo-vertical-menu").css("margin-left"), 10) - 45) / 2 - $(".busData-name").outerWidth() - 2;
        }
        else {
            return (window.innerWidth + parseFloat($("#kobo-vertical-menu").css("margin-left"), 10) - 45) / 2 - 2;
        }
    },
}

function fadeToggle() {
    $("#content").on('click', '.title-section', function (e) {
        if ($(".listColumn").length) {
            loadPageScript("busSchedulePage", "onInit");
        }
        //e.stopPropagation();
        var $element = $(this).next();
        var $icon = $(this).find('i');
        if ($element.is(':visible')) {
            $element.slideUp();
            $icon.removeClass('fa-angle-up').addClass('fa-angle-down');
        } else {
            $element.slideDown();
            $icon.removeClass('fa-angle-down').addClass('fa-angle-up');
        }
    });
}

function removeTooltip() {
    $('[data-toggle="tooltip"]').tooltip('hide');
    $('body>.tooltip').remove();
}

function scroll() {
    $('.scrollbar-macosx').scrollbar();
    $(".scrollbar-outer").scrollbar();
}

function scrollCallBackEvent() {
    $('.scrollbar-macosx').scrollbar();
    $(".scrollbar-outer").scrollbar();
    document.addEventListener('scroll', function (event) {
            DotNet.invokeMethodAsync("HassyaAllrightCloud", 'ScrollEvt');
        });
}

function zoomTabBooking() {
    $(".zoom-icon--expand").click(function () {
        $(".col-6+.zoom").addClass("active");
    })
    $(".zoom-icon--compress").click(function () {
        $(".col-6+.zoom").removeClass("active");
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

// Use for save report as excel/PDF
function downloadFileClientSide(file, fileType, reportName) {
    contentType = "application/pdf";
    var sliceSize = 1024;
    var byteCharacters = atob(file);
    var bytesLength = byteCharacters.length;
    var slicesCount = Math.ceil(bytesLength / sliceSize);
    var byteArrays = new Array(slicesCount);
    for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
        var begin = sliceIndex * sliceSize;
        var end = Math.min(begin + sliceSize, bytesLength);
        var bytes = new Array(end - begin);
        for (var offset = begin, i = 0; offset < end; ++i, ++offset) {
            bytes[i] = byteCharacters[offset].charCodeAt(0);
        }
        byteArrays[sliceIndex] = new Uint8Array(bytes);
    }
    var blob = new Blob(byteArrays, { type: contentType });
    var a = window.document.createElement("a");
    a.href = window.URL.createObjectURL(blob, { type: "text/plain" });
    var today = new Date();
    var now = today.getFullYear() + (leadByZero(today.getMonth() + 1)) +
        leadByZero(today.getDate()) + leadByZero(today.getHours()) + leadByZero(today.getMinutes());
    var name = reportName + now + ".";
    a.download = name + fileType;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

function leadByZero(x) {
    return x < 10 ? "0" + x : "" + x
}

function loadSortableJs(elementId, reference) {
    var favoriteMenuList = document.getElementById(elementId);
    Sortable.create(favoriteMenuList, {
        onUpdate: function (evt) {
            var itemId = $(evt.item).data("id");  // dragged HTMLElement
            var oldIndex = evt.oldIndex;  // element's old index within old parent
            var newIndex = evt.newIndex;  // element's new index within new parent
            var list = evt.from.getElementsByClassName('sortable--item');
            var orderedList = [];
            [].forEach.call(list, function (favoriteMenuList, index) {
                favoriteMenuList.setAttribute("data-order", index);
                orderedList.push({
                    id: favoriteMenuList.getAttribute("data-id"),
                    order: favoriteMenuList.getAttribute("data-order")
                });
            });
            console.log(list);
            reference.invokeMethodAsync("OnFavouriteMenuOrderChange", orderedList, elementId, oldIndex, newIndex);
        }
    });
}
function scrollToTop(selector = "#table-container") {
    $(selector).scrollTop(0);
}
function resizeOneColumn(selector = ".resize") {
    let width = $("#resize-header").width();
    let el = document.getElementById("resize-item");
    let isMouseDown = false;
    let pageX;
    el.addEventListener("mousedown", function (e) {
        pageX = e.pageX;
        isMouseDown = true;
    });
    document.addEventListener("mouseup", function () {
        isMouseDown = false;
        pageX = undefined;
    });
    document.addEventListener("mousemove", function (e) {
        if (isMouseDown) {
            let diffX = e.pageX - pageX;
            pageX = e.pageX;
            width = width + diffX;
            if (width < 400) {
                width = 400
            }
            var els = document.querySelectorAll('.resize');
            for (var i = 0; i < els.length; i++) {
                els[i].setAttribute("style", `width: ${width}px`);
            }
        }
    });
}
function setEventforDecimalField(selector = ".two-decimal-3", firstPart = 3) {
    // $(".number :input").attr('type', 'tel');

    // remove comma in currency field when focus
    $(`${selector} :input`).focus(function (e) {
        if (this.value.includes(".00")) {
            this.value = this.value.replace(".00", "");
        }
    });
    // re-format currency with comma
    $(`${selector} :input`).blur(function (e) {
        if (this.value.length > 0) {
            this.value = parseFloat(this.value).toFixed(2);
        }
        else {
            this.value = parseFloat(0).toFixed(2);
        }
    });
    // just allow input number
    $(`${selector} :input`).keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8 || x == 46)) {
            if (this.value.length < firstPart || (this.value.length == firstPart && x == 46) || this.value.includes("."))
                return true;
            else {
                e.preventDefault();
                return false;
            }
        }
        else {
            e.preventDefault();
            return false;
        }
    });
    //disable drop text
    $(`${selector} :input`).bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable drop text
    $(`${selector} :input`).bind("drop", function (e) {
        var data = e.originalEvent.dataTransfer.getData("text");
        if (isNumber(data) == false)
            e.preventDefault();
    });
}
function formatTime(selector = '.time') {
    $(`${selector} input`).focus(function (e) {
        if (this.value == "00:00")
            this.value = "";
        else
            this.value = this.value.replace(":", "");
    })
    // just allow input number
    $(`${selector} input`).keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
    $(`${selector} input`).blur(function (e) {
        if (this.value.length > 3) {
            let temp = this.value.replace(":", "");
            this.value = temp.slice(0, 2) + ":" + temp.slice(2);
        }
        else {
            this.value = "00:00";
        }
    })
}
function selectedRow() {
    $(".normal-table tbody tr").click(function () {
        $(this).addClass("selected-row").siblings().removeClass("selected-row");
    });
}
function inputNumber(selector = ".number", inputNegativeNumber = false) {
    // just allow input number
    $(`${selector} input`).keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8) || (inputNegativeNumber && x == 45))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
}

//press enter to next tabindex and shift+enter in pre tabindex
function EnterTab(selector = ".focus-form", ignoreFirstItem = false, useSelectorOnly = false) {
    {
        let el = useSelectorOnly ? selector : `${selector} input:not([readonly]), ${selector} button.lifecycle-btn`;
        let el$ = $(el);
        // Setting focus on first textbox
        if (ignoreFirstItem)
            el$.splice(0, 1);

        // binding keydown event to textbox

        el$.bind('keydown', function (e) {
            // detecting keycode returned from keydown and comparing if its equal to 13 (enter key code)
            if ((e.which == 13 || e.which == 9) && e.shiftKey) {
                // by default if you hit enter key while on textbox so below code will prevent that default behaviour

                e.preventDefault();

                // getting next index by getting current index and incrementing it by 1 to go to next textbox

                var preIndex = $(el).index(this) - 1;
                var maxIndex = $(el).length;

                if (preIndex < 0)
                    preIndex = maxIndex - 1; // focus on last input

                for (i = preIndex; i > 0; i--) {
                    var selectedIndex = $(el).eq(i).attr('tabindex');
                    if (selectedIndex == -1) {
                        preIndex = preIndex - 1;
                    }
                    else {
                        preIndex = preIndex;
                        break;
                    }
                }
                if (preIndex > -1) {
                    // setting index to next textbox using CSS3 selector of nth child

                    $(el).eq(preIndex).focus();
                }
            }
            else
                if (e.keyCode == 13 || e.which == 9) {
                    // by default if you hit enter key while on textbox so below code will prevent that default behaviour

                    e.preventDefault();
                    // getting next index by getting current index and incrementing it by 1 to go to next textbox

                    var nextIndex = $(el).index(this) + 1;

                    // getting total number of textboxes on the page to detect how far we need to go

                    var maxIndex = $(el).length;

                    if (nextIndex >= maxIndex)
                        nextIndex = 0; // focus on first input

                    for (i = nextIndex; i < maxIndex; i++) {
                        var selectedIndex = $(el).eq(i).attr('tabindex');
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
                        $(el).eq(nextIndex).focus();
                    }
                }
        });
    }
}

function setBlurEventOnPressEnter(selector = ".enter") {
    $(`${selector} input`).keypress(function (e) {
        if (e.which == 13)
            $(this).blur();
    });
}

function focus(selector = '.focus-after-input-code') {
    $(`${selector} input`).focus();
}

function setEventforNumberField(selector = '.code-number', type = 'number') {
    $(`${selector} input`).attr('type', 'tel');

    // remove comma in currency field when focus
    $(`${selector} input`).focus(function (e) {
        if (this.value != null) {
            this.value = this.value.trim();
        }
        if (this.value != null && this.value.trim() != "" && !isNaN(this.value)) {
            if (type == "decimal") {
                this.value = parseFloat(this.value).toFixed(1);
            } else {
                this.value = parseInt(this.value);
            }
        }
    });

    // re-format currency with comma
    $(`${selector} input`).blur(function (e) {
        if (this.value != null) {
            this.value = this.value.trim();
        }

        if (this.value != null && this.value.trim() != "" && !isNaN(this.value)) {
            if (type == "decimal") {
                this.value = parseFloat(this.value).toFixed(1);
            } else {
                this.value = parseInt(this.value);
            }
        }
    });

    // just allow input number
    $(`${selector} input`).keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8 ||
            (x >= 35 && x <= 40) || x == 46)) {
            return true;
        }
        else {
            e.preventDefault();
            return false;
        }
    });

    // prevent japanese input
    $(`${selector} input`).keydown(function (e) {
        var x = e.which || e.keycode;
        if (x == 13) {
            let val = $(`${selector} input`).val();
            if (val && isNaN(val)) {
                $(`${selector} input`).val('');
            }
        }
    });

    //disable drop text
    $(`${selector} input`).bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable paste text
    $(`${selector} input`).bind("drop", function (e) {
        var data = e.originalEvent.dataTransfer.getData("text");
        if (isNumber(data) == false)
            e.preventDefault();
    });
    // not allow japanse
    $(`${selector} :input`).keyup(function (e) {
        var regex = "[\u3040-\u309f]|[\u30a0-\u30ff]|[\uff66-\uff9f]|[\u4e00-\u9faf]";
        if (!regex.match(this.value))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
}

function setEventforCodeNumberFieldV2(selector = '.code-number', formatOnBlur = true, padLength = 5, isNumber = false) {
    $(`${selector} input`).attr('type', 'tel');

    // remove comma in currency field when focus
    $(`${selector} input:not([readonly])`).focus(function (e) {
        if (this.getAttribute("readonly") == null && this.value.length > 0) {
            this.value = parseInt(this.value);
        }
    });
    // re-format currency with comma
    $(`${selector} input`).blur(function (e) {
        if (this.value.length > 0 && formatOnBlur) {
            this.value = this.value.padStart(padLength, '0');
        }
    });

    // just allow input number
    $(`${selector} input`).keypress(function (e) {
        var x = e.which || e.keycode;
        if (isNumber) {
            if ((x >= 48 && x <= 57) || x == 8)
                return true;
            else {
                e.preventDefault();
                return false;
            }
        }
        else {
            if (((x >= 48 && x <= 57) || x == 8 ||
                (x >= 35 && x <= 40) || x == 46))
                return true;
            else {
                e.preventDefault();
                return false;
            }
        }
    });

    // prevent japanese input
    $(`${selector} input`).keydown(function (e) {
        var x = e.which || e.keycode;
        if (x == 13) {
            let val = $(`${selector} input`).val();
            if (val && isNaN(val)) {
                $(`${selector} input`).val('');
            }
        }
    });

    //disable drop text
    $(`${selector} input`).bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable paste text
    $(`${selector} input`).bind("drop", function (e) {
        var data = e.originalEvent.dataTransfer.getData("text");
        if (isNumber(data) == false)
            e.preventDefault();
    });
    // not allow japanse
    $(`${selector} :input`).keyup(function (e) {
        var regex = "[\u3040-\u309f]|[\u30a0-\u30ff]|[\uff66-\uff9f]|[\u4e00-\u9faf]";
        if (!regex.match(this.value))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
}

function initSelectableRow(selector = '.tr-selectable') {
    $(selector).click(function () {
        $(this).siblings().removeClass('selected');
        $(this).addClass('selected');
    });
}

function initClickOutSide(elementId, dotnetHelper) {
    window.addEventListener("click", (e) => {
        let datePickerPopup = $('.dxbs-dm.dropdown-menu.dxbs-dropdown-area');
        let datePickerPopup1 = $('.modal.fade.show.dxbs-dropdown-modal');
        let flag = false;
        if (datePickerPopup) {
            for (let i of datePickerPopup) {
                if ($.contains(i, e.target)) {
                    flag = true;
                    break;
                }
            }
        }
        if (datePickerPopup1) {
            for (let i of datePickerPopup1) {
                if ($.contains(i, e.target)) {
                    flag = true;
                    break;
                }
            }
        }
        if (!document.getElementById(elementId).contains(e.target) && !flag) {
            dotnetHelper.invokeMethodAsync("InvokeClickOutside");
        }
    });
}

function formatHHmmss(selector = '.time') {
    $(`${selector} input`).focus(function (e) {
        if (this.value == "00:00:00")
            this.value = "";
        else
            this.value = this.value.replaceAll(":", "");
    })

    // just allow input number
    $(`${selector} input`).keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });

    $(`${selector} input`).blur(function (e) {
        if (this.value.length > 3) {
            let temp = this.value.replaceAll(":", "");
            this.value = temp.slice(0, 2) + ":" + temp.slice(2, 4) + ":" + temp.slice(4);
        }
        else {
            this.value = "00:00:00";
        }
    })
}


function formatDecimalField(selector = ".kobo-decimal", numCount = 2, deCount = 1) {
    let decimalFormat = '0';
    // remove comma in currency field when focus
    $(`${selector} :input`).focus(function (e) {
        if (this.value.includes(`.${decimalFormat}`)) {
            this.value = this.value.replace(`.${decimalFormat}`, ``);
        }
    });
    // re-format currency with comma
    $(`${selector} :input`).blur(function (e) {
        if (this.value.length > 0) {
            this.value = parseFloat(this.value).toFixed(deCount);
        }
        else {
            this.value = parseFloat(0).toFixed(deCount);
        }
    });
    // just allow input number
    $(`${selector} :input`).keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8 || x == 46)) {
            if (this.value.length < numCount || (this.value.length == numCount && x == 46) || this.value.includes("."))
                return true;
            else {
                e.preventDefault();
                return false;
            }
        }
        else {
            e.preventDefault();
            return false;
        }
    });
    //disable drop text
    $(`${selector} :input`).bind("paste", function (e) {
        var clipboarddata = window.event.clipboardData.getData('text');
        if (isNumber(clipboarddata) == false)
            e.preventDefault();
    });
    //disable drop text
    $(`${selector} :input`).bind("drop", function (e) {
        var data = e.originalEvent.dataTransfer.getData("text");
        if (isNumber(data) == false)
            e.preventDefault();
    });
}

function initTooltip(selector = '[data-toggle="tooltip"]',
    template = '<div class="tooltip custom-tooltip" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>') {
    $(selector).tooltip({ template: template });
}

function showNofitications() {
    $(".notifications-link").on("click", function (e) {
        $(".slide-in").toggleClass("is_opened");
        e.stopPropagation();
    });
    $(window).on("click", function (e) {
        if ($(".slide-in").has(event.target).length == 0 && !$(".slide-in").is(event.target)) {
            $(".slide-in").removeClass("is_opened");
        }
    });
}

function closeNofitications() {
    $(".notifications-icon").click(function () {
        $(".slide-in").removeClass("is_opened");
    })
}

function getViewWidth() {
    return window.innerWidth;
}

function speechRecognition(lang, thisReference) {
    let isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
    if (isChrome) {
        var SpeechRecognition = SpeechRecognition || webkitSpeechRecognition;
        let recognition = new SpeechRecognition();
        let button = document.getElementById("start");
        let icon = document.getElementById("speak-icon");
        let effect = document.getElementById("speak-effect");
        let input = document.getElementById("input-speech");
        let flag = true;
        let id = undefined;
        recognition.lang = lang;
        recognition.continuous = false;
        recognition.interimResults = false;
        recognition.maxAlternatives = 1;

        recognition.onstart = function () {
        };

        recognition.onspeechend = function () {
            // when user is done speaking
            recognition.stop();
        }

        // This runs when the speech recognition service returns result
        recognition.onresult = function (event) {
            var transcript = event.results[0][0].transcript;
            //var confidence = event.results[0][0].confidence;
            thisReference.invokeMethodAsync("InputSpeech", transcript).then(function () { }, function (err) { });
        };

        //recognition.onnomatch = function (event) {
        //    thisReference.invokeMethodAsync("InputSpeech", "").then(function () { }, function (err) { });
        //};

        recognition.onend = function () {
            flag = true;
            input.disabled = false;
            button.classList.remove("btn-danger");
            button.classList.add("btn-outline-secondary");
            icon.classList.remove("fa-microphone");
            icon.classList.add("fa-microphone-slash");
            effect.classList.remove("pulse-ring");
        }

        //recognition.onerror = function (event) {
        //    console.log("error", event.error);
        //}

        button.onclick = () => {
            if (flag) {
                recognition.start();
                input.disabled = true;
                button.classList.remove("btn-outline-secondary");
                button.classList.add("btn-danger");
                icon.classList.remove("fa-microphone-slash");
                icon.classList.add("fa-microphone");
                effect.classList.add("pulse-ring");
                flag = false;
                if (id) {
                    clearTimeout(id)
                }
                id = setTimeout(() => {
                    recognition.stop();
                }, 10000);
            }
            else {
                recognition.stop();
                flag = true;
            }
        }
    }
}
function setZeroInputFilter(selector = '.number', padLength = 10) {
    setTimeout(function () {
        let inputFilter = function (value) {
            return /^\d*$/.test(value);
        }
        let textboxes = $(`${selector} input`);
        for (let i = 0; i < textboxes.length; i++) {
            ["input", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
                textboxes[i].addEventListener(event, function () {
                    if (inputFilter(this.value)) {
                        this.oldValue = this.value;
                        this.oldSelectionStart = this.selectionStart;
                        this.oldSelectionEnd = this.selectionEnd;
                    } else if (this.hasOwnProperty("oldValue")) {
                        this.value = this.oldValue;
                        this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                    } else {
                        this.value = "";
                    }
                });
            });
            ["keyup", "blur"].forEach(function (event) {
                if (event == "keyup") {
                    textboxes[i].addEventListener(event, function () {
                        if (event.keyCode === 13 && this.value.length > 0) {
                            this.value = this.value.padStart(padLength, '0');
                        }
                    });
                } else {
                    textboxes[i].addEventListener(event, function () {
                        if (this.value.length > 0) {
                            this.value = this.value.padStart(padLength, '0');
                        }
                    });
                }
            });
        }
        textboxes.attr("maxlength", padLength);
    }, 1000);

}

function focusFirstItem(selector = ".focus-form") {
    document.querySelector(selector).querySelector("input:not(:disabled):not([readonly])").focus();
}

function setInputFilter(selector = '.number', positiveNum = true, maxlength = null) {
    let inputFilter = positiveNum ? function (value) {
        return /^\d*$/.test(value);
    } : function (value) {
        return /^-?\d*$/.test(value);
    };
    let textboxes = $(`${selector} input`);
    for (let i = 0; i < textboxes.length; i++) {
        ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
            textboxes[i].addEventListener(event, function () {
                if (inputFilter(this.value)) {
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                } else {
                    this.value = "";
                }

                if (!positiveNum && maxlength != null && this.value.length === (maxlength + 1)) {
                    this.value = this.value.slice(0, -1);
                }
            });
        });
    }
    if (maxlength != null) {
        positiveNum ? textboxes.attr("maxlength", maxlength) : textboxes.attr("maxlength", maxlength + 1);
    }
}

function setMaxlenghtforNumberField(selector = '.code-number-length') {
    $(`${selector} :input`).keyup(function (e) {
        var textboxes = $(`${selector} input`);
        var good = $(this).val()
            // remove any excess of 7 integer digits
            .replace(/^(\d{7})\d+/, '$1')
            // remove any excess of 2 decimal digits
            .replace(/(\.\d\d).+/, '$1');
        if (good !== $(this).val()) {
            // Only if something had to be fixed, update
            $(this).val(good);
        }
        // Determine max size depending on presence of point
        if (good.indexOf('.') > 1)
            textboxes.attr("maxlength", 10);
        else
            textboxes.attr("maxlength", 8);
    })
};

function clearToDayBtnCalendar(){
    const btns = document.querySelectorAll('.calendar.hide-clear-btn .btn-toolbar.card-footer');
    btns[0].setAttribute('style', 'display: none');
}

function showPageScroll() {
    document.body.style.paddingRight = "0px";
    document.body.classList.remove("custom-modal-open");
}

function hidePageScroll() {
    document.body.style.paddingRight = "16.6666px";
    document.body.classList.add("custom-modal-open");
}

function onOffScroll(isOn = true) {
    document.documentElement.style.overflow = isOn ? "auto" : "hidden";
}