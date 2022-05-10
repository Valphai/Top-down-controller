using System;
using System.Collections.Generic;
using System.Linq;
using TopDownController.Commands;
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

            UpdateUnitQueue();
        }
        private void UpdateUnitQueue()
        {
            foreach (Character chara in charaSelections.CharaList.ToArray())
            {
                if (chara.PathCompleted || 
                    (!Input.GetKey(QueueUnitsButton) && Input.GetMouseButtonDown(1))
                )
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
            ICommand[] commands = null;
            Transform hitClickable = null;
            Vector3 point = Vector3.zero;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Clickable))
            {
                hitClickable = hitInfo.transform;
                Character toInteractWith = hitClickable.GetComponent<Character>();
                point = hitInfo.point;

                commands = new ICommand[] {
                    new MoveCommand(),
                    new InteractCommand(toInteractWith)
                };
            }
            else if (Physics.Raycast(ray, out RaycastHit hitInfo2, Mathf.Infinity, Ground))
            {
                hitClickable = hitInfo2.transform;
                point = hitInfo2.point;

                Marker.transform.position = point;

                commands = new ICommand[] {
                    new MoveCommand(),
                };
            }
            Vector3[] points = 
                GetPositionsAround(
                    point, charaSelections.CharaSelected, hitClickable
                );
            QueueUnits(commands, points);
        }


        private void QueueUnits(ICommand[] commands, Vector3[] positions)
        {
            Character[] charaArray = charaSelections.CharaSelected.ToArray();
            for (int i = 0; i < charaArray.Length; i++)
            {
                Character chara = charaArray[i];
                Vector3 goal = positions[i];
                if (Input.GetKey(QueueUnitsButton))
                {
                    foreach (ICommand command in commands)
                    {
                        chara.MoveOrderQueue.Enqueue(() => command.Execute(goal, chara));
                    }
                }
                else
                {
                    chara.MoveOrderQueue.Clear();
                    foreach (ICommand command in commands)
                    {
                        chara.MoveOrderQueue.Enqueue(() => command.Execute(goal, chara));
                    }
                }
            }
        }
        private Vector3[] GetPositionsAround(
            Vector3 point, List<Character> selectedCharas, Transform clicked
        )
        {
            Vector3[] positions = new Vector3[selectedCharas.Count];

            for (int i = 0; i < selectedCharas.Count; i++)
            {
                Vector3 charaPos = selectedCharas[i].transform.position;
                Vector3 target = clicked.position;
                if (Vector3.Distance(target, charaPos) < selectedCharas[i].InteractionRange)
                {
                    positions[i] = charaPos;
                    continue;
                }

                float angle = i * 2 * Mathf.PI / selectedCharas.Count;
                positions[i] = point + ApplyRotation(angle) * selectedCharas[i].InteractionRange;
            }
            return positions;
        }

        private Vector3 ApplyRotation(float angle)
        {
            return Quaternion.Euler(0, 0, angle) * Vector3.right;
        }
    }
}
