using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datwendo.ConnectorFeeder.Models;
using Datwendo.ConnectorFeeder.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Datwendo.ClientConnector.Settings;
using Orchard.ContentManagement.MetaData;
using Datwendo.ConnectorFeeder.Settings;


namespace Datwendo.ConnectorFeeder.Drivers
{
    [OrchardFeature("Datwendo.ConnectorFeeder")]
    public class ConnectorFeederPartDriver : ContentPartDriver<ConnectorFeederPart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public ConnectorFeederPartDriver(IContentDefinitionManager contentDefinitionManager)
        {
            T = NullLocalizer.Instance;
            _contentDefinitionManager = contentDefinitionManager;
        }

        public Localizer T { get; set; }

        
       protected override string Prefix
        {
            get { return "ConnectorFeeder"; }
        }


        protected override DriverResult Display(
            ConnectorFeederPart part, string displayType, dynamic shapeHelper) {
                return ContentShape("Parts_ConnectorFeeder", () => shapeHelper.Parts_ConnectorFeeder(Content: part));
        }

        //GET
        protected override DriverResult Editor(ConnectorFeederPart part, dynamic shapeHelper)
        {
             return Editor(part, null, shapeHelper);
            /*
            var lst = _contentDefinitionManager.ListPartDefinitions().Select(c => new PartView { Name = c.Name, DisplayName = c.Name });
            ConnectorFeederPartViewModel vm = new ConnectorFeederPartViewModel { Feeder = part, PartsLst = lst };
            return ContentShape("Parts_ConnectorFeeder_Edit",
            () => shapeHelper.EditorTemplate(
                TemplateName: "Parts/ConnectorFeeder",
                Model: vm,
                Prefix: Prefix));*/
        }

        //POST
        protected override DriverResult Editor(ConnectorFeederPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var settings    = part.TypePartDefinition.Settings.GetModel<ConnectorFeederSettings>();
            var lst         = _contentDefinitionManager.ListPartDefinitions().Select(c => new FPartView { Name = c.Name, DisplayName = c.Name });
            var viewModel   = new ConnectorFeederPartEditViewModel
            {
                Settings    = settings,
                PartsLst    = lst,
                Feeder      = part
            };

            if (updater != null && updater.TryUpdateModel(viewModel, Prefix, null, null))
            {
                if (string.IsNullOrEmpty(settings.PartName) )
                {
                    updater.AddModelError("PartName", T("You have not defined the recipient Part."));
                }

                if (string.IsNullOrEmpty(settings.PropertyName))
                {
                    updater.AddModelError("PropertyName", T("You have not defined the Property."));
                }

            }

            return ContentShape("Parts_ConnectorFeeder_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: "Parts.ConnectorFeeder.Edit", Model: viewModel, Prefix: Prefix));
        }


        protected override void Importing(ConnectorFeederPart part, ImportContentContext context)
        {
            var PublisherId         = context.Attribute(part.PartDefinition.Name, "PublisherId");
            int PId                 = 0;
            if (int.TryParse(PublisherId,out PId))
                part.PublisherId    = PId;
            var DataReceived        = context.Attribute(part.PartDefinition.Name, "DataReceived");
            if (!string.IsNullOrEmpty(DataReceived))
                part.DataReceived   = DataReceived;
        }

        protected override void Exporting(ConnectorFeederPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("PublisherId", part.PublisherId.ToString());
            context.Element(part.PartDefinition.Name).SetAttributeValue("DataReceived", part.DataReceived);
        }
    }
}