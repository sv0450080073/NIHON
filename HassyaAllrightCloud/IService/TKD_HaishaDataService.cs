using System.Collections.Generic;
using System;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;
using System.Globalization;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_HaishaDataListService
    {
        void Updatebusdatagreen(ItemBus updatedBus, int userlogin, int YouCdSeq, int YouSitCdSeq);
        void UpdatebusdatagreenwithHaisha(TkdHaisha updatedBus, int userlogin, int YouCdSeq, int YouSitCdSeq, decimal Zeiritsu, byte KataKbn);
        void Updatebusdatagray(ItemBus updatedBus, int userlogin);
        void Updatebusdata(ItemBus updatedBus, int userlogin);
        void Updatebusdeletedata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin);
        void UpdatebusMergedata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin,ItemStaff updatedStaff,string formNm, byte nippoKbn);
        void UpdatebusSimpledata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin);
        void UpdatebusUndodata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin, TimeBooking timeBooking);
        void UpdatebusHaiIKbn(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin,byte haiIKbn);
        bool UpdateHaishadata(BusBookingDataAllocation dataupdate, int userlogin, bool checkDeleteDriver);
        void Updatebustimedata(ItemBus updatedBus, int userlogin, DateTime datestart, DateTime dateend);
        void UpdateStafftimedata(ItemStaff updatedBus, int userlogin, DateTime datestart, DateTime dateend);
        Task updateHaiindata(BusAllocationDataUpdate dataupdate, bool checkDeleteDriver);
        Task SplitBusUpdate(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, SplitBusData busdate1, SplitBusData busdate2, int userlogin,string formNm);

        Task UpdateTKD_KobanBusallocation(BusAllocationDataUpdate dataupdate);

        Task<bool> HaitaCheck(BusAllocationHaitaCheck busAllocationHaitaCheck, bool isCheckNull);
    }
    public class TKD_HaishaDataService : ITKD_HaishaDataListService
    {
        private readonly KobodbContext _dbContext;
        private readonly ITPM_EigyosDataListService _EigyosDataService;
        private readonly BusScheduleHelper _busScheduleHelper;
        private readonly ITKD_YoshaDataListService _YoshaDataService;
        private readonly ITKD_YykSyuListService _YykSyuService;
        private readonly ITKD_KobanDataService _KobanDataService;
        private readonly ITKD_HaiinDataService _HaiinDataService;
        private readonly ITKD_MihrimService _MihrimService;
        private readonly IBusBookingDataListService _busBookingDataService;
        private readonly IStaffsChartService _StaffsChartService;
        private readonly IBusAllocationService _BusAllocationService;

        private readonly object dataupdateHaisha_BunkRen;

        public TKD_HaishaDataService(KobodbContext context, ITPM_EigyosDataListService TPM_EigyosDataService, BusScheduleHelper BusScheduleHelper, ITKD_YoshaDataListService TKD_YoshaDataListService, 
            ITKD_YykSyuListService TKD_YykSyuListService, ITKD_KobanDataService TKD_KobanDataService, IBusBookingDataListService BusBookingDataService, ITKD_HaiinDataService TKD_HaiinDataService, 
            ITKD_MihrimService TKD_MihrimService, IStaffsChartService StaffsChartService, IBusBookingDataListService BusBookingDataListService, IBusAllocationService BusAllocationService)
        {
            _dbContext = context;
            _EigyosDataService = TPM_EigyosDataService;
            _busScheduleHelper = BusScheduleHelper;
            _YoshaDataService = TKD_YoshaDataListService;
            _YykSyuService = TKD_YykSyuListService;
            _KobanDataService = TKD_KobanDataService;
            _busBookingDataService = BusBookingDataService;
            _HaiinDataService = TKD_HaiinDataService;
            _MihrimService = TKD_MihrimService;
            _StaffsChartService = StaffsChartService;
            _BusAllocationService = BusAllocationService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <param name="userlogin"></param>
        public void UpdatebusHaiIKbn(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin, byte haiIKbn)
        {
            var std = _dbContext.TkdHaisha.Find(UkeNo, UnkRen, TeiDanNo, BunkRen);
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.UpdPrgId = "KU0100";
            std.HaiIkbn = haiIKbn;
            std.UpdSyainCd = userlogin;
            std.HenKai = std.HenKai++;
            _dbContext.Update(std);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// update bus line
        /// </summary>
        /// <param name="dataupdate"></param>
        /// <param name="userlogin"></param>
        /// <returns></returns>
        public bool UpdateHaishadata(BusBookingDataAllocation dataupdate, int userlogin, bool checkDeleteDriver)
        {
            try
            {
                var std = _dbContext.TkdHaisha.Find(dataupdate.Haisha_UkeNo, dataupdate.Haisha_UnkRen, dataupdate.Haisha_TeiDanNo, dataupdate.Haisha_BunkRen);
                std.GoSya = dataupdate.Haisha_GoSya.ToString("D4");
                std.KhinKbn = 2;
                std.HaiSkbn = 2;
                std.HaiIkbn = 2;
                std.GuideSiteiEigyoCdSeq = 0;
                std.GuiWnin = (short)dataupdate.Driverlstitem.Where(t => t.Syokum_SyokumuCdSeq == 3 || t.Syokum_SyokumuCdSeq == 4).Count();
                std.SyuEigCdSeq = dataupdate.Haisha_SyuEigCdSeq;
                std.KikEigSeq = dataupdate.Haisha_KikEigSeq;
                std.HaiSsryCdSeq = dataupdate.Haisha_HaiSSryCdSeq;
                std.KssyaRseq = dataupdate.Haisha_HaiSSryCdSeq;
                std.DanTaNm2 = dataupdate.Haisha_DanTaNm2;
                std.IkMapCdSeq = dataupdate.Haisha_IkMapCdSeq;
                std.IkNm = dataupdate.Haisha_IkNm;
                std.SyuKoYmd = dataupdate.Haisha_SyuKoYmd;
                std.SyuKoTime = dataupdate.Haisha_SyuKoTime;
                std.SyuPaTime = dataupdate.Haisha_SyuPaTime;
                std.HaiSymd = dataupdate.Haisha_HaiSYmd;
                std.HaiStime = dataupdate.Haisha_HaiSTime;
                std.HaiScdSeq = dataupdate.Haisha_HaiSCdSeq;
                std.HaiSnm = dataupdate.Haisha_HaiSNm;
                std.HaiSjyus1 = dataupdate.Haisha_HaiSJyus1;
                std.HaiSjyus2 = dataupdate.Haisha_HaiSJyus2;
                std.HaiSkigou = dataupdate.Haisha_HaiSKigou;
                std.HaiSkouKcdSeq = dataupdate.Haisha_HaiSKouKCdSeq;
                std.HaiSbinCdSeq = dataupdate.Haisha_HaiSBinCdSeq;
                std.HaiSsetTime = std.HaiSbinCdSeq != 0 ? dataupdate.Haisha_HaiSSetTime.ToString("hhmm") : "";
                std.KikYmd = dataupdate.Haisha_KikYmd;
                std.KikTime = dataupdate.Haisha_KikTime;
                std.TouYmd = dataupdate.Haisha_TouYmd;
                std.TouChTime = dataupdate.Haisha_TouChTime;
                std.TouCdSeq = dataupdate.Haisha_TouCdSeq;
                std.TouNm = dataupdate.Haisha_TouNm;
                std.TouJyusyo1 = dataupdate.Haisha_TouJyusyo1;
                std.TouJyusyo2 = dataupdate.Haisha_TouJyusyo2;
                std.TouKigou = dataupdate.Haisha_TouKigou;
                std.TouKouKcdSeq = dataupdate.Haisha_TouKouKCdSeq;
                std.TouBinCdSeq = dataupdate.Haisha_TouBinCdSeq;
                std.TouSetTime = std.TouBinCdSeq != 0 ? dataupdate.Haisha_TouSetTime.ToString("hhmm") : "";
                std.JyoSyaJin = dataupdate.Haisha_JyoSyaJin;
                std.PlusJin = dataupdate.Haisha_PlusJin;
                std.DrvJin = dataupdate.Haisha_DrvJin;
                std.GuiSu = dataupdate.Haisha_GuiSu;
                std.OthJinKbn1 = dataupdate.Haisha_OthJinKbn1;
                std.OthJin1 = dataupdate.Haisha_OthJin1;
                std.OthJinKbn2 = dataupdate.Haisha_OthJinKbn2;
                std.OthJin2 = dataupdate.Haisha_OthJin2;
                std.PlatNo = dataupdate.Haisha_PlatNo;
                std.HaiSkouKnm = dataupdate.HaishaExp_HaiSKouKNm;
                std.HaiSbinNm = dataupdate.HaishaExp_HaisBinNm;
                std.TouSkouKnm = dataupdate.HaishaExp_TouSKouKNm;
                std.TouSbinNm = dataupdate.HaishaExp_TouSBinNm;
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU0100";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);

                _dbContext.SaveChanges();
                if (dataupdate.Driverlstitem.Count > 0) //edit driver but don't delete all
                {
                    List<int> haiInRenlst = new List<int>();
                    foreach (var item in dataupdate.Driverlstitem)
                    {
                        int max = 0;
                        var gethaiinlst = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.Haisha_UkeNo && t.BunkRen == dataupdate.Haisha_BunkRen && t.UnkRen == dataupdate.Haisha_UnkRen && t.TeiDanNo == dataupdate.Haisha_TeiDanNo).ToList();
                        if (gethaiinlst.Count() != 0)
                        {
                            max = gethaiinlst.Max(t => t.HaiInRen);
                        }
                        if (item.HaiInRen == 0)
                        {
                            TkdHaiin insertnew = new TkdHaiin();
                            insertnew.UkeNo = dataupdate.Haisha_UkeNo;
                            insertnew.BunkRen = dataupdate.Haisha_BunkRen;
                            insertnew.UnkRen = dataupdate.Haisha_UnkRen;
                            insertnew.TeiDanNo = dataupdate.Haisha_TeiDanNo;
                            insertnew.Syukinbasy = item.StartComment == null ? "" : item.StartComment;
                            insertnew.TaiknTime = item.EndTime;
                            insertnew.TaiknBasy = item.EndComment == null ? "" : item.EndComment;
                            insertnew.HaiInRen = (byte)(max + 1);
                            insertnew.SyainCdSeq = item.SyainCdSeq;
                            insertnew.SyukinTime = item.StartTime;
                            insertnew.SiyoKbn = 1;
                            insertnew.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            insertnew.UpdTime = DateTime.Now.ToString("HHmmss");
                            insertnew.UpdPrgId = "KU0100";
                            insertnew.UpdSyainCd = userlogin;
                            insertnew.HenKai = 0;
                            _dbContext.TkdHaiin.Add(insertnew);
                            _dbContext.SaveChanges();
                            haiInRenlst.Add(insertnew.HaiInRen);
                        }
                        else
                        {
                            var update = _dbContext.TkdHaiin.Find(dataupdate.Haisha_UkeNo, dataupdate.Haisha_UnkRen, dataupdate.Haisha_TeiDanNo, dataupdate.Haisha_BunkRen, (byte)item.HaiInRen);
                            update.SyainCdSeq = item.SyainCdSeq;
                            update.Syukinbasy = item.StartComment == null ? "" : item.StartComment;
                            update.TaiknTime = item.EndTime;
                            update.TaiknBasy = item.EndComment == null ? "" : item.EndComment;
                            update.SyukinTime = item.StartTime;
                            update.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            update.UpdTime = DateTime.Now.ToString("HHmmss");
                            update.UpdPrgId = "KU0100";
                            update.UpdSyainCd = userlogin;
                            update.HenKai = update.HenKai++;
                            _dbContext.Update(update);
                            _dbContext.SaveChanges();
                            haiInRenlst.Add(item.HaiInRen);
                        }
                    }
                    var updatenull = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.Haisha_UkeNo && t.BunkRen == dataupdate.Haisha_BunkRen && t.UnkRen == dataupdate.Haisha_UnkRen && t.TeiDanNo == dataupdate.Haisha_TeiDanNo && !haiInRenlst.Contains(t.HaiInRen)).ToList();
                    foreach (var item in updatenull)
                    {
                        var update = _dbContext.TkdHaiin.Find(dataupdate.Haisha_UkeNo, dataupdate.Haisha_UnkRen, dataupdate.Haisha_TeiDanNo, dataupdate.Haisha_BunkRen, item.HaiInRen);
                        update.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        update.UpdTime = DateTime.Now.ToString("HHmmss");
                        update.UpdPrgId = "KU0100";
                        update.SiyoKbn = 2;
                        update.UpdSyainCd = userlogin;
                        update.HenKai = update.HenKai++;
                        _dbContext.Update(update);
                        _dbContext.SaveChanges();
                    }
                }
                else
                {
                    if (checkDeleteDriver == true)
                    {
                        var checkDataHaiin = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.Haisha_UkeNo
                       && t.SiyoKbn == 1).ToList();
                        foreach (var item in checkDataHaiin)
                        {
                            item.SiyoKbn = 2;
                            _dbContext.Update(item);
                            _dbContext.SaveChanges();
                        }
                        int max = 0;
                        var gethaiinlst = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.Haisha_UkeNo && t.BunkRen == dataupdate.Haisha_BunkRen && t.UnkRen == dataupdate.Haisha_UnkRen && t.TeiDanNo == dataupdate.Haisha_TeiDanNo).ToList();
                        if (gethaiinlst.Count() != 0)
                        {
                            max = gethaiinlst.Max(t => t.HaiInRen);
                        }
                        if (dataupdate.SyaRyo_SyainCdSeq != 0)
                        {
                            TkdHaiin insertnew = new TkdHaiin();
                            insertnew.UkeNo = dataupdate.Haisha_UkeNo;
                            insertnew.BunkRen = dataupdate.Haisha_BunkRen;
                            insertnew.UnkRen = dataupdate.Haisha_UnkRen;
                            insertnew.TeiDanNo = dataupdate.Haisha_TeiDanNo;
                            insertnew.Syukinbasy = "";
                            insertnew.TaiknTime = dataupdate.ReturnTime.ToString("hhmm");
                            insertnew.TaiknBasy = "";
                            insertnew.HaiInRen = (byte)(max + 1);
                            insertnew.SyainCdSeq = dataupdate.SyaRyo_SyainCdSeq;
                            insertnew.SyukinTime = dataupdate.DepartureTime.ToString("hhmm");
                            insertnew.SiyoKbn = 1;
                            insertnew.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            insertnew.UpdTime = DateTime.Now.ToString("HHmmss");
                            insertnew.UpdPrgId = "KU0100";
                            insertnew.UpdSyainCd = userlogin;
                            insertnew.HenKai = 0;
                            _dbContext.TkdHaiin.Add(insertnew);
                            _dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        var checkDataHaiinDefault_SyainCdSeq = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.Haisha_UkeNo).ToList();
                        if (checkDataHaiinDefault_SyainCdSeq.Count == 0)
                        {
                            if (dataupdate.SyaRyo_SyainCdSeq != 0)
                            {
                                TkdHaiin insertnew = new TkdHaiin();
                                insertnew.UkeNo = dataupdate.Haisha_UkeNo;
                                insertnew.BunkRen = dataupdate.Haisha_BunkRen;
                                insertnew.UnkRen = dataupdate.Haisha_UnkRen;
                                insertnew.TeiDanNo = dataupdate.Haisha_TeiDanNo;
                                insertnew.Syukinbasy = "";
                                insertnew.TaiknTime = dataupdate.ReturnTime.ToString("hhmm");
                                insertnew.TaiknBasy = "";
                                insertnew.HaiInRen = 1;
                                insertnew.SyainCdSeq = dataupdate.SyaRyo_SyainCdSeq;
                                insertnew.SyukinTime = dataupdate.DepartureTime.ToString("hhmm");
                                insertnew.SiyoKbn = 1;
                                insertnew.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                                insertnew.UpdTime = DateTime.Now.ToString("HHmmss");
                                insertnew.UpdPrgId = "KU0100";
                                insertnew.UpdSyainCd = userlogin;
                                insertnew.HenKai = 0;
                                _dbContext.TkdHaiin.Add(insertnew);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// delete bus line
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <param name="userlogin"></param>
        public void Updatebusdeletedata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin)
        {
            var std = _dbContext.TkdHaisha.Find(UkeNo, UnkRen, TeiDanNo, BunkRen);
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.UpdPrgId = "KU0100";
            std.UpdSyainCd = userlogin;
            std.HenKai = (short)(std.HenKai + 1);
            std.SiyoKbn = 2;

            var updatecutline = _dbContext.TkdHaisha.Where(t => t.UkeNo == UkeNo && t.TeiDanNo == TeiDanNo && t.SiyoKbn == 1).OrderBy(t => t.HaiSymd).ThenBy(t => t.HaiStime).ToList();
            TkdHaisha updateMoney = new TkdHaisha();
            if (std.BunKsyuJyn == 1 || std.BunKsyuJyn == 0)
            {
                updateMoney = updatecutline.Where(t => t.BunKsyuJyn == 2).First();
            }
            else
            {
                updateMoney = updatecutline.Where(t => t.BunKsyuJyn == (std.BunKsyuJyn - 1)).First();
            }
            var stdupdatemoney = _dbContext.TkdHaisha.Find(updateMoney.UkeNo, updateMoney.UnkRen, updateMoney.TeiDanNo, updateMoney.BunkRen);
            stdupdatemoney.SyaRyoUnc += std.SyaRyoUnc;
            stdupdatemoney.SyaRyoSyo += std.SyaRyoSyo;
            stdupdatemoney.SyaRyoTes += std.SyaRyoTes;
            _dbContext.Update(stdupdatemoney);
            _dbContext.SaveChanges();
            _HaiinDataService.DeleteHaiin(UkeNo, UnkRen, TeiDanNo, BunkRen);
             

        }
        /// <summary>
        /// update undo status
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <param name="userlogin"></param>
        public void UpdatebusUndodata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin, TimeBooking timeBooking)
        {
                var std = _dbContext.TkdHaisha.Find(UkeNo, UnkRen, TeiDanNo, BunkRen);
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU0100";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);
                std.HaiSkbn = 1;
                std.SyuKoTime = timeBooking.SyuKoTime;
                std.SyuKoYmd = timeBooking.SyuKoYmd;
                std.SyuPaTime = timeBooking.SyuPaTime;
                std.HaiSymd = timeBooking.HaiSYmd;
                std.HaiStime = timeBooking.HaiSTime;
                std.TouYmd = timeBooking.TouYmd;
                std.TouChTime = timeBooking.TouChTime;
                std.KikYmd = timeBooking.KikYmd;
                std.KikTime = timeBooking.KikTime;
                _dbContext.Update(std);
                _dbContext.SaveChanges();     
        }
        /// <summary>
        /// update approve busline
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <param name="userlogin"></param>
        public void UpdatebusSimpledata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin)
        {
            var std = _dbContext.TkdHaisha.Find(UkeNo, UnkRen, TeiDanNo, BunkRen);
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.UpdPrgId = "KU0100";
            std.UpdSyainCd = userlogin;
            std.HenKai = (short)(std.HenKai + 1);
            std.HaiSkbn = 2;
            _dbContext.TkdHaisha.Update(std);
            _dbContext.SaveChanges();
        }
        public async Task updateHaiindata(BusAllocationDataUpdate dataupdate, bool checkDeleteDriver)
        {
            await _KobanDataService.DeleteKobanbyHaisha(dataupdate.YYKSHO_UkeNo, dataupdate.HAISHA_UnkRen, dataupdate.HAISHA_TeiDanNo, dataupdate.HAISHA_BunkRen);
            if (dataupdate.DriverAssigneds.Count > 0) //edit driver but don't delete all
            {
                List<int> haiInRenlst = new List<int>();
                foreach (var item in dataupdate.DriverAssigneds)
                {

                    int max = 0;
                    var gethaiinlst = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.YYKSHO_UkeNo && t.BunkRen == dataupdate.HAISHA_BunkRen && t.UnkRen == dataupdate.HAISHA_UnkRen && t.TeiDanNo == dataupdate.HAISHA_TeiDanNo).ToList();
                    if (gethaiinlst.Count() != 0)
                    {
                        max = gethaiinlst.Max(t => t.HaiInRen);
                    }
                    if (item.HaiInRen == 0)
                    {
                        TkdHaiin insertnew = new TkdHaiin();
                        insertnew.UkeNo = dataupdate.YYKSHO_UkeNo;
                        insertnew.BunkRen = dataupdate.HAISHA_BunkRen;
                        insertnew.UnkRen = dataupdate.HAISHA_UnkRen;
                        insertnew.TeiDanNo = dataupdate.HAISHA_TeiDanNo;
                        insertnew.Syukinbasy = item.StartComment == null ? "" : item.StartComment;
                        insertnew.TaiknTime = item.EndTime;
                        insertnew.TaiknBasy = item.EndComment == null ? "" : item.EndComment;
                        insertnew.HaiInRen = (byte)(max + 1);
                        insertnew.SyainCdSeq = item.SyainCdSeq;
                        insertnew.SyukinTime = item.StartTime;
                        insertnew.SiyoKbn = 1;
                        insertnew.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        insertnew.UpdTime = DateTime.Now.ToString("HHmmss");
                        insertnew.UpdPrgId = "KU0600";
                        insertnew.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        insertnew.HenKai = 0;
                        insertnew.SyokumuKbn = item.SyokumuKbn;
                        _dbContext.TkdHaiin.Add(insertnew);
                        await _dbContext.SaveChangesAsync();
                        haiInRenlst.Add(insertnew.HaiInRen);

                    }
                    else
                    {
                        var update = _dbContext.TkdHaiin.Find(dataupdate.YYKSHO_UkeNo, dataupdate.HAISHA_UnkRen, dataupdate.HAISHA_TeiDanNo, dataupdate.HAISHA_BunkRen, (byte)item.HaiInRen);
                        update.SyainCdSeq = item.SyainCdSeq;
                        update.Syukinbasy = item.StartComment == null ? "" : item.StartComment;
                        update.TaiknTime = item.EndTime;
                        update.TaiknBasy = item.EndComment == null ? "" : item.EndComment;
                        update.SyukinTime = item.StartTime;
                        update.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        update.UpdTime = DateTime.Now.ToString("HHmmss");
                        update.UpdPrgId = "KU0600";
                        update.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq; ;
                        update.HenKai = update.HenKai++;
                        update.SyokumuKbn = item.SyokumuKbn;
                        _dbContext.Update(update);
                        await _dbContext.SaveChangesAsync();
                        haiInRenlst.Add(item.HaiInRen);
                    }
                }
                var updatenull = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.YYKSHO_UkeNo && t.BunkRen == dataupdate.HAISHA_BunkRen && t.UnkRen == dataupdate.HAISHA_UnkRen && t.TeiDanNo == dataupdate.HAISHA_TeiDanNo && !haiInRenlst.Contains(t.HaiInRen)).ToList();
                foreach (var item in updatenull)
                {
                    var update = _dbContext.TkdHaiin.Find(dataupdate.YYKSHO_UkeNo, dataupdate.HAISHA_UnkRen, dataupdate.HAISHA_TeiDanNo, dataupdate.HAISHA_BunkRen, item.HaiInRen);
                    update.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    update.UpdTime = DateTime.Now.ToString("HHmmss");
                    update.UpdPrgId = "KU0600";
                    update.SiyoKbn = 2;
                    update.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq; ;
                    update.HenKai = update.HenKai++;
                    update.SyokumuKbn = item.SyokumuKbn;
                    _dbContext.Update(update);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else
            {
                if (checkDeleteDriver == true)
                {
                    var checkDataHaiin = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.YYKSHO_UkeNo
                   && t.SiyoKbn == 1).ToList();
                    foreach (var item in checkDataHaiin)
                    {
                        item.SiyoKbn = 2;
                        _dbContext.Update(item);
                        await _dbContext.SaveChangesAsync();
                    }
                    int max = 0;
                    var gethaiinlst = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.YYKSHO_UkeNo && t.BunkRen == dataupdate.HAISHA_BunkRen && t.UnkRen == dataupdate.HAISHA_UnkRen && t.TeiDanNo == dataupdate.HAISHA_TeiDanNo).ToList();
                    if (gethaiinlst.Count() != 0)
                    {
                        max = gethaiinlst.Max(t => t.HaiInRen);
                    }
                    if (dataupdate.SYARYO_SyainCdSeq != 0)
                    {
                        TkdHaiin insertnew = new TkdHaiin();
                        insertnew.UkeNo = dataupdate.YYKSHO_UkeNo;
                        insertnew.BunkRen = dataupdate.HAISHA_BunkRen;
                        insertnew.UnkRen = dataupdate.HAISHA_UnkRen;
                        insertnew.TeiDanNo = dataupdate.HAISHA_TeiDanNo;
                        insertnew.Syukinbasy = "";
                        insertnew.TaiknTime = dataupdate.KikTime;
                        insertnew.TaiknBasy = "";
                        insertnew.HaiInRen = (byte)(max + 1);
                        insertnew.SyainCdSeq = dataupdate.SYARYO_SyainCdSeq;
                        insertnew.SyukinTime = dataupdate.SyuKoTime;
                        insertnew.SiyoKbn = 1;
                        insertnew.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        insertnew.UpdTime = DateTime.Now.ToString("HHmmss");
                        insertnew.UpdPrgId = "KU0600";
                        insertnew.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        insertnew.HenKai = 0;
                        insertnew.SyokumuKbn = dataupdate.SyokumuKbn;
                        _dbContext.TkdHaiin.Add(insertnew);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    var checkDataHaiinDefault_SyainCdSeq = _dbContext.TkdHaiin.Where(t => t.UkeNo == dataupdate.YYKSHO_UkeNo && t.UnkRen == dataupdate.HAISHA_UnkRen
                     && t.BunkRen == dataupdate.HAISHA_BunkRen && t.TeiDanNo == dataupdate.HAISHA_TeiDanNo).ToList();
                    if (checkDataHaiinDefault_SyainCdSeq.Count == 0)
                    {
                        if (dataupdate.SYARYO_SyainCdSeq != 0)
                        {
                            TkdHaiin insertnew = new TkdHaiin();
                            insertnew.UkeNo = dataupdate.YYKSHO_UkeNo;
                            insertnew.BunkRen = dataupdate.HAISHA_BunkRen;
                            insertnew.UnkRen = dataupdate.HAISHA_UnkRen;
                            insertnew.TeiDanNo = dataupdate.HAISHA_TeiDanNo;
                            insertnew.Syukinbasy = "";
                            insertnew.TaiknTime = dataupdate.KikTime;
                            insertnew.TaiknBasy = "";
                            insertnew.HaiInRen = 1;
                            insertnew.SyainCdSeq = dataupdate.SYARYO_SyainCdSeq;
                            insertnew.SyukinTime = dataupdate.SyuKoTime;
                            insertnew.SiyoKbn = 1;
                            insertnew.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            insertnew.UpdTime = DateTime.Now.ToString("HHmmss");
                            insertnew.UpdPrgId = "KU0600";
                            insertnew.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            insertnew.HenKai = 0;
                            insertnew.SyokumuKbn = dataupdate.SyokumuKbn;
                            _dbContext.TkdHaiin.Add(insertnew);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            
           
        }
       public async Task UpdateTKD_KobanBusallocation(BusAllocationDataUpdate dataupdate)
        {
            List<ItemStaff> lstStaffsLines = new List<ItemStaff>();
            List<StaffsLines> lstStafflines = new List<StaffsLines>();
            lstStaffsLines.Clear();
            lstStafflines = await _StaffsChartService.GetStaffdatahaiiinBusallocation(dataupdate.YYKSHO_UkeNo, dataupdate.HAISHA_UnkRen, dataupdate.HAISHA_TeiDanNo, dataupdate.HAISHA_BunkRen, new ClaimModel().TenantID);
            foreach (var items in lstStafflines)
            {
                ItemStaff staffItem = new ItemStaff();
                staffItem.BookingId = items.Haisha_UkeNo;
                staffItem.HaUnkRen = items.Haisha_UnkRen;
                staffItem.TeiDanNo = items.Haisha_TeiDanNo;
                staffItem.BunkRen = items.Haisha_BunkRen;
                staffItem.HenKai = 1;
                staffItem.Id = items.Yykasho_UkeCd.ToString();
                staffItem.UkeCd = items.Yykasho_UkeCd;
                staffItem.BusLine = items.Haiin_SyainCdSeq.ToString();
                staffItem.StartDate = dataupdate.HaiSYmd.ToString("yyyyMMdd");
                staffItem.TimeStart = int.Parse(dataupdate.HaiSTime);
                staffItem.EndDate = dataupdate.TouYmd.ToString("yyyyMMdd");
                staffItem.TimeEnd = int.Parse(dataupdate.TouChTime);
                staffItem.StartDateDefault = dataupdate.SyuKoYmd.ToString("yyyyMMdd");
                staffItem.EndDateDefault = dataupdate.KikYmd.ToString("yyyyMMdd");
                staffItem.TimeStartDefault = int.Parse(dataupdate.SyuKoTime);
                staffItem.TimeEndDefault = int.Parse(dataupdate.KikTime);
                staffItem.JyoSyaJin = items.Haisha_JyoSyaJin;
                staffItem.AllowEdit = true;
                staffItem.Status = 1;
                staffItem.CCSStyle = "";
                staffItem.Top = 0.3125;
                staffItem.Height = 2;
                staffItem.Name = "";
                staffItem.DanTaNm = items.Haisha_DanTaNm2;
                staffItem.IkNm = items.Haisha_IkNm;
                staffItem.TokuiNm = "";
                staffItem.NumberDriver = items.Haisha_DrvJin;
                staffItem.NumberGuider = items.Haisha_GuiSu;
                staffItem.MinDate = items.Unkobi_HaiSYmd;
                staffItem.MinTime = int.Parse(items.Unkobi_HaiSTime);
                staffItem.Maxdate = items.Unkobi_TouYmd;
                staffItem.MaxTime = int.Parse(items.Unkobi_TouChTime);
                staffItem.HasYmd = items.Haisha_HaiSYmd;
                staffItem.Zeiritsu = 1;
                staffItem.BookingType = 1;
                staffItem.CodeKb_CodeKbn = "";
                staffItem.KSKbn = items.Haisha_KSKbn;
                staffItem.YouTblSeq = 0;
                staffItem.SyaSyu_SyaSyuNm = items.Syasyu_SyaSyuNm;
                staffItem.SyaSyu_SyaSyuNm_Haisha = "";
                staffItem.BusLineType = items.Haisha_UkeNo;
                staffItem.Tokisk_YouSRyakuNm = null;
                staffItem.Tokisk_SitenCdSeq = 0;
                staffItem.TokiSk_RyakuNm = items.Tokisk_RyakuNm;
                staffItem.TokiSt_RyakuNm = items.Tokist_RyakuNm;
                staffItem.Shuri_ShuriTblSeq = 0;
                staffItem.SyaSyuRen = items.Haisha_SyaSyuRen;
                staffItem.BranchId = 0;
                staffItem.CompanyId = 0;
                staffItem.BunKSyuJyn = 1;
                staffItem.UnkoJKbn = items.Unkobi_UnkoJKbn;
                staffItem.unSyuKoYmd = items.Unkobi_SyukoYmd;
                staffItem.unKikYmd = items.Unkobi_KikYmd;
                staffItem.unHaiSYmd = items.Unkobi_HaiSYmd;
                staffItem.unTouYmd = items.Unkobi_TouYmd;
                staffItem.CanBeDeleted = false;
                staffItem.CanSimpledispatch = true;
                staffItem.BusVehicle = 0;
                if (items.Syokum_SyokumuKbn == 1 || items.Syokum_SyokumuKbn == 2)
                { staffItem.BusVehicle = 1; }
                if (items.Syokum_SyokumuKbn == 3 || items.Syokum_SyokumuKbn == 4)
                { staffItem.BusVehicle = 2; }
                staffItem.haiInRen = items.Haiin_HaiInRen;
                lstStaffsLines.Add(staffItem);
            }
            foreach (var itemhaiin in lstStaffsLines)
            {
                _KobanDataService.UpdateTimeKoban(itemhaiin.StartDateDefault, itemhaiin.TimeStartDefault.ToString("D4"), itemhaiin.EndDateDefault, itemhaiin.TimeEndDefault.ToString("D4"), int.Parse(itemhaiin.BusLine), _busBookingDataService.Getcompany(itemhaiin.unHaiSYmd, int.Parse(itemhaiin.BusLine)), itemhaiin, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, "KU0600");
            }
        }
        /// <summary>
        /// insert data to line green
        /// </summary>
        /// <param name="updatedBus"></param>
        /// <param name="userlogin"></param>
        /// <param name="YouCdSeq"></param>
        /// <param name="YouSitCdSeq"></param>
        public void Updatebusdatagreen(ItemBus updatedBus, int userlogin, int YouCdSeq, int YouSitCdSeq)
        {
            TkdYousha insertyoshadata = new TkdYousha();
            insertyoshadata.UkeNo = updatedBus.BookingId;
            insertyoshadata.UnkRen = 1;
            insertyoshadata.YouTblSeq = _dbContext.TkdYousha.Where(t => t.UkeNo == updatedBus.BookingId && t.UnkRen == updatedBus.haUnkRen).Count() + 1;
            insertyoshadata.HenKai = 0;
            insertyoshadata.YouCdSeq = YouCdSeq;
            insertyoshadata.YouSitCdSeq = YouSitCdSeq;
            int UriKbn = _dbContext.TkmKasSet.Where(t => t.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).First().UriKbn;
            insertyoshadata.HasYmd = updatedBus.HasYmd;
            if(UriKbn==2)
            {
             insertyoshadata.HasYmd = updatedBus.EndDate;
            }
            insertyoshadata.SihYotYmd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyyMMdd");
            insertyoshadata.SihYm =updatedBus.HasYmd.Substring(0,6);
            insertyoshadata.SyaRyoUnc = 0;
            insertyoshadata.ZeiKbn = 1;
            insertyoshadata.Zeiritsu = updatedBus.Zeiritsu;
            insertyoshadata.SyaRyoSyo = 0;
            insertyoshadata.TesuRitu = 0;
            insertyoshadata.SyaRyoTes = 0;
            insertyoshadata.JitaFlg = 0;
            insertyoshadata.CompanyCdSeq = 0;
            insertyoshadata.SihKbn = 1;
            insertyoshadata.ScouKbn = 1;
            insertyoshadata.SiyoKbn = 1;
            insertyoshadata.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            insertyoshadata.UpdTime = DateTime.Now.ToString("HHmmss");
            insertyoshadata.UpdSyainCd = userlogin;
            insertyoshadata.UpdPrgId = "KU0100";
            _dbContext.TkdYousha.Add(insertyoshadata);
            _dbContext.SaveChanges();

            var std = _dbContext.TkdHaisha.Find(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen);
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.Kskbn = 1;
            std.HaiSkbn = 1;
            std.HaiIkbn = 1;
            std.HaiSsryCdSeq = 0;
            std.KssyaRseq = 0;
            std.SyuEigCdSeq = 0;
            std.KikEigSeq = 0;
            std.UpdPrgId = Common.UpdPrgId;
            std.UpdSyainCd = userlogin;
            std.HenKai = (short)(std.HenKai + 1);
            std.YouTblSeq = insertyoshadata.YouTblSeq;
            std.YouKataKbn = (byte)updatedBus.YykSyu_KataKbn;
            _dbContext.Update(std);
            _dbContext.SaveChanges();
            TkdYouSyu insertyousyu = new TkdYouSyu();
            insertyousyu.UkeNo = updatedBus.BookingId;
            insertyousyu.UnkRen = 1;
            insertyousyu.YouTblSeq = insertyoshadata.YouTblSeq;
            insertyousyu.HenKai = 0;
            insertyousyu.SyaSyuRen = std.SyaSyuRen;
            insertyousyu.YouKataKbn=(byte)updatedBus.YykSyu_KataKbn;
            insertyousyu.SyaSyuDai = 1;
            insertyousyu.SyaSyuTan = 0;
            insertyousyu.SyaRyoUnc = 0;
            insertyousyu.SiyoKbn = 1;
            insertyousyu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            insertyousyu.UpdTime = DateTime.Now.ToString("HHmmss");
            insertyousyu.UpdSyainCd = userlogin;
            insertyousyu.UpdPrgId = "KU0100";
            _dbContext.TkdYouSyu.Add(insertyousyu);
            _dbContext.SaveChanges();
            TkdMihrim inserttkdMihrim = new TkdMihrim();
            inserttkdMihrim.UkeNo = updatedBus.BookingId;
            inserttkdMihrim.MihRen = _dbContext.TkdMihrim.Where(t => t.UkeNo == updatedBus.BookingId && t.UnkRen == updatedBus.haUnkRen).Count() >  (short)0 ? (short)(_dbContext.TkdMihrim.Where(t => t.UkeNo == updatedBus.BookingId && t.UnkRen == updatedBus.haUnkRen).Max(t => t.MihRen) + 1) :  (short)1;
            inserttkdMihrim.HenKai = 0;
            inserttkdMihrim.SihFutSyu = 1;
            inserttkdMihrim.UnkRen = updatedBus.haUnkRen;
            inserttkdMihrim.YouTblSeq=insertyoshadata.YouTblSeq;
            inserttkdMihrim.HaseiKin = 0;
            inserttkdMihrim.SyaRyoSyo = 0;
            inserttkdMihrim.SyaRyoTes = 0;
            inserttkdMihrim.YoushaGak = 0;
            inserttkdMihrim.SihRaiRui = 0;
            inserttkdMihrim.CouKesRui = 0;
            inserttkdMihrim.YouFutTumRen = 0;
            inserttkdMihrim.SiyoKbn = 1;
            inserttkdMihrim.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            inserttkdMihrim.UpdTime = DateTime.Now.ToString("HHmmss");
            inserttkdMihrim.UpdSyainCd = userlogin;
            inserttkdMihrim.UpdPrgId = "KU0100";
            _dbContext.TkdMihrim.Add(inserttkdMihrim);
            _dbContext.SaveChanges();
            _busBookingDataService.UpdateUnkobi(updatedBus.BookingId, updatedBus.haUnkRen, "KU0100");
            _busBookingDataService.UpdateYyksho(updatedBus.BookingId, new ClaimModel().TenantID, "KU0100");
            _HaiinDataService.DeleteHaiin(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedBus"></param>
        /// <param name="userlogin"></param>
        /// <param name="YouCdSeq"></param>
        /// <param name="YouSitCdSeq"></param>
        /// <param name="Zeiritsu"></param>
        /// <param name="KataKbn"></param>
        public void UpdatebusdatagreenwithHaisha(TkdHaisha updatedBus, int userlogin, int YouCdSeq, int YouSitCdSeq, decimal Zeiritsu, byte KataKbn)
        {
            TkdYousha insertyoshadata = new TkdYousha();
            insertyoshadata.UkeNo = updatedBus.UkeNo;
            insertyoshadata.UnkRen = 1;
            insertyoshadata.YouTblSeq = _dbContext.TkdYousha.Where(t => t.UkeNo == updatedBus.UkeNo && t.UnkRen== updatedBus.UnkRen).Count() + 1;
            insertyoshadata.HenKai = 0;
            insertyoshadata.YouCdSeq = YouCdSeq;
            insertyoshadata.YouSitCdSeq = YouSitCdSeq;
            int UriKbn = _dbContext.TkmKasSet.Where(t => t.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).First().UriKbn;
            insertyoshadata.HasYmd = updatedBus.HaiSymd;
            if (UriKbn == 2)
            {
                insertyoshadata.HasYmd = updatedBus.TouYmd;
            }
            insertyoshadata.SihYotYmd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyyMMdd");
            insertyoshadata.SihYm = updatedBus.HaiSymd.Substring(0, 6);
            insertyoshadata.SyaRyoUnc = 0;
            insertyoshadata.ZeiKbn = 1;
            insertyoshadata.Zeiritsu = Zeiritsu;
            insertyoshadata.SyaRyoSyo = 0;
            insertyoshadata.TesuRitu = 0;
            insertyoshadata.SyaRyoTes = 0;
            insertyoshadata.JitaFlg = 0;
            insertyoshadata.CompanyCdSeq = 0;
            insertyoshadata.SihKbn = 1;
            insertyoshadata.ScouKbn = 1;
            insertyoshadata.SiyoKbn = 1;
            insertyoshadata.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            insertyoshadata.UpdTime = DateTime.Now.ToString("HHmmss");
            insertyoshadata.UpdSyainCd = userlogin;
            insertyoshadata.UpdPrgId = "KU0100";
            _dbContext.TkdYousha.Add(insertyoshadata);
            _dbContext.SaveChanges();

            var std = _dbContext.TkdHaisha.Find(updatedBus.UkeNo, updatedBus.UnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen);
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.Kskbn = 1;
            std.HaiSsryCdSeq = 0;
            std.KssyaRseq = 0;
            std.SyuEigCdSeq = 0;
            std.KikEigSeq = 0;
            std.UpdPrgId = Common.UpdPrgId;
            std.UpdSyainCd = userlogin;
            std.HenKai = (short)(std.HenKai + 1);
            std.YouTblSeq = insertyoshadata.YouTblSeq;
            std.YouKataKbn = KataKbn;
            _dbContext.Update(std); 
            _dbContext.SaveChanges();
            TkdYouSyu insertyousyu = new TkdYouSyu();
            insertyousyu.UkeNo = updatedBus.UkeNo;
            insertyousyu.UnkRen = 1;
            insertyousyu.YouTblSeq = insertyoshadata.YouTblSeq;
            insertyousyu.HenKai = 0;
            insertyousyu.SyaSyuRen = std.SyaSyuRen;
            insertyousyu.YouKataKbn = KataKbn;
            insertyousyu.SyaSyuDai = 1;
            insertyousyu.SyaSyuTan = 0;
            insertyousyu.SyaRyoUnc = 0;
            insertyousyu.SiyoKbn = 1;
            insertyousyu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            insertyousyu.UpdTime = DateTime.Now.ToString("HHmmss");
            insertyousyu.UpdSyainCd = userlogin;
            insertyousyu.UpdPrgId = "KU0100";
            _dbContext.TkdYouSyu.Add(insertyousyu);
            _dbContext.SaveChanges();
            _busBookingDataService.UpdateUnkobi(updatedBus.UkeNo, updatedBus.UnkRen, "KU0100");
            _busBookingDataService.UpdateYyksho(updatedBus.UkeNo, new ClaimModel().TenantID, "KU0100");
            _HaiinDataService.DeleteHaiin(updatedBus.UkeNo, updatedBus.UnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen);
            _MihrimService.DeleteMihrim(updatedBus.UkeNo, updatedBus.UnkRen, insertyoshadata.YouTblSeq);
        }
        /// <summary>
        /// insert data to line gray
        /// </summary>
        /// <param name="updatedBus"></param>
        /// <param name="userlogin"></param>
        public void Updatebusdatagray(ItemBus updatedBus, int userlogin)
        {
            try
            {
                var std = _dbContext.TkdHaisha.Find(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen);
                if (updatedBus.YouTblSeq > 0)
                {
                    var updateTkdYousha = _dbContext.TkdYousha.Find(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.YouTblSeq);
                    updateTkdYousha.SiyoKbn = 2;
                    _dbContext.SaveChanges();
                }
                std.HaiSsryCdSeq = 0;
                std.YouTblSeq = 0;
                std.KssyaRseq = 0;
                std.SyuEigCdSeq = 0;
                std.KikEigSeq = 0;
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU0100";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);
                std.Kskbn = 1;
                _dbContext.Update(std);
                _dbContext.SaveChanges();
                _busBookingDataService.UpdateUnkobi(updatedBus.BookingId, updatedBus.haUnkRen,"KU0100");
                _busBookingDataService.UpdateYyksho(updatedBus.BookingId, new ClaimModel().TenantID,"KU0100");
            }
            catch
            {
            }

        }

        /// <summary>
        /// update bus line
        /// </summary>
        /// <param name="updatedBus"></param>
        /// <param name="userlogin"></param>
        public void Updatebusdata(ItemBus updatedBus, int userlogin)
        {
            if (updatedBus.BookingId != "0" && updatedBus.BookingId != "-1")
            {
                if (updatedBus.YouTblSeq > 0)
                {
                    var updateTkdYousha = _dbContext.TkdYousha.Find(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.YouTblSeq);
                    updateTkdYousha.SiyoKbn = 2;
                    updateTkdYousha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    updateTkdYousha.UpdTime = DateTime.Now.ToString("HHmmss");
                    updateTkdYousha.UpdPrgId = "KU0100";
                    updateTkdYousha.UpdSyainCd = userlogin;
                    updateTkdYousha.HenKai = (short)(updateTkdYousha.HenKai + 1);
                    var TkdYouSyulst = _dbContext.TkdYouSyu.Where(t => t.UkeNo == updatedBus.BookingId && t.UnkRen == updatedBus.haUnkRen && t.YouTblSeq == updatedBus.YouTblSeq && t.SiyoKbn == 1).ToList();
                   foreach(var item in TkdYouSyulst)
                    {
                        var updateTkdYouSyu = _dbContext.TkdYouSyu.Find(item.UkeNo, item.UnkRen, item.YouTblSeq, item.SyaSyuRen);
                        updateTkdYouSyu.SiyoKbn = 2;
                        updateTkdYouSyu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        updateTkdYouSyu.UpdTime = DateTime.Now.ToString("HHmmss");
                        updateTkdYouSyu.UpdPrgId = "KU0100";
                        updateTkdYouSyu.UpdSyainCd = userlogin;
                        updateTkdYouSyu.HenKai = (short)(updateTkdYousha.HenKai + 1);
                    }    
                    _dbContext.SaveChanges();
                    _MihrimService.DeleteMihrim(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.YouTblSeq);
                    var checkyousha = _dbContext.TkdHaisha.Where(t => t.UkeNo == updatedBus.BookingId && t.UnkRen == updatedBus.haUnkRen && t.TeiDanNo != updatedBus.TeiDanNo && t.BunkRen == updatedBus.BunkRen && t.YouTblSeq== updatedBus.YouTblSeq).ToList();
                    if(checkyousha.Count>0)
                    {
                        foreach(var item in checkyousha)
                        {
                            var updateTkdHaisha = _dbContext.TkdHaisha.Find(item.UkeNo, item.UnkRen, item.TeiDanNo,item.BunkRen);
                            updateTkdHaisha.YouTblSeq = 0;
                            updateTkdHaisha.Kskbn = 1;
                            updateTkdHaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            updateTkdHaisha.UpdTime = DateTime.Now.ToString("HHmmss");
                            updateTkdHaisha.UpdPrgId = "KU0100";
                            updateTkdHaisha.UpdSyainCd = userlogin;
                            updateTkdHaisha.HenKai = (short)(updateTkdHaisha.HenKai + 1);
                             _dbContext.SaveChanges();
                        }    
                    }    
                }
                // #3069 work around
                var list = _dbContext.TkdHaisha.Where(h => h.UkeNo == updatedBus.BookingId && h.UnkRen == updatedBus.haUnkRen && h.TeiDanNo == updatedBus.TeiDanNo && h.SiyoKbn == 1).Select(h => new { h.SyaRyoUnc, h.SyaRyoSyo, h.SyaRyoTes, h.KikYmd, h.KikTime, h.TouYmd, h.TouChTime, h.BunkRen });
                foreach(var item in list.Where(t=>t.BunkRen!=updatedBus.BunkRen).ToList())
                {
                    var stdgray = _dbContext.TkdHaisha.Find(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.TeiDanNo, item.BunkRen);
                    // #3069 work around
                    stdgray.SyaRyoUnc = item.SyaRyoUnc;
                    stdgray.SyaRyoSyo = item.SyaRyoSyo;
                    stdgray.SyaRyoTes = item.SyaRyoTes;
                    stdgray.KikYmd = item.KikYmd;
                    stdgray.KikTime = item.KikTime;
                    stdgray.TouYmd = item.TouYmd;
                    stdgray.TouChTime = item.TouChTime;
                    // #3069 work around
                    stdgray.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    stdgray.UpdTime = DateTime.Now.ToString("HHmmss");
                    stdgray.UpdPrgId = "KU0100";
                    stdgray.UpdSyainCd = userlogin;
                    stdgray.HenKai = (short)(stdgray.HenKai + 1);
                    _dbContext.Update(stdgray);
                    _dbContext.SaveChanges();
                }
                var obj = list.Where(h => h.BunkRen == updatedBus.BunkRen).Single();
                // #3069 work around
                var std = _dbContext.TkdHaisha.Find(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen);
                // #3069 work around
                std.SyaRyoUnc = obj.SyaRyoUnc;
                std.SyaRyoSyo = obj.SyaRyoSyo;
                std.SyaRyoTes = obj.SyaRyoTes;
                std.KikYmd = obj.KikYmd;
                std.KikTime = obj.KikTime;
                std.TouYmd = obj.TouYmd;
                std.TouChTime = obj.TouChTime;
                // #3069 work around
                std.HaiSsryCdSeq = int.Parse(updatedBus.BusLine);
                std.KssyaRseq = int.Parse(updatedBus.BusLine);
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU0100";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);
                std.SyuEigCdSeq = _EigyosDataService.GetBranchbyBusId(int.Parse(updatedBus.BusLine), updatedBus.EndDate);
                std.KikEigSeq = _EigyosDataService.GetBranchbyBusId(int.Parse(updatedBus.BusLine), updatedBus.EndDate);
                std.YouTblSeq = 0;
                std.YouKataKbn = 9;
                std.Kskbn = 2;
                _dbContext.Update(std);
                _dbContext.SaveChanges();
            }
            if (updatedBus.BookingId == "-1")
            {
                TkdShuri updateShuri = new TkdShuri();
                updateShuri.SyaRyoCdSeq = int.Parse(updatedBus.BusLine);
                updateShuri.BikoNm = updatedBus.BikoNm == null ? "" : updatedBus.BikoNm;
                updateShuri.SiyoKbn = 1;
                updateShuri.ShuriCdSeq = updatedBus.ShuriCdSeq;
                updateShuri.ShuriSymd = updatedBus.StartDate;
                updateShuri.ShuriStime = updatedBus.TimeStart.ToString("D4");
                updateShuri.ShuriEymd = updatedBus.EndDate;
                updateShuri.ShuriEtime = updatedBus.TimeEnd.ToString("D4");
                updateShuri.HenKai += 1;
                updateShuri.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                updateShuri.UpdTime = DateTime.Now.ToString("hhmmss");
                updateShuri.UpdSyainCd = userlogin;
                updateShuri.UpdPrgId = "KU0100";
                _dbContext.TkdShuri.Add(updateShuri);
                _dbContext.SaveChanges();
                DateTime startdatene;
                DateTime.TryParseExact(updatedBus.StartDate,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out startdatene);

                DateTime enddatene;
                DateTime.TryParseExact(updatedBus.EndDate,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out enddatene);
                double totaldate = (enddatene - startdatene).TotalDays;
                if (totaldate == 0)
                {
                    TkdShuYmd newitem = new TkdShuYmd();
                    newitem.ShuriTblSeq = updateShuri.ShuriTblSeq;
                    newitem.ShuriYmd = updatedBus.StartDate;
                    newitem.HenKai = 0;
                    newitem.ShuriStime = updatedBus.TimeStart.ToString("D4");
                    newitem.ShuriEtime = updatedBus.TimeEnd.ToString("D4");
                    newitem.SiyoKbn = 1;
                    newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    newitem.UpdTime = DateTime.Now.ToString("hhmmss");
                    newitem.UpdSyainCd = userlogin;
                    newitem.UpdPrgId = "KU0100";
                    _dbContext.TkdShuYmd.Add(newitem);
                }
                else
                {
                    if (totaldate == 1)
                    {
                        TkdShuYmd newitemfirst = new TkdShuYmd();
                        newitemfirst.ShuriTblSeq = updateShuri.ShuriTblSeq;
                        newitemfirst.ShuriYmd = updatedBus.StartDate;
                        newitemfirst.HenKai = 0;
                        newitemfirst.ShuriStime = updatedBus.TimeStart.ToString("D4");
                        newitemfirst.ShuriEtime = "2359";
                        newitemfirst.SiyoKbn = 1;
                        newitemfirst.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        newitemfirst.UpdTime = DateTime.Now.ToString("hhmmss");
                        newitemfirst.UpdSyainCd = userlogin;
                        newitemfirst.UpdPrgId = "KU0100";
                        _dbContext.TkdShuYmd.Add(newitemfirst);

                        TkdShuYmd newitemlast = new TkdShuYmd();
                        newitemlast.ShuriTblSeq = updateShuri.ShuriTblSeq;
                        newitemlast.ShuriYmd = updatedBus.EndDate;
                        newitemlast.HenKai = 0;
                        newitemlast.ShuriStime = "0000";
                        newitemlast.ShuriEtime = updatedBus.TimeEnd.ToString("D4");
                        newitemlast.SiyoKbn = 1;
                        newitemlast.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        newitemlast.UpdTime = DateTime.Now.ToString("hhmmss");
                        newitemlast.UpdSyainCd = userlogin;
                        newitemlast.UpdPrgId = "KU0100";
                        _dbContext.TkdShuYmd.Add(newitemlast);
                    }
                    else
                    {
                        TkdShuYmd newitemfirst = new TkdShuYmd();
                        newitemfirst.ShuriTblSeq = updateShuri.ShuriTblSeq;
                        newitemfirst.ShuriYmd = updatedBus.StartDate;
                        newitemfirst.HenKai = 0;
                        newitemfirst.ShuriStime = updatedBus.TimeStart.ToString("D4");
                        newitemfirst.ShuriEtime = "2359";
                        newitemfirst.SiyoKbn = 1;
                        newitemfirst.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        newitemfirst.UpdTime = DateTime.Now.ToString("hhmmss");
                        newitemfirst.UpdSyainCd = userlogin;
                        newitemfirst.UpdPrgId = "KU0100";
                        _dbContext.TkdShuYmd.Add(newitemfirst);
                        for (int i = 1; i < totaldate; i++)
                        {
                            TkdShuYmd newitem = new TkdShuYmd();
                            newitem.ShuriTblSeq = updateShuri.ShuriTblSeq;
                            newitem.ShuriYmd = startdatene.AddDays(i).ToString("yyyyMMdd");
                            newitem.HenKai = 0;
                            newitem.ShuriStime = "0000";
                            newitem.ShuriEtime = "2359";
                            newitem.SiyoKbn = 1;
                            newitem.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            newitem.UpdTime = DateTime.Now.ToString("hhmmss");
                            newitem.UpdSyainCd = userlogin;
                            newitem.UpdPrgId = "KU0100";
                            _dbContext.TkdShuYmd.Add(newitem);
                        }
                        TkdShuYmd newitemlast = new TkdShuYmd();
                        newitemlast.ShuriTblSeq = updateShuri.ShuriTblSeq;
                        newitemlast.ShuriYmd = updatedBus.EndDate;
                        newitemlast.HenKai = 0;
                        newitemlast.ShuriStime = "0000";
                        newitemlast.ShuriEtime = updatedBus.TimeEnd.ToString("D4");
                        newitemlast.SiyoKbn = 1;
                        newitemlast.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        newitemlast.UpdTime = DateTime.Now.ToString("hhmmss");
                        newitemlast.UpdSyainCd = userlogin;
                        newitemlast.UpdPrgId = "KU0100";
                        _dbContext.TkdShuYmd.Add(newitemlast);
                    }

                }
                _dbContext.SaveChanges();

            }
        }

        /// <summary>
        /// update time in bus line
        /// </summary>
        /// <param name="updatedBus"></param>
        /// <param name="userlogin"></param>
        /// <param name="datestart"></param>
        /// <param name="dateend"></param>
        public async void Updatebustimedata(ItemBus updatedBus, int userlogin, DateTime datestart, DateTime dateend)
        {
            if (updatedBus.BookingId != "0")
            {
                var std = _dbContext.TkdHaisha.Find(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen);
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                DateTime startdateold;
                DateTime.TryParseExact(std.HaiSymd + std.HaiStime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out startdateold);
                DateTime startdefaultdateold;
                DateTime.TryParseExact(std.SyuKoYmd + std.SyuKoTime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out startdefaultdateold);
                Double reststart = ((TimeSpan)(startdateold - startdefaultdateold)).TotalMinutes;

                string enddatest = std.TouYmd;
                string endtime = std.TouChTime;
                DateTime enddatetime = DateTime.ParseExact(std.TouYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (_busScheduleHelper.ConvertTime(std.KikTime).Days >= 1)
                {
                    enddatest = enddatetime.AddDays(_busScheduleHelper.ConvertTime(std.TouChTime).Days).ToString("yyyyMMdd");
                    endtime = _busScheduleHelper.ConvertTime(std.TouChTime).Hours.ToString("D2") + _busScheduleHelper.ConvertTime(std.TouChTime).Minutes.ToString("D2");
                }

                DateTime enddateold;
                DateTime.TryParseExact(enddatest + endtime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out enddateold);
                string enddatestrgry = std.KikYmd;
                string endtimegry = std.KikTime;
                DateTime enddatetimegry = DateTime.ParseExact(std.KikYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (_busScheduleHelper.ConvertTime(std.KikTime).Days >= 1)
                {
                    enddatestrgry = enddatetimegry.AddDays(_busScheduleHelper.ConvertTime(std.KikTime).Days).ToString("yyyyMMdd");
                    endtimegry = _busScheduleHelper.ConvertTime(std.KikTime).Hours.ToString("D2") + _busScheduleHelper.ConvertTime(std.KikTime).Minutes.ToString("D2");
                }
                DateTime enddefaultdateold;
                DateTime.TryParseExact(enddatestrgry + endtimegry,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out enddefaultdateold);
                double restend = ((TimeSpan)(enddefaultdateold - enddateold)).TotalMinutes;
                std.HaiSymd = datestart.ToString("yyyyMMdd");
                std.HaiStime = datestart.ToString("HHmm");
                std.SyuKoYmd = datestart.AddMinutes(-reststart).ToString("yyyyMMdd");
                std.SyuKoTime = datestart.AddMinutes(-reststart).ToString("HHmm");
                std.TouYmd = dateend.ToString("yyyyMMdd");
                std.TouChTime = dateend.ToString("HHmm");
                std.KikYmd = dateend.AddMinutes(restend).ToString("yyyyMMdd");
                std.KikTime = dateend.AddMinutes(restend).ToString("HHmm");
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU0100";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);
                _dbContext.Update(std);
                _dbContext.SaveChanges();
                List<ItemStaff> lstStaffsLines = new List<ItemStaff>();
                List<StaffsLines> lstStafflines = new List<StaffsLines>();
                lstStafflines = await _StaffsChartService.GetStaffdatabookingbyid(updatedBus.BookingId, updatedBus.haUnkRen, updatedBus.TeiDanNo, updatedBus.BunkRen, new ClaimModel().TenantID);
                foreach (var item in lstStafflines)
                {
                    ItemStaff staffItem = new ItemStaff();
                    staffItem.BookingId = item.Haisha_UkeNo;
                    staffItem.HaUnkRen = item.Haisha_UnkRen;
                    staffItem.TeiDanNo = item.Haisha_TeiDanNo;
                    staffItem.BunkRen = item.Haisha_BunkRen;
                    staffItem.HenKai = 1;
                    staffItem.Id = item.Yykasho_UkeCd.ToString();
                    staffItem.UkeCd = item.Yykasho_UkeCd;
                    staffItem.BusLine = item.Haiin_SyainCdSeq.ToString();
                    staffItem.StartDate = item.Haisha_HaiSYmd;
                    staffItem.TimeStart = int.Parse(item.Haisha_HaiSTime);
                    staffItem.EndDate = item.Haisha_TouYmd;
                    staffItem.TimeEnd = int.Parse(item.Haisha_TouChTime);
                    staffItem.StartDateDefault = item.Haisha_SyuKoYmd;
                    staffItem.EndDateDefault = item.Haisha_KikYmd;
                    staffItem.TimeStartDefault = int.Parse(item.Haisha_SyuKoTime);
                    staffItem.TimeEndDefault = int.Parse(item.Haisha_KikTime);
                    staffItem.JyoSyaJin = item.Haisha_JyoSyaJin;
                    staffItem.AllowEdit = true;
                    staffItem.Status = 1;
                    staffItem.CCSStyle = "";
                    staffItem.Top = 0.3125;
                    staffItem.Height = 2;
                    staffItem.Name = "";
                    staffItem.DanTaNm = item.Haisha_DanTaNm2;
                    staffItem.IkNm = item.Haisha_IkNm;
                    staffItem.TokuiNm = "";
                    staffItem.NumberDriver = item.Haisha_DrvJin;
                    staffItem.NumberGuider = item.Haisha_GuiSu;
                    staffItem.MinDate = item.Unkobi_HaiSYmd;
                    staffItem.MinTime = int.Parse(item.Unkobi_HaiSTime);
                    staffItem.Maxdate = item.Unkobi_TouYmd;
                    staffItem.MaxTime = int.Parse(item.Unkobi_TouChTime);
                    staffItem.HasYmd = item.Haisha_HaiSYmd;
                    staffItem.Zeiritsu = 1;
                    staffItem.BookingType = 1;
                    staffItem.CodeKb_CodeKbn = "";
                    staffItem.KSKbn = item.Haisha_KSKbn;
                    staffItem.YouTblSeq = 0;
                    staffItem.SyaSyu_SyaSyuNm = item.Syasyu_SyaSyuNm;
                    staffItem.SyaSyu_SyaSyuNm_Haisha = "";
                    staffItem.BusLineType = item.Haisha_UkeNo;
                    staffItem.Tokisk_YouSRyakuNm = null;
                    staffItem.Tokisk_SitenCdSeq = 0;
                    staffItem.TokiSk_RyakuNm = item.Tokisk_RyakuNm;
                    staffItem.TokiSt_RyakuNm = item.Tokist_RyakuNm;
                    staffItem.Shuri_ShuriTblSeq = 0;
                    staffItem.SyaSyuRen = item.Haisha_SyaSyuRen;
                    staffItem.BranchId = 0;
                    staffItem.CompanyId = 0;
                    staffItem.BunKSyuJyn = 1;
                    staffItem.UnkoJKbn = item.Unkobi_UnkoJKbn;
                    staffItem.unSyuKoYmd = item.Unkobi_SyukoYmd;
                    staffItem.unKikYmd = item.Unkobi_KikYmd;
                    staffItem.unHaiSYmd = item.Unkobi_HaiSYmd;
                    staffItem.unTouYmd = item.Unkobi_TouYmd;
                    staffItem.CanBeDeleted = false;
                    staffItem.CanSimpledispatch = true;
                    staffItem.BusVehicle = 0;
                    if (item.Syokum_SyokumuKbn == 1 || item.Syokum_SyokumuKbn == 2)
                    { staffItem.BusVehicle = 1; }
                    if (item.Syokum_SyokumuKbn == 3 || item.Syokum_SyokumuKbn == 4)
                    { staffItem.BusVehicle = 2; }
                    staffItem.haiInRen = item.Haiin_HaiInRen;
                    lstStaffsLines.Add(staffItem);
                    foreach (var itemhaiin in lstStaffsLines)
                    {
                        _KobanDataService.DeleteKobanbyUkeno(itemhaiin.BookingId, itemhaiin.HaUnkRen, itemhaiin.TeiDanNo, itemhaiin.BunkRen, int.Parse(itemhaiin.BusLine));
                        _KobanDataService.UpdateTimeKoban(itemhaiin.StartDateDefault, itemhaiin.TimeStartDefault.ToString("D4"), itemhaiin.EndDateDefault, itemhaiin.TimeEndDefault.ToString("D4"), int.Parse(itemhaiin.BusLine), _busBookingDataService.Getcompany(itemhaiin.unHaiSYmd, int.Parse(itemhaiin.BusLine)), itemhaiin, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,"KU0100");
                    }
                }
            }
        }


        /// <summary>
        /// update time in bus line
        /// </summary>
        /// <param name="updatedBus"></param>
        /// <param name="userlogin"></param>
        /// <param name="datestart"></param>
        /// <param name="dateend"></param>
        public async void UpdateStafftimedata(ItemStaff updatedStaff, int userlogin, DateTime datestart, DateTime dateend)
        {
            if (updatedStaff.BookingId != "0")
            {
                var std = _dbContext.TkdHaisha.Find(updatedStaff.BookingId, updatedStaff.HaUnkRen, updatedStaff.TeiDanNo, updatedStaff.BunkRen);
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                DateTime startdateold;
                DateTime.TryParseExact(std.HaiSymd + std.HaiStime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out startdateold);
                DateTime startdefaultdateold;
                DateTime.TryParseExact(std.SyuKoYmd + std.SyuKoTime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out startdefaultdateold);
                Double reststart = ((TimeSpan)(startdateold - startdefaultdateold)).TotalMinutes;

                string enddatest = std.TouYmd;
                string endtime = std.TouChTime;
                DateTime enddatetime = DateTime.ParseExact(std.TouYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (_busScheduleHelper.ConvertTime(std.KikTime).Days >= 1)
                {
                    enddatest = enddatetime.AddDays(_busScheduleHelper.ConvertTime(std.TouChTime).Days).ToString("yyyyMMdd");
                    endtime = _busScheduleHelper.ConvertTime(std.TouChTime).Hours.ToString("D2") + _busScheduleHelper.ConvertTime(std.TouChTime).Minutes.ToString("D2");
                }

                DateTime enddateold;
                DateTime.TryParseExact(enddatest + endtime,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out enddateold);
                string enddatestrgry = std.KikYmd;
                string endtimegry = std.KikTime;
                DateTime enddatetimegry = DateTime.ParseExact(std.KikYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (_busScheduleHelper.ConvertTime(std.KikTime).Days >= 1)
                {
                    enddatestrgry = enddatetimegry.AddDays(_busScheduleHelper.ConvertTime(std.KikTime).Days).ToString("yyyyMMdd");
                    endtimegry = _busScheduleHelper.ConvertTime(std.KikTime).Hours.ToString("D2") + _busScheduleHelper.ConvertTime(std.KikTime).Minutes.ToString("D2");
                }
                DateTime enddefaultdateold;
                DateTime.TryParseExact(enddatestrgry + endtimegry,
                                           "yyyyMMddHHmm",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out enddefaultdateold);
                double restend = ((TimeSpan)(enddefaultdateold - enddateold)).TotalMinutes;
                std.HaiSymd = datestart.ToString("yyyyMMdd");
                std.HaiStime = datestart.ToString("HHmm");
                std.SyuKoYmd = datestart.AddMinutes(-reststart).ToString("yyyyMMdd");
                std.SyuKoTime = datestart.AddMinutes(-reststart).ToString("HHmm");
                std.TouYmd = dateend.ToString("yyyyMMdd");
                std.TouChTime = dateend.ToString("HHmm");
                std.KikYmd = dateend.AddMinutes(restend).ToString("yyyyMMdd");
                std.KikTime = dateend.AddMinutes(restend).ToString("HHmm");
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU1300";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);
                _dbContext.Update(std);
                _dbContext.SaveChanges();
                List<Driverlst> driverlst = new List<Driverlst>();
                driverlst = await _busBookingDataService.GetbusdriverbyHaiSha(updatedStaff.BookingId, updatedStaff.HaUnkRen, updatedStaff.TeiDanNo, updatedStaff.BunkRen,updatedStaff.StartDate);
                if (driverlst.Count != 0)
                foreach(var item in driverlst)
                {
                    _KobanDataService.DeleteKobanbyUkeno(updatedStaff.BookingId, updatedStaff.HaUnkRen, updatedStaff.TeiDanNo, updatedStaff.BunkRen, item.SyainCdSeq);
                    _KobanDataService.UpdateTimeKoban(std.SyuKoYmd, std.SyuKoTime, std.KikYmd, std.KikTime, item.SyainCdSeq,item.CompanyCdSeq, updatedStaff, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,"KU1300");
                }    
            }
        }
        /// <summary>
        /// merge bus line when split
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <param name="userlogin"></param>
        public async void UpdatebusMergedata(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, int userlogin,ItemStaff updatedStaff,string formNm,byte nippoKbn)
        {
            // #3069 work around
            var list = _dbContext.TkdHaisha.Where(h => h.UkeNo == UkeNo && h.UnkRen == UnkRen && h.TeiDanNo == TeiDanNo && h.SiyoKbn == 1).Select(h => new { h.BunkRen, h.SyaRyoUnc, h.SyaRyoSyo, h.SyaRyoTes, });
            var std = _dbContext.TkdHaisha.Find(UkeNo, UnkRen, TeiDanNo, BunkRen);
            var updatecutline = _dbContext.TkdHaisha.Where(t => t.UkeNo == UkeNo && t.TeiDanNo == TeiDanNo && t.SiyoKbn == 1).OrderBy(t => t.HaiSymd).ThenBy(t => t.HaiStime).ToList();
            std.BunKsyuJyn = 0;
            std.SyaRyoUnc = list.Sum(t => t.SyaRyoUnc);
            std.SyaRyoSyo = list.Sum(t => t.SyaRyoSyo);
            std.SyaRyoTes = list.Sum(t => t.SyaRyoTes);
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.UpdPrgId = formNm;
            std.UpdSyainCd = userlogin;
            std.HenKai = (short)(std.HenKai + 1);
            std.SyuKoYmd = updatecutline.First().SyuKoYmd;
            std.SyuKoTime = updatecutline.First().SyuKoTime;
            std.SyuPaTime= updatecutline.First().SyuPaTime;
            std.HaiSymd = updatecutline.First().HaiSymd;
            std.HaiStime = updatecutline.First().HaiStime;
            std.KikYmd = updatecutline.Last().KikYmd;
            std.KikTime = updatecutline.Last().KikTime;
            std.TouYmd = updatecutline.Last().TouYmd;
            std.TouChTime = updatecutline.Last().TouChTime;
            std.NippoKbn =nippoKbn;
            _dbContext.Update(std);
            _dbContext.SaveChanges();
            updatecutline.Remove(std);
            foreach (var item in updatecutline)
            {
                var stdupdate = _dbContext.TkdHaisha.Find(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen);
                stdupdate.SiyoKbn = 2;
                stdupdate.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                stdupdate.UpdTime = DateTime.Now.ToString("HHmmss");
                stdupdate.UpdPrgId = formNm;
                stdupdate.UpdSyainCd = userlogin;
                stdupdate.HenKai = (short)(std.HenKai + 1);
                _dbContext.Update(stdupdate);
                _dbContext.SaveChanges();
                _HaiinDataService.DeleteHaiin(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen);
                _YoshaDataService.DeleteYoshaData(item.UkeNo, item.UnkRen, item.YouTblSeq, formNm);
                _YoshaDataService.DeleteYouSyuData(item.UkeNo, item.UnkRen, item.YouTblSeq,item.SyaSyuRen, formNm);
                await _KobanDataService.DeleteKobanbyHaisha(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen);
            }
            _busBookingDataService.UpdateUnkobi(UkeNo, UnkRen, "KU0100");
            _busBookingDataService.UpdateYyksho(UkeNo, new ClaimModel().TenantID, "KU0100");

            //if (updatedStaff.BookingId != "0")
            //{
            //    List<Driverlst> driverlst = new List<Driverlst>();
            //    driverlst = await _busBookingDataService.GetbusdriverbyHaiSha(updatedStaff.BookingId, updatedStaff.HaUnkRen, updatedStaff.TeiDanNo, updatedStaff.BunkRen, updatedStaff.StartDate);
            //    if (driverlst.Count != 0)
            //        foreach (var item in driverlst)
            //        {
            //            _KobanDataService.DeleteKobanbyUkeno(updatedStaff.BookingId, updatedStaff.HaUnkRen, updatedStaff.TeiDanNo, updatedStaff.BunkRen, item.SyainCdSeq);
            //            _KobanDataService.UpdateTimeKoban(std.SyuKoYmd, std.SyuKoTime, std.KikYmd, std.KikTime, item.SyainCdSeq, item.CompanyCdSeq, updatedStaff, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,formNm);
            //        }
            //}
            List<ItemStaff> lstStaffsLines = new List<ItemStaff>();
            List<StaffsLines> lstStafflines = new List<StaffsLines>();
            lstStafflines = await _StaffsChartService.GetStaffdatabookingbyid(UkeNo, UnkRen, TeiDanNo, BunkRen, new ClaimModel().TenantID);
            foreach (var item in lstStafflines)
            {
                ItemStaff staffItem = new ItemStaff();
                staffItem.BookingId = item.Haisha_UkeNo;
                staffItem.HaUnkRen = item.Haisha_UnkRen;
                staffItem.TeiDanNo = item.Haisha_TeiDanNo;
                staffItem.BunkRen = item.Haisha_BunkRen;
                staffItem.HenKai = 1;
                staffItem.Id = item.Yykasho_UkeCd.ToString();
                staffItem.UkeCd = item.Yykasho_UkeCd;
                staffItem.BusLine = item.Haiin_SyainCdSeq.ToString();
                staffItem.StartDate = item.Haisha_HaiSYmd;
                staffItem.TimeStart = int.Parse(item.Haisha_HaiSTime);
                staffItem.EndDate = item.Haisha_TouYmd;
                staffItem.TimeEnd = int.Parse(item.Haisha_TouChTime);
                staffItem.StartDateDefault = item.Haisha_SyuKoYmd;
                staffItem.EndDateDefault = item.Haisha_KikYmd;
                staffItem.TimeStartDefault = int.Parse(item.Haisha_SyuKoTime);
                staffItem.TimeEndDefault = int.Parse(item.Haisha_KikTime);
                staffItem.JyoSyaJin = item.Haisha_JyoSyaJin;
                staffItem.AllowEdit = true;
                staffItem.Status = 1;
                staffItem.CCSStyle = "";
                staffItem.Top = 0.3125;
                staffItem.Height = 2;
                staffItem.Name = "";
                staffItem.DanTaNm = item.Haisha_DanTaNm2;
                staffItem.IkNm = item.Haisha_IkNm;
                staffItem.TokuiNm = "";
                staffItem.NumberDriver = item.Haisha_DrvJin;
                staffItem.NumberGuider = item.Haisha_GuiSu;
                staffItem.MinDate = item.Unkobi_HaiSYmd;
                staffItem.MinTime = int.Parse(item.Unkobi_HaiSTime);
                staffItem.Maxdate = item.Unkobi_TouYmd;
                staffItem.MaxTime = int.Parse(item.Unkobi_TouChTime);
                staffItem.HasYmd = item.Haisha_HaiSYmd;
                staffItem.Zeiritsu = 1;
                staffItem.BookingType = 1;
                staffItem.CodeKb_CodeKbn = "";
                staffItem.KSKbn = item.Haisha_KSKbn;
                staffItem.YouTblSeq = 0;
                staffItem.SyaSyu_SyaSyuNm = item.Syasyu_SyaSyuNm;
                staffItem.SyaSyu_SyaSyuNm_Haisha = "";
                staffItem.BusLineType = item.Haisha_UkeNo;
                staffItem.Tokisk_YouSRyakuNm = null;
                staffItem.Tokisk_SitenCdSeq = 0;
                staffItem.TokiSk_RyakuNm = item.Tokisk_RyakuNm;
                staffItem.TokiSt_RyakuNm = item.Tokist_RyakuNm;
                staffItem.Shuri_ShuriTblSeq = 0;
                staffItem.SyaSyuRen = item.Haisha_SyaSyuRen;
                staffItem.BranchId = 0;
                staffItem.CompanyId = 0;
                staffItem.BunKSyuJyn = 1;
                staffItem.UnkoJKbn = item.Unkobi_UnkoJKbn;
                staffItem.unSyuKoYmd = item.Unkobi_SyukoYmd;
                staffItem.unKikYmd = item.Unkobi_KikYmd;
                staffItem.unHaiSYmd = item.Unkobi_HaiSYmd;
                staffItem.unTouYmd = item.Unkobi_TouYmd;
                staffItem.CanBeDeleted = false;
                staffItem.CanSimpledispatch = true;
                staffItem.BusVehicle = 0;
                if (item.Syokum_SyokumuKbn == 1 || item.Syokum_SyokumuKbn == 2)
                { staffItem.BusVehicle = 1; }
                if (item.Syokum_SyokumuKbn == 3 || item.Syokum_SyokumuKbn == 4)
                { staffItem.BusVehicle = 2; }
                staffItem.haiInRen = item.Haiin_HaiInRen;
                lstStaffsLines.Add(staffItem);
                foreach (var itemhaiin in lstStaffsLines)
                {
                    _KobanDataService.DeleteKobanbyUkeno(itemhaiin.BookingId, itemhaiin.HaUnkRen, itemhaiin.TeiDanNo, itemhaiin.BunkRen, int.Parse(itemhaiin.BusLine));
                    _KobanDataService.UpdateTimeKoban(itemhaiin.StartDateDefault, itemhaiin.TimeStartDefault.ToString("D4"), itemhaiin.EndDateDefault, itemhaiin.TimeEndDefault.ToString("D4"), int.Parse(itemhaiin.BusLine), _busBookingDataService.Getcompany(itemhaiin.unHaiSYmd, int.Parse(itemhaiin.BusLine)), itemhaiin, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, "KU0100");
                }
            }

        }

        /// <summary>
        /// Split bus line
        /// </summary>
        /// <param name="UkeNo"></param>
        /// <param name="UnkRen"></param>
        /// <param name="TeiDanNo"></param>
        /// <param name="BunkRen"></param>
        /// <param name="busdate1"></param>
        /// <param name="busdate2"></param>
        /// <param name="userlogin"></param>
        public  async Task SplitBusUpdate(string UkeNo, short UnkRen, short TeiDanNo, short BunkRen, SplitBusData busdate1, SplitBusData busdate2, int userlogin,string formNm)
        {
            var std = _dbContext.TkdHaisha.Find(UkeNo, UnkRen, TeiDanNo, BunkRen);
            std.SyuKoYmd = busdate1.SyuKoYmd;
            std.SyuKoTime = busdate1.SyuKoTime;
            std.HaiSymd = busdate1.HaiSYmd;
            std.HaiStime = busdate1.HaiSTime;
            std.SyuPaTime = busdate1.SyuPaTime;
            std.TouYmd = busdate1.TouYmd;
            std.TouChTime = busdate1.TouChTime;
            std.KikYmd = busdate1.KikYmd;
            std.KikTime = busdate1.KikTime;
            std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            std.UpdTime = DateTime.Now.ToString("HHmmss");
            std.UpdPrgId = formNm;
            std.UpdSyainCd = userlogin;
            int SyaRyoUnctmp = std.SyaRyoUnc / 2;
            int SyaRyoSyotmp = std.SyaRyoSyo / 2;
            int SyaRyoTestmp = std.SyaRyoTes / 2;
            int YoushaUnctmp = std.YoushaUnc / 2;
            int YoushaSyotmp = std.YoushaSyo / 2;
            int YoushaTestmp = std.YoushaTes / 2;
            if (std.SyaRyoUnc % 2 > 0)
            {
                std.SyaRyoUnc = SyaRyoUnctmp + 1;
            }
            else
            {
                std.SyaRyoUnc = SyaRyoUnctmp;
            }

            if (std.SyaRyoSyo % 2 > 0)
            {
                std.SyaRyoSyo = SyaRyoSyotmp + 1;
            }
            else
            {
                std.SyaRyoSyo = SyaRyoSyotmp;
            }

            if (std.SyaRyoTes % 2 > 0)
            {
                std.SyaRyoTes = SyaRyoTestmp + 1;
            }
            else
            {
                std.SyaRyoTes = SyaRyoTestmp;
            }

            if (std.YoushaUnc % 2 > 0)
            {
                std.YoushaUnc = YoushaUnctmp + 1;
            }
            else
            {
                std.YoushaUnc = YoushaUnctmp;
            }

            if (std.YoushaSyo % 2 > 0)
            {
                std.YoushaSyo = YoushaSyotmp + 1;
            }
            else
            {
                std.YoushaSyo = YoushaSyotmp;
            }

            if (std.YoushaTes % 2 > 0)
            {
                std.YoushaTes = YoushaTestmp + 1;
            }
            else
            {
                std.YoushaTes = YoushaTestmp;
            }
            std.HenKai = (short)(std.HenKai + 1);
            _dbContext.TkdHaisha.Update(std);
            _dbContext.SaveChanges();
            TkdHaisha newhaisha = std;
            newhaisha.BunkRen = (short)((from s in _dbContext.TkdHaisha where s.UkeNo == std.UkeNo && s.UnkRen == std.UnkRen && s.TeiDanNo == std.TeiDanNo orderby s.BunkRen descending select s.BunkRen).First() + 1);
            newhaisha.SyuKoYmd = busdate2.SyuKoYmd;
            newhaisha.SyuKoTime = busdate2.SyuKoTime;
            newhaisha.HaiSymd = busdate2.HaiSYmd;
            newhaisha.HaiStime = busdate2.HaiSTime;
            newhaisha.SyuPaTime = busdate2.SyuPaTime;
            newhaisha.TouYmd = busdate2.TouYmd;
            newhaisha.TouChTime = busdate2.TouChTime;
            newhaisha.KikYmd = busdate2.KikYmd;
            newhaisha.KikTime = busdate2.KikTime;
            newhaisha.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            newhaisha.UpdTime = DateTime.Now.ToString("HHmmss");
            newhaisha.UpdPrgId = formNm;
            newhaisha.UpdSyainCd = userlogin;
            newhaisha.HenKai = 0;
            newhaisha.SyaRyoUnc = SyaRyoUnctmp;
            newhaisha.SyaRyoSyo = SyaRyoSyotmp;
            newhaisha.SyaRyoTes = SyaRyoTestmp;
            newhaisha.YoushaUnc = YoushaUnctmp;
            newhaisha.YoushaSyo = YoushaSyotmp;
            newhaisha.YoushaTes = YoushaTestmp;
            _dbContext.TkdHaisha.Add(newhaisha);
            _dbContext.SaveChanges();
            var Getlistbooking = _dbContext.TkdHaisha.Where(t => t.UkeNo == UkeNo && t.TeiDanNo == TeiDanNo && t.SiyoKbn == 1).Select(h => new { h.UkeNo, h.UnkRen, h.TeiDanNo, h.BunkRen, h.HaiSymd, h.HaiStime, h.TouChTime }).OrderBy(t => t.HaiSymd).ThenBy(t => t.HaiStime).ToList();
            {
                short i = 1;
                foreach (var item in Getlistbooking)
                {
                    var stdupdate = _dbContext.TkdHaisha.Find(item.UkeNo, item.UnkRen, item.TeiDanNo, item.BunkRen);
                    // #3069 work around
                    var workaroundstdupdate = _dbContext.TkdHaisha.Where(h => h.UkeNo == item.UkeNo && h.TeiDanNo == item.TeiDanNo && h.BunkRen == item.BunkRen).Select(h => new { h.HaiSymd, h.HaiStime, h.KikYmd, h.KikTime, h.SyuKoYmd, h.SyuKoTime, h.TouYmd, h.TouChTime, h.SyaRyoUnc, h.SyaRyoSyo, h.SyaRyoTes, h.YoushaUnc, h.YoushaSyo, h.YoushaTes }).Single();
                    stdupdate.SyaRyoUnc = workaroundstdupdate.SyaRyoUnc;
                    stdupdate.SyaRyoSyo = workaroundstdupdate.SyaRyoSyo;
                    stdupdate.SyaRyoTes = workaroundstdupdate.SyaRyoTes;
                    stdupdate.YoushaUnc = workaroundstdupdate.YoushaUnc;
                    stdupdate.YoushaSyo = workaroundstdupdate.YoushaSyo;
                    stdupdate.YoushaTes = workaroundstdupdate.YoushaTes;
                    stdupdate.HaiSymd = workaroundstdupdate.HaiSymd;
                    stdupdate.HaiStime = workaroundstdupdate.HaiStime;
                    stdupdate.KikYmd = workaroundstdupdate.KikYmd;
                    stdupdate.KikTime = workaroundstdupdate.KikTime;
                    stdupdate.SyuKoYmd = workaroundstdupdate.SyuKoYmd;
                    stdupdate.SyuKoTime = workaroundstdupdate.SyuKoTime;
                    stdupdate.TouYmd = workaroundstdupdate.TouYmd;
                    stdupdate.TouChTime = workaroundstdupdate.TouChTime;
                    // #3069 work around
                    stdupdate.BunKsyuJyn = i;
                    stdupdate.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    stdupdate.UpdTime = DateTime.Now.ToString("HHmmss");
                    stdupdate.UpdPrgId = formNm;
                    stdupdate.UpdSyainCd = userlogin;
                    stdupdate.HenKai = (short)(std.HenKai + 1);
                    _dbContext.TkdHaisha.Update(stdupdate);
                    _dbContext.SaveChanges();
                    i++;
                }

            }
            if (std.Kskbn == 1 && std.YouTblSeq > 0)
            {
                int YouSitCdSeq = _YoshaDataService.GetdataYouSitCdSeq(std.UkeNo, std.UnkRen, std.YouTblSeq).Yosha_YouSitCdSeq;
                decimal Zeiritsu = _YoshaDataService.GetdataYouSitCdSeq(std.UkeNo, std.UnkRen, std.YouTblSeq).Yosha_Zeiritsu;
                int YouCdSeq = _YoshaDataService.GetdataYouSitCdSeq(std.UkeNo, std.UnkRen, std.YouTblSeq).Yosha_YouCdSeq;
                byte KataKbn = _YykSyuService.GetKatakbn(std.UkeNo, std.UnkRen, std.SyaSyuRen);
                UpdatebusdatagreenwithHaisha(newhaisha, newhaisha.UpdSyainCd, YouCdSeq, YouSitCdSeq, Zeiritsu, KataKbn);
            }
            var check_haiindata = _dbContext.TkdHaiin.Where(t => t.UkeNo == UkeNo && t.UnkRen == UnkRen && t.TeiDanNo == TeiDanNo && t.BunkRen == BunkRen).ToList();
            if(check_haiindata.Count()>0)
            {
                 
                foreach(var item in check_haiindata)
                {
                    await _HaiinDataService.UpdateStaffLineCutdata(UkeNo, UnkRen, TeiDanNo, newhaisha.BunkRen, 0, item.SyainCdSeq, userlogin, item);
                }
            }
        }

        /// <summary>
        /// Check valid before update data
        /// </summary>
        /// <param name="BusAllocationDataUpdate"></param>
        public async Task<bool> HaitaCheck(BusAllocationHaitaCheck busAllocationHaitaCheck, bool isCheckNull)
        {
            var busAllocationHaitaCheckNew = await _BusAllocationService.GetBusAllocationHaitaCheck(busAllocationHaitaCheck.UkeNo);
            if (isCheckNull)
            {
                busAllocationHaitaCheck.HAIIN_UpdYmdTime = null;
                busAllocationHaitaCheckNew.HAIIN_UpdYmdTime = null;
            }
            var busAllocationHaitaCheckString = JsonConvert.SerializeObject(busAllocationHaitaCheck);
            var busAllocationHaitaCheckNewString = JsonConvert.SerializeObject(busAllocationHaitaCheckNew);
            return busAllocationHaitaCheckString == busAllocationHaitaCheckNewString;
        }
    }
}
