using UnityEngine;

namespace TopDownController.Controller
{
    [RequireComponent(typeof(Outline))]
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

        public virtual void OnMouseOver()
        {
            Select();
        }
        public virtual void OnMouseExit()
        {
            DeSelect();
        }
        public virtual void Select()
        {
            if (Outline)
            {
                Outline.enabled = true;
            }
        }
        public virtual void DeSelect()
        {
            if (Outline)
            {
                Outline.enabled = false;
            }
        }
    }
}