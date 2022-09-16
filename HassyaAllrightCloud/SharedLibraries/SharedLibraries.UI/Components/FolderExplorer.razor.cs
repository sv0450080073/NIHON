using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using SharedLibraries.UI.Services;
using SharedLibraries.Utility.Models;
using System.IO;
using SharedLibraries.Utility.Constant;
using Microsoft.JSInterop;
using System.Globalization;
using BlazorContextMenu;
using SharedLibraries.Utility.Helpers;
using SharedLibraries.Utility.Exceptions;

namespace SharedLibraries.UI.Components
{
    public class FolderExplorerBase : ComponentBase
    {
        protected class TableRow
        {
            private string _updYmdTime;
            public bool IsFile { get; set; }
            public bool IsExpanded { get; set; }
            public string Name { get { return IsFile ? File?.Name : Folder?.Name; } }
            public string UpdName { get { return IsFile ? File?.UpdSyainNm?.ToString() : Folder?.UpdSyainNm?.ToString(); } }
            
            public int DownloadCount
            {
                get
                {
                    return File != null ? File.TotalDownload : 0;
                }
            }

            public string UpdTime
            {
                get
                {
                    string updYmd;
                    string updTime;
                    if (IsFile)
                    {
                        updYmd = File?.UpdYmd;
                        updTime = File?.UpdTime;
                    }
                    else
                    {
                        updYmd = Folder?.UpdYmd;
                        updTime = Folder?.UpdTime;
                    }

                    if (string.IsNullOrEmpty(_updYmdTime))
                        _updYmdTime = string.IsNullOrEmpty(updYmd) || string.IsNullOrEmpty(updTime) ? string.Empty :
                                DateTime.ParseExact($"{updYmd} {updTime}", $"{CommonConst.Format_yyyyMMdd} {CommonConst.Format_HHmmss}", CultureInfo.InvariantCulture)
                                .ToString($"{CommonConst.Format_yyyyMMdd_Slash} {CommonConst.Format_HHmmss_Colon}");
                    return _updYmdTime;
                }
            }
            public S3File File { get; set; }
            public S3Folder Folder { get; set; }
        }
        [Inject] protected IStringLocalizer<FolderExplorer> _lang { get; set; }
        [Inject] protected ISharedLibrariesApi _s3Service { get; set; }
        [Inject] protected NavigationManager _navigation { get; set; }
        [Inject] protected IJSRuntime _jsRuntime { get; set; }
        [Parameter] public int TenantCode { get; set; }
        [Parameter] public int UpdSyainCd { get; set; }
        [Parameter] public Action<Exception> OnError { get; set; }

        protected Tree Tree { get; set; } = new Tree();
        protected DxTreeView tree { get; set; }
        protected Guid treeKey { get; set; }

        protected Tree TreeMove { get; set; } = new Tree();
        protected DxTreeView treeMove { get; set; }
        protected Guid treeMoveKey { get; set; }

        protected List<TenantItem> tenants { get; set; }
        protected List<string> DisabledNodes { get; set; } = new List<string>();
        protected class OrderCondition<TEntity> where TEntity : class
        {
            public string PropertyName { get; set; }
            public Func<TEntity, dynamic> OrderBy { get; set; }
            public SortEnum SortDirection { get; set; }
        }
        protected enum SortEnum
        {
            Asc,
            Desc
        }
        protected OrderCondition<TableRow> sortCondition { get; set; } = new OrderCondition<TableRow>()
        {
            PropertyName = nameof(TableRow.Name),
            OrderBy = r => r.Name,
            SortDirection = SortEnum.Asc
        };
        protected bool showDownloadHistory { get; set; }
        protected bool showMove { get; set; }
        protected bool showRenamePopup { get; set; }
        protected bool Loading { get; set; }
        protected bool showUpload { get; set; }
        protected bool showError { get; set; }
        protected bool showNewFolderPopup { get; set; }
        protected bool showConfirm { get; set; }
        protected bool isRemove { get; set; }
        protected string folderName { get; set; }
        protected string newName { get; set; }
        protected string errorMsg { get; set; }

