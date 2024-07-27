using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Common.Base;
using PenumbraMod.Common.Players;
using PenumbraMod.Common.Systems;
using PenumbraMod.Content;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.ExpertAccessorySlot;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items.Placeable.Furniture;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.UI;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace PenumbraMod
{
    public partial class PenumbraMod : Mod
    {
        public static PenumbraMod Instance { get; set; }
        internal UserInterface _ReaperbarUserInterface;
        internal ReaperUI ReaperBarUI;
        internal UserInterface _UnlockedExpertSlotInterface;
        internal UnlockedUI UnlockedUISlot;
        public PenumbraMod() => Instance = this;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                ReaperBarUI = new ReaperUI();
                UnlockedUISlot = new UnlockedUI();
                _UnlockedExpertSlotInterface = new UserInterface();
                _UnlockedExpertSlotInterface.SetState(UnlockedUISlot);
                _ReaperbarUserInterface = new UserInterface();
                _ReaperbarUserInterface.SetState(ReaperBarUI);
                Main.QueueMainThreadAction(() =>
                {
                    Texture2D brightness6 = Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness6", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness6);
                    Texture2D brightness2 = Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness2", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness2);
                    Texture2D brightness2D = Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness2Death", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness2D);
                    Texture2D brightnessP2 = Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/BrightnessP2", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightnessP2);
                    Texture2D brightness7 = Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Brightness7", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref brightness7);
                    Texture2D lightaccessory = Request<Texture2D>("PenumbraMod/Content/ExpertAccessorySlot/LightUnlocked", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref lightaccessory);
                    Texture2D WeakGlow = Request<Texture2D>("PenumbraMod/Assets/Textures/WeakGlow", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref WeakGlow);
                    Texture2D MediumGlow = Request<Texture2D>("PenumbraMod/Assets/Textures/MediumGlow", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref MediumGlow);
                    Texture2D StrongGlow = Request<Texture2D>("PenumbraMod/Assets/Textures/StrongGlow", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref StrongGlow);
                    Texture2D WeakGlowb = Request<Texture2D>("PenumbraMod/Assets/Textures/WeakGlow-big", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref WeakGlow);
                    Texture2D MediumGlowb = Request<Texture2D>("PenumbraMod/Assets/Textures/MediumGlow-big", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref MediumGlow);
                    Texture2D StrongGlowb = Request<Texture2D>("PenumbraMod/Assets/Textures/StrongGlow-big", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref StrongGlow);
                    Texture2D StrongGlowbg = Request<Texture2D>("PenumbraMod/Assets/Textures/StrongGlowBG", AssetRequestMode.ImmediateLoad).Value;
                    GetTexturePremultiplied(ref StrongGlow);
                });
                Main.instance.LoadItem(ItemID.CursedFlames);
                Main.instance.LoadItem(ItemID.IceSickle);
                Main.instance.LoadItem(ItemID.PlatinumCrown);
                TextureAssets.Item[ItemID.IceSickle] = Request<Texture2D>("PenumbraMod/Content/Items/VanillaResprites/IceSickleResprite");
                TextureAssets.Item[ItemID.CursedFlames] = Request<Texture2D>("PenumbraMod/Content/Items/VanillaResprites/CursedFlameInv");
                TextureAssets.Item[ItemID.PlatinumCrown] = Request<Texture2D>("PenumbraMod/Content/Items/VanillaResprites/PlatinumTiara");
            }
            if (Main.netMode != NetmodeID.Server)
            {
                SkyManager.Instance["PenumbraMod:StormSky"] = new StormSky();
                SkyManager.Instance["PenumbraMod:StormSky2"] = new StormSky2();
            }

        }
        public static void GetTexturePremultiplied(ref Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(
                        buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }
        List<int> collection;
        public override void PostSetupContent()
        {
            /* Boss checklist tier
            public const float SlimeKing = 1f;
            public const float EyeOfCthulhu = 2f;
            public const float EaterOfWorlds = 3f;
            public const float QueenBee = 4f;
            public const float Skeletron = 5f;
            public const float Deerclops = 6f;
            public const float WallOfFlesh = 7f;
            public const float TheTwins = 8f;
            public const float TheDestroyer = 9f;
            public const float SkeletronPrime = 10f;
            public const float Plantera = 11f;
            public const float Golem = 12f;
            public const float DukeFishron = 13f;
            public const float LunaticCultist = 14f;
            public const float Moonlord = 15f;*/

            if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
            {
                #region Eyeofstorm

                var portrait = (SpriteBatch sb, Rectangle rect, Color color) =>
                {
                    Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/Bosses/Eyestorm/Eyeofthestorm_Preview").Value;
                    Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                    sb.Draw(texture, centered, color);
                };
                collection = new List<int>()
                {
                  ItemID.GoldBar,
                ItemID.SandBoots,
                ModContent.ItemType<ShockWave>(),
                ModContent.ItemType<SparkBow>(),
                ModContent.ItemType<StaffofEnergy>(),
                ModContent.ItemType<ChargeGun>(),
                ModContent.ItemType<OmniStaff>(),
                ModContent.ItemType<EyestormBag>(),
                ModContent.ItemType<EyestormRelic>()
                };
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    nameof(Eyeofthestorm),
                    5.5f,
                    () => DownedBossSystem.downedEyestormBoss,
                    ModContent.NPCType<Eyeofthestorm>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ItemType<EyestormSummon>(),
                        ["collectibles"] = collection,
                        ["customPortrait"] = portrait,
                        ["spawnInfo"] = $"PenumbraMod.Eyeofthestorm.BossChecklistIntegration.SpawnInfo",
                        ["despawnMessage"] = $"PenumbraMod.Eyeofthestorm.BossChecklistIntegration.DespawnMessage"
                    }
                );

                #endregion
            }

        }

        public override void Unload()
        {
            TextureAssets.Item[ItemID.CursedFlames] = Request<Texture2D>($"Terraria/Images/Item_519");
            TextureAssets.Item[ItemID.IceSickle] = Request<Texture2D>($"Terraria/Images/Item_1306");
            TextureAssets.Item[ItemID.PlatinumCrown] = Request<Texture2D>($"Terraria/Images/" + ItemID.PlatinumCrown);
        }
        /// <summary>
        /// Eye of the storm glomask effect
        /// </summary>
        public static Color Eyestorm => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.White, Color.Blue * 0.6f, Color.White);
        /// <summary>
        /// Effect used for some storm based projectiles
        /// </summary>
        public static Color Storm => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Blue, Color.LightBlue * 0.6f, Color.Blue);
        /// <summary>
        /// Used for the Phantom's Penumbratic darkmatter scythe
        /// </summary>
        public static Color Phantom => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Purple, Color.Purple * 0.6f, Color.Gray);
        /// <summary>
        /// Used for the composite sword line
        /// </summary>
        public static Color CompositeSword => BaseExtension.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Gray, Color.White * 0.6f, Color.Gray);

        internal static void SaveConfig(PenumbraConfig cfg)
        {
            // There is no current way to manually save a mod configuration file in tModLoader.
            // The method which saves mod config files is private in ConfigManager, so reflection is used to invoke it.
            // Inspired from calamity, im sorry again, but that was the way
            try
            {
                MethodInfo saveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
                if (saveMethodInfo is not null)
                    saveMethodInfo.Invoke(null, new object[] { cfg });
                else
                    Instance.Logger.Error("TML ConfigManager.Save reflection failed. Method signature has changed. Notify Penumbra Devs if you see this in your log.");
            }
            catch
            {
                Instance.Logger.Error("An error occurred while manually saving Penumbra mod configuration. This may be due to a complex mod conflict. It is safe to ignore this error.");
            }
        }
    }
    public class PenumbraModSystem : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Mushrooms"));
            int ShiniesIndex2 = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            int ShiniesIndex3 = tasks.FindIndex(genpass => genpass.Name.Equals("Clean Up Dirt"));
            var ShiniesIndex4 = tasks.FindIndex(genpass => genpass.Name.Equals("Marble"));

            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new MarbleBiome("Marble Biome", 237.4298f));
            }
            if (ShiniesIndex2 != -1)
            {
                tasks.Insert(ShiniesIndex2 + 1, new LostSword("Lost Sword", 237.4298f));
            }
            if (ShiniesIndex3!= -1)
            {
                tasks.Insert(ShiniesIndex3 + 1, new ShimmerStructure("Shimmer Altar", 237.4298f));
            }
            if (ShiniesIndex4 != -1)
            {
                tasks.RemoveAt(ShiniesIndex4);
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            GetInstance<PenumbraMod>()._ReaperbarUserInterface?.Update(gameTime);
            GetInstance<PenumbraMod>()._UnlockedExpertSlotInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                // PenumbraMod2 lmao
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "PenumbraMod2: Reaper Energy Bar",
                    delegate
                    {
                        GetInstance<PenumbraMod>()._ReaperbarUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                                   "PenumbraMod2: Expert Slot Unlocked UI",
                                   delegate
                                   {
                                       GetInstance<PenumbraMod>()._UnlockedExpertSlotInterface.Draw(Main.spriteBatch, new GameTime());
                                       return true;
                                   },
                                   InterfaceScaleType.UI)
                               );
            }
        }
        public override void PostWorldGen()
        {
            #region gume
            int[] itemsToPlaceInHellChests = { ItemType<FlyingGume>() };
            int itemsToPlaceInHellChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 4 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == 0)
                        {
                            if (!WorldGen.genRand.NextBool(5)) break;
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInHellChests[itemsToPlaceInHellChestsChoice]);
                            itemsToPlaceInHellChestsChoice = (itemsToPlaceInHellChestsChoice + 1) % itemsToPlaceInHellChests.Length;
                            break;
                        }
                    }
                }
            }
            #endregion
            #region soulknife
            int[] itemsToPlaceInMarbleChests = { ItemType<SoulStrikeKnifes>() };
            int itemsToPlaceInMarbleChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 50 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == 0)
                        {
                            if (!WorldGen.genRand.NextBool(2)) break;
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInMarbleChests[itemsToPlaceInMarbleChestsChoice]);
                            itemsToPlaceInMarbleChestsChoice = (itemsToPlaceInMarbleChestsChoice + 1) % itemsToPlaceInMarbleChests.Length;
                            break;
                        }
                    }
                }
            }
            #endregion
        }
    }  
    public class LostSword : GenPass
    {
        public LostSword(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            // progress.Message is the message shown to the user while the following code is running.
            // Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
            progress.Message = (string)PenumbraLocalization.LostSword;
            if (!Main.zenithWorld)
            {
                int x = WorldGen.genRand.Next(Main.maxTilesX / 2 - 905, Main.maxTilesX / 2 + 910);
                int y = WorldGen.genRand.Next((int)Main.rockLayer - 201, (int)Main.rockLayer - 200);
                StructureHelper.Generator.GenerateStructure("Content/Structures/LostSwordSurface", new Point16(x, y), PenumbraMod.Instance);
            }
            else 
            {
                int x22 = WorldGen.genRand.Next(Main.maxTilesX / 2 - 405, Main.maxTilesX / 2 + 410);
                int y22 = WorldGen.genRand.Next((int)Main.rockLayer + 20, (int)Main.rockLayer + 50);
                StructureHelper.Generator.GenerateStructure("Content/Structures/LostSwordCavern", new Point16(x22, y22), PenumbraMod.Instance);
            }
               

            int x2 = WorldGen.genRand.Next(Main.maxTilesX / 2 - 405, Main.maxTilesX / 2 + 410);
            int y2 = WorldGen.genRand.Next((int)Main.rockLayer + 20, (int)Main.rockLayer + 50);
            StructureHelper.Generator.GenerateStructure("Content/Structures/LostSwordCavern", new Point16(x2, y2), PenumbraMod.Instance);
        }
    }
    public class MarbleBiome : GenPass
    {
        public MarbleBiome(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            // progress.Message is the message shown to the user while the following code is running.
            // Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
            if (Main.zenithWorld)
            {
                progress.Message = (string)PenumbraLocalization.Marble;
                int x = WorldGen.genRand.Next(Main.maxTilesX / 2 - 200, Main.maxTilesX / 2 + 1400);
                int y = WorldGen.genRand.Next((int)Main.rockLayer + 10, (int)Main.rockLayer + 30);
                StructureHelper.Generator.GenerateStructure("Content/Structures/MarbleBiome", new Point16(x, y), PenumbraMod.Instance);

                int x2 = WorldGen.genRand.Next(Main.maxTilesX / 2 - 1200, Main.maxTilesX / 2 - 800);
                int y2 = WorldGen.genRand.Next((int)Main.rockLayer + 5, (int)Main.rockLayer + 10);
                StructureHelper.Generator.GenerateStructure("Content/Structures/MarbleBiomeArena", new Point16(x2, y2), PenumbraMod.Instance);
            }
            else
            {
                progress.Message = (string)PenumbraLocalization.Marble;
                int x = WorldGen.genRand.Next(Main.maxTilesX / 2 - 200, Main.maxTilesX / 2 + 1400);
                int y = WorldGen.genRand.Next((int)Main.rockLayer + 200, (int)Main.rockLayer + 400);
                StructureHelper.Generator.GenerateStructure("Content/Structures/MarbleBiome", new Point16(x, y), PenumbraMod.Instance);

                int x2 = WorldGen.genRand.Next(Main.maxTilesX / 2 - 1200, Main.maxTilesX / 2 - 800);
                int y2 = WorldGen.genRand.Next((int)Main.rockLayer + 200, (int)Main.rockLayer + 400);
                StructureHelper.Generator.GenerateStructure("Content/Structures/MarbleBiomeArena", new Point16(x2, y2), PenumbraMod.Instance);
            }
           
        }
    }
    public class ShimmerStructure : GenPass
    {
        public ShimmerStructure(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            // progress.Message is the message shown to the user while the following code is running.
            // Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
            progress.Message = (string)PenumbraLocalization.ShimmerStructure;
            if (GenVars.shimmerPosition.X < Main.maxTilesX / 2)
            {
                int x = (int)GenVars.shimmerPosition.X - 84;
                int y = (int)GenVars.shimmerPosition.Y - 18;
                StructureHelper.Generator.GenerateStructure("Content/Structures/ShimmerStructure", new Point16(x, y), PenumbraMod.Instance);
            }
            else
            {
                int x = (int)GenVars.shimmerPosition.X + 71;
                int y = (int)GenVars.shimmerPosition.Y - 18;
                StructureHelper.Generator.GenerateStructure("Content/Structures/ShimmerStructure", new Point16(x, y), PenumbraMod.Instance);
            }

        }
    }
}