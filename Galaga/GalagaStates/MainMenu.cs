namespace Galaga.GalagaStates;

using System.IO;
using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

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
    private int fontSize = 55;//
    private int activeMenuButton = 0;
    private int maxMenuButtons = 1;

    public static MainMenu GetInstance() {
        if (MainMenu.instance == null) {
            MainMenu.instance = new MainMenu();
            MainMenu.instance.ResetState();
        }
        return MainMenu.instance;
    }

    public void ResetState() {
        activeMenuButton = 0;
        // DATA
        // Background
        image = new Image(Path.Combine("Assets", "Images", "TitleImage.png"));
        shape = new StationaryShape(0f, 0f, 1f, 1f);
        backGroundImage = new Entity(shape, image);
        // Buttons
        newGamePosition = new Vec2F(0.3f, 0.35f);//
        newGameExtent = new Vec2F(0.3f, 0.4f);
        quitPosition = new Vec2F(0.3f, 0.25f);//
        quitExtent = new Vec2F(0.3f, 0.4f);
        newGame = new Text("- New Game", newGamePosition, newGameExtent);
        quit = new Text("- Quit", quitPosition, quitExtent);
        greenActive = new Vec3I(0, 204, 0);
        grayPassive = new Vec3I(192, 192, 192);
        newGame.SetColor(greenActive);
        quit.SetColor(grayPassive);
        newGame.SetFontSize(fontSize);
        quit.SetFontSize(fontSize);
        menuButtons = new Text[] { newGame, quit };
    }

    public void UpdateState() {
        GalagaBus.GetBus().ProcessEventsSequentially();
    }

    public void RenderState() {
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
                        activeMenuButton--;
                        menuButtons[0].SetColor(greenActive);
                        menuButtons[1].SetColor(grayPassive);
                    }
                } else if (key == KeyboardKey.Down) {
                    if (activeMenuButton != maxMenuButtons) {
                        activeMenuButton++;
                        menuButtons[1].SetColor(greenActive);
                        menuButtons[0].SetColor(grayPassive);
                    }
                }
                break;

            case KeyboardAction.KeyRelease:
                if (key == KeyboardKey.Enter) {
                    // New Game
                    if (activeMenuButton == 0) {
                        GalagaBus.GetBus().RegisterEvent(
                            new GameEvent {
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "GAME_RUNNING"
                            }
                        );
                        // Quit
                    } else if (activeMenuButton == 1) {
                        GalagaBus.GetBus().RegisterEvent(
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
