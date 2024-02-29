using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga;
public class Player {
    private Entity entity;
    private DynamicShape shape;
    private float moveLeft = 0.0f;
    private float moveRight = 0.0f;
    private const float MOVEMENT_SPEED = 0.1f;

    public Player(DynamicShape shape, IBaseImage image) {
        entity = new Entity(shape, image);
        this.shape = shape;
    }

    public void Render() {
        entity.RenderEntity();
    }

    public void Move() {
        // TODO: move the shape and guard against the window borders
    }

    public void SetMoveLeft(bool val) {
        if (val == true) {
            moveLeft += MOVEMENT_SPEED;
        } else {
            moveLeft -= MOVEMENT_SPEED;
        }
    }

    public void SetMoveRight(bool val) {
        if (val == true) {
            moveRight += MOVEMENT_SPEED;
        } else {
            moveRight -= MOVEMENT_SPEED;
        }
        // TODO:set moveRight appropriately and call UpdateDirection()
    }

}
