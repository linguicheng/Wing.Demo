using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wing.Saga.Client;

namespace Saga.Bank.ABC.TransferSagaUnits
{
    /// <summary>
    /// 当前账户操作
    /// </summary>
    public class MyAccountSagaUnit : SagaUnit<MyAccountUnitModel>
    {
        public override Task<SagaResult> Cancel(MyAccountUnitModel model, SagaResult previousResult)
        {
            MyAccount.Balance += model.Amount;
            return Task.FromResult(new SagaResult());
        }

        public override Task<SagaResult> Commit(MyAccountUnitModel model, SagaResult previousResult)
        {
            var result = new SagaResult();
            if (MyAccount.Balance < model.Amount)
            {
                result.Success = false;
                result.Msg = "转账失败，当前账户余额不足！";
            }
            MyAccount.Balance -= model.Amount;
            return Task.FromResult(result);
        }
    }
}
