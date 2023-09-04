using BannerKings.Managers.Buildings;
using BannerKings.Managers.Diseases;
using BannerKings.Managers.Innovations;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace BannerKings.Managers.Populations
{
    public class DiseaseData : BannerKingsData
    {
        private ExplainedNumber sanitation {
            get; set;
        }

        [SaveableProperty(1)]
        private int infected {
            get; set;
        }

        [SaveableProperty(2)]
        private float contamination {
            get; set;
        }

        [SaveableProperty(3)]
        private Disease activeDisease {
            get; set;
        }

        [SaveableProperty(4)]
        private int terminalCases {
            get; set;
        }

        [SaveableProperty(5)]
        public int deathToll {
            get; set;
        }

        [SaveableProperty(6)]
        private Settlement Settlement {
            get; set;
        }

        public ExplainedNumber Sanitation => sanitation;

        public float Infected => infected;

        public float Contamination => contamination;

        public int TerminalCases => terminalCases;

        public Disease ActiveDisease => activeDisease;

        public DiseaseData(PopulationData data, Settlement settlement)
        {
            Settlement = settlement;
            infected = 0;
            contamination = 0;
            terminalCases = 0;
            deathToll = 0;
        }

        internal override void Update(PopulationData data = null)
        {
            float totalPop = data.TotalPop;
            UpdateSanitation(data);
            UpdateInfected(totalPop);
            UpdateContamination(totalPop);
            UpdateTerminalCases();
            UpdateDeathToll();
        }

        internal void UpdateSanitation(PopulationData data)
        {
            float popCap = BannerKingsConfig.Instance.GrowthModel.CalculateSettlementCap(Settlement, data, true).ResultNumber;
            float baseSanitation = 1 - (data.TotalPop / popCap);
            sanitation = new ExplainedNumber(baseSanitation, false);
            if (Settlement != null && Settlement.IsTown)
            {
                foreach (Building building in Settlement?.Town?.Buildings)
                {
                    if (building.BuildingType == DefaultBuildingTypes.SettlementAquaducts) {
                        sanitation.Add(0.15f, new TextObject("{=!}Aqueducts"));
                    } else if (building.BuildingType == DefaultBuildingTypes.SettlementMarketplace) {
                        sanitation.Add(-0.15f, new TextObject("{=!}Marketplace"));
                    } else if (building.BuildingType == DefaultBuildingTypes.SettlementForum) {
                        sanitation.Add(-0.05f, new TextObject("{=!}Forum"));
                    } else if (building.BuildingType == BKBuildings.Instance.Sewers) {
                        sanitation.Add(0.15f, new TextObject("{=!}Sewers"));
                    } else if (building.BuildingType == BKBuildings.Instance.PublicBaths) {
                        sanitation.Add(0.15f, new TextObject("{=!}Public Baths"));
                    }
                }
            }
            InnovationData innovationData = BannerKingsConfig.Instance.InnovationsManager.GetInnovationData(Settlement.Culture);
            foreach (Innovation innovation in innovationData.Innovations)
            {
                if (innovation == DefaultInnovations.Instance.PublicWorks)
                {
                    sanitation.AddFactor(1.1f, new TextObject("{=d3aY0Bbb}Public Works"));
                }
                else if (innovation == DefaultInnovations.Instance.Sewage) {
                    sanitation.AddFactor(1.05f, new TextObject("{=!}Sewage"));
                }
                else if (innovation == DefaultInnovations.Instance.Soap) {
                    sanitation.AddFactor(1.05f, new TextObject("{=!}Soap"));
                }
            }
        }

        internal void UpdateInfected(float totalPop)
        {
            if (activeDisease == null && contamination == 0)
            {
                Disease disease = GetRandDisease();
                float infectChance = MBRandom.RandomFloat;
                if (infectChance + sanitation.ResultNumber < disease.Infectability)
                {
                    activeDisease = DefaultDiseases.Instance.Plague;
                    infected = (int)(disease.InfectionRate * MBRandom.RandomInt(1, (int)(disease.Infectability * 100)));
                    contamination = (infected / totalPop) * 100;
                    if (Settlement.OwnerClan == Clan.PlayerClan)
                    {
                        OutbreakMessage();
                    }
                }
            }
            else if (activeDisease != null && sanitation.ResultNumber <= activeDisease.Infectability)
            {
                infected = (int)(infected * activeDisease.InfectionRate);
            }
            else if (activeDisease != null)
            {
                if (Settlement.OwnerClan == Clan.PlayerClan)
                {
                    EradicatedMessage();
                }
                activeDisease = null;
                infected = 0;
                deathToll = 0;
            }
        }

        internal void UpdateContamination(float totalPop)
        {
            if (infected > 0 && activeDisease != null)
            {
                contamination = (infected / totalPop) * 100;
            }
            else
            {
                contamination = 0;
            }
        }

        internal void UpdateTerminalCases()
        {
            if (contamination > 0 && activeDisease != null)
            {
                float killChance = MBRandom.RandomFloat;
                if (killChance < activeDisease.Lethality)
                {
                    int potentialCases = (int)(MBRandom.RandomInt(1, (int)(infected * (activeDisease.Lethality))));
                    terminalCases = potentialCases > infected ? infected : potentialCases;
                }
            }
            else
            {
                terminalCases = 0;
            }
        }

        internal void UpdateDeathToll()
        {
            if (activeDisease == null)
            {
                return;
            }
            infected -= terminalCases;
            deathToll += terminalCases;
            if (Settlement.OwnerClan == Clan.PlayerClan && terminalCases > 0)
            {
                DeathTollMessage();
            }
            terminalCases = 0;
        }

        internal void TriggerInfection(Disease disease, Hero hero)
        {
            if (contamination > 0 && activeDisease != null)
            {
                return;
            }
            float infectionChance = MBRandom.RandomFloat;
            if (infectionChance + sanitation.ResultNumber < disease.Infectability)
            {
                activeDisease = disease;
                infected = (int)(disease.InfectionRate * MBRandom.RandomInt(1, (int)(disease.Infectability * 100)));
                if (Settlement.OwnerClan == Clan.PlayerClan)
                {
                    OutbreakMessage();
                }
            }
        }

        internal Disease GetRandDisease()
        {
            int randInt = MBRandom.RandomInt(0, DefaultTypeInitializer<DefaultDiseases, Disease>.Instance.All.Count());
            return DefaultTypeInitializer<DefaultDiseases, Disease>.Instance.All.ElementAt(randInt);
        }

        internal void OutbreakMessage()
        {
            InformationManager.DisplayMessage(new InformationMessage("An outbreak of " + activeDisease.Name.ToString() + " has broken out in " + Settlement + ", infecting " + infected + " people.", Colors.Yellow));
        }

        internal void DeathTollMessage()
        {
            InformationManager.DisplayMessage(new InformationMessage(activeDisease.Name.ToString() + " has killed " + terminalCases +  " people in " + Settlement + ", raising the death toll to " + deathToll + ".", Colors.Yellow));
        }

        internal void EradicatedMessage()
        {
            InformationManager.DisplayMessage(new InformationMessage(activeDisease.Name.ToString() + " has been eradicated from " + Settlement.Name + ".", Colors.Cyan));
        }

        internal void PrintInfoMessage()
        {
            if (activeDisease == null)
            {
                InformationManager.DisplayMessage(new InformationMessage("This town has no known outbreak.", Colors.Cyan));
                return;
            }
            InformationManager.DisplayMessage(new InformationMessage("There is a current outbreak of " + activeDisease.Name.ToString() + " here. It has killed " + deathToll +  " people so far, with " + contamination + "% of the population still infected.", Colors.Yellow));
        }
    }
}
