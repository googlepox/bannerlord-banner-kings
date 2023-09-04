using BannerKings.Managers.Titles;
using BannerKings.Managers.Titles.Laws;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerKings.UI.Kingdoms
{
    public class KingdomDemesneVM : BannerKingsViewModel
    {
        private MBBindingList<DemesneLawVM> laws;
        private MBBindingList<HeirVM> heirs;
        private HeirVM mainHeir;
        private string successionName, successionDescription;

        public KingdomDemesneVM(FeudalTitle title, Kingdom kingdom) : base(null, false)
        {
            Title = title;
            Kingdom = kingdom;
            laws = new MBBindingList<DemesneLawVM>();
            Heirs = new MBBindingList<HeirVM>();
        }

        public FeudalTitle Title { get; private set; }
        public Kingdom Kingdom { get; private set; }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Laws.Clear();
            Heirs.Clear();

            if (Title != null)
            {
                SuccessionDescription = Title.Contract.Succession.Description.ToString();

                bool isKing = Kingdom.Leader == Hero.MainHero && Title.deJure == Hero.MainHero;
                foreach (var law in Title.Contract.DemesneLaws)
                {
                    Laws.Add(new DemesneLawVM(DefaultDemesneLaws.Instance.GetLawsByType(law.LawType)
                        .Where(x => x.IsAdequateForKingdom(Kingdom)).ToList(),
                        law,
                        isKing,
                        OnChange));
                }

                var candidates = BannerKingsConfig.Instance.TitleModel.GetSuccessionCandidates(Kingdom.Leader, Title);
                var explanations = new Dictionary<Hero, ExplainedNumber>();

                foreach (Hero hero in candidates)
                {
                    var explanation = BannerKingsConfig.Instance.TitleModel.GetSuccessionHeirScore(Kingdom.Leader, hero, Title, true);
                    explanations.Add(hero, explanation);
                }

                var sorted = (from x in explanations
                              orderby x.Value.ResultNumber descending
                              select x).Take(6);

                for (int i = 0; i < sorted.Count(); i++)
                {
                    var hero = sorted.ElementAt(i).Key;
                    var exp = sorted.ElementAt(i).Value;
                    if (i == 0)
                    {
                        MainHeir = new HeirVM(hero, exp);
                    }
                    else
                    {
                        Heirs.Add(new HeirVM(hero, exp));
                    }
                }
            }
        }

        private void OnChange(SelectorVM<BKDemesneLawItemVM> obj)
        {
            if (obj.SelectedItem != null)
            {
                var vm = obj.GetCurrentItem();
                var resultLaw = DefaultDemesneLaws.Instance.All.FirstOrDefault(x => x.Equals(vm.DemesneLaw));
                if (resultLaw != null && !resultLaw.Equals(vm.DemesneLaw))
                {
                    InformationManager.ShowInquiry(new InquiryData(new TextObject("{=yAPnOQQQ}Enact Law").ToString(),
                        new TextObject("{=RSWao3jU}Enact the {LAW} law thoughtout the demesne of {TITLE}. The law will be enacted for every title in the hierarchy.\n\nCost: {INFLUENCE} {INFLUENCE_ICON}")
                        .SetTextVariable("LAW", resultLaw.Name)
                        .SetTextVariable("TITLE", Title.FullName)
                        .SetTextVariable("INFLUENCE", resultLaw.InfluenceCost)
                        .SetTextVariable("INFLUENCE_ICON", GameTexts.FindText("str_html_influence_icon"))
                        .ToString(),
                        Hero.MainHero.Clan.Influence >= resultLaw.InfluenceCost,
                        true,
                        GameTexts.FindText("str_selection_widget_accept").ToString(),
                        GameTexts.FindText("str_selection_widget_cancel").ToString(),
                        () =>
                        {
                            Title.EnactLaw(resultLaw, Hero.MainHero);
                            RefreshValues();
                        },
                        () => RefreshValues()));
                }
            }
        }

        [DataSourceProperty]
        public string GovernmentText => new TextObject("{=!}Government").ToString();

        [DataSourceProperty]
        public string InheritanceText => new TextObject("{=!}Inheritance").ToString();

        [DataSourceProperty]
        public string GenderLawText => new TextObject("{=!}Gender Law").ToString();

        [DataSourceProperty]
        public string StructureText => new TextObject("{=!}Contract Structure").ToString();

        [DataSourceProperty]
        public string GovernmentName => Title.Contract.Government.Name.ToString();

        [DataSourceProperty]
        public string SuccessionTName => Title.Contract.Succession.Name.ToString();

        [DataSourceProperty]
        public string InheritanceName => Title.Contract.Inheritance.Name.ToString();

        [DataSourceProperty]
        public string GenderLawName => Title.Contract.GenderLaw.Name.ToString();

        [DataSourceProperty]
        public string HeirText => new TextObject("{=vArnerHC}Heir").ToString();

        [DataSourceProperty]
        public string SuccessionText => new TextObject("{=rTUgik07}Succession").ToString();

        [DataSourceProperty]
        public string LawsText => new TextObject("{=fE6RYz1k}Laws").ToString();

        [DataSourceProperty]
        public string LawsDescriptionText => new TextObject("{=MbSsFJNY}Demesne Laws may be changed a year after they are issued. Changes are made by the sovereign or through voting by the Peers.").ToString();


        [DataSourceProperty]
        public string SuccessionDescription
        {
            get => successionDescription;
            set
            {
                if (value != successionDescription)
                {
                    successionDescription = value;
                    OnPropertyChangedWithValue(value, "SuccessionDescription");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<DemesneLawVM> Laws
        {
            get => laws;
            set
            {
                if (value != laws)
                {
                    laws = value;
                    OnPropertyChangedWithValue(value, "Laws");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<HeirVM> Heirs
        {
            get => heirs;
            set
            {
                if (value != heirs)
                {
                    heirs = value;
                    OnPropertyChangedWithValue(value, "Heirs");
                }
            }
        }

        [DataSourceProperty]
        public HeirVM MainHeir
        {
            get => mainHeir;
            set
            {
                if (value != mainHeir)
                {
                    mainHeir = value;
                    OnPropertyChangedWithValue(value, "MainHeir");
                }
            }
        }
    }
}
