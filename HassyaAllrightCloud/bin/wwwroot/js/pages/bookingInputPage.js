function bookingInputPageTabKey() {
    $('#bookingForm input:not(#bookinginputtab input), .btnbookingform, .custom-hyper').bind("keydown", function (e) {
        /* ENTER PRESSED*/
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#bookingForm input:not(#bookinginputtab input), .btnbookingform, .custom-hyper');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled 
                && !this.className.includes("dropdown-toggle") && !this.className.includes("btn-addrow");
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".btnbookingform"));
            if (idx == summitButton && e.keyCode == 13) {
                return e;
            }
            let focusElement = $(".focus input");
            if (idx == inputs.length - 1) {
                focusElement.focus();
                
            } else {
                inputs[idx + 1].focus(); //  handles submit buttons
                //inputs[idx + 1].select();
            }
            return false;
        }
        if ((e.keyCode == 13 || e.keyCode == 9) && e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#bookingForm input:not(#bookinginputtab input), .btnbookingform, .custom-hyper');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled 
                && !this.className.includes("dropdown-toggle") && !this.className.includes("btn-addrow");
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".btnbookingform"));
            if (idx == summitButton && e.keyCode == 13) {
                return e;
            }
            let focusIndex = inputs.index($(".focus input"));
            if (idx == focusIndex) {
                inputs[inputs.length - 1].focus()
            } else {
                inputs[idx - 1].focus(); //  handles submit buttons
                //inputs[idx - 1].select();
            }
            return false;
        }
    });
}

function minMaxSettingTabKey() {
    $('#minMaxForm input, .submitBtnMinMaxForm').bind("keydown", function (e) {
        /* ENTER PRESSED*/
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#minMaxForm .withtabindex input, .submitBtnMinMaxForm');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled
                    && !this.className.includes("dropdown-toggle");
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".submitBtnMinMaxForm"));
            if (idx == summitButton && e.keyCode == 13) {
                return e;
            }
            if (idx == inputs.length - 1) {
                inputs[0].focus()
            } else {
                inputs[idx + 1].focus(); //  handles submit buttons
                //inputs[idx + 1].select();
            }
            return false;
        }
        if ((e.keyCode == 13 || e.keyCode == 9) && e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#minMaxForm .withtabindex input, .submitBtnMinMaxForm');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled
                    && !this.className.includes("dropdown-toggle");
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".submitBtnMinMaxForm"));
            if (idx == summitButton && e.keyCode == 13) {
                return e;
            }
            if (idx == 0) {
                inputs[inputs.length - 1].focus()
            } else {
                inputs[idx - 1].focus(); //  handles submit buttons
                //inputs[idx - 1].select();
            }
            return false;
        }
    });
}