﻿using System.Collections.Generic;
using BannerKings.Managers.Institutions.Religions.Doctrines;
using BannerKings.Managers.Institutions.Religions.Faiths.Rites;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using static BannerKings.Behaviours.Feasts.Feast;

namespace BannerKings.Managers.Institutions.Religions.Faiths
{
    public abstract class MonotheisticFaith : Faith
    {
        public void Initialize(Divinity mainGod, List<Divinity> pantheon, Dictionary<TraitObject, bool> traits,
            List<Doctrine> doctrines, 
            FaithGroup faithGroup, 
            List<Rite> rites = null, 
            FeastType feastType = FeastType.None)
        {
            Initialize(mainGod, traits, faithGroup, doctrines, rites);
            this.pantheon = pantheon;
        }
    }
}