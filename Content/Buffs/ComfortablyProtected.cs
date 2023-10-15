using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace PenumbraMod.Content.Buffs
{

	public class ComfortablyProtected : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Comfortably Protected"); // Buff display name
			/* Description.SetDefault("You feel comfortably protected..."
                + "\nDefense Increased by 4"); */ // Buff description
		}
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 4;
            player.GetModPlayer<BuffPlayer>().MarshmellowEffect = true;
        }
    }

    
}
