using System.Collections.Generic;
using TopDownController.Commands;
using UnityEngine;

namespace TopDownController.Controller
{
    public class ClickInput : MonoBehaviour
    {
        public LayerMask Clickable;
        public LayerMask Ground;
        
        [Tooltip("use this key pressing on units to add them to selection")]
        public KeyCode SelectUnitsButton = KeyCode.LeftControl;

        [Tooltip("use this key to queue movement order of units")]
        public KeyCode QueueUnitsButton = KeyCode.LeftShift;
        
        [Tooltip("use this key to lock the camera onto the selected character")]
        public KeyCode LockUnitButton = KeyCode.Y;
        [SerializeField] private Marker marker;
        private CharacterSelections charaSelections;
        private Camera cam;
        private float lastExecution;

        private void Start()
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
            if (Input.GetKey(LockUnitButton))
            {
                charaSelections.LockTransform();

            }
        }
        public void InteractOnTheSpotUsing(Character chara)
        {
            QueueUnits(
                new ICommand[] {
                    new MoveCommand(),
                    new InteractCommand()
                }, 
                new Vector3[] { chara.transform.position },
                new List<Character>() { chara }
            );
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
            List<Character> charaSel = charaSelections.CharaSelected;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
            ICommand[] commands = null;
            Transform clicked = null;
            Vector3 point = Vector3.zero;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, Clickable))
            {
                clicked = hitInfo.transform;
                CanInteract toInteractWith = clicked.GetComponent<CanInteract>();
                point = hitInfo.point;

                for (int i = 0; i < charaSel.Count; i++)
                {
                    charaSel[i].InteractableTarget = toInteractWith;
                    Debug.Log(charaSel[i].InteractableTarget);
                }

                marker.ClickedOn(toInteractWith);

                commands = new ICommand[] {
                    new MoveCommand(),
                    new InteractCommand()
                };
            }
            else if (Physics.Raycast(ray, out RaycastHit hitInfo2, Mathf.Infinity, Ground))
            {
                clicked = hitInfo2.transform;
                point = hitInfo2.point;

                for (int i = 0; i < charaSel.Count; i++)
                {
                    charaSel[i].InteractableTarget = null;
                }

                marker.MoveTo(point);

                commands = new ICommand[] {
                    new MoveCommand(),
                };
            }

            if (charaSel.Count <= 0) return;

            Vector3[] points = 
                GetPositionsAround(
                    point, charaSel, clicked
                );
            QueueUnits(commands, points, charaSel);
        }

        private void QueueUnits(
            ICommand[] commands, Vector3[] targetPositions, List<Character> charaSel
        )
        {
            for (int i = 0; i < charaSel.Count; i++)
            {
                Character chara = charaSel[i];
                Vector3 goal = targetPositions[i];
                
                if (Input.GetKey(QueueUnitsButton))
                {
                    foreach (ICommand command in commands)
                    {
                        chara.AddCommand(() => command.Execute(goal, chara));
                    }
                }
                else
                {
                    chara.ResetQueue();
                    foreach (ICommand command in commands)
                    {
                        chara.AddCommand(() => command.Execute(goal, chara));
                    }
                    chara.FollowTheQueue();
                }
            }
        }
        private Vector3[] GetPositionsAround(
            Vector3 point, List<Character> selectedCharas, Transform clicked
        )
        {
            int charaCount = selectedCharas.Count;
            Vector3[] positions = new Vector3[charaCount];

            if (charaCount <= 1)
            {
                positions[0] = point;
                return positions;
            }

            for (int i = 0; i < charaCount; i++)
            {
                float angle = i * 360f / charaCount;
                positions[i] = point + ApplyRotation(angle) * selectedCharas[i].InteractionRange;
            }
            return positions;
        }

        private Vector3 ApplyRotation(float angle)
        {
            return Quaternion.Euler(0, angle, 0) * Vector3.right;
        }
    }
}
