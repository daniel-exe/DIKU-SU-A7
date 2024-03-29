namespace Galaga.GalagaStates;

using System;
using System.IO;
using DIKUArcade.Events;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Input;
using System.Collections.Generic;
using System.Linq;
using DIKUArcade.Physics;
using DIKUArcade.Utilities;
using Galaga.Squadron;
using Galaga.MovementStrategy;

public class GameRunning : IGameState {
    private static GameRunning instance = null;
    // Player
    private Player player;
    private Vec2F playerCentre;
    private EntityContainer<PlayerShot> playerShots;
    private IBaseImage playerShotImage;
    private List<Image> images = ImageStride.CreateStrides
        (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
    private IMovementStrategy moveStrategy;
    // Ny enemies
    private ISquadron spawnSquad;
    private AnimationContainer enemyExplosions;
    private List<Image> explosionStrides;
    private const int EXPLOSION_LENGTH_MS = 500;


    public static GameRunning GetInstance() {
        if (GameRunning.instance == null) {
            GameRunning.instance = new GameRunning();
            GameRunning.instance.ResetState();
        }
        return GameRunning.instance;
    }

    // Resets game with player position and respawns enemies
    public void ResetState() {
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

        SpawnSquadron();
        SetRndMovementStrat();

        playerShots = new EntityContainer<PlayerShot>();
        playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

        enemyExplosions = new AnimationContainer(8);
        explosionStrides = ImageStride.CreateStrides(8,
            Path.Combine("Assets", "Images", "Explosion.png"));

    }

    public void UpdateState() {
        GalagaBus.GetBus().ProcessEventsSequentially();
        player.Move();
        IterateShots();
        moveStrategy.MoveEnemies(spawnSquad.Enemies);
    }

    public void RenderState() {
        player.Render();
        spawnSquad.Enemies.RenderEntities();
        playerShots.RenderEntities();
        enemyExplosions.RenderAnimations();
    }

    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
        switch (action) {
            case KeyboardAction.KeyPress:
                KeyPress(key);
                break;
            case KeyboardAction.KeyRelease:
                KeyRelease(key);
                break;
            default:
                break;
        }
    }

    private void KeyPress(KeyboardKey key) {
        switch (key) {
            case KeyboardKey.Left:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_LEFT",
                });
                break;
            case KeyboardKey.Right:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_RIGHT",
                });
                break;
            case KeyboardKey.Up:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_UP",
                });
                break;
            case KeyboardKey.Down:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_DOWN",
                });
                break;

            case KeyboardKey.Escape:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    EventType = GameEventType.GameStateEvent,
                    Message = "CHANGE_STATE",
                    StringArg1 = "GAME_PAUSED"
                }
                );
                break;
        }
    }
    private void KeyRelease(KeyboardKey key) {
        // switch on key string and disable the player's move direction
        switch (key) {
            case KeyboardKey.Left:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "false",
                    Message = "MOVE_LEFT",
                });
                break;
            case KeyboardKey.Right:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "false",
                    Message = "MOVE_RIGHT",
                });
                break;
            case KeyboardKey.Up:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "false",
                    Message = "MOVE_UP",
                });
                break;
            case KeyboardKey.Down:
                GalagaBus.GetBus().RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "false",
                    Message = "MOVE_DOWN",
                });
                break;
            case KeyboardKey.Escape:
                GalagaBus.GetBus().RegisterEvent(
                    new GameEvent {
                        EventType = GameEventType.GameStateEvent,
                        Message = "CHANGE_STATE",
                        StringArg1 = "GAME_PAUSED"
                    }
                );
                break;
            // We cant figure out how to process the event without creating a ProcessEvent within this class.
            case KeyboardKey.Space:
                playerCentre = player.GetCentrum();
                PlayerShot shot = new PlayerShot(playerCentre, playerShotImage);
                playerShots.AddEntity(shot);
                break;
        }
    }

    //Method that creates enemies.
    private void SpawnSquadron() {
        List<Image> enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "RedMonster.png"));

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
        spawnSquad.CreateEnemies(enemyStridesBlue, enemyStridesRed);
    }

    // Randomly selects a movement strategy by using reflection
    private void SetRndMovementStrat() {
        var moveStrategyList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IMovementStrategy).IsAssignableFrom(p) && p.IsClass)
            .ToList();

        int lengthOfList = moveStrategyList.Count();
        int rndIndex = RandomGenerator.Generator.Next(0, lengthOfList);
        moveStrategy = (IMovementStrategy) Activator.CreateInstance(moveStrategyList[rndIndex]);
    }

    private void AddExplosion(Vec2F position, Vec2F extent) {
        StationaryShape explosionShape = new StationaryShape(position, extent);
        ImageStride explosionStride = new ImageStride(EXPLOSION_LENGTH_MS / 8, explosionStrides);
        enemyExplosions.AddAnimation(explosionShape, EXPLOSION_LENGTH_MS, explosionStride);
    }


    private void IterateShots() {
        playerShots.Iterate(shot => {
            //shot movement speed:
            shot.Shape.MoveY(0.1f);

            if (shot.Shape.Position.Y > 1) { //Shot is deleted if out of bounds.
                shot.DeleteEntity();

            } else {
                spawnSquad.Enemies.Iterate(enemy => {
                    //Since the implementation of the AABB algorithm requires dynamic shape as first
                    //-argument we cast the shots shape to a dynamic shape.
                    DynamicShape shotDynamicShape = shot.Shape.AsDynamicShape();
                    //The method AsDynamicShape sets direction to (0,0) as default. So we change it:
                    shotDynamicShape.ChangeDirection(shot.Direction);
                    Shape enemyShape = enemy.Shape;
                    var collide = CollisionDetection.Aabb(shotDynamicShape, enemyShape);
                    bool collision = collide.Collision;

                    if (collision) {
                        shot.DeleteEntity();
                        if (enemy.GetHit(player.Damage)) {
                            enemy.DeleteEntity();
                            AddExplosion(enemyShape.Position, enemyShape.Extent);
                        }
                    }
                });
            }
        });
    }
}
