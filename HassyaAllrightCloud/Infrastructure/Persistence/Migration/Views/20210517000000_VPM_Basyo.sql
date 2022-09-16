USE [HOC_Kashikiri]
GO

/****** Object:  View [dbo].[VPM_Basyo]    Script Date: 5/17/2021 2:25:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   VIEW [dbo].[VPM_Basyo]
AS

select 
       [TenantCdSeq]
      ,[BasyoKenCdSeq]
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

from HOC_Master..TPM_Basyo

GO


