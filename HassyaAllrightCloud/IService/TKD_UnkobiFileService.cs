using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Linq;
using HassyaAllrightCloud.Domain.Dto;
using System;
using HassyaAllrightCloud.Commons.Constants;
using SharedLibraries.UI.Services;

namespace HassyaAllrightCloud.IService
{
    public interface ITKD_UnkobiFileService
    {
        Task<List<FileInfoData>> getDataFileInfo(int TenantCdSeq, string UkeNo, short UnkRen);
        List<Unkobidatafile> getInfoUnkobi(int TenantCdSeq,string UkeNo);
        string DeleteFile(int TenantCdSeq, string UkeNo, short UnkRen, int FileNo);
    }
    public class TKD_UnkobiFileService : ITKD_UnkobiFileService
    {
        private readonly KobodbContext _context;
        private ISharedLibrariesApi _s3Service;
        public TKD_UnkobiFileService(KobodbContext context, ISharedLibrariesApi s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }
        public string DeleteFile(int TenantCdSeq, string UkeNo, short UnkRen, int FileNo)
        {
            var updateukobifile = _context.TkdUnkobiFile.Find(TenantCdSeq, UkeNo, UnkRen, FileNo);
            updateukobifile.SiyoKbn = 2;
            updateukobifile.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            updateukobifile.UpdTime = DateTime.Now.ToString("HHmmss");
            updateukobifile.UpdPrgId = Common.UpdPrgId;
            updateukobifile.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            _context.TkdUnkobiFile.Update(updateukobifile);
            _context.SaveChanges();
            return updateukobifile.UpdYmd + updateukobifile.UpdTime;
        }
        public List<Unkobidatafile>  getInfoUnkobi(int TenantCdSeq,string UkeNo)
        {
            return (from TKD_Unkobi in _context.TkdUnkobi
                    join TKD_Yyksho in _context.TkdYyksho on TKD_Unkobi.UkeNo equals TKD_Yyksho.UkeNo into TKD_Yyksho_join
                    from TKD_Yyksho in TKD_Yyksho_join.DefaultIfEmpty()
                    where TKD_Unkobi.UkeNo == UkeNo
                    && TKD_Yyksho.TenantCdSeq == TenantCdSeq
                    select new Unkobidatafile()
                    {
                        UnkRen = TKD_Unkobi.UnkRen,
                        UkeNo=TKD_Unkobi.UkeNo,
                        HaiSTime=TKD_Unkobi.HaiStime,
                        HaiSYmd=TKD_Unkobi.HaiSymd,
                        KikYmd=TKD_Unkobi.KikYmd,
                        KikTime=TKD_Unkobi.KikTime,
                        SyuKoTime=TKD_Unkobi.SyuKoTime,
                        SyukoYmd=TKD_Unkobi.SyukoYmd,
                        TouChTime=TKD_Unkobi.TouChTime,
                        TouYmd=TKD_Unkobi.TouYmd,
                    }
                    ).ToList();
        }
        public async Task<List<FileInfoData>> getDataFileInfo(int TenantCdSeq, string UkeNo, short UnkRen)
        {
            List<FileInfoData> Result = _context.TkdUnkobiFile.Where(t => t.TenantCdSeq == TenantCdSeq && t.UkeNo == UkeNo && t.UnkRen == UnkRen).Select(x => new FileInfoData
                {
                    UkeNo = x.UkeNo,
                    TenantCdSeq = x.TenantCdSeq,
                    UnkRen = x.UnkRen,
                    FileNm = null,
                    FileNo = x.FileNo, 
                    FileSize = 0,
                    FolderId = x.FolderId,
                    FileId = x.FileId,
                    SiyoKbn = x.SiyoKbn,
                    UpdYmd = x.UpdYmd,
                    UpdTime = x.UpdTime }).ToList();
            if (Result.Count > 0)
            {
                var Files = await _s3Service.GetFilesAsync(Result[0].FolderId);
                foreach(FileInfoData File in Result)
                {
                    var FileToGet = Files.Find(file => file.EncryptedId == File.FileId);
                    File.FileNm = FileToGet?.Name;
                    File.FileSize = FileToGet == null ? 0 :FileToGet.Size;
                }
            }
            return Result;
        }

    }
}
