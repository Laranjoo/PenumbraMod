using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Common.Base;
using Microsoft.Xna.Framework.Graphics;

namespace PenumbraMod.Content.Buffs
{

	public class MeltedCooldown : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Melted Cooldown"); // Buff display name
			// Description.SetDefault(""); // Buff description
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            if (Main.LocalPlayer.HasBuff<MeltedForce>())
                return false;
            return true;
        }

    }
    public class MeltedEx : ModBuff
    {
        public override string Texture => "PenumbraMod/EMPTY";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Explosion"); // Buff display name
            // Description.SetDefault(""); // Buff description
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;

        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().MeltedEx = true;
        }

    }


}
