using System.Collections.Generic;

namespace UnitTest
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}