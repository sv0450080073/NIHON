﻿USE [HOC_Master]
GO

DELETE FROM TPM_ReportTemplate
GO

INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (1, 1, 'デフォルト', 'images/report/tatekaemesaisho_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (1, 2, 'カスタム1', 'images/report/tatekaemesaisho_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (2, 1, 'デフォルト', 'images/report/vehicle_daily_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (2, 2, 'カスタム1', 'images/report/vehicle_daily_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (3, 1, 'デフォルト', 'images/report/bill_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (3, 2, 'カスタム1', 'images/report/bill_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (4, 1, 'デフォルト', 'images/report/bill_list_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (4, 2, 'カスタム1', 'images/report/bill_list_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (5, 1, 'デフォルト', 'images/report/bill_detail_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (5, 2, 'カスタム1', 'images/report/bill_detail_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (6, 1, 'デフォルト', 'images/report/transportation_daily_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (6, 2, 'カスタム1', 'images/report/transportation_daily_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (7, 1, 'デフォルト', 'images/report/automobile_survey_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (7, 2, 'カスタム1', 'images/report/automobile_survey_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (8, 1, 'デフォルト', 'images/report/unpaid_list_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (8, 2, 'カスタム1', 'images/report/unpaid_list_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (9, 1, 'デフォルト', 'images/report/unpaid_detail_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (9, 2, 'カスタム1', 'images/report/unpaid_detail_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (10, 1, 'デフォルト', 'images/report/deposit_list_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (10, 2, 'カスタム1', 'images/report/deposit_list_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (11, 1, 'デフォルト', 'images/report/deposit_detail_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (11, 2, 'カスタム1', 'images/report/deposit_detail_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (12, 1, 'デフォルト', 'images/report/hikiukesho_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (12, 2, 'カスタム1', 'images/report/hikiukesho_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (13, 1, 'デフォルト', 'images/report/attendance_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (13, 2, 'カスタム1', 'images/report/attendance_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (14, 1, 'デフォルト', 'images/report/general_transportation_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (14, 2, 'カスタム1', 'images/report/general_transportation_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (15, 1, 'デフォルト', 'images/report/sales_daily_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (15, 2, 'カスタム1', 'images/report/sales_daily_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (16, 1, 'デフォルト', 'images/report/sales_summary_daily_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (16, 2, 'カスタム1', 'images/report/sales_summary_daily_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (17, 1, 'デフォルト', 'images/report/annual_transportation_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (17, 2, 'カスタム1', 'images/report/annual_transportation_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (18, 1, 'デフォルト', 'images/report/monthly_transportation_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (18, 2, 'カスタム1', 'images/report/monthly_transportation_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (19, 1, 'デフォルト', 'images/report/receipt_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (19, 2, 'カスタム1', 'images/report/receipt_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (20, 1, 'デフォルト', 'images/report/supermenu_reservation_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (20, 2, 'カスタム1', 'images/report/supermenu_reservation_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (21, 1, 'デフォルト', 'images/report/supermenu_vehicle_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (21, 2, 'カスタム1', 'images/report/supermenu_vehicle_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (22, 1, 'デフォルト', 'images/report/bills_checklist_default.png', 1, N'20210420', '000000', 1, N'KOBO      ')
INSERT INTO [dbo].[TPM_ReportTemplate]　([ReportId], [TemplateId], [TemplateNm], [ImgPath], [SiyoKbn], [UpdYmd], [UpdTime], [UpdSyainCd], [UpdPrgID]) VALUES (22, 2, 'カスタム1', 'images/report/bills_checklist_custom1.png', 1, N'20210420', '000000', 1, N'KOBO      ')