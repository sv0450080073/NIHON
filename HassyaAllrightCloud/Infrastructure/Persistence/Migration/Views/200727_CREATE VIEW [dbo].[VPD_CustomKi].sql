--USE [HOC_Kashikiri]
--GO

/****** Object:  View [dbo].[TPD_CustomKi]    Script Date: 2020/08/13 14:23:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: GetTransportationSummary
-- View-ID				: [TPD_CustomKi]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/08/13
-- Description			: Get TPD_CustomKi from HOC_Master
-- =============================================
CREATE OR ALTER VIEW [dbo].[VPD_CustomKi]
AS
select 
	 *
from HOC_Master..TPD_CustomKi

GO


