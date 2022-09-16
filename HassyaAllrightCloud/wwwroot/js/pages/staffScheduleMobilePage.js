var scheduleLabelChecked = [];
var isPublic = true;
var isUpdateEnd = false;
var recurrence;
var recurrenceForm;
var schedulerDisplay;
var currentDateSelected;
var Dictionnary = {};
var appointmentBackgroundColor = [
    {
        id: 1,
        color: "#f54337"
    }, {

        id: 2,
        color: "#c32a97"
    }, {

        id: 3,
        color: "#009588"
    }, {

        id: 4,
        color: "#33691e"
    }, {

        id: 5,
        color: "#f54337"
    }, {

        id: 6,
        color: "#f57c01"
    }, {

        id: 7,
        color: "#f50197"
    }, {

        id: 8,
        color: "#fdd735"
    }, {

        id: 90,
        color: "#d9d9d9"
    }, {

        id: 91,
        color: "#cbfafa"
    }
];
var init = true;
function scheduleStaffMobile(ScheduleDataDictJson, calendarJson, ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, referece, currentLoginUserId, currentDateInput, isStaff, currentYearMonth) {
    var divschedule = $("#scheduler");
    if (divschedule.length == 0) {
        setTimeout(function () { scheduleStaffMobile(ScheduleDataDictJson, calendarJson, ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, referece, currentLoginUserId, currentDateInput, isStaff, currentYearMonth); }
            , 300);
        return;
    }
    DevExpress.localization.locale(CultureInfo);
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
        "dxDateBox-validation-datetime": Lang["ValidationDateTime"],
        "validation-range": Lang["ValidationRange"],
    }
    var ScheduleData = ParseToObject(ScheduleDataDictJson);
    var CalendarData = ParseToObject(calendarJson);
    var ScheduleTypeData = ParseToObject(ScheduleTypesDataJson);
    var VacationTypeData = ParseToObject(VacationTypesDataJson);
    var ScheduleLabelTypeData = ParseToObject(ScheduleLabelTypeDataJson);
    var StaffData = ParseToObject(StaffDataJson);
    var DisplaySettingData = ParseToObject(DisplaySettingDataJson);
    DevExpress.localization.loadMessages(Dictionnary);
    var init = true;

    var scheduler = $("#scheduler").dxScheduler({
        dataSource: ScheduleData,
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
        onOptionChanged: (e) => {
            if (e.name == "currentDate" && !Array.isArray(e.value)) {
                var valueDate = JSON.stringify(e.value);

                referece.invokeMethodAsync("otherMonth", valueDate).then(data => {
                    e.component._initialized = false;
                    $("#scheduler").empty();
                    scheduleStaffMobile(data[0], calendarJson, ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, referece, currentLoginUserId, e.value, isStaff, currentYearMonth)
                });
            }
        },
        appointmentTemplate: function (model) {
            var object = JSON.stringify(model.appointmentData);
            referece.invokeMethodAsync("setAppointmentCurrentDisplay", object, model.targetedAppointmentData.startDate, model.targetedAppointmentData.endDate, isStaff);
        },
        appointmentTooltipTemplate: function (model, index, element) {
            element.parents().eq(5).css('display', 'none');
            return element;
        },
        appointmentCollectorTemplate: function (model, element) {
            if (element.parent() == undefined) {
                setTimeout(function () { scheduleStaffMobile(ScheduleDataDictJson, calendarJson, ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, referece, currentLoginUserId, currentDateInput, isStaff, true); }
                    , 500);
                return;
            }
            element.parent().addClass('colector-list-sfmb');
            return element;
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
        onCellClick: function (e) {
            var startDate = e.cellData.startDate;
            var dataDate = JSON.stringify(startDate);
            currentDateSelected = new Date(e.cellData.startDate);
            referece.invokeMethodAsync("showScheduleEachStaff", dataDate);
            e.cancel = true;
        },
        onCellDblClick: function (e) {
            e.cancel = true;
        },
        resources: [
            {
                fieldExpr: "displayType",
                dataSource: appointmentBackgroundColor
            }],
        onContentReady: function (e) {            
            $('#scheduler .dx-scheduler-view-switcher').addClass('display-hide-sfmb');
            $('#scheduler .dx-scheduler-view-switcher-label').addClass('display-hide-sfmb');
            var item = $('.dx-overlay.dx-widget.dx-state-invisible.dx-visibility-change-handler.dx-fa-button');
            if (item != undefined) {
                item.addClass('dx-state-invisible');
                $('.dx-fa-button-icon-close').addClass('dx-state-invisible');
                $('.dx-fa-button-icon').removeClass('dx-state-invisible');
                $(document).ready(
                    $(".dx-fa-button-icon").click(function () {
                        scheduler.showAppointmentPopup(createAppointmentPopupData());
                    })
                );
            }
            if (isUpdateEnd) {
                referece.invokeMethodAsync("refreshSchedule");
                isUpdateEnd = false;
            }
            if (init) {
                init = false;
                var div = $(".dx-scheduler-date-table td.dx-scheduler-date-table-cell.dx-scheduler-cell-sizes-horizontal.dx-scheduler-cell-sizes-vertical");
                if (div != undefined) {
                    var dateShow = new Date(currentDateInput);
                    var day = '' + (dateShow.getDate() + 1);
                    if (day.length < 2)
                        day = '0' + day;
                    displayCurrentDate(day, HightLightCurrentDay(day, currentYearMonth))
                    referece.invokeMethodAsync("stateHaschangeAfterReady");
                }
            }
        },
        onAppointmentFormOpening: function (data) {
            if (data.appointmentData.displayType == 1 || (data.appointmentData.displayType == 2 && data.appointmentData.syainCdSeq == currentLoginUserId) || data.appointmentData.displayType == 3 || data.appointmentData.displayType == undefined) {
                RenderAppointmentSettingForm(data, data.appointmentData.displayType, false, false, true);
            }
            data.form.itemOption("mainGroup.text", {
                validationRules: [
                    {
                        type: "required",
                        message: Lang["NullTitle"]
                    },
                    {
                        type: "stringLength",
                        max: 50,
                        message: Lang["OverText"],
                    }
                ]
            })
        },
        onAppointmentAdding: function (e) {
            isUpdateEnd = true;
            e.appointmentData.scheduleLabel = [];
            e.appointmentData.scheduleLabel = scheduleLabelChecked;
            e.appointmentData.isPublic = isPublic == true ? 1 : 0;
            var object = JSON.stringify(e.appointmentData);
            referece.invokeMethodAsync("SaveAppointment", object);

        },
        onAppointmentUpdating: function (e) {
            isUpdateEnd = true;
            e.newData.scheduleLabel = scheduleLabelChecked;
            e.newData.isPublic = isPublic == true ? 1 : 0;
            var object = JSON.stringify(e.newData);
            referece.invokeMethodAsync("UpdateAppointment", object);
        },
        onAppointmentDeleting: function (e) {
            isUpdateEnd = true;
        },
    }).dxScheduler("instance");

    function RenderAppointmentSettingForm(data, scheduleType, isAbsolute, isHope, isOpenForm, isSendNoti = true) {
        var tempData = data;
        var selectedScheduleType = scheduleType;
        scheduleLabelChecked.length = 0;
        var absoloteCheck = isAbsolute;
        var hopeCheck = isHope;
        if (isOpenForm) {
            if (data.appointmentData.yoteiInfo != undefined && data.appointmentData.yoteiInfo.tukiLabelArray != undefined) {
                data.appointmentData.yoteiInfo.tukiLabelArray.forEach(function (e) {
                    if (e.labelType == 1) {
                        absoloteCheck = true;
                    }
                    else if (e.labelType == 2) {
                        hopeCheck = true;
                    }
                });
            }
        }
        if (absoloteCheck) {
            scheduleLabelChecked.splice(0, 0, 1);
        }
        if (hopeCheck) {
            scheduleLabelChecked.splice(0, 0, 2);
        }
        var formItems = data.form.option("items");
        //formItems[0].items.pop();
        if (formItems[0].items.length < 8) {
            //get the original recurence of scheduler before insert more item in formItems
            recurrence = formItems[0].items[2].items[1];
        }

        while (formItems[0].items[0].dataField != "text") {
            formItems[0].items.splice(0, 1);
        }
        formItems[0].items.splice(0, 0, {
            validationRules: [
                {
                    type: "required",
                    message: Lang["NullScheduleType"]
                }
            ],
            label: {
                text: Lang["ScheduleType"]
            },
            editorType: "dxSelectBox",
            dataField: "displayType",
            editorOptions: {
                items: ScheduleTypeData,
                displayExpr: "text",
                valueExpr: "id",
                onValueChanged: function (args) {
                    RenderAppointmentSettingForm(data, args.value, absoloteCheck, hopeCheck, isOpenForm);
                    selectedScheduleType = args.value;
                }
            }
        });

        if (scheduleType != 1) {
            if (formItems[0].items.length > 8) {
                formItems[0].items.splice(8, formItems[0].items.length - 8);
            }
        }

        if (scheduleType == 1) {
            //is vacation plan
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
            //formItems[0].items[4].items.splice(1, 1, {
            //    itemType: "empty",
            //    colspan: 2
            //});
            if (formItems[0].items.length > 8) {
                if (formItems[0].items[8].editorType != "dxTextArea") {
                    formItems[0].items.splice(8, 4);
                }
                else if (formItems[0].items.length > 12) {
                    formItems[0].items.splice(12, 4);
                }
            }
            if (formItems[0].items.length <= 9 || formItems[0].items[9].editorType != "dxCheckBox") {
                formItems[0].items.push({
                    editorType: "dxCheckBox",
                    label: {
                        text: "",
                        visible: false
                    },
                    dataField: "isSendNoti",
                    editorOptions: {
                        text: Lang["IsSenNoti"],
                        value: isSendNoti,
                        onValueChanged: function (args) {
                            RenderAppointmentSettingForm(data, scheduleType, absoloteCheck, hopeCheck, isOpenForm, args.value);
                        }
                    },
                });
                formItems[0].items.push({
                    itemType: "empty",
                    colspan: 2
                });
                formItems[0].items.push({
                    label: {
                        text: "",
                        visible: false
                    },
                    validationRules: [
                        {
                            type: isSendNoti ? "required" : "",
                            message: Lang["NullSendNoti"]
                        }
                    ],
                    editorType: "dxSelectBox",
                    dataField: "staffIdToSend",
                    editorOptions: {
                        disabled: !isSendNoti,
                        items: StaffData,
                        displayExpr: "syainNm",
                        valueExpr: "syainCdSeq",
                    }
                });
            }
            //formItems.splice(1, 1);
        } else if (scheduleType == 2) {
            //is meeting plan
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
            if (recurrence != undefined) {
                formItems[0].items[4].items.splice(1, 1, recurrence);
            }
            if (formItems[0].items.length > 8) {
                if (formItems[0].items[8].editorType != "dxTextArea") {
                    formItems[0].items.splice(8, 3);
                }
            }
            //if (formItems.length == 1) {
            //    formItems.splice(1, 0, recurrenceForm);
            //}
        } else if (scheduleType == 3) {
            //is tranning plan
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
            if (recurrence != undefined) {
                formItems[0].items[4].items.splice(1, 1, recurrence);
            }
            if (formItems[0].items.length > 8) {
                if (formItems[0].items[8].editorType != "dxTextArea") {
                    formItems[0].items.splice(8, 3);
                }
            }
            //if (formItems.length == 1) {
            //    formItems.splice(1, 0, recurrenceForm);
            //}
        }
        else {
            formItems[0].items.splice(1, 0, {
                itemType: "empty",
                colspan: 2
            });
            if (recurrence != undefined) {
                formItems[0].items[4].items.splice(1, 1, recurrence);
            }
            if (formItems[0].items.length > 8) {
                if (formItems[0].items[8].editorType != "dxTextArea") {
                    formItems[0].items.splice(8, 2);
                }
            }
        }

        formItems[0].items.splice(2, 0, {
            label: {
                text: Lang["WithLable"]
            },
            colspan: 2,
            colCountByScreen: {
                lg: 2,
            },
            itemType: "group",
            items: [{
                editorType: "dxCheckBox",
                editorOptions: {
                    text: ScheduleLabelTypeData[0].text,
                    value: absoloteCheck,
                    onValueChanged: function (data) {
                        if (data.value) {
                            var doc = document.getElementById("check-1");
                            if (doc.classList.contains("opacity-disable")) {
                                EnableOpacity("check-1");
                                absoloteCheck = true;
                                scheduleLabelChecked.splice(0, 0, 1);
                            }
                        }
                        else {
                            var doc = document.getElementById("check-1");
                            if (doc.classList.contains("opacity-enable")) {
                                DisableOpacity("check-1");
                                absoloteCheck = false;
                                var index = scheduleLabelChecked.indexOf(1);
                                scheduleLabelChecked.splice(index, 1);
                            }
                        }
                        RenderAppointmentSettingForm(tempData, selectedScheduleType, absoloteCheck, hopeCheck, false);
                    },
                    elementAttr: {
                        id: "check-1",
                        class: absoloteCheck == true ? "red-label opacity-enable" : "red-label opacity-disable"
                    },
                    width: 60,
                },
            }, {
                editorType: "dxCheckBox",
                editorOptions: {
                    text: ScheduleLabelTypeData[1].text,
                    value: hopeCheck,
                    onValueChanged: function (data) {
                        if (data.value) {
                            var doc = document.getElementById("check-2");
                            if (doc.classList.contains("opacity-disable")) {
                                EnableOpacity("check-2");
                                hopeCheck = true;
                                scheduleLabelChecked.splice(0, 0, 2);
                            }
                        }
                        else {
                            var doc = document.getElementById("check-2");
                            if (doc.classList.contains("opacity-enable")) {
                                DisableOpacity("check-2");
                                hopeCheck = false;
                                var index = scheduleLabelChecked.indexOf(2);
                                scheduleLabelChecked.splice(index, 1);
                            }
                        }
                        RenderAppointmentSettingForm(tempData, selectedScheduleType, absoloteCheck, hopeCheck, false);
                    },
                    elementAttr: {
                        id: "check-2",
                        class: hopeCheck == true ? "blue-label opacity-enable" : "blue-label opacity-disable"
                    },
                    width: 60,
                },
            }],
            cssClass: "checkGroup"
        });
        formItems[0].items.splice(3, 0, {
            itemType: "empty",
            colspan: 2
        });

        if (formItems[0].items[7].editorType == "dxSelectBox" && scheduleType == 1) {
            formItems[0].items.splice(7, 1);
        }
        if (formItems[0].items[7].editorType == "dxCheckBox" && scheduleType != 1) {
            formItems[0].items.splice(7, 0, {
                label: {
                    text: Lang["ItemCalendar"]
                },
                validationRules: [
                    {
                        type: "required",
                        message: Lang["ItemCalendarRequired"]
                    }
                ],
                editorType: "dxSelectBox",
                dataField: "calendarSeq",
                editorOptions: {
                    items: CalendarData,
                    displayExpr: "calendarName",
                    valueExpr: "calendarSeq",
                    value: 0
                }
            });
        }
        if (formItems[0].items[7].editorType != "dxCheckBox" && formItems[0].items[7].editorType != "dxSelectBox") {
            formItems[0].items.splice(7, 0, {
                label: {
                    text: Lang["ItemCalendar"]
                },
                validationRules: [
                    {
                        type: "required",
                        message: Lang["ItemCalendarRequired"]
                    }
                ],
                editorType: "dxSelectBox",
                dataField: "calendarSeq",
                editorOptions: {
                    items: CalendarData,
                    displayExpr: "calendarName",
                    valueExpr: "calendarSeq",
                    value: 0
                }
            });
            formItems[0].items.splice(8, 0, {
                label: {
                    text: Lang["PublishSetting"]
                },
                editorType: "dxCheckBox",
                editorOptions: {
                    text: Lang["PublishOutside"],
                    value: (data.appointmentData.isPublic == 1 || data.appointmentData.isPublic == undefined) ? true : false,
                    onValueChanged: function (data) {
                        if (data.value) {
                            isPublic = true;
                        }
                        else {
                            isPublic = false;
                        }
                    }
                },
            });
        }
        data.form.option("items", formItems);
        data.form.itemOption("mainGroup.text", {
            validationRules: [
                {
                    type: "required",
                    message: Lang["NullTitle"]
                },
                {
                    type: "stringLength",
                    max: 50,
                    message: Lang["OverText"],
                }
            ]
        })
    }
    var createAppointmentPopupData = function () {
        var objAdd = JSON.stringify(true);
        referece.invokeMethodAsync("isAddAppoitment", objAdd);
        if (currentDateSelected == undefined) {
            currentDateSelected = new Date(scheduler.option('currentDate'));
        };
        var cellDuration = scheduler.option('cellDuration');
        var timeZoneForm = scheduler.option('timeZone');
        currentDateSelected.setHours(0, 0, 0, 0);
        var edDate = new Date(currentDateSelected.getTime() + 86400000);
        edDate.setHours(0, 0, 0, 0);
        return {
            startDate: new Date(currentDateSelected),
            endDate: new Date(edDate),
            timeZone: timeZoneForm
        };
    };
    $(document).ready(
        $(".btn-add-schedule").click(function () {
            scheduler.showAppointmentPopup(createAppointmentPopupData());
        }),
        $('#addschedulemobile').click(function () {
            scheduler.showAppointmentPopup(createAppointmentPopupData());
        })
    );
    schedulerDisplay = scheduler;
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

function displayCurrentDate(current, isFirst) {
    var div = $(".dx-scheduler-date-table td.dx-scheduler-date-table-cell.dx-scheduler-cell-sizes-horizontal.dx-scheduler-cell-sizes-vertical"); //
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

function displayCalenda(type) {
    if (type) {
        $('#scheduler').removeClass('display-hide-sfmb');
        $(".staff-schedule-mobile").removeClass('height-0-sfmb');
    }
    else {
        $('#scheduler').addClass('display-hide-sfmb');
        $(".staff-schedule-mobile").addClass('height-0-sfmb');
    }

}

function twoDigit(n) { return (n < 10 ? '0' : '') + n; }

function fadeToggleGroupScheduleMB() {
    var tgTitleSection = $(".sp-layout #groupScheduleTitleSectionSfmb");
    if (tgTitleSection != undefined) {
        if (tgTitleSection.length == 0) {
            setTimeout(function () { fadeToggleGroupScheduleMB(); }, 500);
            return;
        }
        tgTitleSection.each(function () {
            $(this).click(function () {
                var $element = $(this).next();
                var $icon = $(this).find('i:first');
                if ($element.is(':visible')) {
                    $element.slideUp();
                    $icon.removeClass('fa-angle-up').addClass('fa-angle-down');
                } else {
                    setTimeout(function () {
                        $element.slideDown();
                        $icon.removeClass('fa-angle-down').addClass('fa-angle-up');
                    }, 400);

                }
            });

        });
    }
}

function EnableOpacity(divId) {
    var doc = document.getElementById(divId);
    doc.classList.remove("opacity-disable");
    doc.classList.add("opacity-enable");
}

function DisableOpacity(divId) {
    var doc = document.getElementById(divId);
    doc.classList.remove("opacity-enable");
    doc.classList.add("opacity-disable");
}

function showEditStaffSchedule(data, referece) {
    var objAdd = JSON.stringify(false);
    referece.invokeMethodAsync("isAddAppoitment", objAdd);
    var ScheduleItem = ParseToObject(data);
    schedulerDisplay.showAppointmentPopup(ScheduleItem);
    // schedulerDisplay.option("dataSource", []);
}

function deleteStaffSchedule(data) {
    var ScheduleItem = ParseToObject(data);
    schedulerDisplay.deleteAppointment(ScheduleItem);
}

function ReRenderStaffScheduleMobile(dataJS, CalendarDataJS, ScheduleTypeDataJS, VacationTypeDataJS, ScheduleLabelTypeDataJS, StaffDataJS, DisplaySettingDataJS, TimeZoneJS, LangJS, CultureInfoJS, refereceJS, currentLoginUserIdJS, currentDate, isStaffJS, currentYearMonth) {
    if (schedulerDisplay != undefined) {
        $("#scheduler").empty();
        scheduleStaffMobile(dataJS, CalendarDataJS, ScheduleTypeDataJS, VacationTypeDataJS, ScheduleLabelTypeDataJS, StaffDataJS, DisplaySettingDataJS, TimeZoneJS, LangJS, CultureInfoJS, refereceJS, currentLoginUserIdJS, currentDate, isStaffJS, currentYearMonth);
    }
}
function HightLightCurrentDay(currentDate, currentYearMonth) {
    if (schedulerDisplay != undefined) {
        var d = new Date(currentDate);
        if (d.getDate() < 15) {
            return parseInt(formatDate(currentDate)) > parseInt(currentYearMonth) ? false : true;
        }
        else {
            return parseInt(formatDate(currentDate)) === parseInt(currentYearMonth) ? false : true;
        }
    }
}

function formatDate(date) {
    if (schedulerDisplay != undefined) {
        var d = new Date(date);
        var month = '' + (d.getMonth() + 1);
        if (month.length < 2)
            month = '0' + month;
        return ['' + d.getFullYear(), month];
    }
}