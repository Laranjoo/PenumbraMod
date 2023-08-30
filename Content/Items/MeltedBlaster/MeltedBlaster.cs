using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.MeltedBlaster
{
    public class MeltedBlaster : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            /* Tooltip.SetDefault("This gun has 2 modes:" +
                "\n[c/ffde38:First Mode:] Hold the gun to charge it up to 5 times" +
                "\nIt shoots Melted Tridents that inflicts 'On Fire!'" +
                "\n1th to 3rd charge scales the trident damage, 4th charge makes the trident pass through tiles" +
                "\nThe 5th charge shoots a high velocity trident that explodes on enemies hit, inflicting the Melted debuff" +
                "\nBut be careful in holding the 5th charge too much, the gun will start to get [c/740038:Overcharged]" +
                "\nWhen [c/740038:Overcharged], the gun explodes, inflicting damage on the player plus the melted debuff" +
                "\nYou can't use the gun when melting" +
                "\n[c/ff6a00:Second Mode:] The gun will shoot fire, inflicting 'On Fire!'" +
                "\nThe fire velocity depends on cursor position"
                + "\n[c/793a80:'Grand things requer grand responsability']"); */

        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 78;
            Item.height = 40;
            Item.useTime = 9999;
            Item.useAnimation = 9999;
            Item.useStyle = 5;
            Item.knockBack = 3;
            Item.value = 11200;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<EMPTY>();
            Item.shootSpeed = 20f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.crit = 25;
            Item.channel = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.itemAnimation = 0;
                player.itemTime = 0;
                Item.shoot = ModContent.ProjectileType<MeltedBlasterProj2>();
                Item.autoReuse = true;
            }
            else
            {
                player.itemAnimation = 0;
                player.itemTime = 0;
                Item.autoReuse = false;
                Item.shoot = ModContent.ProjectileType<MeltedBlasterProj>();
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<MeltedBlasterProj>()] < 1;
        }
        public int TwistedStyle = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                if (TwistedStyle == 0)
                {

                    float launchSpeed = 120f;
                    float launchSpeed2 = 0f;
                    float launchSpeed3 = 0f;

                    Vector2 mousePosition = Main.MouseWorld;
                    Vector2 direction = Vector2.Normalize(mousePosition - player.Center);
                    Vector2 Gun = direction * launchSpeed2;
                    Vector2 Disk = direction * launchSpeed3;
                    Vector2 muzzleOffset = Vector2.Normalize(new Vector2(-10, -25)) * 0f;
                    position = new Vector2(position.X, position.Y);
                    if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                    {
                        position += muzzleOffset;
                    }

                    Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, Gun.X, Gun.Y, ModContent.ProjectileType<MeltedBlasterProj>(), 0, knockback, player.whoAmI);


                }

                if (TwistedStyle == 1)
                {

                    float launchSpeed = 120f;
                    float launchSpeed2 = 0f;
                    float launchSpeed3 = 0f;

                    Vector2 mousePosition = Main.MouseWorld;
                    Vector2 direction = Vector2.Normalize(mousePosition - player.Center);
                    Vector2 Gun = direction * launchSpeed2;
                    Vector2 Disk = direction * launchSpeed3;
                    Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 0f;
                    position = new Vector2(position.X, position.Y);
                    if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                    {
                        position += muzzleOffset;
                    }

                    Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), position.X, position.Y, Gun.X, Gun.Y, ModContent.ProjectileType<MeltedBlasterProj2>(), 0, knockback, player.whoAmI);


                }

            }

            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }
        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (Main.myPlayer == player.whoAmI && player.altFunctionUse == 2 && TwistedStyle == 0)
            {
                TwistedStyle++;
                if (TwistedStyle > 0)
                {
                    TwistedStyle = 1;
                }
                //Item.shoot = TwistedStyle + 120;
                Vector2 velocity = new Vector2(0, 0);
                int basic = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, velocity, ModContent.ProjectileType<GunMode>(), 0, 0, player.whoAmI);
                SoundEngine.PlaySound(SoundID.Item149);
                player.itemAnimation = 0;
                player.itemTime = 0;
            }
            else
            {
                if (TwistedStyle == 1 && player.altFunctionUse == 2)
                {

                    TwistedStyle = 0;
                    Vector2 velocity = new Vector2(0, 0);
                    int basic = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), player.Center, velocity, ModContent.ProjectileType<Charging>(), 0, 0, player.whoAmI);
                    SoundEngine.PlaySound(SoundID.Item149);
                    player.itemAnimation = 0;
                    player.itemTime = 0;
                }

            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeltedEmber>(), 17);
            recipe.AddIngredient(ItemID.Obsidian, 25);
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }


    public class MeltedBlasterProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 999999;
            Projectile.ownerHitCheck = true;
        }
        public int counter;
        public bool boom = false;
        public bool bep = false;
        public bool bep2 = false;
        public bool text = false;
        public bool text1 = false;
        public float movement
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            counter++;
            Vector2 playerCenter = owner.RotatedRelativePoint(owner.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                // This code must only be ran on the client of the projectile owner
                if (owner.channel)
                {
                    float holdoutDistance = owner.HeldItem.shootSpeed * Projectile.scale;
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
                    Projectile.Kill();
                }
            }

            if (Projectile.velocity.X > 0f)
            {
                owner.ChangeDir(1);
            }
            else if (Projectile.velocity.X < 0f)
            {
                owner.ChangeDir(-1);
            }
            owner.ChangeDir(Projectile.direction); // Change the player's direction based on the projectile's own
            owner.heldProj = Projectile.whoAmI; // We tell the player that the drill is the held projectile, so it will draw in their hand
            owner.SetDummyItemTime(2); // Make sure the player's item time does not change while the projectile is out
            Projectile.Center = playerCenter; // Centers the projectile on the player. Projectile.velocity will be added to this in later Terraria code causing the projectile to be held away from the player at a set distance.
            Projectile.rotation = Projectile.velocity.ToRotation();
            owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

            // FROM HERE IS OLD CODE I DID, ITS BAD BUT ITS FUNCTIONAL, IM LAZY TO FIX ;P
            if (boom == true)
            {
                for (int k = 0; k < 25; k++)
                {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 1.2f);
                    Main.dust[dust].velocity *= 6.0f;
                }

                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bop"));

            }
            if (bep == true)
            {

                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bop"));

            }
            if (bep2 == true)
            {

                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bopboom"));

            }
            if (text == true)
            {
                Vector2 velocity9 = (Main.MouseWorld - Projectile.Center) / 99f;
                int basic23456 = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity9, ModContent.ProjectileType<OVERCHARGING>(), 0, 0, owner.whoAmI);
                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/Items/bop"));
            }
            owner.ChangeDir(Main.MouseWorld.X > owner.Center.X ? 1 : -1);

            if (counter > 29 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 8f, ModContent.ProjectileType<MeltedTrident>(), 40, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 30 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 31 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 69 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 10f, ModContent.ProjectileType<MeltedTrident2>(), 50, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 70 && owner.channel || owner.CCed)
            {
                boom = true;
                Projectile.frame = 1;
            }
            if (counter > 71 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 109 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 14f, ModContent.ProjectileType<MeltedTrident3>(), 60, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 110 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 111 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 149 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 18f, ModContent.ProjectileType<MeltedTrident4>(), 80, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 150 && owner.channel || owner.CCed)
            {
                boom = true;
                Projectile.frame = 2;
            }
            if (counter > 151 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 189 && !owner.channel || owner.CCed)
            {
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, owner.DirectionTo(Main.MouseWorld) * 22f, ModContent.ProjectileType<MeltedTrident5>(), 100, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item92);
            }
            if (counter > 190 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 191 && owner.channel || owner.CCed)
            {
                boom = false;
            }

            if (counter > 230 && owner.channel || owner.CCed)
            {

                bep = true;

            }
            if (counter > 231 && owner.channel || owner.CCed)
            {

                bep = false;

            }
            if (counter > 240 && owner.channel || owner.CCed)
            {
                bep2 = true;
            }
            if (counter > 241 && owner.channel || owner.CCed)
            {
                bep2 = false;
            }
            if (counter > 250 && owner.channel || owner.CCed)
            {
                Projectile.frame = 3;
                text = true;
            }
            if (counter > 251 && owner.channel || owner.CCed)
            {
                text = false;
            }
            if (counter > 255 && owner.channel || owner.CCed)
            {
                bep = true;
            }
            if (counter > 256 && owner.channel || owner.CCed)
            {
                bep = false;
            }
            if (counter > 260 && owner.channel || owner.CCed)
            {
                boom = true;
            }
            if (counter > 261 && owner.channel || owner.CCed)
            {
                boom = false;
            }
            if (counter > 265 && owner.channel || owner.CCed)
            {
                bep = true;

            }
            if (counter > 266 && owner.channel || owner.CCed)
            {
                bep = false;
            }
            if (counter > 280)
            {
                Vector2 velocity3 = (Main.MouseWorld - Projectile.Center) / 8f;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity3, ModContent.ProjectileType<MeltedTridentHostile>(), 80, 0, owner.whoAmI);
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;
                SoundEngine.PlaySound(SoundID.Item74);
                for (int k = 0; k < 50; k++)
                {
                    int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LavaMoss, Projectile.oldVelocity.X * 0f, Projectile.oldVelocity.Y * 0f, Scale: 2.2f);
                    Main.dust[dust].velocity *= 7.0f;
                }
                owner.AddBuff(ModContent.BuffType<Melting>(), 360);
            }
            if (!owner.channel || owner.CCed)
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;

                return;
            }
            if (owner.HasBuff(ModContent.BuffType<Melting>()))
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;
            }

        }
    }
    public class MeltedBlasterProj2 : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melted Blaster"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 999999;
            Projectile.ownerHitCheck = true;
        }
        public int counter;

        public float movement
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void AI()
        {
            Projectile.frame = 0;
            Player owner = Main.player[Projectile.owner];
            counter++;
            Vector2 playerCenter = owner.RotatedRelativePoint(owner.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                // This code must only be ran on the client of the projectile owner
                if (owner.channel)
                {
                    float holdoutDistance = owner.HeldItem.shootSpeed * Projectile.scale;
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
                    Projectile.Kill();
                }
            }

            if (Projectile.velocity.X > 0f)
            {
                owner.ChangeDir(1);
            }
            else if (Projectile.velocity.X < 0f)
            {
                owner.ChangeDir(-1);
            }
            owner.ChangeDir(Projectile.direction); // Change the player's direction based on the projectile's own
            owner.heldProj = Projectile.whoAmI; // We tell the player that the drill is the held projectile, so it will draw in their hand
            owner.SetDummyItemTime(2); // Make sure the player's item time does not change while the projectile is out
            Projectile.Center = playerCenter; // Centers the projectile on the player. Projectile.velocity will be added to this in later Terraria code causing the projectile to be held away from the player at a set distance.
            Projectile.rotation = Projectile.velocity.ToRotation();
            owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            owner.ChangeDir(Main.MouseWorld.X > owner.Center.X ? 1 : -1);

            if (counter > 10 && owner.channel || owner.CCed)
            {
                Vector2 velocity = (Main.MouseWorld - Projectile.Center) / 9f;
                int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), owner.Center, velocity, ModContent.ProjectileType<MeltedFire>(), 40, 0, owner.whoAmI);
                SoundEngine.PlaySound(SoundID.Item109);
                counter = 5;
            }


            if (!owner.channel || owner.CCed)
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;

                return;
            }
            if (owner.HasBuff(ModContent.BuffType<Melting>()))
            {
                owner.itemAnimation = 0;
                owner.itemTime = 0;
                Projectile.timeLeft = 0;
                Projectile.Kill();
                counter = 0;
            }

        }
    }



}
