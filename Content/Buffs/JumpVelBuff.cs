using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class JumpVelBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Frog Force"); // Buff display name
            // Description.SetDefault("Your movement speed and jump are increased");
		}
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.5f;
            player.jumpSpeedBoost += 1.6f;
        }


    }

    
}
