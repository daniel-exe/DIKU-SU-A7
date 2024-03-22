namespace GalagaTests;

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
using Galaga.HitStrategy;

[DefaultFloatingPointTolerance(0.0009)]
public class TestsHitStrategy {
    private IHitStrategy jump = new Jump();
    private IHitStrategy increaseSpeed = new IncreaseSpeed();
    private IHitStrategy enraged = new Enraged();
    private IHitStrategy changeColor = new ChangeColor();

    private Enemy enemy = new Enemy(
        new DynamicShape(new Vec2F(0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
        new NoImage(),
        new NoImage()
    );
    private Vec2F oldPosition = new Vec2F(0f, 0f);

    private List<Image> enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
    private List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("..", "Galaga", "Assets", "Images", "RedMonster.png"));
    
    private Enemy colorEnemy = new Enemy( 
        new DynamicShape(new Vec2F(0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
        new NoImage(),
        new NoImage()
    );
    private IBaseImage startImg = new Image(Path.Combine("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
    
    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();

        // Window.CreateOpenGLContext(); // We need a window to handle everything
    }
    
    [SetUp]
    public void Setup() {
        enemy = new Enemy(
            new DynamicShape(new Vec2F(0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
            new NoImage(),
            new NoImage()
        );
        oldPosition = enemy.Shape.Position;
        
        colorEnemy = new Enemy( 
            new DynamicShape(new Vec2F(0.1f, 0.9f), new Vec2F(0.1f, 0.1f)),
            new ImageStride(80, enemyStridesBlue),
            new ImageStride(80, enemyStridesRed)
        );
        startImg = colorEnemy.Image;
    }

    [Test]
    public void TestChangeColor() {
        enemy.HitPoints = 5;     
        changeColor.Hit(enemy);
        Assert.That(enemy.Image != startImg);
    }

    [Test]
    public void TestJump() {
        jump.Hit(enemy);
        Assert.That(enemy.Shape.Position != oldPosition);
    }

    [Test]
    public void TestIncreaseSpeed() {
        int oldSpeed = enemy.Speed;
        increaseSpeed.Hit(enemy);
        Assert.That(enemy.Speed == oldSpeed * 2);
    }

    [Test]
    public void TestEnraged() {
    int oldSpeed = enemy.Speed;
    enemy.HitPoints = 5;

    enraged.Hit(enemy);
    
    Assert.Multiple(() =>
    {
        Assert.AreEqual(enemy.Speed, oldSpeed * 2);
        Assert.That(enemy.Image != startImg);
    });
    }
}
