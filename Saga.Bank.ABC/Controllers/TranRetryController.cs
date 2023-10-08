﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wing.Persistence.Saga;
using Wing.Saga.Client;

namespace Saga.Bank.ABC.Controllers
{
    [ApiController]
    [Route("Wing/Saga/[controller]/[action]")]
    [Authorize]
    public class TranRetryController : ControllerBase
    {
        private readonly ITranRetryService _tranRetryService;
        public TranRetryController(ITranRetryService tranRetryService)
        {
            _tranRetryService = tranRetryService;
        }

        public Task<ResponseData> Commit(RetryData retryData)
        {
           return  _tranRetryService.Commit(retryData);
        }

        public Task<ResponseData> Cancel(RetryData retryData)
        {
            return _tranRetryService.Cancel(retryData);
        }
    }
}
