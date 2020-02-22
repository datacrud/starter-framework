using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Enums;
using Security.Models.ViewModels;

namespace Project.ViewModel
{
    public class DashboardViewModel
    {

        //public double Deposit { get; set; }
        public double DepositToday { get; set; }

        //public double Expense { get; set; }
        public double ExpenseToday { get; set; }

        //public double Sale { get; set; }
        public double SaleToday { get; set; }

        //public double CustomerReceiveable { get; set; }
        //public double CustomerReceiveableToday { get; set; }

        public double Cash { get; set; }
        //public double CashToday { get; set; }
        //public double OpeningCashToday { get; set; }

        public double Bank { get; set; }
        //public double BankToday { get; set; }

        //public double Purchase { get; set; }
        public double PurchaseToday { get; set; }

        //public double PurchaseDue { get; set; }
        //public double PurchaseDueToday { get; set; }

        //public double SupplierPayable { get; set; }
        //public double SupplierPayableToday { get; set; }

        public double Check { get; set; }
        public int CheckQuantity { get; set; }


        public List<LowStockAlertViewModel> LowStockAlerts { get; set; }
        public List<DashboardAlertViewModel> DashboardAlerts { get; set; }
    }

    public class LowStockAlertViewModel
    {
        public string WarehouseName { get; set; }
        public string SupplierName { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string ProductName { get; set; }

        public int StockBox { get; set; }
        public int StockQuantity { get; set; }
        public double StockSquarefit { get; set; }
    }

    public class DashboardAlertViewModel
    {

        public DashboardAlertViewModel()
        {
            
        }

        public DashboardAlertViewModel(UserAlertViewModel model)
        {
            IsShow = model.IsShow;
            Identity = model.Identity;
            Title = model.Title;
            AlertType = model.AlertType;
            AlertClass = model.AlertClass;
            Message = model.Message;
        }

        public bool IsShow { get; set; }
        public int Identity { get; set; }        

        public string Title { get; set; }
        public DashboardAlertType AlertType { get; set; }        
        public string AlertClass { get; set; }
        public string Message { get; set; }
        
    }
}
