function setHeightTopGrid() {
    $(document).ready(function () {
        $(".topbar").each(function () {
            $(this).css("height", $(this).parent().parent().find('.listColumn').outerHeight());
        })
    });
    $(window).on('load resize', function () {
        $(".topbar").each(function () {
            $(this).css("height", $(this).parent().parent().find('.listColumn').outerHeight());
        })
    });
}