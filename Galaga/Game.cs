namespace Galaga;

using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Physics;
using System.Collections.Generic;
using Galaga.Squadron;
using System;


public class Game : DIKUGame, IGameEventProcessor {
    private Player player;
    private GameEventBus eventBus;
    private EntityContainer<PlayerShot> playerShots;
    private IBaseImage playerShotImage;
    public bool bonus = false;
    private ISquadron spawnSquad;
    public Game(WindowArgs windowArgs) : base(windowArgs) {
        eventBus = new GameEventBus();
        eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent, GameEventType.PlayerEvent });
        window.SetKeyEventHandler(KeyHandler);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "player.png"))
        );
        eventBus.Subscribe(GameEventType.PlayerEvent, player);
        playerShots = new EntityContainer<PlayerShot>();
        playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        List<Image> images = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
    }
    public override void Render() {
        player.Render();
        SpawnSquadron();
        playerShots.RenderEntities();
    }

    public override void Update() {
        eventBus.ProcessEventsSequentially();
        player.Move();
        IterateShots();
    }
    private void KeyPress(KeyboardKey key) {
        switch (key) {
            case KeyboardKey.Escape :
                window.CloseWindow();
                break;
            case KeyboardKey.Left : 
                eventBus.RegisterEvent(new GameEvent {
                    EventType = GameEventType.PlayerEvent,  Message = "MoveLeft", StringArg1 = "True"}
                );
                break; 
            case KeyboardKey.Right : 
                eventBus.RegisterEvent(new GameEvent {
                    EventType = GameEventType.PlayerEvent,  Message = "MoveRight", StringArg1 = "True"});                    
                break; 
            default :
                break;
        }
    }
    private void KeyRelease(KeyboardKey key) {
        switch (key) {
            case KeyboardKey.Space :
                Vec2F playerPos = player.GetPosition();
                Vec2F playerExtent = player.GetExtent;
                Vec2F shotPos = new Vec2F(playerPos.X + (playerExtent.X / 2) -0.07f, playerPos.Y + playerExtent.Y-0.05f);
                playerShots.AddEntity(new PlayerShot( shotPos  , new Image(Path.Combine("Assets", "Images", "BulletRed2.png")) ));
                break;
            case KeyboardKey.Left : 
                eventBus.RegisterEvent(new GameEvent{
                    EventType = GameEventType.PlayerEvent,  Message = "MoveLeft", StringArg1 = "False"}
                );
                break;
            case KeyboardKey.Right : 
                eventBus.RegisterEvent(new GameEvent{
                    EventType = GameEventType.PlayerEvent,  Message = "MoveRight", StringArg1 = "False"}
                );
                break;
            default : 
                break;
        }
    }
    private void KeyHandler(KeyboardAction action, KeyboardKey key) {
        // TODO: Switch on KeyBoardAction and call proper method
        if (action == KeyboardAction.KeyRelease) {
            KeyRelease(key);
        } else if (action == KeyboardAction.KeyPress) {
            KeyPress(key);
        } else { //Can maybe be removed??
            return;
        }
    }

    public void ProcessEvent(GameEvent gameEvent) { //laves færdig
        // Leave this empty for now
    }
        //Method for shots:
    private void IterateShots() {
        playerShots.Iterate(shot => {
            shot.Shape.MoveY(0.01f);

            if (shot.Shape.Position.Y > 1) { // Shot is deleted if out of bounds.
                shot.DeleteEntity();
            }
            else {
                spawnSquad.Enemies.Iterate(enemy => {
                    // Since the implementation of the AABB algorithm requires a dynamic shape as the first argument,
                    // we cast the shot's shape to a dynamic shape.
                    DynamicShape shotDynamicShape = shot.Shape.AsDynamicShape();
                    shotDynamicShape.Direction = new Vec2F(0.0f, 0.1f);
                    Shape enemyShape = enemy.Shape;
                    var collide = CollisionDetection.Aabb(shotDynamicShape, enemyShape);
                    bool collision = collide.Collision;
                    if (collision) {
                        System.Console.WriteLine("hit");
                        enemy.DeleteEntity();
                        shot.DeleteEntity();
                    }

                });
            }
        });
    }
    public void SpawnSquadron() {
        if (spawnSquad == null || spawnSquad.Enemies.CountEntities() == 0) {
            List<Image> images = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            Random rand = new Random();
            int num = rand.Next(1, 4);
            switch (num) {
                case 1:
                    spawnSquad = new Rectangle();
                    break;
                case 2:
                    spawnSquad = new Square();
                    break;
                case 3:
                    spawnSquad = new Triangle();
                    break;
                default:
                    break;
            }
            spawnSquad.CreateEnemies(images, images); 
        }
        spawnSquad.Enemies.RenderEntities();
    }
}