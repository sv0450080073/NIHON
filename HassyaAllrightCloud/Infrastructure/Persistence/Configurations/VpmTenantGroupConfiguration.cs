// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmTenantGroupConfiguration : IEntityTypeConfiguration<VpmTenantGroup>
    {
        public void Configure(EntityTypeBuilder<VpmTenantGroup> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_TenantGroup");

            entity.Property(e => e.EndYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.Property(e => e.StaYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.Property(e => e.TenantGroupCd)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.TenantGroupName)
                .IsRequired()
                .HasMaxLength(100)
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
