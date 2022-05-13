namespace TopDownController.Controller
{
    public class Player : Character
    {
        /// <summary>
        /// Define the behaviour when this player has been clicked on
        /// </summary>
        /// <param name="chara">Selected character when click happend</param>
        public override void InteractWith(CanInteract interactable)
        {

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
