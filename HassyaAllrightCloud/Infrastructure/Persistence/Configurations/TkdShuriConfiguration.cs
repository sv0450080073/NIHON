﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdShuriConfiguration : IEntityTypeConfiguration<TkdShuri>
    {
        public void Configure(EntityTypeBuilder<TkdShuri> entity)
        {
            entity.HasKey(e => e.ShuriTblSeq)
                .HasName("TKD_Shuri1");

            entity.ToTable("TKD_Shuri");

            entity.Property(e => e.BikoNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ShuriEtime)
                .IsRequired()
                .HasColumnName("ShuriETime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ShuriEymd)
                .IsRequired()
                .HasColumnName("ShuriEYmd")
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ShuriStime)
                .IsRequired()
                .HasColumnName("ShuriSTime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ShuriSymd)
                .IsRequired()
                .HasColumnName("ShuriSYmd")
                .HasMaxLength(8)
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
        }
    }
}
