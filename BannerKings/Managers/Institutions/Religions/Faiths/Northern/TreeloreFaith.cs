﻿using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Institutions.Religions.Faiths.Northern
{
    public class TreeloreFaith : PolytheisticFaith
    {
        public override bool IsCultureNaturalFaith(CultureObject culture)
        {
            if (culture.StringId == "sturgia" || culture.StringId == "vakken")
            {
                return true;
            }

            return false;
        }
        public override bool IsHeroNaturalFaith(Hero hero) => IsCultureNaturalFaith(hero.Culture);

        public override TextObject GetBlessingAction()
        {
            return new TextObject("{=!}I would like to pledge myself to a cult.");
        }

        public override TextObject GetBlessingActionName()
        {
            return new TextObject("{=bd7HQSJH}pledge an oath to.");
        }

        public override TextObject GetBlessingConfirmQuestion()
        {
            return new TextObject("{=!}Confirm it, {?PLAYER.GENDER}sister{?}brother{\\?} of the forest, and it will be done.");
        }

        public override TextObject GetBlessingQuestion()
        {
            return new TextObject("{=!}To whom would you pledge? The Thunder-Wielder Pérkos, or the godess Méhns?");
        }

        public override TextObject GetBlessingQuickInformation()
        {
            return new TextObject("{=!}{HERO} has pledged an oath to {DIVINITY}.");
        }

        public override TextObject GetClergyForbiddenAnswer(int rank)
        {
            return new TextObject("{=!}What is forbidden? Not upholding family and tradition are crimes in the eyes of the gods. Likewise, betraying in act or word those you take as guests. But most of all, cravenness, of course. A craven is no child of forest, much like spoiled fruit is no nourishment. Fight with boldness or do not fight at all.");
        }

        public override TextObject GetClergyForbiddenAnswerLast(int rank)
        {
            return new TextObject("{=!}The struggle between righteousness and darkness is the nature of the worlds. Our world, that of mankind, lies right in its midst. Uphold all that is godly, or rot in darkness.");
        }

        public override TextObject GetClergyGreeting(int rank) => new TextObject("{=!}Hail to you, foreigner. Know that the spirits watch over all mankind, true believers or otherwise, and that no malice escapes their senses, and no misdeed goes unrecorded by the oak grain.");

        public override TextObject GetClergyGreetingInducted(int rank) => new TextObject("{=!}Hail to you, brethren. How can I help you? Do you wish to hear the truth of the gods, or, perhaps, foretell the future?");

        public override TextObject GetClergyInduction(int rank)
        {
            var induction = GetInductionAllowed(Hero.MainHero, rank);
            if (!induction.Item1)
            {
                return new TextObject("{=!}Alas, one born outside the embrace of the gods, can not choose to be embraced. Though one can be respected for their boldness, only a child of the forest can follow the path of the true gods - it is written in our ancestry. Such is the tale written in the oak grain.[if:convo_bored]");
            }

            return new TextObject("{=!}I ask of you only this - why have you not come before? My brethren of the woods, you have come to your home. The way of Pérkos, Méhns and all the gods of the heavenly canopy, that is your true nature. Your blood and bone.");
        }

        public override TextObject GetClergyInductionLast(int rank)
        {
            var induction = GetInductionAllowed(Hero.MainHero, rank);
            if (!induction.Item1)
            {
                return new TextObject("{=!}Go now, and return to those of your kind, wherever they might be. The children of the forest only accept those born in it.[rf:convo_bored]");
            }

            return new TextObject("{=!}Be welcome as a child of the forest. Defend your brethren and your gods - fight our enemies fiercely, but also be kind to those that visit your hearth. Do not try and convince them of our ways - it is not their place. Yet it is ours to keep unharmed.[if:convo_excited]");
        }

        public override TextObject GetClergyPreachingAnswer(int rank) => new TextObject("{=!}When it comes to the gods, there is nothing the words of man can say that the rustling of leaves or burbling of rivers does not tell better. The forest, offspring of the Great Oak, holds the chronicles of the past. Truth, you see, is embedded within all that which is godly, be it the river flow or the oak grain. I merely interpret it.");

        public override TextObject GetClergyPreachingAnswerLast(int rank) => new TextObject("{=!}But if you insist... We preach the way of the Thunder Wielder. Valor in defending your ancestry, honor in keeping your word, and generosity towards those you take into your hearth.");

        public override TextObject GetClergyProveFaith(int rank)
        {
            return new TextObject("{=!}Naturally, to follow the way of Pérkos is to be a true faithful. Uphold honor, honesty and righteousness. Defend your brethren with valor, as the gods that perch upon the Great Oak do over men. Those you take into your hearth as guests - treat them as your own blood, as much as good sense allows.");
        }

        public override TextObject GetClergyProveFaithLast(int rank)
        {
            return new TextObject("{=!}Most importantly, uphold tradition and ancestry as a child of the forest, remain true to your family and multiply, lest the Great Oak is unrooted by darkness.");
        }

        public override TextObject GetFaithDescription() => new TextObject("{=!}Pérkenweyd is a native faith of the Calradian continent, stretching from the Kachyar peninsula to the Chertyg mountains. Thus, it is the natural faith of the Vakken and Sturgian peoples. Though the Sturgians have been in contact with different cultures and faiths, the Sturgian populace remains true to their ancestry. Pérkenweyd, or 'tree lore', understands...");

        public override TextObject GetFaithName() => new TextObject("{=!}Pérkenweyd");
        public override string GetId() => "treelore";

        public override int GetIdealRank(Settlement settlement, bool isCapital)
        {
            if (settlement.IsVillage)
            {
                return 1;
            }

            return 0;
        }

        public override (bool, TextObject) GetInductionAllowed(Hero hero, int rank)
        {
            if (IsCultureNaturalFaith(hero.Culture))
            {
                return new(true, new TextObject("{=GAuAoQDG}You will be converted"));
            }

            return new(false, new TextObject("{=!}The {FAITH} only accepts those of {STURGIA} and {VAKKEN} cultures")
                .SetTextVariable("FAITH", GetFaithName())
                .SetTextVariable("STURGIA", Utils.Helpers.GetCulture("sturgia").Name)
                .SetTextVariable("VAKKEN", Utils.Helpers.GetCulture("vakken").Name));
        }

        public override Divinity GetMainDivinity() => mainGod;

        public override int GetMaxClergyRank() => 1;

        public override TextObject GetRankTitle(int rank) => new TextObject("{=!}Elder");

        public override MBReadOnlyList<Divinity> GetSecondaryDivinities() => pantheon.GetReadOnlyList();

        public override TextObject GetCultsDescription() => new TextObject("{=J4D4X2XJ}Cults");
    }
}
