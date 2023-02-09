using Zenject;

namespace Menu
{
    public enum MenuState
    {
        Initialize     = 0,
        Title          = 1,
        Menu           = 2,
        ToGame         = 3
    }

    public class MenuStateHandler
    {
        public MenuState state { get; private set; } = MenuState.Initialize;

        private readonly SignalBus signalBus;

        public MenuStateHandler(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        public void ChangeState(MenuState state)
        {
            if (this.state == state) return;

            MenuState preState = this.state;
            
            this.state = state;
            
            signalBus.Fire(new OnMenuChangeState(preState, state));
        }
    }
}