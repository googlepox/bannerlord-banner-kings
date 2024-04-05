﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace BannerKings.Behaviours.Mercenary
{
    internal class CustomTroop
    {
        internal CustomTroop(CharacterObject character)
        {
            Character = character;
            CreateEquipments();
        }

        public void PostInitialize(CultureObject culture)
        {
            if (Name == null)
            {
                Name = new TextObject("{=gaJDVkvHA}Placeholder").ToString();
            }

            FillCharacter(culture.BasicTroop);
            SetName(new TextObject(Name));       
            SetEquipment(Character);
            SetSkills(Character, Skills);
        }

        private void FillCharacter(CharacterObject reference)
        {
            MBObjectManager.Instance.RegisterObject(Character);
            Character.Culture = reference.Culture;

            Character.Age = reference.Age;
            AccessTools.Method(reference.GetType(), "InitializeHeroBasicCharacterOnAfterLoad")
                .Invoke(Character, new object[] { (reference as BasicCharacterObject) });

            BasicCharacterObject basicCharacter = (BasicCharacterObject)Character;
            basicCharacter.Level = reference.Level;
            AccessTools.Field(reference.GetType(), "_occupation").SetValue(Character, Occupation.Mercenary);

            AccessTools.Property(reference.GetType(), "UpgradeTargets").SetValue(Character, new CharacterObject[0]);
        }

        public void SetEquipment(CharacterObject characterl)
        {
            var newRoster = new MBEquipmentRoster();
            AccessTools.Field(newRoster.GetType(), "_equipments").SetValue(newRoster, Equipments);
            AccessTools.Field((Character as BasicCharacterObject).GetType(), "_equipmentRoster")
                .SetValue((Character as BasicCharacterObject), newRoster);
        }

        public void SetName(TextObject text)
        {
            AccessTools.Method((Character as BasicCharacterObject).GetType(), "SetName")
                            .Invoke(Character, new object[] { text });
            Name = text.ToString();
        }

        public void SetSkills(CharacterObject character, CustomTroopPreset preset)
        {
            if (preset != null)
            {
                preset.PostInitialize();
                BasicCharacterObject basicCharacter = character;
                basicCharacter.Level = preset.Level;

                MBCharacterSkills skills = new MBCharacterSkills();
                skills.Skills.SetPropertyValue(DefaultSkills.OneHanded, preset.OneHanded);
                skills.Skills.SetPropertyValue(DefaultSkills.TwoHanded, preset.TwoHanded);
                skills.Skills.SetPropertyValue(DefaultSkills.Polearm, preset.Polearm);
                skills.Skills.SetPropertyValue(DefaultSkills.Riding, preset.Riding);
                skills.Skills.SetPropertyValue(DefaultSkills.Athletics, preset.Athletics);
                skills.Skills.SetPropertyValue(DefaultSkills.Bow, preset.Bow);
                skills.Skills.SetPropertyValue(DefaultSkills.Crossbow, preset.Crossbow);
                skills.Skills.SetPropertyValue(DefaultSkills.Throwing, preset.Throwing);

                AccessTools.Field((character as BasicCharacterObject).GetType(), "DefaultCharacterSkills")
                        .SetValue(character, skills);
                character.DefaultFormationGroup = FetchDefaultFormationGroup(preset.Formation.ToString());
                AccessTools.Property((character as BasicCharacterObject).GetType(), "DefaultFormationClass")
                        .SetValue(character, preset.Formation); 

                Skills = preset;
            }
        }

        protected int FetchDefaultFormationGroup(string innerText)
        {
            FormationClass result;
            if (Enum.TryParse<FormationClass>(innerText, true, out result))
            {
                return (int)result;
            }
            return -1;
        }

        public void CreateEquipments()
        {
            var list = new MBList<Equipment>();
            list.Add(new Equipment());
            list.Add(new Equipment());
            list.Add(new Equipment());
            list.Add(new Equipment());
            list.Add(new Equipment());

            var newRoster = new MBEquipmentRoster();
            AccessTools.Field(newRoster.GetType(), "_equipments").SetValue(newRoster, list);
            AccessTools.Field((Character as BasicCharacterObject).GetType(), "_equipmentRoster")
                .SetValue((Character as BasicCharacterObject), newRoster);

            Equipments = list;
        }

        public void FeedEquipments(List<ItemObject> items, EquipmentIndex index)
        {
            for (int i = 0; i < Equipments.Count; i++)
            {
                var equipment = Equipments[i];
                ItemObject item = null;
                if (items.Count > i)
                {
                    item = items[i];
                }
                else
                {
                    item = items.GetRandomElement();
                }

                EquipmentElement equipmentElement = new EquipmentElement(item);
                equipment[index] = equipmentElement;
            }
        }

        [SaveableProperty(1)] public CharacterObject Character { get; set; }
        [SaveableProperty(2)] public string Name { get; set; }
        [SaveableProperty(3)] public List<Equipment> Equipments { get; set; }
        [SaveableProperty(4)] public CustomTroopPreset Skills { get; set; }
    }
}