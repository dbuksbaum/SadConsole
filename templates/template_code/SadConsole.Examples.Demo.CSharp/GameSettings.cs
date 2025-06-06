﻿using SadConsole.UI;

namespace SadConsole.Examples;

static class GameSettings
{
    public const int GAME_WIDTH = 120;
    public const int GAME_HEIGHT = 45;
    public const int SCREEN_DEMO_WIDTH = 80;
    public const int SCREEN_DEMO_HEIGHT = 25;
    public const int SCREEN_LIST_WIDTH = 30;
    public const int SCREEN_LIST_HEIGHT = 25;
    public const int SCREEN_DESCRIPTION_WIDTH = 50;
    public const int SCREEN_DESCRIPTION_HEIGHT = 13;

    public static Rectangle ScreenListBounds = new Rectangle(2, 2, SCREEN_LIST_WIDTH, SCREEN_LIST_HEIGHT);
    public static Rectangle ScreenDescriptionBounds = new Rectangle(2, ScreenListBounds.MaxExtentY + 4, SCREEN_DESCRIPTION_WIDTH, SCREEN_DESCRIPTION_HEIGHT);
    public static Rectangle ScreenDemoBounds = new Rectangle(ScreenListBounds.MaxExtentX + 5, 2, SCREEN_DEMO_WIDTH, SCREEN_DEMO_HEIGHT);

    public static Colors ControlColorScheme = Colors.CreateSadConsoleBlue();

    public static IDemo[] Demos =
        {
            new DemoAutoTyping(),
            new DemoStringParsing(),
            new DemoControls(),
            new DemoShapes(),
            new DemoKeyboardHandlers(),
            new DemoMultipleCursors(),
            new DemoScrollableConsole(),
            new DemoScrollableViews(),
            new DemoRandomScrolling(),
            new DemoSurfaceOpacity(),
            new DemoAsciiGraphics(),
            new DemoAnimations(),
            new DemoEntitySurface(),
            new DemoLayeredSurface(),
            new DemoSecondSurfaceRenderer(),
            new DemoCustomCellsRenderer(),
            new DemoPlayground(),
            // new DemoControls2(),
            // new DemoShader(),
            // new DemoRotatedSurface(),
            // new DemoFontManipulation(),

        };
}
