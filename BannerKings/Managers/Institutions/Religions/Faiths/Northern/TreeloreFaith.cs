using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Institutions.Religions.Faiths.Northern
{
    public class TreeloreFaith : PolytheisticFaith
    {
        public override Settlement FaithSeat => Settlement.All.First(x => x.StringId == "town_S3");
        public override TextObject GetZealotsGroupName()
        {
            return new TextObject("{=h1PFZoQ8}Scions of the Great Oak");
        }

        public override TextObject GetDescriptionHint()
        {
            return new TextObject("{=DmJJGq8x}Pérkkenukos is a native faith of the Calradian continent, natural to the Vakken peoples, who stretch from the Kachyar peninsula to the Chertyg mountains. Once, they say, there was naught but the sea and sky. High above reigned Pérkken, god of sky and thunder, and deep below, Tursas, king of seas.{newline}Pérkkenukos, the faith in Pérkken, represents such oral Vakken traditions, passed on through tribes and generations, often with nuances of local folklore and beliefs, but ultimately united by a common cultural tradition.");
        }

        public override Banner GetBanner() => new Banner("11.98.2.1528.1528.764.764.1.0.0.100314.40.3.483.483.764.764.0.0.0");

        public override bool IsCultureNaturalFaith(CultureObject culture)
        {
            if (culture.StringId == "vakken")
            {
                return true;
            }

            if (culture.StringId == "lokti")
            {
                return true;
            }

            if (culture.StringId == "sturgia")
            {
                return true;
            }

            return false;
        }
        public override bool IsHeroNaturalFaith(Hero hero) => IsCultureNaturalFaith(hero.Culture);

        public override TextObject GetBlessingAction()
        {
            return new TextObject("{=zjQfp2ak}I would like to pray to the gods.");
        }

        public override TextObject GetBlessingActionName()
        {
            return new TextObject("{=sGBaOkBZ}pray to.");
        }

        public override TextObject GetBlessingConfirmQuestion()
        {
            return new TextObject("{=1wZntvX3}Confirm it, {?PLAYER.GENDER}sister{?}brother{\\?} of the forest, and it will be done.");
        }

        public override TextObject GetBlessingQuestion()
        {
            return new TextObject("{=8Yt6RAgX}To whom would you pledge? The Thunder-Wielder Pérkken, or perhaps Suurihirvi of the forest?");
        }

        public override TextObject GetBlessingQuickInformation()
        {
            return new TextObject("{=j1U1juRf}{HERO} has pledged an oath to {DIVINITY}.");
        }

        public override TextObject GetClergyForbiddenAnswer(int rank)
        {
            return new TextObject("{=Wy6xrbnk}To destroy the forest without purpose is the highest crime on can make. For not only it is the home of our people, but also of the spirits. Our ancestors and the spirits watch over our doings, to forsake them is to forsake our very being. Many a foreigner, such as the Calradoi, can not understand this. Most of them have never wandered deep into the woods to commune with the forest. They have forsaken their ancestors.");
        }

        public override TextObject GetClergyForbiddenAnswerLast(int rank)
        {
            return new TextObject("{=iraP3PjT}Many among the Sturgian nobility have also commited the crime of abandoning their ways. A shame - though different, Sturgians understand many of our beliefs.");
        }

        public override TextObject GetClergyGreeting(int rank) => new TextObject("{=nynzrPop}Tervehtuola, foreigner. I represent here the children of the forest as their Tietaja - 'Shaman', as foreigners say. We Vakken seek no disputes, but merely to live according to our ancestors.");

        public override TextObject GetClergyGreetingInducted(int rank) => new TextObject("{=bpGB5TsD}Tervehtuola, brethren! I serve here as the Tietaja of our siblings. Come see me should you need the guidance of the spirits.");

        public override TextObject GetClergyInduction(int rank)
        {
            var induction = GetInductionAllowed(Hero.MainHero, rank);
            if (!induction.Item1)
            {
                return new TextObject("{=FvwTtMFX}Alas, one born outside the embrace of the gods, can not choose to be embraced. Though one can be respected for their boldness, only a child of the forest can follow the path of the true gods - it is written in our ancestry.[if:convo_bored]");
            }

            return new TextObject("{=7p67cLst}I ask of you only this - why have you not come before? My brethren of the woods, you have come to your home. The way of Pérkken, that is your true nature. Your blood and bone.");
        }

        public override TextObject GetClergyInductionLast(int rank)
        {
            var induction = GetInductionAllowed(Hero.MainHero, rank);
            if (!induction.Item1)
            {
                return new TextObject("{=x2BRcZSr}Go now, and return to those of your kind, wherever they might be. The children of the forest only accept those born in it.[rf:convo_bored]");
            }

            return new TextObject("{=3eB4RvfE}Be welcome as a child of the forest. Defend your brethren and your gods - fight our enemies fiercely, but also be kind to those that visit your hearth. Do not try and convince them of our ways - it is not their place. Yet it is ours to keep unharmed.[if:convo_excited]");
        }

        public override TextObject GetClergyPreachingAnswer(int rank) => new TextObject("{=yaUdYrrH}To guide our people in the ways, I uphold the traditions of our forefathers. That is what I preach: our ancestral way of life, that of the children of the forest.");

        public override TextObject GetClergyPreachingAnswerLast(int rank) => new TextObject("{=KSWCVmqQ}To protect our home and our kin is our highest duty as faithful. Uphold honor in your words, uphold the gods through offerings, and protect our home by showing no mercy to those that come to harm it.");

        public override TextObject GetClergyProveFaith(int rank)
        {
            return new TextObject("{=9XJHdRjb}Find a sacred grove among the trees and pay offering to the gods. Pray to the spirits and your ancestor for guidance. Expand your family and teach them the way of our kin - we shan't forsake our ways as the Sturgians did.");
        }

        public override TextObject GetClergyProveFaithLast(int rank)
        {
            return new TextObject("{=sP2CSqNQ}Keep your words pure and honest, unlike the Battanians with their lies. Protect our home with courage, and show no mercy to our enemies. Have them fear the woods, our home, as the land of trolls and demons they call our spirits.");
        }

        public override TextObject GetFaithDescription() => new TextObject("{=6YNf9OpY}Pérkkenukos is a native faith of the Calradian continent, natural to the Vakken peoples, who stretch from the Kachyar peninsula to the Chertyg mountains. Once, they say, there was naught but the sea and sky. High above reigned Pérkken, god of sky and thunder, and deep below, Tursas, king of seas. A Great Oak once sprang, blocking all sun and moon light from land and sea. Tursas, envious of the oak's heights, set it ablaze. From its ashes, the forests grew, which the Vakken now protect.{newline}Pérkkenukos, the faith in Pérkken, represents such oral Vakken traditions, passed on through tribes and generations, often with nuances of local folklore and beliefs, but ultimately united by a common cultural tradition. As the Vakken are often isolationists, living deep in the woods, their faith is also descentralized and not represented by an organized clergy. However, they all agree on the hallowed status of the region of Omor, a traditional forest-shrine.");

        public override TextObject GetFaithName() => new TextObject("{=EpJbzCCV}Pérkkenukos");
        public override string GetId() => "treelore";

        public override int GetIdealRank(Settlement settlement)
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

            return new(false, new TextObject("{=8k60TAmt}The {FAITH} only accepts those of {STURGIA} and {VAKKEN} cultures")
                .SetTextVariable("FAITH", GetFaithName())
                .SetTextVariable("STURGIA", Utils.Helpers.GetCulture("sturgia").Name)
                .SetTextVariable("VAKKEN", Utils.Helpers.GetCulture("vakken").Name));
        }

        public override int GetMaxClergyRank() => 1;

        public override TextObject GetRankTitle(int rank) => new TextObject("{=9r8ECjQh}Tietaja");

        public override TextObject GetCultsDescription() => new TextObject("{=J4D4X2XJ}Cults");

        public override TextObject GetInductionExplanationText() => new TextObject("{=8k60TAmt}The {FAITH} only accepts those of {STURGIA} and {VAKKEN} cultures")
                .SetTextVariable("FAITH", GetFaithName())
                .SetTextVariable("STURGIA", Utils.Helpers.GetCulture("sturgia").Name)
                .SetTextVariable("VAKKEN", Utils.Helpers.GetCulture("vakken").Name);
    }
}
