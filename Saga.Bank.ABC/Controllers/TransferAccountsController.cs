using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Saga.Bank.ABC.TransferSagaUnits;
using System;
using Wing.Persistence.Saga;
using Wing.Saga.Client;

namespace Saga.Bank.ABC.Controllers
{
    /// <summary>
    /// 转账
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TransferAccountsController : ControllerBase
    {
        public TransferAccountsController()
        {
        }

        /// <summary>
        /// 当前账户余额
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            return $"我是中国农业银行账户，当前账户余额为：{MyAccount.Balance}¥";
        }


        [HttpGet("{amount}")]
        public bool Get(double amount)
        {
            if (amount <= 0)
            {
                throw new Exception("转账金额必须大于0");
            }
            var result = Wing.Saga.Client.Saga.Start("跨行转账", new SagaOptions { TranPolicy = TranPolicy.Forward })
                   .Then(new MyAccountSagaUnit(), new MyAccountUnitModel
                   {
                       Name = "当前账户扣减",
                       BankNo = MyAccount.BankNo,
                       Amount = 1000
                   })
                  .Then(new TransferOutSagaUnit(), new TransferOutUnitModel
                  {
                      Name = "调用收款行接口",
                      BankNo = "987654321",
                      Amount = 1000,
                      BankName = "中国工商银行"
                  })
                  .End();
            if (!result.Success)
            {
                throw new Exception(result.Msg);
            }
            return result.Success;
        }
    }
}
