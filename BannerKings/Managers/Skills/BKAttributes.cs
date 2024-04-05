using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Skills
{
    public class BKAttributes
    {
        public static MBReadOnlyList<CharacterAttribute> AllAttributes
        {
            get
            {
                var allAttrs = TaleWorlds.CampaignSystem.Campaign.Current.GetType()
                    .GetProperty("AllCharacterAttributes", BindingFlags.Instance | BindingFlags.NonPublic);
                return (MBReadOnlyList<CharacterAttribute>) allAttrs.GetValue(TaleWorlds.CampaignSystem.Campaign.Current);
            }
        }

        public CharacterAttribute Wisdom { get; private set; }

        public static BKAttributes Instance => ConfigHolder.CONFIG;

        public void Initialize()
        {
            Wisdom = Game.Current.ObjectManager.RegisterPresumedObject(new CharacterAttribute("wisdom"));
            Wisdom.Initialize(new TextObject("{=CCfPK6ew}Wisdom"),
                new TextObject("{=CCfPK6ew}Wisdom represents your world knowledge and competence to deal with skills that require deep learning."),
                new TextObject("{=rLBAcOoV}WIS"));
        }

        internal struct ConfigHolder
        {
            public static BKAttributes CONFIG = new();
        }
    }
}