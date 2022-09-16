USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Haichi]    Script Date: 5/17/2021 2:26:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER VIEW [dbo].[VPM_Haichi]
AS

select 
       [TenantCdSeq]
      ,[BunruiCdSeq]
      ,[HaiSCdSeq]
      ,[HaiSCd]
      ,[HaiSNm]
      ,[RyakuNm]
      ,[ZipCd]
      ,[Jyus1]
      ,[Jyus2]
      ,[TelNo]
      ,[FaxNo]
      ,[HaiSTanNm]
      ,[HaiSKigou]
      ,[SiyoKbn]
      ,[UpdYmd]
      ,[UpdTime]
      ,[UpdSyainCd]
      ,[UpdPrgID]
from HOC_Master..TPM_Haichi

GO


