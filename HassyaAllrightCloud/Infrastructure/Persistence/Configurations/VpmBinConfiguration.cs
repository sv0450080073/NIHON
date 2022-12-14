// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmBinConfiguration : IEntityTypeConfiguration<VpmBin>
    {
        public void Configure(EntityTypeBuilder<VpmBin> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_Bin");

            entity.Property(e => e.BinNm)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.SiyoEndYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SiyoStaYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyuPaTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TouChTime)
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
