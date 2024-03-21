using NUnit.Framework;
using Galaga.Squadron;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Physics;
using System;


namespace GalagaTest {
    [TestFixture]
    public class TriangleTest {
        private GameEventBus eventBus;
        private EntityContainer<Enemy> enemies;


        [Test]
        public void TestTriangleSpawnOutsidePlayerField() {
            //Arrange
            List<Image> images = ImageStride.CreateStrides
                                (4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            Triangle triangle = new Triangle();


            // Act

            triangle.CreateEnemies(images,images);
            enemies = triangle.Enemies;
            // Assert
            foreach (var enemy in enemies) {
                Vec2F enemyPos = enemy.GetCentrum();
                Assert.GreaterOrEqual(enemyPos.Y, 1.0f);
            }
        }
    }
}
