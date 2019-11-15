using ClientAPI.Dto;
using ClientAPI.Query;
using ClientAPI.Repository.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Handlers.Queries
{
    public class GetTransactionHistoryHandler : IRequestHandler<GetTransactionHistoryQuery, TransactionHistoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetTransactionHistoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionHistoryDto> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => {

                List<TransactionHistory> transactionHistories = new List<TransactionHistory>();
                
                var account = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountNumber == request.AccountNumber);
                var transactions = _unitOfWork.Transaction.GetAll(t => t.AccountId == account.AccountId).OrderByDescending(a => a.TransactionDate).ToList();

                foreach (var trans in transactions)
                {
                    var transactionHistory = new TransactionHistory();
                    transactionHistory.TransactionType = trans.TransactionType;
                    transactionHistory.Amount = trans.Amount;
                    transactionHistory.RemainingBalance = trans.RemainingBalance;
                    transactionHistory.TransactionDate = trans.TransactionDate;
                    transactionHistory.Remarks = trans.Remarks;
                    transactionHistories.Add(transactionHistory);
                }

                var transactionHistoryDto = new TransactionHistoryDto
                {
                    ClientId = account.ClientId,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    Transactions = transactionHistories
                };

                return transactionHistoryDto;
            });
        }
    }
}
