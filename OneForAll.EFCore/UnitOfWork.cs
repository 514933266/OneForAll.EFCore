using OneForAll.Core;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;

namespace OneForAll.EFCore
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _commited;
        private int _effected = 0;
        private readonly TransactionType _transactionType;
        private HashSet<IUnitTransaction> _transactions;

        public HashSet<Exception> Exceptions { get; set; }

        public UnitOfWork(TransactionType transactionType = TransactionType.Local)
        {
            _transactionType = transactionType;
            _transactions = new HashSet<IUnitTransaction>();

            Exceptions = new HashSet<Exception>();
        }

        public IUnitTransaction BeginTransaction()
        {
            var tran = new UnitTransaction(this);
            _transactions.Add(tran);
            return tran;
        }

        public void Dispose()
        {
            if (_transactions != null)
            {
                foreach (var tran in _transactions)
                {
                    tran.Dispose();
                }
            }
        }

        public int Commit()
        {
            if (_commited)
            {
                throw new InvalidOperationException("Duplicate units of work are prohibited!");
            }
            else
            {
                _commited = true;
                try
                {
                    foreach (var tran in _transactions)
                    {
                        if (!tran.Commited)
                        {
                            var effected = tran.Commit(_transactionType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _effected = 0;
                    Exceptions.Add(ex);
                    RollBack();
                }
            }
            return _effected;
        }

        public void RollBack()
        {
            if (_transactions != null)
            {
                foreach (var tran in _transactions)
                {
                    if (tran.Commited) tran.RollBack();
                }
            }
        }
    }

}
