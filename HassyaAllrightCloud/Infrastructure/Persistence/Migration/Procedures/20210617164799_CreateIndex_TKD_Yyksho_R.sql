USE [HOC_Kashikiri]
GO
----------------
-- N.T.LAN.ANH 2021/06/17 fix billchecklist time out
-----------------
/****** Object:  Index [NonClusteredColumnStoreIndex-20210617-163953]    Script Date: 2021/06/17 16:46:13 ******/
CREATE NONCLUSTERED COLUMNSTORE INDEX [NonClusteredColumnStoreIndex-20210617-163953] ON [dbo].[TKD_Yyksho]
(
	[SeiEigCdSeq]
)WITH (DROP_EXISTING = OFF, COMPRESSION_DELAY = 0) ON [PRIMARY]
GO


