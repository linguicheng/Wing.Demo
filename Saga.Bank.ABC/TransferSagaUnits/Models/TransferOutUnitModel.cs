using System;
using Wing.Saga.Client;

namespace Saga.Bank.ABC.TransferSagaUnits
{
    [Serializable]
    public class TransferOutUnitModel : UnitModel
    {
        /// <summary>
        /// 收款账号
        /// </summary>
        public string BankNo { get; set; }

        /// <summary>
        /// 收款行
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 接收金额
        /// </summary>
        public double Amount { get; set; }
    }
}
