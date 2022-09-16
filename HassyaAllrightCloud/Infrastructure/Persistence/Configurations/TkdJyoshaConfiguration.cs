﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdJyoshaConfiguration : IEntityTypeConfiguration<TkdJyosha>
    {
        public void Configure(EntityTypeBuilder<TkdJyosha> entity)
        {
            entity.HasKey(e => new { e.UkeNo, e.UnkRen, e.TeiDanNo, e.BunkRen, e.JyoRen })
                .HasName("TKD_Jyosha1");

            entity.ToTable("TKD_Jyosha");

            entity.Property(e => e.UkeNo)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.JyoChiNm)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.JyoScdSeq).HasColumnName("JyoSCdSeq");

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