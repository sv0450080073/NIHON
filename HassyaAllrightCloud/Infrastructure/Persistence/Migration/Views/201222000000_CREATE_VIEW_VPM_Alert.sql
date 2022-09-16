USE [HOC_Kashikiri]
GO

CREATE OR ALTER VIEW dbo.VPM_Alert AS
SELECT [TenantCdSeq]
      ,[AlertCdSeq]
      ,[AlertKbn]
      ,[AlertCd]
      ,[AlertNm]
      ,[AlertContent]
      ,[DefaultVal]
      ,[DefaultTimeline]
      ,[DefaultZengo]
      ,[DefaultDisplayKbn]
      ,[SiyoKbn]
      ,[UpdYmd]
      ,[UpdTime]
      ,[UpdSyainCd]
      ,[UpdPrgID]
  FROM [HOC_Master].[dbo].[TPM_Alert]
GO

CREATE OR ALTER VIEW dbo.VPM_AlertSet AS
SELECT [SyainCdSeq]
      ,[AlertCd]
      ,[DisplayKbn]
      ,[UpdYmd]
      ,[UpdTime]
      ,[UpdSyainCd]
      ,[UpdPrgID]
  FROM [HOC_Master].[dbo].[TPM_AlertSet]
GO