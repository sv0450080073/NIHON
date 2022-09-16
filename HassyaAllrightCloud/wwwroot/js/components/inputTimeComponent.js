function setEventForTimeInputField() {
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
        this.value = formatCustomTimeWithComma(this.value);
        if ((this.value == "") && ("--:--" == this.preValue)) {
            this.value = "--:--";
        }
        else if ((this.value == "") && ("00:00" == this.preValue)) {
            this.value = "00:00";
        }
    });
    $(".customTime :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8) && ($(this).val().length < 4 || ($(this).val().length = 4 && this.selectionStart != this.selectionEnd)))
            return true;
        else {
            e.preventDefault();
            return false;
        }
    });
}

function formatCustomTimeWithComma(x) {
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