function fadeToggleGroupStaffScheduleMB() {
    var tgTitleSection = $("#contentMobile #tableGroupScheduleMB .group-schedule-mobile-title-section");
    tgTitleSection.each(function () {
        $(this).click(function () {
            var $element = $(this).next();
            var $icon = $(this).find('i:first');
            if ($element.is(':visible')) {
                $element.slideUp();
                $icon.removeClass('fa-angle-up').addClass('fa-angle-down');
            } else {
                setTimeout(function () {
                    $element.slideDown();
                    $icon.removeClass('fa-angle-down').addClass('fa-angle-up');
                }, 400);
                
            }
        });
        
    });
}