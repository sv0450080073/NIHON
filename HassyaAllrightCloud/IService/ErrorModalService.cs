using Blazored.Modal;
using Blazored.Modal.Services;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Pages.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IErrorHandlerService
    {
        void ShowErrorPopup(string title, string message);
        void HandleError(Exception ex);
    }
    public class ErrorHandlerService : IErrorHandlerService
    {
        protected readonly IModalService modalService;
        private ILogger<ErrorHandlerService> _logger;

        public ErrorHandlerService(IModalService modalService, ILogger<ErrorHandlerService> logger)
        {
            this.modalService = modalService;
            _logger = logger;
        }
        public void ShowErrorPopup(string title, string message)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(ErrorModal.ErrorTitle), title);
            parameters.Add(nameof(ErrorModal.ErrorMessage), message);

            modalService.Show<ErrorModal>(null, parameters);
        }

        public void HandleError(Exception ex)
        {
            //_logger.LogError(ex.GetOriginalException()?.Message);
            ShowErrorPopup("エラー", ex.GetOriginalException()?.Message); //TODO: Set message to null after implementing logging
        }
    }
}
