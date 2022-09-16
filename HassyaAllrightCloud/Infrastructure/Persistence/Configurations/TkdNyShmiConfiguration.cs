﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdNyShmiConfiguration : IEntityTypeConfiguration<TkdNyShmi>
    {
        public void Configure(EntityTypeBuilder<TkdNyShmi> entity)
        {
            entity.HasKey(e => new { e.UkeNo, e.NyuSihRen, e.TenantCdSeq })
                .HasName("TKD_NyShmi1");

            entity.ToTable("TKD_NyShmi");

            entity.Property(e => e.UkeNo)
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
