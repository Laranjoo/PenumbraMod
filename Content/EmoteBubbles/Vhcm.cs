using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace PenumbraMod.Content.EmoteBubbles
{
    public class Vhcm : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            // Add NPC emotes to "Town" category.
            AddToCategory(EmoteID.Category.Town);
        }
    }
}
