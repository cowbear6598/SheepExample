using Zenject;

namespace Menu
{
    public interface IMenuService
    {
        void DoChangeState(MenuState state);
        MenuState GetState();
    }

    public class MenuService : IMenuService
    {
        [Inject] private readonly MenuStateHandler   stateHandler;

        public void DoChangeState(MenuState state) => stateHandler.ChangeState(state);
        public MenuState GetState() => stateHandler.state;
    }
}