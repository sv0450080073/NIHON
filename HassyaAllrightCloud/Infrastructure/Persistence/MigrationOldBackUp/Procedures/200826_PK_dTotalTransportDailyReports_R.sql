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
-- SP-ID		:   PK_dTransportDailyReports_R
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Transport Daily Report List
-- Date			:   2020/08/26
-- Author		:   P.M.Nhat
-- Description	:   Get transport daily report list with conditions
-- =============================================
CREATE PROCEDURE [dbo].[PK_dTotalTransportDailyReport_R] 
	-- Add the parameters for the stored procedure here
	@OutStei		int,			-- 出力区分				
	@UnkYmd			varchar(8),		-- 運行年月日				
	@CompanyCd		int,			-- 会社				
	@StaEigyoCd		int,			-- 車輌営業所から				
	@EndEigyoCd		int,			-- 車輌営業所まで				
	@SyuKbn			int				-- 集計区分	
AS
BEGIN
	SELECT eVPM_Eigyos00.EigyoCd																														
        ,eVPM_Eigyos00.RyakuNm																														
        ,eVPM_Eigyos00.EigyoNm																														
        --実在車輌数																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Count(*)																														
                                FROM VPM_SyaRyo AS eVPM_SyaRyo																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eVPM_SyaRyo.SyaRyoCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        -- if @SyuKbn = 2																														
                                        AND (@SyuKbn != 2 OR eVPM_SyaRyo.NinkaKbn = 3)																													
                                        -- else																														
                                        AND (@SyuKbn = 2 OR eVPM_SyaRyo.NinkaKbn = 1)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																											
                                ), 0)) AS TotalActualSyaryo																														
        --実働車輌数																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT count(*)																														
                                FROM (																														
                                        SELECT eVPM_SyaRyo.SyaRyoCd																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Shabni.UkeNo = eTKD_Haisha.UkeNo																														
                                                AND eTKD_Shabni.UnkRen = eTKD_Haisha.UnkRen																														
                                                AND eTKD_Shabni.TeiDanNo = eTKD_Haisha.TeiDanNo																														
                                                AND eTKD_Shabni.BunkRen = eTKD_Haisha.BunkRen																														
                                        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq																														
                                        LEFT JOIN VPM_SyaRyo AS eVPM_SyaRyo ON eTKD_Haisha.HaiSSryCdSeq = eVPM_SyaRyo.SyaRyoCdSeq																														
                                        LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eVPM_SyaRyo.SyaRyoCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                                AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                                AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                        LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																													
                                        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                        WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                                AND eTKD_Shabni.UnkYmd = @UnkYmd																														
                                                AND eTKD_Haisha.SiyoKbn = 1																														
                                                AND eTKD_Yyksho.YoyaSyu = 1																														
                                                -- if @OutStei = 1																														
                                                AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = eTKD_Shabni.UnkYmd)																												
                                                -- else																														
                                                AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = eTKD_Shabni.UnkYmd)																													
                                                -- if @SyuKbn = 2																														
                                                AND (@SyuKbn != 2 OR eVPM_SyaRyo.NinkaKbn = 3)																											
                                                -- else																														
                                                AND (@SyuKbn = 2 OR eVPM_SyaRyo.NinkaKbn = 1)																												
                                                AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)	
                                                AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																													
                                        GROUP BY eVPM_SyaRyo.SyaRyoCd																														
                                        ) AS workTable																														
                                ), 0)) AS TotalWorkStock																														
        --実働車輌数(泊)																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT count(*)																														
                                FROM (																														
                                        SELECT eVPM_SyaRyo.SyaRyoCd																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Shabni.UkeNo = eTKD_Haisha.UkeNo																														
                                                AND eTKD_Shabni.UnkRen = eTKD_Haisha.UnkRen																														
                                                AND eTKD_Shabni.TeiDanNo = eTKD_Haisha.TeiDanNo																														
                                                AND eTKD_Shabni.BunkRen = eTKD_Haisha.BunkRen																														
                                        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq																														
                                        LEFT JOIN VPM_SyaRyo AS eVPM_SyaRyo ON eTKD_Haisha.HaiSSryCdSeq = eVPM_SyaRyo.SyaRyoCdSeq																														
                                        LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eVPM_SyaRyo.SyaRyoCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                                AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                                AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq
                                        LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                        WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                                AND eTKD_Shabni.UnkYmd = @UnkYmd																														
                                                AND eTKD_Haisha.SiyoKbn = 1																														
                                                AND eTKD_Yyksho.YoyaSyu = 1																														
                                                -- if @OutStei = 1																														
                                                AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd != eTKD_Shabni.UnkYmd)																													
                                                -- else																														
                                                AND (@OutStei = 1 OR eTKD_Haisha.KikYmd != eTKD_Shabni.UnkYmd)																													
                                                -- if @SyuKbn = 2																														
                                                AND (@SyuKbn != 2 OR eVPM_SyaRyo.NinkaKbn = 3)																												
                                                -- else																														
                                                AND (@SyuKbn = 2 OR eVPM_SyaRyo.NinkaKbn = 1)																												
                                                AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                                AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)
                                        GROUP BY eVPM_SyaRyo.SyaRyoCd																														
                                        ) AS workTable																														
                                ), 0)) AS TotalWorkNight																														
        --臨時増車																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT count(*)																														
                                FROM (																														
                                        SELECT eVPM_SyaRyo.SyaRyoCd																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Shabni.UkeNo = eTKD_Haisha.UkeNo																														
                                                AND eTKD_Shabni.UnkRen = eTKD_Haisha.UnkRen																														
                                                AND eTKD_Shabni.TeiDanNo = eTKD_Haisha.TeiDanNo																														
                                                AND eTKD_Shabni.BunkRen = eTKD_Haisha.BunkRen																														
                                        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq																														
                                        LEFT JOIN VPM_SyaRyo AS eVPM_SyaRyo ON eTKD_Haisha.HaiSSryCdSeq = eVPM_SyaRyo.SyaRyoCdSeq																														
                                        LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eVPM_SyaRyo.SyaRyoCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                                AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                                AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq
                                        LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                        WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                                AND eTKD_Shabni.UnkYmd <= @UnkYmd																														
                                                AND eTKD_Haisha.SiyoKbn = 1																														
                                                AND eTKD_Yyksho.YoyaSyu = 1																														
                                                -- if @OutStei = 1																														
                                                AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = eTKD_Shabni.UnkYmd)																												
                                                -- else																														
                                                AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = eTKD_Shabni.UnkYmd)																													
                                                -- if @SyuKbn = 2																														
                                                AND (@SyuKbn != 2 OR eVPM_SyaRyo.NinkaKbn != 3)																													
                                                -- else																														
                                                AND (@SyuKbn = 2 OR eVPM_SyaRyo.NinkaKbn != 1)																												
                                                AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                                AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																														
                                        GROUP BY eVPM_SyaRyo.SyaRyoCd																														
                                        ) AS workTable																														
                                ), 0)) AS TempIncrease																														
        --団体件数本社扱い																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT count(*)																														
                                FROM (																														
                                        SELECT eTKD_Haisha.UkeNo																														
                                                ,eTKD_Haisha.UnkRen																														
                                        FROM TKD_Haisha AS eTKD_Haisha																														
                                        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq																														
                                        LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                                AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                                AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq
                                        LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk ON eVPM_Tokisk.TokuiSeq = eTKD_Yyksho.TokuiSeq																														
                                        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya ON eVPM_Gyosya.GyosyaCdSeq = eVPM_Tokisk.GyosyaCdSeq																														
                                        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                        WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                                AND eTKD_Haisha.SiyoKbn = 1																														
                                                AND eTKD_Yyksho.YoyaSyu = 1																														
                                                -- if @OutStei = 1																														
                                                AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																												
                                                -- else																														
                                                AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)																												
                                                AND eVPM_Gyosya.GyosyaKbn != 1																														
                                                AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                                AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																														
                                        GROUP BY eTKD_Haisha.UkeNo																														
                                                ,eTKD_Haisha.UnkRen																														
                                        ) AS workTable																														
                                ), 0)) AS TotalDantaiHeadOffice																														
        --団体件数斡旋業者扱い																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT count(*)																														
                                FROM (																														
                                        SELECT eTKD_Haisha.UkeNo																														
                                                ,eTKD_Haisha.UnkRen																														
                                        FROM TKD_Haisha AS eTKD_Haisha																														
                                        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq																														
                                        LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                                AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                                AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                        LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																													
                                        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk ON eVPM_Tokisk.TokuiSeq = eTKD_Yyksho.TokuiSeq																														
                                        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya ON eVPM_Gyosya.GyosyaCdSeq = eVPM_Tokisk.GyosyaCdSeq																														
                                        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                        WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                                AND eTKD_Haisha.SiyoKbn = 1																														
                                                AND eTKD_Yyksho.YoyaSyu = 1																														
                                                -- if @OutStei = 1																														
                                                AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                                -- else																														
                                                AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)																													
                                                AND eVPM_Gyosya.GyosyaKbn = 1																														
                                                AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)	
                                                AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																													
                                        GROUP BY eTKD_Haisha.UkeNo																														
                                                ,eTKD_Haisha.UnkRen																														
                                        ) AS workTable																														
                                ), 0)) AS TotalDantaiMediator																														
        --運行回数本社扱い																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT SUM(UnkKai)																														
                                FROM (																														
                                        SELECT eTKD_Haisha.UkeNo																														
                                                ,eTKD_Haisha.UnkRen																														
                                                ,CASE 																														
                                                        WHEN eTKD_Shabni.UnkKai IS NULL																														
                                                                THEN 1																														
                                                        ELSE eTKD_Shabni.UnkKai																														
                                                        END AS UnkKai																														
                                        FROM TKD_Haisha AS eTKD_Haisha																														
                                        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq																														
                                        LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                                AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                                AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                        LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																													
                                        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk ON eVPM_Tokisk.TokuiSeq = eTKD_Yyksho.TokuiSeq																														
                                        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya ON eVPM_Gyosya.GyosyaCdSeq = eVPM_Tokisk.GyosyaCdSeq																														
                                        LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                                AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                                AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                                AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                        WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                                AND eVPM_Gyosya.GyosyaKbn != 1																														
                                                AND eTKD_Haisha.SiyoKbn = 1																														
                                                AND eTKD_Yyksho.YoyaSyu = 1																														
                                                -- if @OutStei = 1																														
                                                AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                                -- else																														
                                                AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)																													
                                                AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                                AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																														
                                        ) AS workTable																														
                                ), 0)) AS TotalUnkoHeadOffice																														
        --運行回数斡旋業者扱い																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT SUM(UnkKai)																														
                                FROM (																														
                                        SELECT eTKD_Haisha.UkeNo																														
                                                ,eTKD_Haisha.UnkRen																														
                                                ,CASE 																														
                                                        WHEN eTKD_Shabni.UnkKai IS NULL																														
                                                                THEN 1																														
                                                        ELSE eTKD_Shabni.UnkKai																														
                                                        END AS UnkKai																														
                                        FROM TKD_Haisha AS eTKD_Haisha																														
                                        LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                        LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq																														
                                        LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                                AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                                AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                        LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                        LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																													
                                        LEFT JOIN VPM_Tokisk AS eVPM_Tokisk ON eVPM_Tokisk.TokuiSeq = eTKD_Yyksho.TokuiSeq																														
                                        LEFT JOIN VPM_Gyosya AS eVPM_Gyosya ON eVPM_Gyosya.GyosyaCdSeq = eVPM_Tokisk.GyosyaCdSeq																														
                                        LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                                AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                                AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                                AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                        LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                        WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                                AND eVPM_Gyosya.GyosyaKbn = 1																														
                                                AND eTKD_Haisha.SiyoKbn = 1																														
                                                AND eTKD_Yyksho.YoyaSyu = 1																														
                                                -- if @OutStei = 1																														
                                                AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                                -- else																														
                                                AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)																													
                                                AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                                AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd)																														
                                        ) AS workTable																														
                                ), 0)) AS TotalUnkoMediator																														
        --運送収入合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Haisha.SyaRyoUnc + eTKD_Haisha.SyaRyoSyo)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo	
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumSyaRyoUnc																														
        --手数料合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Haisha.SyaRyoTes)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumSyaRyoTes																														
        --乗車人員合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(YV_ShabniHaishaSum01.JyoSyaJin)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                                ,MAX(eTKD_Shabni.JyoSyaJin) AS JyoSyaJin																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS YV_ShabniHaishaSum01 ON eTKD_Haisha.UkeNo = YV_ShabniHaishaSum01.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = YV_ShabniHaishaSum01.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = YV_ShabniHaishaSum01.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = YV_ShabniHaishaSum01.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumJyoSyaJin																														
        --ﾌﾟﾗｽ人員合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(YV_ShabniHaishaSum01.PlusJin)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                                ,MAX(eTKD_Shabni.PlusJin) AS PlusJin																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS YV_ShabniHaishaSum01 ON eTKD_Haisha.UkeNo = YV_ShabniHaishaSum01.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = YV_ShabniHaishaSum01.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = YV_ShabniHaishaSum01.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = YV_ShabniHaishaSum01.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumPlusJin																														
        --実車一般キロ合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.JisaIPKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE eTKD_Shabni.UnkYmd = @UnkYmd																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        AND left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumJisaIPKm																														
        --実車高速キロ合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.JisaKSKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE eTKD_Shabni.UnkYmd = @UnkYmd																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo	
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        AND left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumJisaKSKm																														
        --回送一般キロ合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.KisoIPKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE eTKD_Shabni.UnkYmd = @UnkYmd																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        AND left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumKisoIPKm																														
        --回送高速キロ合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.KisoKOKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE eTKD_Shabni.UnkYmd = @UnkYmd																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																									
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        AND left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumKisoKOKm																														
        --その他キロ合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.OthKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE eTKD_Shabni.UnkYmd = @UnkYmd																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo	
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        AND left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumOthKm																														
        --総走行キロ合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.JisaIPKm + eTKD_Shabni.JisaKSKm + eTKD_Shabni.KisoIPkm + eTKD_Shabni.KisoKOKm + eTKD_Shabni.OthKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE eTKD_Shabni.UnkYmd = @UnkYmd																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        AND left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumTotalKm																														
        --燃料１合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.Nenryo1)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumNenryo1																														
        --燃料２合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.Nenryo2)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumNenryo2																														
        --燃料３合計																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.Nenryo3)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo	
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR eTKD_Haisha.SyuKoYmd = @UnkYmd)																													
                                        -- else																														
                                        AND (@OutStei = 1 OR eTKD_Haisha.KikYmd = @UnkYmd)	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS SumNenryo3																														
        --運送収入合計当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Haisha.SyaRyoUnc + eTKD_Haisha.SyaRyoSyo)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthSyaRyoUnc																														
        --手数料合計当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Haisha.SyaRyoTes)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo	
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthSyaRyoTes																														
        --乗車人員合計当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(YV_ShabniHaishaSum01.JyoSyaJin)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                                ,MAX(eTKD_Shabni.JyoSyaJin) AS JyoSyaJin																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS YV_ShabniHaishaSum01 ON eTKD_Haisha.UkeNo = YV_ShabniHaishaSum01.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = YV_ShabniHaishaSum01.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = YV_ShabniHaishaSum01.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = YV_ShabniHaishaSum01.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthJyoSyaJin																														
        --ﾌﾟﾗｽ人員合計当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(YV_ShabniHaishaSum01.PlusJin)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                                ,MAX(eTKD_Shabni.PlusJin) AS PlusJin																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS YV_ShabniHaishaSum01 ON eTKD_Haisha.UkeNo = YV_ShabniHaishaSum01.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = YV_ShabniHaishaSum01.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = YV_ShabniHaishaSum01.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = YV_ShabniHaishaSum01.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo																														
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq																														
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthPlusJin																														
        --実車一般キロ当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.JisaIPKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthJisaIPKm																														
        --実車高速キロ当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.JisaKSKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthJisaKSKm																														
        --回送一般キロ当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.KisoIPKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthKisoIPKm																														
        --回送高速キロ当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.KisoKOKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																									
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo	
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthKisoKOKm																														
        --その他キロ当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.OthKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthOthKm																														
        --総走行キロ当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.JisaIPKm + eTKD_Shabni.JisaKSKm + eTKD_Shabni.KisoIPkm + eTKD_Shabni.KisoKOKm + eTKD_Shabni.OthKm)																														
                                FROM (																														
                                        SELECT eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        FROM TKD_Shabni AS eTKD_Shabni																														
                                        WHERE left(eTKD_Shabni.UnkYmd, 6) = left(@UnkYmd, 6)																														
                                        GROUP BY eTKD_Shabni.UkeNo																														
                                                ,eTKD_Shabni.UnkRen																														
                                                ,eTKD_Shabni.TeiDanNo																														
                                                ,eTKD_Shabni.BunkRen																														
                                        ) AS WORK																														
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON WORK.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND WORK.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND WORK.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND WORK.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Haisha AS eTKD_Haisha ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo	
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthTotalKm																														
        --燃料１合計当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.Nenryo1)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthNenryo1																														
        --燃料２合計当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.Nenryo2)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																									
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthNenryo2																														
        --燃料３合計当月分																														
        ,CONVERT(VARCHAR, ISNULL((																														
                                SELECT Sum(eTKD_Shabni.Nenryo3)																														
                                FROM TKD_Haisha AS eTKD_Haisha																														
                                LEFT JOIN VPM_HenSya AS eVPM_HenSya ON eTKD_Haisha.HaiSSryCdSeq = eVPM_HenSya.SyaRyoCdSeq																														
                                        AND eVPM_HenSya.StaYmd <= @UnkYmd																														
                                        AND eVPM_HenSya.EndYmd >= @UnkYmd																																										
                                LEFT JOIN TKD_Shabni AS eTKD_Shabni ON eTKD_Haisha.UkeNo = eTKD_Shabni.UkeNo																														
                                        AND eTKD_Haisha.UnkRen = eTKD_Shabni.UnkRen																														
                                        AND eTKD_Haisha.TeiDanNo = eTKD_Shabni.TeiDanNo																														
                                        AND eTKD_Haisha.BunkRen = eTKD_Shabni.BunkRen																														
                                LEFT JOIN TKD_Yyksho AS eTKD_Yyksho ON eTKD_Haisha.UkeNo = eTKD_Yyksho.UkeNo
                                LEFT JOIN VPM_YoyKbn AS eVPM_YoyKbn ON eTKD_Yyksho.YoyaKbnSeq = eVPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_Eigyos AS eVPM_Eigyos ON eVPM_HenSya.EigyoCdSeq = eVPM_Eigyos.EigyoCdSeq	
                                LEFT JOIN VPM_Compny AS eVPM_Compny ON eVPM_Compny.CompanyCdSeq = eVPM_Eigyos.CompanyCdSeq
                                LEFT JOIN VPM_Tenant AS eVPM_Tenant ON eTKD_Yyksho.TenantCdSeq = eVPM_Tenant.TenantCdSeq																														
                                WHERE eVPM_Eigyos.EigyoCd = eVPM_Eigyos00.EigyoCd																														
                                        AND eTKD_Haisha.SiyoKbn = 1																														
                                        AND eTKD_Yyksho.YoyaSyu = 1																														
                                        -- if @OutStei = 1																														
                                        AND (@OutStei != 1 OR left(eTKD_Haisha.SyuKoYmd, 6) = left(@UnkYmd, 6))																														
                                        -- else																														
                                        AND (@OutStei = 1 OR left(eTKD_Haisha.KikYmd, 6) = left(@UnkYmd, 6))	
                                        AND (@SyuKbn = 0 OR eVPM_YoyKbn.UnsouKbn = @SyuKbn)
                                        AND (@CompanyCd = 0 OR eVPM_Compny.CompanyCd = @CompanyCd) 
                                ), 0)) AS CurMonthNenryo3																														
	FROM VPM_Eigyos AS eVPM_Eigyos00																														
	WHERE 1 = 1																														
			AND (@StaEigyoCd = 0 OR eVPM_Eigyos00.EigyoCd >= @StaEigyoCd) -- if @StaEigyoCd <> ''																														
			AND (@EndEigyoCd = 0 OR eVPM_Eigyos00.EigyoCd <= @EndEigyoCd) -- if @EndEigyoCd <> ''																														
END
GO
