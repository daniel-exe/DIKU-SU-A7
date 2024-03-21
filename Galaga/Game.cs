namespace Galaga;

namespace Galaga;

using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Physics;
using Galaga.Squadron;

public class Game : DIKUGame, IGameEventProcessor {
    private GameEventBus eventBus;
    private Player player;
    private Vec2F playerCentre;

    //Enemies:
    private EntityContainer<Enemy> enemies;

    //Player shots:
    private EntityContainer<PlayerShot> playerShots;
    private IBaseImage playerShotImage;

    //Explosions:
    private AnimationContainer enemyExplosions;
    private List<Image> explosionStrides;
    private const int EXPLOSION_LENGTH_MS = 500;

    //Squadrons:
    private ISquadron spawnSquad;
    //Bonus for fun:
    private bool bonus = false;

    public Game(WindowArgs windowArgs) : base(windowArgs) {
        //Game:
        eventBus = new GameEventBus();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.PlayerEvent,
            GameEventType.WindowEvent
        });

        window.SetKeyEventHandler(KeyHandler);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.PlayerEvent, this); //Maybe subscribe to player
        eventBus.Subscribe(GameEventType.WindowEvent, this);

        //Player:
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "player.png"))
        );

        //Enemies:
        List<Image> enemyStridesBlue = ImageStride.CreateStrides
                                (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        const int numEnemies = 8;
        enemies = new EntityContainer<Enemy>(numEnemies);
        for (int i = 0; i < numEnemies; i++) {
            enemies.AddEntity(new Enemy(
                new DynamicShape(new Vec2F(0.1f + (float) i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                new ImageStride(80, enemyStridesBlue)));
        }

        //Playershots:
        playerShots = new EntityContainer<PlayerShot>();
        playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

        //Explosions:
        enemyExplosions = new AnimationContainer(numEnemies);
        explosionStrides = ImageStride.CreateStrides(8,
            Path.Combine("Assets", "Images", "Explosion.png"));
    }
    public override void Render() {
        player.Render();
        enemies.RenderEntities(); //maybe remove?
        SpawnSquadron();
        playerShots.RenderEntities();
        enemyExplosions.RenderAnimations();
    }

    public override void Update() {
        eventBus.ProcessEventsSequentially();
        player.Move();
        IterateShots();
    }

    private void KeyPress(KeyboardKey key) {
        //switch on key string and set the player's move direction
        switch (key) {
            case KeyboardKey.Left:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_LEFT_PRESS", //Could maybe make just ONE registerevent, and then save a new message for each keypress? .
                });
                break;
            case KeyboardKey.Right:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_RIGHT_PRESS",
                });
                break;
            case KeyboardKey.Up:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_UP_PRESS",
                });
                break;
            case KeyboardKey.Down:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_DOWN_PRESS",
                });
                break;

            //Close window if escape is pressed
            case KeyboardKey.Escape:
                // window.CloseWindow();
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.WindowEvent,
                    Message = "CLOSE_WINDOW",
                });
                break;
        }
    }
    private void KeyRelease(KeyboardKey key) {
        // switch on key string and disable the player's move direction
        switch (key) {
            case KeyboardKey.Left:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_LEFT_RELEASE",
                });
                break;
            case KeyboardKey.Right:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_RIGHT_RELEASE",
                });
                break;
            case KeyboardKey.Up:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_UP_RELEASE",
                });
                break;
            case KeyboardKey.Down:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    Message = "KEY_DOWN_RELEASE",
                });
                break;

            case KeyboardKey.Space:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.InputEvent,
                    Message = "KEY_SPACE_RELEASE",
                    ObjectArg1 = playerShotImage
                });
                break;

            // Activate bonus mode!
            case KeyboardKey.Num_6:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.WindowEvent, //Should this be a WindowEvent??? or something else?
                    Message = "KEY_6_RELEASE",
                });
                break;
        }
    }
    private void KeyHandler(KeyboardAction action, KeyboardKey key) {
        // TODO: Switch on KeyBoardAction and call proper method
        if (action == KeyboardAction.KeyRelease) {
            KeyRelease(key);
        } else if (action == KeyboardAction.KeyPress) {
            KeyPress(key);
        }
    }

    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.WindowEvent) {
            switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    window.CloseWindow();
                    break;

                case "KEY_6_RELEASE":
                    player.ChangeImage(
                        new Image(Path.Combine("Assets", "Images", "alternative_player.png"))
                    );
                    playerShotImage = new Image(Path.Combine("Assets", "Images", "alternative_bullet.png"));
                    bonus = true;
                    break;
            }
        } else if (gameEvent.EventType == GameEventType.PlayerEvent) { //Maybe dont need this check and instead just run switch cases.
            player.ProcessEvent(gameEvent);

        } else if (gameEvent.EventType == GameEventType.InputEvent) {

            switch (gameEvent.Message) {
                case "KEY_SPACE_RELEASE":
                    playerCentre = player.GetPosition();
                    PlayerShot shot = new PlayerShot(playerCentre, playerShotImage);
                    if (bonus) {
                        shot.Extent = new Vec2F(0.020f, 0.042f);
                    }
                    playerShots.AddEntity(shot);
                    break;
            }
        }
    }

    //Method for shots:
    private void IterateShots() {
        playerShots.Iterate(shot => {
            //shot movement speed:
            shot.Shape.MoveY(0.1f);

            if (shot.Shape.Position.Y > 1) { //Shot is deleted if out of bounds.
                shot.DeleteEntity();

            } else {
                enemies.Iterate(enemy => {
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

    //Method for explosions:
    public void AddExplosion(Vec2F position, Vec2F extent) {
        StationaryShape explosionShape = new StationaryShape(position, extent);
        ImageStride explosionStride = new ImageStride(EXPLOSION_LENGTH_MS / 8, explosionStrides);
        enemyExplosions.AddAnimation(explosionShape, EXPLOSION_LENGTH_MS, explosionStride);
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