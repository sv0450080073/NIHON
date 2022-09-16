USE [HOC_Kashikiri]
GO

CREATE OR ALTER VIEW [dbo].[VPM_KinKyu]
AS
SELECT [KinKyuCdSeq]
	,[KinKyuCd]
	,[KinKyuNm]
	,[RyakuNm]
	,[KinKyuKbn]
	,[ColKinKyu]
	,[KyuSyukinNm]
	,[KyuSyukinRyaku]
	,[DefaultSyukinTime]
	,[DefaultTaiknTime]
	,[KyusyutsuKbn]
	,[SiyoKbn]
	,[UpdYmd]
	,[UpdTime]
	,[UpdSyainCd]
	,[UpdPrgID]
	,[TenantCdSeq]
FROM HOC_Master..TPM_KinKyu
GO

