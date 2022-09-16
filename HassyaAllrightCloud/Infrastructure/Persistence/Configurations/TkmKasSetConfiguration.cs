﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkmKasSetConfiguration : IEntityTypeConfiguration<TkmKasSet>
    {
        public void Configure(EntityTypeBuilder<TkmKasSet> entity)
        {
            entity.HasKey(e => e.CompanyCdSeq)
                .HasName("TKM_KasSet1");

            entity.ToTable("TKM_KasSet");

            entity.Property(e => e.CompanyCdSeq).ValueGeneratedNever();

            entity.Property(e => e.CanEkan1).HasColumnName("CanEKan1");

            entity.Property(e => e.CanEkan2).HasColumnName("CanEKan2");

            entity.Property(e => e.CanEkan3).HasColumnName("CanEKan3");

            entity.Property(e => e.CanEkan4).HasColumnName("CanEKan4");

            entity.Property(e => e.CanEkan5).HasColumnName("CanEKan5");

            entity.Property(e => e.CanEkan6).HasColumnName("CanEKan6");

            entity.Property(e => e.CanMdkbn).HasColumnName("CanMDKbn");

            entity.Property(e => e.CanRit1).HasColumnType("numeric(4, 1)");

            entity.Property(e => e.CanRit2).HasColumnType("numeric(4, 1)");

            entity.Property(e => e.CanRit3).HasColumnType("numeric(4, 1)");

            entity.Property(e => e.CanRit4).HasColumnType("numeric(4, 1)");

            entity.Property(e => e.CanRit5).HasColumnType("numeric(4, 1)");

            entity.Property(e => e.CanRit6).HasColumnType("numeric(4, 1)");

            entity.Property(e => e.CanSkan1).HasColumnName("CanSKan1");

            entity.Property(e => e.CanSkan2).HasColumnName("CanSKan2");

            entity.Property(e => e.CanSkan3).HasColumnName("CanSKan3");

            entity.Property(e => e.CanSkan4).HasColumnName("CanSKan4");

            entity.Property(e => e.CanSkan5).HasColumnName("CanSKan5");

            entity.Property(e => e.CanSkan6).HasColumnName("CanSKan6");

            entity.Property(e => e.ColHai)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColHaiin)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcHai)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcHaiin)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcKari)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcKariH)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcNcou)
                .IsRequired()
                .HasColumnName("ColIcNCou")
                .HasMaxLength(30);

            entity.Property(e => e.ColIcNip)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcNyu)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcScou)
                .IsRequired()
                .HasColumnName("ColIcSCou")
                .HasMaxLength(30);

            entity.Property(e => e.ColIcShiha)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcWari)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColIcYou)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColKahar)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColKaku)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColKanyu)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColKari)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColKariH)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColKyoy)
                .IsRequired()
                .HasColumnName("ColKYoy")
                .HasMaxLength(30);

            entity.Property(e => e.ColMiKari)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColNcou)
                .IsRequired()
                .HasColumnName("ColNCou")
                .HasMaxLength(30);

            entity.Property(e => e.ColNin)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColNip)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColNyu)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColScou)
                .IsRequired()
                .HasColumnName("ColSCou")
                .HasMaxLength(30);

            entity.Property(e => e.ColSelect)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColShiha)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColWari)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.ColYou)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.DrvAutoSet).HasDefaultValueSql("('')");

            entity.Property(e => e.EtckinKbn)
                .HasColumnName("ETCKinKbn")
                .HasDefaultValueSql("('')");

            entity.Property(e => e.ExpItem)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.FutSf1flg).HasColumnName("FutSF1Flg");

            entity.Property(e => e.FutSf2flg).HasColumnName("FutSF2Flg");

            entity.Property(e => e.FutSf3flg).HasColumnName("FutSF3Flg");

            entity.Property(e => e.FutSf4flg).HasColumnName("FutSF4Flg");

            entity.Property(e => e.FutTumCdSeq).HasDefaultValueSql("('')");

            entity.Property(e => e.GoSyaAutoSet).HasDefaultValueSql("('')");

            entity.Property(e => e.GuiAutoSet).HasDefaultValueSql("('')");

            entity.Property(e => e.GuideFutTumCdSeq).HasDefaultValueSql("('')");

            entity.Property(e => e.JisKinKyuNm01)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm02)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm03)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm04)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm05)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm06)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm07)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm08)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm09)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JisKinKyuNm10)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JkariKbn).HasColumnName("JKariKbn");

            entity.Property(e => e.JkbunPat).HasColumnName("JKBunPat");

            entity.Property(e => e.JymAchkKbn).HasColumnName("JymAChkKbn");

            entity.Property(e => e.JyoSenInfoPtnCol1)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol10)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol11)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol12)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol13)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol14)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol15)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol2)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol3)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol4)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol5)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol6)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol7)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol8)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnCol9)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn1).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn10).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn11).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn12).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn13).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn14).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn15).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn2).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn3).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn4).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn5).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn6).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn7).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn8).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenInfoPtnKbn9).HasDefaultValueSql("('')");

            entity.Property(e => e.JyoSenMjPtnCol1)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JyoSenMjPtnCol2)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JyoSenMjPtnCol3)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JyoSenMjPtnCol4)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JyoSenMjPtnCol5)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.JyoSyaChkSiyoKbn).HasDefaultValueSql("('')");

            entity.Property(e => e.KarSyuKiTimeSiyoKbn).HasDefaultValueSql("('')");

            entity.Property(e => e.QuotationTransfer).HasDefaultValueSql("((1))");

            entity.Property(e => e.SeiCom1)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(e => e.SeiCom2)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(e => e.SeiCom3)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(e => e.SeiCom4)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(e => e.SeiCom5)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(e => e.SeiCom6)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(e => e.SeiMuki).HasDefaultValueSql("('')");

            entity.Property(e => e.SeisanCdSeq).HasDefaultValueSql("('')");

            entity.Property(e => e.SenBackPtnCol)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SenBackPtnKbn).HasDefaultValueSql("('')");

            entity.Property(e => e.SenDayRenge).HasDefaultValueSql("('')");

            entity.Property(e => e.SenDefWidth).HasDefaultValueSql("('')");

            entity.Property(e => e.SenObptnCol)
                .IsRequired()
                .HasColumnName("SenOBPtnCol")
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SenObptnKbn)
                .HasColumnName("SenOBPtnKbn")
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol1)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol10)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol11)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol12)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol13)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol14)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol15)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol2)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol3)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol4)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol5)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol6)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol7)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol8)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnCol9)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn1).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn10).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn11).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn12).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn13).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn14).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn15).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn2).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn3).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn4).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn5).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn6).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn7).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn8).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenInfoPtnKbn9).HasDefaultValueSql("('')");

            entity.Property(e => e.SyaSenMjPtnCol1)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.SyaSenMjPtnCol2)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.SyaSenMjPtnCol3)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.SyaSenMjPtnCol4)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.SyaSenMjPtnCol5)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.TehaiAutoSet).HasDefaultValueSql("('')");

            entity.Property(e => e.UpdPrgId)
                .IsRequired()
                .HasColumnName("UpdPrgID")
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValueSql("('')");

            entity.Property(e => e.UpdSyainCd).HasDefaultValueSql("('')");

            entity.Property(e => e.UpdTime)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValueSql("('')");

            entity.Property(e => e.UpdYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValueSql("('')");

            entity.Property(e => e.UriMdkbn).HasColumnName("UriMDKbn");

            entity.Property(e => e.YoySyuKiTimeSiyoKbn).HasDefaultValueSql("('')");

            entity.Property(e => e.YykHaiStime)
                .IsRequired()
                .HasColumnName("YykHaiSTime")
                .HasMaxLength(4)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.YykTouTime)
                .IsRequired()
                .HasMaxLength(4)
                .HasDefaultValueSql("('')");

            entity.Property(e => e.KaizenKijunYmd)
                .IsRequired()
                .HasMaxLength(8)
                .HasDefaultValueSql("('')");
        }
    }
}