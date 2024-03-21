namespace Galaga.GalagaStates;

using System;
using System.IO;
using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Utilities;
using Galaga.Squadron;
using Galaga.MovementStrategy;


public class MainMenu : IGameState {
    private static MainMenu instance = null;
    // Background
    private Image image;
    private StationaryShape shape;
    private Entity backGroundImage;
    // Buttons
    private Vec2F newGamePosition;
    private Vec2F newGameExtent;
    private Vec2F quitPosition;
    private Vec2F quitExtent;
    private Text newGame;
    private Text quit;
    private Text[] menuButtons;
    // Button attributes
    private Vec3I greenActive;
    private Vec3I grayPassive;
    private int fontSize = 16;
    private int activeMenuButton = 0;
    private int maxMenuButtons = 1;

    public static MainMenu GetInstance() {
        if (MainMenu.instance == null) {
            MainMenu.instance = new MainMenu();
            MainMenu.instance.ResetState();
        }
        return MainMenu.instance;
    }

    // Only implemented to fulfill contract
    public void ResetState() {
        activeMenuButton = 0;
    }

    // Only implemented to fulfill contract
    public void UpdateState() {
        GalagaBus.GetBus().ProcessEventsSequentially();
    }

    public void RenderState() {
        // DATAAAAA
        // Background
        image = new Image(Path.Combine("Assets", "Images", "TitleImage.png"));
        shape = new StationaryShape(0f, 0f, 1f, 1f);
        backGroundImage= new Entity(shape, image);
        // Buttons
        newGamePosition = new Vec2F(0.4f, 0.3f);
        newGameExtent = new Vec2F(0.2f, 0.1f);
        quitPosition = new Vec2F(0.4f, 0.45f);
        quitExtent = new Vec2F(0.2f, 0.1f);
        newGame = new Text("- New Game", newGamePosition, newGameExtent);
        quit = new Text("- Quit", quitPosition, quitExtent);
        menuButtons = new List<Text> { newGame, quit };
        // Button attributes
        greenActive = new Vec3I(0, 204, 0);
        grayPassive = new Vec3I(192, 192, 192);
        // Set colors - I think maybe this part should be in UpdateState()?
        for (int i = 0; i >= maxMenuButtons; i++) {
            if (i == activeMenuButton) {
                menuButtons[i].SetColor(greenActive);
            } else {
                menuButtons[i].SetColor(grayPassive);
            }
            menuButtons[i].SetFontSize(fontSize);
        }
        // Render
        backGroundImage.Shape.Render();
        foreach (Text button in menuButtons) {
            button.RenderText();
        }
    }

    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
        switch (action) {
            case KeyboardAction.KeyPress:
                if (key = KeyboardKey.Up) {
                    if (activeMenuButton != 0) {
                        activeMenuButton --;
                    }
                } else if (key = KeyboardKey.Down) {
                    if (activeMenuButton != maxMenuButtons) {
                        activeMenuButton ++;
                    }
                }
                break;
            case KeyboardAction.KeyRelease:
                if (key = KeyboardKey.Enter) {
                    // New Game
                    if (activeMenuButton == 0) {
                        GalagaBus.GetBus().RegisterEvent (
                            new GameEvent {
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "GAME_RUNNING"
                            }
                        );
                    // Quit
                    } else if (activeMenuButton == 1) {
                        GalagaBus.GetBus().RegisterEvent (
                            new GameEvent {
                                EventType = GameEventType.WindowEvent,
                                Message = "CLOSE_WINDOW",
                            }
                        );
                    }
                }
                break;
            default:
                break;
        }
    }
}