using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityModManagerNet;
using SolastaModApi;
using SolastaModApi.Extensions;
using ModKit;
using ModKit.Utility;
using SolastaModHelpers;
using SolastaModHelpers.Helpers;
using Helpers = SolastaModHelpers.Helpers;


namespace SolastaCraftyFeats
{
    public static class Main
    {
        public static readonly string MOD_FOLDER = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Conditional("DEBUG")]
        internal static void Log(string msg) => Logger.Log(msg);
        internal static void Error(Exception ex) => Logger?.Error(ex.ToString());
        internal static void Error(string msg) => Logger?.Error(msg);
        internal static void Warning(string msg) => Logger?.Warning(msg);
        internal static UnityModManager.ModEntry.ModLogger Logger { get; private set; }
        internal static ModManager<Core, Settings> Mod { get; private set; }
        internal static MenuManager Menu { get; private set; }
        internal static Settings Settings { get { return Mod.Settings; } }

        internal static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Logger = modEntry.Logger;

                Mod = new ModManager<Core, Settings>();
                Menu = new MenuManager();
                modEntry.OnToggle = OnToggle;

                Translations.Load(MOD_FOLDER);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool enabled)
        {
            if (enabled)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Mod.Enable(modEntry, assembly);
                Menu.Enable(modEntry, assembly);
            }
            else
            {
                Menu.Disable(modEntry);
                Mod.Disable(modEntry, false);
                ReflectionCache.Clear();
            }
            return true;
        }
        public class CraftyFeatBuilder : BaseDefinitionBuilder<FeatDefinition>
        {
            protected CraftyFeatBuilder(string name, string guid, string title_string, string description_string, FeatDefinition base_Feat) : base(base_Feat, name, guid)
            {
                if (title_string != "")
                {
                    Definition.GuiPresentation.Title = title_string;
                }
                if (description_string != "")
                {
                    Definition.GuiPresentation.Description = description_string;
                }
            }
            public static FeatDefinition CreateCopyFrom(string name, string guid, string new_title_string, string new_description_string, FeatDefinition base_Feat)
            {
                return new CraftyFeatBuilder(name, guid, new_title_string, new_description_string, base_Feat).AddToDB();

            }
        }

        internal static void OnGameReady()
        {
            FeatureDefinitionAttributeModifier crafty_int = SolastaModHelpers.Helpers.CopyFeatureBuilder<FeatureDefinitionAttributeModifier>.createFeatureCopy
                ("AttributeModifierFeatCraftyInt", "b23c3b73-7690-42ba-aa49-7ca3451daa05",
                "SolastaCraftyFeats/&AttributeIntTitle", "SolastaCraftyFeats/&AttributeIntDescription",
                DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierCreed_Of_Pakri.GuiPresentation.SpriteReference,
                DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierCreed_Of_Pakri);

            FeatureDefinitionAttributeModifier crafty_wis = SolastaModHelpers.Helpers.CopyFeatureBuilder<FeatureDefinitionAttributeModifier>.createFeatureCopy
                ("AttributeModifierFeatCraftyWis", "23f944c7-2359-43cc-8bdc-71833bf35302",
                "SolastaCraftyFeats/&AttributeWisTitle", "SolastaCraftyFeats/&AttributeWisDescription",
                DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierCreed_Of_Maraike.GuiPresentation.SpriteReference,
                DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierCreed_Of_Maraike);

            FeatureDefinitionAttributeModifier crafty_dex = SolastaModHelpers.Helpers.CopyFeatureBuilder<FeatureDefinitionAttributeModifier>.createFeatureCopy
                ("AttributeModifierFeatCraftyDex", "4db12466-67da-47a4-8d96-a9bf9cf3a251",
                "SolastaCraftyFeats/&AttributeDexTitle", "SolastaCraftyFeats/&AttributeDexDescription",
                DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierCreed_Of_Misaye.GuiPresentation.SpriteReference,
                DatabaseHelper.FeatureDefinitionAttributeModifiers.AttributeModifierCreed_Of_Misaye);

            FeatureDefinitionProficiency crafty_arcana = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateSkillsProficiency
                ("CraftyArcana", "44a54666-80ba-475c-90b1-774e86f1a69a",
                "SolastaCraftyFeats/&CraftySkillsTitle", "SolastaCraftyFeats/&CraftyArcanaDescription",
                Helpers.Skills.Arcana);
            crafty_arcana.SetProficiencyType(RuleDefinitions.ProficiencyType.SkillOrExpertise);

            FeatureDefinitionProficiency crafty_medicine = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateSkillsProficiency
                ("CraftyMedicine", "1ac54869-a8ce-4a51-a858-1f7e34680b96",
                "SolastaCraftyFeats/&CraftySkillsTitle", "SolastaCraftyFeats/&CraftyMedicineDescription",
                Helpers.Skills.Medicine);
            crafty_medicine.SetProficiencyType(RuleDefinitions.ProficiencyType.SkillOrExpertise);

            FeatureDefinitionProficiency crafty_nature = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateSkillsProficiency
                ("CraftyNature", "7399b06a-bfda-4e60-8366-17e0d6cec0d0",
                "SolastaCraftyFeats/&CraftySkillsTitle", "SolastaCraftyFeats/&CraftyNatureDescription",
                Helpers.Skills.Nature);
            crafty_nature.SetProficiencyType(RuleDefinitions.ProficiencyType.SkillOrExpertise);

            FeatureDefinitionProficiency crafty_herbalism_kit = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateToolsProficiency
                ("CraftyHerbalismKit", "9345e1fd-ec4c-4509-acb5-3f3257b25ec4",
                "SolastaCraftyFeats/&CraftyToolsTitle", 
                Helpers.Tools.HerbalismKit);
            crafty_herbalism_kit.SetProficiencyType(RuleDefinitions.ProficiencyType.ToolOrExpertise);

            FeatureDefinitionProficiency crafty_manacalon_rosary = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateToolsProficiency
                ("CraftyManacalonRosary", "0685a944-76cd-423a-81a1-9ceec507d69a",
                "SolastaCraftyFeats/&CraftyToolsTitle",
                Helpers.Tools.EnchantingTool);
            crafty_manacalon_rosary.SetProficiencyType(RuleDefinitions.ProficiencyType.ToolOrExpertise);

            FeatureDefinitionProficiency crafty_poisoners_kit = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateToolsProficiency
                ("CraftyPoisonersKit", "32ddae84-66e7-4b56-b5ec-0ec91a713e2e",
                "SolastaCraftyFeats/&CraftyToolsTitle",
                Helpers.Tools.PoisonerKit);
            crafty_poisoners_kit.SetProficiencyType(RuleDefinitions.ProficiencyType.ToolOrExpertise);

            FeatureDefinitionProficiency crafty_scroll_kit = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateToolsProficiency
                ("CraftyScrollKit", "5309bd7f-b533-40ff-ae95-d977e02d61fe",
                "SolastaCraftyFeats/&CraftyToolsTitle",
                Helpers.Tools.ScrollKit);
            crafty_scroll_kit.SetProficiencyType(RuleDefinitions.ProficiencyType.ToolOrExpertise);

            FeatureDefinitionProficiency crafty_smiths_tools = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateToolsProficiency
                ("CraftySmithsTools", "48905450-4b35-480f-9868-f340c7902920",
                "SolastaCraftyFeats/&CraftyToolsTitle",
                Helpers.Tools.SmithTool);
            crafty_smiths_tools.SetProficiencyType(RuleDefinitions.ProficiencyType.ToolOrExpertise);

            FeatureDefinitionProficiency crafty_bows = SolastaModHelpers.Helpers.ProficiencyBuilder.CreateWeaponProficiency
                ("CraftyBows", "62a71277-b62d-41e6-9546-19f6faa2b5a7",
                "SolastaCraftyFeats/&CraftyBowsTitle", "SolastaCraftyFeats/&CraftyBowsDescription",
                Helpers.WeaponProficiencies.Shortbow, Helpers.WeaponProficiencies.Longbow,
                Helpers.WeaponProficiencies.LightCrossbow, Helpers.WeaponProficiencies.HeavyCrossbow);

            FeatDefinition ApothecaryIntFeat = CraftyFeatBuilder.CreateCopyFrom
                ("ApothecaryInt", "ef387249-45e0-4899-aadd-44810f8aeb6d",
                "SolastaCraftyFeats/&ApothecaryIntFeatTitle", "SolastaCraftyFeats/&ApothecaryIntFeatDescription",
                DatabaseHelper.FeatDefinitions.ArmorMaster);
            ApothecaryIntFeat.Features.Clear();
            ApothecaryIntFeat.Features.Add(crafty_int);
            ApothecaryIntFeat.Features.Add(crafty_herbalism_kit);
            ApothecaryIntFeat.Features.Add(crafty_arcana);

            FeatDefinition ApothecaryWisFeat = CraftyFeatBuilder.CreateCopyFrom
                ("ApothecaryWis", "4fd80bf9-7749-4c01-9d95-6eb56c644fe2",
                "SolastaCraftyFeats/&ApothecaryWisFeatTitle", "SolastaCraftyFeats/&ApothecaryWisFeatDescription",
                DatabaseHelper.FeatDefinitions.ArmorMaster);
            ApothecaryWisFeat.Features.Clear();
            ApothecaryWisFeat.Features.Add(crafty_wis);
            ApothecaryWisFeat.Features.Add(crafty_herbalism_kit);
            ApothecaryWisFeat.Features.Add(crafty_medicine);

            FeatDefinition ManacalonCrafter = CraftyFeatBuilder.CreateCopyFrom
                ("ManacalonCrafter", "290f73c8-201c-489e-bdcb-7a39ab40915c",
                "SolastaCraftyFeats/&ManacalonCrafterFeatTitle", "SolastaCraftyFeats/&ManacalonCrafterFeatDescription",
                DatabaseHelper.FeatDefinitions.MasterEnchanter);
            ManacalonCrafter.Features.Clear();
            ManacalonCrafter.Features.Add(crafty_int);
            ManacalonCrafter.Features.Add(crafty_manacalon_rosary);
            ManacalonCrafter.Features.Add(crafty_arcana);

            FeatDefinition ToxicologistInt = CraftyFeatBuilder.CreateCopyFrom
                ("ToxicologistInt", "702d1b4d-953c-406d-a900-d5d376ed29d3",
                "SolastaCraftyFeats/&ToxicologistIntFeatTitle", "SolastaCraftyFeats/&ToxicologistIntFeatDescription",
                DatabaseHelper.FeatDefinitions.ArmorMaster);
            ToxicologistInt.Features.Clear();
            ToxicologistInt.Features.Add(crafty_int);
            ToxicologistInt.Features.Add(crafty_poisoners_kit);
            ToxicologistInt.Features.Add(crafty_nature);

            FeatDefinition ToxicologistWis = CraftyFeatBuilder.CreateCopyFrom
                ("ToxicologistWis", "1bb4acbd-1890-48ae-9f86-46c2cb95cb79",
                "SolastaCraftyFeats/&ToxicologistWisFeatTitle", "SolastaCraftyFeats/&ToxicologistWisFeatDescription",
                DatabaseHelper.FeatDefinitions.ArmorMaster);
            ToxicologistWis.Features.Clear();
            ToxicologistWis.Features.Add(crafty_wis);
            ToxicologistWis.Features.Add(crafty_poisoners_kit);
            ToxicologistWis.Features.Add(crafty_medicine);

            FeatDefinition CraftyScribe = CraftyFeatBuilder.CreateCopyFrom
                ("CraftyScribe", "bd83e063-2751-4898-8070-f74ca925f8b5",
                "SolastaCraftyFeats/&CraftyScribeFeatTitle", "SolastaCraftyFeats/&CraftyScribeFeatDescription",
                DatabaseHelper.FeatDefinitions.MasterEnchanter);
            CraftyScribe.Features.Clear();
            CraftyScribe.Features.Add(crafty_int);
            CraftyScribe.Features.Add(crafty_scroll_kit);
            CraftyScribe.Features.Add(crafty_arcana);

            FeatDefinition CraftyFletcher = CraftyFeatBuilder.CreateCopyFrom
                ("CraftyFletcher", "67c5f2d2-a98c-49a1-a1ab-16cc8f4b4ba4",
                "SolastaCraftyFeats/&CraftyFletcherFeatTitle", "SolastaCraftyFeats/&CraftyFletcherFeatDescription",
                DatabaseHelper.FeatDefinitions.ArmorMaster);
            CraftyFletcher.Features.Clear();
            CraftyFletcher.Features.Add(crafty_dex);
            CraftyFletcher.Features.Add(crafty_smiths_tools);
            CraftyFletcher.Features.Add(crafty_bows);

        }
    }
}
