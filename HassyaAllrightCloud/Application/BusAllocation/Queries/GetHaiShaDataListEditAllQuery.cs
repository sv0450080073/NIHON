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
    public class GetHaiShaDataListEditAllQuery : IRequest<List<TkdHaisha>>
    {
        public BusAllocationDataEditSimultaneously BusAllocationDataEditSimultaneously { get; set; }
        public class Handler : IRequestHandler<GetHaiShaDataListEditAllQuery, List<TkdHaisha>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<TkdHaisha>> Handle(GetHaiShaDataListEditAllQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var dataSource = request.BusAllocationDataEditSimultaneously.DataSourceItem;
                    var dataUpdateList = request.BusAllocationDataEditSimultaneously.DataUpdateList;
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
            private void SetTkdHaishaData(ref TkdHaisha tkdHaisha, BusAllocationDataUpdateAll busAllocationDataUpdateAll)
            {
                if (tkdHaisha != null && busAllocationDataUpdateAll != null)
                {
                    var data = busAllocationDataUpdateAll;
                    tkdHaisha.HenKai++;
                    if (CheckStringValue(data.DanTaNm2))
                    {
                        tkdHaisha.DanTaNm2 = data.DanTaNm2;
                    }
                    if (data.TPM_CodeKbDataKenCD != null && data.TPM_CodeKbDataKenCD.BasyoBasyoMapCdSeq != 0)
                    {
                        tkdHaisha.IkMapCdSeq = data.TPM_CodeKbDataKenCD.BasyoBasyoMapCdSeq;
                    }
                    if (CheckStringValue(data.IkNm))
                    {
                        tkdHaisha.IkNm = data.IkNm;
                    }
                    if (data.BranchChartData != null && data.BranchChartData.EigyoCdSeq != 0)
                    {
                        tkdHaisha.SyuEigCdSeq = data.BranchChartData.EigyoCdSeq;
                    }
                    if (data.BranchChartDataKik != null && data.BranchChartDataKik.EigyoCdSeq != 0)
                    {
                        tkdHaisha.KikEigSeq = data.BranchChartDataKik.EigyoCdSeq;
                    }
                    if (!data.DisableSyukoDateTime)
                    {
                        tkdHaisha.SyuKoYmd = ParseDateToString(data.SyuKoYmd);
                        tkdHaisha.SyuKoTime = data.SyuKoTime;
                    }
                    if (!data.DisableHaiSDateTime)
                    {
                        tkdHaisha.HaiSymd = ParseDateToString(data.HaiSYmd);
                        tkdHaisha.HaiStime = data.HaiSTime;
                    }
                    if (!data.DisableSyaPaTime)
                    {
                        tkdHaisha.SyuPaTime = data.SyuPaTime;
                    }
                    if (!data.DisableTouDateTime)
                    {
                        tkdHaisha.TouYmd = ParseDateToString(data.TouYmd);
                        tkdHaisha.TouChTime = data.TouChTime;
                    }
                    if (!data.DisableKikDateTime)
                    {
                        tkdHaisha.KikYmd = ParseDateToString(data.KikYmd);
                        tkdHaisha.KikTime = data.KikTime;
                    }
                    if (data.TPM_CodeKbDataBunruiCDStart.HaiChiHaiSCdSeq != 0)
                    {
                        tkdHaisha.HaiScdSeq = data.TPM_CodeKbDataBunruiCDStart.HaiChiHaiSCdSeq;                                             
                    }
                    if (CheckStringValue(data.HaiSNm))
                    {
                        tkdHaisha.HaiSnm = data.HaiSNm;
                    }
                    if (CheckStringValue(data.HaiSJyus1))
                    {
                        tkdHaisha.HaiSjyus1 = data.HaiSJyus1;
                    }
                    if (CheckStringValue(data.HaiSJyus2))
                    {
                        tkdHaisha.HaiSjyus2 = data.HaiSJyus2;
                    }
                    if (CheckStringValue(data.HaiSKigou))
                    {
                        tkdHaisha.HaiSkigou = data.HaiSKigou;
                    }
                    if (data.TPM_CodeKbDataBunruiCDEnd.HaiChiHaiSCdSeq != 0)
                    {
                        tkdHaisha.TouCdSeq = data.TPM_CodeKbDataBunruiCDEnd.HaiChiHaiSCdSeq;
                    }
                    if (CheckStringValue(data.TouNm))
                    {
                        tkdHaisha.TouNm = data.TouNm;
                    }
                    if (CheckStringValue(data.TouJyusyo1))
                    {
                        tkdHaisha.TouJyusyo1 = data.TouJyusyo1;
                    }
                    if (CheckStringValue(data.TouJyusyo2))
                    {
                        tkdHaisha.TouJyusyo2 = data.TouJyusyo2;
                    }
                    if (CheckStringValue(data.TouKigou))
                    {
                        tkdHaisha.TouKigou = data.TouKigou;
                    }                  
                    if(!data.DisableHaiSSetTime)
                    {
                        if (data.TPM_CodeKbDataDepotStart != null && data.TPM_CodeKbDataDepotStart.KoutuKoukCdSeq != 0)
                        {
                            tkdHaisha.HaiSkouKcdSeq = data.TPM_CodeKbDataDepotStart.KoutuKoukCdSeq;
                            tkdHaisha.HaiSkouKnm = data.TPM_CodeKbDataDepotStart.KoutuRyakuNm;
                            tkdHaisha.HaiSbinCdSeq = data.TPM_CodeKbDataDepotStart.BinBinCdSeq;
                        }
                        if (CheckStringValue(data.HaisBinNm))
                        {
                            tkdHaisha.HaiSbinNm = data.HaisBinNm;
                        }
                        if (CheckStringValue(data.HaiSSetTime))
                        {
                            tkdHaisha.HaiSsetTime = data.HaiSSetTime;
                        }
                    }                                                    
                    if (!data.DisableTouSetTime)
                    {
                        if (data.TPM_CodeKbDataDepotEnd != null && data.TPM_CodeKbDataDepotEnd.KoutuKoukCdSeq != 0)
                        {
                            tkdHaisha.TouKouKcdSeq = data.TPM_CodeKbDataDepotEnd.KoutuKoukCdSeq;
                            tkdHaisha.TouSkouKnm = data.TPM_CodeKbDataDepotEnd.KoutuRyakuNm;
                            tkdHaisha.TouBinCdSeq = data.TPM_CodeKbDataDepotEnd.BinBinCdSeq;
                        }
                        if (CheckStringValue(data.TouSetTime))
                        {
                            tkdHaisha.TouSetTime = data.TouSetTime;
                        }
                        if (CheckStringValue(data.TouBinNm))
                        {
                            tkdHaisha.TouBinNm = data.TouBinNm;
                        }
                    }
                    
                    tkdHaisha.Kskbn = 2;
                    if (data.screenKbn == 0)
                    {
                        tkdHaisha.HaiSkbn = 2;
                    }
                    /*tkdHaisha.HaiIkbn = CheckHaiin(tkdHaisha.DrvJin, tkdHaisha.GuiSu, tkdHaisha.UkeNo,
                                                   tkdHaisha.UnkRen, tkdHaisha.TeiDanNo, tkdHaisha.BunkRen,
                                                   tkdHaisha.HaiSymd);  */
                    tkdHaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdHaisha.UpdTime = DateTime.Now.ToString("HHmmss");
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
                if (haiinDataList.Count == 0)
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
            private string ParseDateToString(DateTime valueDate)
            {
                return valueDate.ToString("yyyyMMdd");
            }
            private bool CheckStringValue(string strValue)
            {
                bool result = false;
                if (!string.IsNullOrEmpty(strValue) && !string.IsNullOrWhiteSpace(strValue))
                {
                    result = true;
                }
                return result;
            }

        }
    }
}
