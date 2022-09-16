using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HassyaAllrightCloud.IService
{
    public interface IScheduleCustomGroupService
    {
        Task<List<GroupScheduleInfo>> Get(int SyainCdSeq);
        Task<bool> Update(CustomGroupScheduleForm customGroupScheduleForm);
        Task<bool> Delete(int GroupId);
    }

    public class ScheduleCustomGroupService : IScheduleCustomGroupService
    {
        private readonly KobodbContext _dbContext;

        public ScheduleCustomGroupService(KobodbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<GroupScheduleInfo>> Get(int SyainCdSeq)
        {
            List<GroupScheduleWithMembers> GroupScheduleWithMembers = await (
                from s in _dbContext.TkdSchCusGrp
                join m in _dbContext.TkdSchCusGrpMem
                    on s.CusGrpSeq equals m.CusGrpSeq
                where s.SiyoKbn == 1
                    && s.SyainCdSeq == SyainCdSeq
                orderby s.CusGrpSeq,
                    s.SyainCdSeq
                select new GroupScheduleWithMembers
                {
                    GroupId = s.CusGrpSeq,
                    GroupName = s.GrpNnm,
                    MemberId = m.SyainCdSeq
                }).ToListAsync();
            List<GroupScheduleInfo> Result = new List<GroupScheduleInfo>();
            foreach (GroupScheduleWithMembers GroupScheduleWithMember in GroupScheduleWithMembers)
            {
                if (Result.Count() == 0 || Result[Result.Count() - 1].GroupId != GroupScheduleWithMember.GroupId)
                {
                    Result.Add(new GroupScheduleInfo
                    {
                        CompanyId = 0,
                        GroupId = GroupScheduleWithMember.GroupId,
                        GroupName = GroupScheduleWithMember.GroupName,
                        MembersId = new List<int> { GroupScheduleWithMember.MemberId }
                    });
                }
                else
                {
                    Result[Result.Count() - 1].MembersId.Add(GroupScheduleWithMember.MemberId);
                }
            }
            return await Task.FromResult(Result);

        }

        public async Task<bool> Update(CustomGroupScheduleForm customGroupScheduleForm)
        {
            TkdSchCusGrp GroupToBeUpdate;
            List<int> MembersTobeAddCdSeq = new List<int>();
            List<int> MembersTobeDeleteCdSeq = new List<int>();
            if (customGroupScheduleForm.GroupSeq != null)
            {
                GroupToBeUpdate = _dbContext.TkdSchCusGrp.Find(customGroupScheduleForm.GroupSeq);
                List<int> GroupMembers = await (from m in _dbContext.TkdSchCusGrpMem
                                                where m.CusGrpSeq == customGroupScheduleForm.GroupSeq
                                                select m.SyainCdSeq).ToListAsync();
                MembersTobeAddCdSeq = customGroupScheduleForm.StaffList.FindAll(x => !GroupMembers.Contains(x.SyainCdSeq)).Select(x => x.SyainCdSeq).ToList();
                MembersTobeDeleteCdSeq = GroupMembers.FindAll(x => customGroupScheduleForm.StaffList.FindAll(s => s.SyainCdSeq == x).Count() == 0);
            }
            else
            {
                GroupToBeUpdate = new TkdSchCusGrp() { SiyoKbn = 1, SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq };
                MembersTobeAddCdSeq = customGroupScheduleForm.StaffList.Select(x => x.SyainCdSeq).ToList();
            }
            GroupToBeUpdate.GrpNnm = customGroupScheduleForm.GroupName;
            GroupToBeUpdate.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
            GroupToBeUpdate.UpdTime = DateTime.Now.ToString("HHmmss");
            GroupToBeUpdate.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
            GroupToBeUpdate.UpdPrgId = Common.UpdPrgId;

            using (IDbContextTransaction dbTran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (customGroupScheduleForm.GroupSeq != null)
                    {
                        _dbContext.TkdSchCusGrp.Update(GroupToBeUpdate);
                    }
                    else
                    {
                        _dbContext.TkdSchCusGrp.Add(GroupToBeUpdate);
                    }
                    _dbContext.SaveChanges();
                    foreach (int MemberTobeAddCdSeq in MembersTobeAddCdSeq)
                    {
                        TkdSchCusGrpMem MemberTobeAdd = new TkdSchCusGrpMem
                        {
                            CusGrpSeq = GroupToBeUpdate.CusGrpSeq,
                            SyainCdSeq = MemberTobeAddCdSeq,
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                            UpdTime = DateTime.Now.ToString("HHmmss"),
                            UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                            UpdPrgId = Common.UpdPrgId
                        };
                        _dbContext.TkdSchCusGrpMem.Add(MemberTobeAdd);
                    }
                    foreach (int MemberTobeDeleteCdSeq in MembersTobeDeleteCdSeq)
                    {
                        TkdSchCusGrpMem MemberTobeDelete = _dbContext.TkdSchCusGrpMem.Find(GroupToBeUpdate.CusGrpSeq, MemberTobeDeleteCdSeq);
                        _dbContext.TkdSchCusGrpMem.Remove(MemberTobeDelete);
                    }
                    await _dbContext.SaveChangesAsync();
                    dbTran.Commit();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    dbTran.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    return false;
                }
            }
        }
        public async Task<bool> Delete(int GroupId)
        {
            TkdSchCusGrp GroupToBeDelete = _dbContext.TkdSchCusGrp.Find(GroupId);
            List<TkdSchCusGrpMem> MembersToBeDelete = await (
                from g in _dbContext.TkdSchCusGrpMem
                where g.CusGrpSeq == GroupId
                select g).ToListAsync();
            using (IDbContextTransaction dbTran = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.TkdSchCusGrp.Remove(GroupToBeDelete);
                    foreach (TkdSchCusGrpMem MemberToBeDelete in MembersToBeDelete)
                    {
                        _dbContext.TkdSchCusGrpMem.Remove(MemberToBeDelete);
                    }
                    await _dbContext.SaveChangesAsync();
                    dbTran.Commit();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    dbTran.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    return false;
                }
            }
        }
    }
}
