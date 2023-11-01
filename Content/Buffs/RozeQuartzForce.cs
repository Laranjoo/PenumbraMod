using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class RozeQuartzForce : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().liferegen = true;
        }
    }
}
