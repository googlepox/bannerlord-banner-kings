using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;
using System.Xml;

namespace BannerKings.UI.Extensions
{
    [PrefabExtension("SettlementNameplateItem", "descendant::ListPanel[@Id='EventsListPanel']")]
    internal class SettlementNameplateItemExtension : PrefabExtensionInsertPatch
    {
        private XmlDocument document;

        public override InsertType Type => InsertType.Append;

        public SettlementNameplateItemExtension()
        {
            this.document = new XmlDocument();
            this.document.LoadXml("<Widget IsVisible=\"@IsInRange\"> <Children> <Widget WidthSizePolicy=\"Fixed\" HeightSizePolicy=\"Fixed\" SuggestedWidth=\"14\" SuggestedHeight=\"9\" PositionXOffset=\"-20\" PositionYOffset=\"@DiseaseIconYOffset\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Center\" Sprite=\"Mission\\PersonalKillfeed\\kill_feed_skull\" AlphaFactor=\"0.9\" IsEnabled=\"false\" IsVisible=\"@HasDisease\"/> </Children> </Widget>");
        }

        [PrefabExtensionXmlDocument(false)]
        public XmlDocument GetPrefabExtension()
        {
            return this.document;
        }
    }
}
