using UnityEngine;

namespace TopDownController.Controller
{
    public abstract class CanInteract : MonoBehaviour
    {
        private Outline outline;
        public Outline Outline
        {
            get 
            {
                if (outline == null)
                {
                    outline = GetComponent<Outline>();
                }
                return outline;
            }
            private set { outline = value; }
        }        
        
        public abstract void InteractWith(CanInteract interactable);

        private void OnMouseOver()
        {
            if (Outline)
            {
                Outline.enabled = true;
            }
        }
        public void Select()
        {
            if (Outline)
            {
                Outline.enabled = true;
            }
        }
        public void DeSelect()
        {
            if (Outline)
            {
                Outline.enabled = false;
            }
        }
    }
}