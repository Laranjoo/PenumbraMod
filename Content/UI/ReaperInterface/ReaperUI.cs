using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraMod.Content.Items.ReaperJewels;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace PenumbraMod.Content.DamageClasses
{
    internal class ReaperUI : UIState
    {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the Mod class.
        public static bool clickedagain = false;
        public static bool showModelChangeArrows = false;
        private UIText text;
        private UIText ModelText;
        private UIImage barFrame;
        private UIImage barFrameHover;
        private UIImage barFrame2;
        private UIImage barFrameHover2;
        private UIImage barFrame3;
        private UIImage barFrameHover3;
        private UIImage barFrame4;
        private UIImage barFrame4Clicked;
        private UIImage barFrameHover4;
        private UIImage barFrame5;
        private UIImage barFrameHover5;
        #region Crystals
        // UI
        private UIImage AmethystCrystal;
        private UIImage AmethystCrystalSlot2;

        private UIImage TopazCrystal;
        private UIImage TopazCrystalSlot2;

        private UIImage SapphireCrystal;
        private UIImage SapphireCrystalSlot2;

        private UIImage EmeraldCrystal;
        private UIImage EmeraldCrystalSlot2;

        private UIImage RubyCrystal;
        private UIImage RubyCrystalSlot2;

        private UIImage DiamondCrystal;
        private UIImage DiamondCrystalSlot2;

        private UIImage MagicCrystal;
        private UIImage MagicCrystalSlot2;

        private UIImage AzuriteCrystal;
        private UIImage AzuriteCrystalSlot2;

        private UIImage PrimeyeCrystal;
        private UIImage PrimeyeCrystalSlot2;

        private UIImage Ablazed;
        private UIImage AblazedSlot2;

        // simpler

        private UIImage Slimy;
        private UIImage SlimySlot2;

        private UIImage Terra;
        private UIImage TerraSlot2;

        private UIImage Bloodstained;
        private UIImage BloodstainedSlot2;

        private UIImage Darkened;
        private UIImage DarkenedSlot2;

        private UIImage Spectre;
        private UIImage SpectreSlot2;

        // way simpler

        private UIImage Corr;
        private UIImage Corr2;

        private UIImage Ony;
        private UIImage Ony2;

        private UIImage Aqua;
        private UIImage Aqua2;

        private UIImage Peri;
        private UIImage Peri2;

        private UIImage Roz;
        private UIImage Roz2;


        #endregion

        // barSlots with model variations
        private UIImage barSlots;
        private UIImage barSlots2;
        private UIImage barSlots3;
        private UIImage barSlots4;
        private UIImage barSlots5;

        // Buttons with model variations
        private ReaperButton CrystalButton;
        private ReaperButton CrystalButton2;
        private ReaperButton CrystalButton3;
        private ReaperButton CrystalButton4;
        private ReaperButton CrystalButton4Clicked; // Retro has a special thing
        private ReaperButton CrystalButton5;

        // Model changing buttons
        private ReaperButton ChangeStyleArrowRight;
        private ReaperButton ChangeStyleArrowLeft;
        private ReaperButton DeleteChangeStyleButton;
        private ReaperButton ChangeStyleButton;

        // Jewel slots
        private UIImage barSlotsNoJewel;
        private UIImage barSlotsJewelRight;
        private UIImage barSlotsJewelLeft;

        private Color gradientA;
        private Color gradientB;

        private Color gradientA2;
        private Color gradientB2;

        private Color gradientA3;
        private Color gradientB3;

        private Color gradientA4;
        private Color gradientB4;

        private Color gradientA5;
        private Color gradientB5;

        private UIImage barDrawing;
        private DraggableUI panel2;
        private UIPanel panel;
        private CrystalSlots item;
        private CrystalSlots2 item2;
        // Path to make it more readable
        const string Path = "PenumbraMod/Content/UI/ReaperInterface/";

        public override void OnInitialize()
        {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding. You can use a UIPanel if you wish for a background.

            // This is one of the most painful thing to code i ever made (Laranjoo)
            // I know its not organizated and has a lot of gimmicks and shenanigans, but hey it works!

            // Separated invisible panel to make the model changing buttons work
            panel = new UIPanel();
            panel.SetPadding(0);
            SetRectangle(panel, left: 700, top: 30, width: 120, height: 120);
            panel.BackgroundColor = new Color(0, 0, 0, 255) * 0f;
            panel.BorderColor = new Color(0, 0, 0, 255) * 0f;
            // General panel for everything
            panel2 = new DraggableUI();
            panel2.SetPadding(0);
            SetRectangle(panel2, left: 700, top: 30, width: 92, height: 23);
            panel2.BackgroundColor = new Color(0, 0, 0, 255) * 0f;
            panel2.BorderColor = new Color(0, 0, 0, 255) * 0f;

            // The bar sprite (with different models)

            barFrame = new UIImage(Request<Texture2D>(Path + "ReaperClassBar", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame.SetPadding(0);
            SetRectangle(barFrame, left: 0, top: 0, width: 92, height: 23);

            barFrameHover = new UIImage(Request<Texture2D>(Path + "ReaperClassBarHover", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrameHover.SetPadding(0);
            SetRectangle(barFrameHover, left: 0, top: 0, width: 96, height: 24);

            barFrame2 = new UIImage(Request<Texture2D>(Path + "ReaperClassBar2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame2.SetPadding(0);
            SetRectangle(barFrame2, left: 0, top: 0, width: 92, height: 23);

            barFrameHover2 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarHover2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrameHover2.SetPadding(0);
            SetRectangle(barFrameHover2, left: 0, top: 0, width: 96, height: 24);

            barFrame3 = new UIImage(Request<Texture2D>(Path + "ReaperClassBar3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame3.SetPadding(0);
            SetRectangle(barFrame3, left: 0, top: 0, width: 92, height: 23);

            barFrameHover3 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarHover3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrameHover3.SetPadding(0);
            SetRectangle(barFrameHover3, left: 0, top: 0, width: 96, height: 24);

            barFrame4 = new UIImage(Request<Texture2D>(Path + "ReaperClassBar4", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame4.SetPadding(0);
            SetRectangle(barFrame4, left: 0, top: 0, width: 92, height: 23);

            barFrame4Clicked = new UIImage(Request<Texture2D>(Path + "ReaperClassBar4Clicked", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame4Clicked.SetPadding(0);
            SetRectangle(barFrame4Clicked, left: 0, top: 0, width: 92, height: 23);

            barFrameHover4 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarHover4", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrameHover4.SetPadding(0);
            SetRectangle(barFrameHover4, left: 0, top: 0, width: 96, height: 24);

            barFrame5 = new UIImage(Request<Texture2D>(Path + "ReaperClassBar5", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrame5.SetPadding(0);
            SetRectangle(barFrame5, left: 0, top: 0, width: 92, height: 23);

            barFrameHover5 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarHover5", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barFrameHover5.SetPadding(0);
            SetRectangle(barFrameHover5, left: 0, top: 0, width: 96, height: 24);

            #region BarSlots w/Models

            // Model 1
            barSlots = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlots1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots.SetPadding(0);
            SetRectangle(barSlots, left: 20, top: 39, width: 0, height: 0);
            // Model 2
            barSlots2 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlots2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots2.SetPadding(0);
            SetRectangle(barSlots2, left: 20, top: 39, width: 0, height: 0);
            // Model 3
            barSlots3 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlots3", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots3.SetPadding(0);
            SetRectangle(barSlots3, left: 20, top: 39, width: 0, height: 0);
            // Model 4
            barSlots4 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlots4", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots4.SetPadding(0);
            SetRectangle(barSlots4, left: 20, top: 39, width: 0, height: 0);
            // Model 5
            barSlots5 = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlots5", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlots5.SetPadding(0);
            SetRectangle(barSlots5, left: 20, top: 39, width: 0, height: 0);

            // Slots Outlines
            barSlotsNoJewel = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlotsUsed0", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlotsNoJewel.SetPadding(0);
            SetRectangle(barSlotsNoJewel, left: 20, top: 39, width: 0, height: 0);

            barSlotsJewelRight = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlotsUsed1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlotsJewelRight.SetPadding(0);
            SetRectangle(barSlotsJewelRight, left: 20, top: 39, width: 0, height: 0);

            barSlotsJewelLeft = new UIImage(Request<Texture2D>(Path + "ReaperClassBarSlotsUsed2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barSlotsJewelLeft.SetPadding(0);
            SetRectangle(barSlotsJewelLeft, left: 20, top: 39, width: 0, height: 0);

            #endregion

            // For dimensioning the actual bar being drawn when increasing the energy
            barDrawing = new UIImage(Request<Texture2D>("PenumbraMod/EMPTY", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            barDrawing.SetPadding(0);
            barDrawing.Color = new Color(0, 0, 0, 255) * 0f;
            SetRectangle(barDrawing, left: 0, top: 0, width: 83, height: 8);

            #region Crystalss
            AmethystCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AmethystCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AmethystCrystal.SetPadding(0);
            SetRectangle(AmethystCrystal, left: 22, top: 9, width: 10, height: 12);

            AmethystCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AmethystCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AmethystCrystalSlot2.SetPadding(0);
            SetRectangle(AmethystCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            TopazCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TopazCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            TopazCrystal.SetPadding(0);
            SetRectangle(TopazCrystal, left: 22, top: 9, width: 10, height: 12);

            TopazCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TopazCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            TopazCrystalSlot2.SetPadding(0);
            SetRectangle(TopazCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            SapphireCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SapphireCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SapphireCrystal.SetPadding(0);
            SetRectangle(SapphireCrystal, left: 22, top: 9, width: 10, height: 12);

            SapphireCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SapphireCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SapphireCrystalSlot2.SetPadding(0);
            SetRectangle(SapphireCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            EmeraldCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/EmeraldCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            EmeraldCrystal.SetPadding(0);
            SetRectangle(EmeraldCrystal, left: 22, top: 9, width: 10, height: 12);

            EmeraldCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/EmeraldCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            EmeraldCrystalSlot2.SetPadding(0);
            SetRectangle(EmeraldCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            RubyCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/RubyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            EmeraldCrystal.SetPadding(0);
            SetRectangle(EmeraldCrystal, left: 22, top: 9, width: 10, height: 12);

            RubyCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/RubyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            RubyCrystalSlot2.SetPadding(0);
            SetRectangle(RubyCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            DiamondCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DiamondCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            DiamondCrystal.SetPadding(0);
            SetRectangle(DiamondCrystal, left: 22, top: 9, width: 10, height: 12);

            DiamondCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DiamondCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            DiamondCrystalSlot2.SetPadding(0);
            SetRectangle(DiamondCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            MagicCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/MagicCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MagicCrystal.SetPadding(0);
            SetRectangle(MagicCrystal, left: 22, top: 9, width: 10, height: 12);

            MagicCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/MagicCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            MagicCrystalSlot2.SetPadding(0);
            SetRectangle(MagicCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            AzuriteCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AzuriteCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AzuriteCrystal.SetPadding(0);
            SetRectangle(AzuriteCrystal, left: 22, top: 9, width: 10, height: 12);

            AzuriteCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AzuriteCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AzuriteCrystalSlot2.SetPadding(0);
            SetRectangle(AzuriteCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            PrimeyeCrystal = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/ThePrimeyeCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            PrimeyeCrystal.SetPadding(0);
            SetRectangle(PrimeyeCrystal, left: 22, top: 9, width: 10, height: 12);

            PrimeyeCrystalSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/ThePrimeyeCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            PrimeyeCrystalSlot2.SetPadding(0);
            SetRectangle(PrimeyeCrystalSlot2, left: 61, top: 9, width: 10, height: 12);

            Ablazed = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AblazedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Ablazed.SetPadding(0);
            SetRectangle(Ablazed, left: 22, top: 9, width: 10, height: 12);

            AblazedSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AblazedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            AblazedSlot2.SetPadding(0);
            SetRectangle(AblazedSlot2, left: 61, top: 9, width: 10, height: 12);

            Slimy = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SlimyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Slimy.SetPadding(0);
            SetRectangle(Slimy, left: 22, top: 9, width: 10, height: 12);

            SlimySlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SlimyCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SlimySlot2.SetPadding(0);
            SetRectangle(SlimySlot2, left: 61, top: 9, width: 10, height: 12);

            Terra = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TerraCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Terra.SetPadding(0);
            SetRectangle(Terra, left: 22, top: 9, width: 10, height: 12);

            TerraSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/TerraCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            TerraSlot2.SetPadding(0);
            SetRectangle(TerraSlot2, left: 61, top: 9, width: 10, height: 12);

            Bloodstained = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/BloodstainedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Bloodstained.SetPadding(0);
            SetRectangle(Bloodstained, left: 22, top: 9, width: 10, height: 12);

            BloodstainedSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/BloodstainedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            BloodstainedSlot2.SetPadding(0);
            SetRectangle(BloodstainedSlot2, left: 61, top: 9, width: 10, height: 12);

            Darkened = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DarkenedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Darkened.SetPadding(0);
            SetRectangle(Darkened, left: 22, top: 9, width: 10, height: 12);

            DarkenedSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/DarkenedCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            DarkenedSlot2.SetPadding(0);
            SetRectangle(DarkenedSlot2, left: 61, top: 9, width: 10, height: 12);

            Spectre = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SpectreCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Spectre.SetPadding(0);
            SetRectangle(Spectre, left: 22, top: 9, width: 10, height: 12);

            SpectreSlot2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/SpectreCrystalMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            SpectreSlot2.SetPadding(0);
            SetRectangle(SpectreSlot2, left: 61, top: 9, width: 10, height: 12);

            Corr = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/CorrosiveJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Corr.SetPadding(0);
            SetRectangle(Corr, left: 22, top: 9, width: 10, height: 12);

            Corr2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/CorrosiveJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Corr2.SetPadding(0);
            SetRectangle(Corr2, left: 61, top: 9, width: 10, height: 12);

            Ony = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/OnyxJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Ony.SetPadding(0);
            SetRectangle(Ony, left: 22, top: 9, width: 10, height: 12);

            Ony2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/OnyxJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Ony2.SetPadding(0);
            SetRectangle(Ony2, left: 61, top: 9, width: 10, height: 12);

            Aqua = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AquamarineJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Aqua.SetPadding(0);
            SetRectangle(Aqua, left: 22, top: 9, width: 10, height: 12);

            Aqua2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/AquamarineJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Aqua2.SetPadding(0);
            SetRectangle(Aqua2, left: 61, top: 9, width: 10, height: 12);

            Roz = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/RozeQuartzJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Roz.SetPadding(0);
            SetRectangle(Ony, left: 22, top: 9, width: 10, height: 12);

            Roz2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/RozeQuartzJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Roz2.SetPadding(0);
            SetRectangle(Roz2, left: 61, top: 9, width: 10, height: 12);

            Peri = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/PeridotJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Peri.SetPadding(0);
            SetRectangle(Ony, left: 22, top: 9, width: 10, height: 12);

            Peri2 = new UIImage(Request<Texture2D>("PenumbraMod/Content/Items/ReaperJewels/PeridotJewelMini", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Peri2.SetPadding(0);
            SetRectangle(Peri2, left: 61, top: 9, width: 10, height: 12);
            #endregion

            // Text, obvious
            text = new UIText("", 0.8f); // text to show stat
            text.Width.Set(46, 0f);
            text.Height.Set(22, 0f);
            text.Top.Set(22, 0f);
            text.Left.Set(-38, 0f);

            // Text, obvious
            ModelText = new UIText("", 0.8f); // text to show stat
            ModelText.Width.Set(46, 0f);
            ModelText.Height.Set(22, 0f);
            ModelText.Top.Set(50, 0f);
            ModelText.Left.Set(-40, 0f);

            // Jewel slots
            item = new CrystalSlots();
            SetRectangle(item, left: 1, top: 50, width: 52, height: 150);

            item2 = new CrystalSlots2();
            SetRectangle(item2, left: 39, top: 50, width: 52, height: 150);

            // Buttons
            Asset<Texture2D> CrystalButtonn = ModContent.Request<Texture2D>(Path + "ReaperClassBarButton");
            CrystalButton = new ReaperButton(CrystalButtonn, LocalizedTextForReaperBar.Text2);
            SetRectangle(CrystalButton, left: 42, top: 7.5f, width: 10f, height: 13f);
            CrystalButton.OnLeftClick += new MouseEvent(ButtonClicked);
            CrystalButton.SetVisibility(1f, 0.65f);

            Asset<Texture2D> CrystalButtonn2 = ModContent.Request<Texture2D>(Path + "ReaperClassBarButton2");
            CrystalButton2 = new ReaperButton(CrystalButtonn2, LocalizedTextForReaperBar.Text2);
            SetRectangle(CrystalButton2, left: 42, top: 7.5f, width: 10f, height: 13f);
            CrystalButton2.OnLeftClick += new MouseEvent(ButtonClicked);
            CrystalButton2.SetVisibility(1f, 0.65f);

            Asset<Texture2D> CrystalButtonn3 = ModContent.Request<Texture2D>(Path + "ReaperClassBarButton3");
            CrystalButton3 = new ReaperButton(CrystalButtonn3, LocalizedTextForReaperBar.Text2);
            SetRectangle(CrystalButton3, left: 42, top: 7.5f, width: 10f, height: 13f);
            CrystalButton3.OnLeftClick += new MouseEvent(ButtonClicked);
            CrystalButton3.SetVisibility(1f, 0.65f);

            Asset<Texture2D> CrystalButtonn4 = ModContent.Request<Texture2D>(Path + "ReaperClassBarButton4");
            CrystalButton4 = new ReaperButton(CrystalButtonn4, LocalizedTextForReaperBar.Text2);
            SetRectangle(CrystalButton4, left: 39, top: 12f, width: 10f, height: 13f);
            CrystalButton4.OnLeftClick += new MouseEvent(ButtonClicked);
            CrystalButton4.SetVisibility(1f, 0.65f);

            Asset<Texture2D> CrystalButtonn4Clicked = ModContent.Request<Texture2D>(Path + "ReaperClassBarButton4Clicked");
            CrystalButton4Clicked = new ReaperButton(CrystalButtonn4Clicked, LocalizedTextForReaperBar.Text2);
            SetRectangle(CrystalButton4Clicked, left: 39, top: 12f, width: 10f, height: 13f);
            CrystalButton4Clicked.OnLeftClick += new MouseEvent(ButtonClicked);
            CrystalButton4Clicked.SetVisibility(1f, 0.65f);

            Asset<Texture2D> CrystalButtonn5 = ModContent.Request<Texture2D>(Path + "ReaperClassBarButton5");
            CrystalButton5 = new ReaperButton(CrystalButtonn5, LocalizedTextForReaperBar.Text2);
            SetRectangle(CrystalButton5, left: 39, top: 12f, width: 10f, height: 13f);
            CrystalButton5.OnLeftClick += new MouseEvent(ButtonClicked);
            CrystalButton5.SetVisibility(1f, 0.65f);

            Asset<Texture2D> ChangeStylebutton = ModContent.Request<Texture2D>(Path + "ChangeStyleButton");
            ChangeStyleButton = new ReaperButton(ChangeStylebutton, "");
            SetRectangle(ChangeStyleButton, left: 39, top: 12f, width: 10f, height: 13f);
            ChangeStyleButton.OnLeftClick += new MouseEvent(ModelClicked);
            ChangeStyleButton.SetVisibility(1f, 0.75f);

            Asset<Texture2D> DeleteChangeStylebutton = ModContent.Request<Texture2D>(Path + "DeleteChangeStyleButton");
            DeleteChangeStyleButton = new ReaperButton(DeleteChangeStylebutton, "");
            SetRectangle(DeleteChangeStyleButton, left: 39, top: 12f, width: 10f, height: 13f);
            DeleteChangeStyleButton.OnLeftClick += new MouseEvent(DeleteModelButton);
            DeleteChangeStyleButton.SetVisibility(1f, 0.75f);

            Asset<Texture2D> ArrowLeft = ModContent.Request<Texture2D>(Path + "ChangeStyleArrowLeft");
            ChangeStyleArrowLeft = new ReaperButton(ArrowLeft, "");
            SetRectangle(ChangeStyleArrowLeft, left: 39, top: 12f, width: 10f, height: 13f);
            ChangeStyleArrowLeft.OnLeftClick += new MouseEvent(ArrowClickedLeft);
            ChangeStyleArrowLeft.SetVisibility(1f, 0.75f);

            Asset<Texture2D> ArrowRight = ModContent.Request<Texture2D>(Path + "ChangeStyleArrowRight");
            ChangeStyleArrowRight = new ReaperButton(ArrowRight, "");
            SetRectangle(ChangeStyleArrowRight, left: 39, top: 12f, width: 10f, height: 13f);
            ChangeStyleArrowRight.OnLeftClick += new MouseEvent(ArrowClickedRight);
            ChangeStyleArrowRight.SetVisibility(1f, 0.75f);

            // Model 1
            gradientA = new Color(199, 60, 10);
            gradientB = new Color(134, 15, 46);
            // Model 2
            gradientA2 = new Color(103, 0, 209);
            gradientB2 = new Color(184, 60, 219);
            // Model 3
            gradientA3 = new Color(200, 159, 46);
            gradientB3 = new Color(255, 226, 148);
            // Model 4
            gradientA4 = new Color(232, 174, 0);
            gradientB4 = new Color(255, 230, 71);
            // Model 5
            gradientA5 = new Color(118, 118, 146);
            gradientB5 = new Color(226, 140, 113);

            // Now, for appending!

            // Append text and slots
            Append(panel);
            Append(panel2);
            panel2.Append(item);
            panel2.Append(item2);
            panel2.Append(text);
            panel2.Append(ModelText);

            // The slots below the bar, not the outlined ones
            panel2.Append(barSlots);
            panel2.Append(barSlots2);
            panel2.Append(barSlots3);
            panel2.Append(barSlots4);
            panel2.Append(barSlots5);

            // Models
            panel2.Append(barFrame);
            panel2.Append(barFrameHover);
            panel2.Append(barFrame2);
            panel2.Append(barFrameHover2);
            panel2.Append(barFrame3);
            panel2.Append(barFrameHover3);
            panel2.Append(barFrame4);
            panel2.Append(barFrame4Clicked);
            panel2.Append(barFrameHover4);
            panel2.Append(barFrame5);
            panel2.Append(barFrameHover5);

            // Jewel Slots, the outlined ones
            panel2.Append(barSlotsNoJewel);
            panel2.Append(barSlotsJewelRight);
            panel2.Append(barSlotsJewelLeft);

            // Buttons and the bar thingy kljhdfgabkhg nbdzbxhk nbdfk bhjnzvxkbhjldzvhsovgad
            panel2.Append(barDrawing);
            panel2.Append(CrystalButton);
            panel2.Append(CrystalButton2);
            panel2.Append(CrystalButton3);
            panel2.Append(CrystalButton4);
            panel2.Append(CrystalButton4Clicked);
            panel2.Append(CrystalButton5);
            panel.Append(ChangeStyleArrowLeft);
            panel.Append(ChangeStyleArrowRight);
            panel.Append(ChangeStyleButton);
            panel.Append(DeleteChangeStyleButton);
            #region Crystalsss
            // JEWELS!!!!!!!!!! b
            panel2.Append(AmethystCrystal);
            panel2.Append(AmethystCrystalSlot2);
            panel2.Append(TopazCrystal);
            panel2.Append(TopazCrystalSlot2);
            panel2.Append(SapphireCrystal);
            panel2.Append(SapphireCrystalSlot2);
            panel2.Append(EmeraldCrystal);
            panel2.Append(EmeraldCrystalSlot2);
            panel2.Append(RubyCrystal);
            panel2.Append(RubyCrystalSlot2);
            panel2.Append(DiamondCrystal);
            panel2.Append(DiamondCrystalSlot2);
            panel2.Append(MagicCrystal);
            panel2.Append(MagicCrystalSlot2);
            panel2.Append(AzuriteCrystal);
            panel2.Append(AzuriteCrystalSlot2);
            panel2.Append(PrimeyeCrystal);
            panel2.Append(PrimeyeCrystalSlot2);
            panel2.Append(Spectre);
            panel2.Append(SpectreSlot2);
            panel2.Append(Slimy);
            panel2.Append(SlimySlot2);
            panel2.Append(Bloodstained);
            panel2.Append(BloodstainedSlot2);
            panel2.Append(Darkened);
            panel2.Append(DarkenedSlot2);
            panel2.Append(Terra);
            panel2.Append(TerraSlot2);
            panel2.Append(Ablazed);
            panel2.Append(AblazedSlot2);
            panel2.Append(Corr);
            panel2.Append(Corr2);
            panel2.Append(Ony);
            panel2.Append(Ony2);
            panel2.Append(Peri);
            panel2.Append(Peri2);
            panel2.Append(Aqua);
            panel2.Append(Aqua2);
            panel2.Append(Roz);
            panel2.Append(Roz2);
            #endregion
        }
        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
        private void ButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (clickedagain)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                clickedagain = false;
                Main.playerInventory = false;
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                clickedagain = true;
                Main.playerInventory = true;
            }
        }
        private void ModelClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            if (showModelChangeArrows)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                showModelChangeArrows = false;
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                showModelChangeArrows = true;
            }

        }
        private void DeleteModelButton(UIMouseEvent evt, UIElement listeningElement)
        {
            GetInstance<PenumbraConfig>().ModelChange = false;
            PenumbraMod.SaveConfig(PenumbraConfig.Instance);
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
        private void ArrowClickedRight(UIMouseEvent evt, UIElement listeningElement)
        {
            GetInstance<PenumbraConfig>().ReaperBarModel += 1;
            PenumbraMod.SaveConfig(PenumbraConfig.Instance);
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
        private void ArrowClickedLeft(UIMouseEvent evt, UIElement listeningElement)
        {
            GetInstance<PenumbraConfig>().ReaperBarModel -= 1;
            PenumbraMod.SaveConfig(PenumbraConfig.Instance);
            SoundEngine.PlaySound(SoundID.MenuTick);
        }
        #region Crystals
        // 1st slot
        public bool amycryst = false;
        public bool emecryst = false;
        public bool magcryst = false;
        public bool rubycryst = false;
        public bool saphcryst = false;
        public bool diamcryst = false;
        public bool topcryst = false;
        public bool azucryst = false;
        public bool pricryst = false;
        public bool blood = false;
        public bool abla = false;
        public bool terra = false;
        public bool spec = false;
        public bool darke = false;
        public bool slim = false;
        public bool corr = false;
        public bool ony = false;
        public bool aqua = false;
        public bool peri = false;
        public bool roz = false;

        //2nd slot
        public bool amycryst2 = false;
        public bool emecryst2 = false;
        public bool magcryst2 = false;
        public bool rubycryst2 = false;
        public bool saphcryst2 = false;
        public bool diamcryst2 = false;
        public bool topcryst2 = false;
        public bool azucryst2 = false;
        public bool pricryst2 = false;
        public bool blood2 = false;
        public bool abla2 = false;
        public bool terra2 = false;
        public bool spec2 = false;
        public bool darke2 = false;
        public bool slim2 = false;
        public bool corr2 = false;
        public bool ony2 = false;
        public bool aqua2 = false;
        public bool peri2 = false;
        public bool roz2 = false;
        #endregion
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            // Another painful part
            if (Main.LocalPlayer.HeldItem.DamageType != GetInstance<ReaperClass>())
                return;
            int Model = GetInstance<PenumbraConfig>().ReaperBarModel;
            if (!Main.playerInventory)
                clickedagain = false;
            SetRectangle(panel, left: panel2.GetDimensions().X, top: panel2.GetDimensions().Y, width: 120, height: 120); // Since the model change buttons need a "parent" panel to properly work, then make the panel follow the main one
            #region Bar
            #region OutlinedSlots
            if (clickedagain && Main.playerInventory && item.Item.type == ItemID.None && item2.Item.type == ItemID.None)
                SetRectangle(barSlotsNoJewel, left: 20, top: 39, width: 52, height: 42);
            else
                SetRectangle(barSlotsNoJewel, left: 2145623, top: 1613446, width: 0, height: 0);

            if (clickedagain && Main.playerInventory && item.Item.type != ItemID.None && item2.Item.type == ItemID.None)
                SetRectangle(barSlotsJewelRight, left: 20, top: 39, width: 52, height: 42);
            else
                SetRectangle(barSlotsJewelRight, left: 2145623, top: 1613446, width: 0, height: 0);

            if (clickedagain && Main.playerInventory && item.Item.type == ItemID.None && item2.Item.type != ItemID.None)
                SetRectangle(barSlotsJewelLeft, left: 20, top: 39, width: 52, height: 42);
            else
                SetRectangle(barSlotsJewelLeft, left: 2145623, top: 1613446, width: 0, height: 0);

            if (clickedagain && Main.playerInventory && item.Item.type != ItemID.None && item2.Item.type != ItemID.None)
            {
                SetRectangle(barSlotsNoJewel, left: 2145623, top: 1613446, width: 0, height: 0);
                SetRectangle(barSlotsJewelLeft, left: 2145623, top: 1613446, width: 0, height: 0);
                SetRectangle(barSlotsJewelRight, left: 2145623, top: 1613446, width: 0, height: 0);
            }
            #endregion
            #region Model 1
            if (Model == 1)
            {
                SetRectangle(barDrawing, left: 0, top: 2, width: 80, height: 12);
                SetRectangle(barFrame, left: 0, top: 0, width: 92, height: 23);
                SetRectangle(CrystalButton, left: 42, top: 7.5f, width: 10f, height: 13f);
                if (panel2.IsMouseHovering)
                    SetRectangle(barFrameHover, left: -1, top: -1, width: 92, height: 23);
                else
                    SetRectangle(barFrameHover, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            else
            {
                SetRectangle(barFrame, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(CrystalButton, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(barFrameHover, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            if (Model == 1 && clickedagain && Main.playerInventory) // This is separated because for some reason it was drawing everytime
                SetRectangle(barSlots, left: 20, top: 39, width: 0, height: 0);
            else
                SetRectangle(barSlots, left: 2145623, top: 1613446, width: 0, height: 0);
            #endregion
            #region Model 2
            if (Model == 2)
            {
                SetRectangle(barDrawing, left: 0, top: 4, width: 84, height: 8);
                SetRectangle(barFrame2, left: 0, top: 0, width: 92, height: 23);
                SetRectangle(CrystalButton2, left: 42, top: 7.5f, width: 10f, height: 13f);
                if (panel2.IsMouseHovering)
                    SetRectangle(barFrameHover2, left: -1, top: -1, width: 92, height: 23);
                else
                    SetRectangle(barFrameHover2, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            else
            {
                SetRectangle(barFrame2, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(CrystalButton2, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(barFrameHover2, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            if (Model == 2 && clickedagain && Main.playerInventory) // This is separated because for some reason it was drawing everytime
                SetRectangle(barSlots2, left: 20, top: 39, width: 0, height: 0);
            else
                SetRectangle(barSlots2, left: 2145623, top: 1613446, width: 0, height: 0);
            #endregion
            #region Model 3
            if (Model == 3)
            {
                SetRectangle(barDrawing, left: 2, top: 0, width: 82, height: 12);
                SetRectangle(barFrame3, left: 0, top: 0, width: 92, height: 23);
                SetRectangle(CrystalButton3, left: 42, top: 7.5f, width: 10f, height: 13f);
                if (panel2.IsMouseHovering)
                    SetRectangle(barFrameHover3, left: -1, top: -1, width: 92, height: 23);
                else
                    SetRectangle(barFrameHover3, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            else
            {
                SetRectangle(barFrame3, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(CrystalButton3, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(barFrameHover3, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            if (Model == 3 && clickedagain && Main.playerInventory) // This is separated because for some reason it was drawing everytime
                SetRectangle(barSlots3, left: 20, top: 38, width: 0, height: 0);
            else
                SetRectangle(barSlots3, left: 2145623, top: 1613446, width: 0, height: 0);
            #endregion
            #region Model 4
            if (Model == 4)
            {
                SetRectangle(barDrawing, left: 2, top: 2, width: 83, height: 8);
                if (!clickedagain)
                {
                    SetRectangle(barFrame4, left: 2, top: 0, width: 92, height: 23);
                    SetRectangle(barFrame4Clicked, left: 1235451, top: 13455143, width: 92, height: 23);
                    SetRectangle(CrystalButton4, left: 39, top: 9f, width: 10f, height: 13f);
                    SetRectangle(CrystalButton4Clicked, left: 1235451, top: 13455143, width: 92, height: 23);
                }
                else
                {
                    SetRectangle(barFrame4, left: 1235451, top: 13455143, width: 92, height: 23);
                    SetRectangle(barFrame4Clicked, left: 2, top: 0, width: 92, height: 23);
                    SetRectangle(CrystalButton4, left: 1235451, top: 13455143, width: 92, height: 23);
                    SetRectangle(CrystalButton4Clicked, left: 39, top: 9f, width: 10f, height: 13f);
                }
                if (panel2.IsMouseHovering)
                    SetRectangle(barFrameHover4, left: -1, top: -1, width: 92, height: 23);
                else
                    SetRectangle(barFrameHover4, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            else
            {
                SetRectangle(barFrame4, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(barFrame4Clicked, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(CrystalButton4, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(CrystalButton4Clicked, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(barFrameHover4, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            if (Model == 4 && clickedagain && Main.playerInventory) // This is separated because for some reason it was drawing everytime
                SetRectangle(barSlots4, left: 20, top: 38, width: 0, height: 0);
            else
                SetRectangle(barSlots4, left: 2145623, top: 1613446, width: 0, height: 0);
            #endregion
            #region Model 5
            if (Model == 5)
            {
                SetRectangle(barDrawing, left: 3, top: 0, width: 77, height: 12);
                SetRectangle(barFrame5, left: 0, top: -2, width: 92, height: 23);
                SetRectangle(CrystalButton5, left: 40, top: 8.5f, width: 10f, height: 13f);
                if (panel2.IsMouseHovering)
                    SetRectangle(barFrameHover5, left: -1, top: -1, width: 92, height: 23);
                else
                    SetRectangle(barFrameHover5, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            else
            {
                SetRectangle(barFrame5, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(CrystalButton5, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(barFrameHover5, left: 34663411346, top: 13464631346, width: 92, height: 23);
            }
            if (Model == 5 && clickedagain && Main.playerInventory) // This is separated because for some reason it was drawing everytime
                SetRectangle(barSlots5, left: 20, top: 39, width: 0, height: 0);
            else
                SetRectangle(barSlots5, left: 2145623, top: 1613446, width: 0, height: 0);
            #endregion
            #endregion
            #region Crystals
            if (item.Item.type == ItemType<AmythestCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst = true;
                amycryst = true;
                SetRectangle(AmethystCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                //  Main.LocalPlayer.ClearBuff(BuffType<AmethystForce>());
                amycryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst = false;
                SetRectangle(AmethystCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<AmythestCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst2 = true;
                amycryst2 = true;
                SetRectangle(AmethystCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                //  Main.LocalPlayer.ClearBuff(BuffType<AmethystForce>());
                amycryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().amycryst2 = false;
                SetRectangle(AmethystCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<TopazCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst = true;
                topcryst = true;
                SetRectangle(TopazCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<TopazForce>());
                topcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst = false;
                SetRectangle(TopazCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<TopazCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst2 = true;
                topcryst2 = true;
                SetRectangle(TopazCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<TopazForce>());
                topcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().topcryst2 = false;
                SetRectangle(TopazCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<SapphireCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst = true;
                saphcryst = true;
                SetRectangle(SapphireCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                //  Main.LocalPlayer.ClearBuff(BuffType<SapphireForce>());
                saphcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst = false;
                SetRectangle(SapphireCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<SapphireCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst2 = true;
                saphcryst2 = true;
                SetRectangle(SapphireCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                //   Main.LocalPlayer.ClearBuff(BuffType<SapphireForce>());
                saphcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().saphcryst2 = false;
                SetRectangle(SapphireCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<EmeraldCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst = true;
                emecryst = true;
                SetRectangle(EmeraldCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<EmeraldForce>());
                emecryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst = false;
                SetRectangle(EmeraldCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<EmeraldCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst2 = true;
                emecryst2 = true;
                SetRectangle(EmeraldCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<EmeraldForce>());
                emecryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().emecryst2 = false;
                SetRectangle(EmeraldCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<RubyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst = true;
                rubycryst = true;
                SetRectangle(RubyCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<RubyForce>());
                rubycryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst = false;
                SetRectangle(RubyCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<RubyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst2 = true;
                rubycryst2 = true;
                SetRectangle(RubyCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<RubyForce>());
                rubycryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().rubycryst2 = false;
                SetRectangle(RubyCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<DiamondCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst = true;
                diamcryst = true;
                SetRectangle(DiamondCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<DiamondForce>());
                diamcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst = false;
                SetRectangle(DiamondCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<DiamondCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst2 = true;
                diamcryst2 = true;
                SetRectangle(DiamondCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<DiamondForce>());
                diamcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().diamcryst2 = false;
                SetRectangle(DiamondCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<MagicCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst = true;
                magcryst = true;
                SetRectangle(MagicCrystal, left: 21, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<MagicForce>());
                magcryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst = false;
                SetRectangle(MagicCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<MagicCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst2 = true;
                magcryst2 = true;
                SetRectangle(MagicCrystalSlot2, left: 58, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<MagicForce>());
                magcryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().magcryst2 = false;
                SetRectangle(MagicCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<AzuriteCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst = true;
                azucryst = true;
                SetRectangle(AzuriteCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                // Main.LocalPlayer.ClearBuff(BuffType<AzuriteForce>());
                azucryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst = false;
                SetRectangle(AzuriteCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<AzuriteCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst2 = true;
                azucryst2 = true;
                SetRectangle(AzuriteCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<AzuriteForce>());
                azucryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().azucryst2 = false;
                SetRectangle(AzuriteCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<ThePrimeyeCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst = true;
                pricryst = true;
                SetRectangle(PrimeyeCrystal, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<PrimeyeForce>());
                pricryst = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst = false;
                SetRectangle(PrimeyeCrystal, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<ThePrimeyeCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst2 = true;
                pricryst2 = true;
                SetRectangle(PrimeyeCrystalSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                //Main.LocalPlayer.ClearBuff(BuffType<PrimeyeForce>());
                pricryst2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().pricryst2 = false;
                SetRectangle(PrimeyeCrystalSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<AblazedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla = true;
                abla = true;
                SetRectangle(Ablazed, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                abla = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla = false;
                SetRectangle(Ablazed, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<AblazedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla2 = true;
                abla2 = true;
                SetRectangle(AblazedSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                abla2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().abla2 = false;
                SetRectangle(AblazedSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<SlimyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim = true;
                slim = true;
                SetRectangle(Slimy, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                slim = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim = false;
                SetRectangle(Slimy, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<SlimyCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim2 = true;
                slim2 = true;
                SetRectangle(SlimySlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                slim2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().slim2 = false;
                SetRectangle(SlimySlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<TerraCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra = true;
                terra = true;
                SetRectangle(Terra, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                terra = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra = false;
                SetRectangle(Terra, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<TerraCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra2 = true;
                terra2 = true;
                SetRectangle(TerraSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                terra2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().terra2 = false;
                SetRectangle(TerraSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<BloodstainedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood = true;
                blood = true;
                SetRectangle(Bloodstained, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                blood = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood = false;
                SetRectangle(Bloodstained, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<BloodstainedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood2 = true;
                blood2 = true;
                SetRectangle(BloodstainedSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                blood2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().blood2 = false;
                SetRectangle(BloodstainedSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<DarkenedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke = true;
                darke = true;
                SetRectangle(Darkened, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                darke = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke = false;
                SetRectangle(Darkened, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<DarkenedCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke = true;
                darke2 = true;
                SetRectangle(DarkenedSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                darke2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().darke2 = false;
                SetRectangle(DarkenedSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<SpectreCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec = true;
                spec = true;
                SetRectangle(Spectre, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                spec = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec = false;
                SetRectangle(Spectre, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<SpectreCrystal>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec2 = true;
                spec2 = true;
                SetRectangle(SpectreSlot2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                spec2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().spec2 = false;
                SetRectangle(SpectreSlot2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<CorrosiveJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr = true;
                corr = true;
                SetRectangle(Corr, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                corr = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr = false;
                SetRectangle(Corr, left: 1235451, top: 13455143, width: 10, height: 12);
            }
            if (item2.Item.type == ItemType<CorrosiveJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr2 = true;
                corr2 = true;
                SetRectangle(Corr2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                corr2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().corr2 = false;
                SetRectangle(Corr2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<OnyxJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony = true;
                ony = true;
                SetRectangle(Ony, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                ony = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony = false;
                SetRectangle(Ony, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item2.Item.type == ItemType<OnyxJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony2 = true;
                ony2 = true;
                SetRectangle(Ony2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                ony2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ony2 = false;
                SetRectangle(Ony2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<PeridotJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().peri = true;
                peri = true;
                SetRectangle(Peri, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                peri = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().peri = false;
                SetRectangle(Peri, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item2.Item.type == ItemType<PeridotJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().peri2 = true;
                peri2 = true;
                SetRectangle(Peri2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                peri2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().peri2 = false;
                SetRectangle(Peri2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<AquamarineJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().aqua = true;
                aqua = true;
                SetRectangle(Aqua, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                aqua = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().aqua = false;
                SetRectangle(Aqua, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item2.Item.type == ItemType<AquamarineJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().aqua2 = true;
                aqua2 = true;
                SetRectangle(Aqua2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                aqua2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().aqua2 = false;
                SetRectangle(Aqua2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item.Item.type == ItemType<RozeQuartzJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 3000)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().roz = true;
                roz = true;
                SetRectangle(Roz, left: 23, top: 8, width: 10, height: 12);
            }
            else
            {
                roz = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().roz = false;
                SetRectangle(Roz, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            if (item2.Item.type == ItemType<RozeQuartzJewel>())
            {
                if (Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().ReaperEnergy > 7300)
                    Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().roz2 = true;
                roz2 = true;
                SetRectangle(Roz2, left: 60, top: 8, width: 10, height: 12);
            }
            else
            {
                roz2 = false;
                Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>().roz2 = false;
                SetRectangle(Roz2, left: 1235451, top: 13455143, width: 10, height: 12);
            }

            #endregion
            #region Model Buttons

            if (ChangeStyleButton.IsMouseHovering)
            {
                Main.instance.MouseText((string)LocalizedTextForReaperBar.Text3);
                Main.LocalPlayer.mouseInterface = true;
            }

            if (DeleteChangeStyleButton.IsMouseHovering)
            {
                Main.instance.MouseText((string)LocalizedTextForReaperBar.Text4);
                Main.LocalPlayer.mouseInterface = true;
            }

            if (ChangeStyleArrowRight.IsMouseHovering)
            {
                Main.instance.MouseText("");
                Main.LocalPlayer.mouseInterface = true;
            }

            if (ChangeStyleArrowLeft.IsMouseHovering)
            {
                Main.instance.MouseText("");
                Main.LocalPlayer.mouseInterface = true;
            }


            if (GetInstance<PenumbraConfig>().ModelChange && clickedagain)
                SetRectangle(ChangeStyleButton, left: 80, top: 38f, width: 20f, height: 24f);
            else
                SetRectangle(ChangeStyleButton, left: 1235451, top: 13455143, width: 92, height: 23);

            if (GetInstance<PenumbraConfig>().ModelChange && showModelChangeArrows && clickedagain)
            {
                SetRectangle(DeleteChangeStyleButton, left: 110, top: 43f, width: 10f, height: 13f);

                if (Model != 5)
                    SetRectangle(ChangeStyleArrowRight, left: 61, top: 90f, width: 10f, height: 14f);
                else
                    SetRectangle(ChangeStyleArrowRight, left: 1235451, top: 13455143, width: 92, height: 23);

                if (Model != 1)
                    SetRectangle(ChangeStyleArrowLeft, left: 22, top: 90f, width: 10f, height: 14f);
                else
                    SetRectangle(ChangeStyleArrowLeft, left: 1235451, top: 13455143, width: 92, height: 23);
            }
            else
            {
                SetRectangle(DeleteChangeStyleButton, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(ChangeStyleArrowLeft, left: 1235451, top: 13455143, width: 92, height: 23);
                SetRectangle(ChangeStyleArrowRight, left: 1235451, top: 13455143, width: 92, height: 23);
            }

            #endregion
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            var ReaperClassPlayer = Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>();
            int Model = GetInstance<PenumbraConfig>().ReaperBarModel;
            // Calculate quotient
            float quotient = (float)ReaperClassPlayer.ReaperEnergy / ReaperClassPlayer.ReaperEnergyMax; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
            quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = barDrawing.GetInnerDimensions().ToRectangle();
            hitbox.X += 1;
            hitbox.Width += 1;
            hitbox.Y += 1;
            hitbox.Height += 1;

            // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                //float percent = (float)i / steps; // Alternate Gradient Approach
                float percent = (float)i / (right - left);
                if (Model == 1)
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i + 2, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
                if (Model == 2)
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i + 2, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA2, gradientB2, percent));
                if (Model == 3)
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i + 2, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA3, gradientB3, percent));
                if (Model == 4)
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i + 1, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA4, gradientB4, percent));
                if (Model == 5)
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i + 1, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA5, gradientB5, percent));
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.HeldItem.DamageType != GetInstance<ReaperClass>())
                return;
            var ReaperClassPlayer = Main.LocalPlayer.GetModPlayer<ReaperClassDPlayer>();
            int Model = GetInstance<PenumbraConfig>().ReaperBarModel;
            // Setting the text per tick to update and show our resource values.
            if (GetInstance<PenumbraConfig>().UITEXT)
                text.SetText(LocalizedTextForReaperBar.Text + $" {ReaperClassPlayer.ReaperEnergy / 10f} / {ReaperClassPlayer.ReaperEnergyMax / 10f}");
            if (GetInstance<PenumbraConfig>().ModelChange && showModelChangeArrows && clickedagain)
            {
                if (Model == 1)
                    ModelText.SetText(LocalizedTextForReaperBar.Model1);
                if (Model == 2)
                    ModelText.SetText(LocalizedTextForReaperBar.Model2);
                if (Model == 3)
                    ModelText.SetText(LocalizedTextForReaperBar.Model3);
                if (Model == 4)
                    ModelText.SetText(LocalizedTextForReaperBar.Model4);
                if (Model == 5)
                    ModelText.SetText(LocalizedTextForReaperBar.Model5);
            }
            else
                ModelText.SetText("");

            base.Update(gameTime);
        }
    }
    class LocalizedTextForReaperBar : ModSystem
    {
        public static LocalizedText Text { get; private set; }
        public static LocalizedText Text2 { get; private set; }
        public static LocalizedText Text3 { get; private set; }
        public static LocalizedText Text4 { get; private set; }
        public static LocalizedText Model1 { get; private set; }
        public static LocalizedText Model2 { get; private set; }
        public static LocalizedText Model3 { get; private set; }
        public static LocalizedText Model4 { get; private set; }
        public static LocalizedText Model5 { get; private set; }
        public override void Load()
        {
            string category = "UI";
            Text ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ReaperBar"));
            Text2 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ReaperBarJewel"));
            Text3 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ReaperBarModel"));
            Text4 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.ReaperBarModelDelete"));
            Model1 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Model1"));
            Model2 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Model2"));
            Model3 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Model3"));
            Model4 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Model4"));
            Model5 ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}.Model5"));
        }
    }
    internal class ReaperButton : UIImageButton
    {
        // Tooltip text that will be shown on hover
        internal string hoverText;
        internal static LocalizedText text;
        public ReaperButton(Asset<Texture2D> texture, string hoverText) : base(texture)
        {
            this.hoverText = hoverText;
        }
        public ReaperButton(Asset<Texture2D> texture, LocalizedText Text) : base(texture)
        {
            text = Text;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.DrawSelf(spriteBatch);

            // IsMouseHovering becomes true when the mouse hovers over the current UIElement
            if (IsMouseHovering)
            {
                Main.instance.MouseText(hoverText);
                Main.instance.MouseText((string)text);
            }

        }
    }
    internal class CrystalSlots : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public CrystalSlots(int context = ItemSlot.Context.BankItem, float scale = 0f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults();
            Width.Set(52, 0f);
            Height.Set(150, 0f);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            if (ReaperUI.clickedagain && Main.playerInventory)
            {
                ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
                spriteBatch.Draw(TextureAssets.Item[Item.type].Value, rectangle.TopLeft() + new Vector2(17, 6), Color.White);
            }

            Main.inventoryScale = oldScale;

        }
    }
    internal class CrystalSlots2 : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        internal Func<Item, bool> ValidItemFunc;

        public CrystalSlots2(int context = ItemSlot.Context.BankItem, float scale = 0f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(0);
            Width.Set(52, 0f);
            Height.Set(150, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    // Handle handles all the click and hover actions based on the context.
                    ItemSlot.Handle(ref Item, _context);
                }
            }
            // Draw draws the slot itself and Item. Depending on context, the color will change, as will drawing other things like stack counts.
            if (ReaperUI.clickedagain && Main.playerInventory)
            {
                ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());
                spriteBatch.Draw(TextureAssets.Item[Item.type].Value, rectangle.TopLeft() + new Vector2(20, 6), Color.White);
            }

            Main.inventoryScale = oldScale;

        }
    }
    public class DraggableUI : UIPanel
    {
        // Stores the offset from the top left of the UIPanel while dragging
        private Vector2 offset;
        // A flag that checks if the panel is currently being dragged
        private bool dragging;

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.LeftMouseDown(evt);
            // When the mouse button is down, then we start dragging
            DragStart(evt);
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            // When the mouse button is up, then we stop dragging
            DragEnd(evt);
        }

        private void DragStart(UIMouseEvent evt)
        {
            // The offset variable helps to remember the position of the panel relative to the mouse position
            // So no matter where you start dragging the panel, it will move smoothly
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt)
        {
            Vector2 endMousePosition = evt.MousePosition;
            dragging = false;

            Left.Set(endMousePosition.X - offset.X, 0f);
            Top.Set(endMousePosition.Y - offset.Y, 0f);

            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Checking ContainsPoint and then setting mouseInterface to true is very common
            // This causes clicks on this UIElement to not cause the player to use current items

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();
            }

            // Here we check if the DragableUIPanel is outside the Parent UIElement rectangle
            // (In our example, the parent would be ExampleCoinsUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
            // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                // Recalculate forces the UI system to do the positioning math again.
                Recalculate();
            }
        }
    }

}
