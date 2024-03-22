namespace GalagaTests;

using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using Galaga.GalagaStates;

public class TestStateTransform {
    [Test]
    public void TestStringToStatePass() {
        string validString = "GameRunning";
        GameStateType gameState = StateTransformer.TransformStringToState(validString);
        Assert.AreEqual(GameStateType.GameRunning, gameState);
    }

    [Test]
    public void TestStringToStateFail() {
        string invalidString = "asdf";
        Assert.Throws<ArgumentException>(
            () => StateTransformer.TransformStringToState(invalidString)
        );
    }

    [Test]
    public void TestStateToString1() {
        GameStateType running = GameStateType.GameRunning;
        string gameStateAsString = StateTransformer.TransformStateToString(running);
        Assert.AreEqual("GameRunning", gameStateAsString);
    }

    [Test]
    public void TestStateToString2() {
        GameStateType mainMenu = GameStateType.MainMenu;
        string gameStateAsString = StateTransformer.TransformStateToString(mainMenu);
        Assert.AreEqual("MainMenu", gameStateAsString);
    }
}
