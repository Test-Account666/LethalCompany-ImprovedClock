using BepInEx.Configuration;

namespace ImprovedClock;

public static class ConfigManager {
    // This variable name is a joke btw. I don't have anything against anyone.
    public static ConfigEntry<bool> enableNormalHumanBeingClock = null!;
    public static ConfigEntry<bool> enableRealtimeClock = null!;

    public static ConfigEntry<bool> showClockInShip = null!;
    public static ConfigEntry<int> clockVisibilityInShip = null!;

    public static ConfigEntry<bool> showClockInFacility = null!;
    public static ConfigEntry<int> clockVisibilityInFacility = null!;

    public static ConfigEntry<bool> showClockInSpectator = null!;
    public static ConfigEntry<int> clockVisibilityInSpectator = null!;

    public static ConfigEntry<int> clockNumberColorRed = null!;
    public static ConfigEntry<int> clockNumberColorGreen = null!;
    public static ConfigEntry<int> clockNumberColorBlue = null!;

    public static ConfigEntry<int> clockBoxColorRed = null!;
    public static ConfigEntry<int> clockBoxColorGreen = null!;
    public static ConfigEntry<int> clockBoxColorBlue = null!;

    public static ConfigEntry<int> clockIconColorRed = null!;
    public static ConfigEntry<int> clockIconColorGreen = null!;
    public static ConfigEntry<int> clockIconColorBlue = null!;

    public static ConfigEntry<int> clockShipLeaveIconColorRed = null!;
    public static ConfigEntry<int> clockShipLeaveIconColorGreen = null!;
    public static ConfigEntry<int> clockShipLeaveIconColorBlue = null!;


    public static ConfigEntry<int> clockSizeMultiplier = null!;

    public static ConfigEntry<bool> useAlternativeDangerIcon = null!;

    public static ConfigEntry<string> disabledClockMoons = null!;

    public static void Initialize(ConfigFile configFile) {
        #region General

        enableNormalHumanBeingClock = configFile.Bind("General", "Enable 24 hour clock", true,
                                                      "If true, will make the clock display 24 hour time instead of 12 hour time.");

        enableRealtimeClock = configFile.Bind("General", "Enable realtime clock", true, "If true, will make the clock update instantly.");

        #endregion General

        #region Visibility

        showClockInShip = configFile.Bind("Visibility", "Show clock in ship", true, "If true, will show the click in the ship too.");
        clockVisibilityInShip = configFile.Bind("Visibility", "Clock visibility in ship", 100,
                                                new ConfigDescription("The visibility percentage to use for clock visibility in ship.",
                                                                      new AcceptableValueRange<int>(0, 100)));

        showClockInFacility = configFile.Bind("Visibility", "Show clock in facility", false, "If true, will show the click in the facility too.");
        clockVisibilityInFacility = configFile.Bind("Visibility", "Clock visibility in facility", 100,
                                                    new ConfigDescription("The visibility percentage to use for clock visibility in facility.",
                                                                          new AcceptableValueRange<int>(0, 100)));

        showClockInSpectator = configFile.Bind("Visibility", "Show clock in spectator", true, "If true, will show the click in the spectator too.");
        clockVisibilityInSpectator = configFile.Bind("Visibility", "Clock visibility in spectator", 100,
                                                     new ConfigDescription("The visibility percentage to use for clock visibility in spectator.",
                                                                           new AcceptableValueRange<int>(0, 100)));

        #endregion Visibility


        clockSizeMultiplier = configFile.Bind("Clock Customization", "Clock Size Percentage", 100,
                                              new ConfigDescription("A multiplier applied to the clock's size.",
                                                                    new AcceptableValueRange<int>(50, 200)));

        useAlternativeDangerIcon = configFile.Bind("Clock Customization", "Use Alternative Danger Icon", false,
                                                   "If true, will use an alternative danger icon.");


        #region colors

        clockNumberColorRed = configFile.Bind("Number Customization", "Color Red", 255,
                                              new ConfigDescription("Defines how much red is in the clock numbers' color",
                                                                    new AcceptableValueRange<int>(0, 255)));
        clockNumberColorGreen = configFile.Bind("Number Customization", "Color Green", 76,
                                                new ConfigDescription("Defines how much green is in the clock numbers' color",
                                                                      new AcceptableValueRange<int>(0, 255)));
        clockNumberColorBlue = configFile.Bind("Number Customization", "Color Blue", 0,
                                               new ConfigDescription("Defines how much blue is in the clock numbers' color",
                                                                     new AcceptableValueRange<int>(0, 255)));


        clockBoxColorRed = configFile.Bind("Box Customization", "Color Red", 255,
                                           new ConfigDescription("Defines how much red is in the clock box's color",
                                                                 new AcceptableValueRange<int>(0, 255)));
        clockBoxColorGreen = configFile.Bind("Box Customization", "Color Green", 76,
                                             new ConfigDescription("Defines how much green is in the clock box's color",
                                                                   new AcceptableValueRange<int>(0, 255)));
        clockBoxColorBlue = configFile.Bind("Box Customization", "Color Blue", 0,
                                            new ConfigDescription("Defines how much blue is in the clock box's color",
                                                                  new AcceptableValueRange<int>(0, 255)));


        clockIconColorRed = configFile.Bind("Icon Customization", "Color Red", 255,
                                            new ConfigDescription("Defines how much red is in the clock icon's color",
                                                                  new AcceptableValueRange<int>(0, 255)));
        clockIconColorGreen = configFile.Bind("Icon Customization", "Color Green", 76,
                                              new ConfigDescription("Defines how much green is in the clock icon's color",
                                                                    new AcceptableValueRange<int>(0, 255)));
        clockIconColorBlue = configFile.Bind("Icon Customization", "Color Blue", 0,
                                             new ConfigDescription("Defines how much blue is in the clock icon's color",
                                                                   new AcceptableValueRange<int>(0, 255)));


        clockShipLeaveIconColorRed = configFile.Bind("Ship Leave Icon Customization", "Color Red", 255,
                                                     new ConfigDescription("Defines how much red is in the clock ship leave icon's color",
                                                                           new AcceptableValueRange<int>(0, 255)));
        clockShipLeaveIconColorGreen = configFile.Bind("Ship Leave Icon Customization", "Color Green", 76,
                                                       new ConfigDescription("Defines how much green is in the clock ship leave icon's color",
                                                                             new AcceptableValueRange<int>(0, 255)));
        clockShipLeaveIconColorBlue = configFile.Bind("Ship Leave Icon Customization", "Color Blue", 0,
                                                      new ConfigDescription("Defines how much blue is in the clock ship leave icon's color",
                                                                            new AcceptableValueRange<int>(0, 255)));

        #endregion colors

        #region Special Cases

        disabledClockMoons = configFile.Bind("Special Cases", "Disabled Clock Moons", "Example1, Example2",
                                             "A comma-separated list of moons where the clock is disabled (Does *NOT* affect spectator clock.) "
                                           + "Uses `contains`, so only names are required. For example, `Roid` will disable clock on Asteroid-13 and Asteroid-14.");

        #endregion Special Cases
    }
}