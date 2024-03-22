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
    private GameEventBus eventBus;
    private EntityContainer<Enemy> enemies;

    public GameEventBus EventBus {
        get => EventBus1;
        set => EventBus1 = value;
    }
    public GameEventBus EventBus1 {
        get => eventBus;
        set => eventBus = value;
    }

    [Test]
        public void TestTriangleSpawnOutsidePlayerField() {
            List<Image> images = ImageStride.CreateStrides
                                (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            Triangle triangle = new Triangle();

            triangle.CreateEnemies(images,images);

            foreach (var enemy in enemies) {
                Vec2F enemyPos = enemy.GetCentrum();
                Assert.GreaterOrEqual(enemyPos.Y, 1.0f);
            }
        }
}
