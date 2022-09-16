using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Net.Http;
using HassyaAllrightCloud.Infrastructure.Services;
using System.Linq;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_YykSyuListService
    {
        Task<List<TkdYykSyu>> Get();
        Task<TkdYykSyu> Get(int id);
        void Add(IDictionary<string, object> tkd_yykSyu);
        byte GetKatakbn(string UkeNo, int UnkRen, int SyaSyuRen);
        Task<HttpResponseMessage> InsertTkdYouSyuAsync(TkdYykSyu tkdYykSyu);
    }
    public class TKD_YykSyuService : ITKD_YykSyuListService
    {
        private readonly KobodbContext _dbContext;
        private readonly AppSettingsService _config;
        private readonly CustomHttpClient _httpClient;
        public TKD_YykSyuService(KobodbContext context, AppSettingsService config, CustomHttpClient httpClient)
        {
            _dbContext = context;
            _config = config;
            _httpClient = httpClient;
        }
        public async Task<List<TkdYykSyu>> Get()
        {
            return await _dbContext.TkdYykSyu.ToListAsync();
        }
        public byte GetKatakbn(string UkeNo, int UnkRen, int SyaSyuRen)
        {
            return (from s in _dbContext.TkdYykSyu where s.UkeNo == UkeNo && s.UnkRen == UnkRen && s.SyaSyuRen == SyaSyuRen select s.KataKbn).First();

        }
        public async Task<TkdYykSyu> Get(int id)
        {
            var toDo = await _dbContext.TkdYykSyu.FindAsync(id);
            return toDo;
        }
        public void Add(IDictionary<string, object> data)
        {
            var yyksyu = new TkdYykSyu();
            yyksyu.UkeNo = data["UkeNo"].ToString();
            yyksyu.UnkRen = 1;
            yyksyu.SyaSyuRen = Convert.ToInt16(data["id"]); ;
            yyksyu.HenKai = 21;
            yyksyu.SyaSyuCdSeq = (byte)(int)data["SyaSyuCdSeq"];
            yyksyu.KataKbn = (byte)(int)data["KataKbn"];
            yyksyu.SyaSyuDai = Convert.ToInt16(data["SyaSyuDai"]);
            yyksyu.SyaSyuTan = Convert.ToInt32(data["SyaSyuTan"]);
            yyksyu.SyaRyoUnc = Convert.ToInt32(data["SyaRyoUnc"]);
            yyksyu.DriverNum = (byte)(int)data["DriverNum"];
            yyksyu.UnitBusPrice = Convert.ToInt32(data["UnitBusPrice"]);
            yyksyu.UnitBusFee = Convert.ToInt32(data["UnitBusFee"]);
            yyksyu.GuiderNum = (byte)(int)data["GuiderNum"];
            yyksyu.UnitGuiderPrice = Convert.ToInt32(data["UnitGuiderPrice"]);
            yyksyu.UnitGuiderFee = Convert.ToInt32(data["UnitGuiderFee"]);
            yyksyu.SiyoKbn = 1;
            yyksyu.UpdYmd = "20200107";
            yyksyu.UpdTime = "160343";
            yyksyu.UpdSyainCd = 1;
            yyksyu.UpdPrgId = "KJ1000P   ";
            _dbContext.TkdYykSyu.Add(yyksyu);
            _dbContext.SaveChanges();
        }
        public async Task<HttpResponseMessage> InsertTkdYouSyuAsync(TkdYykSyu tkdYykSyu)
        {
            string baseUrl = _config.GetBaseUrl();
            var client = new HttpClient();
            return await client.PostAsync($"{baseUrl}/api/TkdYykSyus", _httpClient.getStringContentFromObject(tkdYykSyu));
        }
    }
}
