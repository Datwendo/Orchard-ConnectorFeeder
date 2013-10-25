using System.Web.Mvc;
using Orchard.Localization;
using Orchard;
using Orchard.Mvc;
using Datwendo.ConnectorFeeder.Models;
using Orchard.DisplayManagement;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement.MetaData;
using System.Collections.Generic;
using System.Linq;
using System;
using Orchard.UI.Admin;
using Orchard.Themes;
using Orchard.Core.Common.Models;
using System.Reflection;


namespace Datwendo.ConnectorFeeder.Controllers {
    [Admin]
    [OrchardFeature("Datwendo.ConnectorFeeder")]
    public class ConnectorFeederController : Controller, IUpdateModel  {
        public ConnectorFeederController() 
        {}

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}
