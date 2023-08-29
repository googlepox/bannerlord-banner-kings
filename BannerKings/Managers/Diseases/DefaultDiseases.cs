using BannerKings.Managers.Traits;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Diseases
{
    internal class DefaultDiseases : DefaultTypeInitializer<DefaultDiseases, Disease>
    {
        public Disease Plague {
            get; private set;
        }

        public Disease Flu {
            get; private set;
        }

        public Disease Consumption {
            get; private set;
        }

        public Disease Typhus {
            get; private set;
        }

        public Disease Measles {
            get; private set;
        }

        public Disease Syphilis {
            get; private set;
        }

        public Disease Smallpox {
            get; private set;
        }

        public override IEnumerable<Disease> All {
            get {
                yield return Plague;
                yield return Flu;
                yield return Consumption;
                yield return Typhus;
                yield return Measles;
                yield return Syphilis;
                yield return Smallpox;
                foreach (Disease item in ModAdditions)
                {
                    yield return item;
                }
            }
        }

        public override void Initialize()
        {
            Plague = new Disease("plague");
            Plague.Initialize(new TextObject("Plague"), new TextObject("{=!}"), 0.9f, 10f, 2.9f, 0.9f, BKTraits.Instance.DiseasePlague);
            Flu = new Disease("flu");
            Flu.Initialize(new TextObject("Flu"), new TextObject("{=!}"), 0.7f, 1f, 0.5f, 2.5f, BKTraits.Instance.DiseaseFlu);
            Consumption = new Disease("consumption");
            Consumption.Initialize(new TextObject("Consumption"), new TextObject("{=!}"), 0.3f, 10f, 2.22f, 0.7f, BKTraits.Instance.DiseaseConsumption);
            Typhus = new Disease("typhus");
            Typhus.Initialize(new TextObject("Typhus"), new TextObject("{=!}"), 0.5f, 6f, 2.44f, 0.4f, BKTraits.Instance.DiseaseTyphus);
            Measles = new Disease("measles");
            Measles.Initialize(new TextObject("Measles"), new TextObject("{=!}"), 0.7f, 8f, 2.6f, 0.4f, BKTraits.Instance.DiseaseMeasles);
            Syphilis = new Disease("syphilis");
            Syphilis.Initialize(new TextObject("Syphilis"), new TextObject("{=!}"), 0.6f, 7f, 2.4f, 0.5f, BKTraits.Instance.DiseaseSyphilis);
            Smallpox = new Disease("smallpox");
            Smallpox.Initialize(new TextObject("Smallpox"), new TextObject("{=!}"), 0.7f, 10f, 2.7f, 0.8f, BKTraits.Instance.DiseaseSmallpox);
        }
    }
}
