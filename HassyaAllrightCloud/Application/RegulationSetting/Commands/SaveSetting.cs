using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto.RegulationSetting;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Threading;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.RegulationSetting.Commands
{
    public class SaveSetting : IRequest<bool>
    {
        public RegulationSettingFormModel Model { get; set; }
        public class Handler : IRequestHandler<SaveSetting, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(SaveSetting request, CancellationToken cancellationToken)
            {
                try
                {
                    TkmKasSet tkmKasSet = _context.TkmKasSet.Where(x => x.CompanyCdSeq == request.Model.Company.CompanyCdSeq).FirstOrDefault();
                    if (tkmKasSet != null)
                    {
                        tkmKasSet.UriKbn = (byte)request.Model.EditFormSaleClassification;
                        tkmKasSet.SyohiHasu = (byte)request.Model.EditFormTaxFraction;
                        tkmKasSet.TesuHasu = (byte)request.Model.EditFormFeeFraction;
                        tkmKasSet.HoukoKbn = (byte)request.Model.EditFormReportClassification;
                        tkmKasSet.HouZeiKbn = (byte)request.Model.EditFormReportSummary;
                        tkmKasSet.HouOutKbn = (byte)request.Model.EditFormReportOutput;
                        tkmKasSet.JkariKbn = (byte)request.Model.EditFormAutoTemporaryBus;
                        tkmKasSet.AutKarJyun = (byte)request.Model.EditFormPriority;
                        tkmKasSet.JkbunPat = (byte)request.Model.EditFormAutoTemporaryBusDivision;
                        tkmKasSet.SyaIrePat = (byte)request.Model.EditFormVehicleReplacement;
                        tkmKasSet.JymAchkKbn = (byte)request.Model.EditFormCrewCompatibilityCheck;
                        tkmKasSet.UriHenKbn = request.Model.EditFormSaleChange ? (byte)2 : (byte)1;
                        tkmKasSet.UriMdkbn = (byte)request.Model.EditFormSalesChangeDateClassification;
                        tkmKasSet.UriHenKikan = !string.IsNullOrWhiteSpace(request.Model.EditFormSalesChangeablePeriod) ? (byte)Int32.Parse(request.Model.EditFormSalesChangeablePeriod) : (byte)0;
                        tkmKasSet.UriZeroChk = (byte)request.Model.EditFormCheckZeroYen;
                        tkmKasSet.CanKbn = request.Model.EditFormCancelClassification ? (byte)2 : (byte)1;
                        tkmKasSet.CanMdkbn = (byte)request.Model.EditFormCancelDateClassification;
                        tkmKasSet.CanKikan = !string.IsNullOrWhiteSpace(request.Model.EditFormCancellationPeriod) ? (byte)Int32.Parse(request.Model.EditFormCancellationPeriod) : (byte)0;
                        tkmKasSet.YouTesuKbn = (byte)request.Model.EditFormHiredBusFee;
                        tkmKasSet.YouSagaKbn = (byte)request.Model.EditFormHiredBusDifferentClassification;
                        tkmKasSet.SyaUntKbn = (byte)request.Model.EditFormFareByVehicle;
                        tkmKasSet.ZasyuKbn = (byte)request.Model.EditFormTransportationMiscellaneousIncome;
                        tkmKasSet.FutSf1flg = (byte)request.Model.EditFormIncidentalType1Addition;
                        tkmKasSet.FutSf2flg = (byte)request.Model.EditFormIncidentalType2Addition;
                        tkmKasSet.FutSf3flg = (byte)request.Model.EditFormIncidentalType3Addition;
                        tkmKasSet.FutSf4flg = (byte)request.Model.EditFormIncidentalType4Addition;
                        tkmKasSet.UntZeiKbn = (byte)request.Model.EditFormFareTaxDisplay;
                        tkmKasSet.TumZeiKbn = (byte)request.Model.EditFormLoadingGoodsTaxDisplay;
                        tkmKasSet.CanRit1 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate1) ? decimal.Parse(request.Model.EditFormCancelRate1) : (decimal)0.0;
                        tkmKasSet.CanSkan1 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate1StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate1StartTime) : (byte)0;
                        tkmKasSet.CanEkan1 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate1EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate1EndTime) : (byte)0;
                        tkmKasSet.CanRit2 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate2) ? decimal.Parse(request.Model.EditFormCancelRate2) : (decimal)0.0;
                        tkmKasSet.CanSkan2 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate2StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate2StartTime) : (byte)0;
                        tkmKasSet.CanEkan2 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate2EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate2EndTime) : (byte)0;
                        tkmKasSet.CanRit3 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate3) ? decimal.Parse(request.Model.EditFormCancelRate3) : (decimal)0.0;
                        tkmKasSet.CanSkan3 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate3StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate3StartTime) : (byte)0;
                        tkmKasSet.CanEkan3 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate3EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate3EndTime) : (byte)0;
                        tkmKasSet.CanRit4 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate4) ? decimal.Parse(request.Model.EditFormCancelRate4) : (decimal)0.0;
                        tkmKasSet.CanSkan4 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate4StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate4StartTime) : (byte)0;
                        tkmKasSet.CanEkan4 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate4EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate4EndTime) : (byte)0;
                        tkmKasSet.CanRit5 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate5) ? decimal.Parse(request.Model.EditFormCancelRate5) : (decimal)0.0;
                        tkmKasSet.CanSkan5 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate5StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate5StartTime) : (byte)0;
                        tkmKasSet.CanEkan5 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate5EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate5EndTime) : (byte)0;
                        tkmKasSet.CanRit6 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate6) ? decimal.Parse(request.Model.EditFormCancelRate6) : (decimal)0.0;
                        tkmKasSet.CanSkan6 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate6StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate6StartTime) : (byte)0;
                        tkmKasSet.CanEkan6 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate6EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate6EndTime) : (byte)0;
                        tkmKasSet.GetSyoKbn = (byte)request.Model.EditFormMonthlyProcess;
                        tkmKasSet.SeiKrksKbn = (byte)request.Model.EditFormBillForward;
                        tkmKasSet.DaySyoKbn = (byte)request.Model.EditFormDailyProcess;
                        tkmKasSet.AutKouKbn = (byte)request.Model.EditFormAutoKoban;
                        tkmKasSet.KoteiCopyFlg = (byte)request.Model.EditFormInitCopyProcessData;
                        tkmKasSet.FutaiCopyFlg = (byte)request.Model.EditFormInitCopyIncidentalData;
                        tkmKasSet.TumiCopyFlg = (byte)request.Model.EditFormInitCopyLoadingGoodData;
                        tkmKasSet.TehaiCopyFlg = (byte)request.Model.EditFormInitCopyArrangeData;
                        tkmKasSet.JoshaCopyFlg = (byte)request.Model.EditFormInitCopyBoardingPlaceData;
                        tkmKasSet.YykCopyFlg = (byte)request.Model.EditFormInitCopyReservationRemarkData;
                        tkmKasSet.UkbCopyFlg = (byte)request.Model.EditFormInitCopyOperationDateRemarkData;
                        tkmKasSet.SeiGenFlg = (byte)request.Model.EditFormCurrentInvoice;
                        tkmKasSet.MeiShyKbn = (byte)request.Model.EditFormDisplayDetailSelection;
                        tkmKasSet.SyaSenMjPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacter1DisplayByBusType != null ? request.Model.EditFormCharacter1DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenMjPtnCol1 = request.Model.EditFormColor1DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenMjPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacter2DisplayByBusType != null ? request.Model.EditFormCharacter2DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenMjPtnCol2 = request.Model.EditFormColor2DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenMjPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacter3DisplayByBusType != null ? request.Model.EditFormCharacter3DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenMjPtnCol3 = request.Model.EditFormColor3DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenMjPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacter4DisplayByBusType != null ? request.Model.EditFormCharacter4DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenMjPtnCol4 = request.Model.EditFormColor4DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenMjPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacter5DisplayByBusType != null ? request.Model.EditFormCharacter5DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenMjPtnCol5 = request.Model.EditFormColor5DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenMjPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacter1DisplayByCrew != null ? request.Model.EditFormCharacter1DisplayByCrew.CodeKbn : "0");
                        tkmKasSet.JyoSenMjPtnCol1 = request.Model.EditFormColor1DisplayByCrew.Replace("#", string.Empty);
                        tkmKasSet.JyoSenMjPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacter2DisplayByCrew != null ? request.Model.EditFormCharacter2DisplayByCrew.CodeKbn : "0");
                        tkmKasSet.JyoSenMjPtnCol2 = request.Model.EditFormColor2DisplayByCrew.Replace("#", string.Empty);
                        tkmKasSet.JyoSenMjPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacter3DisplayByCrew != null ? request.Model.EditFormCharacter3DisplayByCrew.CodeKbn : "0");
                        tkmKasSet.JyoSenMjPtnCol3 = request.Model.EditFormColor3DisplayByCrew.Replace("#", string.Empty);
                        tkmKasSet.JyoSenMjPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacter4DisplayByCrew != null ? request.Model.EditFormCharacter4DisplayByCrew.CodeKbn : "0");
                        tkmKasSet.JyoSenMjPtnCol4 = request.Model.EditFormColor4DisplayByCrew.Replace("#", string.Empty);
                        tkmKasSet.JyoSenMjPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacter5DisplayByCrew != null ? request.Model.EditFormCharacter5DisplayByCrew.CodeKbn : "0");
                        tkmKasSet.JyoSenMjPtnCol5 = request.Model.EditFormColor5DisplayByCrew.Replace("#", string.Empty);
                        tkmKasSet.SeiCom1 = request.Model.EditFormBillComent1;
                        tkmKasSet.SeiCom2 = request.Model.EditFormBillComent2;
                        tkmKasSet.SeiCom3 = request.Model.EditFormBillComent3;
                        tkmKasSet.SeiCom4 = request.Model.EditFormBillComent4;
                        tkmKasSet.SeiCom5 = request.Model.EditFormBillComent5;
                        tkmKasSet.SeiCom6 = request.Model.EditFormBillComent6;
                        tkmKasSet.JisKinKyuNm01 = request.Model.JisKinKyuNm01 == null ? "" : request.Model.JisKinKyuNm01;
                        tkmKasSet.JisKinKyuNm02 = request.Model.JisKinKyuNm02 == null ? "" : request.Model.JisKinKyuNm02;
                        tkmKasSet.JisKinKyuNm03 = request.Model.JisKinKyuNm03 == null ? "" : request.Model.JisKinKyuNm03;
                        tkmKasSet.JisKinKyuNm04 = request.Model.JisKinKyuNm04 == null ? "" : request.Model.JisKinKyuNm04;
                        tkmKasSet.JisKinKyuNm05 = request.Model.JisKinKyuNm05 == null ? "" : request.Model.JisKinKyuNm05;
                        tkmKasSet.JisKinKyuNm06 = request.Model.JisKinKyuNm06 == null ? "" : request.Model.JisKinKyuNm06;
                        tkmKasSet.JisKinKyuNm07 = request.Model.JisKinKyuNm07 == null ? "" : request.Model.JisKinKyuNm07;
                        tkmKasSet.JisKinKyuNm08 = request.Model.JisKinKyuNm08 == null ? "" : request.Model.JisKinKyuNm08;
                        tkmKasSet.JisKinKyuNm09 = request.Model.JisKinKyuNm09 == null ? "" : request.Model.JisKinKyuNm09;
                        tkmKasSet.JisKinKyuNm10 = request.Model.JisKinKyuNm10 == null ? "" : request.Model.JisKinKyuNm10;
                        tkmKasSet.SyaSenInfoPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification1DisplayByBusType != null ? request.Model.EditFormCharacterClassification1DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol1 = request.Model.EditFormColorClassification1DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification2DisplayByBusType != null ? request.Model.EditFormCharacterClassification2DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol2 = request.Model.EditFormColorClassification2DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification3DisplayByBusType != null ? request.Model.EditFormCharacterClassification3DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol3 = request.Model.EditFormColorClassification3DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification4DisplayByBusType != null ? request.Model.EditFormCharacterClassification4DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol4 = request.Model.EditFormColorClassification4DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification5DisplayByBusType != null ? request.Model.EditFormCharacterClassification5DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol5 = request.Model.EditFormColorClassification5DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn6 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification6DisplayByBusType != null ? request.Model.EditFormCharacterClassification6DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol6 = request.Model.EditFormColorClassification6DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn7 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification7DisplayByBusType != null ? request.Model.EditFormCharacterClassification7DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol7 = request.Model.EditFormColorClassification7DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn8 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification8DisplayByBusType != null ? request.Model.EditFormCharacterClassification8DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol8 = request.Model.EditFormColorClassification8DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn9 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification9DisplayByBusType != null ? request.Model.EditFormCharacterClassification9DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol9 = request.Model.EditFormColorClassification9DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn10 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification10DisplayByBusType != null ? request.Model.EditFormCharacterClassification10DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol10 = request.Model.EditFormColorClassification10DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn11 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification11DisplayByBusType != null ? request.Model.EditFormCharacterClassification11DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol11 = request.Model.EditFormColorClassification11DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn12 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification12DisplayByBusType != null ? request.Model.EditFormCharacterClassification12DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol12 = request.Model.EditFormColorClassification12DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn13 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification13DisplayByBusType != null ? request.Model.EditFormCharacterClassification13DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol13 = request.Model.EditFormColorClassification13DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn14 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification14DisplayByBusType != null ? request.Model.EditFormCharacterClassification14DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol14 = request.Model.EditFormColorClassification14DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.SyaSenInfoPtnKbn15 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification15DisplayByBusType != null ? request.Model.EditFormCharacterClassification15DisplayByBusType.CodeKbn : "0");
                        tkmKasSet.SyaSenInfoPtnCol15 = request.Model.EditFormColorClassification15DisplayByBusType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification1DisplayByCrewType != null ? request.Model.EditFormCharacterClassification1DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol1 = request.Model.EditFormColorClassification1DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification2DisplayByCrewType != null ? request.Model.EditFormCharacterClassification2DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol2 = request.Model.EditFormColorClassification2DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification3DisplayByCrewType != null ? request.Model.EditFormCharacterClassification3DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol3 = request.Model.EditFormColorClassification3DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification4DisplayByCrewType != null ? request.Model.EditFormCharacterClassification4DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol4 = request.Model.EditFormColorClassification4DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification5DisplayByCrewType != null ? request.Model.EditFormCharacterClassification5DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol5 = request.Model.EditFormColorClassification5DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn6 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification6DisplayByCrewType != null ? request.Model.EditFormCharacterClassification6DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol6 = request.Model.EditFormColorClassification6DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn7 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification7DisplayByCrewType != null ? request.Model.EditFormCharacterClassification7DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol7 = request.Model.EditFormColorClassification7DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn8 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification8DisplayByCrewType != null ? request.Model.EditFormCharacterClassification8DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol8 = request.Model.EditFormColorClassification8DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn9 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification9DisplayByCrewType != null ? request.Model.EditFormCharacterClassification9DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol9 = request.Model.EditFormColorClassification9DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn10 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification10DisplayByCrewType != null ? request.Model.EditFormCharacterClassification10DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol10 = request.Model.EditFormColorClassification10DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn11 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification11DisplayByCrewType != null ? request.Model.EditFormCharacterClassification11DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol11 = request.Model.EditFormColorClassification11DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn12 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification12DisplayByCrewType != null ? request.Model.EditFormCharacterClassification12DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol12 = request.Model.EditFormColorClassification12DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn13 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification13DisplayByCrewType != null ? request.Model.EditFormCharacterClassification13DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol13 = request.Model.EditFormColorClassification13DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn14 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification14DisplayByCrewType != null ? request.Model.EditFormCharacterClassification14DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol14 = request.Model.EditFormColorClassification14DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.JyoSenInfoPtnKbn15 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification15DisplayByCrewType != null ? request.Model.EditFormCharacterClassification15DisplayByCrewType.CodeKbn : "0");
                        tkmKasSet.JyoSenInfoPtnCol15 = request.Model.EditFormColorClassification15DisplayByCrewType.Replace("#", string.Empty);
                        tkmKasSet.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        tkmKasSet.UpdTime = DateTime.Now.ToString("hhmmss");
                        tkmKasSet.QuotationTransfer = (byte)request.Model.EditFormInitTransferEstimateData;
                        tkmKasSet.UpdPrgId = "KM0000P";
                        tkmKasSet.UpdSyainCd = new ClaimModel().SyainCdSeq;
                        tkmKasSet.KaizenKijunYmd = "";

                        _context.TkmKasSet.Update(tkmKasSet);
                    }
                    else
                    {
                        TkmKasSet newKasSet = new TkmKasSet()
                        {
                            CompanyCdSeq = request.Model.Company.CompanyCdSeq,
                            UriKbn = (byte)request.Model.EditFormSaleClassification,
                            SyohiHasu = (byte)request.Model.EditFormTaxFraction,
                            TesuHasu = (byte)request.Model.EditFormFeeFraction,
                            HoukoKbn = (byte)request.Model.EditFormReportClassification,
                            HouZeiKbn = (byte)request.Model.EditFormReportSummary,
                            HouOutKbn = (byte)request.Model.EditFormReportOutput,
                            JkariKbn = (byte)request.Model.EditFormAutoTemporaryBus,
                            AutKarJyun = (byte)request.Model.EditFormPriority,
                            JkbunPat = (byte)request.Model.EditFormAutoTemporaryBusDivision,
                            SyaIrePat = (byte)request.Model.EditFormVehicleReplacement,
                            JymAchkKbn = (byte)request.Model.EditFormCrewCompatibilityCheck,
                            UriHenKbn = request.Model.EditFormSaleChange ? (byte)2 : (byte)1,
                            UriMdkbn = (byte)request.Model.EditFormSalesChangeDateClassification,
                            UriHenKikan = !string.IsNullOrWhiteSpace(request.Model.EditFormSalesChangeablePeriod) ? (byte)Int32.Parse(request.Model.EditFormSalesChangeablePeriod) : (byte)0,
                            UriZeroChk = (byte)request.Model.EditFormCheckZeroYen,
                            CanKbn = request.Model.EditFormCancelClassification ? (byte)2 : (byte)1,
                            CanMdkbn = (byte)request.Model.EditFormCancelDateClassification,
                            CanKikan = !string.IsNullOrWhiteSpace(request.Model.EditFormCancellationPeriod) ? (byte)Int32.Parse(request.Model.EditFormCancellationPeriod) : (byte)0,
                            CanWaitKbn = (byte)0,
                            CanJidoKbn = (byte)0,
                            YouTesuKbn = (byte)request.Model.EditFormHiredBusFee,
                            YouSagaKbn = (byte)request.Model.EditFormHiredBusDifferentClassification,
                            SyaUntKbn = (byte)request.Model.EditFormFareByVehicle,
                            ZasyuKbn = (byte)request.Model.EditFormTransportationMiscellaneousIncome,
                            FutSf1flg = (byte)request.Model.EditFormIncidentalType1Addition,
                            FutSf2flg = (byte)request.Model.EditFormIncidentalType2Addition,
                            FutSf3flg = (byte)request.Model.EditFormIncidentalType3Addition,
                            FutSf4flg = (byte)request.Model.EditFormIncidentalType4Addition,
                            SokoJunKbn = (byte)0,
                            UntZeiKbn = (byte)request.Model.EditFormFareTaxDisplay,
                            TumZeiKbn = (byte)request.Model.EditFormLoadingGoodsTaxDisplay,
                            ColKari = Constants.WhiteColor,
                            ColKariH = Constants.WhiteColor,
                            ColNin = Constants.WhiteColor,
                            ColHai = Constants.WhiteColor,
                            ColHaiin = Constants.WhiteColor,
                            ColWari = Constants.WhiteColor,
                            ColShiha = Constants.WhiteColor,
                            ColKaku = Constants.WhiteColor,
                            ColNip = Constants.WhiteColor,
                            ColNyu = Constants.WhiteColor,
                            ColYou = Constants.WhiteColor,
                            ColMiKari = Constants.WhiteColor,
                            ColIcKari = Constants.WhiteColor,
                            ColIcKariH = Constants.WhiteColor,
                            ColIcHai = Constants.WhiteColor,
                            ColIcHaiin = Constants.WhiteColor,
                            ColIcWari = Constants.WhiteColor,
                            ColIcShiha = Constants.WhiteColor,
                            ColIcNip = Constants.WhiteColor,
                            ColIcNyu = Constants.WhiteColor,
                            ColIcYou = Constants.WhiteColor,
                            ColNcou = Constants.WhiteColor,
                            ColIcNcou = Constants.WhiteColor,
                            ColScou = Constants.WhiteColor,
                            ColIcScou = Constants.WhiteColor,
                            ColKyoy = Constants.WhiteColor,
                            ColSelect = Constants.WhiteColor,
                            ColKanyu = Constants.WhiteColor,
                            ColKahar = Constants.WhiteColor,
                            SryHyjSyu = 0,
                            SryHyjHga = 0,
                            SryHyjTde = 0,
                            SryHyjTch = 0,
                            SryHyjTka = 0,
                            CanRit1 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate1) ? decimal.Parse(request.Model.EditFormCancelRate1) : (decimal)0.0,
                            CanSkan1 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate1StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate1StartTime) : (byte)0,
                            CanEkan1 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate1EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate1EndTime) : (byte)0,
                            CanRit2 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate2) ? decimal.Parse(request.Model.EditFormCancelRate2) : (decimal)0.0,
                            CanSkan2 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate2StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate2StartTime) : (byte)0,
                            CanEkan2 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate2EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate2EndTime) : (byte)0,
                            CanRit3 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate3) ? decimal.Parse(request.Model.EditFormCancelRate3) : (decimal)0.0,
                            CanSkan3 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate3StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate3StartTime) : (byte)0,
                            CanEkan3 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate3EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate3EndTime) : (byte)0,
                            CanRit4 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate4) ? decimal.Parse(request.Model.EditFormCancelRate4) : (decimal)0.0,
                            CanSkan4 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate4StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate4StartTime) : (byte)0,
                            CanEkan4 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate4EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate4EndTime) : (byte)0,
                            CanRit5 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate5) ? decimal.Parse(request.Model.EditFormCancelRate5) : (decimal)0.0,
                            CanSkan5 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate5StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate5StartTime) : (byte)0,
                            CanEkan5 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate5EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate5EndTime) : (byte)0,
                            CanRit6 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate6) ? decimal.Parse(request.Model.EditFormCancelRate6) : (decimal)0.0,
                            CanSkan6 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate6StartTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate6StartTime) : (byte)0,
                            CanEkan6 = !string.IsNullOrWhiteSpace(request.Model.EditFormCancelRate6EndTime) ? (byte)Int32.Parse(request.Model.EditFormCancelRate6EndTime) : (byte)0,
                            GetSyoKbn = (byte)request.Model.EditFormMonthlyProcess,
                            SeiKrksKbn = (byte)request.Model.EditFormBillForward,
                            DaySyoKbn = (byte)request.Model.EditFormDailyProcess,
                            KouYouSet = 0,
                            AutKouKbn = (byte)request.Model.EditFormAutoKoban,
                            SenHyoHi = 0,
                            SenYouDefFlg = 0,
                            SenMikDefFlg = 0,
                            KoteiCopyFlg = (byte)request.Model.EditFormInitCopyProcessData,
                            FutaiCopyFlg = (byte)request.Model.EditFormInitCopyIncidentalData,
                            TumiCopyFlg = (byte)request.Model.EditFormInitCopyLoadingGoodData,
                            TehaiCopyFlg = (byte)request.Model.EditFormInitCopyArrangeData,
                            JoshaCopyFlg = (byte)request.Model.EditFormInitCopyBoardingPlaceData,
                            YykCopyFlg = (byte)request.Model.EditFormInitCopyReservationRemarkData,
                            UkbCopyFlg = (byte)request.Model.EditFormInitCopyOperationDateRemarkData,
                            SeiGenFlg = (byte)request.Model.EditFormCurrentInvoice,
                            MeiShyKbn = (byte)request.Model.EditFormDisplayDetailSelection,
                            SyaSenMjPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacter1DisplayByBusType != null ? request.Model.EditFormCharacter1DisplayByBusType.CodeKbn : "0"),
                            SyaSenMjPtnCol1 = request.Model.EditFormColor1DisplayByBusType.Replace("#", string.Empty),
                            SyaSenMjPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacter2DisplayByBusType != null ? request.Model.EditFormCharacter2DisplayByBusType.CodeKbn : "0"),
                            SyaSenMjPtnCol2 = request.Model.EditFormColor2DisplayByBusType.Replace("#", string.Empty),
                            SyaSenMjPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacter3DisplayByBusType != null ? request.Model.EditFormCharacter3DisplayByBusType.CodeKbn : "0"),
                            SyaSenMjPtnCol3 = request.Model.EditFormColor3DisplayByBusType.Replace("#", string.Empty),
                            SyaSenMjPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacter4DisplayByBusType != null ? request.Model.EditFormCharacter4DisplayByBusType.CodeKbn : "0"),
                            SyaSenMjPtnCol4 = request.Model.EditFormColor4DisplayByBusType.Replace("#", string.Empty),
                            SyaSenMjPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacter5DisplayByBusType != null ? request.Model.EditFormCharacter5DisplayByBusType.CodeKbn : "0"),
                            SyaSenMjPtnCol5 = request.Model.EditFormColor5DisplayByBusType.Replace("#", string.Empty),
                            JyoSenMjPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacter1DisplayByCrew != null ? request.Model.EditFormCharacter1DisplayByCrew.CodeKbn : "0"),
                            JyoSenMjPtnCol1 = request.Model.EditFormColor1DisplayByCrew.Replace("#", string.Empty),
                            JyoSenMjPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacter2DisplayByCrew != null ? request.Model.EditFormCharacter2DisplayByCrew.CodeKbn : "0"),
                            JyoSenMjPtnCol2 = request.Model.EditFormColor2DisplayByCrew.Replace("#", string.Empty),
                            JyoSenMjPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacter3DisplayByCrew != null ? request.Model.EditFormCharacter3DisplayByCrew.CodeKbn : "0"),
                            JyoSenMjPtnCol3 = request.Model.EditFormColor3DisplayByCrew.Replace("#", string.Empty),
                            JyoSenMjPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacter4DisplayByCrew != null ? request.Model.EditFormCharacter4DisplayByCrew.CodeKbn : "0"),
                            JyoSenMjPtnCol4 = request.Model.EditFormColor4DisplayByCrew.Replace("#", string.Empty),
                            JyoSenMjPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacter5DisplayByCrew != null ? request.Model.EditFormCharacter5DisplayByCrew.CodeKbn : "0"),
                            JyoSenMjPtnCol5 = request.Model.EditFormColor5DisplayByCrew.Replace("#", string.Empty),
                            SeiCom1 = request.Model.EditFormBillComent1,
                            SeiCom2 = request.Model.EditFormBillComent2,
                            SeiCom3 = request.Model.EditFormBillComent3,
                            SeiCom4 = request.Model.EditFormBillComent4,
                            SeiCom5 = request.Model.EditFormBillComent5,
                            SeiCom6 = request.Model.EditFormBillComent6,
                            JisKinKyuNm01 = request.Model.JisKinKyuNm01,
                            JisKinKyuNm02 = request.Model.JisKinKyuNm02,
                            JisKinKyuNm03 = request.Model.JisKinKyuNm03,
                            JisKinKyuNm04 = request.Model.JisKinKyuNm04,
                            JisKinKyuNm05 = request.Model.JisKinKyuNm05,
                            JisKinKyuNm06 = request.Model.JisKinKyuNm06,
                            JisKinKyuNm07 = request.Model.JisKinKyuNm07,
                            JisKinKyuNm08 = request.Model.JisKinKyuNm08,
                            JisKinKyuNm09 = request.Model.JisKinKyuNm09,
                            JisKinKyuNm10 = request.Model.JisKinKyuNm10,
                            SyaSenInfoPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification1DisplayByBusType != null ? request.Model.EditFormCharacterClassification1DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol1 = request.Model.EditFormColorClassification1DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification2DisplayByBusType != null ? request.Model.EditFormCharacterClassification2DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol2 = request.Model.EditFormColorClassification2DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification3DisplayByBusType != null ? request.Model.EditFormCharacterClassification3DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol3 = request.Model.EditFormColorClassification3DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification4DisplayByBusType != null ? request.Model.EditFormCharacterClassification4DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol4 = request.Model.EditFormColorClassification4DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification5DisplayByBusType != null ? request.Model.EditFormCharacterClassification5DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol5 = request.Model.EditFormColorClassification5DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn6 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification6DisplayByBusType != null ? request.Model.EditFormCharacterClassification6DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol6 = request.Model.EditFormColorClassification6DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn7 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification7DisplayByBusType != null ? request.Model.EditFormCharacterClassification7DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol7 = request.Model.EditFormColorClassification7DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn8 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification8DisplayByBusType != null ? request.Model.EditFormCharacterClassification8DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol8 = request.Model.EditFormColorClassification8DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn9 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification9DisplayByBusType != null ? request.Model.EditFormCharacterClassification9DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol9 = request.Model.EditFormColorClassification9DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn10 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification10DisplayByBusType != null ? request.Model.EditFormCharacterClassification10DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol10 = request.Model.EditFormColorClassification10DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn11 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification11DisplayByBusType != null ? request.Model.EditFormCharacterClassification11DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol11 = request.Model.EditFormColorClassification11DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn12 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification12DisplayByBusType != null ? request.Model.EditFormCharacterClassification12DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol12 = request.Model.EditFormColorClassification12DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn13 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification13DisplayByBusType != null ? request.Model.EditFormCharacterClassification13DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol13 = request.Model.EditFormColorClassification13DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn14 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification14DisplayByBusType != null ? request.Model.EditFormCharacterClassification14DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol14 = request.Model.EditFormColorClassification14DisplayByBusType.Replace("#", string.Empty),
                            SyaSenInfoPtnKbn15 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification15DisplayByBusType != null ? request.Model.EditFormCharacterClassification15DisplayByBusType.CodeKbn : "0"),
                            SyaSenInfoPtnCol15 = request.Model.EditFormColorClassification15DisplayByBusType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn1 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification1DisplayByCrewType != null ? request.Model.EditFormCharacterClassification1DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol1 = request.Model.EditFormColorClassification1DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn2 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification2DisplayByCrewType != null ? request.Model.EditFormCharacterClassification2DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol2 = request.Model.EditFormColorClassification2DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn3 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification3DisplayByCrewType != null ? request.Model.EditFormCharacterClassification3DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol3 = request.Model.EditFormColorClassification3DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn4 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification4DisplayByCrewType != null ? request.Model.EditFormCharacterClassification4DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol4 = request.Model.EditFormColorClassification4DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn5 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification5DisplayByCrewType != null ? request.Model.EditFormCharacterClassification5DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol5 = request.Model.EditFormColorClassification5DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn6 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification6DisplayByCrewType != null ? request.Model.EditFormCharacterClassification6DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol6 = request.Model.EditFormColorClassification6DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn7 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification7DisplayByCrewType != null ? request.Model.EditFormCharacterClassification7DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol7 = request.Model.EditFormColorClassification7DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn8 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification8DisplayByCrewType != null ? request.Model.EditFormCharacterClassification8DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol8 = request.Model.EditFormColorClassification8DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn9 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification9DisplayByCrewType != null ? request.Model.EditFormCharacterClassification9DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol9 = request.Model.EditFormColorClassification9DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn10 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification10DisplayByCrewType != null ? request.Model.EditFormCharacterClassification10DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol10 = request.Model.EditFormColorClassification10DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn11 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification11DisplayByCrewType != null ? request.Model.EditFormCharacterClassification11DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol11 = request.Model.EditFormColorClassification11DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn12 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification12DisplayByCrewType != null ? request.Model.EditFormCharacterClassification12DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol12 = request.Model.EditFormColorClassification12DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn13 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification13DisplayByCrewType != null ? request.Model.EditFormCharacterClassification13DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol13 = request.Model.EditFormColorClassification13DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn14 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification14DisplayByCrewType != null ? request.Model.EditFormCharacterClassification14DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol14 = request.Model.EditFormColorClassification14DisplayByCrewType.Replace("#", string.Empty),
                            JyoSenInfoPtnKbn15 = (byte)Int32.Parse(request.Model.EditFormCharacterClassification15DisplayByCrewType != null ? request.Model.EditFormCharacterClassification15DisplayByCrewType.CodeKbn : "0"),
                            JyoSenInfoPtnCol15 = request.Model.EditFormColorClassification15DisplayByCrewType.Replace("#", string.Empty),
                            JyoSyaChkSiyoKbn = 0,
                            SenDayRenge = 0,
                            SenDefWidth = 0,
                            YykHaiStime = "0000",
                            YykTouTime = "0000",
                            TehaiAutoSet = 0,
                            GoSyaAutoSet = 0,
                            YoySyuKiTimeSiyoKbn = 0,
                            KarSyuKiTimeSiyoKbn = 0,
                            GuiAutoSet = 0,
                            DrvAutoSet = 0,
                            SenBackPtnKbn = 0,
                            SenBackPtnCol = Constants.WhiteColor,
                            SenObptnKbn = 0,
                            SenObptnCol = Constants.WhiteColor,
                            FutTumCdSeq = 0,
                            EtckinKbn = 0,
                            SeisanCdSeq = 0,
                            GuideFutTumCdSeq = 0,
                            SeiMuki = 0,
                            ExpItem = string.Empty,
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                            UpdTime = DateTime.Now.ToString("hhmmss"),
                            QuotationTransfer = (byte)request.Model.EditFormInitTransferEstimateData,
                            UpdPrgId = "KM0000P",
                            UpdSyainCd = new ClaimModel().SyainCdSeq,
                            KaizenKijunYmd = ""
                        };
                        _context.TkmKasSet.Add(newKasSet);
                    }
                    _context.SaveChanges();
                }
                catch(Exception ex)
                {

                }
                
                return true;
            }
        }
    }
}
