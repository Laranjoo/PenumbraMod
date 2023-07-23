using Microsoft.Xna.Framework;
using PenumbraMod.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Biomes
{
    public class AkuBiome : ModBiome
    {
        // Select Music
       public override int Music => MusicLoader.GetMusicSlot("PenumbraMod/Assets/Music/funk3");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;       
        // Populate the Bestiary Filter
       // public override string BestiaryIcon => "PenumbraMod/Content/Biomes/AkuIcon";
      //  public override string BackgroundPath => "PenumbraMod/Content/Biomes/AkuBiomeBestiary";
        public override Color? BackgroundColor => base.BackgroundColor;

        public override string MapBackground => BackgroundPath; // Re-uses Bestiary Background for Map Background

        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            bool b1 = ModContent.GetInstance<AkuCheckSystem>().AkuCount >= 40;
            bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
            return b1 && b3;
        }
    }
}
