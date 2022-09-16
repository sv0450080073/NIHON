function bookingInputPageTabKey() {
    $('#bookingForm input, .btnbookingform, .custom-hyper').on("keyup", function (e) {
        /* ENTER PRESSED*/
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#bookingForm input, .btnbookingform, .custom-hyper');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled 
                && !this.className.includes("dropdown-toggle") && !this.className.includes("btn-addrow");
            })
            let idx = e.keyCode == 13 ? inputs.index(this) : inputs.index(this) - 1;
            let summitButton = inputs.index($(".btnbookingform"));
            if (idx == summitButton && e.keyCode == 13 && $(".btnbookingform").is(':enabled')) {
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
            let inputs = $('#bookingForm input, .btnbookingform, .custom-hyper');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled 
                && !this.className.includes("dropdown-toggle") && !this.className.includes("btn-addrow");
            })
            let idx = e.keyCode == 13 ? inputs.index(this) : inputs.index(this) - 1;
            let summitButton = inputs.index($(".btnbookingform"));
            if (idx == summitButton && e.keyCode == 13 && $(".btnbookingform").is(':enabled')) {
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

let backIdx = 0;
let backKeyCode = 0;
function onValidateMinMax(isErrorFare, isErrorFee) {
    console.log('attribute was modified.');
    /* FOCUS ELEMENT */
    let inputs = $('#minMaxForm .withtabindex input, .submitBtnMinMaxForm');
    inputs = inputs.filter(function (index) {
        return !this.readOnly && !this.disabled
            && !this.className.includes("dropdown-toggle");
    })
    if (backIdx == 12 && backKeyCode == 13) {
        if (isErrorFare && !isErrorFee) {
            inputs[12].focus();
        } else {
            inputs[13].focus();
        }
    } else if (backIdx == 13 && backKeyCode == 13) {
        if (isErrorFare) {
            inputs[12].focus();
        } else if (!isErrorFare && isErrorFee) {
            inputs[13].focus();
        } else {
            try{
                inputs[14].focus();
            }catch(err){
                inputs[13].focus();
            }
        }
    }
};

function futaiTabKey(){
    $('#futaiForm .btnFutaiForm button').bind("keydown", function (e) {
        if ((e.keyCode == 9) && !e.shiftKey) {
            let buttons = $('#futaiForm .btnFutaiForm button');
            buttons = buttons.filter(function (index) {
                return !this.readOnly && !this.disabled;
            });
            let idx = buttons.index(this);
            if(idx === buttons.length - 1){
                let inputs = $('#futaiForm .withtabindex input');
                inputs[0].focus();
                return false;
            }
        }
    });
}

function futaiEnterKey(){
    $('#futaiForm input').bind("keydown", function (e) {
        /* ENTER PRESSED*/
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            /* FOCUS ELEMENT */
            let inputs = $('#futaiForm .withtabindex input');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled;
            })
            let idx = inputs.index(this);

            if (idx == inputs.length - 1) {
                if(e.keyCode == 9){
                    let buttons = $('#futaiForm .btnFutaiForm button');
                    buttons = buttons.filter(function (index) {
                        return !this.readOnly && !this.disabled;
                    });
                    buttons[0].focus();
                }else if(e.keyCode == 13){
                    inputs[0].focus();
                }
            } else {
                inputs[idx + 1].focus();
                inputs[idx + 1].value = inputs[idx + 1].value.replace(/,/g, "");
            }
            return false;
        }
        if (e.keyCode == 9 && e.shiftKey) {
            let inputs = $('#futaiForm .withtabindex input');
            inputs = inputs.filter(function (index) {
                return !this.readOnly && !this.disabled;
            });
            let idx = inputs.index(this);
            if(idx === 0){
                let buttons = $('#futaiForm .btnFutaiForm button');
                buttons = buttons.filter(function (index) {
                    return !this.readOnly && !this.disabled;
                })
                buttons[buttons.length - 1].focus();
            }else{
                console.log('shift');
                inputs[idx - 1].focus();
                //inputs[idx - 1].value = inputs[idx + 1].value.replace(/,/g, "");
            }
            return false;
        }
    })
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

            e.preventDefault();
            backIdx = idx;
            backKeyCode = e.keyCode;
            return e.keyCode == 13 ? e : false;
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

function setTooltipBookingInput() {
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip({
            html: true,
            sanitize: false,
            delay: { show: 500, hide: 100 }
        })
    })
}

function EnterTab(selector = ".focus-form", ignoreFirstItem = false, useSelectorOnly = false) {
    {
        let el = useSelectorOnly ? selector : `${selector} input:not([readonly]), ${selector} button.lifecycle-btn`;
        let el$ = $(el);
        // Setting focus on first textbox
        if (ignoreFirstItem)
            el$.splice(0, 1);

        // binding keydown event to textbox

        el$.on('keyup', function (e) {
            // detecting keycode returned from keydown and comparing if its equal to 13 (enter key code)
            if ((e.which == 13 || e.which == 9) && e.shiftKey) {
                // by default if you hit enter key while on textbox so below code will prevent that default behaviour

                e.preventDefault();

                // getting next index by getting current index and incrementing it by 1 to go to next textbox

                var preIndex = $(el).index(this) - 1;
                var maxIndex = $(el).length;

                if (preIndex < 0)
                    preIndex = maxIndex - 1; // focus on last input

                for (i = preIndex; i > 0; i--) {
                    var selectedIndex = $(el).eq(i).attr('tabindex');
                    if (selectedIndex == -1) {
                        preIndex = preIndex - 1;
                    }
                    else {
                        preIndex = preIndex;
                        break;
                    }
                }
                if (preIndex > -1) {
                    // setting index to next textbox using CSS3 selector of nth child

                    $(el).eq(preIndex).focus();
                }
            }
            else
                if (e.keyCode == 13 || e.which == 9) {
                    // by default if you hit enter key while on textbox so below code will prevent that default behaviour

                    e.preventDefault();
                    // getting next index by getting current index and incrementing it by 1 to go to next textbox

                    var nextIndex = $(el).index(this) + 1;

                    // getting total number of textboxes on the page to detect how far we need to go

                    var maxIndex = $(el).length;

                    if (nextIndex >= maxIndex)
                        nextIndex = 0; // focus on first input

                    for (i = nextIndex; i < maxIndex; i++) {
                        var selectedIndex = $(el).eq(i).attr('tabindex');
                        if (selectedIndex == -1) {
                            nextIndex = nextIndex + 1;
                        }
                        else {
                            nextIndex = nextIndex;
                            break;
                        }
                    }

                    // check to see if next index is still smaller then max index
                    if (nextIndex < maxIndex) {
                        // setting index to next textbox using CSS3 selector of nth child
                        $(el).eq(nextIndex).focus();
                    }
                }
        });
    }
}

function EnterTabV2(selector = ".focus-form") {
    $('body').on('keydown', 'input, select', function (e) {
        if (e.key === "Enter") {
            var self = $(this), form = self.parents('form:eq(0)'), focusable, next;
            focusable = form.find('input,a,select,button,textarea').filter(':visible');
            next = focusable.eq(focusable.index(this) + (e.shiftKey ? -1 : 1));
            if (next.length) {
                next.focus();
            } else {
                form.submit();
            }
            return false;
        }
    });
}