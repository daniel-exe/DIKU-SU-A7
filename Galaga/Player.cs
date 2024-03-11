using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga;
public class Player {
    private Entity entity;
    private DynamicShape shape;
    private float moveLeft = 0.0f;
    private float moveRight = 0.0f;
    private const float MOVEMENT_SPEED = 0.01f;

    public Player(DynamicShape shape, IBaseImage image) {
        entity = new Entity(shape, image);
        this.shape = shape;
    }

    public void Render() {
        entity.RenderEntity();
    }

    public void Move() {
        shape.Move();
        if (shape.Position.X > 1.0f - shape.Extent.X) {
            shape.Position.X = 1.0f - shape.Extent.X;
        }
        if (shape.Position.X < 0.0f) {
            shape.Position.X = 0.0f;
        }
    }

    public void SetMoveLeft(bool val) {
        if (val) {
            moveLeft -= MOVEMENT_SPEED;
        } else {
            moveLeft = 0.0f;
        }
        UpdateDirection();
    }

    public void SetMoveRight(bool val) {
        if (val) {
            moveRight += MOVEMENT_SPEED;
        } else {
            moveRight = 0.0f;
        }
        UpdateDirection();
    }

    private void UpdateDirection() {
        shape.ChangeDirection( new Vec2F((moveLeft + moveRight), shape.Direction.Y) );
    }

    public Vec2F GetPosition() {
        return entity.Shape.Position;
    }
}
