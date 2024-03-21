namespace Galaga.GalagaStates;

using System.IO;
using DIKUArcade.Input;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

public class GamePaused : IGameState {
    private static GamePaused instance = null;
    // Background
    private Image image;
    private StationaryShape shape;
    private Entity backGroundImage;
    // Buttons
    private Vec2F continuePosition;
    private Vec2F continueExtent;
    private Vec2F mainMenuPosition;
    private Vec2F mainMenuExtent;
    private Text continueButton;
    private Text mainMenuButton;
    private Text[] menuButtons;
    // Button attributes
    private Vec3I greenActive;
    private Vec3I grayPassive;
    private int fontSize = 16;
    private int activeMenuButton = 0;
    private int maxMenuButtons = 1;

    public static GamePaused GetInstance() {
        if (GamePaused.instance == null) {
            GamePaused.instance = new GamePaused();
            GamePaused.instance.ResetState();
        }
        return GamePaused.instance;
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
        image = new Image(Path.Combine("Assets", "galaga.png"));
        shape = new StationaryShape(0f, 0f, 1f, 1f);
        backGroundImage = new Entity(shape, image);
        // Buttons
        continuePosition = new Vec2F(0.4f, 0.3f);
        continueExtent = new Vec2F(0.2f, 0.1f);
        mainMenuPosition = new Vec2F(0.4f, 0.45f);
        mainMenuExtent = new Vec2F(0.2f, 0.1f);
        continueButton = new Text("- Continue", continuePosition, continueExtent);
        mainMenuButton = new Text("- Main Menu", mainMenuPosition, mainMenuExtent);
        menuButtons = new Text[] { continueButton, mainMenuButton };
        // Button attributes
        greenActive = new Vec3I(0, 204, 0);
        grayPassive = new Vec3I(192, 192, 192);
        // Set colors
        for (int i = 0; i >= maxMenuButtons; i++) {
            if (i == activeMenuButton) {
                menuButtons[i].SetColor(greenActive);
            } else {
                menuButtons[i].SetColor(grayPassive);
            }
            menuButtons[i].SetFontSize(fontSize);
        }
        // Render
        backGroundImage.Image.Render(backGroundImage.Shape);
        foreach (Text button in menuButtons) {
            button.RenderText();
        }
    }

    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
        switch (action) {
            case KeyboardAction.KeyPress:
                if (key == KeyboardKey.Up) {
                    if (activeMenuButton != 0) {
                        activeMenuButton --;
                    }
                } else if (key == KeyboardKey.Down) {
                    if (activeMenuButton != maxMenuButtons) {
                        activeMenuButton ++;
                    }
                }
                break;
            case KeyboardAction.KeyRelease:
                if (key == KeyboardKey.Enter) {
                    // Continue
                    if (activeMenuButton == 0) {
                        GalagaBus.GetBus().RegisterEvent (
                            new GameEvent {
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "GAME_RUNNING"
                            }
                        );
                    // Main Menu
                    } else if (activeMenuButton == 1) {
                        GalagaBus.GetBus().RegisterEvent (
                            new GameEvent {
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "MAIN_MENU",
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
