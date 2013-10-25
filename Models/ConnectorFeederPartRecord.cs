using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using Orchard.Localization;



namespace Datwendo.ConnectorFeeder.Models
{
    [OrchardFeature("Datwendo.ConnectorFeeder")]
    public class ConnectorFeederPartRecord: ContentPartRecord 
    {
        public virtual int PublisherId { get; set; }
        public virtual string DataReceived { get; set; }         
    }

}
