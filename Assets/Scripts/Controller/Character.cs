using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace TopDownController
{
    public abstract class Character : MonoBehaviour, IPointerClickHandler
    {
        public Queue<Action> MoveOrderQueue;
        public bool IsControllable; // can click on
        [HideInInspector] public Outline Outline;
        [HideInInspector] public Animator Anim;
        public float InteractionRange = .7f;
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
            MoveOrderQueue = new Queue<Action>();
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

            Action action = MoveOrderQueue.Dequeue();
            action?.Invoke();
        }
        public void NavigatePosition(Vector3 point)
        {
            agent.destination = point;
        }
        public void AttackAnimation()
        {
            Anim.SetTrigger("attack");
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
        private void OnMouseOver()	
        {
            Outline.Activate();
        }
        private void OnMouseExit()	
        {
            if (!charaSelections.CharaSelected.Contains(this))
            {
                Outline.Remove();
            }
        }
    }
}
