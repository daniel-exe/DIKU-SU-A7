using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Galaga.MovementStrategy;

namespace Galaga;
public class Enemy : Entity, IMovementStrategy {
    public float StartPositionX { get; }
    public float StartPositionY { get; }
    private IMovementStrategy moveStrategy = new NoMove();

    public Enemy(DynamicShape shape, IBaseImage image, IMovementStrategy movementStrategy) : base(shape, image) {
        StartPositionX = shape.Position.X;
        StartPositionY = shape.Position.Y;
        this.moveStrategy = movementStrategy;
    }

    public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image) {
        StartPositionX = shape.Position.X;
        StartPositionY = shape.Position.Y;
    }

    public void MoveEnemy (Enemy enemy) {
        moveStrategy.MoveEnemy(enemy);
    }

    public void MoveEnemies (EntityContainer<Enemy> enemies) {
        foreach (Enemy enemy in enemies) {
            moveStrategy.MoveEnemy(enemy);
        }
    }
}
