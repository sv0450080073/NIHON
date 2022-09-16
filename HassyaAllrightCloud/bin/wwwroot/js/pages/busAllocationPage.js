function busAllocationPageTabKey() {
    $('#busallocationForm1 input, #busallocationForm2 input').bind("keydown", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 9 && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#busallocationForm1 input, #busallocationForm2 input');

            inputs = inputs.filter(function (index) {
                return !this.readOnly
                    && !this.disabled
                    && !this.className.includes("nav-link active")
                    || this.className.includes("dx-reset-readonly-style");
            })
            let idx = inputs.index(this);

            if (idx == inputs.length - 1) {
                inputs[0].focus()
            } else {
                inputs[idx + 1].focus(); //  handles submit buttons
            }
            return false;
        }
        if (e.keyCode == 9 && e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#busallocationForm1 input, #busallocationForm2 input');
            inputs = inputs.filter(function (index) {
                return !this.readOnly
                    && !this.disabled
                    && !this.className.includes("nav-link active")
                    || this.className.includes("dx-reset-readonly-style");
            })
            let idx = inputs.index(this);

            if (idx == 0) {
                inputs[inputs.length - 1].focus()
            } else {
                inputs[idx - 1].focus(); //  handles submit buttons
            }
            return false;
        }
    });
    //$('#busallocationForm1 input, #busallocationForm1 a, #busallocationForm1 button, #busallocationForm2 input, #busallocationForm2 a, #busallocationForm2 button').bind("keydown", function (e) {
    //    /* ENTER PRESSED*/
    //    if ( e.keyCode == 9 && !e.shiftKey) {
    //        /* FOCUS ELEMENT */
    //        let inputs = $('#busallocationForm1 input, #busallocationForm1 a, #busallocationForm1 button, #busallocationForm2 input, #busallocationForm2 a, #busallocationForm2 button');
            
    //        inputs = inputs.filter(function (index) {
    //            return !this.readOnly 
    //            && !this.disabled 
    //            && !this.className.includes("nav-link active")
    //            || this.className.includes("dx-reset-readonly-style");
    //        })
    //        let idx = inputs.index(this);

    //        if (idx == inputs.length - 1) {
    //            inputs[0].focus()
    //        } else {
    //            inputs[idx + 1].focus(); //  handles submit buttons
    //        }
    //        return false;
    //    }
    //    if (e.keyCode == 9 && e.shiftKey) {
    //        /* FOCUS ELEMENT */
    //        let inputs = $('#busallocationForm1 input, #busallocationForm1 a, #busallocationForm1 button, #busallocationForm2 input, #busallocationForm2 a, #busallocationForm2 button');
    //        inputs = inputs.filter(function (index) {
    //            return !this.readOnly 
    //            && !this.disabled 
    //            && !this.className.includes("nav-link active") 
    //            || this.className.includes("dx-reset-readonly-style");
    //        })
    //        let idx = inputs.index(this);

    //        if (idx == 0) {
    //            inputs[inputs.length - 1].focus()
    //        } else {
    //            inputs[idx - 1].focus(); //  handles submit buttons
    //        }
    //        return false;
    //    }
    //});
}