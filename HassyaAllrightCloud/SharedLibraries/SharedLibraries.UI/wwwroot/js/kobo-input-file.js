(function () {
    window.inputFile = {
        init: (id, el, elInst) => {
            let input = document.getElementById(id);
            input.addEventListener('change', e => {
                let files = e.target.files;
                if (!el.koboFileCount)
                    el.koboFileCount = 0;
                if (!el.koboInputFile)
                    el.koboInputFile = {};
                let fileList = []
                for (let f of files) {
                    let result = {
                        id: ++el.koboFileCount,
                        lastModified: new Date(f.lastModified).toISOString(),
                        name: f.name,
                        size: f.size,
                        type: f.type,
                        relativePath: f.webkitRelativePath
                    };
                    el.koboInputFile[result.id] = result;

                    // Attach the blob data itself as a non-enumerable property so it doesn't appear in the JSON
                    Object.defineProperty(result, 'blob', { value: f });
                    fileList.push(result);
                }

                let list = [];
                if (fileList.length > 100) {
                    list = subList(fileList, 100);
                } else {
                    list = [fileList];
                }
                let i = 0;
                for (let item of list) {
                    i++;
                    let isEnd = false;
                    if (i == list.length) {
                        isEnd = true;
                    }

                    elInst.invokeMethodAsync('HandleFileChange', item, isEnd).then(function () {
                        el.value = '';
                    }, function (err) {
                        el.value = '';
                        throw new Error(err);
                    });
                }

            }, false);
        },

        readFileData: async (elem, fileId, startOffset, endOffset) => {
            let file = getFileById(elem, fileId);
            let newBlob = file.blob.slice(startOffset, endOffset);
            let base64 = await blobToBase64(newBlob);
            return base64;
        },
    };

    function subList(list, num) {
        let result = [];
        var count = Math.floor(list.length / num);
        var temp = list.length % num;
        if (count > 0) {
            for (var i = 0; i < count; i++) {
                let temp = list.splice(0, num);
                result.push(temp);
            }
            if (temp != 0) {
                result.push(list);
            }
        }
        else
            result.push(list);

        return result;
    }

    function getFileById(elem, fileId) {
        var file = elem.koboInputFile[fileId];
        if (!file) {
            throw new Error('There is no file with ID ' + fileId + '. The file list may have changed');
        }

        return file;
    }


    function blobToBase64(blob) {
        const reader = new FileReader();
        reader.readAsDataURL(blob);
        return new Promise(resolve => {
            reader.onloadend = () => {
                resolve(reader.result.substr(reader.result.indexOf(',') + 1));
            };
        });
    }
})();
