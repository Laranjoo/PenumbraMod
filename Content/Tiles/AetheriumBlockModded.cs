using Microsoft.Xna.Framework;
using PenumbraMod.Common;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
    public class AetheriumBlockModded : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(247, 228, 254), name);
            DustType = DustID.ShimmerSpark;
            HitSound = SoundID.Tink;
            MineResist = 555f;
            MinPick = 99999;
        }
        public static bool cutscene;
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            if (cutscene)
                time++;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (cutscene)
            {
                if (time >= 0 && time <= 1100)
                {
                    if (!Main.hideUI)
                        Main.hideUI = true;
                }
                if (time >= 1101)
                {
                    Main.hideUI = false;
                    Main.LocalPlayer.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene = false;
                    Main.LocalPlayer.GetModPlayer<PenumbraGlobalPlayer>().absolutecamera = false;
                    cutscene = false;
                }
            }
            else
            {
                time = 0;
            }
        }
        public static float time;
        public override void FloorVisuals(Player player)
        {
            if (player.velocity.X != 0f)
            {
                if (player.HasItem(ModContent.ItemType<FirstShardOfTheMageblade>())
                    && player.HasItem(ModContent.ItemType<SecondShardOfTheMageblade>())
                    && player.HasItem(ModContent.ItemType<ThirdShardOfTheMageblade>())
                    && player.HasItem(ModContent.ItemType<FourthShardOfTheMageblade>()))
                {
                    player.AddBuff(ModContent.BuffType<StunnedNPC>(), 600);
                    Main.LocalPlayer.GetModPlayer<PenumbraGlobalPlayer>().MagebladeCutscene = true;
                    cutscene = true;
                }
            }
        }
    }
}
