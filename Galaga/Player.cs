namespace Galaga;
using System;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

public class Player : IGameEventProcessor {
    private Entity entity;
    private DynamicShape shape;

    private float moveLeft = 0.0f;
    private float moveRight = 0.0f;
    private float moveUp = 0.0f;
    private float moveDown = 0.0f;
    private const float MOVEMENT_SPEED = 0.015f;

    public int Damage { get; private set; }

    public Player(DynamicShape shape, IBaseImage image) {
        entity = new Entity(shape, image);
        this.shape = shape;
        Damage = 1;
    }

    public void Render() {
        entity.RenderEntity();
    }

    private void UpdateDirection() {
        shape.Direction.X = moveLeft + moveRight;
        shape.Direction.Y = moveUp + moveDown;
    }

    public void Move() {
        Vec2F pos = shape.Position;
        float posX = pos.X;
        float posY = pos.Y;
        float playerWidth = shape.Extent.X;
        float playerHeight = shape.Extent.Y;

        //Defining the borders:
        float leftBorder = 0;
        float bottomBorder = 0;
        //Since player position is calculated from bottom left corner of the entity
        //we have to subtract the player width and height from the right and bottom borders.
        float rightBorder = 1 - playerWidth;
        float topBorder = 1 - playerHeight;

        //Calculating the next position:
        float nextPosX = posX + moveLeft + moveRight;
        float nextPosY = posY + moveUp + moveDown;
        //Checking if next position is out of bounds:
        if (nextPosX > leftBorder && nextPosX < rightBorder &&
        nextPosY > bottomBorder && nextPosY < topBorder) {
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
    public Vec2F GetCentrum() {
        Vec2F pos = shape.Position;
        float playerWidth = shape.Extent.X;
        float playerHeight = shape.Extent.Y;
        float centreX = pos.X + playerWidth / 2;
        float centreY = pos.Y + playerHeight / 2;
        Vec2F centre = new Vec2F(centreX, centreY);
        return centre;
    }


    public Vec2F GetExtent {
        get {
            return this.shape.Extent;
        }
    }

    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.PlayerEvent) {
            bool boolArg = Convert.ToBoolean(gameEvent.StringArg1);
            switch (gameEvent.Message) {
                case "MOVE_LEFT":
                    this.SetMoveLeft(boolArg);
                    break;

                case "MOVE_RIGHT":
                    this.SetMoveRight(boolArg);
                    break;

                case "MOVE_UP":
                    this.SetMoveUp(boolArg);
                    break;

                case "MOVE_DOWN":
                    this.SetMoveDown(boolArg);
                    break;
            }
        }
    }

    // Used for bonus
    public void ChangeImage(IBaseImage newImage) {
        entity = new Entity(shape, newImage);
    }
}
