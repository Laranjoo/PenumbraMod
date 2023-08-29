using Terraria;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    public class TungstenShield : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 4;
        }

    }
}
