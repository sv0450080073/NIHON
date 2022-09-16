function ResizeScheduleFired() {
    $(window).on("resize", function () {
        repaintSchedule(500);
    });

    $("#content").on('click', '#sidebarCollapse', function () {
        repaintSchedule();
    });
}

function renderDriverSchedule(IsHomePage, ScheduleDataJson, ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo) {
    repaintSchedule(0, false);
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

    var ScheduleData = ParseToObject(ScheduleDataJson);
    var ScheduleTypeData = ParseToObject(ScheduleTypesDataJson);
    var VacationTypeData = ParseToObject(VacationTypesDataJson);
    var ScheduleLabelTypeData = ParseToObject(ScheduleLabelTypeDataJson);
    var StaffData = ParseToObject(StaffDataJson);
    var DisplaySettingData = ParseToObject(DisplaySettingDataJson);
    DevExpress.localization.loadMessages(Dictionnary);

    var init = true;
    $("#driver-schedule").dxScheduler({
        dataSource: ScheduleData,
        views: [
            {
                type: "month",
                name: Lang["Month"],
            },
            {
                type: "week",
                name: Lang["Week"],
                maxAppointmentsPerCell: "unlimited"
            }],
        timeZone: TimeZone,
        firstDayOfWeek: DisplaySettingData.weekStartDay.value,
        currentView: DisplaySettingData.defaultDisplayType.value == 0 ? "month" : "week",
        currentDate: new Date(),
        editing: !IsHomePage,
        appointmentTemplate: function (model) {
            var text = model.appointmentData.text ? model.appointmentData.text : "";
            var htmlString = "<div class='schedule-label-wrapper'><div class='schedule-label'>";
            if (model.appointmentData.scheduleType == 1) {
                htmlString += ("<div class='schedule-label-vacation-" + model.appointmentData.vacationType + "'>" + getTextById(model.appointmentData.vacationType, VacationTypeData) + "</div>");
            } else {
                htmlString += ("<div class='schedule-label-work'>" + Lang["OnWork"] + "</div>");
            }
            for (var label in model.appointmentData.scheduleLabel) {
                htmlString += ("<div class='schedule-label-" + model.appointmentData.scheduleLabel[label] + "'>" + getTextById(model.appointmentData.scheduleLabel[label], ScheduleLabelTypeData)  + "</div>");
            }
            htmlString += "</div>";
            htmlString += ("<div  class='schedule-content-inline'>" + text + "</div>");
            return $(htmlString);
        },
        dateCellTemplate: function (model) {
            if (CultureInfo != "ja-JP" || model.text.length == 1) {
                return model.text;
            }
            if (model.text.length == 3) {
                return getDateOfWeekInJapanese(model.text);
            }
            return model.date.getDate() + "（" + getDateOfWeekInJapanese(model.date.getDay()) + "）";
        },
        onContentReady: function (e) {
            if (init && e.component._currentView.type == 'week') {
                init = false;
                var currentHour = new Date().getHours() - 1;
                var minute = 30;
                if (DisplaySettingData.dayStartTime.value != "Now") {
                    currentHour = parseInt(DisplaySettingData.dayStartTime.value) - 1;
                }
                if (currentHour < 0) {
                    minute = 0;
                }
                e.component.scrollToTime(currentHour, minute, new Date());
            }
        },
        onAppointmentFormOpening: function (data) {
            data.cancel = IsHomePage;
            data.form.option("readOnly", data.appointmentData.isEditable === false);
            ResetOptionOnForm(data, data.appointmentData.scheduleType);
        },
        onAppointmentClick: function (e) {
            e.cancel = IsHomePage;
        },
        onAppointmentDblClick: function (e) {
            e.cancel = IsHomePage;
        }
    }).dxScheduler("instance");

    function ResetOptionOnForm(data, scheduleType) {
        var formItems = data.form.option("items");
        while (formItems[0].items[0].dataField != "text") {
            formItems[0].items.splice(0, 1);
        }
        
        formItems[0].items.splice(0, 0, {
            label: {
                text: Lang["ScheduleType"]
            },
            editorType: "dxSelectBox",
            dataField: "scheduleType",
            editorOptions: {
                items: ScheduleTypeData,
                displayExpr: "text",
                valueExpr: "id",
                onValueChanged: function (args) {
                    ResetOptionOnForm(data, args.value)
                }
            }
        });

        if (scheduleType == 1) {
            formItems[0].items.splice(1, 0, {
                label: {
                    text: Lang["VacationType"]
                },
                editorType: "dxSelectBox",
                dataField: "vacationType",
                editorOptions: {
                    items: VacationTypeData,
                    displayExpr: "text",
                    valueExpr: "id",
                }
            });
        } else if (scheduleType == 2) {
            formItems[0].items.splice(1, 0, {
                label: {
                    text: Lang["Participant"]
                },
                editorType: "dxTagBox",
                dataField: "staffs",
                editorOptions: {
                    items: StaffData,
                    displayExpr: "syainNm",
                    valueExpr: "syainCdSeq",
                }
            });
        } else {
            formItems[0].items.splice(1, 0, {
                itemType: "empty",
                colspan: 2
            });
        }

        formItems[0].items.splice(2, 0, {
            label: {
                text: Lang["WithLabel"]
            },
            editorType: "dxTagBox",
            dataField: "scheduleLabel",
            editorOptions: {
                items: ScheduleLabelTypeData,
                displayExpr: "text",
                valueExpr: "id",
            }
        });
        formItems[0].items.splice(3, 0, {
            itemType: "empty",
            colspan: 2
        });

        data.form.option("items", formItems);
    }
}

function ParseToObject(json) {
    var newJson = json.replace(/"([\w]+)":/g, function ($0, $1) {
        return ('"' + $1.charAt(0).toLowerCase() + $1.slice(1) + '":');
    });
    return JSON.parse(newJson);
}

function getTextById(id, data) {
    return DevExpress.data.query(data)
        .filter("id", id)
        .toArray()[0].text;
}

function getDateOfWeekInJapanese(dateOfWeek) {
    switch (dateOfWeek) {
        case 0:
        case "Sun":
            return "日";
        case 1:
        case "Mon":
            return "月";
        case 2:
        case "Tue":
            return "火";
        case 3:
        case "Wed":
            return "水";
        case 4:
        case "Thu":
            return "木";
        case 5:
        case "Fri":
            return "金";
        case 6:
        case "Sat":
            return "土";
    }
}

function repaintSchedule(delay = 250, isRepaint = true) {
    if (isRepaint) {
        setTimeout(function () {
            adjustHeightForScheduleWrapper();
            var element = document.getElementById("driver-schedule");
            if (element != null) {
                var instance = DevExpress.ui.dxScheduler.getInstance(element);
                instance.repaint();
            }
        }, delay);
    } else {
        adjustHeightForScheduleWrapper();
    }
}

function adjustHeightForScheduleWrapper() {
    var adJustHeight = $(".schedule-navigate .schedule-tab-index").height();
    $(".driver-schedule-wrapper").height("calc(100% - " + adJustHeight + "px)");
}

function fadeToggleExceptButton() {
    $("#content").on('click', '.group-schedule-title-section', function (e) {
        if (e.target.localName != 'button' && e.target.parentElement.localName != 'button') {
            $(this).find('i:first').toggleClass('fa-angle-up').toggleClass('fa-angle-down');
            $(this).next().slideToggle();
        }
        
    });
}