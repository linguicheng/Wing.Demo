using System;
using Wing.Saga.Client;

namespace Saga.Bank.ABC.TransferSagaUnits
{
    [Serializable]
    public class MyAccountUnitModel : UnitModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string BankNo { get; set; }

        /// <summary>
        /// 转出金额
        /// </summary>
        public double Amount { get; set; }
    }
}
