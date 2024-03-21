using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.Collections.Generic;
using System.IO;
using System;

namespace Galaga.Squadron {
    public class Rectangle : ISquadron {
        public EntityContainer<Enemy> Enemies { get; }
        
        public Rectangle() {
            this.Enemies = new EntityContainer<Enemy>();
        }
        public int MaxEnemies { get; }
        public void CreateEnemies(List<Image> enemyStride, List<Image> alternativeEnemyStride) {
            const float startX = 0.3f;   
            const float startY = 0.9f;  
            Random rand = new Random(); 
            int numCols = rand.Next(1, 4);      
            int numRows = rand.Next(2, 4);      
            const float spacingX = 0.1f; 
            const float spacingY = 0.1f; 
            
            for (int row = 0; row < numRows; row++) {
                for (int col = 0; col < numCols; col++) {
                    float posX = startX + col * spacingX;
                    float posY = startY - row * spacingY;
                    this.Enemies.AddEntity(new Enemy(
                        new DynamicShape(new Vec2F(posX, posY), new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStride)
                    ));
                }
            }
        }
    }
}
