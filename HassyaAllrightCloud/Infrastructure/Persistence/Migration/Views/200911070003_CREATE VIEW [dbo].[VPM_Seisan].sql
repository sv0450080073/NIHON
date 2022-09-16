USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Seisan]    Script Date: 2020/09/11 14:23:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: ETCImportConditionSetting
-- View-ID				: [VPM_Seisan]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/08/13
-- Description			: Get TPM_Seisan from HOC_Master
-- =============================================
CREATE OR ALTER VIEW [dbo].[VPM_Seisan]
AS
select 
	 *
from HOC_Master..TPM_Seisan

GO


