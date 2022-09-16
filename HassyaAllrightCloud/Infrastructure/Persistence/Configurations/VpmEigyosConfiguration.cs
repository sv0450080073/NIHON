﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class VpmEigyosConfiguration : IEntityTypeConfiguration<VpmEigyos>
    {
        public void Configure(EntityTypeBuilder<VpmEigyos> entity)
        {
            entity.HasNoKey();

            entity.ToView("VPM_Eigyos");

            entity.Property(e => e.BankCd1)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BankCd2)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BankCd3)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BankSitCd1)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BankSitCd2)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.BankSitCd3)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.CalcuSyuTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.CalcuTaiTime)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.EigyoNm)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ExpItem)
                .IsRequired()
                .HasMaxLength(8000)
                .IsUnicode(false);

            entity.Property(e => e.FaxNo)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.Jyus1)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.Jyus2)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.KouzaMeigi)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.KouzaNo1)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.Property(e => e.KouzaNo2)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.Property(e => e.KouzaNo3)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.Property(e => e.MailAcc)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.Property(e => e.MailPass)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.MailUser)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Popninsyo).HasColumnName("POPNinsyo");

            entity.Property(e => e.PopportNo)
                .IsRequired()
                .HasColumnName("POPPortNo")
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.PopsvrNm)
                .IsRequired()
                .HasColumnName("POPSvrNm")
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.RyakuNm)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.SmtpdomNm)
                .IsRequired()
                .HasColumnName("SMTPDomNm")
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.Smtpninsyo).HasColumnName("SMTPNinsyo");

            entity.Property(e => e.SmtpportNo)
                .IsRequired()
                .HasColumnName("SMTPPortNo")
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();

            entity.Property(e => e.SmtpsvrNm)
                .IsRequired()
                .HasColumnName("SMTPSvrNm")
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.Property(e => e.TelNo)
                .IsRequired()
                .HasMaxLength(14)
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

            entity.Property(e => e.ZipCd)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
        }
    }
}