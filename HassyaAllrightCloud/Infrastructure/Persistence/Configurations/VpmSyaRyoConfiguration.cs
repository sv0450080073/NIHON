﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmSyaRyoConfiguration : IEntityTypeConfiguration<VpmSyaRyo>
    {
        public void Configure(EntityTypeBuilder<VpmSyaRyo> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_SyaRyo");

            entity.Property(e => e.Kana)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.KariSyaRyoNm)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.SyaRyoCdSeq).ValueGeneratedOnAdd();

            entity.Property(e => e.SyaRyoNm)
                .IsRequired()
                .HasMaxLength(10);

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