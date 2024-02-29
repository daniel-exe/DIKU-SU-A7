using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Input;
using DIKUArcade.Events;

namespace Galaga;
public class Game : DIKUGame, IGameEventProcessor {
    private Player player;

    public Game(WindowArgs windowArgs) : base(windowArgs) {
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
    }


    public override void Render(){
        player.Render();
    }

    public override void Update(){
        // window.PollEvents();
    }

    private void KeyPress(KeyboardKey key) {
        // TODO: Close window if escape is pressed
        // TODO: switch on key string and set the player's move direction
    }

    private void KeyRelease(KeyboardKey key) {
        // TODO: switch on key string and disable the player's move direction
    }

    private void KeyHandler(KeyboardAction action, KeyboardKey key) {
        // TODO: Switch on KeyBoardAction and call proper method
        }

    public void ProcessEvent(GameEvent gameEvent) {
        // Leave this empty for now
    }
}
