using UnityEngine;
using TopDownController.Controller;

namespace TopDownController.Entity
{
    public class Enemy : Character
    {
        /// <summary>
        /// Define the behaviour when this enemy clicked on the interactable
        /// </summary>
        /// <param name="interactable">Selected interactable when click happend</param>
        public override void InteractionWith(CanInteract interactable)
        {

        }
        /// <summary>
        /// Define the behaviour when this enemy has been clicked on
        /// </summary>
        /// <param name="interactable">Selected interactable when click happend</param>
        public override void InteractionFrom(CanInteract interactable)
        {
            Debug.Log("I am a bad guy! Deal damage to me here");
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