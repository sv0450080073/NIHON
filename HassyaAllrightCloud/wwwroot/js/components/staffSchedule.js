var currentSettingData;
var currentTimezone;
var scheduleDataGlobal;
var dateNow;
var isHomePageGlobal;
var CurrentViewGlobal;

function SetFocus() {
    setTimeout(function () {
        $(".focus > div > div > input").focus();
    }, 500);
}

function isSat(date) {
    var day = date.getDay();
    return day === 6;
}
function isSun(date) {
    var day = date.getDay();
    return day === 0;
}
function isInMonth(soucre, targer) {
    return soucre === targer;
}

function SetCurrentDisplayData(data, timezone, scheduleDatas) {
    currentSettingData = ParseToObject(data);
    currentTimezone = timezone;
    scheduleDataGlobal = ParseToObject(scheduleDatas);
}

var scheduleLabelChecked = [];
var isPublic = true;
var recurrence;
var recurrenceForm;
function ResizeScheduleFired() {
    $(window).on("resize", function () {
        repaintSchedule(500);
    });

    $("#content").on('click', '#kobo-menu-btn', function () {
        repaintSchedule();
    });
}

function GenerateAppointmentTemplate(model) {
    var text = model.targetedAppointmentData.text ? model.targetedAppointmentData.text : "";
    var htmlString = "<div class='schedule-label-wrapper'>";
    if (model.targetedAppointmentData.allDay == true || model.targetedAppointmentData.allDayKbn == 1 || model.targetedAppointmentData.displayType == 7 || model.targetedAppointmentData.displayType == 8) {
        htmlString += ("<div style='color: black'>" + "終日" + "</div>");
    }
    else if (model.targetedAppointmentData.displayType != 7 && model.targetedAppointmentData.displayType != 8) {
        if (model.targetedAppointmentData.startDateDisplay.toString().substring(5, 10) == model.targetedAppointmentData.endDateDisplay.toString().substring(5, 10)) {
            htmlString += ("<div style='color: black'>" + model.appointmentData.startDateDisplay.substring(11, 16) + " - " + model.appointmentData.endDateDisplay.substring(11, 16) + "</div>");
        }
        else if (model.targetedAppointmentData.recurrenceRule == "") {
            htmlString += ("<div style='color: black'>" + model.targetedAppointmentData.startDateDisplay.substring(5, 10).replace('-', '/') + "(月)" + model.appointmentData.startDateDisplay.substring(11, 16) + " - " +
                model.targetedAppointmentData.endDateDisplay.substring(5, 10).replace('-', '/') + "(火)" + model.appointmentData.endDateDisplay.substring(11, 16) + "</div>");
        }
        else if (model.targetedAppointmentData.recurrenceRule != "") {
            htmlString += ("<div style='color: black'>" + model.targetedAppointmentData.startDate.substring(5, 10).replace('-', '/') + "(月)" + model.appointmentData.startDateDisplay.substring(11, 16) + " - " +
                model.targetedAppointmentData.endDate.substring(5, 10).replace('-', '/') + "(火)" + model.appointmentData.endDateDisplay.substring(11, 16) + "</div>");
        }
    }
    htmlString += "<div class='schedule-label'>";
    if (model.targetedAppointmentData.displayType == 91) {
        if (model.targetedAppointmentData.yoteiInfo.yoteiTypeKbn == 1) {
            htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-leave'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
        }
        if (model.targetedAppointmentData.yoteiInfo.yoteiTypeKbn == 2) {
            htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-meeting'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
        }
        if (model.targetedAppointmentData.yoteiInfo.yoteiTypeKbn == 3) {
            htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-training'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
        }
    }
    if (model.targetedAppointmentData.displayType == 1) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-leave'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.displayType == 2) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-meeting'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.displayType == 3) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-training'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.displayType == 4) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-work'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.displayType == 5) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-leave-approved'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.displayType == 6) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-riding'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.displayType == 7) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-birthday'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.displayType == 8) {
        htmlString += ("<div class='mr-1 mt-025 schedule-label-comments'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
    }
    if (model.targetedAppointmentData.yoteiInfo != undefined && model.targetedAppointmentData.yoteiInfo.tukiLabelArray.length > 0) {
        model.targetedAppointmentData.yoteiInfo.tukiLabelArray.forEach(function (label) {
            htmlString += ("<div class='mr-1 mt-025 schedule-label-confirm-status-" + label.labelType + "'>" + label.labelText + "</div>");
        });
    }
    htmlString += ("<div  class='schedule-content-inline'>" + text + "</div>");

    htmlString += "</div></div>";
    return htmlString;
}

