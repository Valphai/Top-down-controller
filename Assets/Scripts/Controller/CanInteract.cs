using UnityEngine;
using UnityEngine.EventSystems;

namespace TopDownController.Controller
{
    public abstract class CanInteract : MonoBehaviour, IPointerClickHandler
    {
        protected Outline outline;
        private float clicked;
        private float clicktime;
        private float clickdelay = 0.5f;
        
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
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("YEs");
            clicked++;
    
            if (clicked == 1)
                clicktime = Time.time;
    
            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                CharacterSelections.Instance.LockTransform(transform);
            }
            else if (clicked > 2 || Time.time - clicktime > 1)
                clicked = 0;
        }
    }
}