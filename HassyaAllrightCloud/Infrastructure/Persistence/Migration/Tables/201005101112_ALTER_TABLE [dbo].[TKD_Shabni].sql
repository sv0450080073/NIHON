USE [HOC_Kashikiri]
GO

/****** Object:  Table [dbo].[TKD_Shabni]    Script Date: 2020/10/05 14:50:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: HassyaAlrightCloud
-- SP-ID				: [TKD_Shabni]
-- DB-Name				: HOC_Kashikiri
-- Author				: N.T.HIEU
-- Create date			: 2020/10/05
-- Description			: Create Table TKD_Shabni
-- =============================================

USE HOC_Kashikiri
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE TKD_Shabni		
		
ADD 		
	 	InspectionTime char(4) NOT NULL DEFAULT '0200'
	 ,	FareFeeMaxAmount int NOT NULL DEFAULT 0
	 ,	FareFeeMinAmount int NOT NULL DEFAULT 0

GO										
