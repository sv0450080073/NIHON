// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdSchYoteiConfiguration : IEntityTypeConfiguration<TkdSchYotei>
    {
        public void Configure(EntityTypeBuilder<TkdSchYotei> entity)
        {
            entity.HasKey(e => e.YoteiSeq)
                .HasName("TKD_SchYotei1");

            entity.ToTable("TKD_SchYotei");

            entity.Property(e => e.KuriEndYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.KuriReg)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.KuriRule)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.ShoRejBiko)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.ShoUpdTime)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ShoUpdYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TukiLabKbn)
                .IsRequired()
                .HasMaxLength(50)
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

            entity.Property(e => e.YoteiBiko)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.YoteiEtime)
                .IsRequired()
                .HasColumnName("YoteiETime")
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.YoteiEymd)
                .IsRequired()
                .HasColumnName("YoteiEYmd")
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.YoteiStime)
                .IsRequired()
                .HasColumnName("YoteiSTime")
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.YoteiSymd)
                .IsRequired()
                .HasColumnName("YoteiSYmd")
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}
