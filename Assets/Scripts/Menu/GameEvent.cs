using UI;

namespace Menu
{
    public struct OnMenuChangeState
    {
        public MenuState preState;
        public MenuState state;

        public OnMenuChangeState(MenuState preState, MenuState state)
        {
            this.preState = preState;
            this.state    = state;
        }
    }
}