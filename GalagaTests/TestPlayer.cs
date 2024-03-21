namespace GalagaTests;

using System.IO;
using NUnit.Framework;
using Galaga;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Runtime.InteropServices;

public class TestsPlayer {

    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    private Player player;

    [SetUp]
    public void Setup() {
        //Initialize an instance of a player:
        //Player is initiated with starting position (0.45,0.1).
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("../Galaga", "Assets", "Images", "player.png"))
        );
        //Move player:

        //Moving 35 times up with movespeed = 0.01, moves us from (0.45,0.1) to (0.45,0.45).
        //Since we have initiated with 0.1 width and 0.1 height. the centre will now be in (0.5,0.5)
        for (int i = 0; i < 35; i++) {
            player.SetMoveUp(true); //Simulate 35 keypresses to up.
        }
        player.Move();
        player.SetMoveUp(false); //Simulate key Release.
    }

    [Test]
    public void PlayerCenterTest() {
        //Act:
        Vec2F pos = player.GetPosition();
        float x = pos.X;
        float y = pos.Y;
        //Assert:
        Assert.That(x, Is.EqualTo(0.5).Within(.0001));
        Assert.That(y, Is.EqualTo(0.5).Within(.0001));
    }

    [Test]
    public void OutOfBoundsLeftTest() {
        //Arrange:
        //We perform left and down movements that would move us out of bounds to the bottom and left side.
        for (int i = 0; i < 150; i++) {
            player.SetMoveLeft(true); //Simulate 150 Key Presses to Left.
            player.SetMoveDown(true); //Simulate 150 Key Presses to Down.
            player.Move();
            player.SetMoveLeft(false); //Simulate key Release.
            player.SetMoveDown(false); //Simulate key Release.
        }
        //Act:
        Vec2F pos = player.GetPosition();
        float x = pos.X;
        float y = pos.Y;
        //Assert:
        Assert.That(x, Is.EqualTo(0).Within(.0001));
        Assert.That(y, Is.EqualTo(0).Within(.0001));
    }

    [Test]
    public void OutOfBoundsMaxTest() {
        //Arrange:
        //We perform right and up movements that would move us out of bounds to the top and right side.
        for (int i = 0; i < 150; i++) {
            player.SetMoveRight(true); //Simulate 150 Key Presses to Right.
            player.SetMoveUp(true); //Simulate 150 Key Presses to Up.
            player.Move();
            player.SetMoveRight(false); //Simulate Key Release.
            player.SetMoveUp(false); //Simulate Key Release.
        }
        //Act:
        Vec2F pos = player.GetPosition();
        float x = pos.X;
        float y = pos.Y;
        //Assert:
        Assert.That(x, Is.EqualTo(1).Within(.0001));
        Assert.That(y, Is.EqualTo(1).Within(.0001));
    }
}