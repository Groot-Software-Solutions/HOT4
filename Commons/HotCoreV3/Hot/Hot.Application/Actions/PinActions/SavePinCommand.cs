using System.Data;

namespace Hot.Application.Actions.PinActions
{
    public record SavePinCommand(PinFileDataModel pinModel) : IRequest<OneOf<int, AppException>>;
    public class SavePinCommandHandler : IRequestHandler<SavePinCommand, OneOf<int, AppException>>
    {
        private readonly IDbContext _context;
        private AppException? AppException { get; set; }
        private IDbHelper DbHelper { get; set; }

        public SavePinCommandHandler(IDbContext context, IDbHelper dbHelper)
        {
            _context = context;
            DbHelper = dbHelper;
        }

        public async Task<OneOf<int, AppException>> Handle(SavePinCommand request, CancellationToken cancellationToken)
        {

            //create pin batch
            
            var dbtransaction = await BeginTransaction();
            if (dbtransaction == null) return ApplicationError("Data Connection error");
            PinBatch pinBatch = new()
            {
                BatchDate = DateTime.Now,
                Name = "",
                PinBatchTypeId = 8
            };
            //var PinBatchResponse =await _context.PinBatches.AddAsync(dbtransaction, pinBatch);
            var PinBatchResponse = await _context.PinBatches.AddAsync(pinBatch,dbtransaction.Item1, dbtransaction.Item2);
            if (PinBatchResponse.IsT1) return RollbackAndReturnAppError(dbtransaction);

            pinBatch.PinBatchId = PinBatchResponse.AsT0;

            foreach (var item in request.pinModel.Pins)
            {
                item.PinBatchId = pinBatch.PinBatchId;
                var SavePinResponse = await _context.Pins.AddAsync(item, dbtransaction.Item1, dbtransaction.Item2);
                if (SavePinResponse.IsT1) return RollbackAndReturnAppError(dbtransaction);
            }

            bool result = CompleteTransaction(dbtransaction);
            if (result == false) return RollbackAndReturnAppError(dbtransaction);
            return pinBatch.PinBatchId; 
        }

        private async Task<Tuple<IDbConnection, IDbTransaction>> BeginTransaction()
        {
            var result = await DbHelper.BeginTransaction();
            if (result.IsT1)
            {
                AppException = new AppException("DB Error attempting to create transaction", result.AsT1.Message);
            }
            return result.AsT0;
        }
        private bool CompleteTransaction(Tuple<IDbConnection, IDbTransaction> dbtransaction)
        {
            return DbHelper.CommitTransaction(dbtransaction.Item2).ResultOrNull();
        }
        private AppException RollbackAndReturnAppError(Tuple<IDbConnection, IDbTransaction> dbtransaction)
        {
            DbHelper.RollBackTransaction(dbtransaction.Item2);
            return ApplicationError("Error Storing Data");
        }
        private AppException ApplicationError(string message)
        {
            return new AppException("Pins Add", $"{message} - {AppException?.Message}");
        }
    }
}
