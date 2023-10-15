using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace PenumbraMod.Content.Buffs
{

	public class NotProtected : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Comfortably Protected Cooldown"); // Buff display name
			// Description.SetDefault(""); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
			if (Main.LocalPlayer.HasBuff<ComfortablyProtected>())
				return false;
			return true;
        }
    }
}
