using Microsoft.Xna.Framework;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Consumables;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TungstenScythe : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 52;
            Item.height = 42;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 5920;
            Item.rare = 0;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 0f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                player.AddBuff(ModContent.BuffType<TungstenShield>(), 60 * 60);
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.TungstenBar, 14)
            .AddIngredient(ItemID.Wood, 9)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}