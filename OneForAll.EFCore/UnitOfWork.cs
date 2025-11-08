using OneForAll.Core;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _commited;
        private int _effected = 0;
        private HashSet<IUnitTransaction> _transactions;

        public HashSet<Exception> Exceptions { get; set; }

        public UnitOfWork()
        {
            _transactions = new HashSet<IUnitTransaction>();
            Exceptions = new HashSet<Exception>();
        }

        public IUnitTransaction BeginTransaction(TransactionType transactionType = TransactionType.Local)
        {
            IUnitTransaction tran = null;
            switch (transactionType)
            {
                case TransactionType.Local:
                    tran = new UnitTransaction(this);
                    break;
                default:
                    throw new Exception("暂不支持创建除本地事务外的其他事务类型");
            }
            _transactions.Add(tran);
            return tran;
        }

        public void Dispose()
        {
            foreach (var tran in _transactions)
            {
                tran.Dispose();
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
                            var effected = tran.Commit();
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

        public async Task<int> CommitAsync()
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
                            _effected += await tran.CommitAsync();
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
