namespace Galaga.HitStrategy;

using DIKUArcade.Graphics;

public class Enraged : IHitStrategy {
    public bool Hit(Enemy enemy) {
        if (enemy.HitPoints <= 0) {
            return true;
        }

        enemy.Speed *= 2;
        IBaseImage enrageImg = enemy.enemyStridesRed;
        enemy.Image = enrageImg;
        return false;
    }
}