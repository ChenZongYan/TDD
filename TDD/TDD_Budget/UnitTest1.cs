using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnitTest;

namespace TDD_Budget
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OneMonthQuery()
        {
            var budgetRepo = Substitute.For<IBudgetRepo>();

            budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget
                {
                    YearMonth = "202111",
                    Amount = 3000
                }
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var start = new DateTime(2021, 11, 01);
            var end = new DateTime(2021, 11, 30);

            Assert.AreEqual(3000m, budgetService.Query(start, end));
        }

        [Test]
        public void OneDayQuery()
        {
            var budgetRepo = Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202111",
                    Amount = 3000
                }
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var start = new DateTime(2021, 11, 01);
            var end = new DateTime(2021, 11, 01);

            Assert.AreEqual(100m, budgetService.Query(start, end));
            
        }
        
        [Test]
        public void CrossMonth()
        {
            var budgetRepo = Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget()
                {
                YearMonth = "202112",
                Amount = 3100
            }
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var start = new DateTime(2021, 11, 01);
            var end = new DateTime(2021, 12, 31);

            Assert.AreEqual(6100m, budgetService.Query(start, end));
            
        }
        
        [Test]
        public void CrossMonth2()
        {
            var budgetRepo = Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget()
                {
                    YearMonth = "202112",
                    Amount = 3100
                },
                new Budget()
                {
                    YearMonth = "202101",
                    Amount = 3100
                }
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var start = new DateTime(2021, 11, 28);
            var end = new DateTime(2021, 12, 3);

            Assert.AreEqual(600m, budgetService.Query(start, end));
            
        }
        
        [Test]
        public void 測試起始日期大於結束日期要回傳零()
        {
            var budgetRepo = Substitute.For<IBudgetRepo>();
            budgetRepo.GetAll().Returns(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202111",
                    Amount = 3000
                },
                new Budget()
                {
                    YearMonth = "202112",
                    Amount = 3100
                },
                new Budget()
                {
                    YearMonth = "202101",
                    Amount = 3100
                }
            });
            
            var budgetService = new BudgetService(budgetRepo);
            var start = new DateTime(2021, 11, 28);
            var end = new DateTime(2021, 11, 3);

            Assert.AreEqual(0m, budgetService.Query(start, end));
            
        }
    }
}