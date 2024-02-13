using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Buffs;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace PenumbraMod.Content.Items
{
    internal class CorrosionHook : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 98;
            Item.shootSpeed = 18f; // This defines how quickly the hook is shot.
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.shoot = ModContent.ProjectileType<CorrosionHookProjectileFriendly>(); // Makes the item shoot the hook's projectile when used.
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8;
            Item.value = 26000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        int launch;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (launch == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<CorrosionHookProjectile3>()] != 1)
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.UseSound = SoundID.Item1;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorrosionHookProjectile>(), damage * 2, knockback, player.whoAmI);
                launch = 1;
            }
            else if (launch == 1 && player.ownedProjectileCounts[ModContent.ProjectileType<CorrosionHookProjectile>()] != 1)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.useTime = 10;
                Item.useAnimation = 10;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorrosionHookProjectile2>(), damage, knockback, player.whoAmI);
                launch = 2;
            }
            else if (launch == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<CorrosionHookProjectile2>()] != 1)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.UseSound = SoundID.Item152;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorrosionHookProjectile>(), damage * 2, knockback, player.whoAmI);
                launch = 3;
            }
            else if (launch == 3 && player.ownedProjectileCounts[ModContent.ProjectileType<CorrosionHookProjectile>()] != 1)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.useTime = 10;
                Item.useAnimation = 10;
                Item.UseSound = SoundID.Item1;
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CorrosionHookProjectile3>(), damage * 3, knockback, player.whoAmI);
                launch = 0;
            }
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<CorrosionHookProjectile>()] < 1 
                && player.ownedProjectileCounts[ModContent.ProjectileType<CorrosionHookProjectile2>()] < 1
                && player.ownedProjectileCounts[ModContent.ProjectileType<CorrosionHookProjectile3>()] < 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CorrosiveShard>(), 18)
                 .AddIngredient(ModContent.ItemType<CorrodedPlating>(), 6)
               .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    // Bad way to do chain weapons (i think)
    internal class CorrosionHookProjectile : ModProjectile
    {
        private static Asset<Texture2D> chainTexture;

        public override void Load()
        { // This is called once on mod (re)load when this piece of content is being loaded.
          // This is the path to the texture that we'll use for the hook's chain. Make sure to update it.
            chainTexture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosionHookChain");
        }

        public override void Unload()
        { // This is called once on mod reload when this piece of content is being unloaded.
          // It's currently pretty important to unload your static fields like this, to avoid having parts of your mod remain in memory when it's been unloaded.
            chainTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.damage = 98;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = ProjAIStyleID.Hook;
            Projectile.extraUpdates = 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(ModContent.BuffType<Corrosion>(), 120);
        }
        public override float GrappleRange()
        {
            return 460f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1; // The amount of hooks that can be shot out
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 24f; // How fast the grapple returns to you after meeting its max shoot distance
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 0; // How fast you get pulled to the grappling hook projectile's landing position
        }

        // Can customize what tiles this hook can latch onto, or force/prevent latching alltogether, like Squirrel Hook also latching to trees
        public override bool? GrappleCanLatchOnTo(Player player, int x, int y)
        {
            // In any other case, behave like a normal hook
            return false;
        }

        // Draws the grappling hook's chain.
        public override bool PreDrawExtras()
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
            float distanceToPlayer = directionToPlayer.Length();

            while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer))
            {
                directionToPlayer /= distanceToPlayer; // get unit vector
                directionToPlayer *= chainTexture.Height(); // multiply by chain link length

                center += directionToPlayer; // update draw position
                directionToPlayer = playerCenter - center; // update distance
                distanceToPlayer = directionToPlayer.Length();

                Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

                // Draw chain
                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                    chainTexture.Value.Bounds, drawColor, chainRotation + 45,
                    chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            // Stop vanilla from drawing the default chain.
            return false;
        }
    }
    internal class CorrosionHookProjectileFriendly : ModProjectile
    {
        private static Asset<Texture2D> chainTexture;

        public override void Load()
        { // This is called once on mod (re)load when this piece of content is being loaded.
          // This is the path to the texture that we'll use for the hook's chain. Make sure to update it.
            chainTexture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/CorrosionHookChain");
        }

        public override void Unload()
        { // This is called once on mod reload when this piece of content is being unloaded.
          // It's currently pretty important to unload your static fields like this, to avoid having parts of your mod remain in memory when it's been unloaded.
            chainTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GemHookAmethyst); // Copies the attributes of the Amethyst hook's projectile.
            Projectile.width = 14;
            Projectile.height = 38;
        }
        // Amethyst Hook is 300, Static Hook is 600.
        public override float GrappleRange()
        {
            return 500f;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1; // The amount of hooks that can be shot out
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 24f; // How fast the grapple returns to you after meeting its max shoot distance
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 20; // How fast you get pulled to the grappling hook projectile's landing position
        }

        // Can customize what tiles this hook can latch onto, or force/prevent latching alltogether, like Squirrel Hook also latching to trees
        public override bool? GrappleCanLatchOnTo(Player player, int x, int y)
        {
            // In any other case, behave like a normal hook
            return null;
        }

        // Draws the grappling hook's chain.
        public override bool PreDrawExtras()
        {
            Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 center = Projectile.Center;
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
            float distanceToPlayer = directionToPlayer.Length();

            while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer))
            {
                directionToPlayer /= distanceToPlayer; // get unit vector
                directionToPlayer *= chainTexture.Height(); // multiply by chain link length

                center += directionToPlayer; // update draw position
                directionToPlayer = playerCenter - center; // update distance
                distanceToPlayer = directionToPlayer.Length();

                Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

                // Draw chain
                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                    chainTexture.Value.Bounds, drawColor, chainRotation,
                    chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            }
            // Stop vanilla from drawing the default chain.
            return false;
        }
    }
}
