using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳系統.Repository
{
    public static class DropdownRepository
    {
        public static List<string> GetAccountNames()
        {
            return new List<string> { "Food", "Traffic", "Rent", "Dress", "Entertainment", "Learning", "Investment" };
        }

        public static List<string> GetAccountTypes(string accountName)
        {
            switch (accountName)
            {
                case "Food":
                    return new List<string> { "Groceries", "Dining Out", "Snacks" };
                case "Traffic":
                    return new List<string> { "Public Transport", "Taxi", "Fuel" };
                case "Rent":
                    return new List<string> { "Monthly Rent", "Utilities" };
                case "Dress":
                    return new List<string> { "Clothing", "Accessories" };
                case "Entertainment":
                    return new List<string> { "Movies", "Games", "Concerts" };
                case "Learning":
                    return new List<string> { "Books", "Courses", "Workshops" };
                case "Investment":
                    return new List<string> { "Stocks", "Bonds", "Real Estate" };
                default:
                    return new List<string>();
            }
        }
    }
}
