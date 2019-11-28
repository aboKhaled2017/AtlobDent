using Atlob_Dent.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Atlob_Dent.Helpers
{
    public class TransactionHelper
    {
        private Atlob_dent_Context _context { get; set; }
        private IDbContextTransaction _actionOnDbTransaction { get; set; }
        public TransactionHelper(Atlob_dent_Context context)
        {
            _context = context;
            _actionOnDbTransaction = _context.Database.BeginTransaction();
        }
        public TransactionHelper TakeActionOnDb(Action<Atlob_dent_Context>option)
        {
            try
            {
                option.Invoke(_context);
            }
            catch
            {
                _actionOnDbTransaction.Rollback();
            }

            return this;
        }
        public TransactionHelper TakeActionOnDb(Action option)
        {
            try
            {

                option.Invoke();
            }
            catch
            {
                _actionOnDbTransaction.Rollback();
            }
            return this;
        }
        public void CommitChanges()
        {
            _actionOnDbTransaction.Commit();
        }
        public void RollBackChanges()
        {
            _actionOnDbTransaction.Rollback();
        }
        public void BeginAgain()
        {
            _actionOnDbTransaction = _context.Database.BeginTransaction();
        }
        public void End()
        {
            _actionOnDbTransaction.Dispose();
        }
    }
}
