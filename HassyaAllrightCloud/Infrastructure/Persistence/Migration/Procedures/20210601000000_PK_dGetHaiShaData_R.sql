USE [HOC_Kashikiri]
GO
/****** Object:  StoredProcedure [dbo].[PK_dGetHaiShaData]    Script Date: 03/18/2021 9:58:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

------------------------------------------------------------
-- System-Name	:   HassyaAlrightCloud
-- Module-Name	:   HassyaAlrightCloud
-- SP-ID		:   PK_dGetHaiShaData
-- DB-Name		:   HOC_Kashikiri
-- Name			:   Get Bill Address Receipt
-- Date			:   2021/03/18
-- Author		:   Tra Nguyen
-- Description	:   
------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[PK_dGetHaiShaData]
    (
     -- Parameter
         @UkeNo					VARCHAR(15)             
	 ,   @TenantCdSeq           INT
	 ,   @UnkRen           INT
    )
AS 
SELECT   ISNULL(HAISHA.UkeNo, '') AS HAISHAUkeNo      
        ,ISNULL(YYKSHO.UkeCD, 0)		AS YYKSHOUkeCD           
        ,ISNULL(HAISHA.UnkRen, 0)       AS HAISHAUnkRen    
        ,ISNULL(HAISHA.SyaSyuRen, 0)    AS HAISHASyaSyuRen    
        ,ISNULL(HAISHA.TeiDanNo, 0)      AS HAISHATeiDanNo   
        ,ISNULL(HAISHA.BunKSyuJyn, 0)     AS HAISHABunKSyuJyn  
        ,ISNULL(HAISHA.BunkRen, 0)        AS  HAISHABunkRen  
        ,ISNULL(HAISHA.GoSya, '')          AS HAISHAGoSya  
        ,ISNULL(HAISHA.SyuEigCdSeq, 0)     AS 	HAISHASyuEigCdSeq
        ,ISNULL(HAISHA.HaiSSryCdSeq, 0)    AS HAISHAHaiSSryCdSeq
        ,ISNULL(HAISHA.DanTaNm2, '')        AS HAISHADanTaNm2
        ,ISNULL(HAISHA.IkMapCdSeq, 0)      AS 	HAISHAIkMapCdSeq
        ,ISNULL(HAISHA.IkNm, '')            AS HAISHAIkNm
        ,ISNULL(HAISHA.SyuKoYmd, '')        AS HAISHASyuKoYmd
        ,ISNULL(HAISHA.SyuKoTime, '')       AS HAISHASyuKoTime
        ,ISNULL(HAISHA.SyuPaTime, '')       AS HAISHASyuPaTime
        ,ISNULL(HAISHA.HaiSYmd, '')         AS HAISHAHaiSYmd
        ,ISNULL(HAISHA.HaiSTime, '')        AS HAISHAHaiSTime
        ,ISNULL(HAISHA.HaiSCdSeq, 0)       AS HAISHAHaiSCdSeq
        ,ISNULL(HAISHA.HaiSNm, '')          AS HAISHAHaiSNm
        ,ISNULL(HAISHA.HaiSJyus1, '')       AS HAISHAHaiSJyus1
        ,ISNULL(HAISHA.HaiSJyus2, '')       AS HAISHAHaiSJyus2
        ,ISNULL(HAISHA.HaiSKigou, '')       AS HAISHAHaiSKigou
        ,ISNULL(HAISHA.HaiSKouKCdSeq, 0)   AS HAISHAHaiSKouKCdSeq
        ,ISNULL(HAISHA.HaiSKouKNm, '')      AS HAISHAHaiSKouKNm
        ,ISNULL(HAISHA.HaiSBinCdSeq, 0)    AS HAISHAHaiSBinCdSeq
        ,ISNULL(HAISHA.HaisBinNm, '')       AS HAISHAHaisBinNm
        ,ISNULL(HAISHA.HaiSSetTime, '')     AS HAISHAHaiSSetTime
        ,ISNULL(HAISHA.KikYmd, '')          AS HAISHAKikYmd
        ,ISNULL(HAISHA.KikTime, '')         AS HAISHAKikTime
        ,ISNULL(HAISHA.TouYmd, '')          AS HAISHATouYmd
        ,ISNULL(HAISHA.TouChTime, '')       AS HAISHATouChTime
        ,ISNULL(HAISHA.TouCdSeq, 0)        AS HAISHATouCdSeq
        ,ISNULL(HAISHA.TouNm, '')           AS HAISHATouNm
        ,ISNULL(HAISHA.TouJyusyo1, '')      AS HAISHATouJyusyo1
        ,ISNULL(HAISHA.TouJyusyo2, '')      AS HAISHATouJyusyo2
        ,ISNULL(HAISHA.TouKigou, '')        AS HAISHATouKigou
        ,ISNULL(HAISHA.TouKouKCdSeq, 0)    AS HAISHATouKouKCdSeq
        ,ISNULL(HAISHA.TouSKouKNm, '')      AS HAISHATouSKouKNm
        ,ISNULL(HAISHA.TouBinCdSeq, 0)     AS HAISHATouBinCdSeq
        ,ISNULL(HAISHA.TouBinNm, '')        AS HAISHATouBinNm
        ,ISNULL(HAISHA.TouSetTime, '')		AS HAISHATouSetTime
        ,ISNULL(HAISHA.JyoSyaJin, 0)		AS HAISHAJyoSyaJin
        ,ISNULL(HAISHA.PlusJin, 0)			AS HAISHAPlusJin
        ,ISNULL(HAISHA.DrvJin, 0)			AS HAISHADrvJin
        ,ISNULL(HAISHA.GuiSu, 0)			AS HAISHAGuiSu
        ,ISNULL(HAISHA.OthJinKbn1, 0)		AS HAISHAOthJinKbn1
        ,ISNULL(OTHER1.RyakuNm, '')     	AS CodeKbOTHER1					
        ,ISNULL(HAISHA.OthJin1, '')			AS HAISHAOthJin1
        ,ISNULL(HAISHA.OthJinKbn2, 0)		AS HAISHAOthJinKbn2		
        ,ISNULL(OTHER2.RyakuNm, '')     	AS CodeKbOTHER2					
        ,ISNULL(HAISHA.OthJin2, 0)			AS HAISHAOthJin2
        ,ISNULL(HAISHA.KSKbn, 0)			AS HAISHAKSKbn
        ,ISNULL(HAISHA.HaiSKbn, 0)     	AS HAISHAHaiSKbn				
        ,ISNULL(HAISHA.HaiIKbn, 0)     	AS HAISHAHaiIKbn				
        ,ISNULL(HAISHA.NippoKbn, 0)    	AS HAISHANippoKbn			
        ,ISNULL(HAISHA.YouTblSeq, 0)		AS HAISHAYouTblSeq
        ,ISNULL(HAISHA.YouKataKbn, 0)		AS HAISHAYouKataKbn
        ,ISNULL(HAISHA.SyaRyoUnc, 0)		AS HAISHASyaRyoUnc
        ,ISNULL(HAISHA.SyaRyoSyo, 0)		AS HAISHASyaRyoSyo
        ,(ISNULL(HAISHA.SyaRyoUnc, 0) + ISNULL(HAISHA.SyaRyoSyo, 0)) AS HAISHASyaRyoUncSyaRyoSyo
        ,ISNULL(HAISHA.SyaRyoTes, 0)		AS HAISHASyaRyoTes
        ,ISNULL(HAISHA.YoushaUnc, 0)		AS HAISHAYoushaUnc
        ,ISNULL(HAISHA.YoushaSyo, 0)		AS HAISHAYoushaSyo
        ,ISNULL(HAISHA.YoushaTes, 0)		AS HAISHAYoushaTes
        ,ISNULL(HAISHA.PlatNo, '')			AS HAISHAPlatNo
        ,ISNULL(HAISHA.UkeJyKbnCd, 0)		AS HAISHAUkeJyKbnCd	
        ,ISNULL(UNKOBI.HaiSYmd, '')         AS UNKOBIHaiSYmd						
        ,ISNULL(UNKOBI.HaiSTime, '')        AS UNKOBIHaiSTime					
        ,ISNULL(UNKOBI.TouYmd, '')          AS UNKOBITouYmd						
        ,ISNULL(UNKOBI.TouChTime,'' )       AS UNKOBITouChTime					
        ,ISNULL(UNKOBI.DanTaNm, '')         AS UNKOBIDanTaNm					
        ,ISNULL(UNKOBI.KSKbn, 0)           AS UNKOBIKSKbn					
        ,ISNULL(UNKOBI.HaiSKbn, 0)         AS UNKOBIHaiSKbn					
        ,ISNULL(UNKOBI.HaiIKbn, 0)         AS UNKOBIHaiIKbn					
        ,ISNULL(UNKOBI.NippoKbn, 0)        AS UNKOBINippoKbn					
        ,ISNULL(UNKOBI.YouKbn, 0)          AS UNKOBIYouKbn					
        ,ISNULL(UNKOBI.UkeJyKbnCd, 0)      AS UNKOBIUkeJyKbnCd								
        ,ISNULL(YYKSHO.TokuiSeq, 0)        AS YYKSHOTokuiSeq							
        ,ISNULL(YYKSHO.SitenCdSeq, 0)      AS YYKSHOSitenCdSeq								
        ,ISNULL(YYKSHO.SeiEigCdSeq, 0)     AS YYKSHOSeiEigCdSeq							
        ,ISNULL(YYKSHO.SeiTaiYmd, '')       AS YYKSHOSeiTaiYmd					
        ,ISNULL(TOKISK.RyakuNm, '')         AS TOKISKRyakuNm					
        ,ISNULL(TOKIST.RyakuNm, '')         AS TOKISTRyakuNm						
        ,ISNULL(SYARYO.SyaRyoCd, 0)        AS SYARYOSyaRyoCd				
        ,ISNULL(SYARYO.SyaRyoNm, '')        AS SYARYOSyaRyoNm				
        ,ISNULL(SYASYU.KataKbn, 0)         AS SYASYUKataKbn					
        ,ISNULL(KATAKB.RyakuNm, '指定なし') AS KATAKBRyakuNm					
        ,ISNULL(SYASYU.SyaSyuNm, '')        AS SYASYUSyaSyuNm					
        ,ISNULL(KAISHA.RyakuNm, '')         AS KAISHARyakuNm					
        ,ISNULL(EIGYOS.RyakuNm, '')         AS EIGYOSRyakuNm				
        ,ISNULL(YOUSHA.YouCdSeq, 0)        AS YOUSHAYouCdSeq							
        ,ISNULL(YOUSHA.YouSitCdSeq, 0)     AS YOUSHAYouSitCdSeq								
        ,ISNULL(GYOUSYA.GyosyaCd, 0)       AS GYOUSYAGyosyaCd
        ,ISNULL(GYOUSYA.GyosyaNm, '')       AS GYOUSYAGyosyaNm
        ,ISNULL(YOUSHASAKI.TokuiCd, 0)     AS YOUSHASAKITokuiCd
        ,ISNULL(YOUSHASAKI.RyakuNm, '')     AS YOUSHASAKIRyakuNm
        ,ISNULL(YOUSHASAKISITEN.SitenCd, 0)   AS  YOUSHASAKISITENSitenCd      																															
        ,ISNULL(YOUSHASAKISITEN.RyakuNm, '')    AS YOUSHASAKISITENRyakuNm      																															
        ,ISNULL(YOUSHAKATA.RyakuNm, '')         AS YOUSHAKATARyakuNm
	--,(SELECT COUNT (*) FROM TKD_Kaknin WHERE UkeNo = HAISHA.UkeNo AND SiyoKbn = 1)	AS '確認中'
FROM TKD_Haisha AS HAISHA																																
INNER JOIN TKD_Yyksho AS YYKSHO																																
ON YYKSHO.UkeNo = HAISHA.UkeNo																																
	AND YYKSHO.TenantCdSeq = @TenantCdSeq             --ログインユーザーのテナントコード																															
LEFT JOIN TKD_Unkobi AS UNKOBI																																
ON UNKOBI.UkeNo = HAISHA.UkeNo																																
	AND UNKOBI.UnkRen = HAISHA.UnkRen																															
LEFT JOIN VPM_SyaRyo AS SYARYO																																
ON SYARYO.SyaRyoCdSeq = HAISHA.HaiSSryCdSeq																																
LEFT JOIN VPM_SyaSyu AS SYASYU																																
ON SYASYU.SyaSyuCdSeq = SYARYO.SyaSyuCdSeq																																
AND SYASYU.TenantCdSeq = @TenantCdSeq               --ログインユーザーのテナントコード																																
LEFT JOIN VPM_CodeKb AS KATAKB																																
ON KATAKB.CodeKbn = SYASYU.KataKbn																																
	AND KATAKB.CodeSyu = 'KATAKBN'																															
	AND KATAKB.TenantCdSeq = @TenantCdSeq               --ログインユーザーのテナントコード																															
LEFT JOIN VPM_Eigyos AS EIGYOS																																
ON EIGYOS.EigyoCdSeq = HAISHA.SyuEigCdSeq																																
LEFT JOIN VPM_Compny AS KAISHA																																
ON KAISHA.CompanyCdSeq = EIGYOS.CompanyCdSeq																																
	AND KAISHA.TenantCdSeq = @TenantCdSeq               --ログインユーザーのテナントコード																															
LEFT JOIN TKD_Yousha AS YOUSHA																																
ON YOUSHA.UkeNo = HAISHA.UkeNo																																
	AND YOUSHA.UnkRen = HAISHA.UnkRen																															
	AND YOUSHA.YouTblSeq = HAISHA.YouTblSeq																															
	AND YOUSHA.SiyoKbn = 1																															
LEFT JOIN VPM_Tokisk AS YOUSHASAKI																																
ON YOUSHASAKI.TokuiSeq = YOUSHA.YouCdSeq																																
	AND YOUSHASAKI.TenantCdSeq = @TenantCdSeq               --ログインユーザーのテナントコード																															
LEFT JOIN VPM_TokiSt AS YOUSHASAKISITEN																																
ON YOUSHASAKISITEN.TokuiSeq = YOUSHA.YouCdSeq																																
	AND YOUSHASAKISITEN.SitenCdSeq = YOUSHA.YouSitCdSeq																															
	AND YOUSHASAKISITEN.SiyoStaYmd <= HAISHA.HaiSYmd																															
	AND YOUSHASAKISITEN.SiyoEndYmd >= HAISHA.HaiSYmd																															
LEFT JOIN VPM_CodeKb AS YOUSHAKATA																																
ON YOUSHAKATA.CodeKbn = HAISHA.YouKataKbn																																
	AND YOUSHAKATA.CodeSyu = 'KATAKBN'																															
	AND YOUSHAKATA.TenantCdSeq = @TenantCdSeq               --ログインユーザーのテナントコード																															
LEFT JOIN VPM_Tokisk AS TOKISK																																
ON TOKISK.TokuiSeq = YYKSHO.TokuiSeq																																
	AND TOKISK.TenantCdSeq = @TenantCdSeq               --ログインユーザーのテナントコード																															
LEFT JOIN VPM_Gyosya AS GYOUSYA																																
	ON GYOUSYA.TenantCdSeq = TOKISK.TenantCdSeq																															
	AND GYOUSYA.GyosyaCdSeq = TOKISK.GyosyaCdSeq																															
LEFT JOIN VPM_TokiSt AS TOKIST																																
ON TOKIST.TokuiSeq = YYKSHO.TokuiSeq																																
	AND TOKIST.SitenCdSeq = YYKSHO.SitenCdSeq																															
	AND TOKIST.SiyoStaYmd <= HAISHA.HaiSYmd																															
	AND TOKIST.SiyoEndYmd >= HAISHA.HaiSYmd																															
LEFT JOIN VPM_CodeKb AS UKEJYKBN																																
ON UKEJYKBN.CodeKbn = UNKOBI.UkeJyKbnCd																																
	AND UKEJYKBN.CodeSyu = 'UKEJYKBNCD'																															
	AND UKEJYKBN.TenantCdSeq = @TenantCdSeq             --ログインユーザーのテナントコード																															
LEFT JOIN VPM_CodeKb AS OTHER1																																
ON OTHER1.CodeKbn = HAISHA.OthJinKbn1																																
	AND OTHER1.CodeSyu = 'OTHJINKBN'																															
	AND OTHER1.TenantCdSeq = @TenantCdSeq             --ログインユーザーのテナントコード																															
LEFT JOIN VPM_CodeKb AS OTHER2																																
ON OTHER2.CodeKbn = HAISHA.OthJinKbn2																																
	AND OTHER2.CodeSyu = 'OTHJINKBN'																															
	AND OTHER2.TenantCdSeq = @TenantCdSeq             --ログインユーザーのテナントコード																															
WHERE HAISHA.UkeNo = @UkeNo                     --パラメータの受付番号																																
AND HAISHA.UnkRen = @UnkRen                        --パラメータの運行日連番																																
AND HAISHA.SiyoKbn = 1                       --使用中																																
AND YYKSHO.YoyaSyu = 1                       --固定値：予約種別:１（受注）２（受注キャンセル）																																
AND YYKSHO.SiyoKbn = 1                       --使用中																																
AND UNKOBI.SiyoKbn = 1                       --使用中																																
																																
																															
RETURN																													

