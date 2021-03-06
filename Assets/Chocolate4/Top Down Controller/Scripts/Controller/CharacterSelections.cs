using System.Collections.Generic;
using UnityEngine;

namespace TopDownController.Controller
{
    public class CharacterSelections : MonoBehaviour
    {
        private static CharacterSelections _instance;
        public static CharacterSelections Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<CharacterSelections>();
                }

                return _instance;
            }
        }
        public List<Character> CharaList = new List<Character>();
        public List<Character> CharaSelected = new List<Character>();

        [SerializeField]
        private RectTransform boxVisual;
        private Camera cam;
        private CameraMovement camMovement;
        private Rect selectionBox;
        private Vector2 startPos, endPos;

        private void Awake()	
        {
            cam = Camera.main;
            camMovement = cam.GetComponentInParent<CameraMovement>();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            DrawBoxVisual();
        }
        private void Update()
        {
            // click
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                selectionBox = new Rect();
            }
            // dragging
            if (Input.GetMouseButton(0))
            {
                endPos = Input.mousePosition;
                DrawBoxVisual();
                DrawSelection();
            }
            // release
            if (Input.GetMouseButtonUp(0))
            {
                SelectCharas();
                startPos = Vector2.zero;
                endPos = Vector2.zero;
                DrawBoxVisual();
            }

            UpdateUnitQueue();
            UpdateAnimations();
        }
        public void ClickSelect(Character chara)
        {
            DeselectAll();
            if (CharaList.Contains(chara))
            {
                Select(chara);
            }
        }
        public void ShiftClickSelect(Character chara)
        {
            if(!CharaSelected.Contains(chara))
            {
                Select(chara);
            }
            else
            {
                DeSelect(chara);
            }
        }
        public void RemoveFromCharaList(Character chara)
        {
            if(CharaList.Contains(chara))
            {
                CharaList.Remove(chara);
            }
            if(CharaSelected.Contains(chara))
            {
                DeSelect(chara);
            }
        }
        public void DeselectAll()
        {
            foreach (var chara in CharaSelected)
            {
                chara.DeSelect();
            }
            CharaSelected.Clear();
        }
        public void LockTransform()
        {
            if (CharaSelected.Count == 1)
            {
                camMovement.LockTransform(CharaSelected[0].transform);
            }
        }
        public void DeSelect(Character chara)
        {
            chara.DeSelect();
            CharaSelected.Remove(chara);
        }
        private void Select(Character chara)
        {
            chara.Select();
            CharaSelected.Add(chara);
        }
        private void DragSelect(Character chara)
        {
            if (!CharaSelected.Contains(chara))
            {
                Select(chara);
            }
        }
        private void DrawBoxVisual()
        {
            Vector2 boxStart = startPos;
            Vector2 boxEnd = endPos;

            Vector2 boxCenter = (boxStart + boxEnd) / 2;
            boxVisual.position = boxCenter;

            Vector2 boxSize = new Vector2(
                Mathf.Abs(boxStart.x - boxEnd.x),
                Mathf.Abs(boxStart.y - boxEnd.y)
            );

            boxVisual.sizeDelta = boxSize;
        }
        private void DrawSelection()
        {
            // left
            if (Input.mousePosition.x < startPos.x)
            {
                selectionBox.xMin = Input.mousePosition.x;
                selectionBox.xMax = startPos.x;
            }
            // right
            else
            {
                selectionBox.xMin = startPos.x;
                selectionBox.xMax = Input.mousePosition.x;
            }
            // down
            if (Input.mousePosition.y < startPos.y)
            {
                selectionBox.yMin = Input.mousePosition.y;
                selectionBox.yMax = startPos.y;
            }
            // up
            else
            {
                selectionBox.yMin = startPos.y;
                selectionBox.yMax = Input.mousePosition.y;
            }
        }
        private void SelectCharas()
        {
            foreach (Character chara in CharaList)
            {
                if (selectionBox.Contains(cam.WorldToScreenPoint(chara.transform.position)))
                {
                    DragSelect(chara);
                }
            }
        }
        private void UpdateAnimations()
        {
            foreach (Character chara in CharaList)
            {
                chara.MoveAnimation();
            }
        }
        private void UpdateUnitQueue()
        {
            foreach (Character chara in CharaList)
            {
                if (chara.PathCompleted)
                {
                    chara.FollowTheQueue();
                }
                else if (chara.MoveOrderQueue.Count == 1 && chara.InteractableTarget)
                {
                    Vector3 dst = 
                        (chara.InteractableTarget.transform.position - chara.transform.position)
                        * chara.InteractionRange;
                    chara.NavigatePosition(dst);
                }
            }
        }
    }
}
