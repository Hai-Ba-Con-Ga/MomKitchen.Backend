namespace MK.Service.Service
{
    public PaymentTypeService: BaseService, IPaymentTypeService
    {
        public PaymentTypeService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        
        public async Task<ResponseObject<Guid>> Create(CreatePaymentTypeReq req)
        {
            try
            {
                var newPaymentType = _mapper.Map<PaymentType>(req);

                var crateReult = await _unitOfWork.PaymentType.CreateAsync(newPaymentType, isSaveChange: true);

                return Success(crateReult);
            }
            catch (Exception ex)
            {
                return BadRequest<Guid>(ex.Message);
            }
        }

        public async Task<ResponseObject<bool>> Delete (Guid paymentTypeId){
            try
            {
                var deleteResult = await _unitOfWork.PaymentType.SoftDeleteAsync(t => t.Id == paymentTypeId);

                return Success(deleteResult > 0);
            }
            catch (Exception ex)
            {
                return BadRequest<bool>(ex.Message);
            }
        }


        public async Task<ResponseObject<PaymentType>> GetById(Guid paymentTypeId)
        {
            try
            {
                var queryHelper = new QueryHelper<PaymentType>
                {
                    Include = x => x.Include(t => t.OrderPayment)
                };

                var paymentType = await _unitOfWork.PaymentType.GetById(paymentTypeId, queryHelper, isAsNoTracking: false);

                return Success(paymentType);
            }
            catch (Exception ex)
            {
                return BadRequest<PaymentType>(ex.Message);
            }
        }
        // public async Task<IEnumerable<PaymentType>> GetAll()
        // {
        //     try
        //     {
        //         var queryHelper = new QueryHelper<PaymentType>
        //         {
        //             Include = x => x.Include(t => t.OrderPayment)
        //         };

        //         var paymentType = await _unitOfWork.PaymentType.GetAll(queryHelper, isAsNoTracking: false);

        //         return paymentType;
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest<PaymentType>(ex.Message);
        //     }
        // }



    }
}