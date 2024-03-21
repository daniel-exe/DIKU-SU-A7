namespace Galaga.Squadron;

using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System.Collections.Generic;

public interface ISquadron {
        EntityContainer<Enemy> Enemies {get;}
        int MaxEnemies {get;}
        void CreateEnemies (List<Image> enemyStride,
            List<Image> alternativeEnemyStride);
}