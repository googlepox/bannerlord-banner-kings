﻿using BannerKings.Utils.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Institutions.Religions.Faiths.Rites.Northern
{
    public class TreeloreFestival : Festival
    {
        public override int DayOfTheSeason => 1;
        public override int SeasonOfTheYear => 0;

        public override void Execute(Hero executor)
        {
            base.Execute(executor);
        }

        public override TextObject GetDescription() => new TextObject("{=!}The Festival of Pérken is celebrated on the evennight of spring. To the children of the forest, spring represents the creation of the world of men. It is said no world existed between the heavenly home of the gods and birds and the underworld of snakes and worms, until Pérken, the lightning-wielder, struck the bark of the Great World Tree, and from it's sap, mankind blossomed.");

        public override TextObject GetName() => new TextObject("{=!}Festival of Pérken");

        public override float GetPietyReward()
        {
            return 100;
        }

        public override bool MeetsCondition(Hero hero) => hero.Clan != null && hero.IsClanLeader() &&
            hero.Clan.Fiefs.Count > 0 && CampaignTime.Now.GetSeasonOfYear == SeasonOfTheYear &&
            CampaignTime.Now.GetDayOfSeason == DayOfTheSeason;
    }
}