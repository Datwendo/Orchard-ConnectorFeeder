using Orchard.UI.Resources;

namespace Datwendo.ConnectorFeeder {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            manifest.DefineStyle("ConnectorFeederSettings").SetUrl("datwendo-connectorfeeder-settings.css");
        }
    }
}
