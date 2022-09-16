﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmBasyoConfiguration : IEntityTypeConfiguration<VpmBasyo>
    {
        public void Configure(EntityTypeBuilder<VpmBasyo> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_Basyo");

            entity.Property(e => e.BasyoMapCd)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BasyoNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.BasyoTanNm)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.FaxNo)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Jyus1)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Jyus2)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.RyakuNm)
                .IsRequired()
                .HasMaxLength(10);

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
