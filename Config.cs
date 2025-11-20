using RedLoader;
using SUI;

namespace ArmorMod;

public static class Config
{
    public static ConfigCategory Category { get; private set; }

    //public static ConfigEntry<bool> SomeEntry { get; private set; }
    public static ConfigEntry<bool> enableBodyArmor { get; private set; }
    public static ConfigEntry<bool> enableHelmet { get; private set; }
    public static ConfigEntry<bool> cutsceneHelmet { get; private set; }
    public static ConfigEntry<bool> useGloves { get; private set; }
    public static ConfigEntry<bool> useNameTags { get; private set; }
    public static ConfigEntry<bool> enableRobbyHelmet { get; private set; }
    public static ConfigEntry<bool> hideBackpack { get; private set; }
    public static ConfigEntry<bool> useOldJacket { get; private set; }
    public static ConfigEntry<bool> useMasks { get; private set; }
    public static ConfigEntry<bool> useGlasses { get; private set; }
    public static ConfigEntry<bool> hideArmorSystem { get; private set; }
    // Auto populated after calling SettingsRegistry.CreateSettings...
    private static SettingsRegistry.SettingsEntry _settingsEntry;

    public static void Init()
    {
        Category = ConfigSystem.CreateFileCategory("ArmorMod", "Tactical Gear Mod Settings", "ArmorMod.cfg");

        enableBodyArmor = Category.CreateEntry(
            "enableBodyArmor",
            true,
            "Enable body armor?",
            "Enables body armor on you and other players. Requires reload."
            );
        enableHelmet = Category.CreateEntry(
            "enableHelmet",
            true,
            "Enable helmet model?",
            "Enables 'hemlet' model like those seen on dead tacticals on other players. Requires reload."
            );
        cutsceneHelmet = Category.CreateEntry(
            "cutsceneHelmet",
            true,
            "Use the cutscene helmet model?",
            "Enabling this swaps the model from the hemlet model to the cutscene helmet model. Requires reload."
            );
        useGloves = Category.CreateEntry(
            "useGloves",
            true,
            "Replace hands with gloves?",
            "Requires reload."
            );
        useNameTags = Category.CreateEntry(
            "useNameTags",
            true,
            "Enables name tag models on other players.",
            "Requires game reload."
            );
        enableRobbyHelmet = Category.CreateEntry(
            "enableRobbyHelmet",
            true,
            "Enable Kelvin's intro helmet?",
            "Requires reload."
            );
        hideBackpack = Category.CreateEntry(
            "hideBackpack",
            true,
            "Hide player backpacks",
            "Hides the backpacks of other players for a cleaner look. Requires reload."
            );
        useOldJacket = Category.CreateEntry(
            "useOldJacket",
            false,
            "Use Kelvin jacket for players?",
            "Replaces player jacket model with the one Kelvin and other tacticals wear. Requires reload."
            );
        useMasks = Category.CreateEntry(
            "useMasks",
            false,
            "Add balaclavas?",
            "Adds balaclavas to other players. Limits race system to White or Latin due to mesh clipping issues. Requires reload."
            );
        useGlasses = Category.CreateEntry(
            "useGlasses",
            false,
            "Add glasses?",
            "Adds sunglasses to other players. Requires reload."
            );
        hideArmorSystem = Category.CreateEntry(
            "hideArmorSystem",
            false,
            "Hide equipped armor pieces?",
            "Hides equipped armor, such as bone or creepy armor."
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