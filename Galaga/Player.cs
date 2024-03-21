namespace Galaga;

using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Physics;
using System.Collections.Generic;
using System;
public class Player : IGameEventProcessor {
    private Entity entity;
    private DynamicShape shape;
    //Lav positions !!!!
    private float moveLeft = 0.0f; //should have get/set properties??
    private float moveRight = 0.0f; //should have get/set properties??
    private const float MOVEMENT_SPEED = 0.01f; //Private ??
    public Player(DynamicShape shape, IBaseImage image) {
        entity = new Entity(shape, image);
        this.shape = shape;
    }

    public void Render() {
        entity.RenderEntity();
    }

    private void UpdateDirection() {
        shape.Direction.X = moveLeft + moveRight; //Galaga only has left/right movement, so we only update X.
    }

    public void Move() {
        // TODO: move the shape and guard against the window borders
        float posX = GetPosition().X;
        float nextPos = posX + moveLeft + moveRight;
        float leftBoarder = 0;
        float rightBoarder = 1;
        if (nextPos > leftBoarder && nextPos < rightBoarder) {
            shape.Move();
        }
    }
    public void SetMoveLeft(bool val) {
        if (val) {
            moveLeft -= MOVEMENT_SPEED;
        } else {
            moveLeft = 0;
        }
        UpdateDirection();
    }


    public void SetMoveRight(bool val) {
        if (val) {
            moveRight += MOVEMENT_SPEED;
        } else {
            moveRight = 0;
        }
        UpdateDirection();
    }
    public Vec2F GetExtent{
        get {return this.shape.Extent;}
    }
    //Method for getting the players position (the centre coordinates).
    public Vec2F GetPosition() {
        Vec2F pos = shape.Position;

        float playerWidth = shape.Extent.X;
        float playerHeight = shape.Extent.Y;

        float centreX = pos.X + (playerWidth/2);
        float centreY = pos.Y + (playerHeight/2);
        Vec2F centre = new Vec2F(centreX, centreY);
        return centre;
    }


    // lol... :)
    public void ChangeImage(IBaseImage newImage) {
        entity = new Entity(shape, newImage);
    }
    public void ProcessEvent(GameEvent gameEvent) {
        switch(gameEvent.Message) {  
            case "MoveLeft" :
                if (gameEvent.StringArg1 == "True") {
                    this.SetMoveLeft(true);
                }
                else{
                    this.SetMoveLeft(false);
                }
                break;
            case "MoveRight" :
                if (gameEvent.StringArg1 == "True") {
                    SetMoveRight(true);
                }
                else{
                    SetMoveRight(false);
                }
                break;
            default :
                break;
            
        }
    }
}