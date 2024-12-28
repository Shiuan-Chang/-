using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Contract;
using 記帳系統.Models;
using 記帳系統.Repository;

namespace 記帳系統.Presenters
{
    internal class AccountPresenter : IAccountPresenter
    {
        private bool isLoading;
        private string csvSearchPath = @"C:\Users\icewi\OneDrive\桌面\testCSV";
        private IAccountView accountView;
        public IRepository repository;
        public IMapper mapper;
        
        public AccountPresenter(IAccountView view, IRepository repository, IMapper mapper)
        {
            accountView = view;
            this.repository = repository;
            this.mapper = mapper;
        }

        public void LoadData(DateTime startDate, DateTime endDate, List<string> conditionTypes, List<string> analyzeTypes)
        {
            isLoading = true;
            var rawDataList = repository.accountFormGetDatasByDate(startDate, endDate);
            var accountModelList = ConvertToAccountModel(rawDataList);
            isLoading = false;
            //通知view的程式
            accountView.UpdateDataView(accountModelList);
        }

        private List<AccountModel> ConvertToAccountModel(List<AccountRawDataDAO> rawDataList)
        {
            return mapper.Map<List<AccountModel>>(rawDataList);
        }

        public List<AccountingModel> RespondGroupList(List<AccountingModel> lists)
        {
            throw new NotImplementedException();
        }
    }
}
