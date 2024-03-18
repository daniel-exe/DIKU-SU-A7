using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

namespace Galaga;
public class Player : IGameEventProcessor{
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

    private void SetMoveLeft(bool val) {
        if (val) {
            moveLeft -= MOVEMENT_SPEED;
        } else {
            moveLeft = 0.0f;
        }
        UpdateDirection();
    }

    private void SetMoveRight(bool val) {
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

    public void ProcessEvent(GameEvent gameEvent) {
        if (gameEvent.EventType == GameEventType.PlayerEvent) {
            bool boolArg = Convert.ToBoolean(gameEvent.StringArg1);
            switch (gameEvent.Message) {
                case "MOVE_LEFT":
                    SetMoveLeft(boolArg);
                    break;
                case "MOVE_RIGHT":
                    SetMoveRight(boolArg);
                    break;
            }
        }
    }
}
