using System;

namespace Galaga.GalagaStates {
    public class StateTransformer {
        public static GameStateType TransformStringToState(string state) {
            switch (state) {
                case "GameRunning":
                    return GameStateType.GameRunning;
                case "GamePaused":
                    return GameStateType.GamePaused;
                case "MainMenu":
                    return GameStateType.MainMenu;
                default:
                    throw new ArgumentException($"{state} does not match a GameStateType");
            }
        }

        public static string TransformStateToString(GameStateType state) {
            switch (state) {
                case GameStateType.GameRunning:
                    return "GameRunning";
                case GameStateType.GamePaused:
                    return "GamePaused";
                case GameStateType.MainMenu:
                    return "MainMenu";
                // This function shouldn't need a default section at this time
                default:
                    throw new ArgumentException("How did you catch this exception?");
            }
        }
    }
}
