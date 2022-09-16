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
    public class GetHaiShaDataQuery : IRequest<TkdHaisha>
    {
        public BusAllocationDataUpdate BusAllocationDataUpdate { get; set; }
        public class Handler : IRequestHandler<GetHaiShaDataQuery, TkdHaisha>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public Task<TkdHaisha> Handle(GetHaiShaDataQuery request, CancellationToken cancellationToken)
            {
                var data = request.BusAllocationDataUpdate;
                try
                {
                    TkdHaisha tkdHaisha = _context.TkdHaisha.FirstOrDefault(e => e.UkeNo == data.YYKSHO_UkeNo
                                                                            && e.UnkRen == data.HAISHA_UnkRen
                                                                            && e.SyaSyuRen == data.HAISHA_SyaSyuRen
                                                                            && e.TeiDanNo == data.HAISHA_TeiDanNo
                                                                            && e.BunkRen == data.HAISHA_BunkRen);
                    SetTkdHaishaData(ref tkdHaisha, data);
                    return Task.FromResult(tkdHaisha);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void SetTkdHaishaData(ref TkdHaisha tkdHaisha, BusAllocationDataUpdate busAllocationDataUpdate)
            {
                if (tkdHaisha != null && busAllocationDataUpdate != null)
                {
                    var data = busAllocationDataUpdate;
                    tkdHaisha.HenKai++;
                    tkdHaisha.GoSya = data.HAISHA_GoSya;
                    tkdHaisha.SyuEigCdSeq = data.BranchChartData.EigyoCdSeq;
                    tkdHaisha.KikEigSeq = data.BranchChartDataKik.EigyoCdSeq;
                   
                    if (data.BusInfoData != null)
                        tkdHaisha.HaiSsryCdSeq = data.BusInfoData.SyaRyoCdSeq;
                    else
                    {
                        tkdHaisha.HaiSsryCdSeq = 0;
                        tkdHaisha.KssyaRseq = 0;
                    }

                    if (data.screenKbn == 0) // 選択区分 = 2(配車入力)
                        tkdHaisha.KssyaRseq = tkdHaisha.KssyaRseq == 0 && data.BusInfoData != null ? data.BusInfoData.SyaRyoCdSeq : tkdHaisha.KssyaRseq;
                    else // 選択区分 = 1(仮車入力)
                        tkdHaisha.KssyaRseq = data.BusInfoData != null ? data.BusInfoData.SyaRyoCdSeq : 0;

                    tkdHaisha.DanTaNm2 = CheckLimitLengthString(data.DanTaNm2, 100);
                    if (data.TPM_CodeKbDataKenCD != null)
                    {
                        tkdHaisha.IkMapCdSeq = data.TPM_CodeKbDataKenCD.BasyoBasyoMapCdSeq;
                    }
                    tkdHaisha.IkNm = data.IkNm;
                    tkdHaisha.SyuKoYmd = ParseDateToString(data.SyuKoYmd);
                    tkdHaisha.SyuKoTime = data.SyuKoTime;
                    tkdHaisha.SyuPaTime = data.SyuPaTime;
                    tkdHaisha.HaiSymd = ParseDateToString(data.HaiSYmd);
                    tkdHaisha.HaiStime = data.HaiSTime;
                    if (data.TPM_CodeKbDataBunruiCDStart != null)
                    {
                        tkdHaisha.HaiScdSeq = data.TPM_CodeKbDataBunruiCDStart.HaiChiHaiSCdSeq;
                    }
                    tkdHaisha.HaiSnm = CheckLimitLengthString(data.HaiSNm, 50);
                    tkdHaisha.HaiSjyus1 = CheckLimitLengthString(data.HaiSJyus1, 30); 
                    tkdHaisha.HaiSjyus2 = CheckLimitLengthString(data.HaiSJyus2, 30);
                    tkdHaisha.HaiSkigou = CheckLimitLengthString(data.HaiSKigou, 6); 
                    if (data.TPM_CodeKbDataDepotStart != null)
                    {
                        tkdHaisha.HaiSkouKcdSeq = data.TPM_CodeKbDataDepotStart.KoutuKoukCdSeq;
                        tkdHaisha.HaiSkouKnm = data.TPM_CodeKbDataDepotStart.KoutuRyakuNm;
                        tkdHaisha.HaiSbinCdSeq = data.TPM_CodeKbDataDepotStart.BinBinCdSeq;
                    }
                    tkdHaisha.HaiSbinNm = CheckLimitLengthString(data.HaisBinNm ?? "", 10); 
                    tkdHaisha.HaiSsetTime = data.HaiSSetTime ?? "0000";
                    tkdHaisha.KikYmd = ParseDateToString(data.KikYmd);
                    tkdHaisha.KikTime = data.KikTime;
                    tkdHaisha.TouYmd = ParseDateToString(data.TouYmd);
                    tkdHaisha.TouChTime = data.TouChTime;
                    if (data.TPM_CodeKbDataBunruiCDEnd != null)
                    {
                        tkdHaisha.TouCdSeq = data.TPM_CodeKbDataBunruiCDEnd.HaiChiHaiSCdSeq;
                    }                        
                    tkdHaisha.TouNm = CheckLimitLengthString(data.TouNm, 50);
                    tkdHaisha.TouJyusyo1 = CheckLimitLengthString(data.TouJyusyo1,30); 
                    tkdHaisha.TouJyusyo2 = CheckLimitLengthString(data.TouJyusyo2, 30); 
                    tkdHaisha.TouKigou = CheckLimitLengthString(data.TouKigou, 6); 
                    if (data.TPM_CodeKbDataDepotEnd != null)
                    {
                        tkdHaisha.TouKouKcdSeq = data.TPM_CodeKbDataDepotEnd.KoutuKoukCdSeq;
                        tkdHaisha.TouSkouKnm = data.TPM_CodeKbDataDepotEnd.KoutuRyakuNm;
                        tkdHaisha.TouBinCdSeq = data.TPM_CodeKbDataDepotEnd.BinBinCdSeq;
                    }
                    tkdHaisha.TouBinNm = CheckLimitLengthString(data.TouBinNm ?? "", 10); 
                    tkdHaisha.TouSetTime = data.TouSetTime ?? "0000";
                    tkdHaisha.JyoSyaJin = data.JyoSyaJin;
                    tkdHaisha.PlusJin = data.PlusJin;
                    tkdHaisha.DrvJin = data.DrvJin;
                    tkdHaisha.GuiSu = data.GuiSu;
                    tkdHaisha.OthJinKbn1 = (byte)Int16.Parse(data.TPM_CodeKbDataOTHJINKBN01.CodeKb_CodeKbn);
                    tkdHaisha.OthJin1 = data.OthJin1;
                    tkdHaisha.OthJinKbn2 = (byte)Int16.Parse(data.TPM_CodeKbDataOTHJINKBN02.CodeKb_CodeKbn);
                    tkdHaisha.OthJin2 = data.OthJin2;
                    tkdHaisha.Kskbn = (byte)(data.BusInfoData != null ? 2 : 1);

                    if (data.screenKbn == 0) // 選択区分 = 2(配車入力)
                        tkdHaisha.HaiSkbn = (byte)(data.BusInfoData != null ? 2 : 1);
                    else // 選択区分 = 1(仮車入力)
                        tkdHaisha.HaiSkbn = 1;

                    //tkdHaisha.HaiIkbn =  CheckHaiin(data.DrvJin, data.GuiSu, data.YYKSHO_UkeNo, data.HAISHA_UnkRen, data.HAISHA_TeiDanNo, data.HAISHA_BunkRen,
                    //    data.HaiSYmd.ToString("yyyyMMdd"));
                    tkdHaisha.PlatNo = CheckLimitLengthString(data.PlatNo, 20);
                    /*Custom Item*/

                    tkdHaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdHaisha.UpdTime = DateTime.Now.ToString("HHmm");
                    tkdHaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdHaisha.UpdPrgId = "KU0600";
                }
            }

            private string CheckLimitLengthString(string str, int limit)
            {
                if (str.Length > limit)
                {
                    return str.Substring(0, limit);
                }
                return str;
            }
            private string ParseDateToString(DateTime valueDate)
            {
                return valueDate.ToString("yyyyMMdd");
            }
            private string ParseTimeToString(TimeSpan valueTime)
            {
                return valueTime.ToString("hhmm");
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
        }
    }
}
