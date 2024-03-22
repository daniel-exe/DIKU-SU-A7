using NUnit.Framework;
using System;
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
using Galaga.MovementStrategy;

namespace GalagaTests;
[DefaultFloatingPointTolerance(0.0009)]
public class TestsMovementStrategy {
    private ZigZagDown zigZag = new ZigZagDown();
    private Down down = new Down();
    private NoMove noMove = new NoMove();
    private Enemy enemy = new Enemy(
        new DynamicShape(new Vec2F(0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
        new NoImage()
    );
    private Vec2F oldPosition = new Vec2F(0f, 0f);

    [SetUp]
    public void Setup() {
        enemy = new Enemy(
            new DynamicShape(new Vec2F(0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
            new NoImage()
        );
        oldPosition = enemy.Shape.Position;
    }

    [Test]
    public void TestNoMove() {
        noMove.MoveEnemy(enemy);
        Assert.AreEqual(oldPosition, enemy.Shape.Position);
    }

    [Test]
    public void TestDown() {
        down.MoveEnemy(enemy);
        Assert.That(enemy.Shape.Position.Y, Is.EqualTo(0.899));
    }

    [Test]
    public void TestZigZagDown() {
        float period = 0.045f;
        float amplitude = 0.05f;
        // Set X-value at extreme
        while (enemy.Shape.Position.X > amplitude + 0.0001f) {
            zigZag.MoveEnemy(enemy);
        }
        // Predict Y-value at extreme as being 3/4
        // of the period less than the start position.
        float expectedYValue = enemy.StartPositionY - ((period / 4) * 3);
        // Assert
        Assert.That(enemy.Shape.Position.Y, Is.EqualTo(expectedYValue));
    }
}
