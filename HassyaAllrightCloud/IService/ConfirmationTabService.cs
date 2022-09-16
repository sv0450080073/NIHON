using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Domain.Dto;
using Microsoft.Extensions.Logging;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;

namespace HassyaAllrightCloud.IService
{
    public interface IConfirmationTabService
    {
        Task<IEnumerable<ConfirmationTabData>> GetConfirmationDataById(string ukeNo);
        Task<bool> Insert(IEnumerable<ConfirmationTabData> confirmationTabDatas, string ukeNo);
        Task<string> UpdateConfirmStatus(ConfirmStatus confirmStatus, string ukeNo, string date);
    }

    public class ConfirmationTabService : IConfirmationTabService
    {
        private readonly KobodbContext _dbContext;
        private readonly ILogger<ConfirmationTabService> _logger;

        public ConfirmationTabService(KobodbContext dbContext, ILogger<ConfirmationTabService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<ConfirmationTabData>> GetConfirmationDataById(string ukeNo)
        {
            var result = new List<ConfirmationTabData>();
            try
            {
                // check paid or coupon
                var yyksho = await _dbContext.TkdYyksho.Where(x => x.UkeNo == ukeNo).FirstOrDefaultAsync();
                // check lock table
                var lockTable = _dbContext.TkdLockTable.SingleOrDefault(l => l.TenantCdSeq == new ClaimModel().TenantID
                                                                            && l.EigyoCdSeq == yyksho.SeiEigCdSeq);
                var checkEdit = BookingInputHelper.CheckEditable(yyksho, lockTable);
                if (checkEdit.Contains(BookingDisableEditState.PaidOrCoupon))
                {
                    throw new Exception("Booking has been paid or coupon");
                }
                if (checkEdit.Contains(BookingDisableEditState.Locked))
                {
                    throw new Exception("Booking has been lock");
                }

                result = await (from tkdKaknin in _dbContext.TkdKaknin
                                join tkdYyksho in _dbContext.TkdYyksho
                                on tkdKaknin.UkeNo equals tkdYyksho.UkeNo
                                where tkdKaknin.UkeNo == ukeNo
                                select new ConfirmationTabData()
                                {
                                    FixDataNo = tkdKaknin.KaknRen,
                                    KaknYmd = DateTime.ParseExact(tkdKaknin.KaknYmd, "yyyyMMdd", null),
                                    KaknAit = tkdKaknin.KaknAit,
                                    KaknNin = tkdKaknin.KaknNin.ToString(),
                                    SaikFlg = Convert.ToBoolean(tkdKaknin.SaikFlg),
                                    DaiSuFlg = Convert.ToBoolean(tkdKaknin.DaiSuFlg),
                                    KingFlg = Convert.ToBoolean(tkdKaknin.KingFlg),
                                    NitteFlag = Convert.ToBoolean(tkdKaknin.NitteiFlg),
                                }).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            return result;
        }

        /// <summary>
        /// This method use for both insert and update action, if TkdKaknin have data of ukeNo key then Update, otherwise Insert
        /// </summary>
        /// <param name="confirmationTabDatas"></param>
        /// <param name="ukeNo"></param>
        /// <returns></returns>
        public async Task<bool> Insert(IEnumerable<ConfirmationTabData> confirmationTabDatas, string ukeNo)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    short index = 0;
                    bool isInsert = false;
                    if (_dbContext.TkdKaknin.Where(t => t.UkeNo == ukeNo).Count() == 0) isInsert = true;

                    if (!isInsert) _dbContext.TkdKaknin.RemoveRange(_dbContext.TkdKaknin.Where(t => t.UkeNo == ukeNo));
                    foreach (var confirmation in confirmationTabDatas)
                    {
                        var tkdKaknin = new TkdKaknin()
                        {
                            UkeNo = ukeNo,
                            KaknYmd = confirmation.KaknYmd.ToString("yyyyMMdd"),
                            KaknAit = confirmation.KaknAit,
                            KaknNin = short.Parse(confirmation.KaknNin),
                            SaikFlg = Convert.ToByte(confirmation.SaikFlg),
                            DaiSuFlg = Convert.ToByte(confirmation.DaiSuFlg),
                            KingFlg = Convert.ToByte(confirmation.KingFlg),
                            NitteiFlg = Convert.ToByte(confirmation.NitteFlag),
                            BikoTblSeq = 0,
                            HenKai = 0,
                            SiyoKbn = 1,
                            UpdPrgId = Common.UpdPrgId,
                            UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                            UpdTime = DateTime.Now.ToString("HHmmss"),
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd")
                        };
                        if (isInsert)
                        {
                            tkdKaknin.KaknRen = ++index;
                        }
                        else
                        {
                            tkdKaknin.KaknRen = (short)confirmation.FixDataNo;
                        }
                        await _dbContext.TkdKaknin.AddAsync(tkdKaknin);
                    }
                    await _dbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await trans.RollbackAsync();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// this method use to change status confirmation
        /// </summary>
        /// <param name="confirmStatus"></param>
        /// <param name="ukeNo"></param>
        /// <returns></returns>
        public async Task<string> UpdateConfirmStatus(ConfirmStatus confirmStatus, string ukeNo, string date)
        {
            using var trans = _dbContext.Database.BeginTransaction();
            try
            {
                //var yykSho = await _dbContext.TkdYyksho.FindAsync(ukeNo);
                var yykSho = await _dbContext.TkdYyksho.Where(x => x.UkeNo == ukeNo).SingleOrDefaultAsync();
                if (confirmStatus == ConfirmStatus.Fixed)
                {
                    yykSho.KaktYmd = date;
                }
                else
                {
                    yykSho.KaktYmd = string.Empty;
                }
                yykSho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                yykSho.UpdTime = DateTime.Now.ToString("HHmmss");
                await _dbContext.SaveChangesAsync();
                await trans.CommitAsync();
                return yykSho.UpdYmd + yykSho.UpdTime;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine(ex.Message);
                await trans.RollbackAsync();
                return "";
            }
        }
    }
}
