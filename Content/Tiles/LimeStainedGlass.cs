using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Tiles
{
    public class LimeStainedGlass : ModWall
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(new Color(91, 167, 70));
        }
    }
}