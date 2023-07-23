using Microsoft.Xna.Framework;
using PenumbraMod.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Biomes.Vanilla
{
    public class MarbleBiomeChange : ModBiome
    {
        // Select Music
        public override int Music => MusicLoader.GetMusicSlot("PenumbraMod/Assets/Music/MarbleBiome");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;       
        // Populate the Bestiary Filter
        public override string BestiaryIcon => "PenumbraMod/Content/Biomes/Vanilla/MarbleIcon";
        public override string BackgroundPath => "PenumbraMod/Content/Biomes/Vanilla/MarbleBiomeBestiary";
        public override Color? BackgroundColor => base.BackgroundColor;
        public override string MapBackground => BackgroundPath; // Re-uses Bestiary Background for Map Background

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            bool b1 = ModContent.GetInstance<MarbleCheckSystem>().MarbleCount >= 50;
            bool b2 = player.ZoneMarble;
            bool b3 = player.ZoneNormalCaverns;
            return b1 || b2 && b3;
        }
    }
}
