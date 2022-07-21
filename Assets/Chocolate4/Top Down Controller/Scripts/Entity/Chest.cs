using UnityEngine;
using TopDownController.Controller;

namespace TopDownController.Entity
{
    public class Chest : CanInteract
    {
        /// <summary>
        /// Define the behaviour when this chest clicked on a interactable
        /// </summary>
        /// <param name="interactable">Selected interactable when click happend</param>
        public override void InteractionWith(CanInteract interactable)
        {

        }
        /// <summary>
        /// Define the behaviour when this chest has been clicked on
        /// </summary>
        /// <param name="interactable">Selected interactable when click happend</param>
        public override void InteractionFrom(CanInteract interactable)
        {
            Debug.Log("I'm a chest!");
        }
    }
}