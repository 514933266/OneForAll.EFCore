using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using OneForAll.Core.Extension;
using System.Transactions;

namespace OneForAll.EFCore
{
    /// <summary>
    /// 单元事务
    /// </summary>
    public class UnitTransaction : IUnitTransaction
    {
        private int _effected = 0;
        private IDbContextTransaction _tran;

        private readonly IUnitOfWork _uow;


        public bool Commited { get; set; }

        public UnitTransaction(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Register<T>(Func<int> action, T conn)
        {
            BeginDbTransaction(conn);
            _effected += action();
        }

        private void BeginDbTransaction<T>(T conn)
        {
            if (_tran == null)
            {
                var context = conn as DbContext;
                _tran = context.Database.BeginTransaction();
            }
        }

        public int Commit()
        {
            if (Commited)
            {
                throw new InvalidOperationException("事务已经提交，无法重复提交!");
            }
            else
            {
                Commited = true;
                try
                {
                    _tran.Commit();
                }
                catch (Exception ex)
                {
                    _effected = 0;
                    _uow.Exceptions.Add(ex);
                }
            }
            return _effected;
        }

        public void Dispose()
        {
            if (_tran != null)
                _tran.Dispose();
        }

        public void RollBack()
        {
            if (_tran != null)
                _tran.Rollback();
        }
    }
}
