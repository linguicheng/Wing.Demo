using Microsoft.AspNetCore.Mvc;
using Saga.Bank.ICBC.Models;
using System;

namespace Saga.Bank.ICBC.Controllers
{
    /// <summary>
    /// 转账
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TransferReceiveController : ControllerBase
    {
        private static bool _result = false;
        public TransferReceiveController()
        {
        }

        /// <summary>
        /// 当前账户余额
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return $"我是中国工商银行账户，当前账户余额为：{MyAccount.Balance}¥";
        }

        /// <summary>
        /// 手动控制跨行转账收款是否成功，测试需要
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>\
        [HttpGet("{result}")]
        public bool Get(int result)
        {
            _result = result == 1; 
            return _result;
        }

        /// <summary>
        /// 跨行转账收款动作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public bool Post(ReceivedModel model)
        {
            if (model.BankNo != MyAccount.BankNo)
            {
                throw new Exception("账号不存在！");
            }
            if (!_result)
            {
                return false;
            }
            MyAccount.Balance += model.Amount;
            return true;
        }
    }
}
