namespace Galaga.HitStrategy;

using DIKUArcade.Graphics;

public class ChangeColor : IHitStrategy {
    public bool Hit(Enemy enemy) {
        if (enemy.HitPoints <= 0) {
            return true;
        }

        IBaseImage enrageImg = enemy.enemyStridesRed;
        enemy.Image = enrageImg;
        return false;
    }
}