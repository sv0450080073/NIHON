using HassyaAllrightCloud.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Infrastructure.Persistence.Configurations
{
    public class KobanTableTypeConfiguration : IEntityTypeConfiguration<KobanTableType>
    {
        public void Configure(EntityTypeBuilder<KobanTableType> entity)
        {
            entity.HasNoKey();
        }
    }
}
