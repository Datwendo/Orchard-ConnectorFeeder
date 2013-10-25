using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datwendo.ConnectorFeeder.Services;
using Datwendo.ConnectorFeeder.Models;
using Orchard.ContentManagement.Handlers;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Data;

using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Datwendo.ConnectorListener.Models;
using Datwendo.ClientConnector.Models;
using Datwendo.ClientConnector.Services;


namespace Datwendo.ConnectorFeeder.Handlers
{
    [OrchardFeature("Datwendo.ConnectorFeeder")]
    public class ConnectorFeederPartHandler : ContentHandler {

        public Localizer T { get; set; }

        private readonly IConnectorFeederService _feederService;
        private readonly IClientConnectorService _clientConnectorService;

        public ConnectorFeederPartHandler(IRepository<ConnectorFeederPartRecord> repository
            , IConnectorFeederService feederService
            , IClientConnectorService clientConnectorService)
        {            
            T                       = NullLocalizer.Instance;
            _feederService          = feederService;
            _clientConnectorService = clientConnectorService;            
            
            Filters.Add(StorageFilter.For(repository));
            OnCreated<NotificationPart>(ReadData);
        }

        protected void ReadData(CreateContentContext context, NotificationPart part)
        {
            if (part.CounterId == 0 || part.IdxVal == 0 || part.StateCode != 0 || part.DataType == PropagateType.NoData)
            {
                Logger.Debug(string.Format("ConnectorFeederPartHandler.ReadData ContentItem: {0}, CounterId: {1}, IdxVal: {2}, DataType: {3}, StateCode: {4}", 
                    new Object[] { part.ContentItem.TypeDefinition.Name, part.CounterId, part.IdxVal, part.DataType,part.StateCode }));
                return;
            }

            ConnectorFeederPart feeder = part.ContentItem.As<ConnectorFeederPart>();
            if (feeder == null)
            {
                Logger.Debug(string.Format("No ConnectorFeederPart found in ContentItem: {0}, CounterId: {1}, IdxVal: {2}", new Object[] { part.ContentItem.TypeDefinition.Name,part.CounterId, part.IdxVal }));
                return;
            }

            ClientConnectorPart CPart = part.ContentItem.As<ClientConnectorPart>();
            if (CPart == null || _clientConnectorService.GetConnectorId(CPart) != part.CounterId)
            {
                Logger.Debug(string.Format("No ClientConnector found in ContentItem: {0}, CounterId: {1}, IdxVal: {2}", new Object[] { part.ContentItem.TypeDefinition.Name, part.CounterId, part.IdxVal }));               
                return;
            }
            if (_feederService.Propagate(feeder, part.IdxVal,part.DataType))
            {
                return;
            }
            Logger.Debug(string.Format("Error Propagate in ContentItem: {0}, CounterId: {1}, IdxVal: {2}, , DataType: {3}", new Object[] { part.ContentItem.TypeDefinition.Name, part.CounterId, part.IdxVal, part.DataType }));
        }

    }
}