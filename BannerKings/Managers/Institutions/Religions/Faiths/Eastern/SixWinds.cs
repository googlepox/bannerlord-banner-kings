using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerKings.Managers.Institutions.Religions.Faiths.Eastern
{
    public class SixWinds : PolytheisticFaith
    {
        public override Settlement FaithSeat => Settlement.All.First(x => x.StringId == "town_K4");
        public override TextObject GetDescriptionHint()
        {
            return new TextObject("{=9N6T6kWF}Alti Yel, or Six Winds, represents the Devseg beliefs that the universe is, in essence, an everflowing current. Six Winds represent the aspects of this current: U�mag, Tamag, Kurmag, Kunmag, Batmag, Togmag. Each of them is represented by a god or godess, who embodies a necessary aspect of the cosmos, such as the life or death. The Devseg hold all of them in high regard, but U�mag, Heaven, as the most important guide to their ethics, as every faithful strives to reach the purity of U�mag on their death.");
        }

        public override Banner GetBanner() => new Banner("1.74.74.1836.1836.764.764.1.0.0.510.84.116.500.136.764.764.1.0.-315.510.84.116.500.136.739.744.1.0.-315.510.84.116.500.136.789.784.1.0.-315.510.84.116.500.136.764.764.1.1.314.510.84.116.500.136.789.744.1.1.314.510.84.116.500.136.739.784.1.0.314.500.84.116.550.550.764.764.1.0.-91.427.22.116.85.85.614.764.1.0.-91.150.22.116.105.125.915.764.1.0.0.521.84.116.81.73.764.604.1.0.0.145.116.116.95.85.764.924.0.0.0");

        public override TextObject GetBlessingAction() => new TextObject("{=KdMrdCH6}I would like the blessing of the Winds.");

        public override TextObject GetBlessingActionName() => new TextObject("{=sGBaOkBZ}pray to.");

        public override TextObject GetBlessingConfirmQuestion() => new TextObject("{=Yy12qJCH}Are you sure? Once I enter the realm of the Iye and the Winds, they will not take fickle words.");

        public override TextObject GetBlessingQuestion() => new TextObject("{=WO2cV4nH}Tell me which of the Winds do you favor. Or would you rather honor the She-Wolf? Perhaps Iltanlar of the mountains?");

        public override TextObject GetBlessingQuickInformation() => new TextObject("{=pACOhkrE}{HERO} is now pledged to {DIVINITY}.");

        public override TextObject GetClergyForbiddenAnswer(int rank) => new TextObject("{=!}");

        public override TextObject GetClergyForbiddenAnswerLast(int rank) => new TextObject("{=!}");

        public override TextObject GetClergyGreeting(int rank)
        {
            if (IsCultureNaturalFaith(Hero.MainHero.Culture)) return new TextObject("{=NBU1AzLZ}Be welcome, kin. Here we live in harmony with the spirits and breathe in the pure Winds. I am the Kam and speak for them. Come to me whenever you need to contact the spirits, or interpet the messages of the Winds.");
            else return new TextObject("{=Lh5HteaA}Welcome, {?PLAYER.GENDER}madam{?}sir{\\?}. I am the local Kam - a shaman as your people say - and speak for the Devseg. We live in harmony with the spirits and Winds, and hope you can do so as well.");
        }

        public override TextObject GetClergyGreetingInducted(int rank) => new TextObject("{=bDUPLpkx}Be welcome, kin. I can see that you are a faithful of the Heavens, and listener to the Winds. I am the local Kam, if you need to contact the Iye. May good winds guide you.");

        public override TextObject GetClergyInduction(int rank)
        {
            if (GetInductionAllowed(Hero.MainHero, rank).Item1)
            {
                return new TextObject("{=e3vNiy2x}A Kam has no power to 'induct' one into the Winds. Do you not hear them, whispering into your ear? The words of gods are everywhere. You merely pay attention to them or not.");
            }
            else
            {
                return new TextObject("{=GwqpzLHg}A Kam has no power to 'induct' one into the Winds. Even if I could, I would not. You are not one who is fit to bear the name of our good gods. Your foreign customs are a corruption of nature, which is unacceptable to the Winds.");
            }
        }

        public override TextObject GetClergyInductionLast(int rank)
        {
            if (GetInductionAllowed(Hero.MainHero, rank).Item1)
            {
                return new TextObject("{=CcRy9eZL}Go, {?PLAYER.GENDER}madam{?}sir{\\?}, and live according to gods. That is your true induction. Uphold the virtues of the Winds and be in harmony with the Iye. You want to reach U�mag, do you not? May good winds guide you.");
            }
            else
            {
                return new TextObject("{=jBxdRc4h}If you truly wish to live by our traditions, then take them upon yourself. Become a {?PLAYER.GENDER}woman{?}man{\\?} of the hordes, sleep in a yurt and gaze into the Heavens for enlightenment. Such is the way of our forefathers, the way of the Winds.");
            }
        }

        public override TextObject GetClergyPreachingAnswer(int rank) => new TextObject("{=agXGE70X}Leave preaching for the aloof Calradoi. Here, I interpret the Winds. To the Calradoi the world may seem a static, fixed place. But it is not. It is an everflowing current, which flows in 6 directions. These are the Winds that make everything. As a river flows continously downstream, so does everything towards the fate of the gods.");

        public override TextObject GetClergyPreachingAnswerLast(int rank) => new TextObject("{=qHB6zwLQ}We must uphold the teachings of the Winds. To be true to our words and give just treatment to others. To be in harmony with the Iye - spirits, as others call them. Only in such ways we may ascend with Yel U�mag at the time of our death.");

        public override TextObject GetClergyProveFaith(int rank) => new TextObject("{=6xMsXxyA}Give just treatment, specially do those you least want to. Be true to your word, always. Never slaughter an animal without a just reason. Disturb not the Iye, for they share this plane with us, through the rivers, forests and mountains, as the gods desired. U�mag is realm of the virtuous, and only by upholding these principles you may enter.");

        public override TextObject GetClergyProveFaithLast(int rank) => new TextObject("{=ejngAYfp}If you wish to be zealous, hear me then. Sacrifice to the Winds, 6 horses, 6 lambs or 6 cattle heads. Horses so that they may ascend to U�mag as Khiimori and bless us with health. Lambs to bless our steppes with bountiful game and grass. Cattle so that Iltanlar may bless with the strength of his axe.");

        public override TextObject GetCultsDescription() => new TextObject("Gods");

        public override TextObject GetFaithDescription() => new TextObject("{=9N6T6kWF}Alti Yel, or Six Winds, represents the Devseg beliefs that the universe is, in essence, an everflowing current. Six Winds represent the aspects of this current: U�mag, Tamag, Kurmag, Kunmag, Batmag, Togmag. Each of them is represented by a god or godess, who embodies a necessary aspect of the cosmos, such as the life or death. The Devseg hold all of them in high regard, but U�mag, Heaven, as the most important guide to their ethics, as every faithful strives to reach the purity of U�mag on their death. Besides the pantheon of gods, spirits are known to inhabit the natural world, specially mountains, streams and lakes. Such spirits can be benign or otherwise - only through the shamans, the Kams, can the Devseg communicate with and understand them. The Devseg also believe in the Great She-Wolf, who many a great house claims to be their ancestors. Sighting wolves is often considered a good omen, and to be considered a half-wolf is a great honor. Notably tolerant of other faiths, the Six Winds shamans have also incorporated the local Devseg beliefs of Iltanlar, a mountain god of a Devseg tribe that settled on Baltakhand long before the arrival of the hordes.");

        public override TextObject GetFaithName() => new TextObject("{=n4J1ccVy}Alti Yel");

        public override string GetId() => "sixWinds";

        public override int GetIdealRank(Settlement settlement)
        {
            if (settlement.IsVillage || settlement.IsTown)
            {
                return 1;
            }

            return 0;
        }

        public override (bool, TextObject) GetInductionAllowed(Hero hero, int rank)
        {
            TextObject text = new TextObject("{=aSkNfvzG}Induction is possible.");
            bool possible = IsCultureNaturalFaith(hero.Culture);
            if (!possible)
            {
                text = new TextObject("{=1ghdQMKn}Your culture is not accepted.");
            }

            return new ValueTuple<bool, TextObject>(possible, text);
        }

        public override TextObject GetInductionExplanationText() => new TextObject("{=tWmeVUWR}You need to be of a Devseg culture (Khuzait, Iltanlar)");

        public override int GetMaxClergyRank() => 1;

        public override TextObject GetRankTitle(int rank) => new TextObject("{=nXDrh2c5}Kam");

        public override bool IsCultureNaturalFaith(CultureObject culture) => culture.StringId == "khuzait" || culture.StringId == "iltanlar";

        public override bool IsHeroNaturalFaith(Hero hero) => IsCultureNaturalFaith(hero.Culture);

        public override TextObject GetZealotsGroupName() => new TextObject("{=VGFdp6hB}Followers of the Heavens");
    }
}
