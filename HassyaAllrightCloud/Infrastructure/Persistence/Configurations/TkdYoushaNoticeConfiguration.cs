﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdYoushaNoticeConfiguration : IEntityTypeConfiguration<TkdYoushaNotice>
    {
        public void Configure(EntityTypeBuilder<TkdYoushaNotice> entity)
        {
            entity.HasKey(e => new { e.MotoTenantCdSeq, e.MotoUkeNo, e.MotoUnkRen, e.MotoYouTblSeq });

            entity.ToTable("TKD_YoushaNotice");

            entity.Property(e => e.MotoUkeNo)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.DanTaNm).HasMaxLength(100);

            entity.Property(e => e.HaiSYmd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UpdPrgId)
                .HasColumnName("UpdPrgID")
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UpdTime)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UpdYmd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}
