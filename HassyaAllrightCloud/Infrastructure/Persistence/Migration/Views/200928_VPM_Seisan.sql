--USE [HOC_Kashikiri]
--GO

/****** Object:  View [dbo].[VPM_Seisan]    Script Date: 2020/09/21 16:18:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER VIEW [dbo].[VPM_Seisan]
AS
select 
	 [SeisanCdSeq],
	 [SeisanCd],
	 [SeisanNm],
	 [RyakuNm],
	 [SeisanKbn],
	 [SiyoKbn],
	 [UpdYmd],
	 [UpdTime],
	 [UpdSyainCd],
	 [UpdPrgID]
from HOC_Master..TPM_Seisan

GO


