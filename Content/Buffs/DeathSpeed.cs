using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Content.DamageClasses;
using Microsoft.Xna.Framework.Graphics;

namespace PenumbraMod.Content.Buffs
{
    class DeathSpeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Death Speed"); // Buff display name
            /* Description.SetDefault("You feel like the scythe is very light"
                + "\nScythe speed increased by 85%"); */ // Buff description
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed<ReaperClass>() += 0.85f;
            player.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            return false;
        }

    }
    class IceSpeed : ModBuff
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ice Speed"); // Buff display name
            /* Description.SetDefault("You feel like the scythe is very light"
                + "\nScythe speed increased by 50%"); */ // Buff description
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed<ReaperClass>() += 0.50f;
        }


    }


}
