using System.Net.Http;
using System;
using System.Threading.Tasks;
using Wing;
using Wing.Saga.Client;
using Wing.ServiceProvider;
using Newtonsoft.Json;
using Wing.Result;
using System.Text;

namespace Saga.Bank.ABC.TransferSagaUnits
{
    /// <summary>
    /// 账户转出操作
    /// </summary>
    public class TransferOutSagaUnit : SagaUnit<TransferOutUnitModel>
    {
        private readonly IServiceFactory _serviceFactory = App.GetService<IServiceFactory>();
        private readonly IHttpClientFactory _httpClientFactory = App.GetService<IHttpClientFactory>();

        public override Task<SagaResult> Cancel(TransferOutUnitModel model, SagaResult previousResult)
        {
            throw new NotImplementedException();
        }

        public override Task<SagaResult> Commit(TransferOutUnitModel model, SagaResult previousResult)
        {
            return _serviceFactory.InvokeAsync("Saga.Bank.ICBC", async serviceAddr =>
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(serviceAddr.ToString());
                var response = await client.PostAsync("/TransferReceive", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                var sagaResult = new SagaResult();
                if (response.IsSuccessStatusCode)
                {
                    var apiStrResult = await response.Content.ReadAsStringAsync();
                    var apiResult = JsonConvert.DeserializeObject<ApiResult<bool>>(apiStrResult);
                    if (apiResult.Code == ResultType.Success)
                    {
                        sagaResult.Success = apiResult.Data;
                    }
                    else
                    {
                        sagaResult.Success = false;
                    }
                    sagaResult.Msg = apiResult.Msg;
                }
                else
                {
                    sagaResult.Success= false;
                    sagaResult.Msg = $"调用工商银行接口失败，http状态码：{(int)response.StatusCode}";
                }
                return sagaResult;
            });
        }
    }
}
