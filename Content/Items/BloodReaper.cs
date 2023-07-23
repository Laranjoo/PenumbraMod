using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using PenumbraMod.Content.DamageClasses;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
    public class BloodReaper : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("A bloody scythe made for enemy suffering..." +
                "\n[c/b4202a:Special ability:] When used, more scythes are swung in all directions and you dash at the mouse direction"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<ReaperClass>();
            Item.damage = 220;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Red;

            Item.shoot = ModContent.ProjectileType<BloodReaperProj>();
            Item.shootSpeed = 4;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;

            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void UpdateInventory(Player player)
        {
            player.GetTotalDamage(DamageClass.Summon).ApplyTo(Item.damage);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasBuff(ModContent.BuffType<ReaperControl>()))
            {
                const int NumProjectiles = 1;
                player.velocity = player.DirectionTo(Main.MouseWorld) * 14f;
                Item.useTime = 20;
                Item.useAnimation = 20;

                for (int i = 0; i < NumProjectiles; i++)
                {
                    Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(100));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity *= 1f - Main.rand.NextFloat(0f);
                    Vector2 newVelocity2 = velocity.RotatedBy(MathHelper.ToRadians(-100));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity2 *= 1f - Main.rand.NextFloat(0f);
                    Vector2 newVelocity3 = velocity.RotatedBy(MathHelper.ToRadians(-190));

                    // Decrease velocity randomly for nicer visuals.
                    newVelocity2 *= 1f - Main.rand.NextFloat(0f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<BloodReaperProj>(), damage, knockback, player.whoAmI);
                    Projectile.NewProjectileDirect(source, position, newVelocity2, ModContent.ProjectileType<BloodReaperProj>(), damage, knockback, player.whoAmI);
                    Projectile.NewProjectileDirect(source, position, newVelocity3, ModContent.ProjectileType<BloodReaperProj>(), damage, knockback, player.whoAmI);

                }
            }
            else
            {
                Item.useTime = 30;
                Item.useAnimation = 30;
            }
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BloodReaperHold>()] < 1)
            {//Equip animation.
                int index = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.position.X, player.position.Y, 0, 0, ModContent.ProjectileType<BloodReaperHold>(), 0, 0, player.whoAmI, 0f);
            }
        }
    }
    public class BloodReaperHold : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blood Reaper");
            ProjectileID.Sets.IsAWhip[Type] = true;
        }
        public override void SetDefaults()
        {
            AIType = 0;
            Projectile.width = 71;
            Projectile.height = 71;
            Projectile.penetrate = -1;
            Projectile.light = 0.3f;
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.rotation = 90;
        }
        bool firstSpawn = true;
        double deg;

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            return true;
        }
        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.Pi;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.Yellow);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);
            DrawLine(list);

            return true;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            projOwner.heldProj = Projectile.whoAmI;
            deg = Projectile.ai[1];
            double rad = deg * (Math.PI / 180);
            double dist = 5;
            float pos = 2.5f;
            if (firstSpawn)
            {
                firstSpawn = false;
            }
            Projectile.timeLeft = 2;

            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.ai[1] = 280;
                Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            }
            else
            {
                Projectile.ai[1] = 180;
                Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2 * pos;
            }

            Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            if (Projectile.spriteDirection == 1)
            {//Adjust when facing the other direction

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(76f) + projOwner.velocity.X / -60;
            }
            else
            {

                Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(376f) + projOwner.velocity.X / -60;
            }

            Projectile.ai[0]++;

            if (projOwner.dead && !projOwner.active)
            {//Disappear when player dies
                Projectile.timeLeft = 0;
                Projectile.Kill();
                Projectile.alpha = 255;
            }

            if (projOwner.ownedProjectileCounts[ModContent.ProjectileType<BloodReaperProj>()] >= 1)
            {
                Projectile.alpha = 255;
            }
            else
            {
                //Arms will hold the weapon.
                if (projOwner.direction == 1)
                    projOwner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, 30);
                else
                    projOwner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, -30);
                Projectile.alpha = 0;
            }

            if (projOwner.HeldItem?.type != ModContent.ItemType<BloodReaper>())
            {
                Projectile.Kill();
            }
            //Orient projectile
            Projectile.direction = projOwner.direction;
            Projectile.spriteDirection = projOwner.direction;
        }

        public override void Kill(int timeLeft)
        {


        }
    }

}

