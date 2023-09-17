using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    public class PrettyBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().heartbuff = true;
        }
    }
}
