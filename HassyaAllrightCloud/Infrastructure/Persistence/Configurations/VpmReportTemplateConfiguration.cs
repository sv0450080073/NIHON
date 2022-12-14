// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmReportTemplateConfiguration : IEntityTypeConfiguration<VpmReportTemplate>
    {
        public void Configure(EntityTypeBuilder<VpmReportTemplate> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_ReportTemplate");

            entity.Property(e => e.ImgPath)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.TemplateNm)
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
        }
    }
}
