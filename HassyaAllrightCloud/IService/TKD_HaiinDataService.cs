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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_HaiinDataService
    {
        void UpdateStaffMovetoGray(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte haiInRen, int userlogin,ItemStaff updatedStaff);
        void UpdateStaffLinedata(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte haiInRen, int syainCdSeq, int userlogin,ItemStaff updatedStaff);
        Task UpdateStaffLineCutdata(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte haiInRen, int syainCdSeq, int userlogin,TkdHaiin tkdHaiin);
        bool CheckDataHaiin(string ukeNo, short unkRen, short teiDanNo, short bunkRen,  int syainCdSeq);
        void DeleteHaiin(string ukeNo, short unkRen, short teiDanNo, short bunkRen);
    }
        public class TKD_HaiinDataService:ITKD_HaiinDataService
    {
        private readonly KobodbContext _dbContext;
        private readonly BusScheduleHelper _busScheduleHelper;
        private readonly ITKD_KobanDataService _KobanDataService;
        private readonly IStaffsChartService _StaffsChartService;
        private readonly IBusBookingDataListService _BusBookingDataListService;

        public TKD_HaiinDataService(KobodbContext context, BusScheduleHelper BusScheduleHelper, ITKD_KobanDataService TKD_KobanDataService,IStaffsChartService StaffsChartService,IBusBookingDataListService BusBookingDataListService)
        {
            _dbContext = context;
            _busScheduleHelper = BusScheduleHelper;
            _KobanDataService = TKD_KobanDataService;
            _StaffsChartService = StaffsChartService;
            _BusBookingDataListService = BusBookingDataListService;
        }

        public void DeleteHaiin(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            var getdatahaiin = _dbContext.TkdHaiin.Where(t => t.UkeNo == ukeNo && t.UnkRen == unkRen && t.TeiDanNo == teiDanNo && t.BunkRen == bunkRen).ToList();
            if(getdatahaiin.Count>0)
            {
                foreach(var item in getdatahaiin)
                {
                    var std = _dbContext.TkdHaiin.Find(ukeNo, unkRen, teiDanNo, bunkRen, item.HaiInRen);
                    if (std != null)
                    {
                        std.SiyoKbn = 2;
                        std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        std.UpdTime = DateTime.Now.ToString("HHmmss");
                        std.UpdPrgId = "KU1300";
                        std.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        std.HenKai = (short)(std.HenKai + 1);
                        _dbContext.TkdHaiin.Update(std);
                        _dbContext.SaveChanges();
                        _KobanDataService.DeleteKobanbyUkeno(ukeNo, unkRen, teiDanNo, bunkRen, std.SyainCdSeq);
                    }
                }                    
            }    
        }
        public bool CheckDataHaiin(string ukeNo, short unkRen, short teiDanNo, short bunkRen,  int syainCdSeq)
        {
            if(_dbContext.TkdHaiin.Where(t=>t.UkeNo==ukeNo&&t.UnkRen==unkRen&&t.TeiDanNo==teiDanNo&&t.BunkRen==bunkRen && t.SyainCdSeq==syainCdSeq).Count()>0)
            {
                return false;
            }
            return true;
        }
        public void UpdateStaffMovetoGray(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte haiInRen, int userlogin,ItemStaff updatedStaff)
        {
            var std = _dbContext.TkdHaiin.Find(ukeNo, unkRen, teiDanNo, bunkRen, haiInRen);
            if (std != null)
            {
                std.SiyoKbn = 2;
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU1300";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);
                _dbContext.TkdHaiin.Update(std);
                _dbContext.SaveChanges();
                _KobanDataService.DeleteKobanbyUkeno(ukeNo, unkRen, teiDanNo, bunkRen, std.SyainCdSeq);
            }
         }
        public void UpdateStaffLinedata(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte haiInRen, int syainCdSeq, int userlogin,ItemStaff updatedStaff)
        {
            var std = _dbContext.TkdHaiin.Find(ukeNo,unkRen,teiDanNo,bunkRen,haiInRen);
            if(std!=null)
            {
                int SyainCdSeqold = std.SyainCdSeq;
                std.SiyoKbn = 1;
                std.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                std.UpdTime = DateTime.Now.ToString("HHmmss");
                std.UpdPrgId = "KU1300";
                std.UpdSyainCd = userlogin;
                std.HenKai = (short)(std.HenKai + 1);
                std.SyainCdSeq = syainCdSeq;
                _dbContext.TkdHaiin.Update(std);
                _dbContext.SaveChanges();
                _KobanDataService.UpdateLineData(SyainCdSeqold, syainCdSeq, ukeNo, unkRen, teiDanNo, bunkRen, userlogin,updatedStaff);
            }
            else
            {

                TkdHaiin insertHaiin = new TkdHaiin();
                insertHaiin.UkeNo = ukeNo;
                insertHaiin.UnkRen = unkRen;
                insertHaiin.TeiDanNo = teiDanNo;
                insertHaiin.BunkRen = bunkRen;
                insertHaiin.HaiInRen = (byte)(_dbContext.TkdHaiin.Where(t => t.UkeNo == ukeNo && t.UnkRen == unkRen && t.TeiDanNo == teiDanNo && t.BunkRen == bunkRen).Count() == 0 ? 1 : _dbContext.TkdHaiin.Where(t => t.UkeNo == ukeNo && t.UnkRen == unkRen && t.TeiDanNo == teiDanNo && t.BunkRen == bunkRen).Max(t => t.HaiInRen)+1);
                insertHaiin.HenKai = 0;
                insertHaiin.SyainCdSeq = syainCdSeq;
                insertHaiin.SyukinTime = updatedStaff.TimeStart.ToString("D4");
                insertHaiin.TaiknTime = updatedStaff.TimeEnd.ToString("D4");
                insertHaiin.Syukinbasy = "";
                insertHaiin.TaiknBasy = "";
                insertHaiin.BikoTblSeq = 0;
                insertHaiin.SiyoKbn = 1;
                insertHaiin.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                insertHaiin.UpdTime = DateTime.Now.ToString("HHmmss");
                insertHaiin.UpdPrgId = "KU1300";
                insertHaiin.UpdSyainCd = userlogin;
                _dbContext.TkdHaiin.Add(insertHaiin);
                _dbContext.SaveChanges();
                _KobanDataService.UpdateLineData(0, syainCdSeq, ukeNo, unkRen, teiDanNo, bunkRen, userlogin,updatedStaff);
            }            
        }
        public async Task UpdateStaffLineCutdata(string ukeNo, short unkRen, short teiDanNo, short bunkRen, byte haiInRen, int syainCdSeq, int userlogin, TkdHaiin tkdHaiin)
        {
            TkdHaiin insertHaiin = new TkdHaiin();
            insertHaiin.UkeNo = ukeNo;
            insertHaiin.UnkRen = unkRen;
            insertHaiin.TeiDanNo = teiDanNo;
            insertHaiin.BunkRen = bunkRen;
            insertHaiin.HaiInRen = (byte)(_dbContext.TkdHaiin.Where(t => t.UkeNo == ukeNo && t.UnkRen == unkRen && t.TeiDanNo == teiDanNo && t.BunkRen == bunkRen).Count() == 0 ? 1 : _dbContext.TkdHaiin.Where(t => t.UkeNo == ukeNo && t.UnkRen == unkRen && t.TeiDanNo == teiDanNo && t.BunkRen == bunkRen).Max(t => t.HaiInRen) + 1);
            insertHaiin.HenKai = 0;
            insertHaiin.SyainCdSeq = syainCdSeq;
            insertHaiin.SyukinTime = tkdHaiin.SyukinTime;
            insertHaiin.TaiknTime = tkdHaiin.TaiknTime;
            insertHaiin.Syukinbasy = tkdHaiin.Syukinbasy;
            insertHaiin.TaiknBasy = tkdHaiin.TaiknBasy;
            insertHaiin.BikoTblSeq = 0;
            insertHaiin.SiyoKbn = 1;
            insertHaiin.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            insertHaiin.UpdTime = DateTime.Now.ToString("HHmmss");
            insertHaiin.UpdPrgId = "KU1300";
            insertHaiin.UpdSyainCd = userlogin;
            _dbContext.TkdHaiin.Add(insertHaiin);
            _dbContext.SaveChanges();
            List<ItemStaff> lstStaffsLines = new List<ItemStaff>();
            List<StaffsLines> lstStafflines = new List<StaffsLines>();
            lstStaffsLines.Clear();
            lstStafflines =  await _StaffsChartService.GetStaffdatahaiiin(ukeNo,unkRen,teiDanNo, 1, new ClaimModel().TenantID);
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
            }
            foreach (var itemhaiin in lstStaffsLines)
            {
                _KobanDataService.DeleteKobanbyUkeno(itemhaiin.BookingId, itemhaiin.HaUnkRen, itemhaiin.TeiDanNo, itemhaiin.BunkRen, int.Parse(itemhaiin.BusLine));
                _KobanDataService.UpdateTimeKoban(itemhaiin.StartDateDefault, itemhaiin.TimeStartDefault.ToString("D4"), itemhaiin.EndDateDefault, itemhaiin.TimeEndDefault.ToString("D4"), int.Parse(itemhaiin.BusLine), _BusBookingDataListService.Getcompany(itemhaiin.unHaiSYmd, int.Parse(itemhaiin.BusLine)), itemhaiin, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,"KU1300");
            }
        }
    }
}
