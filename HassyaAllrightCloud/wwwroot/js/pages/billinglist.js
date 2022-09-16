function clickSearchDropDown() {
    document.getElementById("control-tab").click();
}

function fadeToggleTable() {
    $("#tr-total-1").off('click').on('click', function () {
        $('.tr-child-1').toggleClass('tr-child-active');
    });
    $("#tr-total-2").off('click').on('click', function () {
        $('.tr-child-2').toggleClass('tr-child-active');
    });
    $("#tr-total-3").off('click').on('click', function () {
        $('.tr-child-3').toggleClass('tr-child-active');
    });
}