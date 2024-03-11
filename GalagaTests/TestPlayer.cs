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

    [SetUp]
    public void Setup() {}

    private Player player = new Player(
        new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
        new NoImage()
    );

    [Test]
    public void TestMoveLeft() {
        // Arrange
        player.SetMoveRight(true);
        player.Move();
        player.SetMoveRight(false);
        float oldX = player.GetPosition().X;
        // Act
        player.SetMoveLeft(true);
        player.Move();
        player.SetMoveLeft(false);
        // Assert
        Assert.True(player.GetPosition().X == (oldX - 0.01f));
    }

    [Test]
    public void TestMoveRight() {
        float oldX = player.GetPosition().X;
        player.SetMoveRight(true);

        player.Move();
        player.SetMoveRight(false);

        Assert.True(player.GetPosition().X == (oldX + 0.01f));
    }

    [Test]
    public void TestMoveBoundary() {
        player.SetMoveLeft(true);
        while (player.GetPosition().X > 0.0f) {
            player.Move();
        }

        player.Move();
        player.SetMoveLeft(false);

        Assert.True(player.GetPosition().X == 0.0f);
    }
}
