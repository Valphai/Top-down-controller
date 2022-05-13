using UnityEngine;

namespace TopDownController.Controller
{
    public class Marker : MonoBehaviour
    {
        public void ClickedOn(CanInteract interactable)
        {
            
        }
        public void MoveTo(Vector3 point)
        {
            transform.position = point;
        }
    }
}