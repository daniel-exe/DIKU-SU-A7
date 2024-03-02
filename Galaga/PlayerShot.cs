using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga;
public class PlayerShot : Entity {
    private static Vec2F extent = new Vec2F(0.008f, 0.021f);
    private static Vec2F direction = new Vec2F(0.0f, 0.1f);

    public void Move() {
        this.Shape.Move();
    }

    public Vec2F GetPosition() {
        return this.Shape.Position;
    }

    public PlayerShot(Vec2F position, IBaseImage image)
        : base((new DynamicShape(position, extent, direction)), image) {}
}
