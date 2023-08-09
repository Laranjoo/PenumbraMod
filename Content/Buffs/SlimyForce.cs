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
            player.GetDamage(ModContent.GetInstance<ReaperClass>()).Flat += 1;
            player.GetAttackSpeed(ModContent.GetInstance<ReaperClass>()) += 0.10f;
        }

    }
    public class SlimyForce2 : ModBuff
    {
        public override string Texture => "PenumbraMod/Content/Buffs/SlimyForce";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(ModContent.GetInstance<ReaperClass>()).Flat += 3;
            player.GetAttackSpeed(ModContent.GetInstance<ReaperClass>()) += 0.15f;
        }

    }
}
