﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmBankStConfiguration : IEntityTypeConfiguration<VpmBankSt>
    {
        public void Configure(EntityTypeBuilder<VpmBankSt> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_BankSt");

            entity.Property(e => e.BankCd)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BankSitCd)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BankSitNm)
                .IsRequired()
                .HasMaxLength(50);

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