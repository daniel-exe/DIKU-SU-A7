using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

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

            enemies = new EntityContainer<Enemy>(numEnemies);
            for (int i = 0; i < numEnemies; i++) {
                enemies.AddEntity(new Enemy(
                    new DynamicShape(new Vec2F(0.1f + (float)i * 0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, images)));
            }
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
            enemies.RenderEntities();
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
}
