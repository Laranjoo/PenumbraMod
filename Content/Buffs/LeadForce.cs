using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace PenumbraMod.Content.Buffs
{

	public class LeadForce : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Lead Force"); // Buff display name
			/* Description.SetDefault("'You feel much resistent!'"
                + "\nDamage reduction increased by 45% until next attack"); */ // Buff description
            Main.buffNoTimeDisplay[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.45f;
            player.GetModPlayer<BuffPlayer>().Leadforce = true;
        }


    }

    
}
