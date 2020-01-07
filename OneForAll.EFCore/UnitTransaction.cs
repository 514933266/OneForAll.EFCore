using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using OneForAll.Core.Extension;

namespace OneForAll.EFCore
{
    public class UnitTransaction : IUnitTransaction
    {
        private int _effected = 0;
        private readonly IUnitOfWork _uow;
        private readonly List<IUnitAction> _actions;

        private IDbContextTransaction _tran;
        
        public bool Commited { get; set; }

        public UnitTransaction(IUnitOfWork uow)
        {
            _uow = uow;
            _actions = new List<IUnitAction>();
        }

        public void Register<T>(Func<int> action, T conn)
        {
            _actions.Add(new UnitAction(action));
            BeginDbTransaction(conn);
        }

        private void BeginDbTransaction<T>(T conn)
        {
            if (_tran == null)
            {
                var context = conn as DbContext;
                _tran = context.Database.BeginTransaction();
            }
        }

        public int Commit(TransactionType transactionType = TransactionType.Local)
        {
            if (Commited)
            {
                throw new InvalidOperationException("Duplicate commit transactions are prohibited!");
            }
            else
            {
                Commited = true;
                switch (transactionType)
                {
                    case TransactionType.Local: CommitLocalTran(_actions); break;
                    default:
                        throw new Exception("Unsupported transaction type!");
                }
            }
            return _effected;
        }

        private void CommitLocalTran(IEnumerable<IUnitAction> actions)
        {
            try
            {
                actions.ForEach(a =>
                {
                    _effected += a.Action();
                });
                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                _tran.Commit();
            }
            catch (Exception ex)
            {
                _effected = 0;
                _uow.Exceptions.Add(ex);
            }
        }

        public void Dispose()
        {
            if (_tran != null) _tran.Dispose();
        }

        public void RollBack()
        {
            if (_tran != null) _tran.Rollback();
        }
    }

}
