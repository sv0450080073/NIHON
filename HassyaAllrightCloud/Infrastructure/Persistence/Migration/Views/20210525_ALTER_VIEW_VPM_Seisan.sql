USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Haichi]    Script Date: 17/05/2021 4:04:26 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create Or Alter VIEW [dbo].[VPM_Seisan]
AS

select 
 *
from HOC_Master..TPM_Seisan

GO
