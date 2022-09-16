﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdYykSyuConfiguration : IEntityTypeConfiguration<TkdYykSyu>
    {
        public void Configure(EntityTypeBuilder<TkdYykSyu> entity)
        {
            entity.HasKey(e => new { e.UkeNo, e.UnkRen, e.SyaSyuRen });

            entity.ToTable("TKD_YykSyu");

            entity.HasIndex(e => new { e.SiyoKbn, e.UkeNo, e.UnkRen, e.SyaSyuRen, e.HenKai })
                .HasName("TKD_YykSyu1");

            entity.HasIndex(e => new { e.SyaSyuDai, e.SyaSyuTan, e.SyaRyoUnc, e.SiyoKbn, e.UkeNo, e.UnkRen })
                .HasName("TKD_YykSyu_SyaSyuDai");

            entity.Property(e => e.UkeNo)
                .HasMaxLength(15)
                .IsFixedLength();

            entity.Property(e => e.DriverNum)
                .HasDefaultValueSql("((1))")
                .HasComment("運転手人数");

            entity.Property(e => e.GuiderNum).HasComment("ガイド人数");

            entity.Property(e => e.SyaRyoUnc).HasComment("運賃（バス代）");

            entity.Property(e => e.UnitBusFee).HasComment("料金単価");

            entity.Property(e => e.UnitBusPrice).HasComment("運賃単価");

            entity.Property(e => e.UnitGuiderFee).HasComment("ガイド料金");

            entity.Property(e => e.UnitGuiderPrice).HasComment("ガイド単価");

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