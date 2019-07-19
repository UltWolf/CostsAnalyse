using CostsAnalyse.Models.Data;
using CostsAnalyse.Services.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ServicesTest.ManagersTests
{
   public  class SaveStatesTests
    {
        private readonly StateManager _stateManager = new StateManager();
        [Fact]
        public void IsSave()
        {
            _stateManager.SaveState(new CostsAnalyse.Models.Data.ParseState(0, CostsAnalyse.Models.Data.Store.Comfy));
        }

        [Fact]
        public void IsRecover()
        {
            ParseState state = _stateManager.RecoverState();
        }
    }
}
