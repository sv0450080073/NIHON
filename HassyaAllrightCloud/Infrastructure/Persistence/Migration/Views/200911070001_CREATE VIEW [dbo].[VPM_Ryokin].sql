USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Ryokin]    Script Date: 2020/08/13 14:23:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: ETCImportConditionSetting
-- View-ID				: [VPM_Ryokin]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/08/13
-- Description			: Get TPM_Ryokin from HOC_Master
-- =============================================
CREATE OR ALTER VIEW [dbo].[VPM_Ryokin]
AS
select 
	 *
from HOC_Master..TPM_Ryokin

GO


