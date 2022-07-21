using UnityEngine;
using TopDownController.Controller;
using System.Collections.Generic;

namespace TopDownController.Examples
{
    public class Die : MonoBehaviour
    {
        public KeyCode KillKey = KeyCode.K;
        private List<Character> charaList;

        private void Start()
        {
            charaList = CharacterSelections.Instance.CharaList;
        }
        void Update()
        {
            if (charaList.Count > 0)
            {
                if (Input.GetKeyDown(KillKey))
                {
                    int randInt = Random.Range(0, charaList.Count);
                    charaList[randInt].Die();
                }
            }
        }
    }
}
