-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dCrewDataAcquisitions_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get CrewDataAcquisition List
-- Date			:   2020/12/11
-- Author		:   P.M.Nhat
-- Description	:   Get CrewDataAcquisition list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dCrewDataAcquisitions_R
	-- Add the parameters for the stored procedure here
	@CompanyCdSeq int,
	@UnkYmd varchar(8)
AS
BEGIN
	SELECT
    		eVPM_Syain.SyainCdSeq AS SyainCdSeq --社員コードSEQ
    		,ISNULL(eVPM_Syain.SyainCd, ' ') AS SyainCd --社員コード
    		,ISNULL(eVPM_Syain.SyainNm, ' ') AS SyainNm --社員名
    		,eVPM_KyoSHe.TenkoNo AS TenkoNo --点呼順
    		,ISNULL(VPM_Syokum.SyokumuCdSeq, 0) AS SyokumuCdSeq --職務コードSEQ
    		,ISNULL(VPM_Syokum.SyokumuNm, ' ') AS SyokumuNm --職務名
    		,ISNULL(VPM_Syokum.SyokumuKbn, 0) AS SyokumuKbn --職務区分
    		,ISNULL(VPM_Syokum.JigyoKbn, 0) AS JigyoKbn --事業区分
    		,ISNULL(VPM_CodeKb.CodeKbnNm, ' ') AS SyokumuKbn_CodeKbnNm --職務区分名
    		,VPM_Eigyos.EigyoCdSeq AS EigyoCdSeq --営業所コードSEQ
    		,ISNULL(VPM_Eigyos.EigyoCd, 0) AS EigyoCd --営業所コード
    		,ISNULL(VPM_Eigyos.EigyoNm, ' ') AS EigyoNm --営業所名
    		,ISNULL(VPM_Eigyos.CompanyCdSeq, 0) AS CompanyCdSeq --会社コードSEQ
	FROM
    		VPM_KyoSHe AS eVPM_KyoSHe
    		JOIN VPM_Syain AS eVPM_Syain ON eVPM_KyoSHe.SyainCdSeq = eVPM_Syain.SyainCdSeq
    		LEFT JOIN VPM_Syokum ON eVPM_KyoSHe.SyokumuCdSeq = VPM_Syokum.SyokumuCdSeq
    		LEFT JOIN VPM_CodeKb ON VPM_Syokum.SyokumuKbn = VPM_CodeKb.CodeKbn
    		AND VPM_CodeKb.CodeSyu = 'SYOKUMUKBN'
    		AND VPM_CodeKb.TenantCdSeq = (
        		SELECT
            		CASE
                		WHEN COUNT(*) > 0 THEN 1
                		ELSE 0
            		END AS TenantCdSeq
        		FROM
            		VPM_CodeSy
        		WHERE
            		CodeSyu = 'SYOKUMUKBN'
            		AND KanriKbn <> 1
    		)
    		LEFT JOIN VPM_Eigyos ON eVPM_KyoSHe.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
	WHERE
    		VPM_Eigyos.CompanyCdSeq = @CompanyCdSeq
    		AND @UnkYmd BETWEEN eVPM_KyoSHe.StaYmd
    		AND eVPM_KyoSHe.EndYmd
	ORDER BY
    		eVPM_Syain.SyainCd
END
GO
