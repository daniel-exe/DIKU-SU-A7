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

    public Player player;

    [SetUp]
    public void Setup() {
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new NoImage());
    }

    [Test]
    public void Test1() {
        float oldX = player.GetPosition().X;
        player.SetMoveRight(true);
        player.Move();
        Assert.True(player.GetPosition().X == (oldX + 0.1f));
    }
}
