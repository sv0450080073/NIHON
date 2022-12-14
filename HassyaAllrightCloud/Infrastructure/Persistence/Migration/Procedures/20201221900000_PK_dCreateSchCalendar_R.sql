USE [HOC_Kashikiri]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
if not exists (select * from sysobjects where name='TKD_SchCalendar' and xtype='U')
CREATE TABLE [dbo].[TKD_SchCalendar](
	[CalendarSeq] [int] IDENTITY(1,1) NOT NULL,
	[CalendarName] [nvarchar](50) NOT NULL,
	[CompanyCdSeq] [int] NOT NULL,
	[SiyoKbn] [tinyint] NOT NULL,
	[UpdYmd] [char](8) NOT NULL,
	[UpdTime] [char](6) NOT NULL,
	[UpdSyainCd] [int] NOT NULL,
	[UpdPrgID] [char](10) NOT NULL,
 CONSTRAINT [PK_TKD_SchCalendar] PRIMARY KEY CLUSTERED 
(
	[CalendarSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO															