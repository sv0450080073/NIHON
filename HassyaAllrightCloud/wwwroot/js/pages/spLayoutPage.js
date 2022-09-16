function checkFromMB(referece) {
    if (referece != undefined) {
        var md = new MobileDetect(window.navigator.userAgent);
        if (md.os() == null) {
            referece.invokeMethodAsync("checkBrower", true);
        }
        else {
            referece.invokeMethodAsync("checkBrower", false);
        }
    }
}