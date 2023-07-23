using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class AmethystForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HeldItem.DamageType == ModContent.GetInstance<ReaperClass>())
                player.HeldItem.scale = 1.15f;
            else
            {

            }
        }

    }

    
}
