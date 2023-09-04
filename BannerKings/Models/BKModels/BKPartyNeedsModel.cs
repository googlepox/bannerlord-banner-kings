using BannerKings.Behaviours.PartyNeeds;
using BannerKings.Managers.Traits;
using BannerKings.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerKings.Models.BKModels
{
    public class BKPartyNeedsModel : IPartyNeedsModel
    {
        public float ArrowsPerSoldier => 0.003f;

        public float ShieldsPerSoldier => 0.003f;

        public float WeaponsPerSoldier => 0.006f;

        public float HorsesPerSoldier => 0.001f;

        public float ClothPerSoldier => 0.01f;

        public float ToolsPerSoldier => 0.01f;

        public float WoodPerSoldier => 0.02f;

        public float AnimalProductsPerSoldier => 0.01f;

        public float AlcoholPerSoldier => 0.025f;

        public float MedicalSuppliesPerSoldier => 0.05f;

        public ExplainedNumber MinimumSoldiersThreshold(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(5f, descriptions);
            if (needs.Party.LeaderHero == null)
            {
                result.Add(float.MaxValue);
                return result;
            }

            result.Add(needs.Party.LeaderHero.GetSkillValue(DefaultSkills.Leadership) / 10f,
                DefaultSkills.Leadership.Name);
            return result;
        }

        public ExplainedNumber CalculateAlcoholNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                result.Add(element.Number * AlcoholPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor, 
                    new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                    .SetTextVariable("TROOP_NAME", element.Character.Name)
                    .SetTextVariable("COUNT", element.Number));
            }

            return result;
        }

        public ExplainedNumber CalculateArrowsNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                FormationClass formation = element.Character.GetFormationClass();
                if (formation == FormationClass.Ranged || formation == FormationClass.HorseArcher)
                {
                    result.Add(element.Number * ArrowsPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor, 
                        new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                        .SetTextVariable("TROOP_NAME", element.Character.Name)
                        .SetTextVariable("COUNT", element.Number));
                }
            }

            return result;
        }

        public ExplainedNumber CalculateClothNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                result.Add(element.Number * ClothPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor, 
                    new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                    .SetTextVariable("TROOP_NAME", element.Character.Name)
                    .SetTextVariable("COUNT", element.Number));
            }

            return result;
        }

        public ExplainedNumber CalculateHorsesNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                if (element.Character.IsMounted)
                {
                    result.Add(element.Number * HorsesPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor, 
                        new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                        .SetTextVariable("TROOP_NAME", element.Character.Name)
                        .SetTextVariable("COUNT", element.Number));
                }
            }

            return result;
        }

        public ExplainedNumber CalculateAnimalProductsNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                result.Add(element.Number * AnimalProductsPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor, 
                    new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                    .SetTextVariable("TROOP_NAME", element.Character.Name)
                    .SetTextVariable("COUNT", element.Number));
            }

            return result;
        }

        public ExplainedNumber CalculateShieldsNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                if (element.Character.Equipment.HasWeaponOfClass(WeaponClass.SmallShield) ||
                    element.Character.Equipment.HasWeaponOfClass(WeaponClass.LargeShield))
                {
                    result.Add(element.Number * ShieldsPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor, 
                        new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                        .SetTextVariable("TROOP_NAME", element.Character.Name)
                        .SetTextVariable("COUNT", element.Number));
                }
            }

            return result;
        }

        public ExplainedNumber CalculateWeaponsNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                if (!element.Character.IsRanged)
                {
                    result.Add(element.Number * WeaponsPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor,
                        new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                        .SetTextVariable("TROOP_NAME", element.Character.Name)
                        .SetTextVariable("COUNT", element.Number));
                }
            }

            return result;
        }

        public ExplainedNumber CalculateToolsNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            float siege = 0f;
            TextObject siegeText = null;
            if (needs.Party.SiegeEvent != null)
            {
                if (needs.Party.BesiegerCamp != null)
                {
                    if (!needs.Party.BesiegerCamp.IsPreparationComplete)
                    {
                        siege = 5f;
                        siegeText = new TextObject("{=KRPA6cmU}Camp under construction");
                    }

                    var engines = needs.Party.BesiegerCamp.SiegeEngines;
                    var preparations = engines?.SiegePreparations;
                    if (preparations != null && !preparations.IsConstructed)
                    {
                        siege = 10f;
                        siegeText = new TextObject("{=PGEJammA}Siege engine under construction");
                    }
                }

                result.AddFactor(siege, siegeText);
                foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
                {
                    result.Add(element.Number * ToolsPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor,
                        new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                        .SetTextVariable("TROOP_NAME", element.Character.Name)
                        .SetTextVariable("COUNT", element.Number));
                }
            } 
           
            return result;
        }

        public ExplainedNumber CalculateWoodNeed(PartySupplies needs, bool descriptions)
        {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null)
            {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            float siege = 0f;
            TextObject siegeText = null;
            if (needs.Party.SiegeEvent != null)
            {
                if (needs.Party.BesiegerCamp != null)
                {
                    if (!needs.Party.BesiegerCamp.IsPreparationComplete)
                    {
                        siege = 5f;
                        siegeText = new TextObject("{=KRPA6cmU}Camp under construction");
                    }

                    var engines = needs.Party.BesiegerCamp.SiegeEngines;
                    var preparations = engines?.SiegePreparations;
                    if (preparations != null && !preparations.IsConstructed)
                    {
                        siege = 10f;
                        siegeText = new TextObject("{=PGEJammA}Siege engine under construction");
                    }
                }
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster())
            {
                result.Add(element.Number * WoodPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor, 
                    new TextObject("{=5Jr8zfXD}{TROOP_NAME}(x{COUNT})")
                    .SetTextVariable("TROOP_NAME", element.Character.Name)
                    .SetTextVariable("COUNT", element.Number));
            }

            result.AddFactor(siege, siegeText);
            return result;
        }

        public ExplainedNumber CalculateMedicalNeed(PartySupplies needs, bool descriptions) {
            ExplainedNumber result = new ExplainedNumber(0f, descriptions);
            result.LimitMin(0f);
            if (needs.Party.CurrentSettlement != null && needs.Party.CurrentSettlement.Town != null) {
                result.Add(-1f, new TextObject("{=eN981XMi}In a town or castle"));
                return result;
            }

            foreach (TroopRosterElement element in needs.Party.MemberRoster.GetTroopRoster()) {
                if (element.Character.IsHero) {
                    foreach (TraitObject trait in BKTraits.Instance.DiseaseTraits) {
                        if (element.Character.GetTraitLevel(trait) > 0) {
                            result.Add(element.Character.GetTraitLevel(trait) * MedicalSuppliesPerSoldier * BannerKingsSettings.Instance.PartySuppliesFactor,
                                new TextObject("{=5Jr8zfXD}{TROOP_NAME}({DISEASE} Severity: {COUNT})")
                                .SetTextVariable("TROOP_NAME", element.Character.Name)
                                .SetTextVariable("COUNT", element.Character.GetTraitLevel(trait))
                                .SetTextVariable("DISEASE", trait.Name));
                        }
                    }
                }
            }

            return result;
        }
    }
}
