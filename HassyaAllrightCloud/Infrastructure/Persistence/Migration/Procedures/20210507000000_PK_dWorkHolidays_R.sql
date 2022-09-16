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
-- SP-ID		:   PK_dWorkHolidays_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get WorkHoliday List
-- Date			:   2020/12/11
-- Author		:   P.M.Nhat
-- Description	:   Get WorkHoliday list with conditions
-- =============================================
CREATE OR ALTER PROCEDURE PK_dWorkHolidays_R
	-- Add the parameters for the stored procedure here
	@CompanyCdSeq int,
	@UnkYmd varchar(8)
AS
BEGIN
	SELECT
    		eTKD_Kikyuj.KinKyuTblCdSeq AS KinKyuTblCdSeq --勤務休日テーブルＳＥＱ
    		,eTKD_Koban.SyukinTime AS SyukinTime --出勤時間
    		,eTKD_Koban.TaiknTime AS TaiknTime --退勤時間
    		,ISNULL(eVPM_KinKyu.KinKyuCd, '') AS KinKyuCd --勤務休日種別コード
    		,ISNULL(eVPM_KinKyu.KinKyuNm, '') AS KinKyuNm --勤務休日種別名
    		,ISNULL(eVPM_KinKyu.RyakuNm, '') AS RyakuNm --勤務休日種別略名
    		,ISNULL(eVPM_KinKyu.KinKyuKbn, 0) AS KinKyuKbn --勤務休日種別区分
    		,ISNULL(eVPM_KinKyu.ColKinKyu, '') AS ColKinKyu --色選択（勤務休日種別）
    		,eTKD_Kikyuj.SyainCdSeq AS SyainCdSeq --社員コードSEQ
    		,ISNULL(eVPM_Syain.SyainCd, '') AS SyainCd --社員コード
    		,ISNULL(eVPM_Syain.SyainNm, '') AS SyainNm --社員名
    		,eTKD_Koban.UpdYmd AS UpdYmd
    		,eTKD_Koban.UpdTime AS UpdTime
	FROM
    		TKD_Kikyuj AS eTKD_Kikyuj
    		JOIN TKD_Koban AS eTKD_Koban ON eTKD_Koban.KinKyuTblCdSeq = eTKD_Kikyuj.KinKyuTblCdSeq
    		LEFT JOIN VPM_KinKyu AS eVPM_KinKyu ON eVPM_KinKyu.KinKyuCdSeq = eTKD_Kikyuj.KinKyuCdSeq
    		LEFT JOIN VPM_Syain AS eVPM_Syain ON eVPM_Syain.SyainCdSeq = eTKD_Kikyuj.SyainCdSeq
    		LEFT JOIN VPM_KyoSHe AS eVPM_KyoSHe ON eVPM_KyoSHe.SyainCdSeq = eTKD_Kikyuj.SyainCdSeq
    		AND eTKD_Kikyuj.KinKyuSYmd BETWEEN eVPM_KyoSHe.StaYmd
    		AND eVPM_KyoSHe.EndYmd
    		LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_Eigyos.EigyoCdSeq = eVPM_KyoSHe.EigyoCdSeq
	WHERE
    		eVPM_Eigyos.CompanyCdSeq = @CompanyCdSeq
    		AND eTKD_Koban.UnkYmd = @UnkYmd
	ORDER BY
    		eTKD_Kikyuj.KinKyuSYmd,
    		eTKD_Kikyuj.KinKyuSTime
END
GO
