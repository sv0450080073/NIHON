﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdBookingMaxMinFareFeeCalcMeisaiConfiguration : IEntityTypeConfiguration<TkdBookingMaxMinFareFeeCalcMeisai>
    {
        public void Configure(EntityTypeBuilder<TkdBookingMaxMinFareFeeCalcMeisai> entity)
        {
            entity.HasKey(e => new { e.UkeNo, e.UnkRen, e.SyaSyuRen, e.Nittei });

            entity.ToTable("TKD_BookingMaxMinFareFeeCalcMeisai");

            entity.Property(e => e.UkeNo)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.ChangeDriverMidnightEarlyMorningTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ChangeDriverRestraintTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ChangeDriverRunningKm).HasColumnType("numeric(7, 2)");

            entity.Property(e => e.InspectionEndTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.InspectionEndYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.InspectionStartTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.InspectionStartYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.MidnightEarlyMorningTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.RestraintTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.RunningKm).HasColumnType("numeric(7, 2)");

            entity.Property(e => e.ServiceKm).HasColumnType("numeric(7, 2)");

            entity.Property(e => e.ServiceTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
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
        }
    }
}