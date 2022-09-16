var referenceGlobal;
function RenderFilterSearch(CustomFilters, reference) {
    referenceGlobal = reference;
    var data = ParseToObject(CustomFilters);
    $("#filter-container").dxSelectBox({
        items: data,
        displayExpr: "filterName",
        showClearButton: true,
        placeholder: "選択してください。",
        noDataText: "データがありません。",
        valueExpr: "id",
        itemTemplate: function (data) {
            var object = JSON.stringify(data);
            var reservation = ParseToObject(object);
            return "<div class='d-flex' style='justify-content:space-between'><div>" + reservation.filterName + "</div>" + "<div>" + "<a id='rename' onClick='renameFiler(" + object + ");'>名前を変更</a>" + "|" + "<a id='delete'onClick='deleteFilter(" + object + ");'>削除</a>" + "</div></div>";
        },
        onValueChanged: function (e) {
            var item = e.component.option('selectedItem');
            FilterSelected(item);
        }  
    });
   
}
function FilterSelected(data) {
    referenceGlobal.invokeMethodAsync("CustomFilterSelected", data);
}
function renameFiler(data) {
    referenceGlobal.invokeMethodAsync("RenameCustomFilter", data);
}
function deleteFilter(data) {
    referenceGlobal.invokeMethodAsync("DeleteCustomFilter", data);
}
function ParseToObject(json) {
    var newJson = json.replace(/"([\w]+)":/g, function ($0, $1) {
        return ('"' + $1.charAt(0).toLowerCase() + $1.slice(1) + '":');
    });
    return JSON.parse(newJson);
}