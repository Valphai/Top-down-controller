using System.Collections.Generic;
using TopDownController.Commands;
using TopDownController.Controller;
using TopDownController.Entity;
using UnityEngine;

namespace TopDownController.Examples
{
    public class AttackingPlayer : Character
    {
        [SerializeField] ClickInput clickInput;

        public override void InteractWith(CanInteract interactable)
        {
            // stops agent movement before interaction
            agent.ResetPath();
            if (interactable is Enemy)
            {
                AttackAnimation();
                InteractableTarget = interactable;
                RepeatAttack();
            }
        }
        private void RepeatAttack()
        {
            clickInput.QueueUnits(
                new ICommand[] {
                    new MoveCommand(),
                    new InteractCommand()
                }, 
                new Vector3[] { transform.position },
                new List<Character>() { this }
            );
        }
    }
}
