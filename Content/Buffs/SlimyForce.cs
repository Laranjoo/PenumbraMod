using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    public class SlimyForce : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(ModContent.GetInstance<ReaperClass>()).Flat += 3;
            player.jumpSpeedBoost += 1.5f;
        }
    }
}
