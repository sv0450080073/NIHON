USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetCodeKbnSetting_R]    Script Date: 05/27/2021 2:57:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tra Nguyen Lam
-- Create date: 2021/05/28
-- Description:	Get regulation setting data 
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[PK_dGetCodeKbnSetting_R]
	@TenantCdSeq	int
AS
BEGIN

SELECT VPM_CodeKb.CodeKbnSeq,
          VPM_CodeKb.CodeKbn,
          VPM_CodeKb.CodeSyu,
          VPM_CodeKb.CodeKbnNm,
          VPM_CodeKb.RyakuNm
     FROM VPM_CodeKb
     LEFT JOIN VPM_CodeSy
          ON VPM_CodeKb.CodeSyu = VPM_CodeSy.CodeSyu
     WHERE VPM_CodeKb.CodeSyu IN ('URIKBN', 'SYOHIHASU', 'TESUHASU', 'HOUKOKBN', 'HOUZEIKBN', 'JKARIKBN', 'AUTKARJYUN', 'JKBUNPAT', 'SYAIREPAT', 'JYMACHKKBN', 'URIHENKBN', 'URIMDKBN', 'URIZEROCHK', 'CANKBN', 'CANMDKBN', 'CANWAITKBN', 'CANJIDOKBN', 'YOUTESUKBN', 'YOUSAGAKBN', 'SYAUNTKBN', 'ZASYUKBN', 'FUTSF1FLG', 'FUTSF2FLG', 'FUTSF3FLG', 'FUTSF4FLG', 'SOKOJUNKBN', 'UNTZEIKBN', 'TUMZEIKBN', 'GETSYOKBN', 'SEIKRKSKBN', 'DAYSYOKBN', 'KOUYOUSET', 'AUTKOUKBN', 'SENYOUDEFFLG', 'SENMIKDEFFLG', 'COPYFLG', 'SEIGENFLG', 'HOUOUTKBN', 'MEISHYKBN', 'SENMJPTNKBN', 'SIYOKBN', 'ETCKINKBN', 'SEIMUKI')
          AND ((VPM_CodeSy.KanriKbn = 1 AND VPM_CodeKb.TenantCdSeq = 0) OR (VPM_CodeSy.KanriKbn <> 1 AND VPM_CodeKb.TenantCdSeq = @TenantCdSeq)) -- ログインユーザーのTenantCdSeq
          AND VPM_CodeKb.SiyoKbn = 1
END
