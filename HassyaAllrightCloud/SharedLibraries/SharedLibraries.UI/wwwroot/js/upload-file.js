window.filePreview = {
    preview: function (base64) {
        var binaryString1 = atob(base64);
        var len1 = binaryString1.length;
        var bytes1 = new Uint8Array(len1);
        for (let i = 0; i < len1; ++i) {
            bytes1[i] = binaryString1.charCodeAt(i);
        }

        var a = new Blob([bytes1], { type: 'application/pdf' });
        var fileURL = URL.createObjectURL(a);
        window.open(fileURL);
    }
}

window.copyText = function (id) {
    /* Get the text field */
    var copyText = document.getElementById(id);

    /* Select the text field */
    copyText.select();
    copyText.setSelectionRange(0, 99999); /* For mobile devices */

    /* Copy the text inside the text field */
    document.execCommand("copy");
}

window.setNodeDisable = function () {
    $('.tree-node-disabled').parent().click(e => { e.preventDefault(); e.stopPropagation(); })
}