// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VkHaishaFutTum1Configuration : IEntityTypeConfiguration<VkHaishaFutTum1>
    {
        public void Configure(EntityTypeBuilder<VkHaishaFutTum1> entity)
        {
            entity.HasNoKey();

            entity.ToView("VK_HaishaFutTum1");

            entity.Property(e => e.FutTumNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.HaiSsryCdSeq).HasColumnName("HaiSSryCdSeq");

            entity.Property(e => e.HaiSsryEigyoCdSeq).HasColumnName("HaiSSryEigyoCdSeq");

            entity.Property(e => e.HaiSymd)
                .IsRequired()
                .HasColumnName("HaiSYmd")
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.KikYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SeiTaiYmd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyukoYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.TesuRitu).HasColumnType("numeric(3, 1)");

            entity.Property(e => e.TouYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.UkeNo)
                .IsRequired()
                .HasMaxLength(15)
                .IsFixedLength();
        }
    }
}
