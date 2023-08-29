using BannerKings.Managers.Diseases;
using BannerKings.Managers.Populations;
using BannerKings.Managers.Traits;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerKings.Behaviours.Diseases {
    internal class BKDiseaseBehavior : CampaignBehaviorBase {
        public override void RegisterEvents() {
            CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, OnDailyTickParty);
            CampaignEvents.SettlementEntered.AddNonSerializedListener(this, OnSettlementEntered);
        }

        public override void SyncData(IDataStore dataStore) {

        }

        private void OnDailyTickParty(MobileParty mobileParty) {
            for (int index = 0; index < mobileParty.MemberRoster.Count; index++) {
                CharacterObject character = mobileParty.MemberRoster.GetCharacterAtIndex(index);
                if (character.IsHero) {
                    foreach (TraitObject trait in BKTraits.Instance.DiseaseTraits) {
                        if (character.GetTraitLevel(trait) > 0) {
                            TriggerProgression(trait, Utils.Helpers.GetDiseaseFromTrait(trait), character.HeroObject);
                            TriggerInfectionParty(trait, mobileParty);
                        }
                    }
                }
            }
        }

        private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero) {
            PopulationData popData = BannerKingsConfig.Instance.PopulationManager.GetPopData(settlement);
            TriggerSpreadDisease(popData, hero);
            if (popData != null && popData.DiseaseData != null && popData.DiseaseData.ActiveDisease != null) {
                TriggerReceiveDisease(hero, popData.DiseaseData.ActiveDisease);
            }
            if ((hero == Hero.MainHero || mobileParty == MobileParty.MainParty)) {
                popData.DiseaseData.PrintInfoMessage();
            }
        }

        private void TriggerInfectionParty(TraitObject trait, MobileParty mobileParty) {
            Disease disease = Utils.Helpers.GetDiseaseFromTrait(trait);
            if (disease != null) {
                float infectChance = MBRandom.RandomFloat;
                if (infectChance + disease.Infectability > mobileParty.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine) / 150f) {
                    mobileParty.MemberRoster.WoundMembersOfRoster(disease.InfectionRate * MBRandom.RandomInt(1, 10));
                    if (mobileParty == MobileParty.MainParty) {
                        InformationManager.DisplayMessage(new InformationMessage(disease.GetName() + " has spread in your party!"));
                    }
                }
            }
        }

        private void TriggerSpreadDisease(PopulationData popData, Hero hero) {
            foreach (TraitObject trait in BKTraits.Instance.DiseaseTraits) {
                if (hero != null && hero.GetTraitLevel(trait) > 0) {
                    if (popData == null) {
                        return;
                    }
                    Disease disease = Utils.Helpers.GetDiseaseFromTrait(trait);
                    popData.DiseaseData.TriggerInfection(disease, hero);
                }
            }
        }

        private void TriggerReceiveDisease(Hero hero, Disease disease) {
            if (hero != null && hero.PartyBelongedTo != null) {
                float infectChance = MBRandom.RandomFloat;
                float surgeonSkill = hero.PartyBelongedTo.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine) / 150f;
                if (disease.Infectability > infectChance + surgeonSkill) {
                    TraitObject diseaseTrait = Utils.Helpers.GetTraitFromDisease(disease);
                    if (diseaseTrait != null && hero.GetTraitLevel(diseaseTrait) == 0) {
                        hero.SetTraitLevel(diseaseTrait, 1);
                        if (hero == Hero.MainHero) {
                            InformationManager.DisplayMessage(new InformationMessage("You have contracted " + disease.Name.ToString() + "!"));
                        }
                    }
                }
            }
        }

        private void TriggerProgression(TraitObject trait, Disease disease, Hero hero) {
            if (trait == null || hero == null || disease == null) {
                return;
            }
            float progressionChance = MBRandom.RandomFloat;
            float medicineSkill;
            if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.EffectiveSurgeon != null) {
                medicineSkill = hero.PartyBelongedTo.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine) / 150f;
            } else {
                medicineSkill = hero.GetSkillValue(DefaultSkills.Medicine) / 150f;
            }
            if (progressionChance + (1 - disease.InfectionRate) > medicineSkill && hero.GetTraitLevel(trait) < 3) {
                hero.SetTraitLevel(trait, hero.GetTraitLevel(trait) + 1);
                if (hero == Hero.MainHero) {
                    InformationManager.DisplayMessage(new InformationMessage("Your case of " + disease.GetName() + " has gotten worse!"));
                }
            }
        }
    }
}
