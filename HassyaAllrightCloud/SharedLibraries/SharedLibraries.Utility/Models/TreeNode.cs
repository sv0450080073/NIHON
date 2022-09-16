using System.Collections.Generic;

namespace SharedLibraries.Utility.Models
{
    public class Tree
    {
        public List<TreeNode> Nodes { get; set; } = new List<TreeNode>();
    }
    public class TreeNode
    {
        public string Name { get; set; }
        public bool HasChilren { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string FullPath { get; set; }
        public int TenantCd { get; set; }
        public List<TreeNode> Nodes { get; set; } = new List<TreeNode>();
    }
}
