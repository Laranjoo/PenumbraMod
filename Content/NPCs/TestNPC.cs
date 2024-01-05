using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace PenumbraMod.Content.NPCs
{
	public class TestNPC : ModNPC
	{
        public override string Texture => "PenumbraMod/icon_small";
        public override void SetStaticDefaults()
		{
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{ // Influences how the NPC looks in the Bestiary
				Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

		public override void SetDefaults()
		{
			NPC.width = 50;
			NPC.height = 50;
			NPC.defense = 9999999;
			NPC.lifeMax = 9999999;
			NPC.value = Item.buyPrice(silver: 6, copper: 5);
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
		}    
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            return 0f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        	// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Serves as a dummy for getting hit"),

               BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
            }); ;
		}
	}
}
