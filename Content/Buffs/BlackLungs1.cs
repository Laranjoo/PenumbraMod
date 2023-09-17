using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class BlackLungs1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Black Lungs");
            // Description.SetDefault("You've been smoking a lot!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;

        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().lifeRegenDebuff = true;

        }
    }
}
