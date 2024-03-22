namespace Galaga.Squadron;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System;
using System.Collections.Generic;


public class Triangle : ISquadron {

    public EntityContainer<Enemy> Enemies {
        get;
    }

    public Triangle() {
        this.Enemies = new EntityContainer<Enemy>();
    }

    public int MaxEnemies {
        get;
    }
    // The triangle instantiates a BASE_X and Y, that constitutes the triangles dimensions
    // where BASE_X is the leftmost vertex' placement.
    // It then calculates the height and uses it to place the top vertex
    // Left vertex uses the forementioned base, and the right most vertex is mirrored
    // Its not as versitile as Square and Rectangle, in terms of build in scaling options.
    public void CreateEnemies(List<Image> enemyStride, List<Image> alternativeEnemyStride) {
        const float BASE_X = 0.4f;
        const float BASE_Y = 0.7f;
        const float SIDE_LENGTH = 0.1f;

        float height = SIDE_LENGTH * (float) Math.Sqrt(3) / 2;

        Vec2F topVertex = new Vec2F(BASE_X + SIDE_LENGTH / 2, BASE_Y - height);
        this.Enemies.AddEntity(new Enemy(
            new DynamicShape(topVertex, new Vec2F(0.1f, 0.1f)),
            new ImageStride(80, enemyStride),
            new ImageStride(80, alternativeEnemyStride)
        ));

        Vec2F leftVertex = new Vec2F(BASE_X, BASE_Y);
        Vec2F rightVertex = new Vec2F(BASE_X + SIDE_LENGTH, BASE_Y);
        this.Enemies.AddEntity(new Enemy(
            new DynamicShape(leftVertex, new Vec2F(0.1f, 0.1f)),
            new ImageStride(80, enemyStride),
            new ImageStride(80, alternativeEnemyStride)
        ));
        this.Enemies.AddEntity(new Enemy(
            new DynamicShape(rightVertex, new Vec2F(0.1f, 0.1f)),
            new ImageStride(80, enemyStride),
            new ImageStride(80, alternativeEnemyStride)
        ));
    }
}