USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetAlertSettingByCode37_R]    Script Date: 1/7/2021 3:18:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_d_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   GetAlertSettingByCode
-- Date			:   2020/12/22
-- Author		:   T.L.DUY
-- Description	:   Get alert setting by code base on sheet excel 3.7
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dAcquisitionProcessOfReservedVehicleInformation_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@StaYmd					CHAR(8),
		@EndYmd					CHAR(8),
		@SharyoSeqList			NVARCHAR(100),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT HAISHA.UkeNo --AS '受付番号'
        ,HAISHA.UnkRen --AS '運行日連番'
        ,HAISHA.SyaSyuRen --AS '車種連番'
        ,HAISHA.TeiDanNo --AS '悌団番号'
        ,HAISHA.BunkRen --AS '分割連番'
        ,HAISHA.HaiSSryCdSeq --AS '配車車輌コードＳＥＱ'
        ,HAISHA.SyuKoYmd --AS '出庫年月日'
        ,HAISHA.KikYmd --AS '帰庫年月日'
        ,SYASYU.KataKbn --AS '型区分'
FROM TKD_Haisha AS HAISHA
INNER JOIN TKD_Yyksho AS YYKSHO ON YYKSHO.UkeNo = HAISHA.UkeNo
        AND YYKSHO.SiyoKbn = 1
        AND YYKSHO.TenantCdSeq = @TenantCdSeq                     
LEFT JOIN VPM_SyaRyo AS SYARYO ON SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq
LEFT JOIN VPM_SyaSyu AS SYASYU ON SYASYU.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq
        AND SYASYU.TenantCdSeq = @TenantCdSeq                        
WHERE HAISHA.SyuKoYmd <= @EndYmd
        AND HAISHA.KikYmd >= @StaYmd
        AND HAISHA.HaiSSryCdSeq IN (SELECT value FROM STRING_SPLIT(@SharyoSeqList, ','))                              
        AND YYKSHO.YoyaSyu = 1 --1：受注                                   
        AND HAISHA.SiyoKbn = 1
        AND HAISHA.HaiSSryCdSeq <> 0 --配車済／仮車済のデータのみ                                    
        AND HAISHA.YouTblSeq = 0 --傭車以外                                 
ORDER BY HAISHA.HaiSSryCdSeq ASC
        ,HAISHA.SyuKoYmd ASC
        ,HAISHA.KikYmd ASC

SET	@ROWCOUNT	=	@@ROWCOUNT
END
