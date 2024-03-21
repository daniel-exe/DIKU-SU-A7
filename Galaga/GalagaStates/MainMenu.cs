using System;
using System.IO;
using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Math;



namespace Galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        // Background
        private Image image = new Image(Path.Combine("Assets", "Images", "TitleImage.png"));
        private StationaryShape shape = new StationaryShape(0f, 0f, 1f, 1f);
        private Entity backGroundImage= new Entity(shape, image);
        // Buttons
        private Vec2F newGamePosition = new Vec2F(0.4f, 0.3f);
        private Vec2F newGameExtent = new Vec2F(0.2f, 0.1f);
        private Vec2F quitPosition = new Vec2F(0.4f, 0.45f);
        private Vec2F quitExtent = new Vec2F(0.2f, 0.1f);
        private Text newGame = new Text("- New Game", newGamePosition, newGameExtent);
        private Text quit = new Text("- Quit", quitPosition, quitExtent);
        private Text[] menuButtons = new List<Text> { newGame, quit };
        // Button attributes
        private Vec3I greenActive = new Vec3I(0, 204, 0);
        private Vec3I grayPassive = new Vec3I(192, 192, 192);
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
            backGroundImage.Render();
            foreach (Text button in menuButtons) {
                menuButtons[i].RenderText();
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
}
