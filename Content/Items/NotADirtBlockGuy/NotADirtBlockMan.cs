using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PenumbraMod.Content.Items.NotADirtBlockGuy
{
    //    Crabhim presence

    //Colossal Defense Overdrive:
    //Unleashes an unimaginable surge in defense, elevating the player's protective capabilities by an astronomical +50,000. The very fabric of reality seems to bend to shield the player from harm, creating an impervious fortress against any onslaught.
    //Supernova Critical Mastery:
    //Ascends to the pinnacle of critical mastery, boosting the critical hit chance by a mind-blowing +30,000%. With this unparalleled precision, every strike becomes a cataclysmic event, capable of shattering foes with astronomical critical damage.
    //Hyperdrive Melee Barrage:
    //Engages an otherworldly melee barrage, accelerating the attack speed for melee weapons to an astonishing +75,000%. The player becomes an indomitable force, executing a relentless flurry of strikes faster than the eye can comprehend.
    //Galactic Damage Surge:
    //Channels an astronomical surge of power, increasing the player's overall damage output by an unfathomable +100,000%. Each blow becomes a cosmic event, unleashing a level of devastation that transcends mortal limits.
    //Celestial Minion Dominion:
    //Bestows minions with celestial might, intensifying their knockback by a cosmic +10,000 units.The player's summoned entourage becomes an unstoppable cosmic force, effortlessly repelling enemies with the strength of celestial bodies.
    //Warp-Speed Velocity Ascent:
    //Achieves an incomprehensible speed boost, catapulting the player's movement speed to a staggering +500,000%. Traveling the terrain becomes a warp-speed journey, transcending the limits of mortal locomotion.
    //Quasar Mining Frenzy:
    //Triggers a quasar-powered mining frenzy, accelerating the mining speed by an astronomical +250,000%. The player effortlessly extracts resources at a speed that defies the laws of mining physics, leaving the landscape stripped of minerals in their wake.
    //Eternal Revitalizing Surge:
    //initiates an eternal surge of revitalization, enhancing life and mana regeneration by an otherworldly +50,000 per second. The player experiences an unending flow of rejuvenation, achieving a state of perpetual vitality.
    //Omnipotent Debilitation Nullification:

    //Bestows omnipotent debilitation nullification, rendering the player impervious to all debuffs across time and space.Poison, fire, and any conceivable affliction find no foothold against this cosmic defense.

    //A God finally found its sucessor.
    public class OverpoweredDirt : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        { // This method gets called every frame your buff is active on your player.
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<OverpoweredDirtPet>());
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/NotADirtBlockGuy/OverpoweredDirt").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/NotADirtBlockGuy/NotADirtBlockGuy").Value;
            if (!Main.zenithWorld)
                spriteBatch.Draw(texture, drawParams.Position, null, drawParams.DrawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(texture2, drawParams.Position, null, drawParams.DrawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class OverpoweredDirtPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 16)
                .WithOffset(0, -10f)
                .WithSpriteDirection(-1)
                .WithCode(DelegateMethods.CharacterPreview.Float);
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FairyQueenPet);
            Projectile.width = 56;
            Projectile.height = 21;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.dino = false;
            if (!Main.zenithWorld)
            {
                if (++Projectile.frameCounter >= 16)
                {
                    Projectile.frameCounter = 0;
                    // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.frame = 0;
                    }
                }
            }
           
            return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/SFX/EpicSound"));
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.zenithWorld)
                SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/SFX/chororo"));
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CheckActive(player);
            if (!Main.zenithWorld)
            {
                AIType = ProjectileID.FairyQueenPet;
                Projectile.Resize(56, 21);
                Projectile.tileCollide = false;
                Projectile.ignoreWater = true;
            }
            else
            {
                AIType = ProjectileID.BabyDino;       
                Projectile.Resize(90, 108);
                Projectile.tileCollide = true;
                Projectile.ignoreWater = false;
                Projectile.velocity.Y += 14f;
                if (Projectile.lavaWet)
                {
                    SoundEngine.PlaySound(new SoundStyle("PenumbraMod/Assets/Sounds/SFX/funni"));
                }
            }
              
            if (Main.myPlayer == player.whoAmI && Projectile.DistanceSQ(player.Center) > 2000 * 2000)
            {
                Projectile.position = player.Center + new Vector2(0, -50); // its big so when it teleports prevent to spawn inside the ground
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }
        }
        private void CheckActive(Player player)
        {
            if (!player.dead && player.HasBuff(ModContent.BuffType<OverpoweredDirt>()))
                Projectile.timeLeft = 2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.penetrate == 0)
                Projectile.Kill();
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/NotADirtBlockGuy/NotADirtBlockMan").Value;
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            int frameHeight = tex.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new(0, startY, tex.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float offsetX = 20f;
            origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
            Vector2 drawOrigin = new(tex.Width * 0.5f, Projectile.height * 0.5f);

            if (!Main.zenithWorld)
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, sourceRectangle, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, 0, 0);
           else
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation , texture.Size() / 2, Projectile.scale, 0, 0);

            return false;
        }
        float MousePositionFloatX;
        float MousePositionFloatY;
        public override void PostDraw(Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = ModContent.Request<Texture2D>("PenumbraMod/Content/Items/NotADirtBlockGuy/DirtEye").Value;
            Vector2 zero = Vector2.Zero;
            MousePositionFloatX = ((Math.Min(Main.screenWidth, Main.MouseScreen.X) - 0) * 100) / (Main.screenWidth - 0) / 100;
            MousePositionFloatY = ((Math.Min(Main.screenHeight, Main.MouseScreen.Y) - 0) * 100) / (Main.screenHeight - 0) / 100;

            if (!Main.zenithWorld)
            Main.EntitySpriteDraw(
                texture, // texture
                Projectile.Center + new Vector2(zero.X + 8 + MathHelper.Lerp(-5, 0, MousePositionFloatX), zero.Y + 5 + MathHelper.Lerp(-5, 0, MousePositionFloatY)) - Main.screenPosition, // position
                null, // rectangle
                Color.White, // color
                Projectile.rotation, // rotation
               texture.Size(), // origin
                Projectile.scale, // scale
                0, // spriteeffects
                0f); // layerdepth
        }
    }
    public class OverpoweredDirtBlock : ModItem
    {
        // Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

            Item.shoot = ModContent.ProjectileType<OverpoweredDirtPet>(); // "Shoot" your pet projectile.
            Item.buffType = ModContent.BuffType<OverpoweredDirt>(); // Apply buff upon usage of the Item.
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }

    }
}
