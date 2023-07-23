using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items
{
	public class SpaghettinisPenumbraticTwistedBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spaghettini's Penumbratic Twisted Blade"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			/* Tooltip.SetDefault("[c/ad08cd:The blade of Penumbra]" +
                "\n[c/75efff:This sword was known to be used in the holy warfare.]" +
                "\n[c/75efff:It makes enemies afraid to approach you]" +
                "\n[c/75efff:But they will feel the pain this blade causes...]" +
                "\n[c/75efff:Shoots an Penumbratic razorblade]" +
                "\n[c/75efff:Hitting enemies will struck them with Super Star Slash.]" +
                "\n[c/75efff:Right click to be able to teleport the player into cursor.]" +
                "\n[c/75efff:It will do a huge super star slash to destroy enemies]" +
                "\n[c/75efff:It gives chaos state for 5 seconds ]" +
                "\n[c/ff0000:''Shall you destroy?'']" +
                "\n[c/00FF00:-Developer Item-]"); */
            Item.staff[Item.type] = true;
        }

        public int TwistedStyle = 0;
        public override void SetDefaults()
        {
            Item.damage = 700;
            Item.DamageType = DamageClass.Melee;
            Item.width = 96;
            Item.height = 96;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 13;
            Item.value = 100000;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.noMelee = true;
            Item.shoot = ProjectileID.SuperStar;
            Item.shootSpeed = 2f;
            Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
        }
        public sealed override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Mod == "Terraria" && line.Name == "ItemName")
            {
                Main.spriteBatch.End(); //end and begin main.spritebatch to apply a shader
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
                var lineshader = GameShaders.Misc["QueenSlime"];
                lineshader.Apply(null);
                Utils.DrawBorderString(Main.spriteBatch, line.Text, new Vector2(line.X, line.Y), Color.White, 1); //draw the tooltip manually
                Main.spriteBatch.End(); //then end and begin again to make remaining tooltip lines draw in the default way
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                return false;
            }
            return true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[0]);
            }
            else
            {
                frame = texture.Frame();
            }
            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;
            for (float i = 0; i < 4; i += 0.35f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;
                spriteBatch.Draw(texture, position + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(93, 31, 216, 70), 0, origin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        bool notboollol = true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            
            if (player.altFunctionUse != 2)
            {
                Item.useTime = 20;
                Item.noUseGraphic = true;
                if (TwistedStyle == 0)
                {
                    if (player.velocity.Y > 5)
                    {
                        if (player.direction == 0)
                            notboollol = true;
                        else
                            notboollol = false;
                    }
                    if (player.velocity.Y < -5)
                    {
                        if (player.direction != 0)
                            notboollol = true;
                        else
                            notboollol = false;
                    }
                   const int NumProjectiles = 1;

                   for (int i = 0; i < NumProjectiles; i++)
                   {
                       Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(0));

                       // Decrease velocity randomly for nicer visuals.
                       newVelocity *= 10f - Main.rand.NextFloat(0f);
                       // Create a projectile.
                       Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<TwistedRazorblade>(), damage, knockback, player.whoAmI);
                   }
                    int basic = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TwistedSwing>(), damage, knockback, player.whoAmI);
                    if (notboollol == true)
                    {
                        Main.projectile[basic].ai[1] = -1;
                        notboollol = false;
                    }
                    else
                    {
                        Main.projectile[basic].ai[1] = 1;
                        notboollol = true;
                    }
                    Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
                }
                else if (TwistedStyle == 1)
                {
                    Item.useTime = 40;
                    Item.noUseGraphic = false;
                    if (player.chaosState == false)
                    {
                        for (int i = 0; i < player.Distance(Main.MouseWorld); i += 16)//16 pixel gap between star slashes
                        {
                            Vector2 e = new Vector2(i, 0).RotatedBy(player.DirectionTo(Main.MouseWorld).ToRotation());
                            float rot = MathHelper.ToRadians(Main.rand.Next(0, 360));
                            Vector2 pos = new Vector2(16, 0).RotatedBy(rot);
                            int basic = Projectile.NewProjectile(source, player.Center + e + pos, pos / -4, ProjectileID.SuperStarSlash, damage * 6, 0, Main.myPlayer);
                            Main.projectile[basic].rotation = rot;
                            Main.projectile[basic].scale = Main.rand.NextFloat(10f, 30f) / 10;
                            // Main.projectile[basic].timeLeft = 8;
                            Main.projectile[basic].penetrate = -1;
                            TwistedStyle = 0;
                        }
                        player.AddBuff(BuffID.ChaosState, 360);
                        player.Center = Main.MouseWorld;
                        SoundEngine.PlaySound(SoundID.Item71, player.Center);
                        
                    }
                }
            }
            else
            {
                int basic = Projectile.NewProjectile(source, player.Center, Vector2.Zero, ProjectileID.PrincessWeapon, 0, 0, Main.myPlayer);
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
            }
            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }
        int TwistedTimer = 0;
       
        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                TwistedTimer++;
                TwistedStyle++;
                if (TwistedStyle > 1)
                {
                    TwistedStyle = 1;
                    TwistedTimer = 0;
                }
                //Item.shoot = TwistedStyle + 120;
                //Main.NewText($"Switching to ItemUseStyleID #{Item.useStyle}");
            }
            else
            {
                //Main.NewText($"This is ItemUseStyleID #{Item.useStyle}");
            }
            return true;
        }
        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3() * 0.80f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, Color.Purple.ToVector3() * 0.7f);
            // Draw the periodic glow effect behind the item when dropped in the world (hence PreDrawInWorld)
            Texture2D texture = TextureAssets.Item[Item.type].Value;

            Rectangle frame;

            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
            }
            else
            {
                frame = texture.Frame();
            }

            Vector2 frameOrigin = frame.Size() / 2f;
            Vector2 offset = new Vector2(Item.width / 2 - frameOrigin.X, Item.height - frame.Height);
            Vector2 drawPos = Item.position - Main.screenPosition + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;

            for (float i = 0f; i < 1f; i += 0.15f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(0, 255, 255, 40), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            for (float i = 0f; i < 1f; i += 0.20f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                spriteBatch.Draw(texture, drawPos + new Vector2(0f, 6f).RotatedBy(radians) * time, frame, new Color(228, 87, 255, 55), rotation, frameOrigin, scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MirrorFragment>(), 30);
            recipe.AddIngredient(ModContent.ItemType<PenumbraticShard>(), 15);
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddIngredient(ItemID.FragmentSolar, 15);
            recipe.AddIngredient(ItemID.SoulofMight, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddIngredient(ItemID.GoldBar, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}