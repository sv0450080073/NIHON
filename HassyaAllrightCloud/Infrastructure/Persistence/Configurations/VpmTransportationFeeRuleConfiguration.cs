﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmTransportationFeeRuleConfiguration : IEntityTypeConfiguration<VpmTransportationFeeRule>
    {
        public void Configure(EntityTypeBuilder<VpmTransportationFeeRule> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_TransportationFeeRule");

            entity.Property(e => e.EndYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.Property(e => e.StartYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.Property(e => e.TransportationPlaceName)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

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
