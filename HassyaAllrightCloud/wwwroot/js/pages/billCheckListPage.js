function fadeToggleTable() {
    $("#total-bill-area").on('click', '#tr-total-1', function () {
        $('.tr-child-1').toggleClass('tr-child-active');
        setTimeout(function () {
            AdjustHeight();
        }, 500);
    });
    $("#total-bill-area").on('click', '#tr-total-2', function () {
        $('.tr-child-2').toggleClass('tr-child-active');
        setTimeout(function () {
            AdjustHeight();
        }, 500);
    });
    $("#total-bill-area").on('click', '#tr-total-3', function () {
        $('.tr-child-3').toggleClass('tr-child-active');
        setTimeout(function () {
            AdjustHeight();
        }, 500);
    });
    
}

function fadeToggleWidthAdjustHeight() {
    AdjustHeight();
    $("#content").on('click', '.bill-list-title-section', function () {
        setTimeout(function () {
            AdjustHeight();
        }, 500);
    });
    window.addEventListener("resize", function () {
        if ($('.bill-list-title-section').length > 0) {
            setTimeout(function () {
                AdjustHeight();
            }, 400);
        }
    });
}

function AdjustHeight() {
    var browserHeight = $(window).height();
    var conditionDiv = $('.express-condition.mb-2');
    var tableArea = $('#table-bill-wrapper');
    if (tableArea[0] != undefined) {
        var tableAreaHeight = tableArea[0].offsetHeight;
        var totalArea = $('#total-bill-area');
        var totalAreaRect = totalArea[0].getBoundingClientRect();
        var adjustHeightValue = 0;
        var scroll = document.documentElement.scrollTop;
        if (conditionDiv.is(":visible")) {
            adjustHeightValue = tableAreaHeight + (browserHeight - totalAreaRect.top - scroll);
        } else {
            adjustHeightValue = tableAreaHeight - (totalAreaRect.bottom - browserHeight + scroll);
        }
        tableArea.css('max-height', adjustHeightValue + "px");
        tableArea.css('min-height', adjustHeightValue + "px");
    }
}

function fadeToggleBillTitle() {
    $("#content").on('click', '.bill-title-section', function (e) {
        //e.stopPropagation();
        var $element = $(this).next();
        var $icon = $(this).find('i');
        if ($element.is(':visible')) {
            $element.slideUp();
            $icon.removeClass('fa-angle-up').addClass('fa-angle-down');
            setTimeout(function () {
                AdjustHeight();
            }, 500);
        } else {
            $element.slideDown();
            $icon.removeClass('fa-angle-down').addClass('fa-angle-up');
            setTimeout(function () {
                AdjustHeight();
            }, 400);
        }
    });

}

function handleKeyPress() {
    $('#bills-check-list-form input').bind("keydown", function (e) {
        if (e.keyCode == 13 && !e.shiftKey) {
            e.preventDefault();
            var nextIndex = $('input').index(this);
            var maxIndex = $('input').length;
            if (nextIndex < maxIndex - 1 && $('#bills-check-list-form input:eq(' + nextIndex + ')').is(":visible")) {
                $('#bills-check-list-form input:eq(' + nextIndex + ')').focus();
            } else {
                this.blur();
            }
        }
        if (e.keyCode == 13 && e.shiftKey) {
            e.preventDefault();
        }
    });
}

function billCheckListPageTabKey() {
    $('#bills-check-list-form input').bind("keydown", function (e) {
        if ((e.keyCode == 13 || e.keyCode == 9) && !e.shiftKey) {
            e.preventDefault();
            let inputs = $('#bills-check-list-form input:visible');
            var idx = inputs.index(this);
            if (idx == inputs.length - 1) {
                inputs[0].focus()
            } else {
                inputs[idx + 1].focus(); //  handles submit buttons
            }
            return false;
        }
        if ((e.keyCode == 13 || e.keyCode == 9) && e.shiftKey) {
            e.preventDefault();
            let inputs = $('#bills-check-list-form input:visible');
            var idx = inputs.index(this);
            if (idx == 0) {
                inputs[inputs.length - 1].focus()
            } else {
                inputs[idx - 1].focus();
            }
            return false;
        }
    });
}

function tabindexFix(selector = ".focus-form", ignoreFirstItem = false, useSelectorOnly = false) {
    {
        let el = useSelectorOnly ? selector : `${selector} input, ${selector} button.lifecycle-btn`;
        let el$ = $(el);
        // Setting focus on first textbox
        if (ignoreFirstItem)
            el$.splice(0, 1);

        // binding keydown event to textbox

        el$.bind('keydown', function (e) {
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