USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetAlertSettingByCode311_R]    Script Date: 1/7/2021 3:08:32 PM ******/
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
-- Description	:   Get alert setting by code base on sheet excel 3.11
------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[PK_dVehicleAcquisitionProcess_R] 
	(
	-- Parameter
		@TenantCdSeq			INT,
		@StaYmd					NVARCHAR(8),
		@EndYmd					NVARCHAR(8),
	-- Output
		@ROWCOUNT				INTEGER OUTPUT		-- RowCount
	)
AS
	SET		@RowCount =	0		-- 
	-- Processing
BEGIN
SELECT DISTINCT Yoyakusho.UkeNo
        ,Yoyakusho.UkeEigCdSeq
        ,Yoyakusho.YoyaKbnSeq
        ,Yoyakusho.Zeiritsu
        ,Yoyakusho.InTanCdSeq
        ,Yoyakusho.KaktYmd
        ,Yoyakusho.SeiTaiYmd
        ,Unkobi.DanTaNm
        ,YoyakuSyasyu.UnkRen
        ,YoyakuSyasyu.SyaSyuRen
        ,YoyakuSyasyu.KataKbn
        ,Haisha.UkeNo
        ,Haisha.UnkRen
        ,Haisha.TeiDanNo
        ,Haisha.BunkRen
        ,Haisha.GoSya
        ,Haisha.HenKai
        ,Haisha.BunKsyuJyn
        ,Haisha.HaiSsryCdSeq
        ,Haisha.KssyaRseq
        ,Haisha.SyuKoYmd
        ,Haisha.SyuKoTime
        ,Haisha.SyuEigCdSeq
        ,Haisha.KikEigSeq
        ,Haisha.HaiSkigou
        ,Haisha.HaiSsetTime
        ,Haisha.IkNm
        ,Haisha.HaiSymd
        ,Haisha.HaiStime
        ,Haisha.HaiSnm
        ,Haisha.HaiSjyus1
        ,Haisha.HaiSjyus2
        ,Haisha.TouYmd
        ,Haisha.TouChTime
        ,Haisha.TouNm
        ,Haisha.TouKigou
        ,Haisha.TouSetTime
        ,Haisha.TouJyusyo1
        ,Haisha.TouJyusyo2
        ,Haisha.SyuPaTime
        ,Haisha.JyoSyaJin
        ,Haisha.PlusJin
        ,Haisha.DrvJin
        ,Haisha.GuiSu
        ,Haisha.OthJin1
        ,Haisha.OthJin2
        ,Haisha.PlatNo
        ,Haisha.HaiCom
        ,Haisha.KikYmd
        ,Haisha.KikTime
        ,Haisha.Kskbn
        ,Haisha.YouKataKbn
        ,Haisha.YouTblSeq
        ,Haisha.SyaRyoUnc
        ,Haisha.HaiSkbn
        ,Haisha.HaiIkbn
        ,Haisha.NippoKbn
        ,SyaSyu.SyaSyuNm
        ,SyaSyu.SyaSyuKigo
        ,SyaRyo.SyaRyoNm
        ,SyaRyo.SyaSyuCdSeq
        ,SyaRyo.TeiCnt
        ,TokuisakiMaster.TokuiSeq
        ,TokuisakiMaster.RyakuNm
        ,TokuisakiSitenMaster.TokuiSeq
        ,TokuisakiSitenMaster.RyakuNm
        ,Yousha.YouTblSeq
        ,Yousha.YouCdSeq
        ,Yousha.YouSitCdSeq
        ,SyaRyo.NinkaKbn
FROM TKD_Yyksho Yoyakusho
LEFT JOIN TKD_Unkobi Unkobi ON Yoyakusho.UkeNo = Unkobi.UkeNo
LEFT JOIN TKD_Haisha Haisha ON Unkobi.UkeNo = Haisha.UkeNo
        AND Unkobi.UnkRen = Haisha.UnkRen
LEFT JOIN TKD_YykSyu YoyakuSyasyu ON Haisha.UkeNo = YoyakuSyasyu.UkeNo
        AND Haisha.UnkRen = YoyakuSyasyu.UnkRen
        AND Haisha.SyaSyuRen = YoyakuSyasyu.SyaSyuRen
LEFT JOIN VPM_Eigyos UketukeEigyosho1 ON Yoyakusho.UkeEigCdSeq = UketukeEigyosho1.EigyoCdSeq
LEFT JOIN VPM_Tokisk TokuisakiMaster ON Yoyakusho.TokuiSeq = TokuisakiMaster.TokuiSeq
LEFT JOIN VPM_TokiSt TokuisakiSitenMaster ON Yoyakusho.TokuiSeq = TokuisakiSitenMaster.TokuiSeq
        AND Yoyakusho.SitenCdSeq = TokuisakiSitenMaster.SitenCdSeq
LEFT JOIN VPM_SyaSyu SyaSyu ON YoyakuSyasyu.SyaSyuCdSeq = SyaSyu.SyaSyuCdSeq
LEFT JOIN TKD_Yousha Yousha ON Haisha.UkeNo = Yousha.UkeNo
        AND Haisha.UnkRen = Yousha.UnkRen
        AND Haisha.YouTblSeq = Yousha.YouTblSeq
        AND YouSha.SiyoKbn = 1
LEFT JOIN VPM_SyaRyo SyaRyo ON Haisha.HaiSSryCdSeq = SyaRyo.SyaRyoCdSeq
LEFT JOIN VPM_SyaSyu SyaSyu1 ON SyaRyo.SyaSyuCdSeq = SyaSyu1.SyaSyuCdSeq
        AND SyaSyu1.TenantCdSeq = @TenantCdSeq
WHERE (
        (Haisha.SyuKoYmd >= @StaYmd AND Haisha.SyuKoYmd <= @EndYmd)
        OR (Haisha.KikYmd >= @StaYmd AND Haisha.KikYmd <= @EndYmd)
        OR (Haisha.SyuKoYmd <= @StaYmd AND Haisha.KikYmd >= @EndYmd)
            )
        AND Yoyakusho.TenantCdSeq = @TenantCdSeq
        AND Yoyakusho.YoyaSyu = 1
        AND Yoyakusho.SiyoKbn = 1
        AND Haisha.SiyoKbn = 1
        AND YoyakuSyasyu.SiyoKbn = 1

SET	@ROWCOUNT	=	@@ROWCOUNT
END
