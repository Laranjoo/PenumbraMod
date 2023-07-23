using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PenumbraMod.Common.DropConditions;
using System.Linq;
using PenumbraMod.Content.Items.Consumables;
using PenumbraMod.Content.Items;
using PenumbraMod.Content.Items.Accessories;
using PenumbraMod.Content.NPCs.Bosses;
using PenumbraMod.Content.Items.Armors;

namespace PenumbraMod.Common.GlobalNPCs
{
	
	public class NPCDROPCHANGES : GlobalNPC
	{
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
            #region DropConditions
            if (npc.type == NPCID.PirateCaptain)
            {
                TerroristKnifeDropCondition exampleDropCondition = new TerroristKnifeDropCondition();
                IItemDropRule conditionalRule = new LeadingConditionRule(exampleDropCondition);
                int itemType = ModContent.ItemType<TerroristsKnife>();
                if (npc.type == NPCID.PirateCaptain)
                {
                    itemType = ModContent.ItemType<TerroristsKnife>();
                }
                IItemDropRule rule = ItemDropRule.Common(itemType, chanceDenominator: 9);
                conditionalRule.OnSuccess(rule);
                npcLoot.Add(conditionalRule);
            }
            if (npc.type == NPCID.Harpy)
            {
                StrawberryDropCondition exampleDropCondition = new StrawberryDropCondition();
                IItemDropRule conditionalRule = new LeadingConditionRule(exampleDropCondition);
                int itemType = ModContent.ItemType<MysticalStrawberry>();
                if (npc.type == NPCID.Harpy)
                {
                    itemType = ModContent.ItemType<MysticalStrawberry>();
                }
                IItemDropRule rule = ItemDropRule.Common(itemType, chanceDenominator: 9);
                conditionalRule.OnSuccess(rule);
                npcLoot.Add(conditionalRule);
            }
            if (npc.type == NPCID.Bunny)
            {
                bunnypaw exampleDropCondition = new bunnypaw();
                IItemDropRule conditionalRule = new LeadingConditionRule(exampleDropCondition);
                int itemType = ModContent.ItemType<BunnysPaw>();
                if (npc.type == NPCID.Bunny)
                {
                    itemType = ModContent.ItemType<BunnysPaw>();
                }
                IItemDropRule rule = ItemDropRule.Common(itemType, chanceDenominator: 17);
                conditionalRule.OnSuccess(rule);
                npcLoot.Add(conditionalRule);
            }
            #endregion
            #region DropChanges
            if (npc.type == NPCID.KingSlime)
            { 
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<InconsistentJelly>(), 1, 5, 10));
            }
            if (Main.expertMode)
            {
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodyHibiscus>(), 1, 1, 1));
                }
            }
            if (npc.type == NPCID.BloodNautilus)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodEelBow>(), 3, 1));
            if (npc.type == NPCID.SkeletronHead)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Witcherster>(), 6, 1));
            }
            if (npc.type == NPCID.DesertScorpionWall)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ScorpionShell>(), 2, 1));
            }
            if (npc.type == NPCID.DesertScorpionWalk)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ScorpionShell>(), 2, 1));
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrokenWoodyThing>(), 2, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ReaperEmblem>(), 5, 1));
            }
            #region OldBloodyStoneArmorSpamCodeBecauseAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            // I hate this
            if (npc.type == NPCID.FaceMonster)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 525, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 525, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 525, 1));
            }
            if (npc.type == NPCID.BloodCrawlerWall)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 525, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 525, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 525, 1));
            }
            if (npc.type == NPCID.BloodMummy)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 525, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 525, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 525, 1));
            }
            if (npc.type == NPCID.BloodJelly)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.BloodFeeder)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.IchorSticker)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.BloodCrawler)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.Crimera)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.Crimslime)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.Herpling)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.FloatyGross)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.CrimsonAxe)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.BigMimicCrimson)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.BigCrimera)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.DesertGhoulCrimson)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            if (npc.type == NPCID.PigronCrimson)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneBreastplate>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneHelmet>(), 20, 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBloodystoneLeggings>(), 20, 1));
            }
            #endregion
            #endregion
        }
    }
}
