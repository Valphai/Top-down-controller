using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace TopDownController
{
    public abstract class Character : MonoBehaviour, IPointerClickHandler
    {
        public Queue<Vector3> MoveOrderQueue;
        public bool IsControllable; // can click on
        [HideInInspector] public Outline Outline;
        [HideInInspector] public Animator Anim;
        public float InteractionRange = 3f;
        private CharacterSelections charaSelections;
        protected NavMeshAgent agent;
        public bool PathCompleted 
        { 
            get 
            { return agent.pathStatus == NavMeshPathStatus.PathComplete && 
                        agent.remainingDistance == 0; }
            set { return; }
        }
        public float VelocityNormalized
        {
            get { return agent.velocity.magnitude/agent.speed; }
            set { return; }
        }
        
        private bool isMoveable;

        public bool IsMoveable
        {
            get
            {
                return IsControllable ? isMoveable : false;
            }
            set 
            {
                if (IsControllable)
                    isMoveable = value;
                else
                    isMoveable = false;
            }
        }
        public abstract void Interact(Character chara);
        private void Awake()	
        {
            agent = GetComponent<NavMeshAgent>();
            Anim = GetComponentInChildren<Animator>();
            MoveOrderQueue = new Queue<Vector3>();
        }
        private void OnEnable()
        {
            charaSelections = 
                GameObject.FindGameObjectWithTag("CharaSelections").GetComponent<CharacterSelections>();
            charaSelections.CharaList.Add(this);
            Outline = GetComponent<Outline>();
        }
        public void FollowTheQueue()
        {
            if (MoveOrderQueue.Count <= 0) return;

            Vector3 point = MoveOrderQueue.Dequeue();
            NavigatePosition(point);
        }
        public void NavigatePosition(Vector3 point)
        {
            agent.destination = point;
        }
        private void OnDestroy()	
        {
            charaSelections.CharaList.Remove(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount >= 1) 
            {
                charaSelections.LockCharacter(this);
            }
        }
    }
}
