USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Gyosya]    Script Date: 5/25/2021 3:09:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VPM_Gyosya]
AS
SELECT GyosyaCdSeq, GyosyaCd, GyosyaNm, GyosyaKbn, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID, TenantCdSeq
FROM   HOC_Master.dbo.TPM_Gyosya
GO