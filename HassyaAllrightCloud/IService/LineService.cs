using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using LexLibrary.Line.NotifyBot;
using LexLibrary.Line.NotifyBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface ILineService
    {
        IEnumerable<LineUser> GetLineUsers();
        bool SaveLineUser(LineUser lineUser);
        Task<bool> SendMessage(List<string> accessTokens, string message);
    }
    public class LineService : ILineService
    {
        private readonly LineNotifyBotApi _lineNotifyBotApi = null;
        private readonly KobodbContext context;
        public LineService(KobodbContext context, LineNotifyBotApi lineNotifyBotApi)
        {
            this.context = context;
            _lineNotifyBotApi = lineNotifyBotApi;
        }

        public IEnumerable<LineUser> GetLineUsers()
        {
            return context.LineUser.AsEnumerable();
        }

        public bool SaveLineUser(LineUser lineUser)
        {
            try
            {
                context.LineUser.Add(lineUser);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendMessage(List<string> accessTokens, string message)
        {
            try
            {
                foreach (var accessToken in accessTokens)
                {
                    await _lineNotifyBotApi.Notify(new NotifyRequestDTO
                    {
                        AccessToken = accessToken,
                        Message = message
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
