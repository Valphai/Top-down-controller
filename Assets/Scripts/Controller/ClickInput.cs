using System;
using UnityEngine;

namespace TopDownController.Controller
{
    public class ClickInput : MonoBehaviour
    {
        public GameObject Marker;
        public LayerMask Clickable;
        public LayerMask Ground;
        
        [Tooltip("use this key pressing on units to add them to selection")]
        public const KeyCode SelectUnitsButton = KeyCode.LeftControl;

        [Tooltip("use this key to queue movement order of units")]
        public const KeyCode QueueUnitsButton = KeyCode.LeftShift;
        
        [Tooltip("use this key to lock the camera onto the selected character")]
        public const KeyCode LockUnitButton = KeyCode.Y;
        private CharacterSelections charaSelections;
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
            charaSelections = CharacterSelections.Instance;
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

            UpdateUnitQueue();
        }
        private void UpdateUnitQueue()
        {
            foreach (Character chara in charaSelections.CharaList.ToArray())
            {
                if (chara.PathCompleted)
                {
                    chara.FollowTheQueue();
                }
            }
            foreach (Character chara in charaSelections.CharaSelected.ToArray())
            {
                if(!Input.GetKey(QueueUnitsButton) && Input.GetMouseButtonDown(1))
                {
                    chara.FollowTheQueue();
                }
            }
        }

        private void LMB()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Clickable))
            {
                Character hitClickable = hitInfo.transform.GetComponent<Character>();
                if (Input.GetKey(SelectUnitsButton))
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
                if (!Input.GetKey(SelectUnitsButton))
                {
                    charaSelections.DeselectAll();
                }
            }
        }

        private void RMB()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Clickable))
            {
                Action<RaycastHit, Character> action = MoveTowards;
                Action<RaycastHit, Character> action2 = Interact;
                QueueUnits(hitInfo, action, action2);
            }
            else if (Physics.Raycast(ray, out RaycastHit hitInfo2, Mathf.Infinity, Ground))
            {
                Marker.transform.position = hitInfo2.point;
                Action<RaycastHit, Character> action = MoveCharacter;
                QueueUnits(hitInfo2, action);
            }
        }

        private void QueueUnits(RaycastHit hitInfo, Action<RaycastHit, Character> action)
        {
            foreach (Character chara in charaSelections.CharaSelected.ToArray())
            {
                if (Input.GetKey(QueueUnitsButton))
                {
                    chara.MoveOrderQueue.Enqueue(() => action(hitInfo, chara));
                }
                else
                {
                    chara.MoveOrderQueue.Clear();
                    chara.MoveOrderQueue.Enqueue(() => action(hitInfo, chara));
                }
            }
        }
        private void QueueUnits(RaycastHit hitInfo, Action<RaycastHit, Character> action, 
            Action<RaycastHit, Character> action2)
        {
            foreach (Character chara in charaSelections.CharaSelected.ToArray())
            {
                if (Input.GetKey(QueueUnitsButton))
                {
                    chara.MoveOrderQueue.Enqueue(() => action(hitInfo, chara));
                    chara.MoveOrderQueue.Enqueue(() => action2(hitInfo, chara));
                }
                else
                {
                    chara.MoveOrderQueue.Clear();
                    chara.MoveOrderQueue.Enqueue(() => action(hitInfo, chara));
                    chara.MoveOrderQueue.Enqueue(() => action2(hitInfo, chara));
                }
            }
        }

        private void MoveTowards(RaycastHit hitInfo, Character chara)
        {
            Character hitClickable = hitInfo.transform.GetComponent<Character>();

            Vector3 target = hitClickable.transform.position;
            Vector3 charaPos = chara.transform.position;

            if (Vector3.Distance(target, charaPos) > chara.InteractionRange)
            {
                Vector3 standByPoint =
                    target + ((charaPos - target).normalized) * chara.InteractionRange;
                MoveCharacter(standByPoint, chara);
            }
            else
                MoveCharacter(charaPos, chara);
        }

        private void Interact(RaycastHit hitInfo, Character chara)
        {
            Character hitClickable = hitInfo.transform.GetComponent<Character>();

            Vector3 target = hitClickable.transform.position;
            Vector3 charaPos = chara.transform.position;
            Vector3 direction = target - charaPos;

            // to prevent message in unity saying "Look rotation viewing vector is zero"
            if (direction != Vector3.zero)  
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                chara.transform.rotation = rotation;
            }
            if (hitClickable is Enemy)
            {
                chara.AttackAnimation();
            }
            hitClickable.InteractWith(chara);
        }
        private void MoveCharacter(RaycastHit hitInfo, Character chara)
        {
            MoveCharacter(hitInfo.point, chara);
        }
        private void MoveCharacter(Vector3 destination, Character chara)
        {
            if (chara.IsControlable)
            {
                chara.NavigatePosition(destination);
            }
            else
                charaSelections.DeSelect(chara);
        }
    }
}
