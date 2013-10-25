using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;
using System.ComponentModel.DataAnnotations;
using Orchard.Environment.Extensions;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Mvc.Html;
using Orchard.Services;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Title.Models;


namespace Datwendo.ConnectorFeeder.Models
{
    [OrchardFeature("Datwendo.ConnectorFeeder")]
    public class ConnectorFeederPart : ContentPart<ConnectorFeederPartRecord>
    {
        public int PublisherId
        {
            get { return Record.PublisherId; }
            set { Record.PublisherId = value; }
        }
        public string DataReceived
        {
            get { return Record.DataReceived; }
            set { Record.DataReceived = value; }
        }
    }
}
