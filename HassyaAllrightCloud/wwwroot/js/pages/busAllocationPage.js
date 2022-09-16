function busAllocationPageTabKey() {
    $('#BusAllocationSearchForm input, #BusAllocationForm input').bind("keydown", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 9 && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#BusAllocationSearchForm input, #BusAllocationForm input');

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
            let inputs = $('#BusAllocationSearchForm input, #BusAllocationForm input');
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

function scrollToTop() {    
    var offsetparent, offsetchild;
    if ($(".active.table-primary").length) {
        offsetparent = $(".area-table").offset().top;        
        offsetchild = $(".active.table-primary").offset().top;
        $(".area-table").scrollTop($(offsetchild)[0] - $(offsetparent)[0] - $(".area-table th").outerHeight());
    }    
}