using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Orchard.ContentManagement.MetaData.Builders;
using Datwendo.ClientConnector.Models;
using Orchard.Localization;
using System;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace Datwendo.ConnectorFeeder.Settings
{
    /// <summary>
    /// Settings when attaching part to a content item
    /// </summary>
    public class ConnectorFeederSettings {

        public ConnectorFeederSettings() {
            KeepDataCopy = false;
        }
        public string contentItemName { get; set; }
        public IEnumerable<dynamic> AllParts { get; set; }
        public IEnumerable<dynamic> AllProperties { get; set; }
        public IEnumerable<dynamic> AllFields { get; set; }
        public virtual bool KeepDataCopy { get; set; }
        public virtual string PartName { get; set; }
        public virtual string PropertyName { get; set; }
        public virtual string FieldName { get; set; } 

        public void Build(ContentTypePartDefinitionBuilder builder) {
            builder.WithSetting("ConnectorFeederSettings.KeepDataCopy", KeepDataCopy.ToString(CultureInfo.InvariantCulture));
            builder.WithSetting("ConnectorFeederSettings.PartName",PartName);
            builder.WithSetting("ConnectorFeederSettings.PropertyName", string.IsNullOrEmpty(PropertyName) ? string.Empty : PropertyName);
            builder.WithSetting("ConnectorFeederSettings.FieldName", string.IsNullOrEmpty(FieldName) ? string.Empty:FieldName);
        }
    }
}
