using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Buffs;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using static Terraria.ModLoader.PlayerDrawLayer;
using PenumbraMod.Content.Items.Accessories;
using System;

namespace PenumbraMod.Content.Items.Armors
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class PrismHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Critical strike chance increased by 10%" +
                "\nReaper damage increased by 10%"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // If your head equipment should draw hair while drawn, use one of the following:
            // ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
            // ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
            // ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
            // ArmorIDs.Head.Sets.DrawBackHair[Item.headSlot] = true;
            // ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true; 

        }

        public override void SetDefaults()
        {
            Item.width = 18; // Width of the item
            Item.height = 18; // Height of the item
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
            Item.rare = ItemRarityID.LightPurple; // The rarity of the item
            Item.defense = 11; // The amount of defense the item will give when equipped

        }
        
       
        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PrismArmor>() && legs.type == ModContent.ItemType<PrismLeggings>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 0.15f;
            player.GetDamage<ReaperClass>() += 0.10f;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<Eyeglow>()] < 1 && player.ownedProjectileCounts[ModContent.ProjectileType<PrismAura>()] < 1;
        }
        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)
        {
            if (Main.dayTime && player.ownedProjectileCounts[ModContent.ProjectileType<PrismAura>()] < 1 && player.ownedProjectileCounts[ModContent.ProjectileType<Eyeglow>()] < 1 && !player.ZoneDirtLayerHeight)
            {
                player.statDefense += 3;
                player.GetDamage(DamageClass.Generic) += 0.05f;
                player.AddBuff(ModContent.BuffType<PrismAura2>(), 90000);
                player.ClearBuff(ModContent.BuffType<PrismThorns>());
                if (Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) > 1f && !player.rocketFrame)
                {
                     int NumProjectiles = 1;
                     for (int i = 0; i < NumProjectiles; i++)
                     {
                          int v = Projectile.NewProjectile(player.GetSource_Accessory(Item), new Vector2(player.position.X + Main.rand.NextFloat(player.width), player.position.Y + Main.rand.NextFloat(player.height)), new Vector2(0f, 0f), ModContent.ProjectileType<PrismAura>(), 45, 0, Main.myPlayer);
                     }
                     int NumProjectiles2 = 1;
                     for (int i = 0; i < NumProjectiles2; i++)
                     {
                           int v = Projectile.NewProjectile(player.GetSource_Accessory(Item), new Vector2(player.position.X + Main.rand.NextFloat(player.width), player.position.Y + Main.rand.NextFloat(player.height)), new Vector2(-8f, -22f), ModContent.ProjectileType<Eyeglow>(), 0, 0, Main.myPlayer);
                     }

                }
                
            }
            if (!Main.dayTime)
            {
                player.AddBuff(ModContent.BuffType<PrismThorns>(), 90000);
                player.GetDamage(DamageClass.Generic) -= 0.05f;
                player.ClearBuff(ModContent.BuffType<PrismAura2>());
            }
            player.setBonus = "Grants the ability to change form during day/night\n" +
                "When at daytime, your defense is increased by 3, and you have an damaging aura to hurt enemies (Doesn't work on underground), and deal more damage\n" +
                "The aura inflict prismatic lightining on enemies, causing extra damage on them, and increases reaper energy for you\n" +
                "When at nighttime, you do not gain extra defense, and you deal less damage, but when you get hit, enemies also get hit\n" +
                "[c/b74bda:'By the empress soul...']";

        }
    }
   
 
}

