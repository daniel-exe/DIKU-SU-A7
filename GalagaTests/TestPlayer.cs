using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using Galaga;


namespace GalagaTests;

public class TestsPlayer {
    private Player player = new Player(
        new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
        new NoImage()
    );

    private GameEvent moveEvent = new GameEvent();

    [SetUp]
    public void Setup() {
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new NoImage()
        );

        moveEvent.EventType = GameEventType.PlayerEvent;
        moveEvent.StringArg1 = "true";
    }

    [Test]
    public void TestMoveLeft() {
        // Arrange
        float oldX = player.GetPosition().X;
        moveEvent.Message = "MOVE_LEFT";
        // Act
        player.ProcessEvent(moveEvent);
        player.Move();
        // Assert
        Assert.True(player.GetPosition().X == (oldX - 0.01f));
    }

    [Test]
    public void TestMoveRight() {
        float oldX = player.GetPosition().X;
        moveEvent.Message = "MOVE_RIGHT";

        player.ProcessEvent(moveEvent);
        player.Move();

        Assert.True(player.GetPosition().X == (oldX + 0.01f));
    }

    [Test]
    public void TestMoveLeftBoundary() {
        moveEvent.Message = "MOVE_LEFT";
        player.ProcessEvent(moveEvent);

        while (player.GetPosition().X > 0.0f) {
            player.Move();
        }

        player.Move();
        Assert.True(player.GetPosition().X == 0.0f);
    }

    [Test]
    public void TestMoveRightBoundary() {
        moveEvent.Message = "MOVE_RIGHT";
        player.ProcessEvent(moveEvent);

        while (player.GetPosition().X < 0.9f) {
            player.Move();
        }

        player.Move();
        Assert.True(player.GetPosition().X == 0.9f);
    }
}
