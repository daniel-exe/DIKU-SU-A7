namespace Galaga.HitStrategy;

public class IncreaseSpeed : IHitStrategy {
    public bool Hit(Enemy enemy) {
        if (enemy.Hitpoints <= 0) {
            return true;
        }
        enemy.Speed *= 10;
        return false;
    }
}