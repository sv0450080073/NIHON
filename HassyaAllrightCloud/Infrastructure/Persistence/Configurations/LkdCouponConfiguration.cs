﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class LkdCouponConfiguration : IEntityTypeConfiguration<LkdCoupon>
    {
        public void Configure(EntityTypeBuilder<LkdCoupon> entity)
        {
            entity.HasKey(e => e.LogSeq)
                .HasName("LKD_Coupon1");

            entity.ToTable("LKD_Coupon");

            entity.Property(e => e.CouNo)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.HakoYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HenKeyItm)
                .IsRequired()
                .HasMaxLength(4000);

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