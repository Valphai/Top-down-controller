using System.Collections.Generic;
using UnityEngine;

namespace TopDownController
{
    public class ClickInput : MonoBehaviour
    {
        public GameObject marker;
        public LayerMask Clickable;
        public LayerMask Ground;
        public Stack<Vector3> OrderQueue;
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
                if (Input.GetKey(KeyCode.LeftShift))
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
                if (!Input.GetKey(KeyCode.LeftShift))
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

                    // NEED TO DEFINE MOVEMENT QUEUE
                    bool isInRange = 
                        Vector3.Distance(hitClickable.transform.position, chara.transform.position) <= chara.InteractionRange;
                    if (isInRange)
                    {
                        if (hitClickable is Enemy)
                        {
                            AttackAnimation(chara);
                        }
                        hitClickable.Interact(chara);
                    }
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
                OrderQueue.Push(destination);
                
                foreach (var chara in charaSelections.CharaSelected.ToArray())
                {
                    if (chara.IsMoveable)
                        chara.NavigatePosition(destination);
                    else
                        charaSelections.DeSelect(chara);
                }
            }
        }
    }
}
