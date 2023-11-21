using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Common.Players;
using PenumbraMod.Content.Buffs;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.Audio;

namespace PenumbraMod.Content.Items.Armors
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class MarshmellowHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Increases damage by 2%"
                + "\n'Its so fluffy!'"); */

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
            Item.value = Item.sellPrice(silver: 1); // How many coins the item is worth
            Item.rare = ItemRarityID.Blue; // The rarity of the item
            Item.defense = 2; // The amount of defense the item will give when equipped

        }

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MarshmellowBody>() && legs.type == ModContent.ItemType<MarshmellowLeggings>();
        }

        // UpdateArmorSet allows you to give set bonuses to the armor.

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.02f;
            player.GetModPlayer<MarshmellowArmorKeybind>().setBonus = true;
        }



        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = (string)PenumbraLocalization.MarshArmorNoKeybind;
            foreach (string key in MarshmellowKeybindSystem.MarshmellowArmorKeybind.GetAssignedKeys())
            {
                player.setBonus = this.GetLocalization("SetBonus").Format(key);
            }

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Consumables.Marshmellow>(), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public class MarshmellowArmorKeybind : ModPlayer
        {
            public bool setBonus = false;
            public int setBonusTimer;
            public override void ResetEffects()
            {
                setBonus = false;
            }
            public override void ProcessTriggers(TriggersSet triggersSet)
            {
                if (MarshmellowKeybindSystem.MarshmellowArmorKeybind.JustPressed && Player.active)
                {
                    if (setBonus)
                    {
                        SoundEngine.PlaySound(SoundID.Item37, Player.position);
                        Player.GetModPlayer<BuffPlayer>().MarshmellowEffect = true;

                        if (Player.HasBuff(ModContent.BuffType<NotProtected>()))
                        {

                        }

                        else
                        {
                            Player.AddBuff(ModContent.BuffType<ComfortablyProtected>(), 10 * 60);
                            Player.AddBuff(ModContent.BuffType<NotProtected>(), 70 * 60);
                        }
                    }



                }
            }

        }
    }
}
