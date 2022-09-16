USE [HOC_Master]
GO

INSERT INTO [dbo].[TPM_ReportSetting]
           ([TenantCdSeq]
           ,[ReportId]
           ,[ReportNm]
           ,[CurrentTemplateId]
           ,[SiyoKbn]
           ,[UpdYmd]
           ,[UpdTime]
           ,[UpdSyainCd]
           ,[UpdPrgID])
     VALUES (0, 1, '立替明細書', 1, 1, N'20201013', N'000000', 1, N'KOBO      ')

GO