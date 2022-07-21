using TopDownController.Controller;
using UnityEngine;

namespace TopDownController.Entity
{
    public class Player : Character
    {
        /// <summary>
        /// Define the behaviour when this player clicked on interactable
        /// </summary>
        /// <param name="interactable">Selected interactable when click happend</param>
        public override void InteractionWith(CanInteract interactable)
        {
            // stops agent movement before interaction
            agent.ResetPath();
            if (interactable is Enemy)
            {
                AttackAnimation();
            }
        }
        /// <summary>
        /// Define the behaviour when this player has been clicked on
        /// </summary>
        /// <param name="interactable">Selected interactable when click happend</param>
        public override void InteractionFrom(CanInteract interactable)
        {
            if (interactable is Enemy)
            {
                Debug.Log("I'm losing hp here!");
            }
        }
        /// <summary>
        /// Modify move animation behaviour here
        /// </summary>
        public override void MoveAnimation()
        {
            base.MoveAnimation();
        }
        /// <summary>
        /// Modify attack animation behaviour here
        /// </summary>
        public override void AttackAnimation()
        {
            base.AttackAnimation();
        }
        /// <summary>
        /// Modify die animation behaviour here
        /// </summary>
        public override void DieAnimation()
        {
            base.DieAnimation();
        }
        /// <summary>
        /// This is a method you'd want to trigger whenever a character dies,
        /// it disables NavMeshAgent, Animator, ... . Also activates ragdoll if
        /// chosen appropriate prefab otherwise does what DieAnimation says.
        /// </summary>
        public override void Die()
        {
            base.Die();
        }
    }
}
