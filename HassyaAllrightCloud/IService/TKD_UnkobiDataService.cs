using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Net.Http;
using HassyaAllrightCloud.Infrastructure.Services;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Commons.Constants;
using System;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_UnkobiDataListService
    {
        Task<List<TkdUnkobi>> Get();
        Task<TkdUnkobi> Get(string id);

        MinMaxDate GetMinMaxDate(string id);
        void Add(IDictionary<string, object> tkd_unkobi);
        void InsertUnkobiFile(BookingFormData bookingdata, List<FileInfoData> filelst);
        Task<HttpResponseMessage> InsertTKD_UnkobiAsync(TkdUnkobi tkdUnkobi);
    }
    public class TKD_UnkobiDataService : ITKD_UnkobiDataListService
    {
        private readonly KobodbContext _dbContext;
        private readonly AppSettingsService _config;
        private readonly CustomHttpClient _httpClient;
        public TKD_UnkobiDataService(KobodbContext context, AppSettingsService config, CustomHttpClient httpClient)
        {
            _dbContext = context;
            _config = config;
            _httpClient = httpClient;
        }
        public void InsertUnkobiFile(BookingFormData bookingdata, List<FileInfoData> filelst)
        {
            foreach(var item in filelst)
            {
                if (item.FileNo == 0 && item.FolderId != null && item.FileId != null)
                {
                    TkdUnkobiFile inserttkdUnkobiFile = new TkdUnkobiFile();
                    inserttkdUnkobiFile.TenantCdSeq = new ClaimModel().TenantID;
                    inserttkdUnkobiFile.FileNo = _dbContext.TkdUnkobiFile.Where(t => t.TenantCdSeq == new ClaimModel().TenantID && t.UkeNo == bookingdata.UkeNo && t.UnkRen == bookingdata.UnkRen).Count() == 0 ? 1 : _dbContext.TkdUnkobiFile.Where(t => t.TenantCdSeq == new ClaimModel().TenantID && t.UkeNo == bookingdata.UkeNo && t.UnkRen == bookingdata.UnkRen).Max(t => t.FileNo) + 1;
                    inserttkdUnkobiFile.UkeNo = bookingdata.UkeNo;
                    inserttkdUnkobiFile.UnkRen = bookingdata.UnkRen;
                    inserttkdUnkobiFile.FileId = item.FileId;
                    inserttkdUnkobiFile.FolderId = item.FolderId;
                    inserttkdUnkobiFile.SiyoKbn = 1;
                    inserttkdUnkobiFile.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    inserttkdUnkobiFile.UpdTime = DateTime.Now.ToString("hhmmss");
                    inserttkdUnkobiFile.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    inserttkdUnkobiFile.UpdPrgId = Common.UpdPrgId;
                    _dbContext.TkdUnkobiFile.Add(inserttkdUnkobiFile);
                    _dbContext.SaveChanges();
                }   
            }    
        }
        public async Task<List<TkdUnkobi>> Get()
        {
            return await _dbContext.TkdUnkobi.ToListAsync();
        }
        public MinMaxDate GetMinMaxDate(string id)
        {
            return (from s in _dbContext.TkdUnkobi
                    where s.UkeNo == id
                    select new MinMaxDate()
                    {
                        Unkobi_HaiSYmd = s.HaiSymd,
                        Unkobi_HaiSTime = s.HaiStime,
                        Unkobi_TouYmd = s.TouYmd,
                        Unkobi_TouChTime = s.TouChTime
                    }).FirstOrDefault();
        }
        public async Task<TkdUnkobi> Get(string id)
        {
            var toDo = await _dbContext.TkdUnkobi.FindAsync(id);
            return toDo;
        }
        public void Add(IDictionary<string, object> data)
        {
            var tkdunkobi = new TkdUnkobi();
            tkdunkobi.UkeNo = data["UkeNo"].ToString();
            tkdunkobi.UnkRen = 2;
            tkdunkobi.HenKai = 3;
            tkdunkobi.HaiSymd = (string)data["HaiSymd"];
            tkdunkobi.HaiStime = (string)data["HaiSTime"];
            tkdunkobi.TouYmd = (string)data["TouYmd"];
            tkdunkobi.TouChTime = (string)data["TouChTime"];
            tkdunkobi.SyuPaTime = "0700";
            tkdunkobi.UriYmd = "20141216";
            tkdunkobi.KanJnm = "幹事Name";
            tkdunkobi.KanjJyus1 = "幹事住所";
            tkdunkobi.KanjJyus2 = "住所詳細";
            tkdunkobi.KanjTel = "              ";
            tkdunkobi.KanjFax = "              ";
            tkdunkobi.KanjKeiNo = "              ";
            tkdunkobi.KanjMail = "kanji@email.com";
            tkdunkobi.KanDmhflg = 0;
            tkdunkobi.DanTaNm = "dantai";
            tkdunkobi.DanTaKana = "ｄａｎｔａｉ ｋａｎａ";
            tkdunkobi.IkMapCdSeq = 20;
            tkdunkobi.IkNm = "小金井";
            tkdunkobi.HaiScdSeq = 12;
            tkdunkobi.HaiSnm = "富山空港";
            tkdunkobi.HaiSjyus1 = "";
            tkdunkobi.HaiSjyus2 = "";
            tkdunkobi.HaiSkouKcdSeq = 0;
            tkdunkobi.HaiSbinCdSeq = 0;
            tkdunkobi.HaiSsetTime = "    ";
            tkdunkobi.TouCdSeq = 0;
            tkdunkobi.TouNm = "";
            tkdunkobi.TouJyusyo1 = "";
            tkdunkobi.TouJyusyo2 = "";
            tkdunkobi.TouKouKcdSeq = 0;
            tkdunkobi.TouBinCdSeq = 0;
            tkdunkobi.TouSetTime = "    ";
            tkdunkobi.AreaMapSeq = 0;
            tkdunkobi.AreaNm = "";
            tkdunkobi.HasMapCdSeq = 0;
            tkdunkobi.HasNm = "";
            tkdunkobi.JyoKyakuCdSeq = 0;
            tkdunkobi.JyoSyaJin = 15;
            tkdunkobi.PlusJin = 1;
            tkdunkobi.DrvJin = 2;
            tkdunkobi.GuiSu = 5;
            tkdunkobi.OthJinKbn1 = 2;
            tkdunkobi.OthJin1 = 3;
            tkdunkobi.OthJinKbn2 = 4;
            tkdunkobi.OthJin2 = 4;
            tkdunkobi.Kskbn = 2;
            tkdunkobi.KhinKbn = 2;
            tkdunkobi.HaiSkbn = 1;
            tkdunkobi.HaiIkbn = 1;
            tkdunkobi.GuiWnin = 0;
            tkdunkobi.NippoKbn = 1;
            tkdunkobi.YouKbn = 1;
            tkdunkobi.UkeJyKbnCd = 99;
            tkdunkobi.SijJoKbn1 = 99;
            tkdunkobi.SijJoKbn2 = 99;
            tkdunkobi.SijJoKbn3 = 99;
            tkdunkobi.SijJoKbn4 = 1;
            tkdunkobi.SijJoKbn5 = 99;
            tkdunkobi.RotCdSeq = 0;
            tkdunkobi.ZenHaFlg = (byte)(int)data["ZenHaFlg"];
            tkdunkobi.KhakFlg = (byte)(int)data["KhakFlg"];
            tkdunkobi.UnkoJkbn = 5;
            tkdunkobi.SyuKoTime = "0600";
            tkdunkobi.KikTime = "1900";
            tkdunkobi.BikoTblSeq = 2;
            tkdunkobi.SiyoKbn = 1;
            tkdunkobi.UpdYmd = "20200107";
            tkdunkobi.UpdTime = "125912";
            tkdunkobi.UpdSyainCd = 1;
            tkdunkobi.UpdPrgId = "CNV       ";
            _dbContext.TkdUnkobi.Add(tkdunkobi);
            _dbContext.SaveChanges();
        }
        public async Task<HttpResponseMessage> InsertTKD_UnkobiAsync(TkdUnkobi tkdUnkobi)
        {
            string baseUrl = _config.GetBaseUrl();
            var client = new HttpClient();
            return await client.PostAsync($"{baseUrl}/api/TkdUnkobis", _httpClient.getStringContentFromObject(tkdUnkobi));
        }
    }
}
