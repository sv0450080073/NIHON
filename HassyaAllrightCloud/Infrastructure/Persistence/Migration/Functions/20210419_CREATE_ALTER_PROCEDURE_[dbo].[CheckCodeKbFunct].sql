USE [HOC_Kashikiri]
GO

/****** Object:  UserDefinedFunction [dbo].[CheckCodeKbFunct]    Script Date: 2021/04/27 9:47:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER FUNCTION [dbo].[CheckCodeKbFunct] (@input varchar(20),@tenantCdSeq int)
RETURNS VARCHAR(1)
AS BEGIN
	declare @Var int
	declare @KanriKbn int
	set @KanriKbn=(select KanriKbn from VPM_CodeSy where CodeSyu=@input)
	if(@KanriKbn=1)
	BEGIN
	   set @Var = 0;
	END
    else IF EXISTS (SELECT * FROM VPM_CodeKb WHERE TenantCdSeq = @tenantCdSeq and CodeSyu=@input)
	BEGIN
		set @Var = @tenantCdSeq
	END
	ELSE
	BEGIN
	   set @Var = 0;
	END
	return CAST(@Var as varchar(1))
END
GO


