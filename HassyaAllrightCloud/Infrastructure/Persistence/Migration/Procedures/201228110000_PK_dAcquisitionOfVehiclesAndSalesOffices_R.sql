USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetAlertSettingByCode36_R]    Script Date: 1/7/2021 3:17:16 PM ******/
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
-- Description	:   Get alert setting by code base on sheet excel 3.6
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dAcquisitionOfVehiclesAndSalesOffices_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@StaYmd					CHAR(8),
		@EndYmd					CHAR(8),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT HENSYA.SyaRyoCdSeq --AS '車両Seq'
        ,HENSYA.StaYmd --AS '開始年月日'
        ,HENSYA.EndYmd --AS '終了年月日'
        ,HENSYA.EigyoCdSeq --AS '営業所Seq'
        ,KAISHA.CompanyCdSeq --AS '会社Seq'
        ,SYASYU.KataKbn --AS '型区分'
FROM VPM_HenSya AS HENSYA
INNER JOIN VPM_SyaRyo AS SYARYO ON SYARYO.SyaRyoCdSeq = HENSYA.SyaRyoCdSeq
INNER JOIN VPM_Eigyos AS EIGYOS ON EIGYOS.EigyoCdSeq = HENSYA.EigyoCdSeq
INNER JOIN VPM_Compny AS KAISHA ON KAISHA.CompanyCdSeq = EIGYOS.CompanyCdSeq
        AND KAISHA.TenantCdSeq = @TenantCdSeq                  
INNER JOIN VPM_SyaSyu AS SYASYU ON SYASYU.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq
        AND SYASYU.SiyoKbn = 1
        AND SYASYU.TenantCdSeq = @TenantCdSeq                  
WHERE HENSYA.StaYmd <= @StaYmd                              
        AND HENSYA.EndYmd >= @EndYmd                        
        AND SYARYO.NinkaKbn <> 7 --オーバー車両以外                             

SET	@ROWCOUNT	=	@@ROWCOUNT
END
