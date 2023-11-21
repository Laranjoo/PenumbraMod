using PenumbraMod.Common.Players;
using PenumbraMod.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Armors
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class MeltedHelmet : ModItem
    {
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
            player.setBonus = (string)PenumbraLocalization.MeltedArmorNoKeybind;
            foreach (string key in MeltedArmorSys.MeltedArmorKeybind.GetAssignedKeys())
            {
                player.setBonus = this.GetLocalization("SetBonus").Format(key);
            }

        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 15);
            recipe.AddIngredient(ItemID.Obsidian, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddIngredient(ItemID.ObsidianSkull);
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
                        Player.AddBuff(ModContent.BuffType<MeltedForce>(), 15 * 60);
                        Player.AddBuff(ModContent.BuffType<MeltedCooldown>(), 75 * 60);
                    }
                }



            }
        }
    }

}
