using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.DamageClasses;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{

    public class PerishedForce : ModBuff
	{
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(ModContent.GetInstance<ReaperClass>()).Flat += 0.03f;
        }

    }
    public class PerishedForceCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<PerishedForce>()))
            return false;
            return true;
        }
    }

}
