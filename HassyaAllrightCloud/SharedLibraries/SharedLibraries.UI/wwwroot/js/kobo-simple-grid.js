(function () {
    var counter = 0;
    window.KoboGrid = {
        id: '',
        updateGridSpliter: function (id) {
            $(`.kobo-simple-grid[data-id='${id}'] .grid-spliter`).height($(`.kobo-simple-grid[data-id='${id}']`).height());
        },
        initColEvents: function (colSticky, borderWidth, id, componentInstance) {
            resizeColumns(borderWidth, colSticky, id, componentInstance);
            initDragDrop(id, componentInstance);
        },
        initLeftAndTop: function (colHeight, borderWidth, colSticky, id) {
            borderWidth = 1;
            KoboGrid.id = id
            setTopForRowSpan(colHeight, id);
            setLeftForColSpan(colSticky, id);
        },
        saveGridLayout: function (id, layoutJson) {
            localStorage.setItem(id, layoutJson);
        },
        getGridLayout: function (id) {
            return localStorage.getItem(id);
        }
    }

    function initDragDrop(id, componentInstance) {

        var cols = $(`.kobo-simple-table[data-id='${id}'] th div.dragdrop`);
        if (cols) {
            for (let col of cols) {
                $(col).on('dragstart', function (e) {
                    listenerDragStart(e, id);
                });
                $(col).on('dragenter', function (e) {
                    listenerDragEnter(e);
                });
                $(col).on('dragover', function (e) {
                    listenerDragOver(e);
                });
                $(col).on('dragleave', function (e) {
                    listenerDragLeave(e);
                });
                $(col).on('drop', function (e) {
                    listenerDrop(e, id, componentInstance);
                });
                $(col).on('dragend', function (e) {
                    listenerDragEnd(e, id);
                });
            }
        }
    }

    function setTopForRowSpan(colHeight, id) {
        let els = $(`.kobo-simple-table[data-id='${id}']>thead>tr`);
        if (els) {
            let css = {};
            let colh = 0;
            let i = true;
            for (let el of els) {
                colh += i ? 0 : colHeight;
                css = { 'position': 'sticky', 'z-index': '11', 'top': i ? 0 : `${colh}rem` }
                $(el).find('th').css(css);
                i = false;
            }
        }
    }

    function setLeftForColSpan(colSticky, id) {
        let bodytrs = $(`.kobo-simple-table[data-id='${id}']>tbody>tr`);
        updateLeftForCols(false, bodytrs, colSticky, 10);
        let headtrs = $(`.kobo-simple-table[data-id='${id}']>thead>tr`);
        updateLeftForCols(true, headtrs, colSticky, 13);
    }

    function updateLeftForCols(isTh, trs, colSticky, zIndex) {

        let tempRows = [];
        let removeList = [];
        if (trs) {
            for (let i = 0; i < trs.length; i++) {
                let tr = trs[i];
                let cols = $(tr).find(isTh ? 'th' : 'td');
                if (cols) {
                    let css = {};
                    let colCount = 0;
                    let width = 0;
                    if (tempRows && tempRows.length) {
                        for (let item of tempRows) {
                            item.rowspan--;
                            if (item.rowspan < 1)
                                removeList.push(item);
                        }

                        if (removeList && removeList.length) {
                            for (let item of removeList)
                                tempRows.splice(tempRows.indexOf(item), 1)
                        }
                    }

                    for (let j = 0; j < cols.length; j++) {
                        let col = cols[j];
                        let rowspan = getRowSpan(col);
                        let colspan = getColSpan(col);

                        while (true) {
                            let row = tempRows.find(e => e.index == colCount + 1);
                            if (row) {
                                colCount += row.colspan;
                                width += getWidth(row.col);
                                if (row.rowspan <= 1)
                                    tempRows.splice(tempRows.indexOf(row), 1);
                            } else {
                                break;
                            }
                        }

                        colCount++;
                        if (rowspan > 1)
                            tempRows.push({ index: colCount, colspan: colspan, rowspan: rowspan, col: col });

                        if (colspan > 1)
                            colCount += (colspan - 1);

                        if (colCount > colSticky) break;
                        else {
                            css = { 'position': 'sticky', 'left': width, 'z-index': zIndex };
                            $(col).css(css);
                            width += getWidth(col);
                        }
                    }
                }
            }
        }
    }

    function getWidth(el) {
        let a = $(el).css('width').replace('px', '');
        return parseInt(a);
    }

    function resizeColumns(borderWidth, colSticky, id, componentInstance) {
        let grid = $(`.kobo-simple-grid[data-id='${id}']`);
        let els = $(`.kobo-simple-grid[data-id='${id}'] .resize-div`);
        let isMouseDown = false;
        let startX;
        let selectedEl = null;
        let destX;
        for (let el of els) {
            el.addEventListener('mousedown', function (e) {
                let xGrid = grid.offset();
                startX = e.pageX - xGrid.left;
                $(`.kobo-simple-grid[data-id='${id}'] .grid-spliter`).css({ left: startX, display: 'block' });
                isMouseDown = true;
                selectedEl = this;
            });
        }
        document.addEventListener('mouseup', function (e) {
            $(`.kobo-simple-grid[data-id='${id}'] .grid-spliter`).css({ display: 'none' });

            if (isMouseDown) {
                let th = $(selectedEl).parents('th:first');
                let width = th.width();
                let diffX = destX - startX;
                width = width + diffX;
                if (width < 50) {
                    width = 50
                }
                console.log(`tr index: ${$(th).parents('tr:first').index()}, th index: ${$(th).index()} update width: ${width}`);
                componentInstance.invokeMethodAsync('UpdateColWidth', $(th).parents('tr:first').index(), $(th).index(), parseInt(width)).then(function () {
                }, function (err) {
                    throw new Error(err);
                });
                $(th).css('min-width', `${width}px`);
                setLeftForColSpan(borderWidth, colSticky, id);
            }

            isMouseDown = false;
            startX = undefined;
            destX = undefined;
            selectedEl = null;
        });
        document.addEventListener('mousemove', function (e) {
            if (isMouseDown) {
                let xGrid = grid.offset();
                destX = e.pageX - xGrid.left;
                $(`.kobo-simple-grid[data-id='${id}'] .grid-spliter`).css({ left: destX, display: 'block' });
            }
        });
    }

    function listenerDragStart(e, id) {

        $('.tableGhost').remove();
        let el = e.target;
        e.originalEvent.dataTransfer.effectAllowed = 'move';
        e.originalEvent.dataTransfer.setData('text', $(el).text());

        //Create column's container
        var dragGhost = document.createElement("table");
        dragGhost.classList.add("tableGhost");
        dragGhost.classList.add("table-bordered");
        //in order tor etrieve the column's original width
        var srcStyle = document.defaultView.getComputedStyle(el);
        dragGhost.style.width = srcStyle.getPropertyValue("width");

        //Create head's clone
        var theadGhost = document.createElement("thead");
        var thisGhost = el.cloneNode(true);
        thisGhost.style.backgroundColor = "pink";
        theadGhost.appendChild(thisGhost);
        dragGhost.appendChild(theadGhost);

        var srcTxt = $(el).text();
        let src = $("th:contains(" + jqescape(srcTxt) + ")");

        //Hide ghost
        dragGhost.style.position = "absolute";
        dragGhost.style.top = "-2000px";

        document.body.appendChild(dragGhost);
        e.originalEvent.dataTransfer.setDragImage(dragGhost, 0, 0);
    }

    function countColIndex(srcEl) {
        let thPrevs = srcEl.prevAll();
        let count = 0;
        for (let i of thPrevs) {
            let colspan = getColSpan(i);
            if (colspan > 1)
                count += (colspan - 1);
        }
        return srcEl.index() + 1 + count;
    }

    function listenerDragOver(e) {
        if (e.preventDefault) { e.preventDefault(); }
        e.originalEvent.dataTransfer.dropEffect = 'move';
        return false;
    }

    function listenerDragEnter(e) {
        let el = e.target;
        counter++;
        $(el).parents('th:first').addClass('over');
    }

    function listenerDragLeave(e) {
        let el = e.target;
        counter--;
        if (counter === 0) {
            $(el).parents('th:first').removeClass('over');
        }
    }

    function getColSpan(el) {
        if (el == null) return 0;
        else {
            let colSpan = $(el).attr('colspan');
            return colSpan ? parseInt(colSpan) : 1;
        }
    }

    function getRowSpan(el) {
        if (el == null) return 0;
        else {
            let rowSpan = $(el).attr('rowspan');
            return rowSpan ? parseInt(rowSpan) : 1;
        }
    }

    function jqescape(str) { return str.replace(/[#;&,\.\+\*~':"!\^\$\[\]\(\)=>|\/\\]/g, '\\$&'); }

    function listenerDrop(e, id, componentInstance) {        
        $(`.kobo-simple-table[data-id='${id}'] .tableGhost`).remove();
        if (e.preventDefault) { e.preventDefault(); }
        if (e.stopPropagation) { e.stopPropagation(); }
        let el = e.target;
        counter = 0;
        $(el).parents('th:first').removeClass('over');
        var srcTxt = e.originalEvent.dataTransfer.getData('text');
        var destTxt = $(el).text();
        if (srcTxt != destTxt) {
            var srcEl = $(`.kobo-simple-table[data-id='${id}'] th>div.cell-container>div:first-child:contains(${jqescape(srcTxt)})`).parents('th:first');
            var srcIndex = countColIndex(srcEl);
            let desEl = $(`.kobo-simple-table[data-id='${id}'] th>div.cell-container>div:first-child:contains(${jqescape(destTxt)})`).parents('th:first');
            var destIndex = countColIndex(desEl);
            console.log(`swap col ${srcIndex} to ${destIndex}`)
            componentInstance.invokeMethodAsync('UpdateColOrder', srcIndex, destIndex).then(function () {
            }, function (err) {
                throw new Error(err);
            });
        }
    }

    function listenerDragEnd(e, id) {
        $(`.kobo-simple-table[data-id='${id} .tableGhost`).remove();
        let cols = $(`.kobo-simple-table[data-id='${id}'] th div.dragdrop`);
        for (let col of cols) {
            $(col).parents('th:first').removeClass('over');
            $(col).parents('th:first').css({ opacity: 1 });
        }
    }
})();
