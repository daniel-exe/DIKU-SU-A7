using System;
using System.IO;
using DIKUArcade.Input;
using DIKUArcade.State;
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
using Squadron;
using MovementStrategy;


namespace Galaga.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance = null;
        // Background
        private Image image = new Image(Path.Combine("Assets", "galaga.png"));
        private StationaryShape shape = new StationaryShape(0f, 0f, 1f, 1f);
        private Entity backGroundImage = new Entity(shape, image);
        // Buttons
        private Vec2F continuePosition = new Vec2F(0.4f, 0.3f);
        private Vec2F continueExtent = new Vec2F(0.2f, 0.1f);
        private Vec2F mainMenuPosition = new Vec2F(0.4f, 0.45f);
        private Vec2F mainMenuExtent = new Vec2F(0.2f, 0.1f);
        private Text continueButton = new Text("- Continue", continuePosition, continueExtent);
        private Text mainMenuButton = new Text("- Main Menu", mainMenuPosition, mainMenuExtent);
        private Text[] menuButtons = new List<Text> { continueButton, mainMenuButton };
        // Button attributes
        private Vec3I greenActive = new Vec3I(0, 204, 0);
        private Vec3I grayPassive = new Vec3I(192, 192, 192);
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
                                    Message = "MAIN_MENU",
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
