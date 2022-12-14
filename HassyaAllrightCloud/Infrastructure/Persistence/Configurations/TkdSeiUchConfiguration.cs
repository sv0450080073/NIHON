// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdSeiUchConfiguration : IEntityTypeConfiguration<TkdSeiUch>
    {
        public void Configure(EntityTypeBuilder<TkdSeiUch> entity)
        {
            entity.HasKey(e => new { e.SeiOutSeq, e.SeiRen, e.SeiMeiRen, e.SeiUchRen, e.TenantCdSeq })
                .HasName("TKD_SeiUch1");

            entity.ToTable("TKD_SeiUch");

            entity.Property(e => e.FutTumNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.HaiSymd)
                .IsRequired()
                .HasColumnName("HaiSYmd")
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.HasYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SyaSyuNm)
                .IsRequired()
                .HasMaxLength(12)
                .HasDefaultValueSql("(' ')");

            entity.Property(e => e.TouYmd)
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

            entity.Property(e => e.YoyaNm)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
