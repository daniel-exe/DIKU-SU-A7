using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        private Entity backGroundImage;
        private Text[] menuButtons;
        private int activeMenuButton;
        private int maxMenuButtons;
        public static MainMenu GetInstance() {
            if (MainMenu.instance == null) {
                MainMenu.instance = new MainMenu();
                MainMenu.instance.ResetState();
            }
            return MainMenu.instance;
        }

        public void RenderState() {
            // Background Image
            var image = new Image(Path.Combine("Assets", "Images", "TitleImage.png"));
            StationaryShape shape = new StationaryShape(0f, 0f, 1f, 1f);
            backGroundImage = new Entity(shape, image);
            backGroundImage.Render();
            // Menu Buttons
            Vec2F newGamePosition = new Vec2F(0.4f, 0.3f);
            Vec2F newGameExtent = new Vec2F(0.2f, 0.1f);
            Vec2F quitPosition = new Vec2F(0.4f, 0.45f);
            Vec2F quitExtent = new Vec2F(0.2f, 0.1f);
            Text newGame = new Text("- New Game", newGamePosition, newGameExtent);
            Text quit =Text("- Quit", quitPosition, quitExtent);

            menuButtons = new List<Text> { newGame, quit };
            activeMenuButton = 0
            maxMenuButtons = 1
            // Set color and font size
            var green = new Vec3I(0, 204, 0);
            var gray = new Vec3I(192, 192, 192);
            for (int i = 0; i >= maxMenuButtons; i++) {
                if (i == activeMenuButton) {
                    menuButtons[i].SetColor(green);
                } else {
                    menuButtons[i].SetColor(gray);
                }
                menuButtons[i].SetFontSize(16);
            }
            // Render
            foreach (button in menuButtons) {
                menuButtons[i].RenderText();
            }
        }
    }
}
