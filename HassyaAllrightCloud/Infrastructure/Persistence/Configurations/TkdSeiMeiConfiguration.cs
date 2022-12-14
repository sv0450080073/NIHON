// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace HassyaAllrightCloud.Infrastructure.Persistence
{
    public class TkdSeiMeiConfiguration : IEntityTypeConfiguration<TkdSeiMei>
    {
        public void Configure(EntityTypeBuilder<TkdSeiMei> entity)
        {
            entity.HasKey(e => new { e.SeiOutSeq, e.SeiRen, e.SeiMeiRen, e.TenantCdSeq })
                .HasName("TKD_SeiMei1");

            entity.ToTable("TKD_SeiMei");

            entity.Property(e => e.SeiOutSeq).HasComment("請求書出力ＳＥＱ");

            entity.Property(e => e.SeiRen).HasComment("請求書連番");

            entity.Property(e => e.SeiMeiRen).HasComment("請求書明細連番");

            entity.Property(e => e.MisyuRen).HasComment("未収明細連番");

            entity.Property(e => e.NyuKinRui)
                .HasColumnType("numeric(9, 0)")
                .HasComment("入金累計");

            entity.Property(e => e.SeiKin).HasComment("請求額");

            entity.Property(e => e.SeiSaHkbn)
                .HasColumnName("SeiSaHKbn")
                .HasComment("請求書再発行区分");

            entity.Property(e => e.SiyoKbn).HasComment("使用区分");

            entity.Property(e => e.SyaRyoSyo).HasComment("消費税額");

            entity.Property(e => e.SyaRyoTes).HasComment("手数料額");

            entity.Property(e => e.UkeNo)
                .IsRequired()
                .HasMaxLength(15)
                .IsFixedLength()
                .HasComment("受付番号");

            entity.Property(e => e.UpdPrgId)
                .IsRequired()
                .HasColumnName("UpdPrgID")
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("最終更新プログラムＩＤ");

            entity.Property(e => e.UpdSyainCd).HasComment("最終更新社員コードＳＥＱ");

            entity.Property(e => e.UpdTime)
                .IsRequired()
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("最終更新時間");

            entity.Property(e => e.UpdYmd)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("最終更新年月日");

            entity.Property(e => e.UriGakKin).HasComment("売上額");

            entity.Property(e => e.Zeiritsu)
                .HasColumnType("numeric(3, 1)")
                .HasComment("消費税率");
        }
    }
}
