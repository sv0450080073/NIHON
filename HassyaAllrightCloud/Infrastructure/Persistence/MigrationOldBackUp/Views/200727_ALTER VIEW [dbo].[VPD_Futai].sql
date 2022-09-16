USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Futai]    Script Date: 2020/08/13 13:54:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: GetTransportationSummary
-- View-ID				: [VPM_Futai]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/08/13
-- Description			: Get TPM_Futai from HOC_Master
-- =============================================

CREATE OR ALTER VIEW [dbo].[VPM_Futai]
AS

select 
	[FutaiCdSeq]
      ,[TenantCdSeq]
	  ,[FutaiCd]
      ,[FutaiNm]
      ,[RyakuNm]
      ,[FutGuiKbn]
      ,[ZeiHyoKbn]
      ,[IT_SehCode]
      ,[IT_SesCode]
      ,[IT_SeuCode]
      ,[SiyoKbn]
      ,[UpdYmd]
      ,[UpdTime]
      ,[UpdSyainCd]
      ,[UpdPrgID]
from HOC_Master..TPM_Futai

GO


