using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;


// Squadron ting flyttet 


namespace Galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance = null;
        // Player
        private Player player;
        private EntityContainer<PlayerShot> playerShots;
        private IBaseImage playerShotImage;
        // Enemy
        private EntityContainer<Enemy> enemies;
        private List<Image> images = ImageStride.CreateStrides
            (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        private const int numEnemies = 8;
        private IMovementStrategy moveStrategy;
        // Explosions
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
            // !! skal slettes, men beholdt i tilfælde af det jeg har lavet ikke virker som det skal. !! 
            //enemies = new EntityContainer<Enemy>(numEnemies);
            //for (int i = 0; i < numEnemies; i++) {
            //   enemies.AddEntity(new Enemy(
            //        new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
            //        new ImageStride(80, images)));
            //}
            SpawnSquadron();
            setRndMovementStrat();

            playerShots = new EntityContainer<PlayerShot>();
            playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

            enemyExplosions = new AnimationContainer(numEnemies);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
        }

        // Updates game
        public void UpdateState() {
            GalagaBus.GetBus().ProcessEventsSequentially();
            player.Move();
            IterateShots();
            moveStrategy.MoveEnemies(enemies);
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





            switch (action) {
                case KeyboardAction.KeyPress:
                    if (key = KeyboardKey.Up) {
                        if (activeMenuButton != 0) {
                            activeMenuButton --;
                        }
                    } else if (key = KeyboardKey.Down) {
                        if (activeMenuButton != maxMenuButtons) {
                            activeMenuButton ++;
                        }
                    }
                    break;
                case KeyboardAction.KeyRelease:
                    if (key = KeyboardKey.Enter) {
                        // New Game
                        if (activeMenuButton == 0) {
                            GalagaBus.GetBus().RegisterEvent (
                                new GameEvent {
                                    EventType = GameEventType.GameStateEvent,
                                    Message = "CHANGE_STATE",
                                    StringArg1 = "GAME_RUNNING"
                                }
                            )
                        // Quit
                        } else if (activeMenuButton == 1) {
                            GalagaBus.GetBus().RegisterEvent (
                                new GameEvent {
                                    EventType = GameEventType.WindowEvent,
                                    Message = "CLOSE_WINDOW",
                                }
                            )
                        }
                    }
                    break;
                default:
                    break;
            }
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
        // Randomly selects a movement strategy by using reflection
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
    public void AddExplosion(Vec2F position, Vec2F extent) {
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
