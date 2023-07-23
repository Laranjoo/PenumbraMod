using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace PenumbraMod.Content.Buffs
{

	public class PrismThorns : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Prismatic Weak Protection"); // Buff display name
			// Description.SetDefault("You feel weak, but still can damage enemies when hit"); // Buff description
            Main.buffNoTimeDisplay[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex)
        {
            player.thorns = 0.45f;
            player.GetModPlayer<BuffPlayer>().aura2 = true;
        }

        
    }

    
}
