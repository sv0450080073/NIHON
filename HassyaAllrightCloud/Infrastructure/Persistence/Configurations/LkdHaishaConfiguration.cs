﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class LkdHaishaConfiguration : IEntityTypeConfiguration<LkdHaisha>
    {
        public void Configure(EntityTypeBuilder<LkdHaisha> entity)
        {
            entity.HasKey(e => e.LogSeq)
                .HasName("LKD_Haisha1");

            entity.ToTable("LKD_Haisha");

            entity.Property(e => e.BunKsyuJyn).HasColumnName("BunKSyuJyn");

            entity.Property(e => e.DanTaNm2)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.GoSya)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.GuiWnin).HasColumnName("GuiWNin");

            entity.Property(e => e.HaiCom)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.HaiIkbn).HasColumnName("HaiIKbn");

            entity.Property(e => e.HaiSbinCdSeq).HasColumnName("HaiSBinCdSeq");

            entity.Property(e => e.HaiScdSeq).HasColumnName("HaiSCdSeq");

            entity.Property(e => e.HaiSjyus1)
                .IsRequired()
                .HasColumnName("HaiSJyus1")
                .HasMaxLength(30);

            entity.Property(e => e.HaiSjyus2)
                .IsRequired()
                .HasColumnName("HaiSJyus2")
                .HasMaxLength(30);

            entity.Property(e => e.HaiSkbn).HasColumnName("HaiSKbn");

            entity.Property(e => e.HaiSkigou)
                .IsRequired()
                .HasColumnName("HaiSKigou")
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HaiSkouKcdSeq).HasColumnName("HaiSKouKCdSeq");

            entity.Property(e => e.HaiSnm)
                .IsRequired()
                .HasColumnName("HaiSNm")
                .HasMaxLength(50);

            entity.Property(e => e.HaiSsetTime)
                .IsRequired()
                .HasColumnName("HaiSSetTime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HaiSsryCdSeq).HasColumnName("HaiSSryCdSeq");

            entity.Property(e => e.HaiStime)
                .IsRequired()
                .HasColumnName("HaiSTime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HaiSymd)
                .IsRequired()
                .HasColumnName("HaiSYmd")
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HenKeyItm)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(e => e.IkNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.KhinKbn).HasColumnName("KHinKbn");

            entity.Property(e => e.KikTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.KikYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kskbn).HasColumnName("KSKbn");

            entity.Property(e => e.KssyaRseq).HasColumnName("KSSyaRSeq");

            entity.Property(e => e.PlatNo)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyuKoTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyuKoYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyuPaTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TouChTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TouJyusyo1)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.TouJyusyo2)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.TouKigou)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TouKouKcdSeq).HasColumnName("TouKouKCdSeq");

            entity.Property(e => e.TouNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TouSetTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TouYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UkeNo)
                .IsRequired()
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.UpdPrgId)
                .IsRequired()
                .HasColumnName("UpdPrgID")
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UpdTime)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UpdYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}