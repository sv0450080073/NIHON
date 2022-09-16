﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdUnkobiExpConfiguration : IEntityTypeConfiguration<TkdUnkobiExp>
    {
        public void Configure(EntityTypeBuilder<TkdUnkobiExp> entity)
        {
            entity.HasKey(e => new { e.UkeNo, e.UnkRen })
                .HasName("TKD_UnkobiExp1");

            entity.ToTable("TKD_UnkobiExp");

            entity.Property(e => e.UkeNo)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.ExpItem)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.HaiSbinNm)
                .IsRequired()
                .HasColumnName("HaiSBinNm")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HaiSkouKnm)
                .IsRequired()
                .HasColumnName("HaiSKouKNm")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TouSbinNm)
                .IsRequired()
                .HasColumnName("TouSBinNm")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TouSkouKnm)
                .IsRequired()
                .HasColumnName("TouSKouKNm")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}
