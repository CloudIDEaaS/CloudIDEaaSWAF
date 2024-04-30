using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.InProcessTransactions.Implementation;

namespace Utils.InProcessTransactions
{
    public static class TransactionExtensions
    {
        public static IStateManager GetStateManager()
        {
            var appDomain = AppDomain.CurrentDomain;
            var stateManager = (IStateManager) appDomain.GetData("InProcessTransactions.StateManager");

            if (stateManager != null)
            {
                return stateManager;
            }

            stateManager = new StateManager();
            appDomain.SetData("InProcessTransactions.StateManager", stateManager);

            return stateManager;
        }

        public static ITransaction Enlist(string name, IEnlistment enlistment)
        {
            var stateManager = GetStateManager();

            return stateManager.Enlist(name, enlistment);
        }

        public static ITransaction Enlist(Guid id, IEnlistment enlistment)
        {
            var stateManager = GetStateManager();

            return stateManager.Enlist(id, enlistment);
        }
    }
}
