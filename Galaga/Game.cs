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

public class Game : DIKUGame, IGameEventProcessor {
    private GameEventBus eventBus;
    private Player player;
    private Vec2F playerCentre;
    //Player shots:
    private EntityContainer<PlayerShot> playerShots;
    private IBaseImage playerShotImage;
    //Explosions:
    private AnimationContainer enemyExplosions;
    private List<Image> explosionStrides;
    private const int EXPLOSION_LENGTH_MS = 500;
    //Squadrons:
    private ISquadron spawnSquad;
    //MovementStrategy:
    private IMovementStrategy moveStrategy;
    //Bonus for fun:
    private bool bonus = false;
    //StateMachine:
    // private StateMachine stateMachine = new StateMachine blabla

    public Game(WindowArgs windowArgs) : base(windowArgs) {
        //Player:
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png"))
        );

        //Evenbus:
        eventBus = new GameEventBus();
        eventBus.InitializeEventBus(new List<GameEventType> {
            GameEventType.InputEvent,
            GameEventType.PlayerEvent,
            GameEventType.WindowEvent
        });

        window.SetKeyEventHandler(KeyHandler);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        eventBus.Subscribe(GameEventType.PlayerEvent, player); //Subscribed to player

        //Enemies:
        SpawnSquadron();

        //Playershots:
        playerShots = new EntityContainer<PlayerShot>();
        playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

        //Explosions:
        enemyExplosions = new AnimationContainer(8);
        explosionStrides = ImageStride.CreateStrides(8,
            Path.Combine("Assets", "Images", "Explosion.png"));

        //Movement Strategy:
        setRndMovementStrat();
    }
    public override void Render() {
        player.Render();
        playerShots.RenderEntities();
        spawnSquad.Enemies.RenderEntities();
        enemyExplosions.RenderAnimations();
    }

    public override void Update() {
        window.PollEvents();
        eventBus.ProcessEventsSequentially();
        player.Move();
        IterateShots();
        moveStrategy.MoveEnemies(spawnSquad.Enemies);
        // Something like stateMachine.ActiveState.UpdateState();
    }

    private void KeyPress(KeyboardKey key) {
        switch (key) {
            case KeyboardKey.Left:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_LEFT", //Could maybe make just ONE registerevent, and then save a new message for each keypress? .
                });
                break;
            case KeyboardKey.Right:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_RIGHT",
                });
                break;
            case KeyboardKey.Up:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_UP",
                });
                break;
            case KeyboardKey.Down:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "true",
                    Message = "MOVE_DOWN",
                });
                break;

            //Close window if escape is pressed
            case KeyboardKey.Escape:
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
                    StringArg1 = "false",
                    Message = "MOVE_LEFT",
                });
                break;
            case KeyboardKey.Right:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "false",
                    Message = "MOVE_RIGHT",
                });
                break;
            case KeyboardKey.Up:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "false",
                    Message = "MOVE_UP",
                });
                break;
            case KeyboardKey.Down:
                eventBus.RegisterEvent(new GameEvent {
                    From = this,
                    EventType = GameEventType.PlayerEvent,
                    StringArg1 = "false",
                    Message = "MOVE_DOWN",
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
        // Something like stateMachine.ActiveState.HandleKeyEvent(action, key);
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

    //Method for shots:
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

    //Method for explosions:
    public void AddExplosion(Vec2F position, Vec2F extent) {
        StationaryShape explosionShape = new StationaryShape(position, extent);
        ImageStride explosionStride = new ImageStride(EXPLOSION_LENGTH_MS / 8, explosionStrides);
        enemyExplosions.AddAnimation(explosionShape, EXPLOSION_LENGTH_MS, explosionStride);
    }


    //Method that creates enemies.
    public void SpawnSquadron() {
       //if (spawnSquad == null || spawnSquad.Enemies.CountEntities() == 0) // HVIS VI SKAL HAVE ENDLES MODE
        if (spawnSquad == null) {
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
    }

    private void setRndMovementStrat() {
        var moveStrategyList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IMovementStrategy).IsAssignableFrom(p) && p.IsClass)
            .ToList();

        int LengthOfList = moveStrategyList.Count();
        int rndIndex = RandomGenerator.Generator.Next(0, LengthOfList);
        moveStrategy = (IMovementStrategy)Activator.CreateInstance(moveStrategyList[rndIndex]);
    }
}
