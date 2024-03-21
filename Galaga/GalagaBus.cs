namespace Galaga;

using DIKUArcade.Events;

public static class GalagaBus {
    private static GameEventBus eventBus;
    public static GameEventBus GetBus() {
        return GalagaBus.eventBus ?? (GalagaBus.eventBus =
                                    new GameEventBus());
    }
}
