﻿using BannerKings.Managers.Education.Lifestyles;
using BannerKings.Managers.Institutions.Religions;
using BannerKings.Managers.Institutions.Religions.Faiths;
using BannerKings.Managers.Skills;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;

namespace BannerKings.Models.Vanilla
{
    public class BKBattleRewardModel : DefaultBattleRewardModel
    {
        public override ExplainedNumber CalculateInfluenceGain(PartyBase party, float influenceValueOfBattle, float contributionShare)
        {
            ExplainedNumber result = base.CalculateInfluenceGain(party, influenceValueOfBattle, contributionShare);
            var leader = party.LeaderHero;
            if (leader != null)
            {
                var education = BannerKingsConfig.Instance.EducationManager.GetHeroEducation(leader);
                if (education.HasPerk(BKPerks.Instance.CommanderWarband))
                {
                    result.AddFactor(0.25f, BKPerks.Instance.CommanderWarband.Name);
                }

                if (BannerKingsConfig.Instance.ReligionsManager.HasBlessing(leader,
                   DefaultDivinities.Instance.Wilund))
                {
                    result.AddFactor(0.3f, DefaultDivinities.Instance.Wilund.Name);
                }
            }

            return result;
        }

        public override ExplainedNumber CalculateRenownGain(PartyBase party, float renownValueOfBattle, float contributionShare)
        {
            var result = base.CalculateRenownGain(party, renownValueOfBattle, contributionShare);

            var leader = party.LeaderHero;
            if (leader != null)
            {
                var education = BannerKingsConfig.Instance.EducationManager.GetHeroEducation(leader);
                if (education.HasPerk(BKPerks.Instance.MercenaryFamousSellswords))
                {
                    result.AddFactor(0.2f, BKPerks.Instance.MercenaryFamousSellswords.Name);
                }

                if (education.Lifestyle != null && education.Lifestyle.Equals(DefaultLifestyles.Instance.Cataphract))
                {
                    result.AddFactor(0.12f, DefaultLifestyles.Instance.Cataphract.Name);
                }

                if (BannerKingsConfig.Instance.ReligionsManager.HasBlessing(leader,
                    DefaultDivinities.Instance.VlandiaMain))
                {
                    result.AddFactor(0.15f, DefaultDivinities.Instance.VlandiaMain.Name);
                }

                if (BannerKingsConfig.Instance.ReligionsManager.HasBlessing(leader,
                    DefaultDivinities.Instance.Wilund))
                {
                    result.AddFactor(0.3f, DefaultDivinities.Instance.Wilund.Name);
                }
            }

            return result;
        }

        public override int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero)
        {
            float relation = base.GetPlayerGainedRelationAmount(mapEvent, hero);
            if (BannerKingsConfig.Instance.ReligionsManager.HasBlessing(Hero.MainHero,
                DefaultDivinities.Instance.VlandiaMain))
            {
                var rel = BannerKingsConfig.Instance.ReligionsManager.GetHeroReligion(hero);
                if (rel != null && rel.Faith == DefaultFaiths.Instance.Canticles)
                {
                    relation *= 1.3f;
                }
            }

            return (int)relation;
        }
    }
}
