//Add class hover for EditItem
function setHover() {
    $(document).ready(function () {
        $(".editable").hover(function () {
            $(".listTimeline").find("[data-bookingID='" + $(this).attr("data-bookingID") + "']").addClass("hover");
        }, function () {
            $(".listTimeline").find(".editable.hover").removeClass("hover");
        })
    });
}

//Set Bootstrap Tooltip EditItem
function setTooltip() {
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').each(function () {
            $(this).tooltip({
                sanitize: false,
                title: $(this).attr("data-original-title"),
                //delay: { show: 500, hide: 100 },
                trigger: "hover",
                //boundary: 'viewport'
            });
        })
    })
}