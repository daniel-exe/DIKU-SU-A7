using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using DIKUArcade.Utilities;
using Galaga.MovementStrategy;


namespace Galaga;
public class Game : DIKUGame, IGameEventProcessor {

    private GameEventBus eventBus;


    public Game(WindowArgs windowArgs) : base(windowArgs) {

        eventBus = new GameEventBus();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.PlayerEvent, GameEventType.WindowEvent
        });
        window.SetKeyEventHandler(KeyHandler);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        eventBus.Subscribe(GameEventType.PlayerEvent, player);
    }

    // Obsolete?
    // public override void Render() {
    // }

    public override void Update() {
        window.PollEvents();
        // Something like stateMachine.ActiveState.UpdateState();
    }

    private void KeyPress(KeyboardKey key) {
        switch(key) {
            case KeyboardKey.Escape:
                GameEvent closeWindow = new GameEvent();
                closeWindow.EventType = GameEventType.WindowEvent;
                closeWindow.Message = "CLOSE_WINDOW";
                eventBus.RegisterEvent(closeWindow);
                break;
            case KeyboardKey.Left:
                GameEvent moveLeft = new GameEvent();
                moveLeft.EventType = GameEventType.PlayerEvent;
                moveLeft.StringArg1 = "true";
                moveLeft.Message = "MOVE_LEFT";
                eventBus.RegisterEvent(moveLeft);
                break;
            case KeyboardKey.Right:
                GameEvent moveRight = new GameEvent();
                moveRight.EventType = GameEventType.PlayerEvent;
                moveRight.StringArg1 = "true";
                moveRight.Message = "MOVE_RIGHT";
                eventBus.RegisterEvent(moveRight);
                break;
            default:
                break;
        }
    }

    private void KeyRelease(KeyboardKey key) {
        switch(key) {
            case KeyboardKey.Left:
                GameEvent moveLeft = new GameEvent();
                moveLeft.EventType = GameEventType.PlayerEvent;
                moveLeft.StringArg1 = "false";
                moveLeft.Message = "MOVE_LEFT";
                eventBus.RegisterEvent(moveLeft);
                break;
            case KeyboardKey.Right:
                GameEvent moveRight = new GameEvent();
                moveRight.EventType = GameEventType.PlayerEvent;
                moveRight.StringArg1 = "false";
                moveRight.Message = "MOVE_RIGHT";
                eventBus.RegisterEvent(moveRight);
                break;
            case KeyboardKey.Space:
                var shotPos = new Vec2F(player.GetPosition().X + 0.05f, player.GetPosition().Y);
                playerShots.AddEntity(new PlayerShot(shotPos, playerShotImage));
                break;
            default:
                break;
        }
    }

    private void KeyHandler(KeyboardAction action, KeyboardKey key) {
// Something like stateMachine.ActiveState.HandleKeyEvent(action, key);


    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.WindowEvent) {
            switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    window.CloseWindow();
                    Console.WriteLine(gameEvent.Message);
                    break;
            }
        }
    }

    private void IterateShots() {
        playerShots.Iterate(shot => {
            shot.Move();
            if (shot.GetPosition().Y > 1.0f) {
                shot.DeleteEntity();
            } else {
                enemies.Iterate(enemy => {
                    if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(),
                        enemy.Shape).Collision) {
                            var explosionExtent = new Vec2F(0.14f, 0.14f);
                            AddExplosion(enemy.Shape.Position, explosionExtent);
                            shot.DeleteEntity();
                            enemy.DeleteEntity();
                        }
                });
            }
        });
    }

    private void AddExplosion(Vec2F position, Vec2F extent) {
        var explosionShape = new DynamicShape(position, extent);
        var explotionLength = EXPLOSION_LENGTH_MS;
        var explosionStride = new ImageStride(EXPLOSION_LENGTH_MS/8, explosionStrides);
        enemyExplosions.AddAnimation(explosionShape, explotionLength, explosionStride);
    }
}
