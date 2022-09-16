using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class BusTypeListHelper
    {
        /// <summary>
        /// </summary>
        /// <example>
        ///  Input : List Branch company 1 - branchCd 1 
        ///                      company 1 - branchCd 3
        ///                      company 2 - branchCd 2
        ///                      
        ///  OutPut :List Branch company 1 - branchCd 1 
        ///                      company 2 - branchCd 2
        ///                      company 1 - branchCd 3
        ///                     
        /// </example>
        /// <param name="source">Source branch list </param>
        /// <returns>Branch list order by branchCd </returns>
        public static List<LoadSaleBranch> OrderByBranchByBranchCd(List<LoadSaleBranch> source)
        {
            if(source!=null && source.Any())
            {
                return source.OrderBy(x => x.EigyoCd).ToList();
            }
            return source;
        }
    }
}
