USE [HOC_Master]
GO
----------------
-- N.T.LAN.ANH 2021/06/17 fix billchecklist time out
-----------------
/****** Object:  Index [NonClusteredColumnStoreIndex-20210617-163842]    Script Date: 2021/06/17 16:48:51 ******/
CREATE NONCLUSTERED COLUMNSTORE INDEX [NonClusteredColumnStoreIndex-20210617-163842] ON [dbo].[TPM_Eigyos]
(
	[EigyoCdSeq]
)WITH (DROP_EXISTING = OFF, COMPRESSION_DELAY = 0) ON [PRIMARY]
GO


