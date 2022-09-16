function partnerBookingPageTabKey() {
    $('#partnerBookingInput input, .btnpartbookingform ').bind("keydown", function (e) {
        /* ENTER PRESSED*/
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#partnerBookingInput input, .btnpartbookingform');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled
                    && !this.className.includes("dropdown-toggle");
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".btnpartbookingform"));
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
            let inputs = $('#partnerBookingInput input , .btnpartbookingform');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled
                    && !this.className.includes("dropdown-toggle") ;
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".btnpartbookingform"));
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


    $('#UpdateHaishaForm input, .btnHaishaform ').bind("keydown", function (e) {
        /* ENTER PRESSED*/
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#UpdateHaishaForm input, .btnHaishaform');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled
                    && !this.className.includes("dropdown-toggle");
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".btnHaishaform"));
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
            let inputs = $('#UpdateHaishaForm input , .btnHaishaform');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled
                    && !this.className.includes("dropdown-toggle");
            })
            let idx = inputs.index(this);
            let summitButton = inputs.index($(".btnHaishaform"));
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