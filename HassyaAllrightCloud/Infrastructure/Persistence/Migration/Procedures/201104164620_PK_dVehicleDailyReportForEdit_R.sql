USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dVehicleDailyReportForEdit_R]    Script Date: 10/14/2020 9:08:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAllrightCloud
-- Module-Name	:   HassyaAllrightCloud
-- SP-ID		:   PK_dVehicleDailyReportsForEdit_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get vehicle daily report list for edit
-- Date			:   2020/08/11
-- Author		:   P.M.NHAT
-- Description	:   Get vehicle daily report list for edit with conditions
-- =============================================
ALTER   PROCEDURE [dbo].[PK_dVehicleDailyReportForEdit_R]
(
	-- Parameter
		@UkeNo			nchar(15)	
	,	@UnkRen			smallint	
	,	@TeiDanNo		smallint			
	,	@BunkRen		smallint	
	,   @UnkYmd         varchar(8)
)
AS
	BEGIN
		SELECT TKD_Shabni.UnkYmd AS UnkYmd
			,TKD_Haisha.UkeNo AS UkeNo
			,TKD_Haisha.UnkRen AS UnkRen
			,TKD_Haisha.TeiDanNo AS TeiDanNo
			,TKD_Haisha.BunkRen AS BunkRen
			,ISNULL(TKD_Shabni.StMeter, 0.00) AS StMeter
			,ISNULL(TKD_Shabni.EndMeter, 0.00) AS EndMeter
			,ISNULL(TKD_Shabni.SyuKoTime, '0000') AS SyuKoTime
			,ISNULL(TKD_Shabni.KikTime, '0000') AS KikTime
			,ISNULL(TKD_Shabni.KoskuTime, '0000') AS KoskuTime
			,ISNULL(TKD_Shabni.SyuPaTime, '0000') AS SyuPaTime
			,ISNULL(TKD_Shabni.TouChTime, '0000') AS TouChTime
			,ISNULL(TKD_Shabni.JisTime, '0000') AS JisTime
			,ISNULL(TKD_Shabni.InspectionTime, '0200') AS InspectionTime
			,ISNULL(TKD_Shabni.JisaIPKm, 0.00) AS JisaIPKm
			,ISNULL(TKD_Shabni.JisaKSKm, 0.00) AS JisaKSKm
			,ISNULL(TKD_Shabni.KisoIPKm, 0.00) AS KisoIPKm
			,ISNULL(TKD_Shabni.KisoKOKm, 0.00) AS KisoKOKm
			,ISNULL(TKD_Shabni.OthKm, 0.00) AS OthKm
			,ISNULL(TKD_Shabni.Nenryo1, 0.00) AS Nenryo1
			,ISNULL(TKD_Shabni.Nenryo2, 0.00) AS Nenryo2
			,ISNULL(TKD_Shabni.Nenryo3, 0.00) AS Nenryo3
			,ISNULL(TKD_Shabni.JyoSyaJin, 0) AS JyoSyaJin
			,ISNULL(TKD_Shabni.PlusJin, 0) AS PlusJin
			,ISNULL(TKD_Shabni.UnkKai, 1) AS UnkKai
			,ISNULL(TKD_Haisha.HaiSYmd, ' ') AS HaiSYmd
			,ISNULL(TKD_Haisha.TouYmd, ' ') AS TouYmd
			,ISNULL(JM_SyaRyo.SyaRyoCd, ' ') AS SyaRyoCd
			,ISNULL(JM_SyaRyo.SyaRyoNm, ' ') AS SyaRyoNm
			,ISNULL(JM_SyaRyo.NenryoCd1Seq, ' ') AS NenryoCd1Seq
			,ISNULL(JM_SyaRyo.NenryoCd2Seq, ' ') AS NenryoCd2Seq
			,ISNULL(JM_SyaRyo.NenryoCd3Seq, ' ') AS NenryoCd3Seq
			,ISNULL(TKD_Haisha.SyuKoYmd, ' ') AS SyuKoYmd
			,ISNULL(TKD_Haisha.KikYmd, ' ') AS KikYmd
			,ISNULL(TKD_Haisha.NippoKbn, ' ') AS NippoKbn
		FROM TKD_Haisha
		LEFT JOIN TKD_Shabni ON TKD_Shabni.UkeNo = TKD_Haisha.UkeNo
				AND TKD_Shabni.UnkRen = TKD_Haisha.UnkRen
				AND TKD_Shabni.TeiDanNo = TKD_Haisha.TeiDanNo
				AND TKD_Shabni.BunkRen = TKD_Haisha.BunkRen
				AND TKD_Haisha.SiyoKbn = 1
		LEFT JOIN VPM_SyaRyo AS JM_SyaRyo ON TKD_Haisha.HaiSSryCdSeq = JM_SyaRyo.SyaRyoCdSeq
		WHERE TKD_Haisha.UkeNo = @UkeNo
				AND TKD_Haisha.UnkRen = @UnkRen
				AND TKD_Haisha.TeiDanNo = @TeiDanNo
				AND TKD_Haisha.BunkRen = @BunkRen			
				AND TKD_Shabni.UnkYmd = @UnkYmd
	END
