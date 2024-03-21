namespace Galaga;

using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

public class Player {
    private Entity entity;
    private DynamicShape shape;

    private float moveLeft = 0.0f;
    private float moveRight = 0.0f;
    private float moveUp = 0.0f;
    private float moveDown = 0.0f;
    private const float MOVEMENT_SPEED = 0.01f;

    public int Damage = 1;

    public Player(DynamicShape shape, IBaseImage image) {
        entity = new Entity(shape, image);
        this.shape = shape;
    }

    public void Render() {
        entity.RenderEntity();
    }

    private void UpdateDirection() {
        shape.Direction.X = moveLeft + moveRight;
        shape.Direction.Y = moveUp + moveDown;
    }

    public void Move() {
        //Move the shape and guard against the window borders
        float posX = GetPosition().X;
        float nextPosX = posX + moveLeft + moveRight;
        float posY = GetPosition().Y;
        float nextPosY = posY + moveUp + moveDown;
        float leftBoarder = 0;
        float rightBoarder = 1;
        float bottomBoarder = 0;
        float topBoarder = 1;
        if ((nextPosX > leftBoarder && nextPosX < rightBoarder) && (nextPosY > bottomBoarder && nextPosY < topBoarder)) {
            shape.Move();
        }
    }

    private void SetMoveLeft(bool val) {
        if (val) {
            moveLeft -= MOVEMENT_SPEED;
        } else {
            moveLeft = 0;
        }
        UpdateDirection();
    }


    private void SetMoveRight(bool val) {
        if (val) {
            moveRight += MOVEMENT_SPEED;
        } else {
            moveRight = 0;
        }
        UpdateDirection();
    }

    private void SetMoveUp(bool val) {
        if (val) {
            moveUp += MOVEMENT_SPEED;
        } else {
            moveUp = 0;
        }
        UpdateDirection();
    }

    private void SetMoveDown(bool val) {
        if (val) {
            moveDown -= MOVEMENT_SPEED;
        } else {
            moveDown = 0;
        }
        UpdateDirection();
    }

    //Method for getting the players position (the centre coordinates).
    public Vec2F GetPosition() { //maybe make this a method of Entity? since used in player AND enemy?
        Vec2F pos = shape.Position;
        float playerWidth = shape.Extent.X;
        float playerHeight = shape.Extent.Y;
        float centreX = pos.X + (playerWidth / 2);
        float centreY = pos.Y + (playerHeight / 2);
        Vec2F centre = new Vec2F(centreX, centreY);
        return centre;
    }


    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.PlayerEvent) { //Maybe dont need this check and instead just run switch cases.
            switch (gameEvent.Message) {
                case "KEY_LEFT_PRESS":
                    this.SetMoveLeft(true);
                    break;

                case "KEY_RIGHT_PRESS":
                    this.SetMoveRight(true);
                    break;

                case "KEY_UP_PRESS":
                    this.SetMoveUp(true);
                    break;

                case "KEY_DOWN_PRESS":
                    this.SetMoveDown(true);
                    break;

                //Releases:
                case "KEY_LEFT_RELEASE":
                    this.SetMoveLeft(false);
                    break;

                case "KEY_RIGHT_RELEASE":
                    this.SetMoveRight(false);
                    break;

                case "KEY_UP_RELEASE":
                    this.SetMoveUp(false);
                    break;

                case "KEY_DOWN_RELEASE":
                    this.SetMoveDown(false);
                    break;
            }
        }
    }



    // Used for bonus
    public void ChangeImage(IBaseImage newImage) {
        entity = new Entity(shape, newImage);
    }
}