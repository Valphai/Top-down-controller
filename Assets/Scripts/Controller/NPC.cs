using UnityEngine;

namespace TopDownController
{
    public class NPC : Character
    {
        /// <summary>
        /// Define the behaviour when this npc has been clicked on
        /// </summary>
        /// <param name="chara">Selected character when click happend</param>
        public override void Interact(Character chara)
        {
            Debug.Log("I'm an NPC! Interact with me here!");
        }
    }
}
