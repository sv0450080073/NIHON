using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Queries
{
    public class GetKarieiByHaishaQuery : IRequest<Dictionary<CommandMode, List<TkdKariei>>>
    {
        private readonly Dictionary<CommandMode, List<TkdHaisha>> _haishaCommands;
        private readonly BookingFormData _bookingData;
        private readonly string _ukeno;

        public GetKarieiByHaishaQuery(string ukeno, BookingFormData bookingData, Dictionary<CommandMode, List<TkdHaisha>> haishaCommands)
        {
            _haishaCommands = haishaCommands ?? throw new ArgumentNullException(nameof(haishaCommands));
            _bookingData = bookingData ?? throw new ArgumentNullException(nameof(bookingData));
            _ukeno = ukeno;
        }

        public class Handler : IRequestHandler<GetKarieiByHaishaQuery, Dictionary<CommandMode, List<TkdKariei>>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetHaishaByUkenoQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetHaishaByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<Dictionary<CommandMode, List<TkdKariei>>> Handle(GetKarieiByHaishaQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    List<TkdKariei> oldKarieis = _context.TkdKariei.Where(_ => _.UkeNo == request._ukeno.ToString() && _.UnkRen == 1).ToList(); //temp unkren = 1
                    
                    List<TkdKariei> removeKarieis = new List<TkdKariei>();
                    List<TkdKariei> updateKarieis = new List<TkdKariei>();
                    List<TkdKariei> createKarieis = new List<TkdKariei>();

                    List<TkdHaisha> removeHaishas = request._haishaCommands[CommandMode.Delete].ToList();
                    List<TkdHaisha> updateHaishas = request._haishaCommands[CommandMode.Update].ToList();
                    List<TkdHaisha> createHaishas = request._haishaCommands[CommandMode.Create].ToList();

                    //delete row case
                    var rowDeletes = oldKarieis.Where(_ => !request._bookingData.VehicleGridDataList.Where(row => row.RowID == _.SyaSyuRen.ToString()).Any());
                    if (rowDeletes?.Any() ?? false)
                    {
                        removeKarieis.AddRange(rowDeletes);
                    }

                    foreach(var vehicleRow in request._bookingData.VehicleGridDataList)
                    {
                        var haishaRemoves = removeHaishas.Where(_ => _.SyaSyuRen.ToString() == vehicleRow.RowID);
                        var haishaUpdates = updateHaishas.Where(_ => _.SyaSyuRen.ToString() == vehicleRow.RowID);
                        var haishaCreates = createHaishas.Where(_ => _.SyaSyuRen.ToString() == vehicleRow.RowID);
                        var oldKariei = oldKarieis.FirstOrDefault(_ => _.UkeNo == request._ukeno.ToString() && _.SyaSyuRen.ToString() == vehicleRow.RowID);
                        var priority = vehicleRow.PriorityAutoAssignBranch;

                        if (oldKariei != null && priority == null) // remove priority assign for branch
                        {
                            removeKarieis.Add(oldKariei);
                        }
                        else if (oldKariei == null && priority != null) // priority assign for branch
                        {
                            int assignedUpdate = haishaUpdates.Count(_ => _.Kskbn == 2);
                            int assignedInsert = haishaCreates.Count(_ => _.Kskbn == 2);

                            createKarieis.Add(new TkdKariei
                            {
                                UkeNo = request._ukeno.ToString(),
                                UnkRen = 1,
                                SyaSyuRen = Int16.Parse(vehicleRow.RowID),
                                KarieiRen = 1,
                                HenKai = 0,
                                KseigSeq = priority.EigyoCdSeq,
                                KariDai = (short)(assignedUpdate + assignedInsert),
                                SiyoKbn = 1,
                                UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                                UpdTime = DateTime.Now.ToString("HHmmss"),
                                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                UpdPrgId = "KJ1000",
                            });
                        }
                        else if (oldKariei != null && priority != null) // update quantity bus assigned
                        {
                            int assignedUpdate = haishaUpdates.Count(_ => _.Kskbn == 2);
                            int assignedInsert = haishaCreates.Count(_ => _.Kskbn == 2);

                            updateKarieis.Add(new TkdKariei
                            {
                                UkeNo = oldKariei.UkeNo,
                                UnkRen = oldKariei.UnkRen,
                                SyaSyuRen = oldKariei.SyaSyuRen,
                                KarieiRen = oldKariei.KarieiRen,
                                HenKai = (short)(oldKariei.HenKai + 1),
                                KseigSeq = vehicleRow.PriorityAutoAssignBranch.EigyoCdSeq,
                                KariDai = (short)(assignedUpdate + assignedInsert),
                                SiyoKbn = oldKariei.SiyoKbn,
                                UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                                UpdTime = DateTime.Now.ToString("HHmmss"),
                                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                                UpdPrgId = "KJ1000",
                            });
                        }
                    }

                    return Task.FromResult(new Dictionary<CommandMode, List<TkdKariei>>() {
                        { CommandMode.Delete,  removeKarieis },
                        { CommandMode.Update,  updateKarieis },
                        { CommandMode.Create,  createKarieis },
                    });
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
