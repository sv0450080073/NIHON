USE HOC_Kashikiri
GO

ALTER TABLE [dbo].[TKD_UnkobiFile]
DROP COLUMN [FileNm], [FileType], [FileLink], [FileSize]
GO

ALTER TABLE [dbo].[TKD_UnkobiFile] ADD FolderId NVARCHAR(255) NOT NULL DEFAULT '',
	FileId NVARCHAR(255) NOT NULL DEFAULT ''
GO