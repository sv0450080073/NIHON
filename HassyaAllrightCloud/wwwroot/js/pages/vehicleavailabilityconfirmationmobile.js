var scheduler;
var mouseDownCount = 0;

function vhScheduleMobile(TimeZone, Lang, CultureInfo, currentDateInput, referece) {
    var divschedule = $("#scheduler");
    if (divschedule.length == 0) {
        setTimeout(function () {
            vhScheduleMobile(TimeZone, Lang, CultureInfo, currentDateInput);
        }, 500);
        return;
    }
    DevExpress.localization.locale(CultureInfo);
    var Dictionnary = {};
    Dictionnary[CultureInfo] = {
        "dxScheduler-allDay": Lang["AllDay"],
        "dxScheduler-editorLabelTitle": Lang["EditorLabelTitle"],
        "dxScheduler-editorLabelStartDate": Lang["EditorLabelStartDate"],
        "dxScheduler-editorLabelEndDate": Lang["EditorLabelEndDate"],
        "dxScheduler-editorLabelDescription": Lang["EditorLabelDescription"],
        "dxScheduler-editorLabelRecurrence": Lang["EditorLabelRecurrence"],
        "Done": Lang["Done"],
        "Cancel": Lang["Cancel"],
        "Select": " ",
        "dxScheduler-recurrenceHourly": Lang["RecurrenceHourly"],
        "dxScheduler-recurrenceDaily": Lang["RecurrenceDaily"],
        "dxScheduler-recurrenceWeekly": Lang["RecurrenceWeekly"],
        "dxScheduler-recurrenceMonthly": Lang["RecurrenceMonthly"],
        "dxScheduler-recurrenceYearly": Lang["RecurrenceYearly"],
        "dxScheduler-recurrenceRepeatEvery": Lang["RecurrenceRepeatEvery"],
        "dxScheduler-recurrenceEnd": Lang["RecurrenceEnd"],
        "dxScheduler-recurrenceNever": Lang["RecurrenceNever"],
        "dxScheduler-recurrenceOn": Lang["RecurrenceOn"],
        "dxScheduler-recurrenceRepeatHourly": Lang["RecurrenceRepeatHourly"],
        "dxScheduler-recurrenceRepeatDaily": Lang["RecurrenceRepeatDaily"],
        "dxScheduler-recurrenceRepeatWeekly": Lang["RecurrenceRepeatWeekly"],
        "dxScheduler-recurrenceRepeatMonthly": Lang["RecurrenceRepeatMonthly"],
        "dxScheduler-recurrenceRepeatYearly": Lang["RecurrenceRepeatYearly"],
        "dxScheduler-recurrenceAfter": Lang["RecurrenceAfter"],
        "dxScheduler-recurrenceRepeatCount": Lang["RecurrenceRepeatCount"],
        "dxScheduler-recurrenceRepeatOn": Lang["RecurrenceRepeatOn"],
        "dxScheduler-confirmRecurrenceEditMessage": Lang["ConfirmRecurrenceEditMessage"],
        "dxScheduler-confirmRecurrenceDeleteMessage": Lang["ConfirmRecurrenceDeleteMessage"],
        "dxScheduler-confirmRecurrenceEditSeries": Lang["ConfirmRecurrenceEditSeries"],
        "dxScheduler-confirmRecurrenceDeleteSeries": Lang["ConfirmRecurrenceDeleteSeries"],
        "dxScheduler-confirmRecurrenceEditOccurrence": Lang["ConfirmRecurrenceEditOccurrence"],
        "dxScheduler-confirmRecurrenceDeleteOccurrence": Lang["ConfirmRecurrenceDeleteOccurrence"],
        "dxCalendar-todayButtonText": Lang["Today"],
    }
    //var ScheduleData = ParseToObject(ScheduleDataDictJson);
    DevExpress.localization.loadMessages(Dictionnary);

    scheduler = $("#scheduler").dxScheduler({
        //dataSource: ScheduleData,
        views: [
            {
                type: "month",
                name: Lang["Month"],
            }
        ],
        adaptivityEnabled: true,
        timeZone: TimeZone,
        currentView: "month",
        currentDate: new Date(currentDateInput),
        height: 250,
        onCellClick: function (e) {
            var dateSelected = new Date(e.cellData.endDate);
            referece.invokeMethodAsync("SelectDate", dateSelected);
            e.cancel = true;
        },
        onCellDblClick: function (e) {
            e.cancel = true;
        },
    }).dxScheduler("instance");
}

function fadeToggleVhScheduleMB() {
    var tgTitleSection = $("#tableVhScheduleMB .vh-schedule-mobile-title-section");
    if (tgTitleSection.length == 0) {
        setTimeout(function () { fadeToggleVhScheduleMB(); }, 500);
        return;
    }
    tgTitleSection.each(function () {
        $(this).click(function () {
            var $element = $(this).next();
            var $icon = $(this).find('i:first');
            if ($element.is(':visible')) {
                setTimeout(function () {
                    $element.slideUp();
                    $icon.removeClass('fa-angle-up').addClass('fa-angle-down');
                }, 10);
            } else {
                setTimeout(function () {
                    $element.slideDown();
                    $icon.removeClass('fa-angle-down').addClass('fa-angle-up');
                }, 20);
            }
        });
    });
}

function displayDate(current, isFirst) {
    var div = $(".dx-scheduler-date-table td.dx-scheduler-date-table-cell.dx-scheduler-cell-sizes-horizontal.dx-scheduler-cell-sizes-vertical"); //
    if (div.length == 0) {
        setTimeout(function () { displayDate(current, isFirst); }, 500);
        return;
    }
    var isSet = 0;
    for (var i = 0; i < div.length; i++) {
        var chil = div[i].childNodes;
        if (chil.length > 0) {
            if (chil[0].innerText == current) {
                if (isSet == 0 && isFirst) {
                    $(div[i]).addClass("dx-state-focused dx-scheduler-focused-cell");
                    isSet = 1;
                }
                else {
                    $(div[i]).removeClass("dx-state-focused dx-scheduler-focused-cell");
                    isFirst = true;
                }
            }
            else {
                $(div[i]).removeClass("dx-state-focused dx-scheduler-focused-cell");
            };
        };
    };
    if (isSet == 0) {
        for (var i = 0; i < div.length; i++) {
            var chil = div[i].childNodes;
            if (chil.length > 0) {
                if (chil[0].innerText == current) {
                    $(div[i]).addClass("dx-state-focused dx-scheduler-focused-cell");
                    isSet = 1;
                }
            };
        };
    }
}

function mouseUp(referece, dateTime) {
    if (mouseDownCount > 10)
        referece.invokeMethodAsync("RedirectVehicleAvailabilityConfirmation", dateTime);
    clearInterval(mdcTimer);
}

function mouseDown() {
    mdcTimer = window.setInterval(function () {
        mouseDownCount++;
    }, 100);
    return false;
}