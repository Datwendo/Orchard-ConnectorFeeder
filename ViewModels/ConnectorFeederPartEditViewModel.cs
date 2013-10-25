using Datwendo.ConnectorFeeder.Models;
using Datwendo.ConnectorFeeder.Settings;
using System.Collections.Generic;

namespace Datwendo.ConnectorFeeder.ViewModels
{
    public class FPartView
    {
        public string Name {get; set; }
        public string DisplayName {get; set; }
    }


    public class ConnectorFeederPartEditViewModel {
        public ConnectorFeederSettings Settings { get; set; } 
        public ConnectorFeederPart Feeder { get; set; }
        public IEnumerable<FPartView> PartsLst { get; set; }
    }
}
