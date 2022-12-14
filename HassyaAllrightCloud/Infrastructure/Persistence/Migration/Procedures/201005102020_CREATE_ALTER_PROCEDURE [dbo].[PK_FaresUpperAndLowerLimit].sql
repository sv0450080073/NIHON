USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_FaresUpperAndLowerLimit]    Script Date: 10/05/2020 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_FaresUpperAndLowerLimit
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Fares Upper And Lower Limit
-- Date			:   2020/10/05
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].PK_FaresUpperAndLowerLimit
    (
     -- Parameter
	       @TenantCdSeq                     VARCHAR(8)
		,  @BackToGarageYmdStr              VARCHAR(8)
	    ,  @BackToGarageYmdEnd              VARCHAR(8)
		,  @DispatchYmdStr                  VARCHAR(8)
		,  @DispatchYmdEnd                  VARCHAR(8)
		,  @ArrivalYmdStr                   VARCHAR(8)
		,  @ArrivalYmdEnd                   VARCHAR(8)
		,  @EigyoCd                         VARCHAR(8)
		,  @EigSyainCd                      VARCHAR(10)
		,  @UkeNoStr                        VARCHAR(15)
		,  @UkeNoEnd                        VARCHAR(15)
		,  @Classification11                VARCHAR(2)
		,  @Classification12                VARCHAR(2)
		,  @Classification13                VARCHAR(2)
		,  @InOrOutPlan                     VARCHAR(2)
		,  @HaveCauseOrNot                  VARCHAR(2)
		,  @HaveCauseClassification11       VARCHAR(2)
		,  @CauseClassification2            VARCHAR(2)
	    ,  @CauseClassification3            VARCHAR(2)

        -- Output
	    ,  @ROWCOUNT	          INTEGER OUTPUT	   -- 処理件数
    )
