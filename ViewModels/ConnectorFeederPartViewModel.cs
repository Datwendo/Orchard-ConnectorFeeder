using Datwendo.ConnectorFeeder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Datwendo.ConnectorFeeder.ViewModels
{
    public class ConnectorFeederPartViewModel
    {
        public ConnectorFeederPart Feeder { get; set; }

        public int PublisherId
        {
            get { return (Feeder != null) ? Feeder.PublisherId : 0; }
            set { if (Feeder != null) Feeder.PublisherId = value; }
        }


        public string DataReceived
        {
            get { return (Feeder != null) ? Feeder.DataReceived : string.Empty; }
            set { if (Feeder != null) Feeder.DataReceived = value; }
        }
    }
}