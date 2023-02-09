using Card;
using UnityEngine;
using Zenject;

namespace InputSystem
{
    public class PlayerController : ITickable
    {
        private readonly IInput input;

        public PlayerController(IInput input)
        {
            this.input = input;
        }
        
        public void Tick()
        {
            if (input.IsPress())
            {
                Ray ray = input.GetPressRay();

                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20, 1 << LayerMask.NameToLayer("Card"));

                if (hit.collider == null) return;

                CardView cardView = hit.collider.GetComponent<CardView>();
                
                cardView.OnRaycastHit();
            }
        }
    }
}