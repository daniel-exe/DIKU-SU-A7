using DIKUArcade.Entities;

namespace Galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        public void MoveEnemy (Enemy enemy) {
            enemy.Shape.MoveY(-0.001f);
        }

        public void MoveEnemies (EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}
