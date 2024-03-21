namespace Galaga.GalagaStates;

using System;

public class StateTransformer {
    public static GameStateType TransformStringToState(string state) {
        switch (state) {
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;
            default:
                throw new ArgumentException($"{state} does not match a GameStateType");
        }
    }

    public static string TransformStateToString(GameStateType state) {
        switch (state) {
            case GameStateType.GameRunning:
                return "GAME_RUNNING";
            case GameStateType.GamePaused:
                return "GAME_PAUSED";
            case GameStateType.MainMenu:
                return "MAIN_MENU";
            // This function shouldn't need a default section at this time
            default:
                throw new ArgumentException("How did you catch this exception?");
        }
    }
}
