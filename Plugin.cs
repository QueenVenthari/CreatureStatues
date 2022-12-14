using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CreatureManager;
using HarmonyLib;
using ItemManager;
using LocationManager;
using PieceManager;
using ServerSync;
using SkillManager;
using StatusEffectManager;
using UnityEngine;
using PrefabManager = ItemManager.PrefabManager;
using Range = LocationManager.Range;

namespace CreatureStatues
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class CreatureStatuesPlugin : BaseUnityPlugin
    {
        internal const string ModName = "CreatureStatues";
        internal const string ModVersion = "0.2.7";
        internal const string Author = "Venthari";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource CreatureStatuesLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        // Location Manager variables
        public Texture2D tex;
        private Sprite mySprite;
        private SpriteRenderer sr;

        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
                "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);


            #region PieceManager Example Code

            // Globally turn off configuration options for your pieces, omit if you don't want to do this.
            BuildPiece.ConfigurationEnabled = false;

            // Format: new("AssetBundleName", "PrefabName", "FolderName");
            
            BuildPiece Stone_Piggy_Statue = new("creaturestatues", "Stone_Piggy_Statue", "assets");

            Stone_Piggy_Statue.Name.English("Stone Piggy Statue");
            Stone_Piggy_Statue.Description.English("A Stone Statue of a Piggy.");
            Stone_Piggy_Statue.RequiredItems.Add("Stone", 10, true);
            Stone_Piggy_Statue.RequiredItems.Add("TrophyBoar", 1, true);
            Stone_Piggy_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Piggy_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Piggy_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Boar_Statue = new("creaturestatues", "Stone_Boar_Statue", "assets");

            Stone_Boar_Statue.Name.English("Stone Boar Statue");
            Stone_Boar_Statue.Description.English("A Stone Statue of a Boar.");
            Stone_Boar_Statue.RequiredItems.Add("Stone", 15, true);
            Stone_Boar_Statue.RequiredItems.Add("TrophyBoar", 1, true);
            Stone_Boar_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Boar_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Boar_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };

            BuildPiece Stone_Neck_Statue = new("creaturestatues", "Stone_Neck_Statue", "assets");

            Stone_Neck_Statue.Name.English("Stone Neck Statue");
            Stone_Neck_Statue.Description.English("A Stone Statue of a Neck.");
            Stone_Neck_Statue.RequiredItems.Add("Stone", 15, true);
            Stone_Neck_Statue.RequiredItems.Add("TrophyNeck", 1, true);
            Stone_Neck_Statue.RequiredItems.Add("GreydwarfEye", 4, true);
            Stone_Neck_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Neck_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Neck_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Doe_Statue = new("creaturestatues", "Stone_Doe_Statue", "assets");

            Stone_Doe_Statue.Name.English("Stone Doe Statue");
            Stone_Doe_Statue.Description.English("A Stone Statue of a Doe.");
            Stone_Doe_Statue.RequiredItems.Add("Stone", 25, true);
            Stone_Doe_Statue.RequiredItems.Add("TrophyDeer", 1, true);
            Stone_Doe_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Doe_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Doe_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };

            BuildPiece Stone_Stag_Statue = new("creaturestatues", "Stone_Stag_Statue", "assets");

            Stone_Stag_Statue.Name.English("Stone Stag Statue");
            Stone_Stag_Statue.Description.English("A Stone Statue of a Stag.");
            Stone_Stag_Statue.RequiredItems.Add("Stone", 35, true);
            Stone_Stag_Statue.RequiredItems.Add("TrophyDeer", 1, true);
            Stone_Stag_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Stag_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Stag_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Bird_Sitting_Statue = new("creaturestatues", "Stone_Bird_Sitting_Statue", "assets");

            Stone_Bird_Sitting_Statue.Name.English("Stone Bird (Sitting)");
            Stone_Bird_Sitting_Statue.Description.English("A Stone Statue of a Bird Sitting.");
            Stone_Bird_Sitting_Statue.RequiredItems.Add("Stone", 15, true);
            Stone_Bird_Sitting_Statue.RequiredItems.Add("Feathers", 1, true);
            Stone_Bird_Sitting_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Bird_Sitting_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Bird_Sitting_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Bird_Flying_Statue = new("creaturestatues", "Stone_Bird_Flying_Statue", "assets");

            Stone_Bird_Flying_Statue.Name.English("Stone Bird (Flying)");
            Stone_Bird_Flying_Statue.Description.English("A Stone Statue of a Bird Flying.");
            Stone_Bird_Flying_Statue.RequiredItems.Add("Stone", 15, true);
            Stone_Bird_Flying_Statue.RequiredItems.Add("Feathers", 1, true);
            Stone_Bird_Flying_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Bird_Flying_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Bird_Flying_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Greyling_Statue = new("creaturestatues", "Stone_Greyling_Statue", "assets");

            Stone_Greyling_Statue.Name.English("Stone Greyling Statue");
            Stone_Greyling_Statue.Description.English("A Stone Statue of a Greyling.");
            Stone_Greyling_Statue.RequiredItems.Add("Stone", 25, true);
            Stone_Greyling_Statue.RequiredItems.Add("Resin", 3, true);
            Stone_Greyling_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Greyling_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Greyling_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Greyling_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Greydwarf_Statue = new("creaturestatues", "Stone_Greydwarf_Statue", "assets");

            Stone_Greydwarf_Statue.Name.English("Stone Greydwarf Statue");
            Stone_Greydwarf_Statue.Description.English("A Stone Statue of a Greydwarf.");
            Stone_Greydwarf_Statue.RequiredItems.Add("Stone", 30, true);
            Stone_Greydwarf_Statue.RequiredItems.Add("TrophyGreydwarf", 1, true);
            Stone_Greydwarf_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Greydwarf_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Greydwarf_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Greydwarf_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Greydwarf_Shaman_Statue = new("creaturestatues", "Stone_Greydwarf_Shaman_Statue", "assets");

            Stone_Greydwarf_Shaman_Statue.Name.English("Stone Greydwarf Shaman Statue");
            Stone_Greydwarf_Shaman_Statue.Description.English("A Stone Statue of a Greydwarf Shaman.");
            Stone_Greydwarf_Shaman_Statue.RequiredItems.Add("Stone", 35, true);
            Stone_Greydwarf_Shaman_Statue.RequiredItems.Add("TrophyGreydwarfShaman", 1, true);
            Stone_Greydwarf_Shaman_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Greydwarf_Shaman_Statue.RequiredItems.Add("Thistle", 2, true);
            Stone_Greydwarf_Shaman_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Greydwarf_Shaman_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Greydwarf_Shaman_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };

            BuildPiece Stone_Greydwarf_Brute_Statue = new("creaturestatues", "Stone_Greydwarf_Brute_Statue", "assets");

            Stone_Greydwarf_Brute_Statue.Name.English("Stone Greydwarf Brute Statue");
            Stone_Greydwarf_Brute_Statue.Description.English("A Stone Statue of a Greydwarf Brute.");
            Stone_Greydwarf_Brute_Statue.RequiredItems.Add("Stone", 40, true);
            Stone_Greydwarf_Brute_Statue.RequiredItems.Add("TrophyGreydwarfBrute", 1, true);
            Stone_Greydwarf_Brute_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Greydwarf_Brute_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_Greydwarf_Brute_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Greydwarf_Brute_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Greydwarf_Brute_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Skeleton_Statue = new("creaturestatues", "Stone_Skeleton_Statue", "assets");

            Stone_Skeleton_Statue.Name.English("Stone Skeleton");
            Stone_Skeleton_Statue.Description.English("A Stone Statue of a Skeleton.");
            Stone_Skeleton_Statue.RequiredItems.Add("Stone", 20, true);
            Stone_Skeleton_Statue.RequiredItems.Add("TrophySkeleton", 1, true);
            Stone_Skeleton_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Skeleton_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Skeleton_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Skeleton_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Skeleton_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Troll_Small_Statue = new("creaturestatues", "Stone_Troll_Small_Statue", "assets");

            Stone_Troll_Small_Statue.Name.English("Stone Troll (Small)");
            Stone_Troll_Small_Statue.Description.English("A Stone Statue of a Small Troll.");
            Stone_Troll_Small_Statue.RequiredItems.Add("Stone", 100, true);
            Stone_Troll_Small_Statue.RequiredItems.Add("TrophyFrostTroll", 1, true);
            Stone_Troll_Small_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Troll_Small_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Troll_Small_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Troll_Small_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Troll_Small_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Troll_Large_Statue = new("creaturestatues", "Stone_Troll_Large_Statue", "assets");

            Stone_Troll_Large_Statue.Name.English("Stone Troll (Large)");
            Stone_Troll_Large_Statue.Description.English("A Stone Statue of a Large Troll.");
            Stone_Troll_Large_Statue.RequiredItems.Add("Stone", 200, true);
            Stone_Troll_Large_Statue.RequiredItems.Add("TrophyFrostTroll", 1, true);
            Stone_Troll_Large_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Troll_Large_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Troll_Large_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Troll_Large_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Troll_Large_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Draugr_Statue = new("creaturestatues", "Stone_Draugr_Statue", "assets");

            Stone_Draugr_Statue.Name.English("Stone Draugr");
            Stone_Draugr_Statue.Description.English("A Stone Statue of a Draugr.");
            Stone_Draugr_Statue.RequiredItems.Add("Stone", 30, true);
            Stone_Draugr_Statue.RequiredItems.Add("TrophyDraugr", 1, true);
            Stone_Draugr_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Draugr_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Draugr_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Draugr_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Draugr_Elite_Statue = new("creaturestatues", "Stone_Draugr_Elite_Statue", "assets");

            Stone_Draugr_Elite_Statue.Name.English("Stone Draugr Elite");
            Stone_Draugr_Elite_Statue.Description.English("A Stone Statue of a Draugr Elite.");
            Stone_Draugr_Elite_Statue.RequiredItems.Add("Stone", 40, true);
            Stone_Draugr_Elite_Statue.RequiredItems.Add("TrophyDraugrElite", 1, true);
            Stone_Draugr_Elite_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Draugr_Elite_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_Draugr_Elite_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Draugr_Elite_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Draugr_Elite_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Draugr_Hag_Statue = new("creaturestatues", "Stone_Draugr_Hag_Statue", "assets");

            Stone_Draugr_Hag_Statue.Name.English("Stone Draugr Hag");
            Stone_Draugr_Hag_Statue.Description.English("A Stone Statue of a Draugr Hag.");
            Stone_Draugr_Hag_Statue.RequiredItems.Add("Stone", 30, true);
            Stone_Draugr_Hag_Statue.RequiredItems.Add("TrophyDraugrFem", 1, true);
            Stone_Draugr_Hag_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Draugr_Hag_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Draugr_Hag_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Draugr_Hag_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Blob_Statue = new("creaturestatues", "Stone_Blob_Statue", "assets");

            Stone_Blob_Statue.Name.English("Stone Blob");
            Stone_Blob_Statue.Description.English("A Stone Statue of a Blob.");
            Stone_Blob_Statue.RequiredItems.Add("Stone", 50, true);
            Stone_Blob_Statue.RequiredItems.Add("TrophyBlob", 1, true);
            Stone_Blob_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Blob_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Blob_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Leech_Statue = new("creaturestatues", "Stone_Leech_Statue", "assets");

            Stone_Leech_Statue.Name.English("Stone Leech");
            Stone_Leech_Statue.Description.English("A Stone Statue of a Leech.");
            Stone_Leech_Statue.RequiredItems.Add("Stone", 50, true);
            Stone_Leech_Statue.RequiredItems.Add("TrophyLeech", 1, true);
            Stone_Leech_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Leech_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Leech_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Wraith_Statue = new("creaturestatues", "Stone_Wraith_Statue", "assets");

            Stone_Wraith_Statue.Name.English("Stone Wraith");
            Stone_Wraith_Statue.Description.English("A Stone Statue of a Wraith.");
            Stone_Wraith_Statue.RequiredItems.Add("Stone", 75, true);
            Stone_Wraith_Statue.RequiredItems.Add("TrophyWraith", 1, true);
            Stone_Wraith_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Wraith_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Wraith_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Abomination_Small_Statue = new("creaturestatues", "Stone_Abomination_Small_Statue", "assets");

            Stone_Abomination_Small_Statue.Name.English("Stone Abomination (Small)");
            Stone_Abomination_Small_Statue.Description.English("A Stone Statue of a Small Abomination.");
            Stone_Abomination_Small_Statue.RequiredItems.Add("Stone", 100, true);
            Stone_Abomination_Small_Statue.RequiredItems.Add("TrophyAbomination", 1, true);
            Stone_Abomination_Small_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Abomination_Small_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Abomination_Small_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Abomination_Large_Statue = new("creaturestatues", "Stone_Abomination_Large_Statue", "assets");

            Stone_Abomination_Large_Statue.Name.English("Stone Abomination (Large)");
            Stone_Abomination_Large_Statue.Description.English("A Stone Statue of a Large Abomination.");
            Stone_Abomination_Large_Statue.RequiredItems.Add("Stone", 500, true);
            Stone_Abomination_Large_Statue.RequiredItems.Add("TrophyAbomination", 1, true);
            Stone_Abomination_Large_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Abomination_Large_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Abomination_Large_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Wolf_Pup_Statue = new("creaturestatues", "Stone_Wolf_Pup_Statue", "assets");

            Stone_Wolf_Pup_Statue.Name.English("Stone Wolf Pup");
            Stone_Wolf_Pup_Statue.Description.English("A Stone Statue of a Wolf Pup.");
            Stone_Wolf_Pup_Statue.RequiredItems.Add("Stone", 25, true);
            Stone_Wolf_Pup_Statue.RequiredItems.Add("TrophyWolf", 1, true);
            Stone_Wolf_Pup_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Wolf_Pup_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Wolf_Pup_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Wolf_Statue = new("creaturestatues", "Stone_Wolf_Statue", "assets");

            Stone_Wolf_Statue.Name
                .English("Stone Wolf");
            Stone_Wolf_Statue.Description.English("A Stone Statue of a Wolf.");
            Stone_Wolf_Statue.RequiredItems.Add("Stone", 50, true);
            Stone_Wolf_Statue.RequiredItems.Add("TrophyWolf", 1, true);
            Stone_Wolf_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Wolf_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Wolf_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Bat_Statue = new("creaturestatues", "Stone_Bat_Statue", "assets");

            Stone_Bat_Statue.Name.English("Stone Bat");
            Stone_Bat_Statue.Description.English("A Stone Statue of a Bat.");
            Stone_Bat_Statue.RequiredItems.Add("Stone", 25, true);
            Stone_Bat_Statue.RequiredItems.Add("LeatherScraps", 1, true);
            Stone_Bat_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Bat_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Bat_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Drake_Statue = new("creaturestatues", "Stone_Drake_Statue", "assets");

            Stone_Drake_Statue.Name.English("Stone Drake");
            Stone_Drake_Statue.Description.English("A Stone Statue of a Drake.");
            Stone_Drake_Statue.RequiredItems.Add("Stone", 100, true);
            Stone_Drake_Statue.RequiredItems.Add("TrophyHatchling", 1, true);
            Stone_Drake_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Drake_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_Drake_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Drake_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Drake_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Ulv_Statue = new("creaturestatues", "Stone_Ulv_Statue", "assets");

            Stone_Ulv_Statue.Name.English("Stone Ulv");
            Stone_Ulv_Statue.Description.English("A Stone Statue of an Ulv.");
            Stone_Ulv_Statue.RequiredItems.Add("Stone", 125, true);
            Stone_Ulv_Statue.RequiredItems.Add("TrophyUlv", 1, true);
            Stone_Ulv_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Ulv_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Ulv_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Ulv_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Ulv_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Fenring_Statue = new("creaturestatues", "Stone_Fenring_Statue", "assets");

            Stone_Fenring_Statue.Name.English("Stone Fenring");
            Stone_Fenring_Statue.Description.English("A Stone Statue of a Fenring.");
            Stone_Fenring_Statue.RequiredItems.Add("Stone", 125, true);
            Stone_Fenring_Statue.RequiredItems.Add("TrophyFenring", 1, true);
            Stone_Fenring_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Fenring_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Fenring_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Fenring_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Fenring_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Cultist_Statue = new("creaturestatues", "Stone_Cultist_Statue", "assets");

            Stone_Cultist_Statue.Name.English("Stone Cultist");
            Stone_Cultist_Statue.Description.English("A Stone Statue of a Cultist.");
            Stone_Cultist_Statue.RequiredItems.Add("Stone", 125, true);
            Stone_Cultist_Statue.RequiredItems.Add("TrophyCultist", 1, true);
            Stone_Cultist_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Cultist_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_Cultist_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Cultist_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Cultist_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Golem_Small_Statue = new("creaturestatues", "Stone_Golem_Small_Statue", "assets");

            Stone_Golem_Small_Statue.Name.English("Stone Golem (Small)");
            Stone_Golem_Small_Statue.Description.English("A Stone Statue of a Small Golem.");
            Stone_Golem_Small_Statue.RequiredItems.Add("Stone", 150, true);
            Stone_Golem_Small_Statue.RequiredItems.Add("TrophySGolem", 1, true);
            Stone_Golem_Small_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Golem_Small_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Golem_Small_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Golem_Large_Statue = new("creaturestatues", "Stone_Golem_Large_Statue", "assets");

            Stone_Golem_Large_Statue.Name.English("Stone Golem (Large)");
            Stone_Golem_Large_Statue.Description.English("A Stone Statue of a Large Golem.");
            Stone_Golem_Large_Statue.RequiredItems.Add("Stone", 300, true);
            Stone_Golem_Large_Statue.RequiredItems.Add("TrophySGolem", 1, true);
            Stone_Golem_Large_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Golem_Large_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Golem_Large_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Deathsquito_Statue = new("creaturestatues", "Stone_Deathsquito_Statue", "assets");

            Stone_Deathsquito_Statue.Name.English("Stone Deathsquito");
            Stone_Deathsquito_Statue.Description.English("A Stone Statue of a Deathsquito.");
            Stone_Deathsquito_Statue.RequiredItems.Add("Stone", 10, true);
            Stone_Deathsquito_Statue.RequiredItems.Add("TrophyDeathsquito", 1, true);
            Stone_Deathsquito_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Deathsquito_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Deathsquito_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Deathsquito_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Deathsquito_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Lox_Calf_Statue = new("creaturestatues", "Stone_Lox_Calf_Statue", "assets");

            Stone_Lox_Calf_Statue.Name.English("Stone Lox Calf");
            Stone_Lox_Calf_Statue.Description.English("A Stone Statue of a Lox Calf.");
            Stone_Lox_Calf_Statue.RequiredItems.Add("Stone", 75, true);
            Stone_Lox_Calf_Statue.RequiredItems.Add("TrophyLox", 1, true);
            Stone_Lox_Calf_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Lox_Calf_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Lox_Calf_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Lox_Calf_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Lox_Calf_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Lox_Statue = new("creaturestatues", "Stone_Lox_Statue", "assets");

            Stone_Lox_Statue.Name.English("Stone Lox");
            Stone_Lox_Statue.Description.English("A Stone Statue of a Lox.");
            Stone_Lox_Statue.RequiredItems.Add("Stone", 375, true);
            Stone_Lox_Statue.RequiredItems.Add("TrophyLox", 1, true);
            Stone_Lox_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Lox_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Lox_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Lox_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Lox_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Fuling_Statue = new("creaturestatues", "Stone_Fuling_Statue", "assets");

            Stone_Fuling_Statue.Name.English("Stone Fuling");
            Stone_Fuling_Statue.Description.English("A Stone Statue of a Fuling.");
            Stone_Fuling_Statue.RequiredItems.Add("Stone", 25, true);
            Stone_Fuling_Statue.RequiredItems.Add("TrophyGoblin", 1, true);
            Stone_Fuling_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Fuling_Statue.RequiredItems.Add("Resin", 2, true);
            Stone_Fuling_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Fuling_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Fuling_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Fuling_Shaman_Statue = new("creaturestatues", "Stone_Fuling_Shaman_Statue", "assets");

            Stone_Fuling_Shaman_Statue.Name.English("Stone Fuling Shaman");
            Stone_Fuling_Shaman_Statue.Description.English("A Stone Statue of a Fuling Shaman.");
            Stone_Fuling_Shaman_Statue.RequiredItems.Add("Stone", 25, true);
            Stone_Fuling_Shaman_Statue.RequiredItems.Add("TrophyGoblinShaman", 1, true);
            Stone_Fuling_Shaman_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Fuling_Shaman_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_Fuling_Shaman_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Fuling_Shaman_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Fuling_Shaman_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Fuling_Berserker_Statue = new("creaturestatues", "Stone_Fuling_Berserker_Statue", "assets");

            Stone_Fuling_Berserker_Statue.Name.English("Stone Fuling Berserker");
            Stone_Fuling_Berserker_Statue.Description.English("A Stone Statue of a Fuling Berserker.");
            Stone_Fuling_Berserker_Statue.RequiredItems.Add("Stone", 50, true);
            Stone_Fuling_Berserker_Statue.RequiredItems.Add("TrophyGoblinBrute", 1, true);
            Stone_Fuling_Berserker_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Fuling_Berserker_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_Fuling_Berserker_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Fuling_Berserker_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Fuling_Berserker_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Fish_Statue = new("creaturestatues", "Stone_Fish_Statue", "assets");

            Stone_Fish_Statue.Name.English("Stone Fish");
            Stone_Fish_Statue.Description.English("A Stone Statue of a Fish.");
            Stone_Fish_Statue.RequiredItems.Add("Stone", 10, true);
            Stone_Fish_Statue.RequiredItems.Add("FishRaw", 1, true);
            Stone_Fish_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Fish_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Fish_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Leviathan_Small_Statue = new("creaturestatues", "Stone_Leviathan_Small_Statue", "assets");

            Stone_Leviathan_Small_Statue.Name.English("Stone Leviathan (Small)");
            Stone_Leviathan_Small_Statue.Description.English("A Stone Statue of a Small Leviathan.");
            Stone_Leviathan_Small_Statue.RequiredItems.Add("Stone", 100, true);
            Stone_Leviathan_Small_Statue.RequiredItems.Add("Chitin", 10, true);
            Stone_Leviathan_Small_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Leviathan_Small_Statue.RequiredItems.Add("Blueberries", 2, true);
            Stone_Leviathan_Small_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Leviathan_Small_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Leviathan_Small_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Leviathan_Large_Statue = new("creaturestatues", "Stone_Leviathan_Large_Statue", "assets");

            Stone_Leviathan_Large_Statue.Name.English("Stone Leviathan (Large)");
            Stone_Leviathan_Large_Statue.Description.English("A Stone Statue of a Large Leviathan.");
            Stone_Leviathan_Large_Statue.RequiredItems.Add("Stone", 250, true);
            Stone_Leviathan_Large_Statue.RequiredItems.Add("Chitin", 25, true);
            Stone_Leviathan_Large_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Leviathan_Large_Statue.RequiredItems.Add("Blueberries", 2, true);
            Stone_Leviathan_Large_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Leviathan_Large_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Leviathan_Large_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Sea_Serpent_Small_Statue = new("creaturestatues", "Stone_Sea_Serpent_Small_Statue", "assets");

            Stone_Sea_Serpent_Small_Statue.Name.English("Stone Sea Serpent (Small)");
            Stone_Sea_Serpent_Small_Statue.Description.English("A Stone Statue of a Small Sea Serpent.");
            Stone_Sea_Serpent_Small_Statue.RequiredItems.Add("Stone", 250, true);
            Stone_Sea_Serpent_Small_Statue.RequiredItems.Add("TrophySerpent", 1, true);
            Stone_Sea_Serpent_Small_Statue.RequiredItems.Add("GreydwarfEye", 6, true);
            Stone_Sea_Serpent_Small_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Sea_Serpent_Small_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Sea_Serpent_Small_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Sea_Serpent_Large_Statue = new("creaturestatues", "Stone_Sea_Serpent_Large_Statue", "assets");

            Stone_Sea_Serpent_Large_Statue.Name.English("Stone Serpent (Large)");
            Stone_Sea_Serpent_Large_Statue.Description.English("A Stone Statue of a Large Serpent.");
            Stone_Sea_Serpent_Large_Statue.RequiredItems.Add("Stone", 500, true);
            Stone_Sea_Serpent_Large_Statue.RequiredItems.Add("TrophySerpent", 1, true);
            Stone_Sea_Serpent_Large_Statue.RequiredItems.Add("GreydwarfEye", 6, true);
            Stone_Sea_Serpent_Large_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Sea_Serpent_Large_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Sea_Serpent_Large_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Eikthyr_Statue = new("creaturestatues", "Stone_Eikthyr_Statue", "assets");

            Stone_Eikthyr_Statue.Name.English("Stone Eikthyr");
            Stone_Eikthyr_Statue.Description.English("A Stone Statue of Eikthyr.");
            Stone_Eikthyr_Statue.RequiredItems.Add("Stone", 250, true);
            Stone_Eikthyr_Statue.RequiredItems.Add("TrophyEikthyr", 1, true);
            Stone_Eikthyr_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Eikthyr_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_Eikthyr_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Eikthyr_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Eikthyr_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_The_Elder_Statue = new("creaturestatues", "Stone_The_Elder_Statue", "assets");

            Stone_The_Elder_Statue.Name.English("Stone Elder");
            Stone_The_Elder_Statue.Description.English("A Stone Statue of The Elder.");
            Stone_The_Elder_Statue.RequiredItems.Add("Stone", 500, true);
            Stone_The_Elder_Statue.RequiredItems.Add("TrophyTheElder", 1, true);
            Stone_The_Elder_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_The_Elder_Statue.RequiredItems.Add("Raspberry", 2, true);
            Stone_The_Elder_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_The_Elder_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_The_Elder_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Bonemass_Statue = new("creaturestatues", "Stone_Bonemass_Statue", "assets");

            Stone_Bonemass_Statue.Name.English("Stone Bonemass");
            Stone_Bonemass_Statue.Description.English("A Stone Statue of Bonemass.");
            Stone_Bonemass_Statue.RequiredItems.Add("Stone", 750, true);
            Stone_Bonemass_Statue.RequiredItems.Add("TrophyBonemass", 1, true);
            Stone_Bonemass_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Bonemass_Statue.RequiredItems.Add("Resin", 5, true);
            Stone_Bonemass_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Bonemass_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Bonemass_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Moder_Statue = new("creaturestatues", "Stone_Moder_Statue", "assets");

            Stone_Moder_Statue.Name.English("Stone Moder");
            Stone_Moder_Statue.Description.English("A Stone Statue of Moder.");
            Stone_Moder_Statue.RequiredItems.Add("Stone", 1000, true);
            Stone_Moder_Statue.RequiredItems.Add("TrophyDragonQueen", 1, true);
            Stone_Moder_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Moder_Statue.RequiredItems.Add("DragonEgg", 1, true);
            Stone_Moder_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Moder_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Moder_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Yagluth_Statue = new("creaturestatues", "Stone_Yagluth_Statue", "assets");

            Stone_Yagluth_Statue.Name.English("Stone Yagluth");
            Stone_Yagluth_Statue.Description.English("A Stone Statue of Yagluth.");
            Stone_Yagluth_Statue.RequiredItems.Add("Stone", 1250, true);
            Stone_Yagluth_Statue.RequiredItems.Add("TrophyGoblinKing", 1, true);
            Stone_Yagluth_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Yagluth_Statue.RequiredItems.Add("SurtlingCore", 25, true);
            Stone_Yagluth_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Yagluth_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Yagluth_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Hare_Sitting_Statue = new("creaturestatues", "Stone_Hare_Sitting_Statue", "assets");

            Stone_Hare_Sitting_Statue.Name.English("Stone Hare (Sitting)");
            Stone_Hare_Sitting_Statue.Description.English("A Stone Statue of a Hare sitting.");
            Stone_Hare_Sitting_Statue.RequiredItems.Add("Stone", 50, true);
            Stone_Hare_Sitting_Statue.RequiredItems.Add("TrophyHare", 1, true);
            Stone_Hare_Sitting_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Hare_Sitting_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Hare_Sitting_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Hare_Running_Statue = new("creaturestatues", "Stone_Hare_Running_Statue", "assets");

            Stone_Hare_Running_Statue.Name.English("Stone Hare (Running)");
            Stone_Hare_Running_Statue.Description.English("A Stone Statue of a Hare running.");
            Stone_Hare_Running_Statue.RequiredItems.Add("Stone", 50, true);
            Stone_Hare_Running_Statue.RequiredItems.Add("TrophyHare", 1, true);
            Stone_Hare_Running_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Hare_Running_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Hare_Running_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Dvergr_Archer_Statue = new("creaturestatues", "Stone_Dvergr_Archer_Statue", "assets");

            Stone_Dvergr_Archer_Statue.Name.English("Stone Dvergr Archer");
            Stone_Dvergr_Archer_Statue.Description.English("A Stone Statue of a Dvergr Archer.");
            Stone_Dvergr_Archer_Statue.RequiredItems.Add("Stone", 100, true);
            Stone_Dvergr_Archer_Statue.RequiredItems.Add("TrophyDvergr", 1, true);
            Stone_Dvergr_Archer_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Dvergr_Archer_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Dvergr_Archer_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Dvergr_Archer_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Dvergr_Mage_Statue = new("creaturestatues", "Stone_Dvergr_Mage_Statue", "assets");

            Stone_Dvergr_Mage_Statue.Name.English("Stone Dvergr Mage");
            Stone_Dvergr_Mage_Statue.Description.English("A Stone Statue of a Dvergr Mage.");
            Stone_Dvergr_Mage_Statue.RequiredItems.Add("Stone", 100, true);
            Stone_Dvergr_Mage_Statue.RequiredItems.Add("TrophyDvergr", 1, true);
            Stone_Dvergr_Mage_Statue.RequiredItems.Add("GreydwarfEye", 2, true);
            Stone_Dvergr_Mage_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Dvergr_Mage_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Dvergr_Mage_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Seeker_Statue = new("creaturestatues", "Stone_Seeker_Statue", "assets");

            Stone_Seeker_Statue.Name.English("Stone Seeker");
            Stone_Seeker_Statue.Description.English("A Stone Statue of a Seeker.");
            Stone_Seeker_Statue.RequiredItems.Add("Stone", 125, true);
            Stone_Seeker_Statue.RequiredItems.Add("TrophySeeker", 1, true);
            Stone_Seeker_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Seeker_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Seeker_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Seeker_Brood_Statue = new("creaturestatues", "Stone_Seeker_Brood_Statue", "assets");

            Stone_Seeker_Brood_Statue.Name.English("Stone Seeker Brood");
            Stone_Seeker_Brood_Statue.Description.English("A Stone Statue of a Seeker Brood.");
            Stone_Seeker_Brood_Statue.RequiredItems.Add("Stone", 75, true);
            Stone_Seeker_Brood_Statue.RequiredItems.Add("TrophySeeker", 1, true);
            Stone_Seeker_Brood_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Seeker_Brood_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Seeker_Brood_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Seeker_Brute_Statue = new("creaturestatues", "Stone_Seeker_Brute_Statue", "assets");

            Stone_Seeker_Brute_Statue.Name.English("Stone Seeker Brute");
            Stone_Seeker_Brute_Statue.Description.English("A Stone Statue of a Seeker Brood.");
            Stone_Seeker_Brute_Statue.RequiredItems.Add("Stone", 175, true);
            Stone_Seeker_Brute_Statue.RequiredItems.Add("TrophySeekerBrute", 1, true);
            Stone_Seeker_Brute_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Seeker_Brute_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Seeker_Brute_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Tick_Statue = new("creaturestatues", "Stone_Tick_Statue", "assets");

            Stone_Tick_Statue.Name.English("Stone Tick");
            Stone_Tick_Statue.Description.English("A Stone Statue of a Tick.");
            Stone_Tick_Statue.RequiredItems.Add("Stone", 50, true);
            Stone_Tick_Statue.RequiredItems.Add("TrophyTick", 1, true);
            Stone_Tick_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Tick_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Tick_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Gjall_Small_Statue = new("creaturestatues", "Stone_Gjall_Small_Statue", "assets");

            Stone_Gjall_Small_Statue.Name.English("Stone Gjall (Small)");
            Stone_Gjall_Small_Statue.Description.English("A Stone Statue of a Small Gjall.");
            Stone_Gjall_Small_Statue.RequiredItems.Add("Stone", 100, true);
            Stone_Gjall_Small_Statue.RequiredItems.Add("TrophyGjall", 1, true);
            Stone_Gjall_Small_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Gjall_Small_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Gjall_Small_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Gjall_Large_Statue = new("creaturestatues", "Stone_Gjall_Large_Statue", "assets");

            Stone_Gjall_Large_Statue.Name.English("Stone Gjall (Large)");
            Stone_Gjall_Large_Statue.Description.English("A Stone Statue of a Large Gjall.");
            Stone_Gjall_Large_Statue.RequiredItems.Add("Stone", 200, true);
            Stone_Gjall_Large_Statue.RequiredItems.Add("TrophyGjall", 1, true);
            Stone_Gjall_Large_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Gjall_Large_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Gjall_Large_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            
            BuildPiece Stone_Queen_Statue = new("creaturestatues", "Stone_Queen_Statue", "assets");

            Stone_Queen_Statue.Name.English("Stone Queen");
            Stone_Queen_Statue.Description.English("A Stone Statue of the Queen.");
            Stone_Queen_Statue.RequiredItems.Add("Stone", 1500, true);
            Stone_Queen_Statue.RequiredItems.Add("TrophySeekerQueen", 1, true);
            Stone_Queen_Statue.RequiredItems.Add("DvergrKey", 1, true);
            Stone_Queen_Statue.Category.Add(BuildPieceCategory.Furniture);
            Stone_Queen_Statue.Crafting.Set(PieceManager.CraftingTable.Workbench);
            Stone_Queen_Statue.SpecialProperties =
                new SpecialProperties()
                    { AdminOnly = false, NoConfig = false };
            #endregion

            /*#region SkillManager Example Code

            Skill
                tenacity = new("Tenacity",
                    "tenacity-icon.png"); // Skill name along with the skill icon. By default the icon is found in the icons folder. Put it there if you wish to load one.

            tenacity.Description.English("Reduces damage taken by 0.2% per level.");
            tenacity.Name.German("Hartnäckigkeit"); // Use this to localize values for the name
            tenacity.Description.German(
                "Reduziert erlittenen Schaden um 0,2% pro Stufe."); // You can do the same for the description
            tenacity.Configurable = true;

            #endregion*/

            /*#region LocationManager Example Code

            _ = new LocationManager.Location("guildfabs", "GuildAltarSceneFab")
            {
                MapIcon = "portalicon.png",
                ShowMapIcon = ShowIcon.Explored,
                MapIconSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),
                    100.0f),
                CanSpawn = true,
                SpawnArea = Heightmap.BiomeArea.Everything,
                Prioritize = true,
                PreferCenter = true,
                Rotation = Rotation.Slope,
                HeightDelta = new Range(0, 2),
                SnapToWater = false,
                ForestThreshold = new Range(0, 2.19f),
                Biome = Heightmap.Biome.Meadows,
                SpawnDistance = new Range(500, 1500),
                SpawnAltitude = new Range(10, 100),
                MinimumDistanceFromGroup = 100,
                GroupName = "groupName",
                Count = 15,
                Unique = true
            };

            #region Location Notes

            // MapIcon                      Sets the map icon for the location.
            // ShowMapIcon                  When to show the map icon of the location. Requires an icon to be set. Use "Never" to not show a map icon for the location. Use "Always" to always show a map icon for the location. Use "Explored" to start showing a map icon for the location as soon as a player has explored the area.
            // MapIconSprite                Sets the map icon for the location.
            // CanSpawn                     Can the location spawn at all.
            // SpawnArea                    If the location should spawn more towards the edge of the biome or towards the center. Use "Edge" to make it spawn towards the edge. Use "Median" to make it spawn towards the center. Use "Everything" if it doesn't matter.</para>
            // Prioritize                   If set to true, this location will be prioritized over other locations, if they would spawn in the same area.
            // PreferCenter                 If set to true, Valheim will try to spawn your location as close to the center of the map as possible.
            // Rotation                     How to rotate the location. Use "Fixed" to use the rotation of the prefab. Use "Random" to randomize the rotation. Use "Slope" to rotate the location along a possible slope.
            // HeightDelta                  The minimum and maximum height difference of the terrain below the location.
            // SnapToWater                  If the location should spawn near water.
            // ForestThreshold              If the location should spawn in a forest. Everything above 1.15 is considered a forest by Valheim. 2.19 is considered a thick forest by Valheim.
            // Biome
            // SpawnDistance                Minimum and maximum range from the center of the map for the location.
            // SpawnAltitude                Minimum and maximum altitude for the location.
            // MinimumDistanceFromGroup     Locations in the same group will keep at least this much distance between each other.
            // GroupName                    The name of the group of the location, used by the minimum distance from group setting.
            // Count                        Maximum number of locations to spawn in. Does not mean that this many locations will spawn. But Valheim will try its best to spawn this many, if there is space.
            // Unique                       If set to true, all other locations will be deleted, once the first one has been discovered by a player.

            #endregion

            #endregion*/

            /*#region StatusEffectManager Example Code

            CustomSE mycooleffect = new("Toxicity");
            mycooleffect.Name.English("Toxicity");
            mycooleffect.Type = EffectType.Consume;
            mycooleffect.IconSprite = null;
            mycooleffect.Name.German("Toxizität");
            mycooleffect.Effect.m_startMessageType = MessageHud.MessageType.TopLeft;
            mycooleffect.Effect.m_startMessage = "My Cool Status Effect Started";
            mycooleffect.Effect.m_stopMessageType = MessageHud.MessageType.TopLeft;
            mycooleffect.Effect.m_stopMessage = "Not cool anymore, ending effect.";
            mycooleffect.Effect.m_tooltip = "<color=orange>Toxic damage over time</color>";
            mycooleffect.AddSEToPrefab(mycooleffect, "SwordIron");

            CustomSE drunkeffect = new("se_drunk", "se_drunk_effect");
            drunkeffect.Name.English("Drunk"); // You can use this to fix the display name in code
            drunkeffect.Icon =
                "DrunkIcon.png"; // Use this to add an icon (64x64) for the status effect. Put your icon in an "icons" folder
            drunkeffect.Name.German("Betrunken"); // Or add translations for other languages
            drunkeffect.Effect.m_startMessageType =
                MessageHud.MessageType.Center; // Specify where the start effect message shows
            drunkeffect.Effect.m_startMessage = "I'm drunk!"; // What the start message says
            drunkeffect.Effect.m_stopMessageType =
                MessageHud.MessageType.Center; // Specify where the stop effect message shows
            drunkeffect.Effect.m_stopMessage = "Sober...again."; // What the stop message says
            drunkeffect.Effect.m_tooltip =
                "<color=red>Your vision is blurry</color>"; // Tooltip that will describe the effect applied to the player
            drunkeffect.AddSEToPrefab(drunkeffect,
                "TankardAnniversary"); // Adds the status effect to the Anniversary Tankard. Applies when equipped.

            // Create a new status effect in code and apply it to a prefab.
            CustomSE codeSE = new("CodeStatusEffect");
            codeSE.Name.English("New Effect");
            codeSE.Type = EffectType.Consume; // Set the type of status effect this should be.
            codeSE.Icon = "ModDevPower.png";
            codeSE.Name.German("Betrunken"); // Or add translations for other languages
            codeSE.Effect.m_startMessageType =
                MessageHud.MessageType.Center; // Specify where the start effect message shows
            codeSE.Effect.m_startMessage = "Mod Dev power, granted."; // What the start message says
            codeSE.Effect.m_stopMessageType =
                MessageHud.MessageType.Center; // Specify where the stop effect message shows
            codeSE.Effect.m_stopMessage = "Mod Dev power, removed."; // What the stop message says
            codeSE.Effect.m_tooltip =
                "<color=green>You now have Mod Dev POWER!</color>"; // Tooltip that will describe the effect applied to the player
            codeSE.AddSEToPrefab(codeSE,
                "SwordCheat"); // Adds the status effect to the Cheat Sword. Applies when equipped.

            #endregion*/

            /*#region ItemManager Example Code

            Item ironFangAxe = new("ironfang", "IronFangAxe", "IronFang");
            ironFangAxe.Name.English("Iron Fang Axe"); // You can use this to fix the display name in code
            ironFangAxe.Description.English("A sharp blade made of iron.");
            ironFangAxe.Name.German("Eisenzahnaxt"); // Or add translations for other languages
            ironFangAxe.Description.German("Eine sehr scharfe Axt, bestehend aus Eisen und Wolfszähnen.");
            ironFangAxe.Crafting.Add("MyAmazingCraftingStation",
                3); // Custom crafting stations can be specified as a string
            ironFangAxe.RequiredItems.Add("Iron", 120);
            ironFangAxe.RequiredItems.Add("WolfFang", 20);
            ironFangAxe.RequiredItems.Add("Silver", 40);
            ironFangAxe.RequiredUpgradeItems
                .Add("Iron", 20); // Upgrade requirements are per item, even if you craft two at the same time
            ironFangAxe.RequiredUpgradeItems.Add("Silver",
                10); // 10 Silver: You need 10 silver for level 2, 20 silver for level 3, 30 silver for level 4
            ironFangAxe.CraftAmount = 2; // We really want to dual wield these


            // If you have something that shouldn't go into the ObjectDB, like vfx or sfx that only need to be added to ZNetScene
            ItemManager.PrefabManager.RegisterPrefab(PrefabManager.RegisterAssetBundle("ironfang"), "axeVisual",
                false); // If our axe has a special visual effect, like a glow, we can skip adding it to the ObjectDB this way
            ItemManager.PrefabManager.RegisterPrefab(PrefabManager.RegisterAssetBundle("ironfang"), "axeSound",
                false); // Same for special sound effects

            Item heroBlade = new("heroset", "HeroBlade");
            heroBlade.Crafting.Add(ItemManager.CraftingTable.Workbench, 2);
            heroBlade.RequiredItems.Add("Wood", 5);
            heroBlade.RequiredItems.Add("DeerHide", 2);
            heroBlade.RequiredUpgradeItems.Add("Wood", 2);
            heroBlade.RequiredUpgradeItems.Add("Flint", 2); // You can even add new items for the upgrade

            Item heroShield = new("heroset", "HeroShield");
            heroShield["My first recipe"].Crafting
                .Add(ItemManager.CraftingTable.Workbench,
                    1); // You can add multiple recipes for the same item, by giving the recipe a name
            heroShield["My first recipe"].RequiredItems.Add("Wood", 10);
            heroShield["My first recipe"].RequiredItems.Add("Flint", 5);
            heroShield["My first recipe"].RequiredUpgradeItems.Add("Wood", 5);
            heroShield["My alternate recipe"].Crafting
                .Add(ItemManager.CraftingTable.Workbench, 1); // And this is our second recipe then
            heroShield["My alternate recipe"].RequiredItems.Add("Bronze", 2);
            heroShield["My alternate recipe"].RequiredUpgradeItems.Add("Bronze", 1);
            heroShield.Snapshot(); // I don't have an icon for this item in my asset bundle, so I will let the ItemManager generate one automatically
            // The icon for the item will have the same rotation as the item in unity

            _ =
                new Conversion(
                        heroBlade) // For some reason, we want to be able to put a hero shield into a smelter, to get a hero blade
                    {
                        Input = "HeroShield",
                        Piece = ConversionPiece.Smelter
                    };

            #endregion*/

            /*#region CreatureManager Example Code

            Creature wereBearBlack = new("werebear", "WereBearBlack")
            {
                Biome = Heightmap.Biome.Meadows,
                GroupSize = new CreatureManager.Range(1, 2),
                CheckSpawnInterval = 600,
                RequiredWeather = Weather.Rain | Weather.Fog,
                Maximum = 2
            };
            wereBearBlack.Localize()
                .English("Black Werebear")
                .German("Schwarzer Werbär")
                .French("Ours-Garou Noir");
            wereBearBlack.Drops["Wood"].Amount = new CreatureManager.Range(1, 2);
            wereBearBlack.Drops["Wood"].DropChance = 100f;

            Creature wereBearRed = new("werebear", "WereBearRed")
            {
                Biome = Heightmap.Biome.AshLands,
                GroupSize = new CreatureManager.Range(1, 1),
                CheckSpawnInterval = 900,
                AttackImmediately = true,
                RequiredGlobalKey = GlobalKey.KilledYagluth,
            };
            wereBearRed.Localize()
                .English("Red Werebear")
                .German("Roter Werbär")
                .French("Ours-Garou Rouge");
            wereBearRed.Drops["Coal"].Amount = new CreatureManager.Range(1, 2);
            wereBearRed.Drops["Coal"].DropChance = 100f;
            wereBearRed.Drops["Flametal"].Amount = new CreatureManager.Range(1, 1);
            wereBearRed.Drops["Flametal"].DropChance = 10f;

            #endregion*/


            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                CreatureStatuesLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                CreatureStatuesLogger.LogError($"There was an issue loading your {ConfigFileName}");
                CreatureStatuesLogger.LogError("Please check your config entries for spelling and format!");
            }
        }

        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }

        class AcceptableShortcuts : AcceptableValueBase
        {
            public AcceptableShortcuts() : base(typeof(KeyboardShortcut))
            {
            }

            public override object Clamp(object value) => value;
            public override bool IsValid(object value) => true;

            public override string ToDescriptionString() =>
                "# Acceptable values: " + string.Join(", ", KeyboardShortcut.AllKeyCodes);
        }

        #endregion
    }
}