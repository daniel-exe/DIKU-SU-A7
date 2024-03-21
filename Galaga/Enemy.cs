namespace Galaga;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Utilities;
using Galaga.HitStrategy;

public class Enemy : Entity {

    private int maxHitpoints = 10; //Maybe constant.
    private int hitpoints;

    public List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "RedMonster.png"));

    public List<Image> enemyStridesGreen = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "GreenMonster.png"));
    private int speed;

    private List<Type> hitStratList;
    private IHitStrategy hitStrat;
    private DynamicShape shape;

    public bool Enraged = false;

    public int Speed {
        get {return speed;}
        set {speed = value;}
    }

    public int MaxHitpoints {
        get {return maxHitpoints;}
    }

    public int Hitpoints {
        get {return hitpoints;}
        set {
            if (value <= MaxHitpoints) {
                hitpoints = value;
            }
            else {
                hitpoints = MaxHitpoints;
            }
        }
    }

    public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image) {
        this.shape = shape;
        Hitpoints = MaxHitpoints;
        getStrategyList();
    }

    public double GetRandomNumber(double minimum, double maximum){
        return RandomGenerator.Generator.NextDouble() * (maximum - minimum) + minimum;
    }

    private void getStrategyList() {
        var type = typeof(IHitStrategy);
        var strategyList = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p.IsClass)
            .ToList();
        hitStratList = strategyList;
    }

    private IHitStrategy getRndStrategy() {
        int lenghtOfList = hitStratList.Count();
        int rndIndex = RandomGenerator.Generator.Next(0, lenghtOfList);
        IHitStrategy rndStrat = (IHitStrategy) Activator.CreateInstance(hitStratList[rndIndex]);
        return rndStrat;
    }

    public bool GetHit(int damage) {
        Hitpoints -= damage;
        hitStrat = getRndStrategy();
        return hitStrat.Hit(this);
    }
}