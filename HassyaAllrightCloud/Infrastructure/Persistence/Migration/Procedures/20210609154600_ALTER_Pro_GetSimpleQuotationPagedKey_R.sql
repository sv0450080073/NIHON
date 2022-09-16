USE [HOC_Kashikiri]
GO

/****** Object:  StoredProcedure [dbo].[Pro_GetSimpleQuotationPagedKey_R]    Script Date: 2021/06/09 15:42:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER   procedure [dbo].[Pro_GetSimpleQuotationPagedKey_R]
		@tenantId int,
		@startPickupDate nvarchar(10),
		@endPickupDate nvarchar(10),
		@startArrivalDate nvarchar(10),
		@endArrivalDate nvarchar(10),
		@startReservationClassification int,
		@endReservationClassification int,
		@ukeCdFrom float,
		@ukeCdTo float,
		@startSupplier int,
		@endSupplier int,
		@branchStart int,
		@branchEnd int
AS
BEGIN
	-- Query 5
	SELECT 
		  IsNULL(Un.UkeNo, '')		AS 'UkeNo'
		  ,IsNULL(Un.UnkRen, '0')	AS 'UnkRen'
	FROM TKD_Yyksho AS Yo
	LEFT JOIN TKD_Unkobi AS Un ON Un.UkeNo=Yo.UkeNo AND Un.SiyoKbn=1
	INNER JOIN VPM_Eigyos AS Ei ON Ei.EigyoCdSeq=Yo.UkeEigCdSeq
	INNER JOIN VPM_Tokisk AS Toku
		  ON Toku.TenantCdSeq=Yo.TenantCdSeq
		  AND Toku.TokuiSeq=Yo.TokuiSeq
		  AND Yo.UkeYmd BETWEEN Toku.SiyoStaYmd AND Toku.SiyoEndYmd
	LEFT JOIN VPM_Gyosya AS Gyosya
		  ON Toku.GyosyaCdSeq = Gyosya.GyosyaCdSeq
		  AND Toku.TenantCdSeq = Gyosya.TenantCdSeq 
		  AND Gyosya.SiyoKbn = 1
	INNER JOIN VPM_TokiSt AS Shiten
		  ON Shiten.SitenCdSeq=Yo.SirSitenCdSeq
		  AND Shiten.TokuiSeq=Yo.TokuiSeq
		  AND Yo.UkeYmd BETWEEN Shiten.SiyoStaYmd AND Shiten.SiyoEndYmd
	INNER JOIN VPM_Syain AS Staff ON Staff.SyainCdSeq=Yo.EigTanCdSeq
	INNER JOIN VPM_Compny AS Company ON Company.CompanyCdSeq=Ei.CompanyCdSeq AND Company.TenantCdSeq=@tenantId
	INNER JOIN VPM_YoyKbn AS YoyKbn ON Yo.YoyaKbnSeq = YoyKbn.YoyaKbnSeq
	WHERE Yo.YoyaSyu=3
		  AND Yo.SiyoKbn=1
		  AND Yo.TenantCdSeq=@tenantId
		  AND (@startPickupDate = '' OR @endPickupDate = '' OR (Un.HaiSYmd BETWEEN @startPickupDate AND @endPickupDate))   
		  AND (@startArrivalDate = '' OR @endArrivalDate = '' OR (Un.TouYmd BETWEEN @startArrivalDate AND @endArrivalDate))
		  --AND ((@bookingTypeList != '' AND Yo.YoyaKbnSeq IN (select * from dbo.FN_SplitString(@bookingTypeList, '-'))) OR @bookingTypeList = '')
		  AND (@startReservationClassification IS NULL OR YoyKbn.YoyaKbn >= @startReservationClassification) -- 予約区分
          AND (@endReservationClassification IS NULL OR YoyKbn.YoyaKbn <= @endReservationClassification) -- 予約区分
		  AND ((@ukeCdFrom != 0 AND Yo.UkeCd >= @ukeCdFrom) OR @ukeCdFrom = 0)
		  AND ((@ukeCdTo != 0 AND Yo.UkeCd <= @ukeCdTo) OR @ukeCdTo = 0)
		  --AND ((@customerStart = 0 AND @customerEnd = 0)
				--OR
				--(@customerStart <> @customerEnd AND Toku.TokuiCd = @customerStart AND Shiten.SitenCd >= @customerSirStart)
				--OR
				--(@customerStart <> @customerEnd AND Toku.TokuiCd = @customerEnd AND Shiten.SitenCd <= @customerSirEnd)
				--OR
				--(@customerStart = @customerEnd AND Toku.TokuiCd = @customerStart AND Shiten.SitenCd >= @customerSirStart and Shiten.SitenCd <= @customerSirEnd)
				--OR
				--(@customerStart = 0 and @customerEnd <> 0 AND ((Toku.TokuiCd = @customerEnd AND Shiten.SitenCd <= @customerSirEnd) OR Toku.TokuiCd < @customerEnd))
				--OR
				--(@customerEnd = 0 and @customerStart <> 0 AND ((Toku.TokuiCd = @customerStart AND Shiten.SitenCd >= @customerSirStart) OR Toku.TokuiCd > @customerStart))
				--OR
				--(Toku.TokuiCd < @customerEnd AND Toku.TokuiCd > @customerStart))
	    AND (@startSupplier IS NULL OR FORMAT(Gyosya.GyosyaCd, '000') + FORMAT(Toku.TokuiCd, '0000') + FORMAT(Shiten.SitenCd, '0000') >= @startSupplier) -- 仕入先コード
            AND (@endSupplier IS NULL OR FORMAT(Gyosya.GyosyaCd, '000') + FORMAT(Toku.TokuiCd, '0000') + FORMAT(Shiten.SitenCd, '0000') <= @endSupplier) -- 仕入先コード
		AND ((@branchStart != 0 AND Ei.EigyoCd >= @branchStart) OR @branchStart = 0)
		AND ((@branchEnd != 0 AND Ei.EigyoCd <= @branchEnd) OR @branchEnd = 0)
	ORDER BY UkeNo, UnkRen
END

GO