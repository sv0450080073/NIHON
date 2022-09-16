function setEventforInputRateField() {
    // just allow input number
    $(".rate :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8 || x == 46)) {
            let index = $(this).val().indexOf('.');
            let lengthDec = 0;
            let lengthFirst = 0;
            let lengthStr = $(this).val().length;
            if ((lengthStr == 4 && this.selectionStart != this.selectionEnd)) {
                return e;
            }
            if ((lengthStr == 2 && this.selectionStart != this.selectionEnd)) {
                return e;
            }
            if (index >= 0) {
                lengthFirst = $(this).val().split('.')[0].length
                lengthDec = $(this).val().split('.')[1].length;
            } else if (index == -1 && lengthStr >= 2 && x != 46) {
                e.preventDefault();
                return false;
            }
            if ((x == 46 && index != -1)  // already contain '.' but input more '.'
               || lengthStr > 3 // xx.x 
               || (lengthDec >= 1 && this.selectionStart > index) // xx._x
               || (lengthFirst >= 2 && this.selectionStart < index)) // xx_.x or x_x.x or _xx.x
            {
               return false;
            }
            return true;
        }
        else {
            e.preventDefault();
            return false;
        }
    });
    $(".rate100 :input").keypress(function (e) {
        var x = e.which || e.keycode;
        if (((x >= 48 && x <= 57) || x == 8 || x == 46)) {
            let index = $(this).val().indexOf('.');
            let lengthDec = 0;
            let lengthFirst = 0;
            let lengthStr = $(this).val().length;
            if($(this).val() == 10 && x == 48){
                return e;
            }
            if ((lengthStr == 4 && this.selectionStart != this.selectionEnd)) {
                return e;
            }
            if ((lengthStr == 3 && this.selectionStart != this.selectionEnd)) {
                return e;
            }
            if ((lengthStr == 2 && this.selectionStart != this.selectionEnd)) {
                return e;
            }
            if (index >= 0) {
                lengthFirst = $(this).val().split('.')[0].length
                lengthDec = $(this).val().split('.')[1].length;
            } else if (index == -1 && lengthStr >= 2 && x != 46) {
                e.preventDefault();
                return false;
            }
            if ((x == 46 && index != -1)  // already contain '.' but input more '.'
               || lengthStr > 3 // xx.x 
               || (lengthDec >= 1 && this.selectionStart > index) // xx._x
               || (lengthFirst >= 2 && this.selectionStart < index)) // xx_.x or x_x.x or _xx.x
            {
               return false;
            }
            return true;
        }
        else {
            e.preventDefault();
            return false;
        }
    });
    ////disable drop text
    //$(".rate :input").bind("paste", function (e) {
    //    var clipboarddata = window.event.clipboardData.getData('text');
    //    if (isNumber(clipboarddata) == false)
    //        e.preventDefault();
    //});
    ////disable drop text
    //$(".rate :input").bind("drop", function (e) {
    //    var data = e.originalEvent.dataTransfer.getData("text");
    //    if (isNumber(data) == false)
    //        e.preventDefault();
    //});
    //// prevent jap
    //$(".rate :input, .number-int :input").blur(function (e) {
    //    if (isNumber(this.value) == false) {
    //        this.value = 0;
    //    }
    //    this.value -= 0;
    //});
}