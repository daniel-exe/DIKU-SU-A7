namespace Galaga.MovementStrategy;

using DIKUArcade.Entities;

public class Down : IMovementStrategy {
    public void MoveEnemy(Enemy enemy) {
        float speed = 0.0003f * enemy.Speed;
        enemy.Shape.MoveY(- speed);
    }

    public void MoveEnemies(EntityContainer<Enemy> enemies) {
        foreach (Enemy enemy in enemies) {
            MoveEnemy(enemy);
        }
    }
}
