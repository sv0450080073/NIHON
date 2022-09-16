USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Basyo]    Script Date: 17/05/2021 3:35:28 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create Or Alter VIEW [dbo].[VPM_Basyo]
AS

Select 
[BasyoKenCdSeq]
      ,[BasyoMapCdSeq]
      ,[BasyoMapCd]
      ,[BasyoNm]
      ,[RyakuNm]
      ,[SiyoIkiKbn]
      ,[SiyoKouKbn]
      ,[SiyoTehKbn]
      ,[SiyoAreaKbn]
      ,[SiyoHseiKbn]
      ,[ZipCd]
      ,[Jyus1]
      ,[Jyus2]
      ,[TelNo]
      ,[FaxNo]
      ,[BasyoTanNm]
      ,[BunruiCdSeq]
      ,[TehaiCdSeq]
      ,[SiyoKbn]
      ,[UpdYmd]
      ,[UpdTime]
      ,[UpdSyainCd]
      ,[UpdPrgID]
      ,[TenantCdSeq]
From HOC_Master..TPM_Basyo

GO