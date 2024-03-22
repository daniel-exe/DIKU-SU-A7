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

    public float StartPositionX {
        get;
    } //til TA: burde dette måske laves med field + properties?
    public float StartPositionY {
        get;
    } //til TA: burde dette måske laves med field + properties?

    private int maxHitpoints = 10; //Maybe constant.
    private int hitpoints;

    public IBaseImage enemyStridesRed;

    //Bruges ikke!!!??:
    public List<Image> enemyStridesGreen = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "GreenMonster.png"));
    private int speed;

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

    public int MaxHitpoints {
        get {
            return maxHitpoints;
        }
    }

    public int Hitpoints {
        get {
            return hitpoints;
        }
        set {
            if (value <= MaxHitpoints) {
                hitpoints = value;
            } else {
                hitpoints = MaxHitpoints;
            }
        }
    }

    public Enemy(DynamicShape shape, IBaseImage image, IBaseImage alternativeEnemyImage) : base(shape, image) {
        StartPositionX = shape.Position.X;
        StartPositionY = shape.Position.Y;
        this.shape = shape;
        Hitpoints = MaxHitpoints;
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
        Hitpoints -= damage;
        hitStrat = GetRndStrategy();
        return hitStrat.Hit(this);
    }
}