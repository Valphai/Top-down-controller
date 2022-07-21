using System.Collections;
using TopDownController.Controller;
using TopDownController.Entity;
using UnityEngine;

namespace TopDownController.Examples
{
    public class AttackingPlayer : Character
    {
        [SerializeField] private ClickInput clickInput;
        [SerializeField] private float attackSpeed = 1f;
        private IEnumerator attacking;

        public override void Awake()
        {
            base.Awake();
            queueReset += StopAttack;
        }
        public override void OnDisable()
        {
            queueReset -= StopAttack;
        }
        public override void InteractionWith(CanInteract interactable)
        {
            // stops agent movement before interaction
            agent.ResetPath();
            if (interactable is Enemy)
            {
                AttackAnimation();
                InteractableTarget = interactable;
                attacking = RepeatAttack();
                StartCoroutine(attacking);
            }
        }
        public override void InteractionFrom(CanInteract interactable)
        {
            
        }
        private void StopAttack()
        {
            if (attacking != null)
            {
                StopCoroutine(attacking);
            }
        }
        private IEnumerator RepeatAttack()
        {
            yield return new WaitForSeconds(attackSpeed);

            clickInput.InteractOnTheSpotUsing(this);
        }
    }
}
