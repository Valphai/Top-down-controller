using UnityEngine;
using TopDownController.Controller;

namespace TopDownController.Examples
{
    public class ShopKeeper : Character
    {
        [SerializeField] private GameObject shopMenu;
        public override void InteractWith(CanInteract interactable)
        {
            OpenShop();
        }
        public void CloseShop()
        {
            shopMenu.SetActive(false);
        }
        private void OpenShop()
        {
            shopMenu.SetActive(true);
        }
    }
}