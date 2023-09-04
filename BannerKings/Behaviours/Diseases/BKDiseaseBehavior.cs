using BannerKings.Managers.Diseases;
using BannerKings.Managers.Items;
using BannerKings.Managers.Populations;
using BannerKings.Managers.Traits;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerKings.Behaviours.Diseases
{
    internal class BKDiseaseBehavior : BannerKingsBehavior
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, OnDailyTickParty);
            CampaignEvents.SettlementEntered.AddNonSerializedListener(this, OnSettlementEntered);
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        private void OnDailyTickParty(MobileParty mobileParty)
        {
            for (int index = 0; index < mobileParty.MemberRoster.Count; index++)
            {
                CharacterObject character = mobileParty.MemberRoster.GetCharacterAtIndex(index);
                if (character.IsHero)
                {
                    foreach (TraitObject trait in BKTraits.Instance.DiseaseTraits)
                    {
                        if (character.GetTraitLevel(trait) > 0)
                        {
                            TriggerProgression(trait, Utils.Helpers.GetDiseaseFromTrait(trait), character.HeroObject);
                            TriggerInfectionParty(trait, mobileParty);
                        }
                    }
                }
            }
        }

        private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
        {
            PopulationData popData = BannerKingsConfig.Instance.PopulationManager.GetPopData(settlement);
            TriggerSpreadDisease(popData, hero);
            if (popData != null && popData.DiseaseData != null && popData.DiseaseData.ActiveDisease != null)
            {
                TriggerReceiveDisease(hero, popData.DiseaseData.ActiveDisease);
            }
            if ((hero == Hero.MainHero || mobileParty == MobileParty.MainParty))
            {
                popData.DiseaseData.PrintInfoMessage();
            }
        }

        private void TriggerInfectionParty(TraitObject trait, MobileParty mobileParty)
        {
            Disease disease = Utils.Helpers.GetDiseaseFromTrait(trait);
            if (disease != null)
            {
                float infectChance = MBRandom.RandomFloat;
                int medicalSupplies = GetMedicalItemCount(mobileParty);
                if (infectChance + disease.Infectability > mobileParty.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine) / 150f)
                {
                    if (medicalSupplies > 0)
                    {
                        ItemObject itemToRemove;
                        for (int index = 0; index < mobileParty.ItemRoster.Count; index++)
                        {
                            if (mobileParty.ItemRoster.GetItemAtIndex(index).ItemCategory == BKItemCategories.Instance.MedicalSupplies)
                            {
                                itemToRemove = mobileParty.ItemRoster.GetItemAtIndex(index);
                                mobileParty.ItemRoster.AddToCounts(itemToRemove, -1);
                                if (mobileParty == MobileParty.MainParty)
                                {
                                    MBInformationManager.AddQuickInformation(new TextObject("Your party resisted spreading " + disease.Name + "."));
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        mobileParty.MemberRoster.WoundMembersOfRoster(disease.InfectionRate * MBRandom.RandomInt(1, 10));
                        if (mobileParty == MobileParty.MainParty)
                        {
                            InformationManager.DisplayMessage(new InformationMessage(disease.Name + " has spread in your party!"));
                        }
                    }
                }
            }
        }

        private void TriggerSpreadDisease(PopulationData popData, Hero hero)
        {
            foreach (TraitObject trait in BKTraits.Instance.DiseaseTraits)
            {
                if (hero != null && hero.GetTraitLevel(trait) > 0)
                {
                    if (popData == null)
                    {
                        return;
                    }
                    Disease disease = Utils.Helpers.GetDiseaseFromTrait(trait);
                    popData.DiseaseData.TriggerInfection(disease, hero);
                }
            }
        }

        private void TriggerReceiveDisease(Hero hero, Disease disease)
        {
            if (hero != null && hero.PartyBelongedTo != null)
            {
                float infectChance = MBRandom.RandomFloat;
                float surgeonSkill = hero.PartyBelongedTo.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine) / 100f;
                int medicalSupplies = GetMedicalItemCount(hero.PartyBelongedTo);
                if (disease.Infectability > infectChance + surgeonSkill)
                {
                    if (medicalSupplies > 0)
                    {
                        ItemObject itemToRemove;
                        for (int index = 0; index < hero.PartyBelongedTo.ItemRoster.Count; index++)
                        {
                            if (hero.PartyBelongedTo.ItemRoster.GetItemAtIndex(index).ItemCategory == BKItemCategories.Instance.MedicalSupplies)
                            {
                                itemToRemove = hero.PartyBelongedTo.ItemRoster.GetItemAtIndex(index);
                                hero.PartyBelongedTo.ItemRoster.AddToCounts(itemToRemove, -1);
                                if (hero == Hero.MainHero)
                                {
                                    MBInformationManager.AddQuickInformation(new TextObject("You resisted contracting " + disease.Name + "."));
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        TraitObject diseaseTrait = Utils.Helpers.GetTraitFromDisease(disease);
                        if (diseaseTrait != null && hero.GetTraitLevel(diseaseTrait) == 0)
                        {
                            hero.SetTraitLevel(diseaseTrait, 1);
                            if (hero == Hero.MainHero)
                            {
                                MBInformationManager.AddQuickInformation(new TextObject("You have contracted " + disease.Name + "!"));
                            }
                        }
                    }
                }
            }
        }

        private void TriggerProgression(TraitObject trait, Disease disease, Hero hero)
        {
            if (trait == null || hero == null || disease == null)
            {
                return;
            }
            float progressionChance = MBRandom.RandomFloat;
            float medicineSkill;
            int medicalSupplies = GetMedicalItemCount(hero.PartyBelongedTo);
            if (hero.PartyBelongedTo.EffectiveSurgeon != null)
            {
                medicineSkill = hero.PartyBelongedTo.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine) / 100f;
            }
            else
            {
                medicineSkill = hero.GetSkillValue(DefaultSkills.Medicine) / 100f;
            }
            if (progressionChance + (disease.InfectionRate) > medicineSkill && hero.GetTraitLevel(trait) < 3)
            {
                if (medicalSupplies > 0)
                {
                    ItemObject itemToRemove;
                    for (int index = 0; index < hero.PartyBelongedTo.ItemRoster.Count; index++)
                    {
                        if (hero.PartyBelongedTo.ItemRoster.GetItemAtIndex(index).ItemCategory == BKItemCategories.Instance.MedicalSupplies)
                        {
                            itemToRemove = hero.PartyBelongedTo.ItemRoster.GetItemAtIndex(index);
                            hero.PartyBelongedTo.ItemRoster.AddToCounts(itemToRemove, -1);
                            if (hero == Hero.MainHero)
                            {
                                MBInformationManager.AddQuickInformation(new TextObject("Your case of " + disease.Name + " remains under control."));
                            }
                            break;
                        }
                    }
                }
                else
                {
                    hero.SetTraitLevel(trait, hero.GetTraitLevel(trait) + 1);
                    if (hero == Hero.MainHero)
                    {
                        MBInformationManager.AddQuickInformation(new TextObject("Your case of " + disease.Name + " has gotten worse!"));
                    }
                }
            }
            else if (progressionChance + (disease.InfectionRate) < medicineSkill && hero.GetTraitLevel(trait) > 0 && medicalSupplies > 0)
            {
                ItemObject itemToRemove;
                for (int index = 0; index < hero.PartyBelongedTo.ItemRoster.Count; index++)
                {
                    if (hero.PartyBelongedTo.ItemRoster.GetItemAtIndex(index).ItemCategory == BKItemCategories.Instance.MedicalSupplies)
                    {
                        itemToRemove = hero.PartyBelongedTo.ItemRoster.GetItemAtIndex(index);
                        hero.PartyBelongedTo.ItemRoster.AddToCounts(itemToRemove, -1);
                        if (hero == Hero.MainHero)
                        {
                            MBInformationManager.AddQuickInformation(new TextObject("Your case of " + disease.Name + " has been cured!"));
                        }
                        break;
                    }
                }
            }
            else if (progressionChance + (disease.InfectionRate) - medicineSkill >= 1f && hero.GetTraitLevel(trait) == 3 && medicalSupplies == 0)
            {
                hero.AddDeathMark(hero, TaleWorlds.CampaignSystem.Actions.KillCharacterAction.KillCharacterActionDetail.Lost);
            }
        }

        internal int GetMedicalItemCount(MobileParty mobileParty)
        {
            int count = 0;
            foreach (ItemRosterElement element in mobileParty.ItemRoster)
            {
                if (element.EquipmentElement.Item.ItemCategory == BKItemCategories.Instance.MedicalSupplies)
                {
                    count += element.Amount;
                }
            }
            return count;
        }
    }
}
