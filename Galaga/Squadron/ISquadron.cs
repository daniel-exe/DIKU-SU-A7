namespace Galaga;

using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Physics;
using System.Collections.Generic;



public interface ISquadron {
        EntityContainer<Enemy> Enemies {get;}
        int MaxEnemies {get;}
        void CreateEnemies (List<Image> enemyStride,
            List<Image> alternativeEnemyStride);
    }