function renderDriverSchedulePersonal(IsHomePage, calendarJson, ScheduleDataJson, ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, reference, currentLoginUserId, currentDate = new Date()) {
    dateNow = new Date();
    isHomePageGlobal = IsHomePage;
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
        "dxDateBox-validation-datetime": "値は日付または時刻である必要があります",
        "validation-range": "値が範囲外です",
    }

    var CalendarData = ParseToObject(calendarJson);
    var ScheduleData = ParseToObject(ScheduleDataJson);
    var ScheduleTypeData = ParseToObject(ScheduleTypesDataJson);
    var VacationTypeData = ParseToObject(VacationTypesDataJson);
    var ScheduleLabelTypeData = ParseToObject(ScheduleLabelTypeDataJson);
    var StaffData = ParseToObject(StaffDataJson);
    var DisplaySettingData = ParseToObject(DisplaySettingDataJson);
    DevExpress.localization.loadMessages(Dictionnary);
    var StaffDataForMeeting = StaffData.filter(x => !(x.syainCdSeq == currentLoginUserId));
    var init = true;
    var scheduler = $("#driver-schedule").dxScheduler({
        dataSource: ScheduleData,
        views: [
            {
                type: "month",
                name: Lang["Month"],
                maxAppointmentsPerCell: 2
            },
            {
                type: "week",
                name: Lang["Week"],
                maxAppointmentsPerCell: 1
            }],
        timeZone: TimeZone,
        firstDayOfWeek: DisplaySettingData.weekStartDay.value == 1 ? 6 : DisplaySettingData.weekStartDay.value == 2 ? 0 : 1,
        currentView: (CurrentViewGlobal == null || CurrentViewGlobal == undefined) ? DisplaySettingData.defaultDisplayType.value == 1 ? "week" : "month" : CurrentViewGlobal,
        //startDayHour: DisplaySettingData.dayStartTime.value,
        currentDate: currentDate,
        showAllDayPanel: true,
        height: 790,
        editing: true,
        appointmentTemplate: function (model) {
            return $(GenerateAppointmentTemplate(model));
        },
        onCellContextMenu: function (e) {
            updateContextMenu(false, cellContextMenuItems, ".dx-scheduler-date-table-cell", 'item', onItemClick(e));
        },
        dataCellTemplate: function (itemData, itemIndex, itemElement) {
            var currentView = (CurrentViewGlobal == null || CurrentViewGlobal == undefined) ? DisplaySettingData.defaultDisplayType.value == 1 ? "week" : "month" : CurrentViewGlobal;
            var date = itemData.startDate.getMonth().toString();
            var month = dateNow.getMonth().toString();
            var element = $('<div>' + itemData.text + '</div>');
            
            if (isSat(itemData.startDate)) {
                itemElement.addClass("isSat");
            }
            if (isSun(itemData.startDate)) {
                itemElement.addClass("isSun");
            }
            if (currentView == "month") {
                if (!isInMonth(month, date)) {
                    itemElement.addClass("isNotInMonth");
                }
            }
            return itemElement.append(element);

        },
        dateCellTemplate: function (itemData, itemIndex, itemElement) {
            var date = itemData.date;
            var isSatd = isSat(date);
            var isSund = isSun(date);
            var element = $("<div style='height: inherit;padding-top:10px'>" + itemData.text + "</div>");

            if (isSatd) {
                element.addClass('isSat');
            }
            if (isSund) {
                element.addClass('isSun');
            }
            return itemElement.append(element);
        },
        onOptionChanged: function (e) {
            if ((e.name == "currentView" || e.fullName == "currentView") && e.value == Lang["Month"] || e.value == Lang["Week"]) {
                if (e.value == "月") {
                    CurrentViewGlobal = "month";
                }
                else {
                    CurrentViewGlobal = "week";
                }
            }
            if ((e.fullName == "currentDate" || e.name == "currentDate") && !Array.isArray(e.value) && Date.parse(e.value) != "NaN" && (e.value.toString().includes("Mon") || e.value.toString().includes("Tue") || e.value.toString().includes("Wed") || e.value.toString().includes("Thu") || e.value.toString().includes("Fri") || e.value.toString().includes("Sat") || e.value.toString().includes("Sun"))) {
                var Dates = e.value.toString();
                dateNow = e.value;
                scheduler.option("dataSource", null);
                reference.invokeMethodAsync("GetRangeSheduleDataPersonal", Dates).then(data => {
                    scheduler.option("dataSource", ParseToObject(data[0]));
                    //e.component._initialized = false;
                    //$("#driver-schedule").empty();
                    //renderDriverSchedulePersonal(IsHomePage, calendarJson, data[0], ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, reference, currentLoginUserId, e.value, CurrentViewGlobal == null ? DisplaySettingData.defaultDisplayType.value == 1 ? "week" : "month" : CurrentViewGlobal)
                });
            }
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

            var $navigator = e.element.find(".dx-scheduler-navigator");
            var $button = e.element.find(".button-today");
            if ($button.length == 0) {
                var $button = $("<div class='button-today'>")
                    .dxButton({
                        stylingMode: "contained",
                        text: Lang["Today"],
                        type: "normal",
                        width: 66,
                        onClick: function () {
                            var Dates = new Date().toString();
                            dateNow = new Date();
                            reference.invokeMethodAsync("GetRangeSheduleDataPersonal", Dates).then(data => {
                                //scheduler.option("dataSource", ParseToObject(data[0]));
                                //scheduler.option("currentDate", new Date());
                                e.component._initialized = false;
                                $("#driver-schedule").empty();
                                renderDriverSchedulePersonal(IsHomePage, calendarJson, data[0], ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, reference, currentLoginUserId, e.value, CurrentViewGlobal == null ? DisplaySettingData.defaultDisplayType.value == 1 ? "week" : "month" : CurrentViewGlobal)
                            });
                            //renderDriverSchedulePersonal(IsHomePage, calendarJson, ScheduleDataJson, ScheduleTypesDataJson, VacationTypesDataJson, ScheduleLabelTypeDataJson, StaffDataJson, DisplaySettingDataJson, TimeZone, Lang, CultureInfo, reference, currentLoginUserId, currentDate = new Date(), e.component._currentView.type);
                        }
                    });
                $navigator.append($button);
                $(".dx-scheduler-navigator-previous").css("margin-left", "80px");
                $(".button-today").css("margin-left", "-340px");
            }
            e.element.find(".dx-scheduler-date-table-cell").height(113); 
        },
        onAppointmentFormOpening: function (data) {
            if (data.appointmentData.displayType == 1 && data.appointmentData.yoteiInfo.yoteiShoKbn == 3) {
                var object = JSON.stringify(data.appointmentData);
                reference.invokeMethodAsync("ShowRefuseStatusPopup", object);
                data.cancel = true;
            }
            else if (data.appointmentData.displayType == 1 || (data.appointmentData.displayType == 2 && data.appointmentData.syainCdSeq == currentLoginUserId) || data.appointmentData.displayType == 3 || data.appointmentData.displayType == undefined) {
                //show edit form
                var element = document.getElementById("driver-schedule");
                if (element != null) {
                    var instance = DevExpress.ui.dxScheduler.getInstance(element);
                    var view = instance._currentView.type;
                    if (view === "month" || view === Lang["Month"]) {
                        //data.form.updateData("allDay", true);
                        //data.form.itemOption("endDate", {
                        //    editorOptions: {
                        //        value: data.appointmentData.startDate
                        //    }
                        //});
                        //data.form.itemOption("startDate", {
                        //    editorOptions: {
                        //        value: data.appointmentData.startDate
                        //    }
                        //});
                    }
                }
                
                RenderAppointmentSettingForm(data, data.appointmentData.displayType, false, false, true);
            }
            else if (data.appointmentData.displayType == 4 || data.appointmentData.displayType == 5) {
                var object = JSON.stringify(data.appointmentData);
                reference.invokeMethodAsync("ShowWorkingStatusPopup", object);
                data.cancel = true;
            }
            else if (data.appointmentData.displayType == 6) {
                //show haiin popup
                var object = JSON.stringify(data.appointmentData)
                reference.invokeMethodAsync("ShowJourneyPopup", object);
                data.cancel = true;
            }
            else if (data.appointmentData.displayType == 7) {
                data.cancel = true;
            }
            else if (data.appointmentData.displayType == 8) {
                //show calend popup
                reference.invokeMethodAsync("UpdateDateComment", data.appointmentData.dateCommentInfo.calenYmd, data.appointmentData.dateCommentInfo.calenCom);
                data.cancel = true;
            }
            else if (data.appointmentData.displayType == 91) {
                //show company popup
                data.cancel = true;
            }
            else if (data.appointmentData.displayType == 2 && data.appointmentData.syainCdSeq != currentLoginUserId) {
                var object = JSON.stringify(data.appointmentData)
                reference.invokeMethodAsync("ShowFeedbackBookingSchedule", object, true);
                data.cancel = true;
            }
            
        },
        onAppointmentClick: function (e) {
            e.cancel = true;
        },
        onAppointmentDblClick: function (e) {
            if (e.appointmentData.displayType == 7) {
                e.cancel = true;
            }
        },
        onAppointmentUpdating: function (e) {
            e.newData.scheduleLabel = scheduleLabelChecked;
            e.newData.isPublic = isPublic == true ? 1 : 0;
            e.newData.scheduleId = e.newData.yoteiInfo.yoteiSeq;
            var object = JSON.stringify(e.newData);
            reference.invokeMethodAsync("UpdateAppointment", object);
        },
        onAppointmentAdding: function (e) {
            e.appointmentData.scheduleLabel = [];
            e.appointmentData.scheduleLabel = scheduleLabelChecked;
            e.appointmentData.isPublic = isPublic == true ? 1 : 0;
            var object = JSON.stringify(e.appointmentData);
            reference.invokeMethodAsync("SaveAppointment", object);
        },
        onAppointmentDeleting: function (e) {
            if (e.appointmentData.displayType == 1 || e.appointmentData.displayType == 2 || e.appointmentData.displayType == 3) {
                var object = JSON.stringify(e.appointmentData);
                reference.invokeMethodAsync("DeleteAppointment", object);
            }
            e.cancel = true;
        },
        onAppointmentRendered: function (e) {
            //e.appointmentElement.height(42); 
            e.appointmentElement[0].style.backgroundColor = e.appointmentData.color;
        },
        customizeDateNavigatorText: function (e) {
            var element = document.getElementById("driver-schedule");
            if (element != null) {
                var instance = DevExpress.ui.dxScheduler.getInstance(element);
                var view = instance._currentView.type;
                if (view === "month" || view === Lang["Month"]) {
                    return e.startDate.toLocaleString("ja", { year: 'numeric', month: 'long' });
                }
                else {
                    return e.startDate.toLocaleString("ja", { year: 'numeric', month: 'long', day: 'numeric' }) + "-" + e.endDate.toLocaleString("ja", { month: 'long', day: 'numeric' });
                }
            }
        },
        resources: [
            {
                fieldExpr: "displayType",
                dataSource: appointmentBackgroundColor
            }],
        appointmentCollectorTemplate: function (data, element) {
            if (data.isCompact) {
                element[0].innerText = data.appointmentCount;
            } else {
                element[0].innerText = "他" + data.appointmentCount;
            }
            element.addClass("dx-scheduler-appointment-collector-content");
        },
        editing: {
            allowResizing: false,
            allowDragging: false
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
                validationRules: [
                    {
                        type: "required",
                        message: Lang["NullVacationType"]
                    }
                ],
                editorType: "dxSelectBox",
                dataField: "vacationType",
                editorOptions: {
                    items: VacationTypeData,
                    displayExpr: "text",
                    valueExpr: "id",
                }
            });
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
                        text: "承認者に通知",
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
                    items: StaffDataForMeeting,
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
        } else if (scheduleType == 3) {
            //is tranning plan
            formItems[0].items.splice(1, 0, {
                label: {
                    text: Lang["Participant"]
                },
                editorType: "dxTagBox",
                dataField: "staffs",
                editorOptions: {
                    items: StaffDataForMeeting,
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
                    text: "カレンダー"
                },
                validationRules: [
                    {
                        type: "required",
                        message: Lang["ScheduleTypeRequire"]
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
                    text: "カレンダー"
                },
                validationRules: [
                    {
                        type: "required",
                        message: Lang["ScheduleTypeRequire"]
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

        if (data.appointmentData.displayType != undefined) {
            const popup = data.element.find(".dx-scheduler-appointment-popup").dxPopup("instance");
            const toolbarItems = popup.option("toolbarItems");
            const deleteButton = {
                widget: "dxButton",
                location: "before",
                toolbar: "bottom",
                options: {
                    text: "削除",
                    onClick: function () {
                        scheduler.deleteAppointment(data.appointmentData);
                    }
                }
            };
            if (toolbarItems.length = 2) {
                toolbarItems.push(deleteButton);
            }
            popup.option("toolbarItems", toolbarItems);
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
        data.form.itemOption("mainGroup.startDate", {
            validationRules: [
                {
                    type: "required",
                    message: Lang["NullStartDate"]
                }
            ]
        })
        data.form.itemOption("mainGroup.endDate", {
            validationRules: [
                {
                    type: "required",
                    message: Lang["NullEndDate"]
                }
            ]
        })
        if (data.appointmentData.recurrenceRule == "" || data.appointmentData.recurrenceRule == undefined) {
            data.form.itemOption("mainGroup", {
                colSpan: 2,
            })
            data.form.itemOption("recurrenceGroup", {
                visible: false,
            })
        }
        else {
            data.form.itemOption("mainGroup", {
                colSpan: 1,
            })
            data.form.itemOption("recurrenceGroup", {
                visible: true,
            })
        }
    }

    var contextMenuInstance = $("#context-menu").dxContextMenu({
        width: 200,
        dataSource: [],
        disabled: true,
        onHiding: function () {
            updateContextMenu(true, []);
        }
    }).dxContextMenu("instance");

    var updateContextMenu = function (disable, dataSource, target, itemTemplate, onItemClick) {
        contextMenuInstance.option({
            dataSource: dataSource,
            target: target,
            itemTemplate: itemTemplate,
            onItemClick: onItemClick,
            disabled: disable,
        });
    }

    var onItemClick = function (contextMenuEvent) {
        return function (e) {
            e.itemData.onItemClick(contextMenuEvent, e);
        }
    }
    var createAppointment = function (e) {
        e.component.showAppointmentPopup({
            startDate: e.cellData.startDate,
            endDate: e.cellData.endDate
        }, true);
    };
    var createDateComment = function (e) {
        var date = e.cellData.startDate.toString();
        reference.invokeMethodAsync("UpdateDateComment", date, "");
    };
    var cellContextMenuItems = [
        { text: '予約登録', onItemClick: createAppointment },
        { text: '日付コメント', onItemClick: createDateComment },
    ];

    window.addEventListener('resize', function () {
        repaintSchedule();
    });
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

function repaintScheduleGroup(delay = 250, isRepaint = true) {
    if (isRepaint) {
        setTimeout(function () {
            adjustHeightForScheduleWrapper();
            var element = document.getElementById("driver-schedule-group");
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
    if (isHomePageGlobal) {
        $(".driver-schedule-wrapper").height("calc(100% - " + adJustHeight + "px)");
    }
    else {
        $(".driver-schedule-wrapper").height("calc(100% - " + 40 + "px)");
    }
}

function fadeToggleExceptButton() {
    $("#content").on('click', '.group-schedule-title-section', function (e) {
        if (e.target.localName != 'button' && e.target.parentElement.localName != 'button') {
            $(this).find('i:first').toggleClass('fa-angle-up').toggleClass('fa-angle-down');
            $(this).next().slideToggle();
        }
    });
}

var appointmentBackgroundColor = [
    {
        id: 1,
        color: "#fddfdd"
    }, {

        id: 2,
        color: "#f6dff0"
    }, {

        id: 3,
        color: "#dbf0ee"
    }, {

        id: 4,
        color: "#e1e9dd"
    }, {

        id: 5,
        color: "#fddfdd"
    }, {

        id: 6,
        color: "#feeddb"
    }, {

        id: 7,
        color: "#feedf8"
    }, {

        id: 8,
        color: "#fff8dd"
    }, {

        id: 90,
        color: "#d9d9d9"
    }, {

        id: 91,
        color: "#cbfafa"
    }
];

function getDateOfWeekInJapanesegr(model) {
    return model.date.toString().substring(8, 10) + "(" + model.text[0] + ")";
}

function renderDriverScheduleForGroup(calendarData, staffDatas, scheduleDatas, reference, TimeZone, currentView, Lang, currentLoginUserId, ScheduleTypesDataJson ,StaffDataJson, ScheduleLabelTypeDataJson, VacationTypesDataJson, currentDate = new Date()) {
    var CalendarData = ParseToObject(calendarData)
    var StaffData = ParseToObject(staffDatas);
    var ScheduleData = ParseToObject(scheduleDatas);
    var ScheduleTypeData = ParseToObject(ScheduleTypesDataJson);
    var VacationTypeData = ParseToObject(VacationTypesDataJson);
    var ScheduleLabelTypeData = ParseToObject(ScheduleLabelTypeDataJson);
    var StaffDataForAppointment = ParseToObject(StaffDataJson);
    var schedulerGroup = $("#driver-schedule-group").dxScheduler({
        dataSource: ScheduleData,
        views: [{
            type: "timelineWeek",
            groupOrientation: "vertical",
            name: Lang["weekviewtext"],
            cellDuration: 1440,
            maxAppointmentsPerCell: 2
        }, {
            type: "timelineDay",
            groupOrientation: "vertical",
            name: Lang["dayviewtext"],
            cellDuration: 60,
            maxAppointmentsPerCell: 2
        }],
        timeZone: TimeZone,
        currentView: currentView,
        currentDate: currentDate,
        firstDayOfWeek: currentSettingData.weekStartDay.value,
        startDayHour: 0,
        height: (StaffData.length * 113) + 100,
        showAllDayPanel: true,
        editing: {
            allowAdding: false,
            allowDragging: false
        },
        crossScrollingEnabled: true,
        groups: ["kankSya"],
        resources: [{
            fieldExpr: "kankSya",
            dataSource: StaffData,
            useColorAsDefault: false
        }, {
            fieldExpr: "displayType",
            dataSource: appointmentBackgroundColor,
            useColorAsDefault: true
        }],
        appointmentTemplate: function (model) {
            return $(GenerateAppointmentTemplate(model));
        },
        timeCellTemplate: function (itemData, itemIndex, itemElement) {
            var element = document.getElementById("driver-schedule-group");
            if (element != null) {
                var instance = DevExpress.ui.dxScheduler.getInstance(element);
                var view = instance._currentView.type;
                var element = $(`<div>${itemData.text}</div>`);
                if (view === "timelineWeek" || view === Lang["weekviewtext"]) {
                    element.addClass('disable-time');
                    return itemElement.append(element);
                }
                element.addClass('enable-time');
                return itemElement.append(element);
            }
        },
        onAppointmentFormOpening: function (data) {
            if (data.appointmentData.syainCdSeq == undefined) {
                data.cancel = true;
            }
            else if (data.appointmentData.syainCdSeq == currentLoginUserId && (data.appointmentData.displayType == 1 || data.appointmentData.displayType == 2 || data.appointmentData.displayType == 3)) {
                data.form.updateData("endDate", data.appointmentData.endDateDisplay);
                data.form.updateData("startDate", data.appointmentData.startDateDisplay);

                RenderAppointmentSettingForm(CalendarData, data, data.appointmentData.displayType, false, false, true);
            }
            else if (data.appointmentData.displayType != 90) {
                if (data.appointmentData.displayType == 4 || data.appointmentData.displayType == 5) {
                    var object = JSON.stringify(data.appointmentData);
                    reference.invokeMethodAsync("ShowWorkingStatusPopup", object);
                    data.cancel = true;
                }
                else if (data.appointmentData.displayType == 6) {
                    var object = JSON.stringify(data.appointmentData);
                    reference.invokeMethodAsync("ShowJourneyPopup", object);
                    data.cancel = true;
                }
                else if (data.appointmentData.displayType == 1) {
                    var object = JSON.stringify(data.appointmentData);
                    reference.invokeMethodAsync("ShowAppointmentPopup", object);
                    data.cancel = true;
                }
                else if (data.appointmentData.displayType == 2 || data.appointmentData.displayType == 3) {
                    data.appointmentData.kankSya = data.appointmentData.staffs;
                    var object = JSON.stringify(data.appointmentData);
                    reference.invokeMethodAsync("ShowFeedbackBookingSchedule", object, false);
                    data.cancel = true;
                }
            }
        },
        dateCellTemplate: function (model) {
            return getDateOfWeekInJapanesegr(model);
        },
        onAppointmentClick: function (e) {
            e.cancel = true;
        },
        appointmentCollectorTemplate: function (data, $indicatorElement) {
            $indicatorElement.append(
                "<div class='custom-collector'>" + "他" + data.appointmentCount + "</div>"
            )
            return
        },
        appointmentTooltipTemplate: function (model) {
            var text = model.targetedAppointmentData.text ? model.targetedAppointmentData.text : "";
            var htmlString = ("<div style='text-align: left; height:50px'>");

            if (model.targetedAppointmentData.allDay == true || model.targetedAppointmentData.displayType == 7 || model.targetedAppointmentData.displayType == 8) {
                htmlString += ("<div style='color: black'>" + "終日" + "</div>");
            }
            else if (model.targetedAppointmentData.displayType != 7 && model.targetedAppointmentData.displayType != 8) {
                htmlString += ("<div style='color: black'>" + model.targetedAppointmentData.startDate.toString().substring(5, 10).replace('-', '/') + "(月)" + model.appointmentData.startDateDisplay.toString().substring(11, 16) + " - " + model.targetedAppointmentData.endDate.toString().substring(5, 10).replace('-', '/') + "(火)" + model.appointmentData.endDateDisplay.toString().substring(11, 16) + "</div>");
            }

            htmlString += "<div class='schedule-label'>";
            if (model.targetedAppointmentData.displayType == 1) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-leave' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.displayType == 2) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-meeting' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.displayType == 3) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-plan-training' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.displayType == 4) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-work' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.displayType == 5) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-leave-approved' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.displayType == 6) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-riding' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.displayType == 7) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-birthday' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.displayType == 8) {
                htmlString += ("<div class='mr-1 mt-025 schedule-label-comments' style='color: white'>" + model.targetedAppointmentData.typelabel.labelText + "</div>");
            }
            if (model.targetedAppointmentData.yoteiInfo != undefined && model.targetedAppointmentData.yoteiInfo.tukiLabelArray.Count > 0) {
                for (var label in model.targetedAppointmentData.yoteiInfo.tukiLabelArray) {
                    htmlString += ("<div style='color: white' class='mr-1 mt-025 schedule-label-confirm-status-" + label.labelType + "'>" + label.labelText + "</div>");
                }
            }
            htmlString += ("<div  class='schedule-content-inline'>" + text + "</div>");

            htmlString += "</div>";

            htmlString += ("</div>");
            
            return htmlString;
        },
        onOptionChanged: function (e) {
            if (e.name == "currentDate" && !Array.isArray(e.value)) {
                schedulerGroup.option("dataSource", null);
                $("#driver-schedule-group").dxScheduler("instance").repaint();
                var startDate = $("#driver-schedule-group").dxScheduler("instance").getStartViewDate().toString();
                var endDate = $("#driver-schedule-group").dxScheduler("instance").getEndViewDate().toString();
                reference.invokeMethodAsync("GetRangeSheduleDataGroup", startDate, endDate).then(data => {
                    schedulerGroup.option("dataSource", ParseToObject(data[1]));
                    //e.component._initialized = false;
                    //$("#driver-schedule-group").empty();
                    //renderDriverScheduleForGroup(calendarData,data[0], data[1], reference, currentTimezone, e.component._currentView.type, Lang, currentLoginUserId, ScheduleTypesDataJson, StaffDataJson, ScheduleLabelTypeDataJson, VacationTypesDataJson, e.value);
                });
            }
        },
        onContentReady: function (e) {
            var currentHour = new Date().getHours();
            var minute = 00;
            if (currentSettingData.dayStartTime.value != "Now") {
                currentHour = parseInt(currentSettingData.dayStartTime.value);
            }
            if (currentHour < 0) {
                minute = 0;
            }
            e.component.scrollToTime(currentHour, minute, new Date());

            var $navigator = e.element.find(".dx-scheduler-navigator");
            var $button = e.element.find(".button-today");
            if ($button.length == 0) {
                var $button = $("<div class='button-today'>")
                    .dxButton({
                        stylingMode: "contained",
                        text: Lang["Today"],
                        type: "normal",
                        width: 66,
                        onClick: function () {
                            var endDate = new Date();
                            endDate.setDate(endDate.getDate() + 2);
                            var startDate = new Date();
                            startDate.setDate(startDate.getDate() - 4);
                            reference.invokeMethodAsync("GetRangeSheduleDataGroup", startDate.toString(), endDate.toString()).then(data => {
                                e.component._initialized = false;
                                $("#driver-schedule-group").empty();
                                renderDriverScheduleForGroup(calendarData, data[0], data[1], reference, currentTimezone, e.component._currentView.type, Lang, currentLoginUserId, ScheduleTypesDataJson, StaffDataJson, ScheduleLabelTypeDataJson, VacationTypesDataJson, new Date());
                            });
                        }
                    });
                $navigator.append($button);
                $(".dx-scheduler-navigator-previous").css("margin-left", "80px");
                $(".button-today").css("margin-left", "-340px");
            }
        },
        resourceCellTemplate: function (data, index, element) {
            var datas = data.text.split(" ");
            var elementScheduler = document.getElementById("driver-schedule-group");
            if (elementScheduler != null) {
                var instance = DevExpress.ui.dxScheduler.getInstance(elementScheduler);
                var view = instance._currentView.type;
                if (view === "timelineWeek" || view === Lang["weekviewtext"]) {
                    element.append("<div style='float: left' class='col-12'>" + "<div id='staff-info' class='row'>" + datas[0] + "</div>" + "<div style='font-size: smaller' class='row'>" +
                        datas[1] + "</div>" + "<div id='staff-info' class='row'>" +
                        Lang["workhour"] + "</div>" + "<div id='staff-info' class='row'>" + Lang["1weekwk"] + ": " +
                        datas[2] + "</div>" + "<div id='staff-info' class='row'>" + Lang["4weekwk"] + ": " + datas[3] + "</div>");
                }
                else {
                    element.append("<div id='day-staff-info' style='float: left' class='col-12'>" + "<div id='staff-info' class='row mb-2'>" +
                        datas[0] + "</div>" + "<div class='row mb-2'>" + datas[1] + "</div>" + "</div>");
                }
            }
        },
        customizeDateNavigatorText: function (e) {
            var element = document.getElementById("driver-schedule-group");
            if (element != null) {
                var instance = DevExpress.ui.dxScheduler.getInstance(element);
                var view = instance._currentView.type;
                if (view === "timelineWeek" || view === Lang["weekviewtext"]) {
                    return e.startDate.toLocaleString("ja", { year: 'numeric', month: 'long', day: 'numeric' }) + "-" + e.endDate.toLocaleString("ja", { month: 'long', day: 'numeric' });
                }
                else {
                    return e.startDate.toLocaleString("ja", { year: 'numeric', month: 'long', day: 'numeric' });
                }
            }
        },
        onAppointmentDeleting: function (e) {
            if (e.appointmentData.displayType == 1 || e.appointmentData.displayType == 2 || e.appointmentData.displayType == 3) {
                var object = JSON.stringify(e.appointmentData);
                reference.invokeMethodAsync("DeleteAppointment", object);
            }
            e.cancel = true;
        },
        onAppointmentUpdating: function (e) {
            e.newData.scheduleLabel = scheduleLabelChecked;
            e.newData.isPublic = isPublic == true ? 1 : 0;
            e.newData.scheduleId = e.newData.yoteiInfo.yoteiSeq;
            var object = JSON.stringify(e.newData);
            reference.invokeMethodAsync("UpdateAppointment", object);
        },
        appointmentCollectorTemplate: function (data, element) {
            if (data.isCompact) {
                element[0].innerText = data.appointmentCount;
            } else {
                element[0].innerText = "他" + data.appointmentCount;
            }
            element.addClass("dx-scheduler-appointment-collector-content");
        },
        editing: {
            allowResizing: false,
            allowDragging: false
        },
    }).dxScheduler("instance");

    function RenderAppointmentSettingForm(CalendarData, data, scheduleType, isAbsolute, isHope, isOpenForm, isSendNoti = true) {
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
                    message: "スケジュールタイプが必要です"
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
                validationRules: [
                    {
                        type: "required",
                        message: "休暇タイプが必要です"
                    }
                ],
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
                        text: "承認者に通知",
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
                            message: "承認者が必要です"
                        }
                    ],
                    editorType: "dxSelectBox",
                    dataField: "staffIdToSend",
                    editorOptions: {
                        disabled: !isSendNoti,
                        items: StaffDataForAppointment,
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
                    items: StaffDataForAppointment,
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
                    items: StaffDataForAppointment,
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
        if (formItems[0].items[7].editorType != "dxCheckBox" && formItems[0].items[7].editorType != "dxSelectBox") {
            formItems[0].items.splice(7, 0, {
                label: {
                    text: "カレンダー"
                },
                editorType: "dxSelectBox",
                dataField: "calendarSeq",
                editorOptions: {
                    items: CalendarData,
                    displayExpr: "calendarName",
                    valueExpr: "calendarSeq",
                    onValueChanged: function (args) {
                        RenderAppointmentSettingForm(data, args.value, absoloteCheck, hopeCheck, isOpenForm);
                        selectedScheduleType = args.value;
                    }
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

        if (data.appointmentData.displayType != undefined) {
            const popup = data.element.find(".dx-scheduler-appointment-popup").dxPopup("instance");
            const toolbarItems = popup.option("toolbarItems");
            const deleteButton = {
                widget: "dxButton",
                location: "before",
                toolbar: "bottom",
                options: {
                    text: "削除",
                    onClick: function () {
                        schedulerGroup.deleteAppointment(data.appointmentData);
                    }
                }
            };
            if (toolbarItems.length = 2) {
                toolbarItems.push(deleteButton);
            }
            popup.option("toolbarItems", toolbarItems);
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
        data.form.itemOption("mainGroup.startDate", {
            validationRules: [
                {
                    type: "required",
                    message: Lang["NullStartDate"]
                }
            ]
        })
        data.form.itemOption("mainGroup.endDate", {
            validationRules: [
                {
                    type: "required",
                    message: Lang["NullEndDate"]
                }
            ]
        })
    }
}
