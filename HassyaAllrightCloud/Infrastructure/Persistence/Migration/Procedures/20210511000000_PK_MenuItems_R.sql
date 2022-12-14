USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_MenuItems_R]    Script Date: 05/04/2021 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name    :   HassyaAlrightCloud
-- Module-Name    :   HassyaAlrightCloud
-- SP-ID        :   PK_MenuItems_R
-- DB-Name        :   HOC_Kashikiri
-- Name            :   PK_MenuItems_R
-- Date            :   2021/05/04
-- Author        :   Tra Nguyen Lam
-- Description    :   Get menu items for navmenu
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_MenuItems_R]
    (
     -- Parameter
        @SyainCdSeq int,
        @TenantCdSeq  int,
        @ServiceCdSeq int,
        @LangCode varchar(10)
    )
AS 
    BEGIN
        ;WITH eVPM_Kiken AS (
            SELECT
                VPM_Kinou.KinouID,
                VPM_Kinou.ServiceCdSeq,
                VPM_Kinou.KinouUrl
            FROM
                VPM_Kinou
                LEFT JOIN VPM_GpKanr AS VPM_GpKanr01 ON VPM_GpKanr01.KinouID = VPM_Kinou.KinouID
                LEFT JOIN VPM_KinouGpKanr ON VPM_KinouGpKanr.KinouID = VPM_Kinou.KinouID
                LEFT JOIN VPM_GpKanr AS VPM_GpKanr02 ON VPM_GpKanr02.KinouGroupID = VPM_KinouGpKanr.KinouGroupID
                LEFT JOIN VPM_UserKe ON VPM_UserKe.GroupID = VPM_GpKanr01.GroupID
                OR VPM_UserKe.GroupID = VPM_GpKanr02.GroupID
            WHERE
                VPM_UserKe.SyainCdSeq = @SyainCdSeq --ログインユーザー
                AND(
                    VPM_GpKanr01.AuthoriID = '001'
                    OR VPM_KinouGpKanr.AuthoriID = '001'
                ) --アクセス権限
        ),
        eVPM_LicenseService AS (
            SELECT
                DISTINCT VPM_LicenseService.ServiceCdSeq
            FROM
                VPM_LicenseService
                JOIN VPM_TenantLicense ON VPM_TenantLicense.LicenceCdSeq = VPM_LicenseService.LicenceCdSeq
                JOIN VPM_Group ON VPM_Group.LicenceCdSeq = VPM_TenantLicense.LicenceCdSeq
                JOIN VPM_UserKe ON VPM_UserKe.GroupID = VPM_Group.GroupID
            WHERE
                VPM_TenantLicense.KihonOptionKbn = 1 --基本
                AND VPM_TenantLicense.EnvironmentalKbn = 1 --PC
                AND FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN VPM_TenantLicense.StaYmd
                AND VPM_TenantLicense.EndYmd
                AND VPM_TenantLicense.TenantCdSeq = @TenantCdSeq --ログインユーザーのテナント
                AND VPM_UserKe.SyainCdSeq = @SyainCdSeq --ログインユーザー
        )
        SELECT
            DISTINCT VPM_Menu.MenuCdSeq,
            VPM_Menu.Node,
            VPM_Menu.Sort,
            VPM_Menu.ParentMenuCdSeq,
            VPM_Menu.Icon,
            ISNULL(MR.[Name], VPM_Menu.MenuNm) AS MenuNm,
            VPM_Menu.KinouID,
            ISNULL(eVPM_Kiken01.ServiceCdSeq, 0) AS ServiceCdSeq,
            ISNULL(eVPM_Kiken01.KinouUrl, '') AS KinouUrl
        FROM
            VPM_Menu
            LEFT JOIN VPM_MenuResource AS MR ON MR.MenuCdSeq = VPM_Menu.MenuCdSeq AND MR.LangCode = @LangCode
            LEFT JOIN VPM_Menu AS VPM_Menu02 ON VPM_Menu02.ParentMenuCdSeq = VPM_Menu.MenuCdSeq
            LEFT JOIN VPM_Menu AS VPM_Menu03 ON VPM_Menu03.ParentMenuCdSeq = VPM_Menu02.MenuCdSeq
            LEFT JOIN VPM_Menu AS VPM_Menu04 ON VPM_Menu04.ParentMenuCdSeq = VPM_Menu03.MenuCdSeq
            LEFT JOIN eVPM_Kiken AS eVPM_Kiken01 ON eVPM_Kiken01.KinouID = VPM_Menu.KinouID
            LEFT JOIN eVPM_Kiken AS eVPM_Kiken02 ON eVPM_Kiken02.KinouID = VPM_Menu02.KinouID
            LEFT JOIN eVPM_Kiken AS eVPM_Kiken03 ON eVPM_Kiken03.KinouID = VPM_Menu03.KinouID
            LEFT JOIN eVPM_Kiken AS eVPM_Kiken04 ON eVPM_Kiken04.KinouID = VPM_Menu04.KinouID
            LEFT JOIN eVPM_LicenseService AS eVPM_LicenseService01 ON eVPM_LicenseService01.ServiceCdSeq = eVPM_Kiken01.ServiceCdSeq
            LEFT JOIN eVPM_LicenseService AS eVPM_LicenseService02 ON eVPM_LicenseService02.ServiceCdSeq = eVPM_Kiken02.ServiceCdSeq
            LEFT JOIN eVPM_LicenseService AS eVPM_LicenseService03 ON eVPM_LicenseService03.ServiceCdSeq = eVPM_Kiken03.ServiceCdSeq
            LEFT JOIN eVPM_LicenseService AS eVPM_LicenseService04 ON eVPM_LicenseService04.ServiceCdSeq = eVPM_Kiken04.ServiceCdSeq
        WHERE
            VPM_Menu.ServiceCdSeq = @ServiceCdSeq --サービス
            AND (
                eVPM_Kiken01.KinouID IS NOT NULL
                OR eVPM_Kiken02.KinouID IS NOT NULL
                OR eVPM_Kiken03.KinouID IS NOT NULL
                OR eVPM_Kiken04.KinouID IS NOT NULL
            )
            AND (
                eVPM_LicenseService01.ServiceCdSeq IS NOT NULL
                OR eVPM_LicenseService02.ServiceCdSeq IS NOT NULL
                OR eVPM_LicenseService03.ServiceCdSeq IS NOT NULL
                OR eVPM_LicenseService04.ServiceCdSeq IS NOT NULL
            )
        ORDER BY
            VPM_Menu.Node,
            VPM_Menu.Sort
    END
GO                                                                                                                    