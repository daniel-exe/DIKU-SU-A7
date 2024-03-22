namespace Galaga.MovementStrategy;

using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;
public class ZigZagDown : IMovementStrategy {
    public void MoveEnemy(Enemy enemy) {
        float speed = 0.0003f;
        float period = 0.045f;
        float amplitude = 0.05f;

        float newY = enemy.Shape.Position.Y - speed;

        var piMath = (2 * Math.PI * (enemy.StartPositionY - newY)) / period;
        float piAndSinMath = (float) (amplitude * Math.Sin(piMath));
        float newX = enemy.StartPositionX + piAndSinMath;

        enemy.Shape.SetPosition(new Vec2F(newX, newY));
    }

    public void MoveEnemies(EntityContainer<Enemy> enemies) {
        foreach (Enemy enemy in enemies) {
            MoveEnemy(enemy);
        }
    }
}
