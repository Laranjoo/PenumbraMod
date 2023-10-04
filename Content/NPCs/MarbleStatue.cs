using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using PenumbraMod.Content.Biomes.Vanilla;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace PenumbraMod.Content.NPCs
{
	public class MarbleStatue : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailCacheLength[NPC.type] = 7;//How many copies of shadow/trail
            NPCID.Sets.TrailingMode[NPC.type] = 1;
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
			NPC.width = 30;
			NPC.height = 48;
			NPC.damage = 22;
			NPC.defense = 8;
			NPC.lifeMax = 140;
			NPC.HitSound = SoundID.NPCHit41;
			NPC.DeathSound = SoundID.NPCDeath43;
			NPC.value = Item.buyPrice(silver: 6, copper: 5);
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.noGravity = false;
		}    
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Marble, 1, 5, 15));
		}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<MarbleBiomeChange>()))
            {
                return SpawnCondition.Cavern.Chance * 0.9f;
            }
            return 0f;
        }
		bool activateai;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            Player player = Main.player[NPC.target];
			if (player.Distance(NPC.Center) < 170f)
			{
				activateai = true;
                NPC.dontTakeDamage = false;
            }
			if (activateai)
			{
				Jumping(player);
			}
			else
                NPC.dontTakeDamage = true;
            if (player.dead)
				activateai = false;
        }
		void Jumping(Player player)
		{
			Lighting.AddLight(NPC.Center, Color.Red.ToVector3() * 0.4f);
			NPC.ai[0]++;
			if (NPC.ai[0] <= 2)
				SoundEngine.PlaySound(SoundID.NPCHit52, NPC.Center);
			if (NPC.ai[0] == 30)
			{
				if (NPC.velocity.Y == 0f)
				{
					for (int i = 0; i < 20; i++)
						Dust.NewDust(NPC.Center + new Vector2(0, 25), 20, 2, DustID.Marble, 0f, 0f);
                    NPC.velocity = NPC.DirectionTo(player.Center) * 0.1f + new Vector2(NPC.direction * 3, -6f);
                    NPC.TargetClosest(true);
                }
                NPC.netUpdate = true;
			}
			if (NPC.ai[0] >= 31 && NPC.ai[0] <= 40 && NPC.velocity.X != 1)
				NPC.velocity = NPC.DirectionTo(player.Center) * 0.1f + new Vector2(NPC.direction * 3, -6f);
			if (NPC.velocity.Y == 0f)
			{
				if (NPC.ai[0] >= 60)
					NPC.ai[0] = 10;
                NPC.velocity.X = 0f;
            }
				
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/MarbleStatueActivated").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                Color color = Color.Red * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                color.A = 0;
                if (activateai)
                    Main.EntitySpriteDraw(texture, drawPos, null, color, NPC.oldRot[k], drawOrigin, NPC.scale, SpriteEffects.None, 0);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),

               BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,
            }); ;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{

			if (NPC.life <= 0)
			{
				Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Marble, 1f, 1f);
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				int backGoreType = Mod.Find<ModGore>("MarbleStatueGore").Type;
				int backGoreType2 = Mod.Find<ModGore>("MarbleStatueGore2").Type;
				int frontGoreType = Mod.Find<ModGore>("MarbleStatueGore3").Type;
                int frontGoreType2 = Mod.Find<ModGore>("MarbleStatueGore4").Type;
                var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 1; i++)
				{
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType2);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), backGoreType);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), frontGoreType);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-3, 5), Main.rand.Next(-3, 5)), frontGoreType2);
                }


			}

		}
	}
}
