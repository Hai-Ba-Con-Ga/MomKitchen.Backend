using System.Globalization;
using System.Text;
using Microsoft.Extensions.Primitives;
using MK.Domain.Dto.Request.Payment;
using MK.Domain.Enum;
using MK.Service.Util;

namespace MK.Service.Service
{
    public class VnpayService : BaseService, IPaymentService
    {
        public const string VERSION = "2.1.0";
        private SortedList<string, string> _requestData = new SortedList<string, string>();
        private SortedList<string, string> _responseData = new SortedList<string, string>();
        public VnpayService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        #region Request
        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(key))
            {
                _requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in _requestData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();
            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = CommonUtil.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;
            return baseUrl;
        }
        #endregion Request

        #region Response
        public void AddResponseData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            string retValue;
            if (_responseData.TryGetValue(key, out retValue))
            {
                return retValue;
            }
            else
            {
                return string.Empty;
            }
        }
        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = CommonUtil.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetResponseData()
        {
            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }

            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            return data.ToString();
        }

        #endregion

        public class VnPayCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                var vnpCompare = CompareInfo.GetCompareInfo("en-US");
                return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
            }
        }
        public async Task<string> Create(CreateOrderPaymentReq payment, string origin)
        {
            _requestData.Clear();
            var vnp_Returnurl = origin + AppConfig.VnpayConfig.ReturnUrl;
            var vnp_Url = AppConfig.VnpayConfig.Url;
            var vnp_TmnCode = AppConfig.VnpayConfig.TmnCode;
            var vnp_HashSecret = AppConfig.VnpayConfig.HashSecret;
            //TODO: check payment type and order
            var paymentType = await _unitOfWork.PaymentType.GetById(payment.PaymentTypeId);
            if (paymentType == null)
            {
                return "Payment type not found";
            }
            // var order = await _unitOfWork.Order.GetById(payment.OrderId);
            // if (order == null)
            // {
            //     return "Order not found";
            // }

            var entity = _mapper.Map<OrderPayment>(payment);
            await _unitOfWork.OrderPayment.CreateAsync(entity, true);

            // if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            // {
            //     lblMessage.Text = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config";
            //     return;
            // }
            AddRequestData("vnp_Version", VERSION);
            AddRequestData("vnp_Command", "pay");
            AddRequestData("vnp_TmnCode", vnp_TmnCode);
            AddRequestData("vnp_Amount", ((long)entity.Amount * 100).ToString());
            AddRequestData("vnp_CreateDate", entity.CreatedDate.ToString("yyyyMMddHHmmss"));
            AddRequestData("vnp_CurrCode", "VND");
            AddRequestData("vnp_Locale", "vn");
            AddRequestData("vnp_OrderInfo", "Thanh toan don hang: " + entity.Id);
            AddRequestData("vnp_OrderType", "other");
            AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            AddRequestData("vnp_IpAddr", "127.0.0.1");
            var txnRef = entity.Id.ToString();
            AddRequestData("vnp_TxnRef", txnRef); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            AddRequestData("vnp_ExpireDate", entity.CreatedDate.AddMonths(payment.LimitMonth).ToString("yyyyMMddHHmmss"));
            //AddRequestData("vnp_SecureHashType", "HMACSHA512");
            //AddRequestData("vnp_Bill_AccountId", entity.AccountId.ToString());
            string paymentUrl = CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }
        public async Task ProcessCallback(Dictionary<string, StringValues> queryData)
        {
            string msg = "";
            int isSuccess = 0;
            if (queryData.Count > 0)
            {
                string vnp_HashSecret = AppConfig.VnpayConfig.HashSecret;
                var vnpData = queryData;
                _responseData.Clear();
                foreach (string s in vnpData.Keys)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        AddResponseData(s, vnpData[s]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                Guid paymentId = Guid.Parse(GetResponseData("vnp_TxnRef").Split('|')[0]);
                long vnpTranId = Convert.ToInt64(GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = GetResponseData("vnp_ResponseCode");
                String vnp_SecureHash = queryData["vnp_SecureHash"];
                string vnp_TransactionStatus = GetResponseData("vnp_TransactionStatus");
                long vnp_Amount = Convert.ToInt64(GetResponseData("vnp_Amount")) / 100;
                String bankCode = queryData["vnp_BankCode"];
                bool checkSignature = ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var entity = await _unitOfWork.OrderPayment.GetById(paymentId);
                        if (entity != null)
                        {
                            // entity.AccountId = accountId;
                            entity.Status = PaymentStatus.Paid;
                            await _unitOfWork.OrderPayment.UpdateAsync(entity, true);
                            var limitMonth = int.Parse(GetResponseData("vnp_TxnRef").Split('|')[1]);
                        }
                        isSuccess = 1;
                    }
                    else
                    {
                        var entity = await _unitOfWork.OrderPayment.GetById(paymentId);
                        if (entity != null)
                        {
                            entity.Status = PaymentStatus.Failed;
                            await _unitOfWork.OrderPayment.UpdateAsync(entity, true);
                        }
                        msg = string.Format("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}",
                                 paymentId, vnpTranId, vnp_ResponseCode);

                    }

                }
                else
                {
                    msg = "There is an error during the process!";

                }
            }
            queryData.Add("be_msg", msg);
            queryData.Add("isSuccess", isSuccess.ToString());
        }


        public async Task<ResponseObject<OrderPayment>> GetById(Guid paymentId)
        {
            var payment = await _unitOfWork.OrderPayment.GetById(paymentId);
            if (payment == null)
            {
                return NotFound<OrderPayment>("Payment not found");
            }
            return Success(payment);
        }

        public async Task<PagingResponse<OrderPayment>> GetAll(PagingParameters pagingParam = null)
        {
            try
            {
                var queryHelper = new QueryHelper<OrderPayment>()
                {
                    PagingParams = pagingParam ??= new PagingParameters()
                };
                var payments = await _unitOfWork.OrderPayment.GetWithPagination(queryHelper);
                return Success(payments);

            }
            catch (Exception ex)
            {
                return BadRequests<OrderPayment>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> DeletePayment(Guid paymentId)
        {
            try
            {
                var payment = await _unitOfWork.OrderPayment.SoftDeleteAsync(t => t.Id == paymentId);
                return Success(payment > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }


    }
}