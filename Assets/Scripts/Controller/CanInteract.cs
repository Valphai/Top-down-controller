using UnityEngine;

namespace TopDownController.Controller
{
    public abstract class CanInteract : MonoBehaviour
    {
        protected Outline outline;
        
        public abstract void InteractWith(CanInteract interactable);

        private void Awake()
        {
            outline = GetComponent<Outline>();
        }
        public void Select()
        {
            if (outline)
            {
                outline.Activate();
            }
        }
        public void Deselect()
        {
            if (outline)
            {
                outline.Remove();
            }
        }
        
    }
}