AS 
    -- Processing
	BEGIN
	SELECT ISNULL(JT_Unkobi.UkeNo, 0) AS UkeNo																																		
	        ,ISNULL(JT_Unkobi.UnkRen, 0) AS UnkRen																																		
	        ,ISNULL(JT_YykSyuSumBooking.SyaSyuRen, 0) AS SyaSyuRen																																		
	        ,ISNULL(TKD_Haisha.TeiDanNo, 0) AS TeiDanNo																																		
	        ,ISNULL(TKD_Haisha.BunkRen, 0) AS BunkRen																																		
	        ,ISNULL(JM_Syain.SyainCd, 0) AS SyainCd																																		
	        ,ISNULL(JM_SyaRyo.SyaRyoCdSeq, 0) AS SyaRyoCdSeq																																		
	        ,ISNULL(JM_SyaRyo.SyaRyoCd, 0) AS SyaRyoCd																																		
	        ,ISNULL(JM_SyaRyo.SyaryoNm, '') AS SyaryoNm																																		
	        ,ISNULL(JM_SyaRyo.KariSyaRyoNm, '') AS KariSyaRyoNm																																		
	        ,ISNULL(JM_SyaSyu.SyaSyuCd, '') AS SyaSyuCd																																		
	        ,ISNULL(JM_SyaSyu.SyaSyuNm, '') AS SyaSyuNm																																		
	        ,ISNULL(JM_SyaSyu.KataKbn, '') AS KataKbn																																		
	        ,ISNULL(TKD_Haisha.HaiSYmd, '') AS HaiSYmd																																		
	        ,ISNULL(TKD_Haisha.TouYmd, '') AS TouYmd																																		
	        ,ISNULL(TKD_Haisha.SyukoYmd, '') AS SyukoYmd																																		
	        ,ISNULL(TKD_Haisha.KikYmd, '') AS KikYmd																																		
	        ,ISNULL(JT_MaxMinFareFeeCause.UpperLowerCauseKbn, 0) AS CauseKbn																																		
	        ,ISNULL(JT_CauseCodeKb.CodeKbnNm, '') AS CauseNm																																		
	        ,ISNULL(JM_YykSyu.SyaRyoUnc, 0) AS SeikyuGaku																																		
	        ,ISNULL(JT_Shabni.StMeter, 0.00) AS StMeter																																		
	        ,ISNULL(JT_Shabni.EndMeter, 0.00) AS EndMeter																																		
	        ,ISNULL(dbo.FP_ConvertIntToCharTime(JT_Shabni.KoskuTimeHours, JT_Shabni.KoskuTimeMinutes), '0000') AS KoskuTime																																		
	        ,ISNULL(dbo.FP_ConvertIntToCharTime(JT_Shabni.InspectionTimeHours, JT_Shabni.InspectionTimeMinutes), '0200') AS InspectionTime																																		
	        ,ISNULL(JT_YykSyuSumBooking.RunningKmSum, 0) AS RunningKmSum																																		
	        ,ISNULL(JT_YykSyuSumBooking.RestraintTimeSum, '0000') AS RestraintTimeSum																																		
	        ,ISNULL(JT_Shabni.FareFeeMaxAmount, 0) AS JissekiSumMaxAmount																																		
	        ,ISNULL(JT_Shabni.FareFeeMinAmount, 0) AS JissekiSumMinAmount																																		
	        ,ISNULL((JT_YykSyuSumBooking.SumFareMaxAmount + JT_YykSyuSumBooking.SumFeeMaxAmount), 0) AS MitsumoriSumMaxAmount																																		
	        ,ISNULL((JT_YykSyuSumBooking.SumFareMinAmount + JT_YykSyuSumBooking.SumFeeMinAmount), 0) AS MitsumoriSumMinAmount																																		
	        ,ISNULL(JT_YykSyuSumBooking.ChangeDriverFeeFlag, 0) AS ChangeDriverFeeFlag																																		
	        ,ISNULL(JT_YykSyuSumBooking.SpecialFlg, 0) AS SpecialFlg																																		
	        ,ISNULL(JT_YykSyuSumBooking.DisabledPersonDiscount, 0) AS DisabledPersonDiscount																																		
	        ,ISNULL(JT_YykSyuSumBooking.SchoolDiscount, 0) AS SchoolDiscount																																		
	        ,ISNULL(JM_Eigyos.EigyoCdSeq, 0) AS EigyoCdSeq																																		
	        ,ISNULL(JM_Eigyos.EigyoCd, 0) AS EigyoCd																																		
	        ,ISNULL(JM_Eigyos.RyakuNm, '') AS EigyoRyakuNm																																		
	FROM TKD_Haisha																																		
	LEFT JOIN TKD_Yyksho AS JT_Yyksho ON TKD_Haisha.UkeNo = JT_Yyksho.UkeNo																																		
	        AND JT_Yyksho.SiyoKbn = 1																																		
	        AND JT_Yyksho.TenantCdSeq = @TenantCdSeq																																		
	LEFT JOIN TKD_Unkobi AS JT_Unkobi ON TKD_Haisha.UkeNo = JT_Unkobi.UkeNo																																		
	        AND TKD_Haisha.UnkRen = JT_Unkobi.UnkRen																																		
	        AND JT_Unkobi.SiyoKbn = 1																																		
	LEFT JOIN (																																		
	        SELECT UkeNo																																		
	                ,UnkRen																																		
	                ,TeiDanNo																																		
	                ,BunkRen																																		
	                ,SUM(StMeter) AS StMeter																																		
	                ,SUM(EndMeter) AS EndMeter																																		
	                ,AVG(FeeMinAmount) + SUM(FareMinAmount) AS FareFeeMinAmount																																		
	                ,AVG(FeeMaxAmount) + SUM(FareMaxAmount) AS FareFeeMaxAmount																																		
	                ,SUM(CONVERT(INT, SUBSTRING(KoskuTime, 1, 2)) + (CONVERT(INT, SUBSTRING(KoskuTime, 3, 2)) / 60)) AS KoskuTimeHours																																		
	                ,SUM(CONVERT(INT, SUBSTRING(KoskuTime, 3, 2)) % 60) AS KoskuTimeMinutes																																		
	                ,SUM(CONVERT(INT, SUBSTRING(InspectionTime, 1, 2)) + (CONVERT(INT, SUBSTRING(InspectionTime, 3, 2)) / 60)) AS InspectionTimeHours																																		
	                ,SUM(CONVERT(INT, SUBSTRING(InspectionTime, 3, 2)) % 60) AS InspectionTimeMinutes																																		
	        FROM TKD_Shabni																																		
	        GROUP BY UkeNo																																		
	                ,UnkRen																																		
	                ,TeiDanNo																																		
	                ,BunkRen																																		
	        ) AS JT_Shabni ON JT_Shabni.UkeNo = TKD_Haisha.UkeNo																																		
	        AND JT_Shabni.UnkRen = TKD_Haisha.UnkRen																																		
	        AND JT_Shabni.TeiDanNo = TKD_Haisha.TeiDanNo																																		
	        AND JT_Shabni.BunkRen = TKD_Haisha.BunkRen																																		
	LEFT JOIN TKD_MaxMinFareFeeCause AS JT_MaxMinFareFeeCause ON JT_MaxMinFareFeeCause.UkeNo = TKD_Haisha.UkeNo																																		
	        AND JT_MaxMinFareFeeCause.UnkRen = TKD_Haisha.UnkRen																																		
	        AND JT_MaxMinFareFeeCause.TeiDanNo = TKD_Haisha.TeiDanNo																																		
	        AND JT_MaxMinFareFeeCause.BunkRen = TKD_Haisha.BunkRen																																		
	        AND JT_MaxMinFareFeeCause.SiyoKbn = 1																																		
	LEFT JOIN VPM_CodeKb AS JT_CauseCodeKb ON JT_CauseCodeKb.CodeKbn = JT_MaxMinFareFeeCause.UpperLowerCauseKbn																																		
	        AND JT_CauseCodeKb.CodeSyu = 'UPPERLOWERCAUSE'																																		
	        AND JT_CauseCodeKb.SiyoKbn = 1																																		
	        AND JT_CauseCodeKb.TenantCdSeq = (																																		
	                SELECT CASE 																																		
	                                WHEN COUNT(*) = 0																																		
	                                        THEN 0																																		
	                                ELSE 1																																		
	                                END AS TenantCdSeq																																		
	                FROM VPM_CodeKb																																		
	                WHERE CodeSyu = 'UPPERLOWERCAUSE'																																		
	                        AND SiyoKbn = 1																																		
	                        AND TenantCdSeq = 1																																		
	                )																																		
	LEFT JOIN (																																		
	        SELECT TKD_YykSyu.UkeNo																																		
	                ,TKD_YykSyu.UnkRen																																		
	                ,TKD_YykSyu.SyaSyuRen																																		
	                ,TKD_YykSyu.SyaSyuDai																																		
	                ,TKD_BookingMaxMinFareFeeCalc.WaribikiKbn																																		
	                ,(																																		
	                        CASE 																																		
	                                WHEN TKD_BookingMaxMinFareFeeCalc.WaribikiKbn = 1																																		
	                                        THEN 1																																		
	                                ELSE 0																																		
	                                END																																		
	                        ) AS DisabledPersonDiscount																																		
	                ,(																																		
	                        CASE 																																		
	                                WHEN TKD_BookingMaxMinFareFeeCalc.WaribikiKbn = 2																																		
	                                        THEN 1																																		
	                                ELSE 0																																		
	                                END																																		
	                        ) AS SchoolDiscount																																		
	                ,(TKD_BookingMaxMinFareFeeCalc.FareMaxAmount * TKD_YykSyu.SyaSyuDai) AS SumFareMaxAmount																																		
	                ,(TKD_BookingMaxMinFareFeeCalc.FareMinAmount * TKD_YykSyu.SyaSyuDai) AS SumFareMinAmount																																		
	                ,(TKD_BookingMaxMinFareFeeCalc.FeeMaxAmount * TKD_YykSyu.SyaSyuDai) AS SumFeeMaxAmount																																		
	                ,(TKD_BookingMaxMinFareFeeCalc.FeeMinAmount * TKD_YykSyu.SyaSyuDai) AS SumFeeMinAmount																																		
	                ,TKD_BookingMaxMinFareFeeCalc.RunningKmSum																																		
	                ,TKD_BookingMaxMinFareFeeCalc.RestraintTimeSum																																		
	                ,TKD_BookingMaxMinFareFeeCalc.SpecialFlg																																		
	                ,TKD_BookingMaxMinFareFeeCalcMeisai.ChangeDriverFeeFlag																																		
	        FROM TKD_YykSyu																																		
	        LEFT JOIN TKD_BookingMaxMinFareFeeCalc ON TKD_YykSyu.UkeNo = TKD_BookingMaxMinFareFeeCalc.UkeNo																																		
	                AND TKD_YykSyu.UnkRen = TKD_BookingMaxMinFareFeeCalc.UnkRen																																		
	                AND TKD_YykSyu.SyaSyuRen = TKD_BookingMaxMinFareFeeCalc.SyaSyuRen																																		
	        LEFT JOIN TKD_BookingMaxMinFareFeeCalcMeisai ON TKD_BookingMaxMinFareFeeCalc.UkeNo = TKD_BookingMaxMinFareFeeCalcMeisai.UkeNo																																		
	                AND TKD_BookingMaxMinFareFeeCalc.UnkRen = TKD_BookingMaxMinFareFeeCalcMeisai.UnkRen																																		
	                AND TKD_BookingMaxMinFareFeeCalc.SyaSyuRen = TKD_BookingMaxMinFareFeeCalcMeisai.SyaSyuRen																																		
	        ) AS JT_YykSyuSumBooking ON TKD_Haisha.UkeNo = JT_YykSyuSumBooking.UkeNo																																		
	        AND TKD_Haisha.UnkRen = JT_YykSyuSumBooking.UnkRen																																		
	        AND TKD_Haisha.SyaSyuRen = JT_YykSyuSumBooking.SyaSyuRen																																		
	LEFT JOIN VPM_SyaRyo AS JM_SyaRyo ON TKD_Haisha.HaiSSryCdSeq = JM_SyaRyo.SyaRyoCdSeq																																		
	LEFT JOIN TKD_YykSyu AS JM_YykSyu ON TKD_Haisha.UkeNo = JM_YykSyu.UkeNo																																		
	        AND TKD_Haisha.UnkRen = JM_YykSyu.UnkRen																																		
	        AND TKD_Haisha.SyaSyuRen = JM_YykSyu.SyaSyuRen																																		
	LEFT JOIN VPM_HenSya AS JM_HenSya ON TKD_Haisha.HaiSSryCdSeq = JM_HenSya.SyaRyoCdSeq																																		
	        AND TKD_Haisha.HaiSYmd >= JM_HenSya.StaYmd																																		
	        AND TKD_Haisha.HaiSYmd <= JM_HenSya.EndYmd																																		
	LEFT JOIN VPM_Eigyos AS JM_Eigyos ON JM_HenSya.EigyoCdSeq = JM_Eigyos.EigyoCdSeq																																		
	        AND JM_Eigyos.SiyoKbn = 1																																		
	LEFT JOIN VPM_Compny AS JM_Compny ON JM_Eigyos.CompanyCdSeq = JM_Compny.CompanyCdSeq																																		
	        AND JM_Compny.SiyoKbn = 1																																		
	LEFT JOIN VPM_Tenant AS JM_Tenant ON JM_Tenant.TenantCdSeq = JM_Compny.TenantCdSeq																																		
	        AND JM_Tenant.SiyoKbn = 1																																		
	LEFT JOIN VPM_YoyKbn AS JM_YoyKbn ON JT_Yyksho.YoyaKbnSeq = JM_YoyKbn.YoyaKbnSeq																																		
	        AND JM_YoyKbn.SiyoKbn = 1																																		
	LEFT JOIN VPM_SyaSyu AS JM_SyaSyu ON JM_SyaRyo.SyaSyuCdSeq = JM_SyaSyu.SyaSyuCdSeq																																		
	LEFT JOIN VPM_Syain AS JM_Syain ON JM_Syain.SyainCdSeq = JT_Yyksho.EigTanCdSeq																																		
	WHERE 1 = 1																																		
	        AND JT_Yyksho.YoyaSyu = 1																																		
	        AND TKD_Haisha.YouTblSeq = 0																																		
	        AND TKD_Haisha.HaiSSryCdSeq <> 0
			AND (@BackToGarageYmdStr IS NULL OR @BackToGarageYmdStr = '' OR TKD_Haisha.KikYmd >= @BackToGarageYmdStr)
			AND (@BackToGarageYmdEnd IS NULL OR @BackToGarageYmdEnd = '' OR TKD_Haisha.KikYmd <= @BackToGarageYmdEnd)
			AND (@DispatchYmdStr IS NULL OR @DispatchYmdStr = '' OR TKD_Haisha.HaiSYmd >= @DispatchYmdStr)
			AND (@DispatchYmdEnd IS NULL OR @DispatchYmdEnd = '' OR TKD_Haisha.HaiSYmd <= @DispatchYmdEnd)
			AND (@ArrivalYmdStr IS NULL OR @ArrivalYmdStr = '' OR TKD_Haisha.TouYmd >= @ArrivalYmdStr)
			AND (@ArrivalYmdEnd IS NULL OR @ArrivalYmdEnd = '' OR TKD_Haisha.TouYmd <= @ArrivalYmdEnd)
			AND (@EigyoCd IS NULL OR @EigyoCd = '' OR JM_Eigyos.EigyoCd = @EigyoCd)
			AND (@EigSyainCd IS NULL OR @EigSyainCd = '' OR JM_Syain.SyainCd = @EigSyainCd)
			AND (@UkeNoStr IS NULL OR @UkeNoStr = '' OR JT_Unkobi.UkeNo >= @UkeNoStr)
			AND (@UkeNoEnd IS NULL OR @UkeNoEnd = '' OR JT_Unkobi.UkeNo <= @UkeNoEnd)
			AND (@Classification11 IS NULL OR @Classification11 = '' OR (JM_YykSyu.SyaRyoUnc NOT BETWEEN JT_Shabni.FareFeeMinAmount AND JT_Shabni.FareFeeMaxAmount                                                                                                          																																		
	        OR  JM_YykSyu.SyaRyoUnc NOT BETWEEN (JT_YykSyuSumBooking.SumFareMinAmount + JT_YykSyuSumBooking.SumFeeMinAmount) 
			    AND (JT_YykSyuSumBooking.SumFareMaxAmount + JT_YykSyuSumBooking.SumFeeMaxAmount)))
			AND (@Classification12 IS NULL OR @Classification12 = '' OR (JM_YykSyu.SyaRyoUnc NOT BETWEEN JT_Shabni.FareFeeMinAmount AND JT_Shabni.FareFeeMaxAmount                                                                                                          																																		
	            OR JM_YykSyu.SyaRyoUnc NOT BETWEEN (JT_YykSyuSumBooking.SumFareMinAmount + JT_YykSyuSumBooking.SumFeeMinAmount) AND (JT_YykSyuSumBooking.SumFareMaxAmount + JT_YykSyuSumBooking.SumFeeMaxAmount))                                                                                                                                       																																		
	            AND JT_YykSyuSumBooking.RunningKmSum <> (JT_Shabni.EndMeter - JT_Shabni.StMeter))
			AND (@Classification13 IS NULL OR @Classification13 = '' OR (JM_YykSyu.SyaRyoUnc NOT BETWEEN JT_Shabni.FareFeeMinAmount AND JT_Shabni.FareFeeMaxAmount                                                                                                                                                                                                                                                                                                                                                                                                                                  																																		
	            OR JM_YykSyu.SyaRyoUnc NOT BETWEEN (JT_YykSyuSumBooking.SumFareMinAmount + JT_YykSyuSumBooking.SumFeeMinAmount) AND (JT_YykSyuSumBooking.SumFareMaxAmount + JT_YykSyuSumBooking.SumFeeMaxAmount))                                                                                                                                                                                                                                                                                                                                                                                                                                       																																		
	            AND JT_YykSyuSumBooking.RestraintTimeSum <> dbo.FP_CalSmallIntTime(JT_Shabni.KoskuTimeHours, JT_Shabni.KoskuTimeMinutes, JT_Shabni.InspectionTimeHours, JT_Shabni.InspectionTimeMinutes))
			AND (@InOrOutPlan IS NULL OR @InOrOutPlan = '' OR (JM_YykSyu.SyaRyoUnc BETWEEN JT_Shabni.FareFeeMinAmount AND JT_Shabni.FareFeeMaxAmount                                                                                                               																																		
	            AND JM_YykSyu.SyaRyoUnc BETWEEN (JT_YykSyuSumBooking.SumFareMinAmount + JT_YykSyuSumBooking.SumFeeMinAmount) AND (JT_YykSyuSumBooking.SumFareMaxAmount + JT_YykSyuSumBooking.SumFeeMaxAmount)))
			AND (@HaveCauseOrNot IS NULL OR @HaveCauseOrNot = '' OR JT_MaxMinFareFeeCause.UpperLowerCauseKbn IS NULL)
			AND (@HaveCauseClassification11 IS NULL OR @HaveCauseClassification11 = '' OR (JT_MaxMinFareFeeCause.UpperLowerCauseKbn IS NOT NULL))
			AND (@CauseClassification2 IS NULL OR @CauseClassification2 = '' OR JT_MaxMinFareFeeCause.UpperLowerCauseKbn = 1)
			AND (@CauseClassification3 IS NULL OR @CauseClassification3 = '' OR JT_MaxMinFareFeeCause.UpperLowerCauseKbn = 6)
		ORDER BY UkeNo
    SET	@ROWCOUNT	=	@@ROWCOUNT
	END
RETURN																													