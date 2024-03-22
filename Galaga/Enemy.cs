namespace Galaga;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Utilities;
using HitStrategy;

public class Enemy : Entity {

    public float StartPositionX { get; }
    public float StartPositionY { get; }

    private int maxHitPoints = 10;
    private int hitPoints;

    public IBaseImage enemyStridesRed;

    public List<Image> enemyStridesGreen = ImageStride.CreateStrides(2, Path.Combine
    ("Assets", "Images", "GreenMonster.png"));
    private int speed = 1;

    private List<Type> hitStratList;
    private IHitStrategy hitStrat;
    private DynamicShape shape;

    public bool Enraged = false;

    public int Speed {
        get {
            return speed;
        }
        set {
            speed = value;
        }
    }

    public int MaxHitPoints {
        get {
            return maxHitPoints;
        }
    }

    public int HitPoints {
        get {
            return hitPoints;
        }
        set {
            if (value <= MaxHitPoints) {
                hitPoints = value;
            }
            
            else { hitPoints = MaxHitPoints; }
        }
    }

    public Enemy(DynamicShape shape, IBaseImage image, IBaseImage alternativeEnemyImage) : 
    base(shape, image) {
        StartPositionX = shape.Position.X;
        StartPositionY = shape.Position.Y;
        this.shape = shape;
        HitPoints = MaxHitPoints;
        enemyStridesRed = alternativeEnemyImage;
        GetStrategyList();
    }

    public double GetRandomNumber(double minimum, double maximum) {
        return RandomGenerator.Generator.NextDouble() * (maximum - minimum) + minimum;
    }

    private void GetStrategyList() {
        var type = typeof(IHitStrategy);
        var strategyList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p.IsClass)
            .ToList();
        hitStratList = strategyList;
    }

    private IHitStrategy GetRndStrategy() {
        int lenghtOfList = hitStratList.Count();
        int rndIndex = RandomGenerator.Generator.Next(0, lenghtOfList);
        IHitStrategy rndStrat = (IHitStrategy) Activator.CreateInstance(hitStratList[rndIndex]);
        return rndStrat;
    }

    public bool GetHit(int damage) {
        HitPoints -= damage;
        hitStrat = GetRndStrategy();
        return hitStrat.Hit(this);
    }
}
