namespace Galaga.HitStrategy;

public class Jump : IHitStrategy {
    public bool Hit(Enemy enemy) {
        if (enemy.Hitpoints <= 0) {
            return true;
        }
        float enemyWidth = enemy.Shape.Extent.X;
        float enemyHeight = enemy.Shape.Extent.Y;
        //We subtract the width/height from the max value since the coordinate is lower left coorner of enemy.¨
        enemy.Shape.Position.X = (float) enemy.GetRandomNumber(0.0, 1.0 - enemyWidth);
        //Enemy can only be above lower third of screen.
        enemy.Shape.Position.Y = (float) enemy.GetRandomNumber((double) 1 / 3, 1.0 - enemyHeight);

        return false;
    }


}