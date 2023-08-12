using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.Items.Consumables;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class CorrosionCannon : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 87;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 78;
			Item.height = 28;
			Item.value = 15000;
			Item.rare = ItemRarityID.Lime;
			Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CorrosionCannonProj>();
            Item.useTime = 4;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.5f;
            Item.shootSpeed = 18f; // Adjusts how far away from the player to hold the projectile
            Item.noMelee = true; // Turns off damage from the item itself, as we have a projectile
            Item.noUseGraphic = true; // Stops the item from drawing in your hands, for the aforementioned reason
            Item.channel = true; // Important as the projectile checks if the player channels
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 13)
                 .AddIngredient(ModContent.ItemType<CorrodedPlating>(), 9)

               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CorrosionCannonProj : ModProjectile
    {
        public override string Texture => "PenumbraMod/Content/Items/CorrosionCannon";
        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 28;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = -1; // Replace with 20 if you do not want custom code
            Projectile.hide = true; // Hides the projectile, so it will draw in the player's hand when we set the player's heldProj to this one.
        }

        // This code is adapted and simplified from aiStyle 20 to use a different dust and more noises. If you want to use aiStyle 20, you do not need to do any of this.
        // It should be noted that this projectile has no effect on mining and is mostly visual.
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.timeLeft = 60;
            Projectile.ai[1]++;
            Projectile.ai[2]++;
            // Animation code could go here if the projectile was animated. 

            // Plays a sound every 20 ticks. In aiStyle 20, soundDelay is set to 30 ticks.


            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                // This code must only be ran on the client of the projectile owner
                if (player.channel)
                {
                    float holdoutDistance = player.HeldItem.shootSpeed * Projectile.scale;
                    // Calculate a normalized vector from player to mouse and multiply by holdoutDistance to determine resulting holdoutOffset
                    Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);
                    if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
                    {
                        // This will sync the projectile, most importantly, the velocity.
                        Projectile.netUpdate = true;
                    }               
                    // Projectile.velocity acts as a holdoutOffset for held projectiles.
                    Projectile.velocity = holdoutOffset;
                }
                else
                {
                    if (Projectile.ai[1] <= 69)
                        Projectile.Kill();
                    if (player.direction == 1)
                    {
                        if (Projectile.ai[1] >= 70 && Projectile.ai[1] <= 80)
                        {
                            Projectile.rotation -= 0.1f;
                        }
                        if (Projectile.ai[1] >= 81 && Projectile.ai[1] <= 90)
                        {
                            Projectile.rotation += 0.1f;
                        }
                    }
                    else
                    {
                        if (Projectile.ai[1] >= 70 && Projectile.ai[1] <= 80)
                        {
                            Projectile.rotation += 0.1f;
                        }
                        if (Projectile.ai[1] >= 81 && Projectile.ai[1] <= 90)
                        {
                            Projectile.rotation -= 0.1f;
                        }
                    }
                    if (Projectile.ai[1] >= 96)
                        Projectile.Kill();
                }
            }
            if (Projectile.ai[1] >= 70)
            {               
                if (!player.channel)
                {
                    PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 6f, 6f, 20, 1000f, FullName);
                    Main.instance.CameraModifiers.Add(modifier);
                    Vector2 velocity = Projectile.DirectionTo(Main.MouseWorld) * 16f;
                    if (Main.rand.NextBool(5000))
                        SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/fartraresound"), Projectile.position);              
                    else
                    {
                        if (Projectile.ai[1] == 73 || Projectile.ai[1] == 75)
                            SoundEngine.PlaySound(SoundID.Item105, Projectile.position);
                        if (Projectile.ai[1] == 76)
                            SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
                    }                   
                    for (int i = 0; i < 12; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(50));
                        // Decrease velocity randomly for nicer visuals.
                        newVelocity *= 1f - Main.rand.NextFloat(0.4f);
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), playerCenter, newVelocity, ModContent.ProjectileType<CorrosiveExplosion>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                    }
                    Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * 0.05f;
                    Projectile.velocity.Y *= 1f + Main.rand.Next(-3, 4) * 0.05f;
                }
                else
                {
                    Projectile.ai[1] = 72;
                    Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * 0.03f;
                    Projectile.velocity.Y *= 1f + Main.rand.Next(-3, 4) * 0.02f;
                }
                    
            }
            if (Projectile.ai[2] == 70)
                SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
            if (player.channel)
            {
                if (Projectile.velocity.X > 0f)
                {
                    player.ChangeDir(1);
                }
                else if (Projectile.velocity.X < 0f)
                {
                    player.ChangeDir(-1);
                }
                Projectile.rotation = Projectile.velocity.ToRotation();                
            }
            player.ChangeDir(Projectile.direction); // Change the player's direction based on the projectile's own
            player.heldProj = Projectile.whoAmI; // We tell the player that the drill is the held projectile, so it will draw in their hand
            player.SetDummyItemTime(2); // Make sure the player's item time does not change while the projectile is out
            Projectile.Center = playerCenter; // Centers the projectile on the player. Projectile.velocity will be added to this in later Terraria code causing the projectile to be held away from the player at a set distance.
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - 45);
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Player player = Main.player[Projectile.owner];
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            if (player.direction == 1)
            {
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }       
            else
            {
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipVertically, 0);
            }
            return false;
        }
    }
    public class CorrosiveExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(ModContent.BuffType<Corrosion>(), 120);
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.50f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 20;
            Projectile.aiStyle = 0;
            Projectile.netImportant = true;
        }
        public static float easeInOutQuad(float x)
        {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        public override void AI()
        {
            if (Main.rand.NextBool(14))
            {
                int dust = Dust.NewDust(Projectile.position, 46, 44, DustID.GreenTorch, Projectile.velocity.X, 0f, 0);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 4f;
                Main.dust[dust].scale = (float)Main.rand.Next(100, 170) * 0.006f;
            }
            Projectile.alpha += 10;
            Projectile.scale += 0.1f;
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
            Projectile.rotation += 0.3f;
        }
    }
}