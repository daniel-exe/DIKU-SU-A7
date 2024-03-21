using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance = null;

        private Player player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));

        public static GameRunning GetInstance() {
            if (GameRunning.instance == null) {
                GameRunning.instance = new GameRunning();
                GameRunning.instance.ResetState();
            }
            return GameRunning.instance;
        }

        public void RenderState() {
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
            backGroundImage.Render();
            foreach (button in menuButtons) {
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
                            )
                        // Quit
                        } else if (activeMenuButton == 1) {
                            GalagaBus.GetBus().RegisterEvent (
                                new GameEvent {
                                    EventType = GameEventType.WindowEvent,
                                    Message = "CLOSE_WINDOW",
                                }
                            )
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        // Only implemented to fulfill contract
        public void ResetState() {
            activeMenuButton = 0;
        }

        // Only implemented to fulfill contract
        public void UpdateState() {
            RenderState();
        }
    }
}
