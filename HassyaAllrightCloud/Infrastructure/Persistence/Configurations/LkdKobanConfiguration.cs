﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class LkdKobanConfiguration : IEntityTypeConfiguration<LkdKoban>
    {
        public void Configure(EntityTypeBuilder<LkdKoban> entity)
        {
            entity.HasKey(e => e.LogSeq)
                .HasName("LKD_Koban1");

            entity.ToTable("LKD_Koban");

            entity.Property(e => e.BikoNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.FuriYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HenKeyItm)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(e => e.JitdTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.KitYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.KouStime)
                .IsRequired()
                .HasColumnName("KouSTime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.KyuKtime)
                .IsRequired()
                .HasColumnName("KyuKTime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.RouTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SigyCd)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SsinTime)
                .IsRequired()
                .HasColumnName("SSinTime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyukinTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Syukinbasy)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TaikTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TaiknBasy)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TaiknTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UkeNo)
                .IsRequired()
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.UnkYmd)
                .IsRequired()
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

            entity.Property(e => e.UsinyTime)
                .IsRequired()
                .HasColumnName("USinyTime")
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ZangTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}
