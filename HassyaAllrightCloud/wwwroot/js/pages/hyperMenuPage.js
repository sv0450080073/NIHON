function hyperMenuPageTabKey() {
    $('#hyper-form input').bind("keydown", function (e) {
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            e.preventDefault();
            let inputs = $('#hyper-form input:visible');
            var idx = inputs.index(this);
            if (idx == inputs.length - 1) {
                inputs[0].focus()
            } else {
                inputs[idx + 1].focus(); //  handles submit buttons
            }
            return false;
            // var maxIndex = $('input').length;
            // if (nextIndex < maxIndex - 1 && $('#hyper-form input:eq(' + nextIndex + ')').is(":visible")) {
            //     $('#hyper-form input:eq(' + nextIndex + ')').focus();
            // } else {
            //     this.blur();
            // }
        }
        if ((e.keyCode == 13 || e.keyCode == 9) && e.shiftKey) {
            e.preventDefault();
            let inputs = $('#hyper-form input:visible');
            var idx = inputs.index(this);
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
function SetPositionForMenuContext(y, reference) {
    var rY = y;
    var browserHeight = $(window).height();
    var menuContext = $('#gridRowClickMenu');
    var menuHeight = 457;
    var x = browserHeight - y;
    if (x > y) {
        if (x < menuHeight) {
            rY = Math.ceil(browserHeight / 2 - menuHeight / 2);
        }
    }
    else {
        if (y < menuHeight) {
            rY = Math.ceil(browserHeight / 2 - menuHeight / 2);
        }
    }
    reference.invokeMethodAsync("positionGetComplete", rY);
}
