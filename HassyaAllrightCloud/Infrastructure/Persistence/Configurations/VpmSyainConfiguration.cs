﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmSyainConfiguration : IEntityTypeConfiguration<VpmSyain>
    {
        public void Configure(EntityTypeBuilder<VpmSyain> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_Syain");

            entity.Property(e => e.ApaManNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.BirthYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ExpItem)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(e => e.Jyus1)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Jyus2)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Kana)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.KariSyainNm)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.KeitaiNo)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.MailAdd1)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.MailAdd2)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.MenkyoNo)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.NyusyaYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyainCd)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyainCdSeq).ValueGeneratedOnAdd();

            entity.Property(e => e.SyainNm)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.TaisyaYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TelNo)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false)
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

            entity.Property(e => e.ZipCd)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}
