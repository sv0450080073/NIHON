USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PP_mJourneys_R]    Script Date: 08/17/2020 11:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dJourneys_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get journeys data List
-- Date			:   2020/08/18
-- Author		:   N.N.T.AN
-- Description	:   Get journeys data list with conditions
------------------------------------------------------------
CREATE or ALTER   PROCEDURE [dbo].[PK_dJourneys_R]
		(
		--Parameter
			@GroupId			INT = NULL,					--Group id
			@FromDate			VARCHAR(50),				--From Date
			@ToDate				VARCHAR(50),				--To Date
		--Output
			@ROWCOUNT			INTEGER OUTPUT				-- ROWCOUNT
		)
AS
	-- Processing
BEGIN
	SELECT																																	
    TKD_Unkobi.DanTaNm AS Title -- タイトル																																	
    ,TKD_Haisha.SyuKoYmd -- 出庫年月日（開始年月日）																																	
    ,TKD_Haisha.SyuKoTime -- 出庫時間（開始時間）																																	
    ,TKD_Haisha.KikYmd -- 帰庫年月日（終了年月日）																																	
    ,TKD_Haisha.KikTime -- 帰庫時間（終了時間）																																	
    ,TKD_Haiin.SyainCdSeq -- 社員SEQ																																	
    ,VPM_Syain.SyainNm -- 社員名																																	
    ,TKD_Haisha.HaiSNm -- 配車地																																	
    ,TKD_Haisha.HaiSTime -- 配車時間																																	
    ,TKD_Haisha.TouNm -- 到着地																																	
    ,TKD_Haisha.TouChTime -- 到着時間																																	
    ,TKD_Unkobi.DanTaNm -- 団体名																																	
    ,TKD_Haisha.IkNm -- 行先名																																	
FROM																																	
    TKD_Haiin --配車情報取得																																	
    JOIN TKD_Haisha ON TKD_Haisha.UkeNo = TKD_Haiin.UkeNo																																	
    AND TKD_Haisha.UnkRen = TKD_Haiin.UnkRen																																	
    AND TKD_Haisha.TeiDanNo = TKD_Haiin.TeiDanNo																																	
    AND TKD_Haisha.BunkRen = TKD_Haiin.BunkRen																																	
    AND TKD_Haisha.SiyoKbn = 1 --社員情報取得																																	
    JOIN VPM_Syain ON VPM_Syain.SyainCdSeq = TKD_Haiin.SyainCdSeq --営業所取得																																	
    JOIN VPM_KyoSHe ON VPM_KyoSHe.SyainCdSeq = TKD_Haiin.SyainCdSeq																																	
    AND (																																	
        CAST(VPM_KyoSHe.StaYmd AS DATE) <= CAST(GETDATE() AS DATE)																																	
    )																																	
    AND (																																	
        CAST(VPM_KyoSHe.EndYmd AS DATE) >= CAST(GETDATE() AS DATE)																																	
    ) --運行日取得																																	
    JOIN TKD_Unkobi ON TKD_Unkobi.UkeNo = TKD_Haisha.UkeNo																																	
    AND TKD_Unkobi.UnkRen = TKD_Haisha.UnkRen																																	
    AND TKD_Unkobi.SiyoKbn = 1																																	
WHERE																																	
    TKD_Haiin.SiyoKbn = 1							
	AND TKD_Haisha.TouYmd >= @FromDate																																		
    AND TKD_Haisha.SyuKoYmd <= @ToDate
    AND VPM_KyoSHe.EigyoCdSeq = @GroupId																												
																									

		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN