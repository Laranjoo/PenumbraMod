using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;

namespace PenumbraMod.Content.Buffs
{

	public class PrismAura2 : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Prismatic Aura"); // Buff display name
			// Description.SetDefault("You have an protective aura around you"); // Buff description
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {

            player.GetModPlayer<BuffPlayer>().aura = true;
        }

        
    }

    
}
