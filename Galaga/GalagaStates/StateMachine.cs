namespace Galaga.GalagaStates;

using DIKUArcade.Events;
using DIKUArcade.State;

public class StateMachine : IGameEventProcessor {
    public IGameState ActiveState {
        get; private set;
    }
    public StateMachine() {
        ActiveState = MainMenu.GetInstance();
        GameRunning.GetInstance();
        GamePaused.GetInstance();
    }

    // Sets current game state
    private void SwitchState(GameStateType stateType) {
        switch (stateType) {
            case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;
            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                break;
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                break;
        }
    }

    // Catches GameStateEvents and calls .ResetState() when needed
    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.GameStateEvent) {
            switch (gameEvent.StringArg1) {
                case "GAME_RUNNING":
                    if (ActiveState == MainMenu.GetInstance()) {
                        GameRunning.GetInstance().ResetState();
                    }
                    SwitchState(GameStateType.GameRunning);
                    break;
                case "GAME_PAUSED":
                    SwitchState(GameStateType.GamePaused);
                    GamePaused.GetInstance().ResetState();
                    break;
                case "MAIN_MENU":
                    SwitchState(GameStateType.MainMenu);
                    break;
            }
        }
    }
}
