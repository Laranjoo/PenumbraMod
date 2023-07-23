using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Common.Players;
using static PenumbraMod.Content.Items.Armors.MarshmellowHelm;
using PenumbraMod.Content.Buffs;
using Terraria.GameInput;
using Terraria.Audio;

namespace PenumbraMod.Content.Items.Armors
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class MeltedHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Increases damage by 3%");

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
            Item.rare = ItemRarityID.LightRed; // The rarity of the item
            Item.defense = 8; // The amount of defense the item will give when equipped

        }

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MeltedBreastplate>() && legs.type == ModContent.ItemType<MeltedLeggings>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.GetModPlayer<MeltedKeybind>().setBonus = true;

        }
        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = "[c/cccccc:Select a keybind for [Melted Armor Ability] in Controls]";
            foreach (string key in MeltedArmorSys.MeltedArmorKeybind.GetAssignedKeys())
            {

                player.setBonus = "[c/ff0000:Press " + key + " To increase your movement speed by 15%]\n" +
                    "[c/ff0000:Increase your reaper attack speed by 25% and damage by 5%, when you get hit, you explode, dealing damage on enemies]\n" +
                    "[c/ff0000:This effect has 30 seconds and has a 60 seconds cooldown]\n" +
                    "[c/ff5b00:'Why obsidian armor when you have this?']";
            }

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 15);
            recipe.AddIngredient(ItemID.Obsidian, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
    public class MeltedKeybind : ModPlayer
    {
        public bool setBonus = false;
        public int setBonusTimer;
        public override void ResetEffects()
        {
            setBonus = false;
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (MeltedArmorSys.MeltedArmorKeybind.JustPressed && Player.active)
            {
                if (setBonus)
                {
                    SoundEngine.PlaySound(SoundID.Item45, Player.position);
                    Player.GetModPlayer<BuffPlayer>().MeltedArmor = true;

                    if (Player.HasBuff(ModContent.BuffType<MeltedCooldown>()))
                    {

                    }

                    else
                    {
                        Player.AddBuff(ModContent.BuffType<MeltedForce>(), 1800);
                        Player.AddBuff(ModContent.BuffType<MeltedCooldown>(), 5400);
                    }
                }



            }
        }
    }

}
