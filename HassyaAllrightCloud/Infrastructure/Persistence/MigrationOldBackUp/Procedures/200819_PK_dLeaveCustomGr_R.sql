USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dLeaveCustomGr_R]    Script Date: 08/24/2020 11:09:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   Kashikiri
-- SP-ID		:   PK_dLeaveCustomGr_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Leave day custom group List
-- Date			:   2020/08/17
-- Author		:   N.N.T.AN
-- Description	:   Get leave day custom group list with conditions
------------------------------------------------------------
CREATE or ALTER    PROCEDURE [dbo].[PK_dLeaveCustomGr_R]
		(
		--Parameter
            @TenantCdSeq		INT = NULL,					--Tenant id
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
			    TKD_Kikyuj.KinKyuTblCdSeq																																					
			    ,VPM_CodeKb.CodeKbnNm AS TukiLabel --付きラベル																																					
			    ,VPM_KinKyu.KinKyuNm AS Title --タイトル																																					
			    ,TKD_Kikyuj.KinKyuSYmd --勤務休日開始年月日																																					
			    ,TKD_Kikyuj.KinKyuSTime --勤務休日開始時間																																					
			    ,TKD_Kikyuj.KinKyuEYmd --勤務休日終了年月日																																					
			    ,TKD_Kikyuj.KinKyuETime --勤務休日終了時間																																					
			    ,TKD_Kikyuj.BikoNm --備考																																					
			    ,VPM_Syain.SyainCd --社員コード																																					
			    ,VPM_Syain.SyainNm --社員名																																					
			    ,VPM_CodeKb.CodeKbnNm --勤務休日種別区分																																					
			    ,VPM_KinKyu.KinKyuNm --勤務休日種別名		
				,VPM_KinKyu.KinKyuKbn
			FROM																																					
			    TKD_Kikyuj 																																					
			    JOIN VPM_Syain ON VPM_Syain.SyainCdSeq = TKD_Kikyuj.SyainCdSeq 																																					
			    JOIN TKD_SchCusGrpMem ON TKD_SchCusGrpMem.SyainCdSeq = TKD_Kikyuj.SyainCdSeq 																																					
			    LEFT JOIN VPM_KinKyu ON VPM_KinKyu.KinKyuCdSeq = TKD_Kikyuj.KinKyuCdSeq																																					
			    AND VPM_KinKyu.SiyoKbn = 1 																																					
			    LEFT JOIN VPM_CodeKb ON VPM_CodeKb.CodeKbn = VPM_KinKyu.KinKyuKbn																																					
			    AND VPM_CodeKb.CodeSyu = 'KINKYUKBN'																																					
			    AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq																																				
			    AND VPM_CodeKb.SiyoKbn = 1																																					
			WHERE																																					
			    TKD_Kikyuj.SiyoKbn = 1																																					
			    AND TKD_Kikyuj.KinKyuEYmd >= @FromDate																																				
			    AND TKD_Kikyuj.KinKyuSYmd <= @ToDate																																				
			    AND TKD_SchCusGrpMem.CusGrpSeq = @GroupId	
				AND TKD_Kikyuj.KinKyuCdSeq > 0
																																								


		SET	@ROWCOUNT	=	@@ROWCOUNT
END
RETURN