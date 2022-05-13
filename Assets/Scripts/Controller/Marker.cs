using UnityEngine;

namespace TopDownController.Controller
{
    public class Marker : MonoBehaviour
    {
        /// <summary>
        /// Define here the behaviour when clicked on interactable
        /// </summary>
        public void ClickedOn(CanInteract interactable)
        {
            
        }
        /// <summary>
        /// Define here the behaviour when clicked on the ground
        /// </summary>
        public void MoveTo(Vector3 point)
        {
            transform.position = point;
        }
    }
}