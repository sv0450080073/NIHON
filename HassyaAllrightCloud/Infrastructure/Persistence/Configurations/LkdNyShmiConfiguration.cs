// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class LkdNyShmiConfiguration : IEntityTypeConfiguration<LkdNyShmi>
    {
        public void Configure(EntityTypeBuilder<LkdNyShmi> entity)
        {
            entity.HasKey(e => e.LogSeq)
                .HasName("LKD_NyShmi1");

            entity.ToTable("LKD_NyShmi");

            entity.Property(e => e.HenKeyItm)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(e => e.UkeNo)
                .IsRequired()
                .HasMaxLength(15)
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
