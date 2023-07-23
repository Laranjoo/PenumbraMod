using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.ModLoader.IO;
using PenumbraMod.Content.Items.Consumables;

namespace PenumbraMod.Content.NPCs
{
    public class GraveyardBiome : ModBiome
    {
        // Calculate when the biome is active.
        public override bool IsBiomeActive(Player player)
        {
            bool b1 = Main.LocalPlayer.ZoneGraveyard;

            // Second, we will limit this biome to the inner horizontal third of the map as our second custom condition
            bool b2 = Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;

            // Finally, we will limit the height at which this biome can be active to above ground (ie sky and surface). Most (if not all) surface biomes will use this condition.
            bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;
            return b1 && b2 && b3;
        }
    }
    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    [AutoloadHead]
	public class vhcmnpc : ModNPC
	{
		public int NumberOfTimesTalkedTo = 0;
        public const string ShopName = "Shop";
        public override void SetStaticDefaults() {
			// DisplayName automatically assigned from localization files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Vhcm");
			Main.npcFrameCount[Type] = 26; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 400; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				.SetBiomeAffection<GraveyardBiome>(AffectionLevel.Love)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love) // Loves living near the Steampunker.
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Likes living near the guide.
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Hate)
                .SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike)// Dislikes living near the party girl.
			; // < Mind the semicolon!
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("He came from a misterious city... we dont know why he likes living in dark places, he likes dark vibes and rain, such strange..."),

			});
		}
		public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */ { // Requirements for the town NPC to spawn.
			for (int k = 0; k < 255; k++) {
				Player player = Main.player[k];
				if (!player.active) {
					continue;
				}

				if (player.InModBiome<GraveyardBiome>()) {
					return true;
				}
			}

			return false;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return new ExamplePersonProfile();
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Vhcm",
				"Roctiv",
				"Nott",
				"Otten"
			};
		}

		public override void FindFrame(int frameHeight) {
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

            // These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Mods.PenumbraMod.Dialogue.vhcmnpc.Standart"));
            chat.Add(Language.GetTextValue("Mods.PenumbraMod.Dialogue.vhcmnpc.Standart2"));
            chat.Add(Language.GetTextValue("Mods.PenumbraMod.Dialogue.vhcmnpc.Standart3"));
            if (!NPC.homeless)
            {
                if (Main.raining && Main.LocalPlayer.ZoneOverworldHeight)
                    chat.Add(Language.GetTextValue("Mods.PenumbraMod.Dialogue.vhcmnpc.Rainoutside"));

            }
            else
            {
                if (Main.raining && Main.LocalPlayer.ZoneOverworldHeight)
                    chat.Add(Language.GetTextValue("Mods.PenumbraMod.Dialogue.vhcmnpc.Rainoutsidehome"));
            }
            NumberOfTimesTalkedTo++;
			if (NumberOfTimesTalkedTo >= 10) {
                chat.Add(Language.GetTextValue("Mods.PenumbraMod.Dialogue.vhcmnpc.TalkALot"));
            }

			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
		}

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton) {
				shop = ShopName;
			}
		}
		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName)
	            .Add(ItemID.Tombstone)
				.Add(ItemID.AngelStatue)
				.Add(ItemID.LesserHealingPotion)
				.Add<JumpVelPotion>();
			npcShop.Register();
		}
        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            foreach (Item item in items)
            {
                // Skip 'air' items and null items.
                if (item == null || item.type == ItemID.None)
                {
                    continue;
                }
            }
        }
       
        public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<vhcmknife>()));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered.
		public override bool CanGoToStatue(bool toKingStatue) => false;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<knifeproj>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 8f;
            randomOffset = 2f;
        }

        public override void LoadData(TagCompound tag) {
			NumberOfTimesTalkedTo = tag.GetInt("numberOfTimesTalkedTo");
		}

		public override void SaveData(TagCompound tag) {
			tag["numberOfTimesTalkedTo"] = NumberOfTimesTalkedTo;
		}
	}

	public class ExamplePersonProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc) {
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				return ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/vhcmnpc");

			return ModContent.Request<Texture2D>("PenumbraMod/Content/NPCs/vhcmnpc");
		}

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("PenumbraMod/Content/NPCs/vhcmnpc_Head");
	}
}