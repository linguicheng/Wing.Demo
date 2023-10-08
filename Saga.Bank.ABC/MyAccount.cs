namespace Saga.Bank.ABC
{
    public class MyAccount
    {
        /// <summary>
        /// 当前账户余额
        /// </summary>
        public static double Balance { get; set; } = 10000;

        /// <summary>
        /// 银行账号
        /// </summary>
        public static readonly string BankNo = "123456";
    }
}
