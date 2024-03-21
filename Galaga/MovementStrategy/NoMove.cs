namespace Galaga.MovementStrategy;

using DIKUArcade.Entities;

public class NoMove : IMovementStrategy {
    public void MoveEnemy (Enemy enemy) {}
    public void MoveEnemies (EntityContainer<Enemy> enemies) {}
}
