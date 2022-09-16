using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetAssignedEmployeeByUkenoQuery : IRequest<List<AssignedEmployee>>
    {
        public string Ukeno { get; set; } = "";
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }                 
        public class Handler : IRequestHandler<GetAssignedEmployeeByUkenoQuery, List<AssignedEmployee>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<AssignedEmployee>> Handle(GetAssignedEmployeeByUkenoQuery request, CancellationToken cancellationToken)
            {
                var result = new List<AssignedEmployee>();
                int[] arrPosition = new int[4] { 1, 2, 3, 4 };
                try
                {
                    result = (from HAIIN in _context.TkdHaiin
                              join HAISHA in _context.TkdHaisha
                              on new { H1 = HAIIN.UkeNo, H2 = HAIIN.UnkRen, H3 = HAIIN.TeiDanNo, H4 = HAIIN.BunkRen, H5 = 1 }
                              equals new { H1 = HAISHA.UkeNo, H2 = HAISHA.UnkRen, H3 = HAISHA.TeiDanNo, H4 = HAISHA.BunkRen, H5 = (int)HAISHA.SiyoKbn }
                              into HAISHA_join
                              from HAISHA in HAISHA_join.DefaultIfEmpty()
                              join SYAIN in _context.VpmSyain
                              on HAIIN.SyainCdSeq equals SYAIN.SyainCdSeq
                              into SYAIN_join
                              from SYAIN in SYAIN_join.DefaultIfEmpty()
                              join KYOSHE in _context.VpmKyoShe
                              on HAIIN.SyainCdSeq equals KYOSHE.SyainCdSeq
                              into KYOSHE_join
                              from KYOSHE in KYOSHE_join.DefaultIfEmpty()
                              join SYOKUM in _context.VpmSyokum
                              on KYOSHE.SyokumuCdSeq equals SYOKUM.SyokumuCdSeq
                              into SYOKUM_join
                              from SYOKUM in SYOKUM_join.DefaultIfEmpty()
                              join EIGYOS in _context.VpmEigyos
                              on KYOSHE.EigyoCdSeq equals EIGYOS.EigyoCdSeq
                              into EIGYOS_join
                              from EIGYOS in EIGYOS_join.DefaultIfEmpty()
                              where HAIIN.UkeNo == request.Ukeno
                              && HAIIN.UnkRen == request.UnkRen
                              && HAIIN.TeiDanNo == request.TeiDanNo
                              && HAIIN.BunkRen == request.BunkRen
                              && HAIIN.SiyoKbn == 1
                              && arrPosition.Contains(SYOKUM.SyokumuKbn)
                              && String.Compare(KYOSHE.StaYmd, HAISHA.HaiSymd) <= 0
                              && String.Compare(KYOSHE.EndYmd, HAISHA.HaiSymd) >= 0
                              orderby HAIIN.HaiInRen
                              select new AssignedEmployee()
                              {
                                  HAIIN_UkeNo= HAIIN.UkeNo,
                                  HAIIN_UnkRen = HAIIN.UnkRen,
                                  HAIIN_TeiDanNo = HAIIN.TeiDanNo,
                                  HAIIN_BunkRen = HAIIN.BunkRen,
                                  HAIIN_HaiInRen = HAIIN.HaiInRen,
                                  HAIIN_SyainCdSeq= HAIIN.SyainCdSeq,
                                  HAIIN_Syukinbasy= HAIIN.Syukinbasy,
                                  HAIIN_TaiknBasy = HAIIN.TaiknBasy,
                                  EIGYOS_EigyoCdSeq = EIGYOS.EigyoCdSeq,
                                  EIGYOS_EigyoCd = EIGYOS.EigyoCd,
                                  EIGYOS_EigyoNm = EIGYOS.EigyoNm,
                                  SYAIN_SyainCd = SYAIN.SyainCd,
                                  SYAIN_SyainNm = SYAIN.SyainNm
                              }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }


    }
}
