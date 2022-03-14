using System.Collections;
using UnityEngine;

namespace TopDownController
{
    public class ClickInput : MonoBehaviour
    {
        public GameObject marker;
        public LayerMask Clickable;
        public LayerMask Ground;
        /// <summary> use this key pressing on units to add them to selection </summary>
        public const KeyCode SelectUnits = KeyCode.LeftControl;
        /// <summary> use this key to queue movement order of units </summary>
        public const KeyCode QueueUnits = KeyCode.LeftShift;
        private CharacterSelections charaSelections;
        private Camera cam;

        private void Awake()	
        {
            cam = Camera.main;
            charaSelections = 
                GameObject.FindGameObjectWithTag("CharaSelections").GetComponent<CharacterSelections>();
        }
        private void Update()	
        {
            if (Input.GetMouseButtonDown(0))
            {
                LMB();
            }
            if (Input.GetMouseButtonDown(1))
            {
                RMB();
            }
        }

        private void LMB()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Clickable))
            {
                Character hitClickable = hitInfo.transform.GetComponent<Character>();
                if (Input.GetKey(SelectUnits))
                {
                    charaSelections.ShiftClickSelect(hitClickable);
                }
                else
                {
                    charaSelections.ClickSelect(hitClickable);
                }
            }
            else
            {
                if (!Input.GetKey(SelectUnits))
                {
                    charaSelections.DeselectAll();
                }
            }
        }

        private void RMB()
        {
            MoveCharacter();
            Interact();
        }

        private void Interact()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitinfo, Mathf.Infinity, Clickable))
            {
                Character hitClickable = hitinfo.transform.GetComponent<Character>();
                
                foreach (Character chara in charaSelections.CharaSelected)
                {
                    bool isInRange = 
                        Vector3.Distance(hitClickable.transform.position, chara.transform.position) <= 
                                        chara.InteractionRange;
                    
                    chara.MoveOrderQueue.Clear();
                    if (hitClickable is Enemy)
                    {
                        AttackAnimation(chara);
                        Debug.Log("Attack");
                    }
                    hitClickable.Interact(chara);
                }
            }
        }

        private void AttackAnimation(Character chara)
        {
            chara.Anim.SetTrigger("attack");
        }

        private void MoveCharacter()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Ground))
            {
                Vector3 destination = hitInfo.point;
                marker.transform.position = destination;
                
                foreach (var chara in charaSelections.CharaSelected.ToArray())
                {
                    if (chara.IsMoveable)
                    {
                        if (Input.GetKey(QueueUnits))
                        {
                            chara.MoveOrderQueue.Enqueue(destination);
                        }
                        else
                        {
                            chara.MoveOrderQueue.Clear();
                            chara.NavigatePosition(destination);
                        }
                    }
                    else
                        charaSelections.DeSelect(chara);
                }
            }
        }
    }
}
