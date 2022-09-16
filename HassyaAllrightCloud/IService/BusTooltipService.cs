using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;

namespace HassyaAllrightCloud.IService
{
    public interface ILoadBusTooltipListService
    {
        LoadBusTooltip GetBustooltip(int compid);
        LoadBusTooltip GetStafftooltip(int compid);
    }
    public class BusTooltipService : ILoadBusTooltipListService
    {
        private readonly KobodbContext _dbContext;

        public BusTooltipService(KobodbContext context)
        {
            _dbContext = context;
        }
        
        /// <summary>
        /// load tooltip data
        /// </summary>
        /// <param name="CurrentCompanyCdSeq"></param>
        /// <returns></returns>
        public LoadBusTooltip GetBustooltip(int CurrentCompanyCdSeq)
        {
            return (from s in _dbContext.TkmKasSet
                    where s.CompanyCdSeq == CurrentCompanyCdSeq 
                    select new LoadBusTooltip
                    {
                        SyaSenMjPtnKbn1 = s.SyaSenMjPtnKbn1,
                        SyaSenMjPtnCol1 = s.SyaSenMjPtnCol1,
                        SyaSenMjPtnKbn2 = s.SyaSenMjPtnKbn2,
                        SyaSenMjPtnCol2 = s.SyaSenMjPtnCol2,
                        SyaSenMjPtnKbn3 = s.SyaSenMjPtnKbn3,
                        SyaSenMjPtnCol3 = s.SyaSenMjPtnCol3,
                        SyaSenMjPtnKbn4 = s.SyaSenMjPtnKbn4,
                        SyaSenMjPtnCol4 = s.SyaSenMjPtnCol4,
                        SyaSenMjPtnKbn5 = s.SyaSenMjPtnKbn5,
                        SyaSenMjPtnCol5 = s.SyaSenMjPtnCol5,
                        SyaSenInfoPtnKbn1 = s.SyaSenInfoPtnKbn1,
                        SyaSenInfoPtnCol1 = s.SyaSenInfoPtnCol1,
                        SyaSenInfoPtnKbn2 = s.SyaSenInfoPtnKbn2,
                        SyaSenInfoPtnCol2 = s.SyaSenInfoPtnCol2,
                        SyaSenInfoPtnKbn3 = s.SyaSenInfoPtnKbn3,
                        SyaSenInfoPtnCol3 = s.SyaSenInfoPtnCol3,
                        SyaSenInfoPtnKbn4 = s.SyaSenInfoPtnKbn4,
                        SyaSenInfoPtnCol4 = s.SyaSenInfoPtnCol4,
                        SyaSenInfoPtnKbn5 = s.SyaSenInfoPtnKbn5,
                        SyaSenInfoPtnCol5 = s.SyaSenInfoPtnCol5,
                        SyaSenInfoPtnKbn6 = s.SyaSenInfoPtnKbn6,
                        SyaSenInfoPtnCol6 = s.SyaSenInfoPtnCol6,
                        SyaSenInfoPtnKbn7 = s.SyaSenInfoPtnKbn7,
                        SyaSenInfoPtnCol7 = s.SyaSenInfoPtnCol7,
                        SyaSenInfoPtnKbn8 = s.SyaSenInfoPtnKbn8,
                        SyaSenInfoPtnCol8 = s.SyaSenInfoPtnCol8,
                        SyaSenInfoPtnKbn9 = s.SyaSenInfoPtnKbn9,
                        SyaSenInfoPtnCol9 = s.SyaSenInfoPtnCol9,
                        SyaSenInfoPtnKbn10 = s.SyaSenInfoPtnKbn10,
                        SyaSenInfoPtnCol10 = s.SyaSenInfoPtnCol10,
                        SyaSenInfoPtnKbn11 = s.SyaSenInfoPtnKbn11,
                        SyaSenInfoPtnCol11 = s.SyaSenInfoPtnCol11,
                        SyaSenInfoPtnKbn12 = s.SyaSenInfoPtnKbn12,
                        SyaSenInfoPtnCol12 = s.SyaSenInfoPtnCol12,
                        SyaSenInfoPtnKbn13 = s.SyaSenInfoPtnKbn13,
                        SyaSenInfoPtnCol13 = s.SyaSenInfoPtnCol13,
                        SyaSenInfoPtnKbn14 = s.SyaSenInfoPtnKbn14,
                        SyaSenInfoPtnCol14 = s.SyaSenInfoPtnCol14,
                        SyaSenInfoPtnKbn15 = s.SyaSenInfoPtnKbn15,
                        SyaSenInfoPtnCol15 = s.SyaSenInfoPtnCol15,
                        UriKbn = s.UriKbn,
                    }).FirstOrDefault();
        }
        public LoadBusTooltip GetStafftooltip(int CurrentCompanyCdSeq)
        {
            return (from s in _dbContext.TkmKasSet
                    where s.CompanyCdSeq == CurrentCompanyCdSeq 
                    select new LoadBusTooltip
                    {
                        SyaSenMjPtnKbn1 = s.JyoSenMjPtnKbn1,
                        SyaSenMjPtnCol1 = s.JyoSenMjPtnCol1,
                        SyaSenMjPtnKbn2 = s.JyoSenMjPtnKbn2,
                        SyaSenMjPtnCol2 = s.JyoSenMjPtnCol2,
                        SyaSenMjPtnKbn3 = s.JyoSenMjPtnKbn3,
                        SyaSenMjPtnCol3 = s.JyoSenMjPtnCol3,
                        SyaSenMjPtnKbn4 = s.JyoSenMjPtnKbn4,
                        SyaSenMjPtnCol4 = s.JyoSenMjPtnCol4,
                        SyaSenMjPtnKbn5 = s.JyoSenMjPtnKbn5,
                        SyaSenMjPtnCol5 = s.JyoSenMjPtnCol5,
                        SyaSenInfoPtnKbn1 = s.JyoSenInfoPtnKbn1,
                        SyaSenInfoPtnCol1 = s.JyoSenInfoPtnCol1,
                        SyaSenInfoPtnKbn2 = s.JyoSenInfoPtnKbn2,
                        SyaSenInfoPtnCol2 = s.JyoSenInfoPtnCol2,
                        SyaSenInfoPtnKbn3 = s.JyoSenInfoPtnKbn3,
                        SyaSenInfoPtnCol3 = s.JyoSenInfoPtnCol3,
                        SyaSenInfoPtnKbn4 = s.JyoSenInfoPtnKbn4,
                        SyaSenInfoPtnCol4 = s.JyoSenInfoPtnCol4,
                        SyaSenInfoPtnKbn5 = s.JyoSenInfoPtnKbn5,
                        SyaSenInfoPtnCol5 = s.JyoSenInfoPtnCol5,
                        SyaSenInfoPtnKbn6 = s.JyoSenInfoPtnKbn6,
                        SyaSenInfoPtnCol6 = s.JyoSenInfoPtnCol6,
                        SyaSenInfoPtnKbn7 = s.JyoSenInfoPtnKbn7,
                        SyaSenInfoPtnCol7 = s.JyoSenInfoPtnCol7,
                        SyaSenInfoPtnKbn8 = s.JyoSenInfoPtnKbn8,
                        SyaSenInfoPtnCol8 = s.JyoSenInfoPtnCol8,
                        SyaSenInfoPtnKbn9 = s.JyoSenInfoPtnKbn9,
                        SyaSenInfoPtnCol9 = s.JyoSenInfoPtnCol9,
                        SyaSenInfoPtnKbn10 = s.JyoSenInfoPtnKbn10,
                        SyaSenInfoPtnCol10 = s.JyoSenInfoPtnCol10,
                        SyaSenInfoPtnKbn11 = s.JyoSenInfoPtnKbn11,
                        SyaSenInfoPtnCol11 = s.JyoSenInfoPtnCol11,
                        SyaSenInfoPtnKbn12 = s.JyoSenInfoPtnKbn12,
                        SyaSenInfoPtnCol12 = s.JyoSenInfoPtnCol12,
                        SyaSenInfoPtnKbn13 = s.JyoSenInfoPtnKbn13,
                        SyaSenInfoPtnCol13 = s.JyoSenInfoPtnCol13,
                        SyaSenInfoPtnKbn14 = s.JyoSenInfoPtnKbn14,
                        SyaSenInfoPtnCol14 = s.JyoSenInfoPtnCol14,
                        SyaSenInfoPtnKbn15 = s.JyoSenInfoPtnKbn15,
                        SyaSenInfoPtnCol15 = s.JyoSenInfoPtnCol15,
                        UriKbn = s.UriKbn,
                    }).FirstOrDefault();
        }
    }
}
