USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Bin]    Script Date: 5/17/2021 2:26:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER VIEW [dbo].[VPM_Bin]
AS

select 
	   [TenantCdSeq]
	  ,[KoukCdSeq]
      ,[BinCdSeq]
      ,[BinCd]
      ,[BinNm]
      ,[SyuPaTime]
      ,[TouChTime]
      ,[SiyoStaYmd]
      ,[SiyoEndYmd]
      ,[UpdYmd]
      ,[UpdTime]
      ,[UpdSyainCd]
      ,[UpdPrgID]

from HOC_Master..TPM_Bin

GO


