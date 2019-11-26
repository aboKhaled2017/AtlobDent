using Atlob_Dent.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Helpers
{
    public class TransactionHelper
    {
        private Atlob_dent_Context _context { get; set; }
        private IDbContextTransaction _registerCustomerTransaction { get; set; }
        public TransactionHelper(Atlob_dent_Context context)
        {
            _context = context;
            _registerCustomerTransaction = _context.Database.BeginTransaction();
        }
        public void TakeAction(Action<Atlob_dent_Context>option)
        {
            option.Invoke(_context);
            _registerCustomerTransaction.Commit();
        }
        public void RollBackAction()
        {
            _registerCustomerTransaction.Rollback();
        }
    }
}
