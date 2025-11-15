using RedLoader;
using SUI;

namespace ArmorMod;

public static class Config
{
    public static ConfigCategory Category { get; private set; }

    //public static ConfigEntry<bool> SomeEntry { get; private set; }
    public static ConfigEntry<bool> enableHelmet { get; private set; }
    public static ConfigEntry<bool> cutsceneHelmet { get; private set; }
    public static ConfigEntry<bool> hideBackpack { get; private set; }
    // Auto populated after calling SettingsRegistry.CreateSettings...
    private static SettingsRegistry.SettingsEntry _settingsEntry;

    public static void Init()
    {
        Category = ConfigSystem.CreateFileCategory("ArmorMod", "ArmorMod", "ArmorMod.cfg");

        enableHelmet = Category.CreateEntry(
            "enableHelmet",
            true,
            "Enable helmet model?",
            "Enables helmet model on other players. Only applies when player model is spawned in, I.E. when a player joins."
            );
        cutsceneHelmet = Category.CreateEntry(
            "cutsceneHelmet",
            false,
            "Use the cutscene helmet model?",
            "Enabling this swaps the model from the hemlet model to the cutscene helmet model. Only applies when player model is spawned in, I.E. when a player joins."
            );
       hideBackpack = Category.CreateEntry(
            "hideBackpack",
            false,
            "Hide player backpacks",
            "Hides the backpacks of other players, good for if you want the Trailer 1 look when combined with the helmet"
            ); 
      
        // SomeEntry = Category.CreateEntry(
        //     "some_entry",
        //     true,
        //     "Some entry",
        //     "Some entry that does some stuff.");
    }

    // Same as the callback in "CreateSettings". Called when the settings ui is closed.
    public static void OnSettingsUiClosed()
    {
    }
}