using UnityEngine;

namespace TopDownController.Controller
{
    [RequireComponent(typeof(Outline))]
    public abstract class CanInteract : MonoBehaviour
    {
        [HideInInspector, SerializeField] 
        public CanInteract InteractableTarget;
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
        
        /// <summary>
        /// Set behaviour when interaction is to be given to the clicked chara
        /// </summary>
        public abstract void InteractionWith(CanInteract interactable);
        /// <summary>
        /// Set behaviour when interaction is to be received from the selected chara
        /// </summary>
        public abstract void InteractionFrom(CanInteract interactable);

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