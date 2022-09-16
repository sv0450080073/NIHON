USE [HOC_Kashikiri];
GO

/****** Object:  StoredProcedure [dbo].[PK_YusoSyu_E]    Script Date: 2020/07/28 9:57:22 ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- System-Name			: HassyaAlrightCloud
-- Module-Name			: GetTransportationSummary
-- SP-ID				: [PK_YusoSyu_E]
-- DB-Name				: HOC_Kashikiri
-- Author				: Tra Nguyen
-- Create date			: 2020/07/28
-- Description			: 輸送実績報告書テーブルのINSERT/DELETE処理
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[PK_YusoSyu_E]
--パラメータ
(@StrDate      CHAR(8),		-- Format yyyyMMdd
 @EndDate      CHAR(8),		-- Format yyyyMMdd
 @UnsoKbn      TINYINT,		-- UnsoKbnの値がそれぞれ1 (一般)、2 (特定)、3 (特殊)
 @CompanyCdSeq INT, 
 @StrEigyoCd   CHAR(5), 
 @EndEigyoCd   CHAR(5), 
 @KinouId      CHAR(6), 
 @SyainCdSeq   INT,			-- 最終更新社員コード 
 @UpdPrgID     CHAR(10),	-- 最終更新プログラムID
 @TenantCdSeq INT
)
AS
     DECLARE @syshiduke CHAR(08);				-- システム日付(YYYYMMDD)
     DECLARE @sysjikan CHAR(06);				-- システム時間(HHMMSS)
     DECLARE @wk_UpdPrgID VARCHAR(MAX);			-- ｗｋ最終更新プログラムID
     DECLARE @InString TABLE
     (NinkaKbn TINYINT, 
      [Type]   TINYINT
     );
     DECLARE @JyoSubTable TABLE
     (EigyoCdSeq INT, 
      KataKbn    TINYINT, 
      NenRyoKbn  TINYINT, 
      Cnt        INT
     );
     DECLARE @RinjiTable TABLE
     (EigyoCdSeq INT, 
      KataKbn    TINYINT, 
      NenRyoKbn  TINYINT, 
      Cnt        INT, 
      JisaKm     NUMERIC(9, 2), 
      KisoKm     NUMERIC(9, 2), 
      Jinin      INT
     );
     DECLARE @JituTable TABLE
     (EigyoCdSeq INT, 
      KataKbn    TINYINT, 
      NenRyoKbn  TINYINT, 
      Cnt        INT, 
      JisaKm     NUMERIC(9, 2), 
      KisoKm     NUMERIC(9, 2), 
      Jinin      INT
     );
     DECLARE @UnkoTable TABLE
     (EigyoCdSeq    INT, 
      KataKbn       TINYINT, 
      NenRyoKbn     TINYINT, 
      Cnt           INT, 
      Unchin        NUMERIC(10, 0), 
      UnsoZaSyu     NUMERIC(10, 0), 
      UnkoKikak1Cnt INT, 
      UnkoOthCnt    INT
     );
     DECLARE @KataNenryo TABLE
     (KataKbn   TINYINT, 
      NenRyoKbn TINYINT
     );
     DECLARE @ZaSyuKbn TINYINT;
     DECLARE @UnkinKbn TINYINT;
     DECLARE @FutaiSyu1Flg TINYINT;
     DECLARE @FutaiSyu2Flg TINYINT;
     DECLARE @FutaiSyu3Flg TINYINT;
     DECLARE @FutaiSyu4Flg TINYINT;
     DECLARE @UnkoKbn TINYINT;
     DECLARE @UnsoSyuKbn_GuideRyo TINYINT; --運送収入区分_ガイド料（0:ガイド料を含まない　1:ガイド料を含む）
     DECLARE @YusoJinKbn TINYINT; --輸送人員区分（0:輸送人員　1:輸送延人員）

    BEGIN
        --システム日付(YYYYMMDD)及びシステム時間(HHMMSS)取得
        SELECT @syshiduke = FP_tblHiduke, 
               @sysjikan = FP_tblJikan
        FROM FP_DatTim();
        SET @wk_UpdPrgID = REPLACE(@UpdPrgID, '''', '''''');

        -- 運送区分別、仮想認可区分チェックテーブル作成
        IF @UnsoKbn = 1
            BEGIN
                INSERT INTO @InString
                VALUES
                (1, 
                 1
                );
                INSERT INTO @InString
                VALUES
                (2, 
                 2
                );
                INSERT INTO @InString
                VALUES
                (3, 
                 2
                );
                INSERT INTO @InString
                VALUES
                (4, 
                 2
                );
                INSERT INTO @InString
                VALUES
                (5, 
                 2
                );
                INSERT INTO @InString
                VALUES
                (6, 
                 2
                );
            END;
            ELSE
            IF @UnsoKbn = 2
                BEGIN
                    INSERT INTO @InString
                    VALUES
                    (1, 
                     2
                    );
                    INSERT INTO @InString
                    VALUES
                    (2, 
                     2
                    );
                    INSERT INTO @InString
                    VALUES
                    (3, 
                     1
                    );
                    INSERT INTO @InString
                    VALUES
                    (4, 
                     2
                    );
                    INSERT INTO @InString
                    VALUES
                    (5, 
                     2
                    );
                    INSERT INTO @InString
                    VALUES
                    (6, 
                     2
                    );
                END;
                ELSE
                IF @UnsoKbn = 3
                    BEGIN
                        INSERT INTO @InString
                        VALUES
                        (1, 
                         2
                        );
                        INSERT INTO @InString
                        VALUES
                        (2, 
                         1
                        );
                        INSERT INTO @InString
                        VALUES
                        (3, 
                         2
                        );
                        INSERT INTO @InString
                        VALUES
                        (4, 
                         1
                        );
                        INSERT INTO @InString
                        VALUES
                        (5, 
                         1
                        );
                        INSERT INTO @InString
                        VALUES
                        (6, 
                         1
                        );
                    END;

        -- 仮想型区分、燃料区分振り分けテーブル
        INSERT INTO @KataNenryo
        VALUES
        (1, 
         1
        );
        INSERT INTO @KataNenryo
        VALUES
        (1, 
         2
        );
        INSERT INTO @KataNenryo
        VALUES
        (1, 
         3
        );
        INSERT INTO @KataNenryo
        VALUES
        (1, 
         4
        );
        INSERT INTO @KataNenryo
        VALUES
        (1, 
         5
        );
        INSERT INTO @KataNenryo
        VALUES
        (2, 
         1
        );
        INSERT INTO @KataNenryo
        VALUES
        (2, 
         2
        );
        INSERT INTO @KataNenryo
        VALUES
        (2, 
         3
        );
        INSERT INTO @KataNenryo
        VALUES
        (2, 
         4
        );
        INSERT INTO @KataNenryo
        VALUES
        (2, 
         5
        );
        INSERT INTO @KataNenryo
        VALUES
        (3, 
         1
        );
        INSERT INTO @KataNenryo
        VALUES
        (3, 
         2
        );
        INSERT INTO @KataNenryo
        VALUES
        (3, 
         3
        );
        INSERT INTO @KataNenryo
        VALUES
        (3, 
         4
        );
        INSERT INTO @KataNenryo
        VALUES
        (3, 
         5
        );
        SELECT @FutaiSyu2Flg = FutSF2Flg, 
               @FutaiSyu1Flg = FutSF1Flg, 
               @ZaSyuKbn = ZasyuKbn, 
               @UnkinKbn = HouZeiKbn, 
               @FutaiSyu3Flg = FutSF3Flg, 
               @FutaiSyu4Flg = FutSF4Flg, 
               @UnkoKbn = HoukoKbn
        FROM TKM_KasSet JOIN VPM_Compny ON VPM_Compny.CompanyCdSeq = TKM_KasSet.CompanyCdSeq
        WHERE VPM_Compny.CompanyCdSeq = @CompanyCdSeq AND VPM_Compny.TenantCdSeq = @TenantCdSeq;
        IF EXISTS
        (
            SELECT Kinou01, 
                   Kinou02
            FROM VPD_CustomKi
            WHERE SyainCdSeq = @SyainCdSeq
                  AND KinouId = @KinouId
        )
            SELECT @YusoJinKbn = Kinou01, 
                   @UnsoSyuKbn_GuideRyo = Kinou02
            FROM VPD_CustomKi
            WHERE SyainCdSeq = @SyainCdSeq
                  AND KinouId = @KinouId;
            ELSE
            BEGIN
                SELECT @YusoJinKbn = Kinou01, 
                       @UnsoSyuKbn_GuideRyo = Kinou02
                FROM VPD_CustomKi
                WHERE SyainCdSeq = 0
                      AND KinouId = @KinouId;
            END;
        BEGIN TRY
            INSERT INTO @JyoSubTable
                   SELECT EigyoCdSeq, 
                          KataKbn, 
                          NenRyoKbn, 
                          SUM(JyouCnt) AS Cnt
                   FROM
                   (
                       SELECT VPM_HenSya.EigyoCdSeq, 
                              VPM_SyaSyu.KataKbn, 
                              VPM_SyaSyu.NenRyoKbn,
                              CASE
                                  WHEN VPM_HenSya.StaYmd <= @StrDate
                                       AND VPM_HenSya.ENDYmd >= @EndDate
                                  THEN DAY(CONVERT(DATETIME, @EndDate))
                                  WHEN VPM_HenSya.StaYmd <= @StrDate
                                       AND VPM_HenSya.ENDYmd < @EndDate
                                  THEN DAY(CONVERT(DATETIME, VPM_HenSya.ENDYmd))
                                  WHEN VPM_HenSya.StaYmd > @StrDate
                                       AND VPM_HenSya.ENDYmd >= @EndDate
                                  THEN DAY(CONVERT(DATETIME, @EndDate)) - DAY(CONVERT(DATETIME, VPM_HenSya.StaYmd)) + 1
                                  ELSE DAY(CONVERT(DATETIME, VPM_HenSya.ENDYmd)) - DAY(CONVERT(DATETIME, VPM_HenSya.StaYmd)) + 1
                              END AS JyouCnt
                       FROM VPM_SyaRyo
                            INNER JOIN VPM_SyaSyu ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
                                                     AND VPM_SyaSyu.SiyoKbn = 1
													 AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq
                            INNER JOIN VPM_HenSya ON VPM_SyaRyo.SyaRyoCdSeq = VPM_HenSya.SyaRyoCdSeq
                                                     AND VPM_HenSya.StaYmd <= @EndDate
                                                     AND VPM_HenSya.ENDYmd >= @StrDate
                            LEFT JOIN VPM_Eigyos ON VPM_HenSya.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
                                                    AND VPM_Eigyos.SiyoKbn = 1
                            LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
                                                    AND VPM_Compny.SiyoKbn = 1
                            LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
                                                    AND VPM_Tenant.SiyoKbn = 1
                       WHERE VPM_SyaRyo.NinkaKbn IN
                       (
                           SELECT NinkaKbn
                           FROM @InString
                           WHERE [Type] = 1
                       )
                             AND VPM_Compny.CompanyCdSeq = @CompanyCdSeq
                             AND VPM_Tenant.TenantCdSeq = @TenantCdSeq
                   ) AS Main
                   GROUP BY Main.EigyoCdSeq, 
                            Main.KataKbn, 
                            Main.NenRyoKbn;
            INSERT INTO @RinjiTable
                   SELECT EigyoCdSeq, 
                          KataKbn, 
                          NenRyoKbn, 
                          COUNT(EigyoCdSeq) AS Cnt, 
                          SUM(JisaKm) AS JisaKm, 
                          SUM(KisoKm) AS KisoKm, 
                          SUM(Jinin) AS Jinin
                   FROM
                   (
                       SELECT UnkYmd, 
                              SyaRyoCdSeq, 
                              EigyoCdSeq, 
                              KataKbn, 
                              NenRyoKbn, 
                              SUM(JisaKm) AS JisaKm, 
                              SUM(KisoKm) AS KisoKm, 
                              SUM(CASE @YusoJinKbn
                                      WHEN 0
                                      THEN Jinin
                                      WHEN 1
                                      THEN NobeJinin
                                      ELSE 0
                                  END) AS Jinin
                       FROM
                       (
                           SELECT TKD_Shabni.UnkYmd, 
                                  VPM_SyaRyo.SyaRyoCdSeq, 
                                  VPM_HenSya.EigyoCdSeq, 
                                  VPM_SyaSyu.KataKbn, 
                                  VPM_SyaSyu.NenRyoKbn, 
                                  (TKD_Shabni.JisaIPKm + TKD_Shabni.JisaKSKm) AS JisaKm, 
                                  (TKD_Shabni.KisoIPkm + TKD_Shabni.KisoKOKm) AS KisoKm, 
                                  ISNULL(TKD_Shabni2.JyoSyaJin, 0) AS Jinin, 
                                  TKD_Shabni.JyoSyaJin AS NobeJinin
                           FROM TKD_Shabni
                                LEFT JOIN
                           (
                               SELECT UkeNo, 
                                      UnkRen, 
                                      TeiDanNo, 
                                      BunkRen,
                                      CASE
                                          WHEN
                               (
                                   SELECT HoukoKbn
                                   FROM TKM_KasSet
                                   WHERE CompanyCdSeq = @CompanyCdSeq
                               ) = 1
                                          THEN MIN(UnkYmd)
                                          ELSE MAX(UnkYmd)
                                      END AS UnkYmd, 
                                      MAX(JyoSyaJin) AS JyoSyaJin
                               FROM TKD_Shabni
                               WHERE SiyoKbn = 1
                               GROUP BY UkeNo, 
                                        UnkRen, 
                                        TeiDanNo, 
                                        BunkRen
                           ) AS TKD_Shabni2 ON TKD_Shabni.UkeNo = TKD_Shabni2.UkeNo
                                               AND TKD_Shabni.UnkRen = TKD_Shabni2.UnkRen
                                               AND TKD_Shabni.TeiDanNo = TKD_Shabni2.TeiDanNo
                                               AND TKD_Shabni.BunkRen = TKD_Shabni2.BunkRen
                                               AND TKD_Shabni.UnkYmd = TKD_Shabni2.UnkYmd
                                LEFT JOIN TKD_Haisha ON TKD_Shabni.UkeNo = TKD_Haisha.UkeNo
                                                        AND TKD_Shabni.UnkRen = TKD_Haisha.UnkRen
                                                        AND TKD_Shabni.TeiDanNo = TKD_Haisha.TeiDanNo
                                                        AND TKD_Shabni.BunkRen = TKD_Haisha.BunkRen
                                                        AND TKD_Haisha.SiyoKbn = 1
                                LEFT JOIN TKD_Unkobi ON TKD_Shabni.UkeNo = TKD_Unkobi.UkeNo
                                                        AND TKD_Shabni.UnkRen = TKD_Unkobi.UnkRen
                                                        AND TKD_Unkobi.SiyoKbn = 1
                                LEFT JOIN TKD_YykSho ON TKD_Shabni.UkeNo = TKD_YykSho.UkeNo
                                                        AND TKD_YykSho.SiyoKbn = 1
														AND TKD_YykSho.TenantCdSeq = @TenantCdSeq
                                LEFT JOIN VPM_YoyKbn ON TKD_YykSho.YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_SyaRyo ON TKD_Haisha.HaiSSryCdSeq = VPM_SyaRyo.SyaRyoCdSeq
                                LEFT JOIN VPM_SyaSyu ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
                                                        AND VPM_SyaSyu.SiyoKbn = 1
														AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq
                                LEFT JOIN VPM_HenSya ON VPM_SyaRyo.SyaRyoCdSeq = VPM_HenSya.SyaRyoCdSeq
                                                        AND VPM_HenSya.StaYmd <= TKD_Haisha.SyuKoYmd
                                                        AND VPM_HenSya.ENDYmd >= TKD_Haisha.SyuKoYmd
                                LEFT JOIN VPM_Eigyos ON VPM_HenSya.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
                                                        AND VPM_Eigyos.SiyoKbn = 1
                                LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
                                                        AND VPM_Compny.SiyoKbn = 1
                                LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
                                                        AND VPM_Tenant.SiyoKbn = 1
                           WHERE TKD_Shabni.UnkYmd >= @StrDate
                                 AND TKD_Shabni.UnkYmd <= @EndDate
                                 AND VPM_YoyKbn.UnsouKbn = @UnsoKbn
                                 AND VPM_SyaRyo.NinkaKbn IN
                           (
                               SELECT NinkaKbn
                               FROM @InString
                               WHERE [Type] = 2
                           )
                                 AND TKD_Shabni.SiyoKbn = 1
                                 AND VPM_Compny.CompanyCdSeq = @CompanyCdSeq
                                 AND VPM_Tenant.TenantCdSeq = @TenantCdSeq
                                 AND TKD_YykSho.YoyaSyu = 1
                       ) AS Sub
                       GROUP BY UnkYmd, 
                                SyaRyoCdSeq, 
                                EigyoCdSeq, 
                                KataKbn, 
                                NenRyoKbn
                   ) AS Main
                   GROUP BY Main.EigyoCdSeq, 
                            Main.KataKbn, 
                            Main.NenRyoKbn;
            INSERT INTO @JituTable
                   SELECT EigyoCdSeq, 
                          KataKbn, 
                          NenRyoKbn, 
                          COUNT(EigyoCdSeq) AS Cnt, 
                          SUM(JisaKm) AS JisaKm, 
                          SUM(KisoKm) AS KisoKm, 
                          SUM(Jinin) AS Jinin
                   FROM
                   (
                       SELECT UnkYmd, 
                              SyaRyoCdSeq, 
                              EigyoCdSeq, 
                              KataKbn, 
                              NenRyoKbn, 
                              SUM(JisaKm) AS JisaKm, 
                              SUM(KisoKm) AS KisoKm, 
                              SUM(CASE @YusoJinKbn
                                      WHEN 0
                                      THEN Jinin
                                      WHEN 1
                                      THEN NobeJinin
                                      ELSE 0
                                  END) AS Jinin
                       FROM
                       (
                           SELECT TKD_Shabni.UnkYmd, 
                                  VPM_SyaRyo.SyaRyoCdSeq, 
                                  VPM_HenSya.EigyoCdSeq, 
                                  VPM_SyaSyu.KataKbn, 
                                  VPM_SyaSyu.NenRyoKbn, 
                                  (TKD_Shabni.JisaIPKm + TKD_Shabni.JisaKSKm) AS JisaKm, 
                                  (TKD_Shabni.KisoIPkm + TKD_Shabni.KisoKOKm) AS KisoKm, 
                                  ISNULL(TKD_Shabni2.JyoSyaJin, 0) AS Jinin, 
                                  TKD_Shabni.JyoSyaJin AS NobeJinin
                           FROM TKD_Shabni
                                LEFT JOIN
                           (
                               SELECT UkeNo, 
                                      UnkRen, 
                                      TeiDanNo, 
                                      BunkRen,
                                      CASE
                                          WHEN
                               (
                                   SELECT HoukoKbn
                                   FROM TKM_KasSet
                                   WHERE CompanyCdSeq = @CompanyCdSeq
                               ) = 1
                                          THEN MIN(UnkYmd)
                                          ELSE MAX(UnkYmd)
                                      END AS UnkYmd, 
                                      MAX(JyoSyaJin) AS JyoSyaJin
                               FROM TKD_Shabni
                               WHERE SiyoKbn = 1
                               GROUP BY UkeNo, 
                                        UnkRen, 
                                        TeiDanNo, 
                                        BunkRen
                           ) AS TKD_Shabni2 ON TKD_Shabni.UkeNo = TKD_Shabni2.UkeNo
                                               AND TKD_Shabni.UnkRen = TKD_Shabni2.UnkRen
                                               AND TKD_Shabni.TeiDanNo = TKD_Shabni2.TeiDanNo
                                               AND TKD_Shabni.BunkRen = TKD_Shabni2.BunkRen
                                               AND TKD_Shabni.UnkYmd = TKD_Shabni2.UnkYmd
                                LEFT JOIN TKD_Haisha ON TKD_Shabni.UkeNo = TKD_Haisha.UkeNo
                                                        AND TKD_Shabni.UnkRen = TKD_Haisha.UnkRen
                                                        AND TKD_Shabni.TeiDanNo = TKD_Haisha.TeiDanNo
                                                        AND TKD_Shabni.BunkRen = TKD_Haisha.BunkRen
                                                        AND TKD_Haisha.SiyoKbn = 1
                                LEFT JOIN TKD_Unkobi ON TKD_Shabni.UkeNo = TKD_Unkobi.UkeNo
                                                        AND TKD_Shabni.UnkRen = TKD_Unkobi.UnkRen
                                                        AND TKD_Unkobi.SiyoKbn = 1
                                LEFT JOIN TKD_YykSho ON TKD_Shabni.UkeNo = TKD_YykSho.UkeNo
                                                        AND TKD_YykSho.SiyoKbn = 1
														AND TKD_YykSho.TenantCdSeq = @TenantCdSeq
                                LEFT JOIN VPM_YoyKbn ON TKD_YykSho.YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq
                                LEFT JOIN VPM_SyaRyo ON TKD_Haisha.HaiSSryCdSeq = VPM_SyaRyo.SyaRyoCdSeq
                                LEFT JOIN VPM_SyaSyu ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
                                                        AND VPM_SyaSyu.SiyoKbn = 1
														AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq
                                LEFT JOIN VPM_HenSya ON VPM_SyaRyo.SyaRyoCdSeq = VPM_HenSya.SyaRyoCdSeq
                                                        AND VPM_HenSya.StaYmd <= TKD_Haisha.SyuKoYmd
                                                        AND VPM_HenSya.ENDYmd >= TKD_Haisha.SyuKoYmd
                                LEFT JOIN VPM_Eigyos ON VPM_HenSya.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
                                                        AND VPM_Eigyos.SiyoKbn = 1
                                LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
                                                        AND VPM_Compny.SiyoKbn = 1
                                LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
                                                        AND VPM_Tenant.SiyoKbn = 1
                           WHERE TKD_Shabni.UnkYmd >= @StrDate
                                 AND TKD_Shabni.UnkYmd <= @EndDate
                                 AND VPM_YoyKbn.UnsouKbn = @UnsoKbn
                                 AND VPM_SyaRyo.NinkaKbn IN
                           (
                               SELECT NinkaKbn
                               FROM @InString
                               WHERE [Type] = 1
                           )
                                 AND TKD_Shabni.SiyoKbn = 1
                                 AND VPM_Compny.CompanyCdSeq = @CompanyCdSeq
                                 AND VPM_Tenant.TenantCdSeq = @TenantCdSeq
                                 AND TKD_YykSho.YoyaSyu = 1
                       ) AS Sub
                       GROUP BY UnkYmd, 
                                SyaRyoCdSeq, 
                                EigyoCdSeq, 
                                KataKbn, 
                                NenRyoKbn
                   ) AS Main
                   GROUP BY Main.EigyoCdSeq, 
                            Main.KataKbn, 
                            Main.NenRyoKbn;

							WITH FUTTUM01
AS (
	SELECT Haisha.UkeNo
		,Haisha.Unkren
		,Haisha.TeiDanNo
		,Haisha.BunkRen
		,FutTum.FutTumKbn
		,FutTum.FutTumCdSeq
		,ISNULL(Futai.FutGuiKbn, 6) AS FutGuiKbn
		,FutTum.FutTumNm
		,FutTum.Zeiritsu
		,FutTum.TesuRitu
		,FLOOR((FutTum.UriGakKin + FutTum.SyaRyoSyo) / DaiSu.SumDaiSu) AS ZeiKomi
		,(FutTum.UriGakKin + FutTum.SyaRyoSyo) % DaiSu.SumDaiSu AS ZeiKomiHasu
		,FLOOR((FutTum.UriGakKin) / DaiSu.SumDaiSu) AS UriGakKin
		,(FutTum.UriGakKin) % DaiSu.SumDaiSu AS UriGakKinHasu
		,FLOOR((FutTum.SyaRyoTes) / DaiSu.SumDaiSu) AS TesuRyo
		,(FutTum.SyaRyoTes) % DaiSu.SumDaiSu AS TesuHasu
		,DaiSu.SumDaiSu
		,DENSE_RANK() OVER (
			PARTITION BY HAISHA.UkeNo
			,HAISHA.UnkRen ORDER BY HAISHA.YouTblSeq
				,HAISHA.TeiDanNo
				,HAISHA.BunkRen
			) AS DenseRank
	FROM TKD_Haisha AS Haisha
	INNER JOIN (
		SELECT UkeNo
			,UnkRen
			,FutTumKbn
			,FutTumCdSeq
			,MAX(FutTumNm) AS FutTumNm
			,MAX(Zeiritsu) AS Zeiritsu
			,MAX(TesuRitu) AS TesuRitu
			,sum(CONVERT(bigint, UriGakKin - ISNULL(MFutTu.MUriGakKin, 0))) AS UriGakKin
			,sum(CONVERT(bigint, SyaRyoSyo - ISNULL(MFutTu.MSyaRyoSyo, 0))) AS SyaRyoSyo
			,sum(CONVERT(bigint, SyaRyoTes - ISNULL(MFutTu.MSyaRyoTes, 0))) AS SyaRyoTes
		FROM TKD_FutTum
		LEFT JOIN (
			SELECT UkeNo AS MUkeNo
				,UnkRen AS MUnkRen
				,FutTumKbn AS MFutTumKbn
				,FutTumRen AS MFutTumRen
				,sum(CONVERT(bigint, UriGakKin)) AS MUriGakKin
				,sum(CONVERT(bigint, SyaRyoSyo)) AS MSyaRyoSyo
				,sum(CONVERT(bigint, SyaRyoTes)) AS MSyaRyoTes
				,sum(CONVERT(bigint, UriGakKin + SyaRyoSyo)) AS MZeiKomiKin
			FROM TKD_MFutTu
			WHERE SiyoKbn = 1
				AND Suryo > 0
				AND SUBSTRING(UkeNo,  1,  5)   =  FORMAT(@TenantCdSeq,   '00000')
			GROUP BY UkeNo
				,UnkRen
				,FutTumKbn
				,FutTumRen
			) AS MFutTu ON MFutTu.MUkeNo = TKD_FutTum.UkeNo
			AND MFutTu.MUnkRen = TKD_FutTum.UnkRen
			AND MFutTu.MFutTumKbn = TKD_FutTum.FutTumKbn
			AND MFutTu.MFutTumRen = TKD_FutTum.FutTumRen
		WHERE TKD_FutTum.SiyoKbn = 1
			AND SUBSTRING(TKD_FutTum.UkeNo,  1,  5)   =  FORMAT(@TenantCdSeq,   '00000')
		GROUP BY UkeNo
			,UnkRen
			,FutTumKbn
			,FutTumCdSeq
		) AS FutTum ON Haisha.UkeNo = FutTum.UkeNo
		AND Haisha.UnkRen = FutTum.UnkRen
	LEFT JOIN VPM_Futai AS Futai ON FutTum.FutTumCdSeq = Futai.FutaiCdSeq
		AND Futai.TenantCdSeq = @TenantCdSeq
	LEFT JOIN (
		SELECT Haisha.UkeNo
			,Haisha.UnkRen
			,COUNT(Haisha.UkeNo) AS SumDaiSu
		FROM TKD_Haisha AS Haisha
		LEFT JOIN TKD_Yyksho AS Yyksho ON Haisha.UkeNo = Yyksho.UkeNo AND Yyksho.TenantCdSeq = @TenantCdSeq
		WHERE Yyksho.YoyaSyu = 1
			AND Haisha.SiyoKbn = 1
			AND Yyksho.TenantCdSeq = @TenantCdSeq
		GROUP BY Haisha.UkeNo
			,Haisha.UnkRen
		) DaiSu ON FutTum.UkeNo = DaiSu.UkeNo
		AND FutTum.UnkRen = DaiSu.UnkRen
	WHERE Haisha.SiyoKbn = 1
	)
	,FUTTUM02
AS (
	SELECT FT01.UkeNo
		,FT01.Unkren
		,FT01.TeiDanNo
		,FT01.BunkRen
		,FT01.FutTumKbn
		,FT01.FutTumCdSeq
		,FT01.FutGuiKbn
		,FT01.FutTumNm
		,FT01.Zeiritsu
		,FT01.TesuRitu
		,FT01.ZeiKomi
		,CASE WHEN FT01.DenseRank <= FT01.ZeiKomiHasu THEN 1 ELSE 0 END AS ZeiKomiHasu
		,FT01.UriGakKin
		,CASE WHEN FT01.DenseRank <= FT01.UriGakKinHasu THEN 1 ELSE 0 END AS UriGakKinHasu
		,FT01.TesuRyo
		,CASE WHEN FT01.DenseRank <= FT01.TesuHasu THEN 1 ELSE 0 END AS TesuHasu
	FROM FUTTUM01 AS FT01
	)
	,FUTTUM03
AS (
	SELECT FT02.UkeNo
		,FT02.Unkren
		,FT02.TeiDanNo
		,FT02.BunkRen
		,FT02.FutTumKbn
		,FT02.FutTumCdSeq
		,FT02.FutGuiKbn
		,MAX(FT02.FutTumNm) AS FutTumNm
		,MAX(FT02.TesuRitu) AS TesuRitu
		,sum(CONVERT(bigint, FT02.UriGakKin + FT02.UriGakKinHasu)) AS UriGakKin
		,sum(CONVERT(bigint, (FT02.ZeiKomi + FT02.ZeiKomiHasu) - (FT02.UriGakKin + FT02.UriGakKinHasu))) AS SyaRyoSyo
		,sum(CONVERT(bigint, (FT02.TesuRyo + FT02.TesuHasu))) AS SyaRyoTes
	FROM FUTTUM02 AS FT02
	GROUP BY FT02.UkeNo
		,FT02.UnkRen
		,FT02.TeiDanNo
		,FT02.BunkRen
		,FT02.FutTumKbn
		,FT02.FutTumCdSeq
		,FT02.FutGuiKbn
	)
	,MFUTTU01
AS (
	SELECT MFutTu.UkeNo
		,MFutTu.UnkRen
		,MFutTu.TeiDanNo
		,MFutTu.BunkRen
		,MFutTu.FutTumKbn
		,sum(CONVERT(bigint, MFutTu.UriGakKin)) AS UriGakKin
		,sum(CONVERT(bigint, MFutTu.SyaRyoSyo)) AS SyaRyoSyo
		,sum(CONVERT(bigint, MFutTu.SyaRyoTes)) AS SyaRyoTes
		,FutTum.FutTumCdSeq
	FROM TKD_MFutTu AS MFutTu
	LEFT JOIN TKD_FutTum AS FutTum ON MFutTu.UkeNo = FutTum.UkeNo
		AND MFutTu.UnkRen = FutTum.UnkRen
		AND MFutTu.FutTumKbn = FutTum.FutTumKbn
		AND MFutTu.FutTumRen = FutTum.FutTumRen
		AND SUBSTRING(FutTum.UkeNo,  1,  5)   =  FORMAT(@TenantCdSeq,   '00000')
	WHERE MFutTu.Suryo > 0
		AND MFutTu.SiyoKbn = 1
		AND SUBSTRING(MFutTu.UkeNo,  1,  5)   =  FORMAT(@TenantCdSeq,   '00000')
	GROUP BY MFutTu.UkeNo
		,MFutTu.UnkRen
		,MFutTu.TeiDanNo
		,MFutTu.BunkRen
		,MFutTu.FutTumKbn
		,FutTum.FutTumCdSeq
	), VK_HaishaFutTum2  AS (
	SELECT HAISHA.UkeNo
	,HAISHA.UnkRen
	,HAISHA.TeiDanNo
	,HAISHA.BunkRen
	,YYKSHO.SeiTaiYmd
	,HAISHA.SyukoYmd
	,HAISHA.HaiSYmd
	,HAISHA.TouYmd
	,HAISHA.KikYmd
	,HAISHA.YouTblSeq
	,HAISHA.SyuEigCdSeq
	,HAISHA.HaiSSryCdSeq
	,ISNULL(EIGYOS01.EigyoCdSeq, 0) AS HaiSSryEigyoCdSeq
	,ISNULL(FUTTUM.FutTumKbn, 0) AS FutTumKbn
	,ISNULL(FUTTUM.FutTumCdSeq, 0) AS FutTumCdSeq
	,ISNULL(FUTTUM.FutTumNm, '') AS FutTumNm
	,ISNULL(FUTTUM.FutGuiKbn, 0) AS FutGuiKbn
	,ISNULL(FUTTUM.UriGakKin, 0) + ISNULL(MFUTTU.UriGakKin, 0) AS UriGakKin
	,ISNULL(FUTTUM.SyaRyoSyo, 0) + ISNULL(MFUTTU.SyaRyoSyo, 0) AS SyaRyoSyo
	,ISNULL(FUTTUM.SyaRyoTes, 0) + ISNULL(MFUTTU.SyaRyoTes, 0) AS SyaRyoTes
	,ISNULL(FUTTUM.TesuRitu, 0) AS TesuRitu
FROM TKD_Haisha AS HAISHA
INNER JOIN FUTTUM03 AS FUTTUM ON HAISHA.UkeNo = FUTTUM.UkeNo
	AND HAISHA.UnkRen = FUTTUM.UnkRen
	AND HAISHA.TeiDanNo = FUTTUM.TeiDanNo
	AND HAISHA.BunkRen = FUTTUM.BunkRen
LEFT JOIN MFUTTU01 AS MFUTTU ON HAISHA.UkeNo = MFUTTU.UkeNo
	AND HAISHA.UnkRen = MFUTTU.UnkRen
	AND HAISHA.TeiDanNo = MFUTTU.TeiDanNo
	AND HAISHA.BunkRen = MFUTTU.BunkRen
	AND FUTTUM.FutTumCdSeq = MFUTTU.FutTumCdSeq
	AND FUTTUM.FutTumKbn = MFUTTU.FutTumKbn
LEFT JOIN TKD_Yyksho AS YYKSHO ON HAISHA.UkeNo = YYKSHO.UkeNo AND YYKSHO.TenantCdSeq = @TenantCdSeq
LEFT JOIN VPM_HenSya AS HENSYA01 ON HAISHA.HaiSSryCdSeq = HENSYA01.SyaRyoCdSeq
	AND YYKSHO.SeiTaiYmd BETWEEN HENSYA01.StaYmd AND HENSYA01.EndYmd
LEFT JOIN VPM_Eigyos AS EIGYOS01 ON HENSYA01.EigyoCdSeq = EIGYOS01.EigyoCdSeq
LEFT JOIN VPM_Tenant AS TENANT01 ON YYKSHO.TenantCdSeq = TENANT01.TenantCdSeq
	AND TENANT01.SiyoKbn = 1
WHERE YYKSHO.YoyaSyu = 1
	AND HAISHA.SiyoKbn = 1
	AND TENANT01.TenantCdSeq = @TenantCdSeq
	)
            INSERT INTO @UnkoTable
                   SELECT EigyoCdSeq, 
                          KataKbn, 
                          NenRyoKbn, 
                          SUM(CONVERT(bigint, UnkKai)) AS Cnt, 
                          SUM(CONVERT(bigint, Unchin)) AS Unchin, 
                          SUM(CONVERT(bigint, Zasyu)) AS UnsoZaSyu, 
                          SUM(CONVERT(bigint, Kikak1)) AS UnkoKikak1Cnt, 
                          SUM(CONVERT(bigint, Oth)) AS UnkoOthCnt
                   FROM
                   (
                       SELECT VPM_SyaRyo.SyaRyoCdSeq, 
                              VPM_HenSya.EigyoCdSeq, 
                              VPM_SyaSyu.KataKbn, 
                              VPM_SyaSyu.NenRyoKbn, 
                              (TKD_Haisha.SyaRyoUnc + CASE @UnkinKbn
                                                          WHEN 1
                                                          THEN TKD_Haisha.SyaRyoSyo
                                                          WHEN 2
                                                          THEN 0
                                                          ELSE 0
                                                      END + CASE @UnsoSyuKbn_GuideRyo
                                                                WHEN 1
                                                                THEN ISNULL(JT_Guide.UriGakKin, 0) + CASE @UnkinKbn
                                                                                                         WHEN 1
                                                                                                         THEN ISNULL(JT_Guide.SyaRyoSyo, 0)
                                                                                                         ELSE 0
                                                                                                     END
                                                                ELSE 0
                                                            END) AS Unchin, 
                              (CASE @FutaiSyu1Flg
                                   WHEN 1
                                   THEN ISNULL(JT_Futai.Futai, 0)
                                   ELSE 0
                               END + CASE @FutaiSyu2Flg
                                         WHEN 1
                                         THEN ISNULL(JT_Futai.Tukoryo, 0)
                                         ELSE 0
                                     END + CASE @FutaiSyu3Flg
                                               WHEN 1
                                               THEN ISNULL(JT_Futai.Tehairyo, 0)
                                               ELSE 0
                                           END + CASE @FutaiSyu4Flg
                                                     WHEN 1
                                                     THEN ISNULL(JT_Futai.GuideRyo, 0)
                                                     ELSE 0
                                                 END) AS Zasyu, 
                              (CASE VPM_Gyosya.GyosyaKbn
                                   WHEN 1
                                   THEN CASE VPM_YoyKbn.Yoyagamsyu
                                            WHEN 2
                                            THEN 1
                                            ELSE 0
                                        END
                                   ELSE 0
                               END) AS Kikak1, 
                              (CASE VPM_Gyosya.GyosyaKbn
                                   WHEN 1
                                   THEN CASE VPM_YoyKbn.Yoyagamsyu
                                            WHEN 2
                                            THEN 0
                                            ELSE 1
                                        END
                                   ELSE 0
                               END) AS Oth, 
                              (CASE
                                   WHEN TKD_Shabni.UnkKai IS NULL
                                   THEN 1
                                   ELSE TKD_Shabni.UnkKai
                               END) AS UnkKai
                       FROM TKD_Haisha
                            LEFT JOIN TKD_Unkobi ON TKD_Haisha.UkeNo = TKD_Unkobi.UkeNo
                                                    AND TKD_Haisha.UnkRen = TKD_Unkobi.UnkRen
                                                    AND TKD_Unkobi.SiyoKbn = 1
                            LEFT JOIN TKD_Shabni ON TKD_Haisha.UkeNo = TKD_Shabni.UkeNo
                                                    AND TKD_Haisha.UnkRen = TKD_Shabni.TeiDanNo
                                                    AND TKD_Haisha.TeiDanNo = TKD_Shabni.TeiDanNo
                                                    AND TKD_Haisha.BunkRen = TKD_Shabni.BunkRen
                            LEFT JOIN TKD_YykSho ON TKD_Haisha.UkeNo = TKD_YykSho.UkeNo
                                                    AND TKD_YykSho.SiyoKbn = 1
													AND TKD_YykSho.TenantCdSeq = @TenantCdSeq
                            LEFT JOIN VPM_Tokisk ON TKD_YykSho.TokuiSeq = VPM_Tokisk.TokuiSeq
                                                    AND VPM_Tokisk.SiyoStaYmd <= TKD_YykSho.SeiTaiYmd
                                                    AND VPM_Tokisk.SiyoEndYmd >= TKD_YykSho.SeiTaiYmd
                            LEFT JOIN VPM_Gyosya ON VPM_Tokisk.GyosyaCdSeq = VPM_Gyosya.GyosyaCdSeq
                                                    AND VPM_Gyosya.SiyoKbn = 1
                            LEFT JOIN VPM_YoyKbn ON TKD_YykSho.YoyaKbnSeq = VPM_YoyKbn.YoyaKbnSeq
                            LEFT JOIN VPM_SyaRyo ON TKD_Haisha.HaiSSryCdSeq = VPM_SyaRyo.SyaRyoCdSeq
                            LEFT JOIN VPM_SyaSyu ON VPM_SyaRyo.SyaSyuCdSeq = VPM_SyaSyu.SyaSyuCdSeq
                                                    AND VPM_SyaSyu.SiyoKbn = 1
													AND VPM_SyaSyu.TenantCdSeq = @TenantCdSeq
                            LEFT JOIN VPM_HenSya ON VPM_SyaRyo.SyaRyoCdSeq = VPM_HenSya.SyaRyoCdSeq
                                                    AND VPM_HenSya.StaYmd <= TKD_Haisha.SyuKoYmd
                                                    AND VPM_HenSya.EndYmd >= TKD_Haisha.SyuKoYmd
                            LEFT JOIN VPM_Eigyos ON VPM_HenSya.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
                                                    AND VPM_Eigyos.SiyoKbn = 1
                            LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
                                                    AND VPM_Compny.SiyoKbn = 1
                            LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
                                                    AND VPM_Tenant.SiyoKbn = 1
                            LEFT JOIN
                       (
                           SELECT UkeNo, 
                                  UnkRen, 
                                  TeiDanNo, 
                                  BunkRen, 
                                  SUM(CONVERT(bigint, Futai)) Futai, 
                                  SUM(CONVERT(bigint, Tukoryo)) Tukoryo, 
                                  SUM(CONVERT(bigint, Tehairyo)) Tehairyo, 
                                  SUM(CONVERT(bigint, GuideRyo)) GuideRyo
                           FROM
                           (
                               SELECT TKD_MFutTu.UkeNo, 
                                      TKD_MFutTu.UnkRen, 
                                      TKD_MFutTu.TeiDanNo, 
                                      TKD_MFutTu.BunkRen,
                                      CASE VPM_Futai.FutGuiKbn
                                          WHEN 2
                                          THEN TKD_MFutTu.UriGakKin + CASE @UnkinKbn
                                                                          WHEN 1
                                                                          THEN TKD_MFutTu.SyaRyoSyo
                                                                          WHEN 2
                                                                          THEN 0
                                                                          ELSE 0
                                                                      END
                                          ELSE 0
                                      END AS Futai,
                                      CASE VPM_Futai.FutGuiKbn
                                          WHEN 3
                                          THEN TKD_MFutTu.UriGakKin + CASE @UnkinKbn
                                                                          WHEN 1
                                                                          THEN TKD_MFutTu.SyaRyoSyo
                                                                          WHEN 2
                                                                          THEN 0
                                                                          ELSE 0
                                                                      END
                                          ELSE 0
                                      END AS Tukoryo,
                                      CASE VPM_Futai.FutGuiKbn
                                          WHEN 4
                                          THEN TKD_MFutTu.UriGakKin + CASE @UnkinKbn
                                                                          WHEN 1
                                                                          THEN TKD_MFutTu.SyaRyoSyo
                                                                          WHEN 2
                                                                          THEN 0
                                                                          ELSE 0
                                                                      END
                                          ELSE 0
                                      END AS Tehairyo,
                                      CASE VPM_Futai.FutGuiKbn
                                          WHEN 5
                                          THEN TKD_MFutTu.UriGakKin + CASE @UnkinKbn
                                                                          WHEN 1
                                                                          THEN TKD_MFutTu.SyaRyoSyo
                                                                          WHEN 2
                                                                          THEN 0
                                                                          ELSE 0
                                                                      END
                                          ELSE 0
                                      END AS GuideRyo
                               FROM TKD_MFutTu
                                    LEFT JOIN TKD_FutTum ON TKD_MFutTu.UkeNo = TKD_FutTum.UkeNo
                                                            AND TKD_MFutTu.UnkRen = TKD_FutTum.UnkRen
                                                            AND TKD_MFutTu.FutTumKbn = TKD_FutTum.FutTumKbn
                                                            AND TKD_MFutTu.FutTumRen = TKD_FutTum.FutTumRen
                                                            AND TKD_MFutTu.Suryo <> 0
                                                            AND TKD_MFutTu.SiyoKbn = 1
                                    LEFT JOIN VPM_Futai ON TKD_FutTum.FutTumCdSeq = VPM_Futai.FutaiCdSeq AND VPM_Futai.TenantCdSeq = @TenantCdSeq
                                    LEFT JOIN VPM_Tenant ON VPM_Futai.TenantCdSeq = VPM_Tenant.TenantCdSeq
                                                            AND VPM_Tenant.SiyoKbn = 1
                               WHERE TKD_MFutTu.SiyoKbn = 1
                                     AND TKD_MFutTu.FutTumKbn = 1
                                     AND VPM_Tenant.TenantCdSeq = @TenantCdSeq
                           ) AS Main
                           GROUP BY UkeNo, 
                                    UnkRen, 
                                    TeiDanNo, 
                                    BunkRen
                       ) AS JT_Futai ON TKD_Haisha.UkeNo = JT_Futai.UkeNo
                                        AND TKD_Haisha.UnkRen = JT_Futai.UnkRen
                                        AND TKD_Haisha.TeiDanNo = JT_Futai.TeiDanNo
                                        AND TKD_Haisha.BunkRen = JT_Futai.BunkRen
                            LEFT JOIN
                       (
                           SELECT UkeNo, 
                                  UnkRen, 
                                  TeiDanNo, 
                                  BunkRen, 
                                  SUM(CONVERT(bigint, UriGakKin)) AS UriGakKin, 
                                  SUM(CONVERT(bigint, SyaRyoSyo)) AS SyaRyoSyo
                           FROM VK_HaishaFutTum2 AS HaishaFutTum
                           WHERE HaishaFutTum.FutGuiKbn = 5
                                 AND SUBSTRING(HaishaFutTum.UkeNo, 1, 5) = FORMAT(@TenantCdSeq, '00000') 
                                 AND HaishaFutTum.SyuKoYmd >= CASE @UnkoKbn
                                                                  WHEN 1
                                                                  THEN @StrDate
                                                                  WHEN 2
                                                                  THEN HaishaFutTum.SyuKoYmd
                                                                  ELSE HaishaFutTum.SyuKoYmd
                                                              END
                                 AND HaishaFutTum.SyuKoYmd <= CASE @UnkoKbn
                                                                  WHEN 1
                                                                  THEN @EndDate
                                                                  WHEN 2
                                                                  THEN HaishaFutTum.SyuKoYmd
                                                                  ELSE HaishaFutTum.SyuKoYmd
                                                              END
                                 AND HaishaFutTum.KikYmd >= CASE @UnkoKbn
                                                                WHEN 1
                                                                THEN HaishaFutTum.KikYmd
                                                                WHEN 2
                                                                THEN @StrDate
                                                                ELSE HaishaFutTum.KikYmd
                                                            END
                                 AND HaishaFutTum.KikYmd <= CASE @UnkoKbn
                                                                WHEN 1
                                                                THEN HaishaFutTum.KikYmd
                                                                WHEN 2
                                                                THEN @EndDate
                                                                ELSE HaishaFutTum.KikYmd
                                                            END
                           GROUP BY UkeNo, 
                                    UnkRen, 
                                    TeiDanNo, 
                                    BunkRen
                       ) AS JT_Guide ON TKD_Haisha.UkeNo = JT_Guide.UkeNo
                                        AND TKD_Haisha.UnkRen = JT_Guide.UnkRen
                                        AND TKD_Haisha.TeiDanNo = JT_Guide.TeiDanNo
                                        AND TKD_Haisha.BunkRen = JT_Guide.BunkRen
                       WHERE TKD_Haisha.SiyoKbn = 1
                             AND TKD_Haisha.SyuKoYmd >= CASE @UnkoKbn
                                                            WHEN 1
                                                            THEN @StrDate
                                                            WHEN 2
                                                            THEN TKD_Haisha.SyuKoYmd
                                                            ELSE TKD_Haisha.SyuKoYmd
                                                        END
                             AND TKD_Haisha.SyuKoYmd <= CASE @UnkoKbn
                                                            WHEN 1
                                                            THEN @EndDate
                                                            WHEN 2
                                                            THEN TKD_Haisha.SyuKoYmd
                                                            ELSE TKD_Haisha.SyuKoYmd
                                                        END
                             AND TKD_Haisha.KikYmd >= CASE @UnkoKbn
                                                          WHEN 1
                                                          THEN TKD_Haisha.KikYmd
                                                          WHEN 2
                                                          THEN @StrDate
                                                          ELSE TKD_Haisha.KikYmd
                                                      END
                             AND TKD_Haisha.KikYmd <= CASE @UnkoKbn
                                                          WHEN 1
                                                          THEN TKD_Haisha.KikYmd
                                                          WHEN 2
                                                          THEN @EndDate
                                                          ELSE TKD_Haisha.KikYmd
                                                      END
                             AND VPM_YoyKbn.UnsouKbn = @UnsoKbn
                             AND VPM_Compny.CompanyCdSeq = @CompanyCdSeq
                             AND VPM_Tenant.TenantCdSeq = @TenantCdSeq
                             AND VPM_SyaRyo.NinkaKbn <> 7
                             AND TKD_Haisha.SiyoKbn = 1
                             AND TKD_YykSho.YoyaSyu = 1
                             AND (TKD_Haisha.KSKbn <> 1
                                  OR TKD_Haisha.HaiSKbn = 2)
                   ) AS Main
                   GROUP BY EigyoCdSeq, 
                            KataKbn, 
                            NenRyoKbn;
            DELETE TKD_JitHou
            FROM TKD_JitHou
                 LEFT JOIN VPM_Eigyos ON TKD_JitHou.EigyoCdSeq = VPM_Eigyos.EigyoCdSeq
                                         AND VPM_Eigyos.SiyoKbn = 1
                 LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
                                         AND VPM_Compny.SiyoKbn = 1
                 LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
                                         AND VPM_Tenant.SiyoKbn = 1
            WHERE TKD_JitHou.UnsouKbn = @UnsoKbn
                  AND TKD_JitHou.SyoriYm = dbo.FP_RpZero(4, YEAR(CONVERT(DATETIME, @StrDate))) + dbo.FP_RpZero(2, MONTH(CONVERT(DATETIME, @StrDate)))
            AND VPM_Compny.CompanyCdSeq = @CompanyCdSeq
            AND VPM_Tenant.TenantCdSeq = @TenantCdSeq
            AND VPM_Eigyos.EigyoCd >= CASE RTRIM(LTRIM(@StrEigyoCd))
                                          WHEN ''
                                          THEN VPM_Eigyos.EigyoCd
                                          ELSE @StrEigyoCd
                                      END
            AND VPM_Eigyos.EigyoCd <= CASE RTRIM(LTRIM(@EndEigyoCd))
                                          WHEN ''
                                          THEN VPM_Eigyos.EigyoCd
                                          ELSE @EndEigyoCd
                                      END;
            INSERT INTO TKD_JitHou
                   SELECT @UnsoKbn AS UnsoKbn, 
                          dbo.FP_RpZero(4, YEAR(CONVERT(DATETIME, @StrDate))) + dbo.FP_RpZero(2, MONTH(CONVERT(DATETIME, @StrDate))) AS SyoriYm, 
                          VPM_Eigyos.EigyoCdSeq, 
                          KataNenryo.KataKbn, 
                          KataNenryo.NenRyoKbn, 
                          ISNULL(JyoSumTable.Cnt, 0) AS NobeJyoCnt, 
                          ISNULL(RinjiTable.Cnt, 0) AS NobeRinCnt, 
                          ISNULL(JyoSumTable.Cnt, 0) + ISNULL(RinjiTable.Cnt, 0) AS NobeSumCnt, 
                          ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0) AS NobeJitCnt,
                          CASE ISNULL(JyoSumTable.Cnt, 0) + ISNULL(RinjiTable.Cnt, 0)
                              WHEN 0
                              THEN 0.0
                              ELSE CONVERT(NUMERIC(4, 1), ROUND(CONVERT(NUMERIC, ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0)) / CONVERT(NUMERIC, ISNULL(JyoSumTable.Cnt, 0) + ISNULL(RinjiTable.Cnt, 0)) * 100, 1))
                          END AS JitudoRitu,
                          CASE ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0)
                              WHEN 0
                              THEN 0.0
                              ELSE CONVERT(NUMERIC(4, 1), ROUND(CONVERT(NUMERIC, ISNULL(RinjiTable.Cnt, 0)) / CONVERT(NUMERIC, ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0)) * 100, 1))
                          END AS RinjiRitu, 
                          ISNULL(RinjiTable.JisaKm, 0) + ISNULL(JituTable.JisaKm, 0) AS JitJisoKm, 
                          ISNULL(RinjiTable.KisoKm, 0) + ISNULL(JituTable.KisoKm, 0) AS JitKisoKm, 
                          ISNULL(RinjiTable.Jinin, 0) + ISNULL(JituTable.Jinin, 0) AS YusoJin, 
                          ISNULL(UnkoTable.Cnt, 0) AS UnkoCnt, 
                          ISNULL(UnkoTable.UnkoKikak1Cnt, 0) AS UnkoKikak1Cnt, 
                          0 AS UnkoKikak2Cnt, 
                          ISNULL(UnkoTable.UnkoOthCnt, 0) AS UnkoOthCnt, 
                          ISNULL(UnkoTable.Unchin, 0) AS UnsoSyu,
                          CASE ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0)
                              WHEN 0
                              THEN 0.00
                              ELSE CONVERT(NUMERIC(7, 2), ROUND((CONVERT(NUMERIC, ISNULL(RinjiTable.JisaKm, 0) + ISNULL(JituTable.JisaKm, 0)) + CONVERT(NUMERIC, ISNULL(RinjiTable.KisoKm, 0) + ISNULL(JituTable.KisoKm, 0))) / CONVERT(NUMERIC, ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0)), 2))
                          END AS DayTotalKm,
                          CASE ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0)
                              WHEN 0
                              THEN 0.00
                              ELSE CONVERT(NUMERIC(7, 2), ROUND((CONVERT(NUMERIC, ISNULL(RinjiTable.Jinin, 0)) + ISNULL(JituTable.Jinin, 0)) / (CONVERT(NUMERIC, ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0))), 2))
                          END AS DayYusoJin,
                          CASE ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0)
                              WHEN 0
                              THEN 0
                              ELSE CONVERT(NUMERIC(10, 0), ROUND(CONVERT(NUMERIC, ISNULL(UnkoTable.Unchin, 0)) / (CONVERT(NUMERIC, ISNULL(RinjiTable.Cnt, 0) + ISNULL(JituTable.Cnt, 0))), 2))
                          END AS DayUnsoSyu,
                          CASE ISNULL(UnkoTable.Cnt, 0)
                              WHEN 0
                              THEN 0.00
                              ELSE CONVERT(NUMERIC(7, 2), ROUND(CONVERT(NUMERIC, ISNULL(RinjiTable.JisaKm, 0)) / CONVERT(NUMERIC, ISNULL(UnkoTable.Cnt, 0)), 2))
                          END AS DayJisaKm,
                          CASE @ZaSyuKbn
                              WHEN 1
                              THEN ISNULL(UnkoTable.UnsoZaSyu, 0)
                              ELSE 0
                          END AS UnsoZaSyu, 
                          @sysHiduke AS UpdYmd, 
                          @sysJikan AS UpdTime, 
                          @SyainCdSeq AS UpdSyainCd, 
                          @UpdPrgID AS UpdPrgID
                   FROM VPM_Eigyos
                        LEFT JOIN @KataNenryo AS KataNenryo ON 1 = 1
                        LEFT JOIN @JyoSubTable AS JyoSumTable ON VPM_Eigyos.EigyoCdSeq = JyoSumTable.EigyoCdSeq
                                                                 AND KataNenryo.KataKbn = JyoSumTable.KataKbn
                                                                 AND KataNenryo.NenRyoKbn = JyoSumTable.NenRyoKbn
                        LEFT JOIN @RinjiTable AS RinjiTable ON VPM_Eigyos.EigyoCdSeq = RinjiTable.EigyoCdSeq
                                                               AND KataNenryo.KataKbn = RinjiTable.KataKbn
                                                               AND KataNenryo.NenRyoKbn = RinjiTable.NenRyoKbn
                        LEFT JOIN @JituTable AS JituTable ON VPM_Eigyos.EigyoCdSeq = JituTable.EigyoCdSeq
                                                             AND KataNenryo.KataKbn = JituTable.KataKbn
                                                             AND KataNenryo.NenRyoKbn = JituTable.NenRyoKbn
                        LEFT JOIN @UnkoTable AS UnkoTable ON VPM_Eigyos.EigyoCdSeq = UnkoTable.EigyoCdSeq
                                                             AND KataNenryo.KataKbn = UnkoTable.KataKbn
                                                             AND KataNenryo.NenRyoKbn = UnkoTable.NenRyoKbn
                        LEFT JOIN VPM_Compny ON VPM_Eigyos.CompanyCdSeq = VPM_Compny.CompanyCdSeq
                                                AND VPM_Compny.SiyoKbn = 1
                        LEFT JOIN VPM_Tenant ON VPM_Compny.TenantCdSeq = VPM_Tenant.TenantCdSeq
                                                AND VPM_Tenant.SiyoKbn = 1
                   WHERE VPM_Compny.CompanyCdSeq = @CompanyCdSeq
                         AND VPM_Tenant.TenantCdSeq = @TenantCdSeq
                         AND VPM_Eigyos.EigyoCd >= CASE RTRIM(LTRIM(@StrEigyoCd))
                                                       WHEN ''
                                                       THEN VPM_Eigyos.EigyoCd
                                                       ELSE @StrEigyoCd
                                                   END
                         AND VPM_Eigyos.EigyoCd <= CASE RTRIM(LTRIM(@EndEigyoCd))
                                                       WHEN ''
                                                       THEN VPM_Eigyos.EigyoCd
                                                       ELSE @EndEigyoCd
                                                   END;
        END TRY
        -- エラー処理
        BEGIN CATCH
            THROW;
        END CATCH;
        RETURN;
    END;
GO