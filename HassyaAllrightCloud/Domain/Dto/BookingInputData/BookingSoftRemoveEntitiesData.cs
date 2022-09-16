using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Domain.Dto.BookingInputData
{
    public class BookingSoftRemoveEntitiesData
    {
        public List<TkdYouSyu> YouSyus { get; set; }
        public List<TkdYousha> Youshas { get; set; }
        public List<TkdYmfuTu> YmfuTus { get; set; }
        public List<TkdYfutTu> YfutTus { get; set; }
        public List<TkdMihrim> Mihrims { get; set; }
        public List<TkdMfutTu> MfutTus { get; set; }
        public List<TkdTehai> Tehais { get; set; }
        public List<TkdKotei> Koteis { get; set; }
        public List<TkdKoteik> Koteiks { get; set; }
        public List<TkdHaiin> Haiins { get; set; }
        public List<TkdKoban> Kobans { get; set; }

    }
}
