using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Diseases
{
    public class Disease : BannerKingsObject
    {
        protected float infectability;

        public float Infectability => infectability;

        protected float lethality;

        public float Lethality => lethality;

        protected float infectionRate;

        public float InfectionRate => infectionRate;

        protected float resistance;

        public float Resistance => resistance;

        protected TraitObject diseaseTrait;

        public TraitObject DiseaseTrait => diseaseTrait;

        public Disease(string stringId) : base(stringId)
        {

        }

        public void Initialize(TextObject name, TextObject description, float infectability, float lethality, float infectionRate, float resistance, TraitObject trait)
        {
            Initialize(name, description);
            this.infectability = infectability;
            this.lethality = lethality;
            this.infectionRate = infectionRate;
            this.resistance = resistance;
            this.diseaseTrait = trait;
        }

        public void PostInitialize()
        {
            Disease disease = DefaultDiseases.Instance.GetById(this);
            Initialize(disease.Name, disease.Description, disease.infectability, disease.lethality, disease.infectionRate, disease.resistance, disease.DiseaseTrait);
        }
    }
}
