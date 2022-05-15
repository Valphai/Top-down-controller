using UnityEngine;
using TopDownController.Controller;

namespace TopDownController.Entity
{
    public class Chest : CanInteract
    {
        /// <summary>
        /// Define the behaviour when this chest has been clicked on
        /// </summary>
        /// <param name="interactable">Selected interactable when click happend</param>
        public override void InteractWith(CanInteract interactable)
        {
            Debug.Log("I'm a chest!");
        }
    }
}