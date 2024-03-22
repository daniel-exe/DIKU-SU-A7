namespace Galaga.MovementStrategy;

using DIKUArcade.Entities;
public interface IMovementStrategy {
    void MoveEnemy(Enemy enemy);
    void MoveEnemies(EntityContainer<Enemy> enemies);
}
