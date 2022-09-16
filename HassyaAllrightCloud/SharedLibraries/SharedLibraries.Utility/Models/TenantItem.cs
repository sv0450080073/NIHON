namespace SharedLibraries.Utility.Models
{
    public class TenantItem
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public string DisplayName { get { return $"{Code:00000}:{Name}"; } }
    }
}
