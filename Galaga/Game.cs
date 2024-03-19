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
    private Player player;
    private GameEventBus eventBus;
    private EntityContainer<Enemy> enemies;
    private EntityContainer<PlayerShot> playerShots;
    private IBaseImage playerShotImage;
    private AnimationContainer enemyExplosions;
    private List<Image> explosionStrides;
    private const int EXPLOSION_LENGTH_MS = 500;

    private IMovementStrategy moveStrategy;

    public Game(WindowArgs windowArgs) : base(windowArgs) {
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        eventBus = new GameEventBus();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.PlayerEvent, GameEventType.WindowEvent
        });
        window.SetKeyEventHandler(KeyHandler);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        eventBus.Subscribe(GameEventType.PlayerEvent, player);

        List<Image> images = ImageStride.CreateStrides
            (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        const int numEnemies = 8;
        enemies = new EntityContainer<Enemy>(numEnemies);
        for (int i = 0; i < numEnemies; i++) {
            enemies.AddEntity(new Enemy(
                new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, images)));
        }

        moveStrategy = getRndMovementStrat();
        playerShots = new EntityContainer<PlayerShot>();
        playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

        enemyExplosions = new AnimationContainer(numEnemies);
        explosionStrides = ImageStride.CreateStrides(8,
            Path.Combine("Assets", "Images", "Explosion.png"));
    }

    public override void Render() {
        player.Render();
        enemies.RenderEntities();
        playerShots.RenderEntities();
        enemyExplosions.RenderAnimations();
    }

    public override void Update() {
        window.PollEvents();
        eventBus.ProcessEventsSequentially();
        player.Move();
        IterateShots();
        moveStrategy.MoveEnemies(enemies);
    }

    // Randomly selects a movement strategy by using reflection
    private IMovementStrategy getRndMovementStrat() {
        var moveStrategyList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IMovementStrategy).IsAssignableFrom(p) && p.IsClass)
            .ToList();

        int LengthOfList = moveStrategyList.Count();
        int rndIndex = RandomGenerator.Generator.Next(0, LengthOfList);
        return (IMovementStrategy)Activator.CreateInstance(moveStrategyList[rndIndex]);
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
