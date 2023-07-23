using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

namespace PenumbraMod.Content.Items
{
    public class BrightnessLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brighness Lantern"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("AHHHHHHHHHH ITS SO BRIGHT" +
                "\nIT SHOULD KILL YOUR EYES!" +
                "\nYOU CANT EVEN SEE YOURSELF!!!"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 31;
            Item.height = 32;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8;
            Item.value = 25600;
            Item.rare = 8;
            Item.UseSound = SoundID.Item66;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BrightnessFULL>();
            Item.shootSpeed = 16f;

        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BrightnessPlayer>()] < 1)
            {//Equip animation.
                int index = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<BrightnessPlayer>(), 0, 0, player.whoAmI, 0f);
            }
            
        }
        public class BrightnessFULL : ModProjectile
        {

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("BRIGHTNESS");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            }

            public override void SetDefaults()
            {
                Projectile.damage = 60;
                Projectile.knockBack = 6f;
                Projectile.width = 384;
                Projectile.height = 384;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.timeLeft = 600;
                Projectile.light = 4f;
            }
            public override Color? GetAlpha(Color lightColor)
            {
                // return Color.White;
                return new Color(255, 249, 82, 0) * Projectile.Opacity;
            }
            public override void AI()
            {

                Projectile.rotation = Projectile.velocity.ToRotation();
                // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
                if (Projectile.spriteDirection == -1)
                {
                    Projectile.rotation += MathHelper.Pi;
                    // For vertical sprites use MathHelper.PiOver2
                }
            }
            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }

                return true;
            }


        }
        public class BrightnessPlayer : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Brightness");
                Main.projFrames[Projectile.type] = 1;
            }
            public override void SetDefaults()
            {

                AIType = 0;

                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.timeLeft = 60;
                Projectile.penetrate = -1;
                Projectile.light = 0.3f;
                Projectile.hide = false;
                Projectile.alpha = 0;
                Projectile.netImportant = true;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
            }
            
            bool firstSpawn = true;
            int newOffsetY;
            float spawnProgress;
            bool dustSpawn = true;

            float rotationStrength = 0.1f;
            double deg;

            bool startSound = true;
            bool endSound = false;

            public override Color? GetAlpha(Color lightColor)
            {
                return new Color(255, 255, 0, 0) * Projectile.Opacity;
            }
            public override bool PreAI()
            {
                Player player = Main.player[Projectile.owner];
                return true;
            }
            public override void AI()
            {
                Player projOwner = Main.player[Projectile.owner];
                projOwner.heldProj = Projectile.whoAmI;

                if (firstSpawn)
                {

                    firstSpawn = false;
                }
                projOwner.opacityForAnimation = 0f;
                if (Projectile.spriteDirection == 1)
                {//Adjust when facing the other direction

                    Projectile.ai[1] = 280;
                }
                else
                {

                    Projectile.ai[1] = 280;
                }
                deg = Projectile.ai[1];
                double rad = deg * (Math.PI / 180);
                double dist = 10;

                Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
                Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
                if (Projectile.spriteDirection == 1)
                {//Adjust when facing the other direction

                    Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(180f);
                }
                else
                {

                    Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(180f);
                }
                //Projectile.Center += projOwner.gfxOffY * Vector2.UnitY;//Prevent glitchy animation.
                Projectile.timeLeft = 10;
                Projectile.ai[0] += 1f;

                if (projOwner.dead && !projOwner.active)
                {//Disappear when player dies
                    projOwner.opacityForAnimation = 1f;
                    Projectile.Kill();
                }
                if (projOwner.HeldItem?.type != ModContent.ItemType<BrightnessLantern>())
                {
                    projOwner.opacityForAnimation = 1f;
                    Projectile.Kill();
                }
            
                if (projOwner.ownedProjectileCounts[ModContent.ProjectileType<BrightnessFULL>()] >= 1)
                {

                    Projectile.frame = 0;
                }
                else
                {
                    //Arms will hold the weapon.
                    projOwner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.None, (projOwner.Center -
                        new Vector2(Projectile.Center.X + (projOwner.velocity.X * 0.05f), Projectile.Center.Y + (projOwner.velocity.Y * 0.05f))
                        ).ToRotation() + MathHelper.PiOver2);
                    Projectile.frame = 0;
                }

                //Orient projectile
                Projectile.direction = projOwner.direction;
                Projectile.spriteDirection = Projectile.direction;
              
            }
           
        }  
    }
}