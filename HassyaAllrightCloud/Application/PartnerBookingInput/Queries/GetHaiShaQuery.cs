using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetHaiShaQuery : IRequest<TkdHaisha>
    {
        public string Ukeno { get; set; }
        public HaiShaDataUpdate HaiShaDataUpdate { get; set; }
        public class Handler : IRequestHandler<GetHaiShaQuery, TkdHaisha>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetHaiShaQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetHaiShaQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<TkdHaisha> Handle(GetHaiShaQuery request, CancellationToken cancellationToken)
            {
                var paramHaiSha = request.HaiShaDataUpdate;
                try
                {
                    TkdHaisha tkdHaisha = _context.TkdHaisha.FirstOrDefault(e => e.UkeNo == request.Ukeno
                                                                            && e.UnkRen == paramHaiSha.HAISHA_UnkRen
                                                                            && e.SyaSyuRen == paramHaiSha.HAISHA_SyaSyuRen
                                                                            && e.TeiDanNo == paramHaiSha.HAISHA_TeiDanNo
                                                                            && e.BunkRen == paramHaiSha.HAISHA_BunkRen);
                    SetTkdHaishaData(ref tkdHaisha, paramHaiSha);
                    return Task.FromResult(tkdHaisha);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void SetTkdHaishaData(ref TkdHaisha tkdHaisha, HaiShaDataUpdate HaiShaDataUpdate)
            {
                if (tkdHaisha != null && HaiShaDataUpdate != null)
                {
                    tkdHaisha.HenKai++;
                    tkdHaisha.HaiSymd = HaiShaDataUpdate.HaiSYmd;
                    tkdHaisha.HaiStime = HaiShaDataUpdate.HaiSTime;
                    tkdHaisha.SyuPaTime = HaiShaDataUpdate.SyuPaTime;
                    tkdHaisha.TouYmd = HaiShaDataUpdate.TouYmd;
                    tkdHaisha.TouChTime = HaiShaDataUpdate.TouChTime;
                    tkdHaisha.GoSya = HaiShaDataUpdate.GoSya;

                    if (HaiShaDataUpdate.DataNm2.Length > 100)
                    {
                        tkdHaisha.DanTaNm2 = HaiShaDataUpdate.DataNm2.Substring(0, 100);
                    }
                    else
                    {
                        tkdHaisha.DanTaNm2 = HaiShaDataUpdate.DataNm2;
                    }                   

                        tkdHaisha.HaiScdSeq = HaiShaDataUpdate.codeKbnBunruiDataPopupStart?.HAICHI_HaiSCdSeq ?? 0;
                        tkdHaisha.HaiSnm = TrimStrWithMaxLength(HaiShaDataUpdate?.HAISHA_HaiSNm ?? string.Empty, 50);

                    if(HaiShaDataUpdate.HAISHA_HaiSJyus1.Length >25)
                    {
                        tkdHaisha.HaiSjyus1 = HaiShaDataUpdate.HAISHA_HaiSJyus1.Substring(0,25);
                    }
                    else
                    {
                        tkdHaisha.HaiSjyus1 = HaiShaDataUpdate.HAISHA_HaiSJyus1;
                    }
                    if (HaiShaDataUpdate.HAISHA_HaiSJyus2.Length > 25)
                    {
                        tkdHaisha.HaiSjyus2 = HaiShaDataUpdate.HAISHA_HaiSJyus2.Substring(0, 25);
                    }
                    else
                    {
                        tkdHaisha.HaiSjyus2 = HaiShaDataUpdate.HAISHA_HaiSJyus2;
                    }
                    if (HaiShaDataUpdate.HAISHA_HaiSKigou.Length > 6)
                    {
                        tkdHaisha.HaiSkigou = HaiShaDataUpdate.HAISHA_HaiSKigou.Substring(0, 6);
                    }
                    else
                    {
                        tkdHaisha.HaiSkigou = HaiShaDataUpdate.HAISHA_HaiSKigou;
                    }
                   
                    tkdHaisha.HaiSkouKcdSeq = HaiShaDataUpdate.vehicleDispatchPopupStart?.KOUTU_KoukCdSeq ?? 0;
                    tkdHaisha.HaiSkouKnm = HaiShaDataUpdate.vehicleDispatchPopupStart?.KOUTU_KoukNm ?? string.Empty;
                    tkdHaisha.HaiSbinCdSeq = HaiShaDataUpdate.vehicleDispatchPopupStart?.BIN_BinCdSeq ?? 0;                                              

                    tkdHaisha.HaiSbinNm = HaiShaDataUpdate.BIN_BinNmStart?? "";
                    tkdHaisha.HaiSsetTime = HaiShaDataUpdate.BIN_TouChTime ?? "0000";

                    tkdHaisha.TouCdSeq = HaiShaDataUpdate.codeKbnBunruiDataPopupEnd?.HAICHI_HaiSCdSeq ?? 0;
                    tkdHaisha.TouNm = TrimStrWithMaxLength(HaiShaDataUpdate.HAISHA_TouchName ?? string.Empty, 50);

                    if (HaiShaDataUpdate.HAISHA_TouJyusyo1.Length > 25)
                    {
                        tkdHaisha.TouJyusyo1 = HaiShaDataUpdate.HAISHA_TouJyusyo1.Substring(0, 25);
                    }
                    else
                    {
                        tkdHaisha.TouJyusyo1 = HaiShaDataUpdate.HAISHA_TouJyusyo1;
                    }
                    if (HaiShaDataUpdate.HAISHA_TouJyusyo2.Length > 25)
                    {
                        tkdHaisha.TouJyusyo2 = HaiShaDataUpdate.HAISHA_TouJyusyo2.Substring(0, 25);
                    }
                    else
                    {
                        tkdHaisha.TouJyusyo2 = HaiShaDataUpdate.HAISHA_TouJyusyo2;
                    }
                    if (HaiShaDataUpdate.HAISHA_TouKigou.Length > 6)
                    {
                        tkdHaisha.TouKigou = HaiShaDataUpdate.HAISHA_TouKigou.Substring(0, 6);
                    }
                    else
                    {
                        tkdHaisha.TouKigou = HaiShaDataUpdate.HAISHA_TouKigou;
                    }                  

                    tkdHaisha.TouKouKcdSeq = HaiShaDataUpdate.vehicleDispatchPopupEnd?.KOUTU_KoukCdSeq ?? 0;
                    tkdHaisha.TouSkouKnm = HaiShaDataUpdate.vehicleDispatchPopupEnd?.KOUTU_KoukNm ?? string.Empty;
                    tkdHaisha.TouBinCdSeq = HaiShaDataUpdate.vehicleDispatchPopupEnd?.BIN_BinCdSeq ?? 0;                     

                    tkdHaisha.TouBinNm = HaiShaDataUpdate.BIN_BinNmEnd ?? "";
                    tkdHaisha.TouSetTime = HaiShaDataUpdate.BIN_SyuPaTime ?? "0000";
                    tkdHaisha.JyoSyaJin = (short)HaiShaDataUpdate.HAISHA_JyoSyaJin;
                    tkdHaisha.PlusJin = (short)HaiShaDataUpdate.HAISHA_PlusJin;
                    if(HaiShaDataUpdate.tPM_CodeKbDataOTHJINKBN01 !=null)
                    {
                        tkdHaisha.OthJinKbn1 = (byte)Int16.Parse(HaiShaDataUpdate.tPM_CodeKbDataOTHJINKBN01.CodeKb_CodeKbn);
                        tkdHaisha.OthJin1 = (short)HaiShaDataUpdate.OthJin1;
                    }                   
                    if(HaiShaDataUpdate.tPM_CodeKbDataOTHJINKBN02 !=null)
                    {
                        tkdHaisha.OthJinKbn2 = (byte)Int16.Parse(HaiShaDataUpdate.tPM_CodeKbDataOTHJINKBN02.CodeKb_CodeKbn);
                        tkdHaisha.OthJin2 = (short)HaiShaDataUpdate.OthJin2;
                    }                    
                    tkdHaisha.IkMapCdSeq = HaiShaDataUpdate.tPM_CodeKbDataKenCD?.BasyoBasyoMapCdSeq ?? 0;
                    tkdHaisha.IkNm = TrimStrWithMaxLength(HaiShaDataUpdate.HAISHA_IkNm ?? string.Empty, 50);
                    
                    tkdHaisha.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                    tkdHaisha.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                    tkdHaisha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdHaisha.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                }
            }

            private string TrimStrWithMaxLength(string str, int maxLength) => string.IsNullOrEmpty(str) || str.Length <= maxLength ? str : str.Substring(0, maxLength);
        }
    }
}
