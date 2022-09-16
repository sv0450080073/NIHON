USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_KyoSHe]    Script Date: 2020/11/12 13:22:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VPM_KyoSHe]
AS
SELECT                      SyainCdSeq, SyokumuCdSeq, EigyoCdSeq, TenkoNo, StaYmd, EndYmd, NorGroupCdSeq, NorPrintOutNo, YakushokuCdSeq, SyokusyuCdSeq, LegalHoliday, LegalHoliday2, BigTypeDrivingFlg, 
                                      MediumTypeDrivingFlg, SmallTypeDrivingFlg, ExpItem, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID
FROM                         HOC_Master.dbo.TPM_KyoSHe
GO