using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillCheckList.Queries
{
    public class GetDataForBillCheckListToTalAllQuery : IRequest<List<BillCheckListTotalData>>
    {
        public BillsCheckListFormData billCheckListData { get; set; }
        public int companyId { get; set; }
        public int tenantId { get; set; }
        public class Handler : IRequestHandler<GetDataForBillCheckListToTalAllQuery, List<BillCheckListTotalData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }
            public void CreateParameter(DbCommand command, string procedureName, BillsCheckListFormData billFormData, int CompanyId, int TenantId)
            {
                command.CommandText = "EXECUTE " + procedureName + " " +
                        " @TenantCdSeq," +
                        " @CompanyCdSeq," +
                        " @StartBillPeriod," +
                        " @EndBillPeriod," +
                        " @BillOffice," +
                        " @StartBillAddress," +
                        " @EndBillAddress," +
                        " @StartReceiptNumber," +
                        " @EndReceiptNumber," +
                        " @StartReservationClassification," +
                        " @EndReservationClassification," +
                        " @StartBillClassification," +
                        " @EndBillClassification," +
                        " @BillTypes," +
                        " @BillAddress," +
                        " @BillIssuedClassification";
                List<DbParameter> Parameters = new List<DbParameter>();

                // ログインユーザのテナントID
                DbParameter parameter0 = command.CreateParameter();
                parameter0.ParameterName = "@TenantCdSeq";
                parameter0.Value = TenantId;
                Parameters.Add(parameter0);

                // ログインユーザの会社のCompanyCdSeq
                DbParameter parameter1 = command.CreateParameter();
                parameter1.ParameterName = "@CompanyCdSeq";
                parameter1.Value = CompanyId;
                Parameters.Add(parameter1);

                // 請求対象期間
                DbParameter parameter2 = command.CreateParameter();
                parameter2.ParameterName = "@StartBillPeriod";
                if (billFormData.BillPeriodFrom == null)
                {
                    parameter2.Value = DBNull.Value;
                }
                else
                {
                    parameter2.Value = ((DateTime)billFormData.BillPeriodFrom).ToString("yyyyMMdd");
                }
                DbParameter parameter3 = command.CreateParameter();
                parameter3.ParameterName = "@EndBillPeriod";
                if (billFormData.BillPeriodTo == null)
                {
                    parameter3.Value = DBNull.Value;
                }
                else
                {
                    parameter3.Value = ((DateTime)billFormData.BillPeriodTo).ToString("yyyyMMdd");
                }
                Parameters.Add(parameter2);
                Parameters.Add(parameter3);

                // 請求営業所
                DbParameter parameter4 = command.CreateParameter();
                parameter4.ParameterName = "@BillOffice";
                if (billFormData.BillOffice == null)
                {
                    parameter4.Value = DBNull.Value;
                }
                else
                {
                    parameter4.Value = billFormData.BillOffice.EigyoCdSeq;
                }
                Parameters.Add(parameter4);

                // 請求先
                DbParameter parameter5 = command.CreateParameter();
                parameter5.ParameterName = "@StartBillAddress";
                if (billFormData.StartBillAddress == null)
                {
                    parameter5.Value = DBNull.Value;
                }
                else
                {
                    parameter5.Value = billFormData.StartBillAddress.GyoSyaCd.ToString("D3") + billFormData.StartBillAddress.TokuiCd.ToString("D4") + billFormData.StartBillAddress.SitenCd.ToString("D4");
                }
                DbParameter parameter6 = command.CreateParameter();
                parameter6.ParameterName = "@EndBillAddress";
                if (billFormData.EndBillAddress == null)
                {
                    parameter6.Value = DBNull.Value;
                }
                else
                {
                    parameter6.Value = billFormData.EndBillAddress.GyoSyaCd.ToString("D3") + billFormData.EndBillAddress.TokuiCd.ToString("D4") + billFormData.EndBillAddress.SitenCd.ToString("D4");
                }
                Parameters.Add(parameter5);
                Parameters.Add(parameter6);

                // 予約番号
                DbParameter parameter7 = command.CreateParameter();
                parameter7.ParameterName = "@StartReceiptNumber";
                if (string.IsNullOrEmpty(billFormData.StartReceiptNumber))
                {
                    parameter7.Value = DBNull.Value;
                }
                else
                {
                    parameter7.Value = TenantId.ToString("D5") + CommonUtil.FormatCodeNumber(billFormData.StartReceiptNumber);
                }
                DbParameter parameter8 = command.CreateParameter();
                parameter8.ParameterName = "@EndReceiptNumber";
                if (string.IsNullOrEmpty(billFormData.EndReceiptNumber))
                {
                    parameter8.Value = DBNull.Value;
                }
                else
                {
                    parameter8.Value = TenantId.ToString("D5") + CommonUtil.FormatCodeNumber(billFormData.EndReceiptNumber);
                }
                Parameters.Add(parameter7);
                Parameters.Add(parameter8);

                // 予約区分
                DbParameter parameter9 = command.CreateParameter();
                parameter9.ParameterName = "@StartReservationClassification";
                if (billFormData.StartReservationClassification == null)
                {
                    parameter9.Value = DBNull.Value;
                }
                else
                {
                    parameter9.Value = billFormData.StartReservationClassification.YoyaKbn;
                }
                DbParameter parameter10 = command.CreateParameter();
                parameter10.ParameterName = "@EndReservationClassification";
                if (billFormData.EndReservationClassification == null)
                {
                    parameter10.Value = DBNull.Value;
                }
                else
                {
                    parameter10.Value = billFormData.EndReservationClassification.YoyaKbn;
                }
                Parameters.Add(parameter9);
                Parameters.Add(parameter10);

                // 請求区分
                DbParameter parameter11 = command.CreateParameter();
                parameter11.ParameterName = "@StartBillClassification";
                if (billFormData.StartBillClassification == null)
                {
                    parameter11.Value = DBNull.Value;
                }
                else
                {
                    parameter11.Value = billFormData.StartBillClassification.CodeKbn;
                }
                DbParameter parameter12 = command.CreateParameter();
                parameter12.ParameterName = "@EndBillClassification";
                if (billFormData.EndBillClassification == null)
                {
                    parameter12.Value = DBNull.Value;
                }
                else
                {
                    parameter12.Value = billFormData.EndBillClassification.CodeKbn;
                }
                Parameters.Add(parameter11);
                Parameters.Add(parameter12);

                // 請求発行済区分
                DbParameter parameter13 = command.CreateParameter();
                parameter13.ParameterName = "@BillTypes";
                string lstBillType = "";
                if (billFormData.itemFare)
                {
                    lstBillType += "1,";
                }
                if (billFormData.itemIncidental)
                {
                    lstBillType += "2,";
                }
                if (billFormData.itemTollFee)
                {
                    lstBillType += "3,";
                }
                if (billFormData.itemArrangementFee)
                {
                    lstBillType += "4,";
                }
                if (billFormData.itemGuideFee)
                {
                    lstBillType += "5,";
                }
                if (billFormData.itemShippingCharge)
                {
                    lstBillType += "6,";
                }
                if (billFormData.itemCancellationCharge)
                {
                    lstBillType += "7,";
                }
                if ("".Equals(lstBillType))
                {
                    parameter13.Value = DBNull.Value;
                }
                else
                {
                    parameter13.Value = lstBillType.Substring(0, lstBillType.Length - 1);
                }
                Parameters.Add(parameter13);

                // 請求先
                DbParameter parameter14 = command.CreateParameter();
                parameter14.ParameterName = "@BillAddress";
                if (billFormData.BillAdress == null)
                {
                    parameter14.Value = DBNull.Value;
                }
                else
                {
                    parameter14.Value = billFormData.BillAdress.GyoSyaCd.ToString("D3") + billFormData.BillAdress.TokuiCd.ToString("D4") + billFormData.BillAdress.SitenCd.ToString("D4");
                }

                Parameters.Add(parameter14);

                // 
                DbParameter parameter15 = command.CreateParameter();
                parameter15.ParameterName = "@BillIssuedClassification";
                if (billFormData.BillIssuedClassification == null || billFormData.BillIssuedClassification.IdValue == 0)
                {
                    parameter15.Value = DBNull.Value;
                }
                else
                {
                    parameter15.Value = billFormData.BillIssuedClassification.IdValue;
                }
                Parameters.Add(parameter15);
                foreach (var parameter in Parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            public async Task<List<BillCheckListTotalData>> Handle(GetDataForBillCheckListToTalAllQuery request, CancellationToken cancellationToken)
            {
                int outputRowCoutVehicle = 0;
                List<BillCheckListTotalData> list = new List<BillCheckListTotalData>();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    CreateParameter(command, "PK_dBillCheckListSum_R", request.billCheckListData, request.companyId, request.tenantId);
                    command.CommandText += ","
                        + " @ROWCOUNT";
                    DbParameter rowCount = command.CreateParameter();
                    rowCount.ParameterName = "@ROWCOUNT";
                    rowCount.Value = outputRowCoutVehicle;

                    command.Parameters.Add(rowCount);
                    _dbContext.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new BillCheckListTotalData
                            {
                                Type = 3,
                                SeiFutSyu = (byte)reader["SeiFutSyu"],
                                BillAmountTotal = (long)reader["SeiKin"],
                                DepositAmountTotal = (decimal)reader["NyuKinRui"],
                                UnpaidAmountTotal = (decimal)reader["MisyuG"],
                                SalesAmountTotal = (long)reader["UriGakKin"],
                                TaxAmountTotal = (long)reader["SyaRyoSyo"],
                                CommissionAmount = (long)reader["SyaRyoTes"]

                            });
                        }
                        reader.Close();
                    }
                    _dbContext.Database.CloseConnection();
                }
                if(list.Count > 0)
                {
                    list.Add(new BillCheckListTotalData()
                    {
                        Type = 3,
                        SeiFutSyu = 0,
                        BillAmountTotal = list.Sum(x => x.BillAmountTotal),
                        DepositAmountTotal = list.Sum(x => x.DepositAmountTotal),
                        UnpaidAmountTotal = list.Sum(x => x.UnpaidAmountTotal),
                        SalesAmountTotal = list.Sum(x => x.SalesAmountTotal),
                        TaxAmountTotal = list.Sum(x => x.TaxAmountTotal),
                        CommissionAmount = list.Sum(x => x.CommissionAmount),
                    });
                }
                return await Task.FromResult(list);
            }
        }
    }
}
