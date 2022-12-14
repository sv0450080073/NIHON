USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_CaculateTime_R]    Script Date: 10/24/2020 10:20:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_CaculateTime_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Caculate Time
-- Date			:   2020/10/24
-- Author		:   N.T.HIEU
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].PK_CaculateTime_R
    (
     -- Parameter
	     @Time1		VARCHAR(4)
	,	 @Time2		VARCHAR(4)
	 
        -- Output
	    ,  @ROWCOUNT	          INTEGER OUTPUT	   -- 処理件数
    )
AS 
	DECLARE @strSql		VARCHAR(MAX)
    -- Processing
	BEGIN
		SELECT  dbo.FP_CalCharTime(@Time1,@Time2) AS TimeSum
    SET	@ROWCOUNT	=	@@ROWCOUNT
	END
RETURN																													
