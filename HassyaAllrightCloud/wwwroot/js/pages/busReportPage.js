// Use for save report as excel/PDF
function downloadFilebusReport(file, fileType) {
    contentType = "application/pdf";
    var sliceSize = 1024;
    var byteCharacters = atob(file);
    var bytesLength = byteCharacters.length;
    var slicesCount = Math.ceil(bytesLength / sliceSize);
    var byteArrays = new Array(slicesCount);
    for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
        var begin = sliceIndex * sliceSize;
        var end = Math.min(begin + sliceSize, bytesLength);
        var bytes = new Array(end - begin);
        for (var offset = begin, i = 0; offset < end; ++i, ++offset) {
            bytes[i] = byteCharacters[offset].charCodeAt(0);
        }
        byteArrays[sliceIndex] = new Uint8Array(bytes);
    }
    var blob = new Blob(byteArrays, { type: contentType });
    var a = window.document.createElement("a");
    a.href = window.URL.createObjectURL(blob, { type: "text/plain" });
    a.download = "Haisyahyo." + fileType;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}