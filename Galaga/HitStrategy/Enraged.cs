namespace Galaga.HitStrategy;

using DIKUArcade.Graphics;

public class Enraged : IHitStrategy {
    public bool Hit(Enemy enemy) {
        if (enemy.Hitpoints <= 0) {
            return true;
        }

        //Only triggers enrage if not already enraged (See rapport for more info)
        // if (! enemy.Enraged) {
        //     enemy.Speed *= 2;
        //     ImageStride enrageImg = new ImageStride(80, enemy.enemyStridesRed);
        //     enemy.Image = enrageImg;
        //     enemy.Enraged = true;
        // }

        enemy.Speed *= 2;
        ImageStride enrageImg = new ImageStride(80, enemy.enemyStridesRed);
        enemy.Image = enrageImg;
        return false;
    }
}