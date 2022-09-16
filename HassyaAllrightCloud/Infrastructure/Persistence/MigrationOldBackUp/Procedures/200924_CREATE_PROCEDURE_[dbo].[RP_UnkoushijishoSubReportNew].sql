USE [HOC_Kashikiri_New2109]
GO

/****** Object:  StoredProcedure [dbo].[RP_UnkoushijishoSubReportNew]    Script Date: 2020/09/24 14:10:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE
PROCEDURE [dbo].[RP_UnkoushijishoSubReportNew]
    @Ukeno nchar(15),
    @SyuKoYmd  char(10),
    @KikYmd    char(10),
    @TeiDanNo  smallint,
    @UnkRen    smallint,
    @BunkRen   smallint,
    @Dateparam nvarchar(10) AS
    CREATE TABLE #Datetmp
                 (
                              RowID      int identity(1,1)
                            , datehaisha date
                            , TomKbn     tinyint
                            , Nittei     smallint
                 )
                 DECLARE @SyuKodate date
               , @Kikdate           date
               , @dtStart           date
               , @dtpara            date
               , @i                 int
               , @row_number        int
               , @totalRecords      int
               , @totalRecordstmp   int DECLARE @TomKbn tinyint
               , @Nittei            smallint
               , @date              date DECLARE @datehaisha date
               , @Koutei            nvarchar
                 (
                              102
                 )
               , @TehNm nvarchar
                 (
                              102
                 )
               , @TehTel char
                 (
                              14
                 )
               , @TehRen           int DECLARE @CountRecordKotei INT=0
               , @CountRecordTehai INT                              =0 DECLARE @strSQL NVARCHAR
                 (
                              MAX
                 )
                 =''
               , @Condition VARCHAR
                 (
                              20
                 )
                 ='' set @SyuKodate=convert
                 (
                              datetime
                            , @SyuKoYmd
                            , 111
                 )
                 set @Kikdate=convert
                 (
                              datetime
                            , @KikYmd
                            , 111
                 )
                 set @dtStart=@SyuKodate set @i=1 while @SyuKodate <= @Kikdate BEGIN if
                                                                                        (
                                                                                                  select
                                                                                                            UNKOBI.ZenHaFlg
                                                                                                  from
                                                                                                            TKD_Haisha As HAISHA
                                                                                                            LEFT JOIN
                                                                                                                      TKD_Unkobi AS UNKOBI
                                                                                                                      ON
                                                                                                                                UNKOBI.UkeNo      = HAISHA.UkeNo
                                                                                                                                AND UNKOBI.UnkRen = HAISHA.UnkRen
                                                                                                  where
                                                                                                            HAISHA.UkeNo       =@Ukeno
                                                                                                            and HAISHA.UnkRen  =@UnkRen
                                                                                                            and HAISHA.BunkRen =@BunkRen
                                                                                                            and HAISHA.TeiDanNo=@TeiDanNo
                 )
                               =1
                 and @SyuKodate=@dtStart begin
    insert into #Datetmp values
           (@SyuKodate, 2, 1
           )
    Select
           @SyuKodate = DATEADD(day,1,@SyuKodate)
end else if
            (
                      select
                                UNKOBI.KhakFlg
                      from
                                TKD_Haisha As HAISHA
                                LEFT JOIN
                                          TKD_Unkobi AS UNKOBI
                                          ON
                                                    UNKOBI.UkeNo      = HAISHA.UkeNo
                                                    AND UNKOBI.UnkRen = HAISHA.UnkRen
                      where
                                HAISHA.UkeNo       =@Ukeno
                                and HAISHA.UnkRen  =@UnkRen
                                and HAISHA.BunkRen =@BunkRen
                                and HAISHA.TeiDanNo=@TeiDanNo
)
              =1
and @SyuKodate=@Kikdate begin
    insert into #Datetmp values
           (@SyuKodate, 3, 1
           )
    Select
           @SyuKodate = DATEADD(day,1,@SyuKodate)
end else begin
    insert into #Datetmp values
           (@SyuKodate, 1
                , @i
           )
    Select
           @SyuKodate = DATEADD(day,1,@SyuKodate) Set @i=@i+1
END
END
    select
           @totalRecords = COUNT(*)
    FROM
           #Datetmp
    CREATE TABLE #tblKotei
                 (
                              datehaisha date
                            , Koutei     nvarchar(102)
                            , KouRen     int
                 )
    CREATE TABLE #tblTehai
                 (
                              datehaisha date
                            , TehNm      nvarchar(102)
                            , TehTel     char(14)
                            , TehRen     int
                            , RowID      int identity(1,1)
                 )
    CREATE TABLE #tblTehaitmp
                 (
                              datehaisha date
                            , TehNm      nvarchar(102)
                            , TehTel     char(14)
                            , TehRen     int
                            , RowID      int identity(1,1)
                 )
                 set @i=1 WHILE
                 (
                              @i <= @totalRecords
                 )
                 begin
    SELECT
           @TomKbn=TomKbn
         , @Nittei=Nittei
         , @date  =datehaisha
    FROM
           #Datetmp
    WHERE
           RowID=@i
           -- select data TKD_Kotei
           if
              (
                     select
                            count(*)
                     from
                            TKD_Kotei
                     where
                            UkeNo       =@Ukeno
                            and UnkRen  =@UnkRen
                            and BunkRen =@BunkRen
                            and TeiDanNo=@TeiDanNo
                            and TomKbn  =@TomKbn
                            and Nittei  =@Nittei
                            and SiyoKbn =1
           )
           >0 begin
    INSERT INTO #tblKotei
           (datehaisha
                , Koutei
                , KouRen
           )
    SELECT
             @date
           , Koutei
           , ROW_NUMBER() OVER(ORDER BY KouRen ASC)
    FROM
             TKD_Kotei AS KOTEI
    WHERE
             KOTEI.UkeNo        = @Ukeno　　　　--3/　運行指示書情報取得で取得した受付番号    	
             AND KOTEI.UnkRen   = @UnkRen   --3/　運行指示書情報取得で取得した運行日連番    	
             AND KOTEI.TeiDanNo = @TeiDanNo --3/  運行指示書情報取得で取得した悌団番号    	
             AND KOTEI.BunkRen  = @BunkRen  --3/  運行指示書情報取得で取得した分割連番 	
             And KOTEI.Nittei   =@Nittei
             and KOTEI.TomKbn   =@TomKbn
             AND KOTEI.SiyoKbn  = 1
    order by
             Nittei
           , KouRen
end else begin
    INSERT INTO #tblKotei
           (datehaisha
                , Koutei
                , KouRen
           )
    SELECT
             @date
           , Koutei
           , ROW_NUMBER() OVER(ORDER BY KouRen ASC)
    FROM
             TKD_Kotei AS KOTEI
    WHERE
             KOTEI.UkeNo        = @Ukeno　　　　
             AND KOTEI.UnkRen   = 1
             AND KOTEI.TeiDanNo = 0
             AND KOTEI.BunkRen  = 0
             And KOTEI.Nittei   =@Nittei
             and KOTEI.TomKbn   =@TomKbn
             AND KOTEI.SiyoKbn  = 1
    order by
             Nittei
           , KouRen
end
-- select data TKD_Tehai
if
   (
          select
                 count(*)
          from
                 TKD_Kotei
          where
                 UkeNo       =@Ukeno
                 and UnkRen  =@UnkRen
                 and BunkRen =@BunkRen
                 and TeiDanNo=@TeiDanNo
                 and Nittei  =@Nittei
                 and SiyoKbn =1
)
>0 begin
    INSERT INTO #tblTehai
           (datehaisha
                , TehNm
                , TehTel
                , TehRen
           )
    SELECT
             @date
           , TehNm
           , TehTel
           , ROW_NUMBER() OVER(ORDER BY TehRen ASC)
    FROM
             TKD_Tehai AS TEIHAI
    WHERE
             TEIHAI.UkeNo        = @Ukeno　　　　
             AND TEIHAI.UnkRen   = @UnkRen
             AND TEIHAI.TeiDanNo = @TeiDanNo
             AND TEIHAI.BunkRen  = @BunkRen
             And TEIHAI.Nittei   =@Nittei
             AND TEIHAI.SiyoKbn  = 1
             and TEIHAI.TomKbn   =@TomKbn
end else begin
    INSERT INTO #tblTehai
           (datehaisha
                , TehNm
                , TehTel
                , TehRen
           )
    SELECT
             @date
           , TehNm
           , TehTel
           , ROW_NUMBER() OVER(ORDER BY TehRen ASC)
    FROM
             TKD_Tehai AS TEIHAI
    WHERE
             TEIHAI.UkeNo        = @Ukeno　　　　
             AND TEIHAI.UnkRen   = 1
             AND TEIHAI.TeiDanNo = 0
             AND TEIHAI.BunkRen  = 0
             And TEIHAI.Nittei   =@Nittei
             AND TEIHAI.SiyoKbn  = 1
             and TEIHAI.TomKbn   =@TomKbn
end SET @i                       =@i+1
end set @i                       =1
    select
           @totalRecordstmp = COUNT(*)
    FROM
           #tblTehai WHILE (@i <= @totalRecordstmp) begin
    SELECT
           @date  =datehaisha
         , @TehNm =TehNm
         , @TehTel=TehTel
         , @TehRen=TehRen
    FROM
           #tblTehai
    WHERE
           RowID=@i
    insert into #tblTehai values
           (@date
                , @TehNm
                ,''
                , @TehRen*2-1
           )
    insert into #tblTehai values
           (@date
                , @TehTel
                ,''
                , @TehRen*2
           )
    delete
    FROM
           #tblTehai
    WHERE
           RowID=@i SET @i=@i+1
end SET @i      =1
    INSERT INTO #tblTehaitmp
           (datehaisha
                , TehNm
                , TehTel
                , TehRen
           )
    SELECT
             tbl.datehaisha
           , tbl.TehNm
           , tbl.TehTel
           , tbl.TehRen
    FROM
             #tblTehai AS tbl
    ORDER BY
             tbl.RowID DECLARE @tblsubreport TABLE(dateKotei date,Koutei nvarchar(102),TehNm nvarchar(102),TehTel char(14)) WHILE (@i <= @totalRecords) begin
    SELECT
           @date=datehaisha
    FROM
           #Datetmp
    WHERE
           RowID=@i if
                       (
                              select
                                     COUNT(*)
                              from
                                     #tblKotei
                              where
                                     datehaisha=@date
           )
           =0
           and
           (
                  select
                         COUNT(*)
                  from
                         #tblTehaitmp
                  where
                         datehaisha=@date
           )
           =0 begin Set @i=@i+1
end else begin
    SELECT
           @CountRecordKotei = COUNT(*)
    FROM
           #tblKotei
    where
           datehaisha=@date
    SELECT
           @CountRecordTehai = COUNT(*)
    FROM
           #tblTehaitmp
    where
           datehaisha=@date SET @Condition = IIF(@CountRecordKotei>=@CountRecordTehai,'LEFT','RIGHT') SET @strSQL='' SET @strSQL = @strSQL + 'SELECT ISNULL(Kotei.datehaisha,Tehai.datehaisha) ,' + '		ISNULL(Kotei.Koutei,''''),' + '		ISNULL(Tehai.TehNm,''''),' + '		ISNULL(Tehai.TehTel,'''') ' + 'FROM #tblKotei AS Kotei '+ @Condition + ' JOIN #tblTehaitmp AS Tehai
		ON Kotei.datehaisha=Tehai.datehaisha and Kotei.KouRen=Tehai.TehRen  where Kotei.datehaisha='''+convert(varchar, @date, 23)+''' or Tehai.datehaisha='''+convert(varchar, @date, 23)+''''
    INSERT INTO @tblsubreport EXEC
           (@strSQL
           )
           Set @i=@i+1
end
end
--select * from #Datetmp
if
(@Dateparam <>''
)
begin set @dtpara=convert
(datetime
     , @Dateparam
     , 111
)
    select *
    FROM
           @tblsubreport
    where
           dateKotei=@dtpara
end else
    select *
    FROM
           @tblsubreport
GO


