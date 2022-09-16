using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class HaitaModelCheck
    {
        public string TableName { get; set; }
        public List<PrimaryKeyToCheck> PrimaryKeys { get; set; }
        public string CurrentUpdYmdTime { get; set; }
    }

    public class PrimaryKeyToCheck
    {
        public string PrimaryKey { get; set; }
        public string Value { get; set; }
    }
}
