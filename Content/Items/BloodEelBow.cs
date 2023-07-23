using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class BloodEelBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 61;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = 5;
            Item.knockBack = 6;
            Item.value = 24000;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = 1;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 18f;
            Item.noMelee = true;
            Item.crit = 25;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
                type = ModContent.ProjectileType<BloodEelProjHead>();

            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 31f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }

    }
    public class BloodEelProjHead : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 64;
            Projectile.width = 20;
            Projectile.height = 28;
            Projectile.knockBack = 6;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.25f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
    }
}