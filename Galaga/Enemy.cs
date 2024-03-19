using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Galaga.MovementStrategy;

namespace Galaga;
public class Enemy : Entity {
    public float StartPositionX { get; }
    public float StartPositionY { get; }

    // Johan TA's ide
    // private hitStrategy blabla

    public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image) {
        StartPositionX = shape.Position.X;
        StartPositionY = shape.Position.Y;
    }

    // Johan TA's ide
    // public bool Hit() {
    //     return hitStrategy.Hit(this);
    // }

}
