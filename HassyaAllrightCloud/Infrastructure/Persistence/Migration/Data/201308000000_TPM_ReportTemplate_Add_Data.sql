USE [HOC_MASTER]
GO

INSERT INTO [dbo].[TPM_ReportTemplate]
           ([TenantCdSeq]
           ,[ReportId]
           ,[TemplateId]
           ,[TemplateNm]
           ,[ImgPath]
           ,[SiyoKbn]
           ,[UpdYmd]
           ,[UpdTime]
           ,[UpdSyainCd]
           ,[UpdPrgID])
     VALUES (0, 1 , 1 , 'デフォルト' , 'images/report/tatekaemesaisho_default.png' , 1 , N'20201013'  , N'000000' , 1 , N'KOBO      ')

INSERT INTO [dbo].[TPM_ReportTemplate]
           ([TenantCdSeq]
           ,[ReportId]
           ,[TemplateId]
           ,[TemplateNm]
           ,[ImgPath]
           ,[SiyoKbn]
           ,[UpdYmd]
           ,[UpdTime]
           ,[UpdSyainCd]
           ,[UpdPrgID])
     VALUES (0, 1 , 2 , 'カスタム1' , 'images/report/tatekaemesaisho_custom1.png' , 1 , N'20201013'  , N'000000' , 1 , N'KOBO      ')
GO