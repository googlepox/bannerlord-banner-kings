﻿using System.Collections.Generic;
using System.Linq;
using BannerKings.Managers.Innovations;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace BannerKings.Managers
{
    public class InnovationsManager
    {
        public InnovationsManager()
        {
            Innovations = new Dictionary<CultureObject, InnovationData>();
        }

        [SaveableProperty(1)] private Dictionary<CultureObject, InnovationData> Innovations { get; set; }

        private void InitializeInnovations()
        {
            foreach (var culture in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>().Where(culture => !culture.IsBandit && culture.CanHaveSettlement))
            {
                if (!Innovations.ContainsKey(culture))
                {
                    InnovationData data = new InnovationData(new List<Innovation>(), culture);
                    Innovations.Add(culture, data);
                    foreach (var innovation in DefaultInnovations.Instance.All)
                        data.AddInnovation(innovation.GetCopy(culture));
                    data.Update();
                }
            }
        }

        public void PostInitialize()
        {
            InitializeInnovations();
            foreach (var data in Innovations.Values)
            {
                foreach (var innovation in DefaultInnovations.Instance.All)
                {
                    data.AddInnovation(innovation);
                }

                data.PostInitialize();
            }

            foreach (var culture in TaleWorlds.CampaignSystem.Campaign.Current.ObjectManager.GetObjectTypeList<CultureObject>())
            {
                InnovationData data = BannerKingsConfig.Instance.InnovationsManager.GetInnovationData(culture);
                if (data == null) continue;

                data.Era.TriggerEra(culture);
            }
        }

        public void UpdateInnovations()
        {
            foreach (var data in Innovations.Values)
            {
                data.Update();
            }
        }

        public InnovationData GetInnovationData(CultureObject culture)
        {
            if (culture == null) return null;

            InnovationData data = null;
            if (Innovations.ContainsKey(culture))
            {
                data = Innovations[culture];
            }

            return data;
        }
    }
}