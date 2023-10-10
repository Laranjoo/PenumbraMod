using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.NPCs
{
	// Party Zombie is a pretty basic clone of a vanilla NPC. To learn how to further adapt vanilla NPC behaviors, see https://github.com/tModLoader/tModLoader/wiki/Advanced-Vanilla-Code-Adaption#example-npc-npc-clone-with-modified-projectile-hoplite
	public class MarshmellowSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Marshmellow Slime");

			Main.npcFrameCount[Type] = 2;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{ // Influences how the NPC looks in the Bestiary
				Velocity = 0.1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 5;
			NPC.defense = 2;
			NPC.lifeMax = 50;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 320f;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = 1; // Slime AI, important to choose the aiStyle that matches the NPCID that we want to mimic
			AIType = NPCID.BlueSlime; // Use vanilla zombie's type when executing AI code. (This also means it will try to despawn during daytime)
			AnimationType = NPCID.BlueSlime; // Use vanilla zombie's type when executing animation code. Important to also match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
			Banner = Item.NPCtoBanner(NPCID.BlueSlime);
			BannerItem = Item.BannerToItem(Banner); // Makes kills of this NPC go towards dropping the banner it's associated with.

		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Marshmellow>(), 1, 1, 4));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.OverworldDay.Chance * 0.5f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Fluffy slime that loves to jump over the world."),


				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{

			if (NPC.life <= 0)
			{
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				int backGoreType = Mod.Find<ModGore>("marshslimegore").Type;
				int backGoreType2 = Mod.Find<ModGore>("marshslimegore2").Type;
				int frontGoreType = Mod.Find<ModGore>("marshslimegore3").Type;

				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 1; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType2);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), frontGoreType);
				}


			}

		}
	}
}
