﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpdCustomKiConfiguration : IEntityTypeConfiguration<VpdCustomKi>
    {
        public void Configure(EntityTypeBuilder<VpdCustomKi> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPD_CustomKi");

            entity.Property(e => e.ExpItem1)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ExpItem2)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.ExpItem3)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou01)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou02)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou03)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou04)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou05)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou06)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou07)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou08)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou09)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Kinou10)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.KinouId)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}
