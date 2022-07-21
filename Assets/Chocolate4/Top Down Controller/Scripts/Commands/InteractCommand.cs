using TopDownController.Controller;
using UnityEngine;

namespace TopDownController.Commands
{
    public class InteractCommand : ICommand 
    {
        public void Execute(Vector3 point, Character chara)
        {
            Interact(chara);
        }
        private void Interact(Character chara)
        {
            CanInteract toInteractWith = chara.InteractableTarget;
            chara.RotateTowards(toInteractWith.transform.position);

            chara.InteractableTarget = null;
            chara.InteractWith(toInteractWith);
            toInteractWith.InteractWith(chara);
        }
    }
}