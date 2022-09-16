USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Tenant]    Script Date: 3/23/2021 1:05:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create or Alter VIEW [dbo].[VPM_Tenant]
AS

select 
[TenantCdSeq]
      ,[TenantCd]
      ,[TenantCompanyName]
      ,[MailAddress]
      ,[Address1]
      ,[Address2]
      ,[TellNumber]
      ,[FaxNumber]
      ,[SalesRepresentativeCdSeq]
      ,[SystemRepresentative1CdSeq]
      ,[SystemRepresentative2CdSeq]
      ,[Customer1]
      ,[Customer2]
      ,[Remarks]
      ,[StaYmd]
      ,[EndYmd]
      ,[SiyoKbn]
      ,[UpdYmd]
      ,[UpdTime]
      ,[UpdSyainCd]
      ,[UpdPrgID]

from HOC_Master..TPM_Tenant

GO


