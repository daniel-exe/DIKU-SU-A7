namespace Galaga;

using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using Galaga.GalagaStates;

public class Game : DIKUGame, IGameEventProcessor {
    private StateMachine stateMachine;

    public Game(WindowArgs windowArgs) : base(windowArgs) {

        //Initialize Statemachine:
        stateMachine = new StateMachine();

        //Initialize GalagaBus:
        GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.PlayerEvent,
            GameEventType.WindowEvent,
            GameEventType.GameStateEvent
        });

        window.SetKeyEventHandler(KeyHandler);
        GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
        GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
    }
    public override void Render() {
        stateMachine.ActiveState.RenderState();
    }
    public override void Update() {
        window.PollEvents();
        // GalagaBus.GetBus().ProcessEventsSequentially();
        stateMachine.ActiveState.UpdateState();
        // GalagaBus.GetBus().ProcessEvents();
    }
    private void KeyHandler(KeyboardAction action, KeyboardKey key) {
        stateMachine.ActiveState.HandleKeyEvent(action, key);
    }

    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.WindowEvent) {
            switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    window.CloseWindow();
                    System.Console.WriteLine("Game Closed");
                    break;
                default:
                    break;
                    // case "KEY_6_RELEASE":
                    //     player.ChangeImage(
                    //         new Image(Path.Combine("Assets", "Images", "alternative_player.png"))
                    //     );
                    //     playerShotImage = new Image(Path.Combine("Assets", "Images", "alternative_bullet.png"));
                    //     bonus = true;
                    //     break;
            }

        }
        // else if (gameEvent.EventType == GameEventType.InputEvent) {


    }
}
