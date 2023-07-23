using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Common.Base;

namespace PenumbraMod.Content.Buffs
{

	public class LowVoltage : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Low Voltage"); // Buff display name
			// Description.SetDefault("You are recieving low voltage energy!"); // Buff description
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().l = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().l = true;
        }


    }
    public class MediumVoltage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Medium Voltage"); // Buff display name
            // Description.SetDefault("You are recieving medium voltage energy!"); // Buff description
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().l2 = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().l2 = true;
        }


    }
    public class HighVoltage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("High Voltage"); // Buff display name
            // Description.SetDefault("You are recieving high voltage energy!"); // Buff description
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BuffPlayer>().l3 = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.PenumbraNPCBuff().l3 = true;
        }


    }

}
