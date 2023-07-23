using Terraria.ModLoader;

namespace PenumbraMod.Common.Players
{
	// Acts as a container for keybinds registered by this mod.
	// See Common/Players/ExampleKeybindPlayer for usage.
	public class MarshmellowKeybindSystem : ModSystem
	{
		public static ModKeybind MarshmellowArmorKeybind { get; private set; }

		public override void Load() {
            // Registers a new keybind
            MarshmellowArmorKeybind = KeybindLoader.RegisterKeybind(Mod, "Comfortably Protected Buff", "L");
		}

		// Please see ExampleMod.cs' Unload() method for a detailed explanation of the unloading process.
		public override void Unload() {
            // Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
            MarshmellowArmorKeybind = null;
		}
	}
}
