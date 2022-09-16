USE [HOC_Master]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE Tpm_Compny
ADD TekaSeikyuTouNo VARCHAR(50), ComSealImgUrl VARCHAR(255)
GO

USE [HOC_Kashikiri]
GO
ALTER VIEW dbo.VPM_Compny AS
SELECT TenantCdSeq, CompanyCdSeq, CompanyCd, EigyoCdSeq, CompanyNm, RyakuNm, SyoriYm, KesanM, SimeD, TesuRitu, TokuiSeq, SitenCdSeq, SyutaiEigyoCdSeq, TasyaFlg, BusCHatsuTesu, BusCHatsuTesuZei, SeisanItakuRyo, SyutaiKeitoCdSeq, BusinessPermitDate, 
             BusinessArea, BusinessPermitNumber, VoluntaryInsuranceHuman, VoluntaryInsuranceObject, TekaSeikyuTouNo, ComSealImgUrl, SiyoKbn, UpdYmd, UpdTime, UpdSyainCd, UpdPrgID
FROM   HOC_Master.dbo.TPM_Compny
GO


