using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class TinScythe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tin Scythe"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("May be useful" +
                "\n[c/817d5d:Special ability:] When used, it spawns tin trash to rotate around the player, no more than 5 can be shot"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.width = 44;
            Item.height = 36;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 5570;
            Item.rare = 0;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 2f;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.noUseGraphic = true;
                Item.noMelee = true;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.noUseGraphic = false;
                Item.noMelee = false;
            }
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()) && player.ownedProjectileCounts[ModContent.ProjectileType<TinTrash>()] < 4)
            {
                const int NumProjectiles = 1;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    SoundEngine.PlaySound(SoundID.Item71, player.position);
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(50));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 1f - Main.rand.NextFloat(0f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<TinTrash>(), damage, knockback, player.whoAmI);

                }

            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.TinBar, 15)
            .AddIngredient(ItemID.Wood, 7)
            .AddTile(TileID.Anvils)
            .Register();
        }
        public class TinTrash : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Tin Trash"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.damage = 12;
                Projectile.width = 24;
                Projectile.height = 22;
                Projectile.aiStyle = 908;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = 4;
                Projectile.timeLeft = 1243500;
                Projectile.ignoreWater = false;
                Projectile.tileCollide = false;
                AIType = ProjectileID.TitaniumStormShard;
                Projectile.rotation += 0.3f;
                Projectile.DamageType = ModContent.GetInstance<ReaperClass>();
            }

            #region Circular Motion
            //
            public override void AI()
            {
                Player player = Main.player[Projectile.owner];
                Projectile.rotation = Projectile.AngleTo(player.Center);
                double deg = (double)Projectile.ai[1] * 5;
                double rad = deg * (Math.PI / 180);
                double dist = 50;
                Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
                Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
                Projectile.ai[1] += 1f;
            }
            //
            #endregion 
            public static float easeInOutQuad(float x)
            {
                return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
            }
            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/TinTrashef").Value;
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    if (Projectile.oldPos[k] == Vector2.Zero)
                        return false;
                    for (float j = 0; j < 1; j += 0.0625f)
                    {
                        Vector2 lerpedPos;
                        if (k > 0)
                            lerpedPos = Vector2.Lerp(Projectile.oldPos[k - 1], Projectile.oldPos[k], easeInOutQuad(j));
                        else
                            lerpedPos = Vector2.Lerp(Projectile.position, Projectile.oldPos[k], easeInOutQuad(j));
                        float lerpedAngle;
                        if (k > 0)
                            lerpedAngle = Utils.AngleLerp(Projectile.oldRot[k - 1], Projectile.oldRot[k], easeInOutQuad(j));
                        else
                            lerpedAngle = Utils.AngleLerp(Projectile.rotation, Projectile.oldRot[k], easeInOutQuad(j));
                        lerpedPos += Projectile.Size / 2;
                        lerpedPos -= Main.screenPosition;
                        Main.EntitySpriteDraw(texture, lerpedPos, null, Color.White * 0.2f * (1 - ((float)k / (float)Projectile.oldPos.Length)), lerpedAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                    }
                }


                return true;
            }
            public override void OnKill(int timeLeft)
            {
                // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
        }
    }
}