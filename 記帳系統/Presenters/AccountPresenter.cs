using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Contract;
using 記帳系統.Models;

namespace 記帳系統.Presenters
{
    internal class AccountPresenter : IAccountPresenter
    {
        private IAccountView viewUpdater;
        public AccountPresenter(IAccountView updater)
        {
            viewUpdater = updater;
        }

        public void LoadData(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public List<AccountingModel> RespondGroupList(List<AccountingModel> lists)
        {
            throw new NotImplementedException();
        }
    }
}
