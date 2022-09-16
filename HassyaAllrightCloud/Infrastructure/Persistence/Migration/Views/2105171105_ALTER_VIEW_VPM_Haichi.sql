USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Haichi]    Script Date: 17/05/2021 4:04:26 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create Or Alter VIEW [dbo].[VPM_Haichi]
AS

select 
 [BunruiCdSeq]
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
	  ,[TenantCdSeq]
from HOC_Master..TPM_Haichi

GO
