namespace Galaga.HitStrategy;
public interface IHitStrategy {
    bool Hit(Enemy enemy); //Returns true when hit causes death, false when hit but not dead.
}