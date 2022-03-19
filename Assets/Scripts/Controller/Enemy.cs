using UnityEngine;

namespace TopDownController
{
    public class Enemy : Character
    {
        /// <summary>
        /// Define the behaviour when this enemy has been clicked on
        /// </summary>
        /// <param name="chara">Selected character when click happend</param>
        public override void Interact(Character chara)
        {
            Debug.Log("I am a bad guy! Deal damage to me here");
        }
    }
}