USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_RyokinSplit]    Script Date: 2020/09/10 14:23:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: ETCImportConditionSetting
-- View-ID				: [VPM_RyokinSplit]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/09/10
-- Description			: Get TPM_RyokinSplit from HOC_Master
-- =============================================
CREATE OR ALTER VIEW [dbo].[VPM_RyokinSplit]
AS
select 
	 *
from HOC_Master..TPM_RyokinSplit

GO


