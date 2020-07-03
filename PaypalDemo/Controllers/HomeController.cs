using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PaypalDemo.Infrastructure.DB;
using PaypalDemo.Infrastructure.Helper;
using PaypalDemo.Infrastructure.Mapper;
using PaypalDemo.Models;
using PaypalDemo.Models.Enum;
using PaypalDemo.Models.test;
using PaypalDemo.Models.test.DTO;
using static PaypalDemo.Models.PaypalAgreementRequest;
using Payer = PaypalDemo.Models.PaypalAgreementRequest.Payer;

namespace PaypalDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _myDbContext;
        private PaypalRecordDto dto;
        private readonly string expectedCurrencyCode = "usd";
        private readonly string suggestedAction = "Please contact Customer Support";
        private readonly string topUpTransactionType = ApiTransactionType.top.GetDescription();

        private AddPaypalRecordMapper _addPaypalRecordMapper;
        private AddApiTransactionMapper _addApiTransactionMapper;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration,
            MyDbContext myDbContext)
        {
            _logger = logger;
            this._configuration = configuration;
            this._myDbContext = myDbContext;
            this._addPaypalRecordMapper = new AddPaypalRecordMapper();
            this._addApiTransactionMapper = new AddApiTransactionMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("[controller]/paypaltopup")]
        public async Task<object> paypaltopup()
        {
            var token = await GetPaypalAccessToken();
            var cus = _myDbContext.PaypalTokenId.Where(s => s.Email == "sb-iuunk2467458@personal.example.com").FirstOrDefault();
            var agreementsUrl = "https://api.sandbox.paypal.com/v1/billing-agreements/agreements";

            billingAgreementsResponse result = new billingAgreementsResponse();
            if (string.IsNullOrWhiteSpace(cus.token_id))
            {
                return new Exception("no token_id");
            }
            else
            {
                Dictionary<string, string> getTokenAuthData = new Dictionary<string, string>();
                getTokenAuthData.Add("Authorization", $"Bearer {token.accessToken}");
                var uri = new Uri(agreementsUrl);

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                foreach (KeyValuePair<string, string> keyValuePair in getTokenAuthData ?? new Dictionary<string, string>())
                    httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);

                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = uri;
                BillingAgreementsRequest billingAgreementsRequest = new BillingAgreementsRequest();
                billingAgreementsRequest.token_id = cus.token_id;
                httpRequestMessage.Content = new StringContent(
                      JsonConvert.SerializeObject(billingAgreementsRequest),
                      Encoding.UTF8, "application/json");
                HttpClient httpClient = new HttpClient();

                var response = await httpClient.SendAsync(httpRequestMessage);
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    return new Exception("no token_id");
                }
                string jsonString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<billingAgreementsResponse>(jsonString);
                await PaypalTopUp(result);
            }

            return result;
        }

        private async Task<PaymentResponse> PaypalTopUp(billingAgreementsResponse par)
        {
            var token = await GetPaypalAccessToken();
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                intent = "sale",
                payer = new PaymentRequest.Payer()
                {
                    payment_method = "PAYPAL",
                    funding_instruments = new List<PaymentRequest.Funding_Instruments>
                    {
                       new PaymentRequest.Funding_Instruments()
                       {
                           billing=new PaymentRequest.Billing()
                           {
                               billing_agreement_id=par.id
                           }
                       }
                    }.ToArray()
                },
                transactions = new List<PaymentRequest.Transaction>()
                {
                    new PaymentRequest.Transaction()
                    {
                        amount  =new PaymentRequest.Amount()
                        {
                            currency="USD",
                            total="3"
                        },
                        description="The balance is negative",
                        item_list=new PaymentRequest.Item_List()
                        {
                            items=new List<PaymentRequest.Item>()
                            {
                                new PaymentRequest.Item()
                                {
                                    sku="Stored value",
                                    name= "Stored value",
                                    description= "The balance is negative",
                                    quantity= "1",
                                    price= "3.00",
                                    currency= "USD",
                                    tax= "0",
                                    url= "https=//devusr.returnshelper.com/"
                                }
                            }.ToArray()
                        },
                    }
                }.ToArray(),
                redirect_urls = new PaymentRequest.Redirect_Urls()
                {
                    cancel_url = "https://devusr.returnshelper.com/",
                    return_url = "https://devusr.returnshelper.com/",
                }
            };
            Dictionary<string, string> getTokenAuthData = new Dictionary<string, string>();
            getTokenAuthData.Add("Authorization", $"Bearer {token.accessToken}");
            var uri = new Uri("https://api.sandbox.paypal.com/v1/payments/payment");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            foreach (KeyValuePair<string, string> keyValuePair in getTokenAuthData ?? new Dictionary<string, string>())
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);

            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = uri;

            httpRequestMessage.Content = new StringContent(
                  JsonConvert.SerializeObject(paymentRequest),
                  Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.SendAsync(httpRequestMessage);
            string jsonString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.Created)
            {
                new Exception("失敗");
            }

            var result = JsonConvert.DeserializeObject<PaymentResponse>(jsonString);

            return result;
        }

        [HttpGet]
        [Route("[controller]/paypalapproval")]
        public async Task<LinkAgreement> paypalapproval()
        {
            string baseurl = "";
            try
            {
                var token = await GetPaypalAccessToken();
                var cus = _myDbContext.PaypalTokenId.Where(s => s.Email == "sb-855xe2470314@personal.example.com").FirstOrDefault();

                PaypalAgreement result = new PaypalAgreement();
                if (string.IsNullOrWhiteSpace(cus.token_id))
                {
                    PaypalAgreementRequest paypalAgreementRequest = new PaypalAgreementRequest()
                    {
                        description = "The balance is negative",
                        shipping_address = new Shipping_Address
                        {
                            line1 = "HK",
                            city = "HK",
                            state = "HK",
                            postal_code = "95111",
                            country_code = "HK",
                            recipient_name = "ReturnHelper"
                        },
                        payer = new Payer
                        {
                            payment_method = "PAYPAL"
                        },
                        plan = new Plan
                        {
                            type = "MERCHANT_INITIATED_BILLING",
                            merchant_preferences = new Merchant_Preferences()
                            {
                                accepted_pymt_type = "INSTANT",
                                immutable_shipping_address = true,
                                skip_shipping_address = false,
                                cancel_url = "https://devusr.returnshelper.com/",
                                return_url = "https://devusr.returnshelper.com/",
                            }
                        }
                    };
                    Dictionary<string, string> getTokenAuthData = new Dictionary<string, string>();
                    getTokenAuthData.Add("Authorization", $"Bearer {token.accessToken}");
                    var uri = new Uri($"https://api.sandbox.paypal.com/v1/billing-agreements/agreement-tokens");

                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    foreach (KeyValuePair<string, string> keyValuePair in getTokenAuthData ?? new Dictionary<string, string>())
                        httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);

                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = uri;

                    httpRequestMessage.Content = new StringContent(
                       JsonConvert.SerializeObject(paypalAgreementRequest),
                       Encoding.UTF8, "application/json");
                    HttpClient httpClient = new HttpClient();

                    var response = await httpClient.SendAsync(httpRequestMessage);
                    string jsonString = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<PaypalAgreement>(jsonString);
                    cus.token_id = result.token_id;
                    _myDbContext.Entry(cus).CurrentValues.SetValues(cus);
                    _myDbContext.SaveChanges();
                    baseurl = result.links.Where(s => s.rel == "approval_url").FirstOrDefault().href;
                }
                else
                {
                    baseurl = $@"https://www.sandbox.paypal.com/agreements/approve?ba_token={cus.token_id}";
                }
            }
            catch (Exception ex)
            {
                var aa = ex.ToString();
            }
            LinkAgreement linkAgreement = new LinkAgreement();
            linkAgreement.rel = baseurl;

            return linkAgreement;
        }

        [HttpPost]
        [Route("[controller]/paypal")]
        public async Task<ActionResult<object>> paypal(PaypalOrderParamterModel par)
        {
            _myDbContext.Add(par);
            _myDbContext.SaveChanges();

            //取TOKEN
            var token = await GetPaypalAccessToken();

            PaypalRecordPayload paypalTransaction = new PaypalRecordPayload { paypalOrderId = par.orderID };

            dto = new PaypalRecordDto { paypalRecordPayload = paypalTransaction };
            PaypalAccessTokenResponse paypalAccessTokenResponse = new PaypalAccessTokenResponse();

            paypalAccessTokenResponse = token;
            dto.paypalAccessTokenResponse = paypalAccessTokenResponse;
            //取ORDER資訊
            PaypalCapturePaymentResponse paypalCapturePaymentResponse = new PaypalCapturePaymentResponse();
            paypalCapturePaymentResponse = await CapturePaymentForOrder(par.orderID, token.accessToken);
            dto.paypalCapturePaymentResponse = paypalCapturePaymentResponse;
            if (paypalCapturePaymentResponse.name.IsNullOrEmpty())
            {
                GetCaptureResult(paypalCapturePaymentResponse);
            }

            ApiResponse response = new ApiResponse()
            {
                meta = new ApiResponseMeta()
            };

            if (CapturePaypalPaymentSuccess(response, dto))
            {
                //給公司代號
                dto.paypalRecordPayload.companyId = 1;
                //把資料存入PaypalRecord
                if (AddPaypalRecordSuccess(response, dto))
                {
                    try
                    {
                        var apikey = _myDbContext.Apis.FirstOrDefault();
                        PaypalRecordDto dto2 = await TopUpApiBalance(apikey.apiKey.ToString(), dto);
                        if (AddApiTransactionSuccess(response, dto2))
                        {
                            if (await UpdatePaypalRecordSuccess(response, dto2))
                            {
                                return dto;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var aa = ex.ToString();
                    }
                }
            }

            return dto;
        }

        [HttpGet]
        [Route("[controller]/paypalOrderID")]
        public async Task<ActionResult<Object>> paypalOrderID()
        {
            var token = await PaypalAccessToken();
            var request = new HttpRequestMessage();
            var data = _myDbContext.PaypalOrder.FirstOrDefault();
            Dictionary<string, string> getTokenAuthData = new Dictionary<string, string>();
            getTokenAuthData.Add("Authorization", $"Bearer {token.access_token}");
            getTokenAuthData.Add("PayPal-Request-Id", $"7b92603e-77ed-4896-8e78-5dea2050476a");
            getTokenAuthData.Add("PayPal-Partner-Attribution-Id", $"{data.payerID}");
            // getTokenAuthData.Add("Content-Type", $"application/json");
            var uri = new Uri($"https://api.sandbox.paypal.com/v2/checkout/orders/{data.orderID}/capture");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            try
            {
                foreach (KeyValuePair<string, string> keyValuePair in getTokenAuthData ?? new Dictionary<string, string>())
                    httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);
            }
            catch (Exception ex)
            {
                var aa = ex.ToString();
            }
            httpRequestMessage.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = uri;
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.SendAsync(httpRequestMessage);
            string jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaypalCapturePaymentResponseTest>(jsonString);
            return result;
        }

        private async Task<PaypalAccessTokenResponseDemo> PaypalAccessToken()
        {
            var clientID = _configuration.GetValue<string>("Paypal:ClientID");
            var secret = _configuration.GetValue<string>("Paypal:Secret");
            PaypalAccessTokenResponseDemo token = new PaypalAccessTokenResponseDemo();

            var request = new HttpRequestMessage();
            Dictionary<string, string> getTokenAuthData = new Dictionary<string, string>();
            getTokenAuthData.Add("Authorization", $"Basic {EncryptionHelper.GetBasicAuthorizationString(clientID, secret)}");
            getTokenAuthData.Add("Accept", $"application/json");
            getTokenAuthData.Add("Accept-Language", $"en_US");

            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });
            var uri = new Uri("https://api.sandbox.paypal.com/v1/oauth2/token");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            foreach (KeyValuePair<string, string> keyValuePair in getTokenAuthData ?? new Dictionary<string, string>())
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);

            httpRequestMessage.Content = content;
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = uri;
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.SendAsync(httpRequestMessage);

            string jsonString = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<PaypalAccessTokenResponseDemo>(jsonString);

            return token;
        }

        private async Task<PaypalAccessTokenResponse> GetPaypalAccessToken()
        {
            var clientID = _configuration.GetValue<string>("Paypal:ClientID");
            var secret = _configuration.GetValue<string>("Paypal:Secret");
            PaypalAccessTokenResponse token = new PaypalAccessTokenResponse();

            var request = new HttpRequestMessage();
            Dictionary<string, string> getTokenAuthData = new Dictionary<string, string>();
            getTokenAuthData.Add("Authorization", $"Basic {EncryptionHelper.GetBasicAuthorizationString(clientID, secret)}");
            getTokenAuthData.Add("Accept", $"application/json");
            getTokenAuthData.Add("Accept-Language", $"en_US");

            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });
            var uri = new Uri("https://api.sandbox.paypal.com/v1/oauth2/token");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            foreach (KeyValuePair<string, string> keyValuePair in getTokenAuthData ?? new Dictionary<string, string>())
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);

            httpRequestMessage.Content = content;
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = uri;
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.SendAsync(httpRequestMessage);

            string jsonString = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<PaypalAccessTokenResponse>(jsonString);

            return token;
        }

        private async Task<PaypalCapturePaymentResponse> CapturePaymentForOrder(string paypalOrderId,
            string paypalAccessToken)
        {
            var request = new HttpRequestMessage();
            var data = _myDbContext.PaypalOrder.Where(s => s.orderID == paypalOrderId).FirstOrDefault();
            Dictionary<string, string> getTokenAuthData = new Dictionary<string, string>();
            getTokenAuthData.Add("Authorization", $"Bearer {paypalAccessToken}");
            getTokenAuthData.Add("PayPal-Request-Id", $"7b92603e-77ed-4896-8e78-5dea2050476a");
            getTokenAuthData.Add("PayPal-Partner-Attribution-Id", $"{data.payerID}");
            // getTokenAuthData.Add("Content-Type", $"application/json");
            var uri = new Uri($"https://api.sandbox.paypal.com/v2/checkout/orders/{data.orderID}/capture");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            try
            {
                foreach (KeyValuePair<string, string> keyValuePair in getTokenAuthData ?? new Dictionary<string, string>())
                    httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);
            }
            catch (Exception ex)
            {
                var aa = ex.ToString();
            }
            httpRequestMessage.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = uri;
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.SendAsync(httpRequestMessage);
            string jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaypalCapturePaymentResponse>(jsonString);
            return result;
        }

        private void GetCaptureResult(PaypalCapturePaymentResponse paypalCapturePaymentResponse)
        {
            dto.paypalRecordPayload.paypalCaptureId = paypalCapturePaymentResponse.paypalPurchaseUnitList.First()
                .payments.paypalCaptureList.First().captureId;
            dto.paypalRecordPayload.amount = paypalCapturePaymentResponse.paypalPurchaseUnitList.First()
                .payments.paypalCaptureList.First().amount.value;
            dto.paypalRecordPayload.currencyCode = paypalCapturePaymentResponse.paypalPurchaseUnitList.First()
                .payments.paypalCaptureList.First().amount.currencyCode.ToLower();
        }

        private bool CapturePaypalPaymentSuccess(ApiResponse response, PaypalRecordDto paypalRecordDto)
        {
            if (!paypalRecordDto.resultMessage.IsNullOrEmpty())
            {
                response.meta.error.Add("message", paypalRecordDto.resultMessage);
                return false;
            }

            if (!PaypalPaymentCurrencyIsUsd(response, paypalRecordDto))
            {
                return false;
            }

            return true;
        }

        private bool PaypalPaymentCurrencyIsUsd(ApiResponse response, PaypalRecordDto paypalRecordDto)
        {
            if (!paypalRecordDto.paypalRecordPayload.currencyCode.Equals(expectedCurrencyCode))
            {
                response.meta.error.Add("message",
                    $"Payment captured is not in currency: {expectedCurrencyCode.ToUpper()}. " +
                    $"Top up failed. {suggestedAction}");
                return false;
            }

            return true;
        }

        private bool AddPaypalRecordSuccess(ApiResponse response, PaypalRecordDto paypalRecordDto)
        {
            PaypalRecord paypalRecord = _addPaypalRecordMapper.AddPaypalRecord(paypalRecordDto.paypalRecordPayload);

            var data = _myDbContext.PaypalRecord.Where(s => s.paypalRecordId == paypalRecord.paypalRecordId);
            try
            {
                if (data.Count() > 0)
                {
                    _myDbContext.Entry(data).CurrentValues.SetValues(paypalRecord);
                    _myDbContext.SaveChanges();
                }
                else
                {
                    _myDbContext.Add(paypalRecord);
                    _myDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var aa = ex.ToString();
            }
            if (paypalRecord == null)
            {
                paypalRecordDto.resultMessage = $"Failed to add paypal payment record. {suggestedAction}";
                response.meta.error.Add("message", paypalRecordDto.resultMessage);
                return false;
            }

            return true;
        }

        public async Task<PaypalRecordDto> TopUpApiBalance(string userSessionapiKey,
            PaypalRecordDto paypalRecordDto)
        {
            AddApiTransactionRequest addApiTransactionRequest = new AddApiTransactionRequest
            {
                amount = paypalRecordDto.paypalRecordPayload.amount,
                currencyCode = paypalRecordDto.paypalRecordPayload.currencyCode,
                userApiKey = userSessionapiKey,
                notes = $"Paypal Order ID: {paypalRecordDto.paypalRecordPayload.paypalOrderId}; " +
                        $"Paypal Capture ID: {paypalRecordDto.paypalRecordPayload.paypalCaptureId}"
            };

            return await AddTopUpApiTransactionViaAdminApi(paypalRecordDto, addApiTransactionRequest);
        }

        private async Task<PaypalRecordDto> AddTopUpApiTransactionViaAdminApi(PaypalRecordDto paypalRecordDto,
           AddApiTransactionRequest addApiTransactionRequest)
        {
            AddApiTransactionResponse addApiTransactionResponse = new AddApiTransactionResponse();
            try
            {
                addApiTransactionRequest.transactionType = topUpTransactionType;
                var api = this._myDbContext.Apis.Where(s => s.apiKey.ToString() == addApiTransactionRequest.userApiKey).FirstOrDefault();
                addApiTransactionRequest.apiId = api.apiId;
                addApiTransactionRequest.createBy = api.apiId.ToString();
                ApiTransaction request = _addApiTransactionMapper.CreateApiTransaction(addApiTransactionRequest);

                string query =
               "call P_AddApiTransaction(@apiId,@headId,@currencyCode,@transactionType,@amount,@notes,@createBy)";
                MySqlParameter pApiId = new MySqlParameter("@apiId", request.apiId);
                MySqlParameter pHeadId = new MySqlParameter("@headId", request.headId);
                MySqlParameter pCurrencyCode = new MySqlParameter("@currencyCode", request.currencyCode);
                MySqlParameter pTransactionType = new MySqlParameter("@transactionType", request.transactionType);
                MySqlParameter pAmount = new MySqlParameter("@amount", request.amount);
                MySqlParameter pNotes = new MySqlParameter("@notes", request.notes);
                MySqlParameter pCreateBy = new MySqlParameter("@createBy", request.createBy);

                var data = this._myDbContext.ApiTransactions.FromSqlRaw(query, pApiId, pHeadId, pCurrencyCode,
                pTransactionType,
                pAmount, pNotes, pCreateBy).AsNoTracking().AsEnumerable().ToList().FirstOrDefault();

                addApiTransactionResponse.apiTransactionId = data.apiTransactionId;
                addApiTransactionResponse.transactionNumber = data.transactionNumber;
            }
            catch (Exception ex)
            {
                var aa = ex.ToString();
                addApiTransactionResponse = JsonConvert.DeserializeObject<AddApiTransactionResponse>(ex.Message) ??
                                            ErrorAddApiTransactionResponse();
                paypalRecordDto.resultMessage = addApiTransactionResponse.meta.error["message"];
            }

            if (paypalRecordDto.resultMessage.IsNullOrEmpty())
            {
                paypalRecordDto.paypalRecordPayload.apiTransactionNumber = addApiTransactionResponse.transactionNumber;
                paypalRecordDto.paypalRecordPayload.apiTransactionId = addApiTransactionResponse.apiTransactionId;
            }

            return paypalRecordDto;
        }

        private AddApiTransactionResponse ErrorAddApiTransactionResponse()
        {
            return new AddApiTransactionResponse
            {
                meta = new ApiResponseMeta
                {
                    error = new Dictionary<string, string>()
                    {
                        {"message", "Unexpected error during add transaction."}
                    }
                }
            };
        }

        private bool AddApiTransactionSuccess(ApiResponse response, PaypalRecordDto paypalRecordDto)
        {
            if (!paypalRecordDto.resultMessage.IsNullOrEmpty())
            {
                response.meta.error.Add("message", $"{paypalRecordDto.resultMessage} {suggestedAction}");
                return false;
            }

            return true;
        }

        private async Task<bool> UpdatePaypalRecordSuccess(ApiResponse response, PaypalRecordDto paypalRecordDto)
        {
            var data = _myDbContext.PaypalRecord.Where(s => s.paypalOrderId == paypalRecordDto.paypalRecordPayload.paypalOrderId);
            PaypalRecord paypalRecord = data.FirstOrDefault();
            try
            {
                if (paypalRecord != null)
                {
                    paypalRecord.apiTransactionNumber = paypalRecordDto.paypalRecordPayload.apiTransactionNumber;
                    paypalRecord.apiTransactionId = paypalRecordDto.paypalRecordPayload.apiTransactionId;
                    _myDbContext.Entry(paypalRecord).CurrentValues.SetValues(paypalRecord);
                    _myDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var aaa = ex.ToString();
            }

            if (paypalRecord == null)
            {
                paypalRecordDto.resultMessage = $"Failed to update paypal payment record. {suggestedAction}";
                response.meta.error.Add("message", paypalRecordDto.resultMessage);
                return false;
            }

            return true;
        }
    }
}