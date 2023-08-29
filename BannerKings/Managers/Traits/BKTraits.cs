using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Traits
{
    public class BKTraits : DefaultTypeInitializer<BKTraits, TraitObject>
    {
        public TraitObject PresetScholar { get; } = new TraitObject("PresetScholar");
        public TraitObject PresetDruid { get; } = new TraitObject("PresetDruid");

        public TraitObject Just { get; } = new TraitObject("Just");
        public TraitObject Humble { get; } = new TraitObject("Humble");
        public TraitObject Diligent { get; } = new TraitObject("Diligent");
        public TraitObject Ambitious { get; } = new TraitObject("Ambitious");
        public TraitObject Zealous { get; } = new TraitObject("Zealous");
        public TraitObject Seductive { get; } = new TraitObject("Seductive");

        public TraitObject AptitudeViolence { get; } = new TraitObject("AptitudeViolence");
        public TraitObject AptitudeErudition { get; } = new TraitObject("AptitudeErudition");
        public TraitObject AptitudeSocializing { get; } = new TraitObject("AptitudeSocializing");

        public TraitObject Musician { get; } = new TraitObject("Musician");

        public TraitObject Castrated { get; } = new TraitObject("Castrated");

        public TraitObject DiseasePlague { get; } = new TraitObject("Plague");

        public TraitObject DiseaseFlu { get; } = new TraitObject("Flu");

        public TraitObject DiseaseConsumption { get; } = new TraitObject("Consumption");

        public TraitObject DiseaseTyphus { get; } = new TraitObject("Typhus");

        public TraitObject DiseaseMeasles { get; } = new TraitObject("Syphilis");

        public TraitObject DiseaseSyphilis { get; } = new TraitObject("Syphilis");

        public TraitObject DiseaseSmallpox { get; } = new TraitObject("Smallpox");

        public override IEnumerable<TraitObject> All => throw new NotImplementedException();

        public IEnumerable<TraitObject> PresetTraits {
            get {
                yield return PresetScholar;
            }
        }

        public IEnumerable<TraitObject> PoliticalTraits {
            get {
                yield return DefaultTraits.Oligarchic;
                yield return DefaultTraits.Authoritarian;
                yield return DefaultTraits.Egalitarian;
            }
        }

        public IEnumerable<TraitObject> PersonalityTraits {
            get {
                yield return DefaultTraits.Honor;
                yield return DefaultTraits.Calculating;
                yield return DefaultTraits.Mercy;
                yield return DefaultTraits.Valor;
                yield return DefaultTraits.Generosity;
                yield return Just;
                yield return Humble;
                yield return Diligent;
                yield return Ambitious;
                yield return Zealous;
                yield return Seductive;
            }
        }

        public IEnumerable<TraitObject> AptitudeTraits {
            get {
                yield return AptitudeViolence;
                yield return AptitudeErudition;
                yield return AptitudeSocializing;
            }
        }

        public IEnumerable<TraitObject> LifestyleTraits {
            get {
                yield return Musician;
            }
        }

        public IEnumerable<TraitObject> MedicalTraits {
            get {
                yield return Castrated;
            }
        }

        public IEnumerable<TraitObject> DiseaseTraits {
            get {
                yield return DiseasePlague;
                yield return DiseaseFlu;
                yield return DiseaseConsumption;
                yield return DiseaseTyphus;
                yield return DiseaseMeasles;
                yield return DiseaseSyphilis;
                yield return DiseaseSmallpox;
            }
        }

        public override void Initialize()
        {
            Seductive.Initialize(new TextObject("{=x384LrUe}Seductiveness"),
                new TextObject("{=!}"),
                true,
                -2,
                2);

            Just.Initialize(new TextObject("{=1q03XzNp}Justice"),
                new TextObject("{=1q03XzNp}Justice is the appropriate punishment and reward for a given deed. Just rulers are often respected by their vassals for delivering appropriate sentences, but also adequately rewarding loyalty."),
                true,
                -2,
                2);

            Humble.Initialize(new TextObject("{=g2YTeU2e}Humility"),
                new TextObject("{=!}"),
                true,
                -2,
                2);

            Diligent.Initialize(new TextObject("{=18oVwc0j}Diligence"),
                new TextObject("{=GQcHnPjV}Diligence is one's willingness to work tirelessly towards their goal."),
                true,
                -2,
                2);

            Ambitious.Initialize(new TextObject("{=vZizAC8R}Ambition"),
                new TextObject("{=!}"),
                true,
                -2,
                2);

            Zealous.Initialize(new TextObject("{=HLML3UUw}Zealotry"),
                new TextObject("{=e18wpkZB}Zealotry is one's fervor for their faith. Zealots will favor their faith's teachings over the opinions of others, and justify their actions through faith. Zealous persons are seen are more pious within their faith."),
                true,
                -2,
                2);

            AptitudeViolence.Initialize(new TextObject("{=6Az4EDih}Violence"),
                new TextObject("{=yVBYM231}Aptitude for violence describes how inclined one is towards hurting others. Violent persons are more inclined to learn combat skills."),
                true,
                -2,
                2);

            AptitudeErudition.Initialize(new TextObject("{=b7tgia5e}Erudition"),
                new TextObject("{=3GMTHrkh}Aptitude for erudition describes how inclined one is towards intellectual tasks. Erudite persons are more inclined to learn intellectual skills such as Medicine, Scholarship, and others."),
                true,
                -2,
                2);

            AptitudeSocializing.Initialize(new TextObject("{=o2Zvo6fr}Socializing"),
                new TextObject("{=1ABqAjFQ}Aptitude for socializing describes how apt one is in expressing themselves and resolving conflicts. Social persons are more inclined to learn Leadership, Charm and Trade skills."),
                true,
                -2,
                2);

            DiseasePlague.Initialize(new TextObject("Plague"), new TextObject("{=!}"), true, 0, 3);
            DiseaseFlu.Initialize(new TextObject("Flu"), new TextObject("{=!}"), true, 0, 3);
            DiseaseConsumption.Initialize(new TextObject("Consumption"), new TextObject("{=!}"), true, 0, 3);
            DiseaseTyphus.Initialize(new TextObject("Typhus"), new TextObject("{=!}"), true, 0, 3);
            DiseaseMeasles.Initialize(new TextObject("Measles"), new TextObject("{=!}"), true, 0, 3);
            DiseaseSyphilis.Initialize(new TextObject("Syphilis"), new TextObject("{=!}"), true, 0, 3);
            DiseaseSmallpox.Initialize(new TextObject("Smallpox"), new TextObject("{=!}"), true, 0, 3);
        }
    }
}
