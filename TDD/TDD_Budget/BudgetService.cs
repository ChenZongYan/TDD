using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (start > end) return 0;
            
            var budgets = _budgetRepo.GetAll();

            var totalQueryStartMonth = start.Year * 12 + start.Month;
            var totalQueryEndMonth = end.Year * 12 + end.Month;

            var isFullStartMonthDate = start.Day == 1;
            var isFullEndMonthDate = end.Day == DateTime.DaysInMonth(end.Year, end.Month);

            var daysInMonthByStartDate = DateTime.DaysInMonth(start.Year, start.Month);
            var daysInMonthByEndDate = DateTime.DaysInMonth(end.Year, end.Month);

            var startDateAndEndDateIsSameYearMonth = start.Year == end.Year && start.Month == end.Month;
            
            var startTotalDays = startDateAndEndDateIsSameYearMonth ? (end.Day - start.Day + 1) : (daysInMonthByStartDate - start.Day + 1);
            var endTotalDays = startDateAndEndDateIsSameYearMonth ? (end.Day - start.Day + 1) : (daysInMonthByEndDate - end.Day + 1);;

             return budgets.Where(
                x =>
                {
                    var yearMonth = DateTime.ParseExact(x.YearMonth, "yyyyMM", CultureInfo.InvariantCulture);

                    var totalMonth = yearMonth.Year * 12 + yearMonth.Month;;

                    return totalQueryStartMonth <= totalMonth && totalMonth <= totalQueryEndMonth;
                }
            ).Select((item,index) =>
            {
                var yearMonth = DateTime.ParseExact(item.YearMonth, "yyyyMM", CultureInfo.InvariantCulture);
                var totalMonth = yearMonth.Year * 12 + yearMonth.Month;

                var isMustCalculateAvgAmountByStartDate = (totalMonth == totalQueryStartMonth && !isFullStartMonthDate);
                var isMustCalculateAvgAmountByEndDate = (totalMonth == totalQueryEndMonth && !isFullEndMonthDate);

                if (isMustCalculateAvgAmountByStartDate)
                {
                    return (item.Amount / daysInMonthByStartDate) * startTotalDays;
                }
                else if (isMustCalculateAvgAmountByEndDate)
                {
                    return (item.Amount / daysInMonthByEndDate) * endTotalDays;
                }
                else
                {
                    return (decimal)item.Amount;
                }
            }).Sum();
        }
    }
}