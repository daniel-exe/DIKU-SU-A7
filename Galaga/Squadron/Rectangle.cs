namespace Galaga.Squadron;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using System;
public class Rectangle : ISquadron {
    public EntityContainer<Enemy> Enemies {
        get;
    }

    public Rectangle() {
        this.Enemies = new EntityContainer<Enemy>();
    }
    public int MaxEnemies {
        get;
    }
    public void CreateEnemies(List<Image> enemyStride, List<Image> alternativeEnemyStride) {
        const float START_X = 0.3f;
        const float START_Y = 0.9f;
        Random rand = new Random();
        int numCols = rand.Next(1, 4);
        int numRows = rand.Next(2, 4);
        const float SPACING_X = 0.1f;
        const float SPACING_Y = 0.1f;

        for (int row = 0; row < numRows; row++) {
            for (int col = 0; col < numCols; col++) {
                float posX = START_X + col * SPACING_X;
                float posY = START_Y - row * SPACING_Y;
                this.Enemies.AddEntity(new Enemy(
                    new DynamicShape(new Vec2F(posX, posY), new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStride),
                    new ImageStride(80, alternativeEnemyStride)
                ));
            }
        }
    }
}
