using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Biomes;

namespace PenumbraMod.Content.NPCs
{
	public class Oni : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 8;
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
			NPC.width = 40;
			NPC.height = 58;
			NPC.damage = 22;
			NPC.defense = 8;
			NPC.lifeMax = 40;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(silver: 2, copper: 10);
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Flying;
			AIType = NPCID.Crimera;
			NPC.noGravity = true;
            SpawnModBiomes = new int[] { ModContent.GetInstance<AkuBiome>().Type };
        }
        public override void FindFrame(int frameHeight)
        {
            // This NPC animates with a simple "go from start frame to final frame, and loop back to start frame" rule
            // In this case: First stage: 0-1-2-0-1-2, Second stage: 3-4-5-3-4-5, 5 being "total frame count - 1"
            int startFrame = 0;
            int finalFrame = 7;
            int frameSpeed = 5;
            NPC.frameCounter += 0.9f;
            if (NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= finalFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
        /*
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Obsidian, 1, 3, 7));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MeltedEmber>(), 1, 1, 4));
		}
		*/
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<AkuBiome>()))
            {
                return SpawnCondition.Overworld.Chance * 0.9f;
            }
            return 0f;
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),

                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<AkuBiome>().ModBiomeBestiaryInfoElement),
            }); ;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{

			if (NPC.life <= 0)
			{
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				int backGoreType = Mod.Find<ModGore>("OniGore").Type;
				int backGoreType2 = Mod.Find<ModGore>("OniGore2").Type;
				int frontGoreType = Mod.Find<ModGore>("OniGore3").Type;
                int frontGoreType2 = Mod.Find<ModGore>("OniGore4").Type;
                int frontGoreType3 = Mod.Find<ModGore>("OniGore5").Type;
                var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 1; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType2);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), frontGoreType);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), frontGoreType2);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), frontGoreType3);
                }


			}

		}
	}
}
