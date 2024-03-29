namespace Galaga.HitStrategy;

public class IncreaseSpeed : IHitStrategy {
    public bool Hit(Enemy enemy) {
        if (enemy.HitPoints <= 0) {
            return true;
        }
        enemy.Speed *= 2;
        return false;
    }
}