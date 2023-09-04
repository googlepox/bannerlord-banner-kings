﻿using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Localization;

namespace BannerKings.Models.Vanilla
{
    public class BKDiplomacyModel : DefaultDiplomacyModel
    {
        public override float GetScoreOfDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingClan, out TextObject warReason)
        {
            float result = base.GetScoreOfDeclaringWar(factionDeclaresWar, factionDeclaredWar, evaluatingClan, out warReason);

            if (factionDeclaresWar.IsKingdomFaction)
            {
                Kingdom kingdom = (Kingdom)factionDeclaresWar;
                float wealthFactor = 0;
                int totalWealth = 0;
                foreach (var clan in kingdom.Clans)
                {
                    if (!clan.IsUnderMercenaryService)
                    {
                        wealthFactor += (clan.Gold - 25000) / 100000f;
                        totalWealth += clan.Gold;
                    }
                }

                if (totalWealth < kingdom.Clans.Count * 85000)
                {
                    return 0f;
                }

                result *= wealthFactor / kingdom.Clans.Count;
            }

            return result;
        }

        public override void GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2)
        {
            effectiveHero1 = hero1;
            effectiveHero2 = hero2;
        }

        public override int GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(Hero supporter, bool hasHeroVotedAgainstOwner)
        {
            return base.GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(supporter, hasHeroVotedAgainstOwner);
        }
    }
}


