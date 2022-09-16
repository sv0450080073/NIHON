using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetHaiShaDataListQuery : IRequest<List<TkdHaisha>>
    {
        public BusAllocationDataCopyPaste BusAllocationDataCopyPaste { get; set; }
        public class Handler : IRequestHandler<GetHaiShaDataListQuery, List<TkdHaisha>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdHaisha>> Handle(GetHaiShaDataListQuery request, CancellationToken cancellationToken)
            {
              
                try
                {
                    var dataSource = request.BusAllocationDataCopyPaste.DataSourceItem;
                    var dataUpdateList = request.BusAllocationDataCopyPaste.DataUpdateList;
                    List<TkdHaisha> tkdHaishaList = new List<TkdHaisha>();
                    foreach (var item in dataUpdateList)
                    {
                        TkdHaisha tkdHaisha = _context.TkdHaisha.FirstOrDefault(e => e.UkeNo == item.YYKSHO_UkeNo
                                                                           && e.UnkRen == item.HAISHA_UnkRen
                                                                           && e.SyaSyuRen == item.HAISHA_SyaSyuRen
                                                                           && e.TeiDanNo == item.HAISHA_TeiDanNo
                                                                           && e.BunkRen == item.HAISHA_BunkRen);
                        SetTkdHaishaData(ref tkdHaisha, dataSource);
                        tkdHaishaList.Add(tkdHaisha);
                    }
                    return tkdHaishaList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            private void SetTkdHaishaData(ref TkdHaisha tkdHaisha, BusAllocationDataGrid busAllocationDataGrid)
            {
                if (tkdHaisha != null && busAllocationDataGrid != null)
                {
                    var data = busAllocationDataGrid;
                    tkdHaisha.HenKai++;
                    tkdHaisha.GoSya = data.HAISHA_GoSya;
                    tkdHaisha.SyuEigCdSeq = data.HAISHA_SyuEigCdSeq;
                    tkdHaisha.KikEigSeq = data.HAISHA_KikEigSeq;
                    tkdHaisha.HaiSsryCdSeq = data.HAISHA_HaiSSryCdSeq; 
                    tkdHaisha.KssyaRseq = data.HAISHA_HaiSSryCdSeq;
                    tkdHaisha.DanTaNm2 = data.HAISHA_DanTaNm2;
                    tkdHaisha.IkMapCdSeq = data.HAISHA_IkMapCdSeq;
                    tkdHaisha.IkNm = data.HAISHA_IkNm;
                    tkdHaisha.SyuKoYmd = data.HAISHA_SyuKoYmd;
                    tkdHaisha.SyuKoTime = data.HAISHA_SyuKoTime;
                    tkdHaisha.SyuPaTime = data.HAISHA_SyuPaTime;
                    tkdHaisha.HaiSymd = data.HAISHA_HaiSYmd;
                    tkdHaisha.HaiStime = data.HAISHA_HaiSTime;
                    tkdHaisha.HaiScdSeq = data.HAISHA_HaiSCdSeq;
                    tkdHaisha.HaiSnm = data.HAISHA_HaiSNm;
                    tkdHaisha.HaiSjyus1 = data.HAISHA_HaiSJyus1;
                    tkdHaisha.HaiSjyus2 = data.HAISHA_HaiSJyus2;
                    tkdHaisha.HaiSkigou = data.HAISHA_HaiSKigou;
                    tkdHaisha.HaiSkouKcdSeq = data.HAISHA_HaiSKouKCdSeq;
                    tkdHaisha.HaiSkouKnm = data.HAISHA_HaiSKouKNm;
                    tkdHaisha.HaiSbinCdSeq = data.HAISHA_HaiSBinCdSeq;
                    tkdHaisha.HaiSbinNm = data.HAISHA_HaiSBinNm;
                    tkdHaisha.HaiSsetTime = data.HAISHA_HaiSSetTime;
                    tkdHaisha.KikYmd = data.HAISHA_KikYmd;
                    tkdHaisha.KikTime = data.HAISHA_KikTime;
                    tkdHaisha.TouYmd = data.HAISHA_TouYmd;
                    tkdHaisha.TouChTime = data.HAISHA_TouChTime;
                    tkdHaisha.TouCdSeq = data.HAISHA_TouCdSeq;
                    tkdHaisha.TouNm = data.HAISHA_TouNm;
                    tkdHaisha.TouJyusyo1 = data.HAISHA_TouJyusyo1;
                    tkdHaisha.TouJyusyo2 = data.HAISHA_TouJyusyo2;
                    tkdHaisha.TouKigou = data.HAISHA_TouKigou;
                    tkdHaisha.TouKouKcdSeq = data.HAISHA_TouKouKCdSeq;
                    tkdHaisha.TouSkouKnm = data.HAISHA_TouSKouKNm;
                    tkdHaisha.TouBinCdSeq = data.HAISHA_TouBinCdSeq;
                    tkdHaisha.TouBinNm = data.HAISHA_TouBinNm;
                    tkdHaisha.TouSetTime = data.HAISHA_TouSetTime;
                    tkdHaisha.JyoSyaJin = data.HAISHA_JyoSyaJin;
                    tkdHaisha.PlusJin = data.HAISHA_PlusJin;
                    tkdHaisha.DrvJin = data.HAISHA_DrvJin;
                    tkdHaisha.GuiSu = data.HAISHA_GuiSu;
                    tkdHaisha.OthJinKbn1 = data.HAISHA_OthJinKbn1;
                    tkdHaisha.OthJin1 = data.HAISHA_OthJin1;
                    tkdHaisha.OthJinKbn2 = data.HAISHA_OthJinKbn2;
                    tkdHaisha.OthJin2 = data.HAISHA_OthJin2;
                    tkdHaisha.CustomItems1 = data.HAISHA_CustomItems1;
                    tkdHaisha.CustomItems2 = data.HAISHA_CustomItems2;
                    tkdHaisha.CustomItems3 = data.HAISHA_CustomItems3;
                    tkdHaisha.CustomItems4 = data.HAISHA_CustomItems4;
                    tkdHaisha.CustomItems5 = data.HAISHA_CustomItems5;
                    tkdHaisha.CustomItems6 = data.HAISHA_CustomItems6;
                    tkdHaisha.CustomItems7 = data.HAISHA_CustomItems7;
                    tkdHaisha.CustomItems8 = data.HAISHA_CustomItems8;
                    tkdHaisha.CustomItems9 = data.HAISHA_CustomItems9;
                    tkdHaisha.CustomItems10 = data.HAISHA_CustomItems10;
                    tkdHaisha.CustomItems11 = data.HAISHA_CustomItems11;
                    tkdHaisha.CustomItems12 = data.HAISHA_CustomItems12;
                    tkdHaisha.CustomItems13 = data.HAISHA_CustomItems13;
                    tkdHaisha.CustomItems14 = data.HAISHA_CustomItems14;
                    tkdHaisha.CustomItems15 = data.HAISHA_CustomItems15;
                    tkdHaisha.CustomItems16 = data.HAISHA_CustomItems16;
                    tkdHaisha.CustomItems17 = data.HAISHA_CustomItems17;
                    tkdHaisha.CustomItems18 = data.HAISHA_CustomItems18;
                    tkdHaisha.CustomItems19 = data.HAISHA_CustomItems19;
                    tkdHaisha.CustomItems20 = data.HAISHA_CustomItems20;
                    tkdHaisha.Kskbn = 2;
                    if (data.screenKbn == 0)
                    {
                        tkdHaisha.HaiSkbn = 2;
                    }
                    /*tkdHaisha.HaiIkbn = CheckHaiin(data.HAISHA_DrvJin, data.HAISHA_GuiSu, tkdHaisha.UkeNo,
                        tkdHaisha.UnkRen, tkdHaisha.TeiDanNo, tkdHaisha.BunkRen,
                        tkdHaisha.HaiSymd);*/
                    tkdHaisha.PlatNo = data.HAISHA_PlatNo;
                    tkdHaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdHaisha.UpdTime = DateTime.Now.ToString("HHmm");
                    tkdHaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdHaisha.UpdPrgId = "KU0600";
                }
            }
            private byte CheckHaiin(short drvJin, short guiSu, string ukeno, short unkRen, short teidanNo, short bunkRen, string HaiSha_HaiSYmd)
            {
                var tenantCdSeq = new ClaimModel().TenantID;
                byte result = 0;
                int[] positionArr = new int[] { 1, 2, 3, 4 };
                var haiinDataList = _context.TkdHaiin.Where(x => x.UkeNo == ukeno
                                                        && x.UnkRen == unkRen
                                                        && x.BunkRen == bunkRen
                                                        && x.TeiDanNo == teidanNo).ToList();
                if (haiinDataList.Count == 0) //not emmplooy
                {
                    result = 1;
                }
                else
                {
                    var staffList = (from Haiin in _context.TkdHaiin
                                     join KyoShe in _context.VpmKyoShe
                                     on new { H1 = Haiin.SyainCdSeq, H2 = true, H3 = true }
                                     equals new { H1 = KyoShe.SyainCdSeq, H2 = String.Compare(KyoShe.StaYmd, HaiSha_HaiSYmd) <= 0, H3 = String.Compare(KyoShe.EndYmd, HaiSha_HaiSYmd) >= 0 }
                                     into KyoShe_join
                                     from KyoShe in KyoShe_join.DefaultIfEmpty()
                                     join Syokum in _context.VpmSyokum
                                     on KyoShe.SyokumuCdSeq equals Syokum.SyokumuCdSeq
                                     into Syokum_join
                                     from Syokum in Syokum_join.DefaultIfEmpty()
                                     where Syokum.SiyoKbn == 1
                                     && Syokum.TenantCdSeq == tenantCdSeq
                                     && Haiin.UkeNo == ukeno
                                     && Haiin.UnkRen == unkRen
                                     && Haiin.BunkRen == bunkRen
                                     && Haiin.TeiDanNo == teidanNo
                                     && positionArr.Contains(Syokum.SyokumuKbn)
                                     select new StaffPosition()
                                     {
                                         SyainCdSeq = Haiin.SyainCdSeq,
                                         SyokumuCd = Syokum.SyokumuCd
                                     }).ToList();
                    int countDriver = staffList.Where(x => x.SyokumuCd == 1 || x.SyokumuCd == 2).Count();
                    int countGui = staffList.Where(x => x.SyokumuCd == 3 || x.SyokumuCd == 4).Count();
                    if (drvJin == countDriver && guiSu == countGui)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 3;
                    }
                }
                return result;
            }
        }
    }
}
