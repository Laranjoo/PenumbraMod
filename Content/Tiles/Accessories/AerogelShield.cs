using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Dusts;
using PenumbraMod.Content.Items.Placeable;
using PenumbraMod.Content.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]

    public class AerogelShield : ModItem
    {

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("This slimy shield protects you from the slimes and keep them friendly" +
                "\nGrants immunity to slimed debuff" +
                "\nSpawns Slime Spikes when moving" +
                "\n''You feel like the king!''"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.hasVanityEffects = true;
            Item.width = 34;
            Item.height = 32;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.defense = 2;
            Item.expert = true;
        }
        public static List<int> Slimes = new()
        {
            NPCID.BlueSlime,
            NPCID.SlimeSpiked,
            NPCID.GoldenSlime,
            NPCID.IceSlime,
            NPCID.SpikedIceSlime,
            NPCID.SlimeRibbonYellow,
            NPCID.SlimeRibbonWhite,
            NPCID.SlimeRibbonRed,
            NPCID.SlimeRibbonGreen,
            NPCID.Slimer,
            NPCID.SlimeMasked,
            NPCID.RainbowSlime,
            NPCID.CorruptSlime,
            NPCID.LavaSlime,
            NPCID.UmbrellaSlime,
            NPCID.Crimslime,
            NPCID.SpikedJungleSlime,
            NPCID.DungeonSlime,
            NPCID.IlluminantSlime,
            NPCID.MotherSlime,
            NPCID.SandSlime,
            NPCID.ShimmerSlime,
            NPCID.Slimer2,
            NPCID.ToxicSludge,
            NPCID.Gastropod,
            ModContent.NPCType<MarshmellowSlime>(),
            ModContent.NPCType<BloodystoneSlime>(),
            ModContent.NPCType<CorrosiveSlime>(),
            ModContent.NPCType<InfectedSlime>(),
        };
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Slimed] = true;          
            for (int i = 0; i < NPCLoader.NPCCount; i++)
            {
                if (Slimes.Contains(i))
                {
                    player.npcTypeNoAggro[i] = true;
                }
            }
            if (Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) > 1f && !player.rocketFrame)
            {
                if (Main.rand.NextBool(16))
                {
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), new Vector2(player.position.X + Main.rand.NextFloat(player.width), player.position.Y + Main.rand.NextFloat(player.height)), new Vector2(0f, 0f), ModContent.ProjectileType<SlimeSpike>(), 12, 0, Main.myPlayer);
                }
              
            }
            if (!hideVisual)
            {
                player.GetModPlayer<HasShield>().HasSlimeShield = true;
                if (player.ownedProjectileCounts[ModContent.ProjectileType<SlimyShieldCrown>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), player.position, new Vector2(0f, 0f), ModContent.ProjectileType<SlimyShieldCrown>(), 0, 0, Main.myPlayer);
                }
            }          
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AerogelBar>(20)
                .AddIngredient(ItemID.RoyalGel, 1)
                .AddIngredient(ItemID.Gel, 35)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class HasShield : ModPlayer
    {
        public bool HasSlimeShield;

        // Always reset the accessory field to its default value here.
        public override void ResetEffects()
        {
            HasSlimeShield = false;
        }
    }
    public class SlimeSpike : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 9;
            Projectile.width = 9;
            Projectile.height = 26;
            Projectile.aiStyle = 68;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.scale = 0.7f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 300);
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.LightBlue.ToVector3() * 0.93f);

            int dust = Dust.NewDust(Projectile.Center, 2, 1, DustID.BlueTorch, 0f, 0f, 0, Color.Blue, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 1.0f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 150) * 0.005f;

        }

    }

    public class SlimyShieldCrown : ModProjectile
    {
        public override void SetDefaults()
        {
            AIType = 0;
            Projectile.width = 26;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.timeLeft = 10;
            // Some math magic to make it smoothly move up and down over time
            const float TwoPi = (float)Math.PI * 4f;
            float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 4f);

            if (Main.myPlayer == Projectile.owner)
            {
                if (projOwner.direction == 1)
                    Projectile.Center = projOwner.Top + new Vector2(2, -3f + offset);
                else
                    Projectile.Center = projOwner.Top + new Vector2(-2, -3f + offset);
            }
          

            if (projOwner.dead || !projOwner.active)
            {//Disappear when player dies
                Projectile.timeLeft = 0;
                Projectile.Kill();
                Projectile.alpha = 255;
            }
            if (!projOwner.GetModPlayer<HasShield>().HasSlimeShield)
            {
                Projectile.Kill();
            }

            //Orient projectile
            projOwner.heldProj = Projectile.whoAmI;
            Projectile.direction = projOwner.direction;
            Projectile.spriteDirection = projOwner.direction;
        }
        public override bool PreDraw(ref Color lightColor)
        {      
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            lightColor.A = 0;
            Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
        }
    }
}


