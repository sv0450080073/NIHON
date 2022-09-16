$(function () {
    var $toggleMenu = $(".ShowCalendar"),
        $menu = $(".calendar");

    $toggleMenu.on("click", function (e) {
        e.preventDefault();
        toggleUserMenu();
    });

    $toggleMenu.on("mouseup", function (e) {
        e.preventDefault();
        e.stopPropagation();
    });

    $(document).on("mouseup", function (e) {
        if (!$menu.is(e.target) && $menu.has(e.target).length === 0) {
            $menu.hide();
        }
    });

    function toggleUserMenu() {
        var menuIsVisible = $menu.is(":visible");
        if (menuIsVisible) {
            $menu.hide();
        } else {
            $menu.show();
        }
    }
});