using System;
using System.Linq;
using NSubstitute.Core;

namespace UnitTest
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo; 
        }

        public decimal Query(DateTime start, DateTime end)
        {
            var budgets = _budgetRepo.GetAll();
            var budget = budgets.First();
            var startDay = end.Day - start.Day + 1;
            return budget.Amount / startDay;
            
            
        }
    }
}