using System.IO;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Physics;

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

    public Game(WindowArgs windowArgs) : base(windowArgs) {
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
        eventBus = new GameEventBus();
        eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent });
        window.SetKeyEventHandler(KeyHandler);
        eventBus.Subscribe(GameEventType.InputEvent, this);

        List<Image> images = ImageStride.CreateStrides
            (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        const int numEnemies = 8;
        enemies = new EntityContainer<Enemy>(numEnemies);
        for (int i = 0; i < numEnemies; i++) {
            enemies.AddEntity(new Enemy(
                new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, images)));
        }

        playerShots = new EntityContainer<PlayerShot>();
        playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

        enemyExplosions = new AnimationContainer(numEnemies);
        explosionStrides = ImageStride.CreateStrides(8,
            Path.Combine("Assets", "Images", "Explosion.png"));
    }

    public override void Render(){
        player.Render();
        enemies.RenderEntities();
        playerShots.RenderEntities();
        enemyExplosions.RenderAnimations();
    }

    public override void Update(){
        window.PollEvents();
        eventBus.ProcessEventsSequentially();
        player.Move();
        IterateShots();
    }

    private void KeyPress(KeyboardKey key) {
        switch(key) {
            case KeyboardKey.Escape:
                window.CloseWindow();
                break;
            case KeyboardKey.Left:
                player.SetMoveLeft(true);
                break;
            case KeyboardKey.Right:
                player.SetMoveRight(true);
                break;
            default:
                break;
        }
    }

    private void KeyRelease(KeyboardKey key) {
        switch(key) {
            case KeyboardKey.Left:
                player.SetMoveLeft(false);
                break;
            case KeyboardKey.Right:
                player.SetMoveRight(false);
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
        // gameEvent.Message;
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

    public void AddExplosion(Vec2F position, Vec2F extent) {
        var explosionShape = new DynamicShape(position, extent);
        var explotionLength = EXPLOSION_LENGTH_MS;
        var explosionStride = new ImageStride(EXPLOSION_LENGTH_MS/8,explosionStrides);
        enemyExplosions.AddAnimation(explosionShape, explotionLength, explosionStride);
    }
}
