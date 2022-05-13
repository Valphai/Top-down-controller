using UnityEngine;

namespace TopDownController.Controller
{
    public abstract class CanInteract : MonoBehaviour
    {
        protected Outline outline;
        
        public abstract void InteractWith(CanInteract interactable);

        private void Start()
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
        public void DeSelect()
        {
            if (outline)
            {
                outline.Remove();
            }
        }
        
    }
}