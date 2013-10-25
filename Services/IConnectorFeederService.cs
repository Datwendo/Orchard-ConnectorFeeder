using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Datwendo.ConnectorFeeder.Models;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.ContentManagement;
using Orchard.UI.Notify;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Settings;
using Datwendo.ClientConnector.Models;
using Datwendo.ClientConnector.Settings;
using Datwendo.ClientConnector.Services;
using System.Reflection;
using System.Dynamic;
using Microsoft.CSharp.RuntimeBinder;
using Datwendo.ConnectorListener.Models;
using Datwendo.ConnectorFeeder.Settings;

namespace Datwendo.ConnectorFeeder.Services
{
    public interface IConnectorFeederService : IDependency
    {
        bool Propagate(ConnectorFeederPart part, int Idx,PropagateType DataType);

        bool ReadData(ConnectorFeederPart part, int Idx, out string strVal);
        bool ReadNextWithData(ConnectorFeederPart part, string strval, out int newVal);
        bool ReadBlob(ConnectorFeederPart part, int idx, out IEnumerable<ClientConnector.Models.FileDesc> newVal);
        bool ReadNextWithBlob(ConnectorFeederPart part, IEnumerable<string> fileList, out IEnumerable<ClientConnector.Models.FileDesc> newVal);    
    }

    public class ConnectorFeederService : IConnectorFeederService
    {
        public IOrchardServices Services { get; set; }
        //private readonly INotifier _notifier;
        private readonly IClientConnectorService _clientConnectorService;

        public ConnectorFeederService(IOrchardServices services
            //, INotifier notifier
            , IClientConnectorService clientConnectorService) 
        {
            Services                    = services;
            _clientConnectorService     = clientConnectorService;
            T                           = NullLocalizer.Instance;
            Logger                      = NullLogger.Instance;
            //_notifier                   = notifier;
        }

        public ILogger Logger { get; set; }

        public Localizer T { get; set; }


        #region Settings attached to Content Type

        ConnectorFeederSettings GetTypeSettings(ConnectorFeederPart part)
        {
            return part.TypePartDefinition.Settings.GetModel<ConnectorFeederSettings>();
        }

        public string GetPartName(ConnectorFeederPart part)
        {
            return GetTypeSettings(part).PartName;
        }

        public string GetPropertyName(ConnectorFeederPart part)
        {
            return GetTypeSettings(part).PropertyName;
        }

        bool KeepDataCopy(ConnectorFeederPart part)
        {
            return GetTypeSettings(part).KeepDataCopy;
        }

        #endregion // Settings attached to Content Type


        #region propagate to selected part


        public bool Propagate(ConnectorFeederPart part, int Idx, PropagateType DataType)
        {
            switch (DataType)
            {
                case PropagateType.DataString:
                    return PropagateData(part,Idx);
                case PropagateType.DataBlob:
                    return PropagateBlob(part, Idx);
                default:
                    return true;
            }
        }

        public bool PropagateBlob(ConnectorFeederPart part, int Idx)
        {
            return true;
        }

        public bool PropagateData(ConnectorFeederPart part, int Idx)
        {
            bool ret                = false;
            string strVal           = string.Empty;
            ret                     = ReadData(part, Idx, out strVal);
            if (!ret)
                return ret;
            string partName         = GetPartName(part);
            string propertyName     = GetPropertyName(part);
            var targetPart          = part.ContentItem.Parts.Where(p => p.PartDefinition.Name == partName).SingleOrDefault();
            if ( (targetPart == null) || !(targetPart is ContentPart))
                return false;
            ContentPart cpart       = targetPart as ContentPart;
            object targetProp;
            GetMemberBinder binder  = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, propertyName, targetPart.GetType(), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(0, null) });
            ret                     = cpart.TryGetMember(binder, out targetProp);
            if (ret)
                targetProp           = strVal;
            return ret;

            /*
            Type type       = targetPart.GetType() ;
            PropertyInfo propertyInfo = type.GetProperty( part.PropertyName, BindingFlags.Instance|BindingFlags.Public , null , typeof(String) , new Type[0] , null );
            if (propertyInfo == null)
                return false;
            propertyInfo.SetValue( targetPart , strVal , null ) ;
            return true;*/
        }

        #endregion propagate to selected part

        #region ReadValues

        // Read the actual value for Connector
        public bool ReadData(ConnectorFeederPart part, int Idx, out string strVal)
        {
            bool ret                        = false;
            strVal                          = string.Empty;
            ClientConnectorPart CPart       = part.ContentItem.As<ClientConnectorPart>();
            if (CPart == null)
                return false;
            ret                             = _clientConnectorService.ReadData(CPart,Idx, out strVal);
            return ret;
        }

        public bool ReadBlob(ConnectorFeederPart part, int Idx, out IEnumerable<ClientConnector.Models.FileDesc> newVal)
        {
            bool ret                        = false;
            newVal                          = null;
            ClientConnectorPart CPart       = part.ContentItem.As<ClientConnectorPart>();
            if (CPart == null)
                return false;
            ret                             = _clientConnectorService.ReadBlob(CPart, Idx, out newVal);
            return ret;
        }

        #endregion ReadValues

        #region Store Values

        public bool ReadNextWithData(ConnectorFeederPart part,string strVal, out int newVal)
        {
            Logger.Debug("ConnectorFeederService: ReadNextWithData BEG.");
            bool ret                        = false;
            newVal                          = int.MinValue;
      
            ClientConnectorPart CPart       = part.ContentItem.As<ClientConnectorPart>();
            if (CPart == null)
                return false;
            Logger.Debug("ConnectorFeederService: ReadNextWithData END : {0}", ret);
            return ret;
        }

        public bool ReadNextWithBlob(ConnectorFeederPart part, IEnumerable<string> fileList, out IEnumerable<ClientConnector.Models.FileDesc> newVal)
        {
            Logger.Debug("ConnectorFeederService: ReadNextWithBlob BEG.");
            bool ret                    = false;
            newVal                      = null;

            ClientConnectorPart CPart   = part.ContentItem.As<ClientConnectorPart>();
            if (CPart == null)
                return false;
            ret                         = _clientConnectorService.ReadNextWithBlob(CPart, fileList, out newVal);

            Logger.Debug("ConnectorFeederService: ReadNextWithBlob END : {0}", ret);
            return ret;
        }

        #endregion // Store Values
    }
}