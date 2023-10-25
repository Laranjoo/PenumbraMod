using Microsoft.Xna.Framework;
using PenumbraMod.Common.Base;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.NPCs.Bosses.Eyestorm;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Buffs
{
    // This class serves as an example of a debuff that causes constant loss of life
    // See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
    public class SpikeStunned : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spike Stunned"); // Buff display name
            // Description.SetDefault("The spíkes hurt..."); // Buff description
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        // Allows you to make this buff give certain effects to the given player
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().Vines = true;
        }
    }
    public class hitef : ModBuff
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<PenumbraConfig>().HitEffect;
        }
        public override string Texture => "PenumbraMod/Content/Buffs/DeathSpeed";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        // Allows you to make this buff give certain effects to the given player
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().hiteff = true;
        }
    }
}
