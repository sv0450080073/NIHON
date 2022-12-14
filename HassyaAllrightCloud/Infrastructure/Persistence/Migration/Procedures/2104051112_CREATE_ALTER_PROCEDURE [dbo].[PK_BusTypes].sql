USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_BusTypes]    Script Date: 05/04/2021 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_BusTypes
-- DB-Name		:   HOC_Kashikiri
-- Name			:   PK_BusTypes
-- Date			:   2021/05/04
-- Author		:   N.T.HIEU
-- Description	:   Get Data Receipt Detail And No Detail
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_BusTypes]
    (
     -- Parameter
	    @TenantCdSeq                 VARCHAR(4)  
    )
AS 
	BEGIN
	    SELECT VPM_SyaSyu.SyaSyuCdSeq,
         VPM_SyaSyu.KataKbn,
         ISNULL(VPM_CodeKb01.RyakuNm, VPM_CodeKb02.RyakuNm) AS KataKbnNm,
         VPM_SyaSyu.SyaSyuNm
        FROM VPM_SyaSyu
        LEFT JOIN VPM_CodeKb VPM_CodeKb01
             ON CAST(VPM_SyaSyu.KataKbn AS INT) = VPM_CodeKb01.CodeKbn
             AND VPM_CodeKb01.CodeSyu = 'KATAKBN'
             AND VPM_CodeKb01.TenantCdSeq = @TenantCdSeq
        LEFT JOIN VPM_CodeKb VPM_CodeKb02
             ON CAST(VPM_SyaSyu.KataKbn AS INT) = VPM_CodeKb02.CodeKbn
             AND VPM_CodeKb02.CodeSyu = 'KATAKBN'
             AND VPM_CodeKb02.TenantCdSeq = 0
             AND VPM_CodeKb02.SiyoKbn = 1
        WHERE VPM_SyaSyu.SiyoKbn = 1
        AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq
        ORDER BY VPM_SyaSyu.SyaSyuCd																				
    END
GO																													

