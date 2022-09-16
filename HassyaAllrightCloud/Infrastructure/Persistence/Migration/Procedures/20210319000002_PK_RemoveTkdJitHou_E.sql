USE [HOC_Kashikiri];
GO

/****** Object:  StoredProcedure [dbo].[PK_RemoveTkdJitHou_E]    Script Date: 2020/07/28 9:37:26 ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: GetTransportationSummary
-- SP-ID				: [PK_RemoveTkdJitHou_E]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/07/28
-- Description			: Remove TkdJitHou in @SyoriYm of @CompanyCdSeq has EigyoCd between @StrEigyoCd and @EndEigyoCd
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[PK_RemoveTkdJitHou_E] @SyoriYm      CHAR(6), -- yyyyMM
                                              @CompanyCdSeq INT, 
                                              @StrEigyoCd   CHAR(5), 
                                              @EndEigyoCd   CHAR(5),
                                              @TenantCdSeq  INT
AS
    BEGIN
        DELETE FROM TKD_JitHou
        FROM TKD_JitHou
             LEFT JOIN VPM_Eigyos ON TKD_JitHou.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
             LEFT JOIN VPM_Compny ON VPM_Compny.CompanyCdSeq = VPM_Eigyos.CompanyCdSeq
        WHERE TKD_JitHou.SyoriYm = @SyoriYm
              AND VPM_Compny.CompanyCdSeq = @CompanyCdSeq
              AND VPM_Eigyos.EigyoCd >= CASE TRIM(@StrEigyoCd)
                                            WHEN ''
                                            THEN VPM_Eigyos.EigyoCd
                                            ELSE @StrEigyoCd
                                        END
              AND VPM_Eigyos.EigyoCd <= CASE TRIM(@EndEigyoCd)
                                            WHEN ''
                                            THEN VPM_Eigyos.EigyoCd
                                            ELSE @EndEigyoCd
                                        END
              AND VPM_Compny.TenantCdSeq = @TenantCdSeq
    END;
GO