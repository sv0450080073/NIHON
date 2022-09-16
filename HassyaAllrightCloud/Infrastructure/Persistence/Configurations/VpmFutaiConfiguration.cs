﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmFutaiConfiguration : IEntityTypeConfiguration<VpmFutai>
    {
        public void Configure(EntityTypeBuilder<VpmFutai> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_Futai");

            entity.Property(e => e.FutaiCdSeq).ValueGeneratedOnAdd();

            entity.Property(e => e.FutaiNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ItSehCode).HasColumnName("IT_SehCode");

            entity.Property(e => e.ItSesCode).HasColumnName("IT_SesCode");

            entity.Property(e => e.ItSeuCode).HasColumnName("IT_SeuCode");

            entity.Property(e => e.RyakuNm)
                .IsRequired()
                .HasMaxLength(10);

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