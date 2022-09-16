USE HOC_Kashikiri
GO

ALTER TABLE TKD_Haiin
ADD SyokumuKbn TINYINT;
GO

UPDATE TKD_Haiin
SET SyokumuKbn = ISNULL(VPM_Syokum.SyokumuKbn, 0)
FROM TKD_Haiin
LEFT JOIN VPM_KyoSHe ON VPM_KyoSHe.SyainCdSeq = TKD_Haiin.SyainCdSeq
AND FORMAT(GETDATE(),'yyyyMMdd') BETWEEN VPM_KyoSHe.StaYmd AND VPM_KyoSHe.EndYmd
LEFT JOIN VPM_Syokum ON VPM_Syokum.SyokumuCdSeq = VPM_KyoSHe.SyokumuCdSeq
GO

ALTER TABLE TKD_Haiin
ALTER COLUMN SyokumuKbn TINYINT NOT NULL;
GO