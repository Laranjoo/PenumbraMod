using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Armors;
using PenumbraMod.Content.Items.Placeable;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace PenumbraMod.Content.NPCs
{
    public class BloodystoneSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                Velocity = 0.1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 56;
            NPC.height = 42;
            NPC.damage = 66;
            NPC.defense = 24;
            NPC.lifeMax = 240;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(0, 0, 4, 0);
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = 1; // Slime AI, important to choose the aiStyle that matches the NPCID that we want to mimic
            AIType = NPCID.Crimslime;
            AnimationType = NPCID.CorruptSlime;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 2, 4));
            npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 5, 9));
            npcLoot.Add(ItemDropRule.Common(5091, 1000, 1));
            npcLoot.Add(ItemDropRule.Common(ItemID.MeatGrinder, 200, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodystoneOre>(), 1, 4, 12));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 525, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 525, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 525, 1));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(BuffID.Darkness, 15 * 60);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return SpawnCondition.Crimson.Chance * 0.5f;
            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),


                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson
            });
        }
    }
}
