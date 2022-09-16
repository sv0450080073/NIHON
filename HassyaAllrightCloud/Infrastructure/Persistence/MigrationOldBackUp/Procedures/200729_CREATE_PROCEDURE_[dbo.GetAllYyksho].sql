USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[GetAllYyksho]    Script Date: 2020/07/29 15:42:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllYyksho] @max int, @min int
AS
SELECT UkeNo, UkeCd FROM TKD_Yyksho where UkeCd < @max and UkeCd > @min
;
GO