        protected string bucket { get; set; }
        protected List<string> breadcrumbs { get; set; } = new List<string>();
        protected TableRow selectedRow { get; set; }
        protected TableRow moveFrom { get; set; }
        protected TreeNode moveTo { get; set; }
        protected TableRow removeRow { get; set; }
        protected TableRow changeRow { get; set; }
        protected TreeNode selectedNode { get; set; }
        protected TenantItem selectedTenant { get; set; }

        protected List<DownloadedInfo> DownloadHistory { get; set; } = new List<DownloadedInfo>();
        protected List<TableRow> Rows { get; set; } = new List<TableRow>();

        public const string UpdPrgId = CommonConst.UpdPrgID;

        protected Func<string, string> CustomFileName = (fileName) => fileName;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await InitControls();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("setNodeDisable");
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected void Copy(TableRow row, bool isPassword)
        {
            try
            {
                _jsRuntime.InvokeVoidAsync("copyText", $"copy-{(isPassword ? "password" : "url")}-{row.File.Name}");
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected void DownloadFile(string url, TableRow row)
        {
            try
            {
                if (row.File.SiyoKbn == CommonConst.ActiveSiyoKbn)
                    _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected async Task ShowDownloadHistory(TableRow row)
        {
            try
            {
                if (row.File != null && row.File.TotalDownload > 0)
                {
                    showDownloadHistory = true;
                    DownloadHistory = await _s3Service.GetDownloadHistory(row.File.EncryptedId);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task HandleConfirm()
        {
            try
            {
                showConfirm = false;
                StateHasChanged();
                if (isRemove)
                    await Remove();
                else
                    await ChangeFileStatus();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task ChangeFileStatus()
        {
            try
            {
                if (changeRow != null)
                {
                    await _s3Service.ChangeFileStatus(new UpdateFileStatusModel()
                    {
                        FileId = changeRow.File.EncryptedId,
                        UpdPrgID = CommonConst.UpdPrgID,
                        UpdSyainCd = UpdSyainCd
                    });
                    await RebindGrid(selectedNode?.Id);
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected async Task DownloadFileWithouPass(TableRow row)
        {
            try
            {
                if (row.IsFile && row.File.SiyoKbn == CommonConst.ActiveSiyoKbn)
                {
                    Loading = true;
                    var model = new DownloadModel()
                    {
                        FileId = row.File.EncryptedId,
                        Password = row.File.Password,
                        UpdSyainCd = UpdSyainCd,
                        UpdPrgID = CommonConst.UpdPrgID
                    };

                    var result = await _s3Service.DownloadFileAsync(model);
                    if (result != null)
                    {
                        var extension = Path.GetExtension(row.File.Name);
                        string myExportString = result.Content;
                        await _jsRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, extension.Replace(".", string.Empty).ToLower(), row.File.Name.Replace(extension, string.Empty) + "_");
                    }
                    Loading = false;
                }
            }
            catch (Exception e)
            {
                if (e is EntityNotFoundException)
                    _navigation.NavigateTo("/");
            }
        }
        protected void GetFileDetails(TableRow row)
        {
            try
            {
                row.IsExpanded = !row.IsExpanded;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected string GetMenuId(TableRow row)
        {
            try
            {
                if (!row.IsFile) return "folder-menu";
                else
                {
                    var fileExtension = Path.GetExtension(row.File.Name);
                    if (fileExtension == ".pdf")
                        return "pdf-file-menu";
                    else
                        return "file-menu";
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return string.Empty;
            }
        }

        protected void ChangeState(TableRow row)
        {
            try
            {
                changeRow = row;
                showConfirm = true;
                isRemove = false;
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task SelectionChanged(TreeViewNodeEventArgs e)
        {
            try
            {
                selectedNode = (TreeNode)e.NodeInfo.DataItem;

                if (selectedNode != null)
                {
                    await RebindGrid(selectedNode.Id);
                    RebindBreadcrumb();
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        private void RebindBreadcrumb()
        {
            try
            {
                breadcrumbs.Clear();
                if (TenantCode == 0)
                    breadcrumbs.Add(bucket);
                if (!string.IsNullOrEmpty(selectedNode.FullPath))
                    breadcrumbs.AddRange(selectedNode.FullPath.Trim('/').Split(CommonConst.DirectorySeparatorChar));
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void TreeMoveSelectionChanged(TreeViewNodeEventArgs e)
        {
            try
            {
                moveTo = (TreeNode)e.NodeInfo.DataItem;
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task MoveFolder()
        {
            try
            {
                if (moveFrom != null && moveTo != null && !IsNodeDisable(moveTo))
                {
                    Loading = true;
                    StateHasChanged();

                    try
                    {
                        showMove = false;
                        if (moveFrom.IsFile)
                        {
                            if (moveFrom.File.EncryptedFolderId != moveTo.Id)
                            {
                                await _s3Service.MoveFile(new MoveFileModel()
                                {
                                    UpdSyainCd = UpdSyainCd,
                                    UpdPrgID = CommonConst.UpdPrgID,
                                    FromId = moveFrom.File.EncryptedId,
                                    ToId = moveTo.Id
                                });
                                await InitControls();
                            }
                        }
                        else
                        {
                            if (moveFrom.Folder.EncryptedParentId != moveTo.Id)
                            {
                                await _s3Service.MoveFolder(new MoveFolderModel()
                                {
                                    FromId = moveFrom.Folder.EncryptedId,
                                    ToId = moveTo.Id,
                                    UpdPrgID = CommonConst.UpdPrgID,
                                    UpdSyainCd = UpdSyainCd
                                });
                                await InitControls();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        showError = true;
                        if (e is DuplicateException)
                            errorMsg = _lang["duplicate_error"];
                        else errorMsg = _lang["common_error"];
                    }
                    finally
                    {
                        Loading = false;
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void CloseMoveDialog()
        {
            try
            {
                showMove = false;
                moveTo = null;
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void ShowLoading(bool isShow)
        {
            try
            {
                Loading = isShow;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task CreateNewFolder()
        {
            try
            {
                if (selectedNode != null && !string.IsNullOrEmpty(folderName) && folderName.Length <= 50)
                {
                    showNewFolderPopup = false;
                    Loading = true;
                    StateHasChanged();

                    try
                    {
                        var tenantCd = selectedTenant != null ? selectedTenant.Code : TenantCode == 0 ? selectedNode.TenantCd : TenantCode;

                        await _s3Service.CreateNewFolder(new FolderModel()
                        {
                            FolderName = folderName,
                            ParentId = selectedNode.Id,
                            TenantCdSeq = tenantCd,
                            UpdSyainCd = UpdSyainCd,
                            UpdPrgID = CommonConst.UpdPrgID
                        });
                        await ReBuildSelectedNode();
                    }
                    catch (Exception e)
                    {
                        showError = true;
                        if (e is DuplicateException)
                            errorMsg = _lang["duplicate_error"];
                        else errorMsg = _lang["common_error"];
                    }
                    finally
                    {
                        folderName = string.Empty;
                        Loading = false;
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task Rename()
        {
            try
            {
                if (selectedNode != null && !string.IsNullOrEmpty(newName) && newName.Length <= 50)
                {
                    showRenamePopup = false;
                    Loading = true;
                    StateHasChanged();

                    try
                    {
                        if (selectedRow.IsFile)
                        {
                            await _s3Service.RenameFile(new RenameFileModel()
                            {
                                UpdSyainCd = UpdSyainCd,
                                UpdPrgID = CommonConst.UpdPrgID,
                                FileId = selectedRow.File.EncryptedId,
                                NewName = newName
                            });
                            await RebindGrid(selectedNode.Id);
                        }
                        else
                        {
                            await _s3Service.RenameFolder(new RenameFolderModel()
                            {
                                NewName = newName,
                                UpdPrgID = CommonConst.UpdPrgID,
                                UpdSyainCd = UpdSyainCd,
                                FolderId = selectedRow.Folder.EncryptedId
                            });
                            await ReBuildSelectedNode();
                        }
                    }
                    catch (Exception e)
                    {
                        showError = true;
                        if (e is DuplicateException)
                            errorMsg = _lang["duplicate_error"];
                        else errorMsg = _lang["common_error"];
                    }
                    finally
                    {
                        Loading = false;
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        private async Task ReBuildSelectedNode()
        {
            if (selectedNode != null)
            {
                selectedNode.Nodes.Clear();
                List<TreeNode> folders = await _s3Service.GetChild(selectedNode.Id, TenantCode, 0);
                BuildTree(folders, selectedNode);
                treeKey = Guid.NewGuid();
                treeMoveKey = Guid.NewGuid();
                StateHasChanged();
                tree.ExpandToNode(e => ((TreeNode)e.DataItem).Id == selectedNode.Id);
                tree.SelectNode(e => ((TreeNode)e.DataItem).Id == selectedNode.Id);
            }
        }
        protected void ShowUpload()
        {
            try
            {
                if (selectedNode != null)
                {
                    showUpload = true;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task CloseUpload()
        {
            try
            {
                showUpload = false;
                await RebindGrid(selectedNode.Id);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void ChangeSelectedNode(List<string> paths, int index)
        {
            try
            {
                var result = paths.GetRange(0, index + 1);
                var nodes = Tree.Nodes;
                TreeNode node = null;
                foreach (var path in result)
                {
                    node = nodes.FirstOrDefault(n => n.Name == path);
                    if (node != null)
                        nodes = node.Nodes;
                    else return;
                }

                if (node != null)
                    tree.SelectNode(n => ((TreeNode)n.DataItem).Id == node.Id);
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        private async Task RebindGrid(string folderId)
        {
            Rows.Clear();
            var fileTask = _s3Service.GetFilesAsync(folderId);
            var folderTask = _s3Service.GetFoldersAsync(folderId);
            await Task.WhenAll(fileTask, folderTask);
            var files = fileTask.Result;
            var folders = folderTask.Result;

            if (files != null)
                Rows.AddRange(files.Select(f => new TableRow() { File = f, IsFile = true }));
            if (folders != null)
                Rows.AddRange(folders.Select(f => new TableRow() { Folder = f, IsFile = false }));

            SortRows();

            StateHasChanged();
        }

        private void SortRows()
        {
            if (sortCondition != null)
            {
                if (sortCondition.SortDirection == SortEnum.Asc)
                {
                    Rows = Rows.OrderBy(sortCondition.OrderBy).ToList();
                }
                else
                {
                    Rows = Rows.OrderByDescending(sortCondition.OrderBy).ToList();
                }
            }
        }

        protected string GetSortingClass(string propName)
        {
            try
            {
                return sortCondition.PropertyName != propName ? string.Empty : sortCondition.SortDirection == SortEnum.Asc ? "fa fa-long-arrow-up" : sortCondition.SortDirection == SortEnum.Desc ? "fa fa-long-arrow-down" : string.Empty;
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return string.Empty;
            }
        }

        protected void UpdateSortingList(string propName, Func<TableRow, dynamic> func)
        {
            try
            {
                sortCondition.PropertyName = propName;
                sortCondition.OrderBy = func;
                if (propName == sortCondition.PropertyName)
                {
                    if (sortCondition.SortDirection == SortEnum.Asc)
                        sortCondition.SortDirection = SortEnum.Desc;
                    else if (sortCondition.SortDirection == SortEnum.Desc)
                        sortCondition.SortDirection = SortEnum.Asc;
                }

                SortRows();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void Execute()
        {
            try
            {
                showDownloadHistory = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected string GetDownloadFileUrl(string encryptedFileId)
        {
            try
            {
                return _s3Service.BuildDownloadUrl(_navigation.BaseUri, encryptedFileId);
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return string.Empty;
            }
        }
        protected enum ContextMenuCommand
        {
            CopyURL,
            Download,
            Rename,
            Move,
            Remove,
            Preview
        }

        protected void OpenFolder(TableRow row)
        {
            try
            {
                if (!row.IsFile)
                {
                    tree.SelectNode(e => ((TreeNode)e.DataItem).Id == row.Folder.EncryptedId);
                    tree.ExpandToNode(e => ((TreeNode)e.DataItem).Id == row.Folder.EncryptedId);
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected async Task OnContextMenu(ItemClickEventArgs args, ContextMenuCommand command)
        {
            try
            {
                var row = (TableRow)args.Data;
                if (row != null)
                {
                    switch (command)
                    {
                        case ContextMenuCommand.CopyURL:
                            Copy(row, false);
                            break;
                        case ContextMenuCommand.Download:
                            DownloadFile(GetDownloadFileUrl(row.File.EncryptedId), row);
                            break;
                        case ContextMenuCommand.Move:
                            showMove = true;
                            moveFrom = row;
                            DisabledNodes.Clear();
                            DisabledNodes.Add(row.IsFile ? row.File.EncryptedId : row.Folder.EncryptedId);
                            StateHasChanged();
                            break;
                        case ContextMenuCommand.Remove:
                            showConfirm = true;
                            removeRow = row;
                            isRemove = true;
                            StateHasChanged();
                            break;
                        case ContextMenuCommand.Rename:
                            selectedRow = row;
                            newName = row.Name;
                            showRenamePopup = true;
                            break;
                        case ContextMenuCommand.Preview:
                            var fileExtension = Path.GetExtension(row.File.Name);
                            if (fileExtension.ToLower() == ".pdf")
                            {
                                var model = new DownloadModel() { FileId = row.File.EncryptedId, Password = row.File.Password, UpdSyainCd = UpdSyainCd, UpdPrgID = CommonConst.UpdPrgID };
                                var result = await _s3Service.DownloadFileAsync(model);
                                await _jsRuntime.InvokeVoidAsync("filePreview.preview", result.Content);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task Remove()
        {
            try
            {
                if (removeRow != null)
                {
                    Loading = true;
                    StateHasChanged();
                    if (removeRow.IsFile)
                    {
                        await _s3Service.RemoveFile(removeRow.File.EncryptedId);
                        await RebindGrid(selectedNode.Id);
                    }
                    else
                    {
                        await _s3Service.RemoveFolder(removeRow.Folder.EncryptedId);
                        await ReBuildSelectedNode();
                    }

                    Loading = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        private List<string> GetAllNodesExpanded(IEnumerable<TreeNode> nodes, DxTreeView tree)
        {
            var result = new List<string>();
            GetAllNodesExpanded(nodes, tree, result);
            return result;
        }

        private void GetAllNodesExpanded(IEnumerable<TreeNode> nodes, DxTreeView tree, List<string> result)
        {
            if (nodes != null && nodes.Any())
            {
                foreach (var n in nodes)
                {
                    if (tree.GetNodeExpanded(e => ((TreeNode)e.DataItem).Id == n.Id))
                    {
                        result.Add(n.Id);
                        GetAllNodesExpanded(n.Nodes, tree, result);
                    }
                }
            }
        }

        private async Task InitControls()
        {
            // Get all file paths
            Loading = true;
            StateHasChanged();

            var treeExpandedKeys = new List<string>();
            var treeMoveExpanedKeys = new List<string>();
            if (tree != null && tree.Data != null)
                treeExpandedKeys = GetAllNodesExpanded(tree.Data as IEnumerable<TreeNode>, tree);
            if (treeMove != null && treeMove.Data != null)
                treeMoveExpanedKeys = GetAllNodesExpanded(treeMove.Data as IEnumerable<TreeNode>, treeMove);

            await InitTree();

            StateHasChanged();

            var firstNode = Tree.Nodes.FirstOrDefault();
            if (firstNode != null)
            {
                selectedNode = firstNode;
                StateHasChanged();
                tree.SelectNode(e => ((TreeNode)e.DataItem).Id == firstNode.Id);
                tree.SetNodeExpanded(e => ((TreeNode)e.DataItem).Id == firstNode.Id, true);
            }

            if (treeExpandedKeys.Any())
                SetExpanedNodes(tree, treeExpandedKeys);
            if (treeMoveExpanedKeys.Any())
                SetExpanedNodes(treeMove, treeMoveExpanedKeys);

            tenants = await _s3Service.GetTenants();

            Loading = false;
            StateHasChanged();
        }

        protected bool IsSelectedRootNode()
        {
            try
            {
                return selectedNode.Id == Encryptor.Encrypt(0.ToString());
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return false;
            }
        }

        private async Task InitTree()
        {
            Tree.Nodes.Clear();
            TreeMove.Nodes.Clear();

            treeKey = Guid.NewGuid();
            treeMoveKey = Guid.NewGuid();

            List<TreeNode> folders = await _s3Service.GetChild(string.Empty, TenantCode, 0);

            IEnumerable<TreeNode> fs;
            if (TenantCode != 0)
                fs = folders.Where(e => e.ParentId == Encryptor.Encrypt(0.ToString()));
            else
            {
                fs = folders.Where(e => e.ParentId == Encryptor.Encrypt((-1).ToString()));
                bucket = fs?.FirstOrDefault()?.Name;
            }

            foreach (var folder in fs)
            {
                var treeNode = new TreeNode()
                {
                    Id = folder.Id,
                    FullPath = TenantCode == 0 ? string.Empty : folder.FullPath = folder.Name,
                    HasChilren = folder.HasChilren,
                    Name = folder.Name,
                    ParentId = folder.ParentId,
                    TenantCd = folder.TenantCd
                };
                BuildTree(folders, treeNode);
                Tree.Nodes.Add(treeNode);
                TreeMove.Nodes.Add(treeNode);
            }
        }

        private void SetExpanedNodes(DxTreeView tree, List<string> expandedKeys)
        {
            foreach (var k in expandedKeys)
            {
                tree.SetNodeExpanded(e => ((TreeNode)e.DataItem).Id == k, true);
            }
        }

        protected void BuildTree(List<TreeNode> nodes, TreeNode parentNode)
        {
            try
            {
                foreach (var node in nodes)
                {
                    if (node.ParentId == parentNode.Id)
                    {
                        node.FullPath += (parentNode.FullPath + CommonConst.DirectorySeparatorChar + node.Name).Trim('/');
                        parentNode.Nodes.Add(node);
                        BuildTree(nodes, node);
                    }
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void CancelDisabled(TreeViewNodeCancelEventArgs e)
        {
            try
            {
                e.Cancel = IsNodeDisable((TreeNode)e.NodeInfo.DataItem);
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected bool IsNodeDisable(TreeNode node)
        {
            try
            {
                return DisabledNodes.Contains(node.Id) ||
                (node.TenantCd != 0 && node.TenantCd != selectedNode.TenantCd) ||
                node.Id == (selectedRow.IsFile ? selectedRow.File.EncryptedId : selectedRow.Folder.EncryptedId);
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return true;
            }
        }

        protected string GetFileIcon(S3File file)
        {
            try
            {
                return _s3Service.GetFileIcon(file.FileType);
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return string.Empty;
            }
        }
        protected void OnErrorOccur(Exception e)
        {
            OnError.Invoke(e);
        }

        protected void ShowNewFolderForm()
        {
            try
            {
                if (selectedNode != null)
                {
                    showNewFolderPopup = true;
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
    }
}
