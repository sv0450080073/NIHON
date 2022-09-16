function SelectCellByTable(row, column, tableId, reference) {
    document.onmouseup = closeDragElement;
    document.onmousemove = elementDrag;
    var gridTr = $("#koban-table-" + tableId + " tbody tr");
    var fromRow = row;
    var toRow = row;
    var fromColumn = column;
    var toColumn = column;
    var coloringArray = [];
    function closeDragElement() {
        document.onmouseup = null;
        document.onmousemove = null;
        $("#koban-table-" + tableId + " tbody .dragged-cell").removeClass("dragged-cell");
        if (window.getSelection) {
            if (window.getSelection().empty) {
                window.getSelection().empty();
            } else if (window.getSelection().removeAllRanges) {
                window.getSelection().removeAllRanges();
            }
        } else if (document.selection) {
            document.selection.empty();
        }
        reference.invokeMethodAsync("WhenDragComplete", fromRow, toRow, fromColumn, toColumn);
    }

    function elementDrag() {
        var selection = window.getSelection();
        var parent = getParentElement(selection.extentNode, "tr");
        var child = getParentElement(selection.extentNode, "td");
        var indexRow = gridTr.index(parent);
        if (indexRow < 0) {
            return;
        }
        var rowElement = $("#koban-table-" + tableId + " tbody tr:nth-child(" + (indexRow + 1) + ") td:not(:first)");
        var indexColumn = rowElement.index(child) + 1;
        if (indexColumn == 0) {
            return;
        }

        fromRow = Math.min(row, indexRow);
        toRow = Math.max(row, indexRow);
        fromColumn = Math.min(column, indexColumn);
        toColumn = Math.max(column, indexColumn);
        for (var i = fromRow; i <= toRow; i++) {
            for (var j = fromColumn; j <= toColumn; j++) {
                var element = $("#koban-table-" + tableId + " tbody tr:nth-child(" + (i + 1) + ") td:nth-child(" + (j + 1) + ")");
                element.addClass("dragged-cell");
                var dictionaryToBePush = { row: i, column: j };
                if (!coloringArray.some(e => e.row == i && e.column == j)) {
                    coloringArray.push(dictionaryToBePush);
                }
            }
        }

        for (var i = coloringArray.length - 1; i >= 0; i--) {
            if (!(fromRow <= coloringArray[i].row && coloringArray[i].row <= toRow && fromColumn <= coloringArray[i].column && coloringArray[i].column <= toColumn)) {                
                var element = $("#koban-table-" + tableId + " tbody tr:nth-child(" + (coloringArray[i].row + 1) + ") td:nth-child(" + (coloringArray[i].column + 1) + ")");
                element.removeClass("dragged-cell");
                coloringArray.splice(i, 1);
            }
        }
    }

    function getParentElement(element, type) {
        if (element == null || element.localName == type) {
            return element;
        }
        return getParentElement(element.parentElement, type);
    } 
}