using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BannerKings.Behaviours.Retainer;
using BannerKings.Extensions;
using BannerKings.Managers.Court;
using BannerKings.Managers.Titles;
using BannerKings.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerKings.Models.Vanilla
{
    public class BKClanFinanceModel : DefaultClanFinanceModel
    {
        private Dictionary<Workshop, int> workshopTaxes = new Dictionary<Workshop, int>();

        public override int CalculateNotableDailyGoldChange(Hero hero, bool applyWithdrawals)
        {
            var totalTaxes = 0;
            foreach (var wk in hero.OwnedWorkshops)
            {
                totalTaxes += wk.TaxExpenses();
            }

            var estates = CalculateOwnerIncomeFromEstates(hero, applyWithdrawals);
            return base.CalculateNotableDailyGoldChange(hero, applyWithdrawals) - totalTaxes + estates;
        }

        public override int CalculateOwnerIncomeFromWorkshop(Workshop workshop)
        {
            var result = MathF.Max(0f, workshop.ProfitMade / 2f);
            var taxes = workshop.TaxExpenses();
            if (workshopTaxes.ContainsKey(workshop))
            {
                workshopTaxes[workshop] = taxes;
            }
            else
            {
                workshopTaxes.Add(workshop, taxes);
            }

            return (int)result;
        }

        public int CalculateOwnerIncomeFromEstates(Hero owner, bool applyWithdrawals)
        {
            float result = 0;
            foreach (var estate in BannerKingsConfig.Instance.PopulationManager.GetEstates(owner)) 
            {
                if (estate.EstatesData.Settlement.MapFaction.IsAtWarWith(owner.MapFaction))
                {
                    continue;
                }

                var estateIncome = estate.Income;
                result += estateIncome;
                if (applyWithdrawals)
                {
                    estate.TaxAccumulated -= (int)estateIncome;
                    estate.LastIncome = (int)estateIncome;
                }
            }

            return (int)result;
        }

        public int GetWorkshopTaxes(Workshop workshop)
        {
            var result = 0;
            if (workshopTaxes.ContainsKey(workshop))
            {
                result = workshopTaxes[workshop];
            }

            return result;
        }

        public override int CalculateOwnerIncomeFromCaravan(MobileParty caravan) => 
            (int)(BannerKingsSettings.Instance.RealisticCaravanIncome ? 0f : MathF.Max(0f, (caravan.PartyTradeGold - 10000) / 2f));

        public override ExplainedNumber CalculateClanGoldChange(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false)
        {
            var baseResult = base.CalculateClanGoldChange(clan, true, applyWithdrawals, includeDetails);
            if (BannerKingsConfig.Instance.TitleManager == null)
            {
                return baseResult;
            }

            AddExpenses(clan, ref baseResult, applyWithdrawals);
            AddIncomes(clan, ref baseResult, applyWithdrawals);

            return baseResult;
        }

        public override ExplainedNumber CalculateClanIncome(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false)
        {
            var baseResult = base.CalculateClanIncome(clan, includeDescriptions, applyWithdrawals, includeDetails);
            if (BannerKingsConfig.Instance.TitleManager != null)
            {
                AddIncomes(clan, ref baseResult, applyWithdrawals);
            }

            return baseResult;
        }

        public override ExplainedNumber CalculateClanExpenses(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false)
        {
            var baseResult = base.CalculateClanExpenses(clan, includeDescriptions, applyWithdrawals, includeDetails);
            if (BannerKingsConfig.Instance.TitleManager != null)
            {
                AddExpenses(clan, ref baseResult, applyWithdrawals);
            }

            return baseResult;
        }

        public void AddIncomes(Clan clan, ref ExplainedNumber result, bool applyWithdrawals)
        {
            Contract contract = TaleWorlds.CampaignSystem.Campaign.Current.GetCampaignBehavior<BKRetainerBehavior>().GetContract();
            if (contract != null)
            {
                result.Add(contract.Wage, new TextObject("{=cYac1rMJ}Retainer service for {HERO}")
                    .SetTextVariable("HERO", contract.Contractor.Name));
            }

            foreach (var hero in clan.Heroes)
            {
                int estateIncome = CalculateOwnerIncomeFromEstates(hero, applyWithdrawals);

                if (applyWithdrawals && hero != clan.Leader)
                {
                    hero.ChangeHeroGold(estateIncome);
                }
                else
                {
                    result.Add(estateIncome, new TextObject("{=QEy4Ddar}Estate properties"));
                }
            }

            var kingdom = clan.Kingdom;
            var wkModel = (BKWorkshopModel)TaleWorlds.CampaignSystem.Campaign.Current.Models.WorkshopModel;

            int totalWorkshopTaxes = 0;
            int totalNotablesAids = 0;
            foreach (var town in clan.Fiefs)
            {
                if (!BannerKingsConfig.Instance.AI.AcceptNotableAid(clan, BannerKingsConfig.Instance.PopulationManager.GetPopData(town.Settlement)))
                {
                    continue;
                }
            }

            if (totalNotablesAids > 0)
            {
                result.Add(totalNotablesAids,
                    new TextObject("{=WYDGftvz}Notable aids"));
            }

            var dictionary = BannerKingsConfig.Instance.TitleManager.CalculateVassals(clan);
            if (dictionary.Count >= 0)
            {
                foreach (var pair in dictionary)
                {
                    if (clan.Kingdom != pair.Key.Kingdom)
                    {
                        continue;
                    }

                    var amount = pair.Value.Aggregate(0f, (current, title) => current + (int) title.DueTax);
                    result.Add(amount, new TextObject("{=6keRYbQa}Taxes from {CLAN}").SetTextVariable("CLAN", pair.Key.Name));
                }
            }

            if (!clan.IsUnderMercenaryService && clan.Kingdom != null && FactionManager.GetEnemyKingdoms(clan.Kingdom).Any())
            {
                var title = BannerKingsConfig.Instance.TitleManager.GetSovereignTitle(kingdom);
                if (title is {Contract: { }} &&
                    title.Contract.Rights.Contains(FeudalRights.Army_Compensation_Rights))
                {
                    var model = new DefaultClanFinanceModel();
                    var expense = model.GetType().GetMethod("AddExpenseFromLeaderParty", BindingFlags.Instance | BindingFlags.NonPublic);
                    object[] array = {clan, new ExplainedNumber(), applyWithdrawals};

                    expense.Invoke(model, array);

                    var payment = ((ExplainedNumber) array[1]).ResultNumber * -1f;
                    var limit = clan.Tier * 1000f;

                    if (payment > limit)
                    {
                        payment = limit;
                    }

                    if (kingdom.KingdomBudgetWallet >= payment)
                    {
                        if (applyWithdrawals)
                        {
                            kingdom.KingdomBudgetWallet -= (int) payment;
                        }

                        result.Add(payment, new TextObject("{=r1iYHSWx}Army compensation rights"));
                    }
                }
            }

            var positions = BannerKingsConfig.Instance.CourtManager.GetHeroPositions(clan.Leader);
            if (positions != null)
            {
                foreach (CouncilMember position in positions)
                {
                    result.Add(position.DueWage, new TextObject("{=WvhXhUFS}Councillor role"));
                }
            }
        }

        public void AddExpenses(Clan clan, ref ExplainedNumber result, bool applyWithdrawals)
        {
            var totalWorkshopExpenses = 0;

            var wkModel = (BKWorkshopModel)TaleWorlds.CampaignSystem.Campaign.Current.Models.WorkshopModel;
            foreach (var wk in clan.Leader.OwnedWorkshops)
            {
                if (wk.Settlement.OwnerClan == clan)
                {
                    continue;
                }

                totalWorkshopExpenses += GetWorkshopTaxes(wk);
            }

            if (totalWorkshopExpenses > 0)
            {
                result.Add(-totalWorkshopExpenses, new TextObject("{=6CKOfX2U}Workshop taxes"));
            }

            var data = BannerKingsConfig.Instance.CourtManager.GetCouncil(clan);
            if (data != null)
            {
                var taxes = 0f;
                foreach (var position in data.GetOccupiedPositions())
                {
                    taxes -= position.DueWage;
                    if (applyWithdrawals && !position.Member.IsLord)
                    {
                        position.Member.Gold += position.DueWage;
                    }
                }

                result.Add(taxes, new TextObject("{=L0Dwod0e}Council wages"));
            }


            var suzerain = BannerKingsConfig.Instance.TitleManager.CalculateHeroSuzerain(clan.Leader);
            if (suzerain == null)
            {
                return;
            }

            Kingdom deJureKingdom = null;
            if (suzerain.deJure.Clan != null)
            {
                deJureKingdom = suzerain.deJure.Clan.Kingdom;
            }

            if (deJureKingdom == null || deJureKingdom != clan.Kingdom)
            {
                return;
            }

            var dictionary = BannerKingsConfig.Instance.TitleManager.CalculateVassals(suzerain.deJure.Clan, clan);
            if (dictionary.ContainsKey(clan))
            {
                var amount = dictionary[clan].Aggregate(0f, (current, title) => current + (int)title.DueTax);
                result.Add(-amount, new TextObject("{=rU692V1m}Taxes to {SUZERAIN}").SetTextVariable("SUZERAIN", suzerain.deJure.Name));

            }
        }
    }
}