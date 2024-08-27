using BepInEx.Configuration;

namespace ImprovedClock;

public static class ConfigManager {
    // This variable name is a joke btw. I don't have anything against anyone.
    public static ConfigEntry<bool> enableNormalHumanBeingClock = null!;
    public static ConfigEntry<bool> enableRealtimeClock = null!;

    public static ConfigEntry<bool> showClockInShip = null!;
    public static ConfigEntry<float> clockVisibilityInShip = null!;

    public static ConfigEntry<bool> showClockInFacility = null!;
    public static ConfigEntry<float> clockVisibilityInFacility = null!;

    public static ConfigEntry<bool> showClockInSpectator = null!;
    public static ConfigEntry<float> clockVisibilityInSpectator = null!;

    public static ConfigEntry<float> clockColorRed = null!;
    public static ConfigEntry<float> clockColorGreen = null!;
    public static ConfigEntry<float> clockColorBlue = null!;

    public static void Initialize(ConfigFile configFile) {
        enableNormalHumanBeingClock = configFile.Bind("General", "Enable 24 hour clock", true,
                                                      "If true, will make the clock display 24 hour time instead of 12 hour time.");

        enableRealtimeClock = configFile.Bind("General", "Enable realtime clock", true, "If true, will make the clock update instantly.");

        showClockInShip = configFile.Bind("Visibility", "Show clock in ship", true, "If true, will show the click in the ship too.");
        clockVisibilityInShip = configFile.Bind("Visibility", "Clock visibility in ship", 1F,
                                                new ConfigDescription("The target alpha value to use for clock visibility in ship.",
                                                                      new AcceptableValueRange<float>(0F, 1F)));

        showClockInFacility = configFile.Bind("Visibility", "Show clock in facility", false, "If true, will show the click in the facility too.");
        clockVisibilityInFacility = configFile.Bind("Visibility", "Clock visibility in facility", 1F,
                                                    new ConfigDescription("The target alpha value to use for clock visibility in facility.",
                                                                          new AcceptableValueRange<float>(0F, 1F)));

        showClockInSpectator = configFile.Bind("Visibility", "Show clock in spectator", true, "If true, will show the click in the spectator too.");
        clockVisibilityInSpectator = configFile.Bind("Visibility", "Clock visibility in spectator", 1F,
                                                     new ConfigDescription("The target alpha value to use for clock visibility in spectator.",
                                                                           new AcceptableValueRange<float>(0F, 1F)));

        clockColorRed = configFile.Bind("Customization", "Clock Color Red", 1F,
                                        new ConfigDescription("Defines how much red is in the clock's color",
                                                              new AcceptableValueRange<float>(0F, 1F)));
        clockColorGreen = configFile.Bind("Customization", "Clock Color Green", 0.3F,
                                          new ConfigDescription("Defines how much green is in the clock's color",
                                                                new AcceptableValueRange<float>(0F, 1F)));
        clockColorBlue = configFile.Bind("Customization", "Clock Color Blue", 0F,
                                         new ConfigDescription("Defines how much blue is in the clock's color",
                                                               new AcceptableValueRange<float>(0F, 1F)));
    }
}