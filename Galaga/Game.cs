namespace Galaga;

using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using DIKUArcade.Input;
using DIKUArcade.Utilities;
using Squadron;
using MovementStrategy;
using Galaga.GalagaStates;

public class Game : DIKUGame, IGameEventProcessor {
    //EventBus:
    // private GalagaBus eventBus = GalagaBus.GetBus(); Not Necessary?
    //StateMachine:
    // private StateMachine stateMachine = new StateMachine blabla
    private StateMachine stateMachine;
    public Game(WindowArgs windowArgs) : base(windowArgs) {
        stateMachine = new StateMachine();
        //GalagaBus initializing:
        GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.PlayerEvent,
            GameEventType.WindowEvent,
            GameEventType.GameStateEvent
        });

        window.SetKeyEventHandler(KeyHandler);
        // eventBus.Subscribe(GameEventType.InputEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
        // eventBus.Subscribe(GameEventType.GameStateEvent, stateMachine);
        // eventBus.Subscribe(GameEventType.PlayerEvent, this); //Subscribed to player

    }
    public override void Render() {
        stateMachine.ActiveState.RenderState();
    }
    public override void Update() {
        window.PollEvents();
        stateMachine.ActiveState.UpdateState();
        GalagaBus.GetBus().ProcessEvents();
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

                case "KEY_6_RELEASE":
                    player.ChangeImage(
                        new Image(Path.Combine("Assets", "Images", "alternative_player.png"))
                    );
                    playerShotImage = new Image(Path.Combine("Assets", "Images", "alternative_bullet.png"));
                    bonus = true;
                    break;
            }

        } else if (gameEvent.EventType == GameEventType.InputEvent) {

            switch (gameEvent.Message) {
                case "KEY_SPACE_RELEASE":
                    playerCentre = player.GetCentrum();
                    PlayerShot shot = new PlayerShot(playerCentre, playerShotImage);
                    if (bonus) {
                        shot.Extent = new Vec2F(0.020f, 0.042f);
                    }
                    playerShots.AddEntity(shot);
                    break;
            }
        }
    }
}
