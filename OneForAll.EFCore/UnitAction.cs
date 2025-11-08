using OneForAll.Core.ORM;
using System;

namespace OneForAll.EFCore
{
    public class UnitAction : IUnitAction
    {
        public UnitAction(Func<int> action)
        {
            Action = action;
        }

        public Func<int> Action { get; set; }
    }
}
