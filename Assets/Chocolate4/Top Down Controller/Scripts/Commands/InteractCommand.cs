using TopDownController.Controller;
using UnityEngine;

namespace TopDownController.Commands
{
    public class InteractCommand : ICommand 
    {
        /// <param name="chara">this interacts with interactable target, selected chara</param>
        public void Execute(Vector3 point, Character chara)
        {
            Interact(chara);
        }
        private void Interact(Character chara)
        {
            CanInteract toInteractWith = chara.InteractableTarget;
            chara.RotateTowards(toInteractWith.transform.position);

            chara.InteractableTarget = null;
            chara.InteractionWith(toInteractWith);
            toInteractWith.InteractionFrom(chara);
        }
    }
}