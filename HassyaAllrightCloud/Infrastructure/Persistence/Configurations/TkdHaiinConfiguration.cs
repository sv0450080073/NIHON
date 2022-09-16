﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdHaiinConfiguration : IEntityTypeConfiguration<TkdHaiin>
    {
        public void Configure(EntityTypeBuilder<TkdHaiin> entity)
        {
            entity.HasKey(e => new { e.UkeNo, e.UnkRen, e.TeiDanNo, e.BunkRen, e.HaiInRen })
                .IsClustered(false);

            entity.ToTable("TKD_Haiin");

            entity.Property(e => e.UkeNo)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.SyukinTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Syukinbasy)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TaiknBasy)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TaiknTime)
                .IsRequired()
                .HasMaxLength(4)
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