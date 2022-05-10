using TopDownController.Controller;
using UnityEngine;

namespace TopDownController.Commands
{
    public class InteractCommand : ICommand 
    {
        private readonly Character toInteractWith;

        public InteractCommand(Character charaToInteractWith)
        {
            toInteractWith = charaToInteractWith;
        }

        public void Execute(Vector3 point, Character chara)
        {
            Interact(chara);
        }

        private void Interact(Character chara)
        {
            Vector3 target = toInteractWith.transform.position;
            Vector3 charaPos = chara.transform.position;
            Vector3 direction = target - charaPos;

            // to prevent message in unity saying "Look rotation viewing vector is zero"
            if (direction != Vector3.zero)  
            {
                chara.transform.rotation = Quaternion.LookRotation(direction);
            }
            if (toInteractWith is Enemy)
            {
                chara.AttackAnimation();
            }
            toInteractWith.InteractWith(chara);
        }
    }
}