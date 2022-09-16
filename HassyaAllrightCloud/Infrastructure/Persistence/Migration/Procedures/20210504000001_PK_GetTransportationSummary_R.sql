USE [HOC_Kashikiri];
GO

/****** Object:  StoredProcedure [dbo].[PK_GetTransportationSummary_R]    Script Date: 2020/07/28 9:27:26 ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: GetTransportationSummary
-- SP-ID				: [PK_GetTransportationSummary_R]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/07/28
-- Description			: Get Transportation Summary
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_GetTransportationSummary_R] 
		@SyoriYm     CHAR(6), -- yyyyMM				
        @TenantCdSeq INT,
        @CompnyCdSeq INT,
        @StrEigyoCd INT,
        @EndEigyoCd INT
AS
    BEGIN
        SELECT DISTINCT 
               jh.SyoriYm, 
               c.CompanyCd, 
               C.CompanyNm, 
               e.EigyoCd, 
               e.EigyoNm, 
               jh.UpdYmd, 
               jh.UpdTime
        FROM TKD_JitHou jh
             INNER JOIN
        (
            SELECT SyoriYm, 
                   EigyoCdSeq, 
                   MAX(UpdYmd) AS MaxUpdYmd, 
                   MAX(UpdTime) AS MaxUpdTime
            FROM TKD_JitHou
            GROUP BY SyoriYm, 
                     EigyoCdSeq
        ) jlast ON jlast.SyoriYm = jh.SyoriYm
                   AND jlast.EigyoCdSeq = jh.EigyoCdSeq
                   AND jlast.MaxUpdYmd = jh.UpdYmd
                   AND jlast.MaxUpdTime = jh.UpdTime
             LEFT JOIN VPM_Eigyos e ON jh.EigyoCdSeq = e.EigyoCdSeq
                                       AND e.SiyoKbn = 1
             LEFT JOIN VPM_Compny c ON e.CompanyCdSeq = c.CompanyCdSeq
                                       AND c.SiyoKbn = 1
             LEFT JOIN VPM_Tenant AS t ON c.TenantCdSeq = t.TenantCdSeq
                                          AND t.SiyoKbn = 1
        WHERE   jh.SyoriYm = @SyoriYm 
                AND t.TenantCdSeq = @TenantCdSeq 
                AND c.CompanyCdSeq = @CompnyCdSeq 
                AND (@StrEigyoCd = 0 OR e.EigyoCd >= @StrEigyoCd) 
                AND (@EndEigyoCd = 0 OR e.EigyoCd <= @EndEigyoCd)
        ORDER BY jh.SyoriYm DESC, 
                 c.CompanyCd, 
                 e.EigyoCd;
    END;
GO