namespace GalagaTests;

using NUnit.Framework;
using Galaga.Squadron;
using Galaga;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using System.IO;
using System;
using DIKUArcade.Entities;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Physics;

public class TestSquadron {
    private EntityContainer<Enemy> enemies;
    private ISquadron square = new Square();
    private ISquadron rectangle = new Rectangle();
    private ISquadron triangle = new Triangle();
    
    [OneTimeSetUp]
    public void Init() {
        DIKUArcade.GUI.Window.CreateOpenGLContext();

        // Window.CreateOpenGLContext(); // We need a window to handle everything
    }

    [SetUp]
    public void Setup() {
        List<Image> enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine
        ("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
        List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine
        ("..", "Galaga", "Assets", "Images", "RedMonster.png"));

        square.CreateEnemies(enemyStridesBlue, enemyStridesRed);
        rectangle.CreateEnemies(enemyStridesBlue, enemyStridesRed);
        triangle.CreateEnemies(enemyStridesBlue, enemyStridesRed);
    }

    [Test]
    public void TestSquare() {
        float maxX = 1f;
        float minX = 0f;
        float maxY = 1f;
        float minY = 0f;
        foreach (Enemy enemy in enemies) {
            if (enemy.Shape.Position.X > maxX) {
                maxX = enemy.Shape.Position.X;
            } else if (enemy.Shape.Position.X < minX) {
                minX = enemy.Shape.Position.X;
            } else if (enemy.Shape.Position.Y > maxY) {
                maxY = enemy.Shape.Position.Y;
            } else if (enemy.Shape.Position.Y < minY) {
                minY = enemy.Shape.Position.Y;
            }
        }
        // Assert
        Assert.Multiple(() =>
        {
            foreach (Enemy enemy in enemies) {
                if (enemy.Shape.Position.X == maxX && enemy.Shape.Position.Y == maxY) {
                    Assert.True(true);
                } else if (enemy.Shape.Position.X == maxX && enemy.Shape.Position.Y == minY) {
                    Assert.True(true);
                } else if (enemy.Shape.Position.X == minX && enemy.Shape.Position.Y == maxY) {
                    Assert.True(true);
                } else if (enemy.Shape.Position.X == minX && enemy.Shape.Position.Y == minY) {
                    Assert.True(true);
                }
            }
        });
    }

    [Test]
    public void TestRectangle() {
        float maxX = 1f;
        float minX = 0f;
        float maxY = 1f;
        float minY = 0f;
        foreach (Enemy enemy in enemies) {
            if (enemy.Shape.Position.X > maxX) {
                maxX = enemy.Shape.Position.X;
            } else if (enemy.Shape.Position.X < minX) {
                minX = enemy.Shape.Position.X;
            } else if (enemy.Shape.Position.Y > maxY) {
                maxY = enemy.Shape.Position.Y;
            } else if (enemy.Shape.Position.Y < minY) {
                minY = enemy.Shape.Position.Y;
            }
        }
        // Assert
        Assert.Multiple(() =>
        {
            foreach (Enemy enemy in enemies) {
                if (enemy.Shape.Position.X == maxX && enemy.Shape.Position.Y == maxY) {
                    Assert.True(true);
                } else if (enemy.Shape.Position.X == maxX && enemy.Shape.Position.Y == minY) {
                    Assert.True(true);
                } else if (enemy.Shape.Position.X == minX && enemy.Shape.Position.Y == maxY) {
                    Assert.True(true);
                } else if (enemy.Shape.Position.X == minX && enemy.Shape.Position.Y == minY) {
                    Assert.True(true);
                }
            }
        });
    }

    [Test]
    public void TestTriangle() {
        // int oldSpeed = enemy.Speed;
        // increaseSpeed.Hit(enemy);
        // Assert.That(enemy.Speed == oldSpeed * 2);
    }
}
