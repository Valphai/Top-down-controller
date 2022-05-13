using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace TopDownController.Controller
{
    public abstract class Character : CanInteract, IPointerClickHandler
    {
        public Queue<Action> MoveOrderQueue;
        public float InteractionRange = 3f;

        [SerializeField] private bool isControlable;
        private List<Collider> ragdollParts;
        private CharacterSelections charaSelections;
        protected Animator anim;
        protected NavMeshAgent agent;
        public bool PathCompleted 
        {
            get 
            { 
                return agent.hasPath && 
                    agent.remainingDistance < InteractionRange;
            }
        }

        public virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();
            MoveOrderQueue = new Queue<Action>();
            ragdollParts = new List<Collider>();
            GetRagdollParts();
        }

        public virtual void OnEnable()
        {
            charaSelections = 
                GameObject.FindGameObjectWithTag("CharaSelections").GetComponent<CharacterSelections>();
            charaSelections.CharaList.Add(this);
        }
        public virtual void OnDisable()	
        {
            charaSelections.RemoveFromCharaList(this);
        }

        public virtual void MoveAnimation()
        {
            float velocityNormalized = agent.velocity.magnitude / agent.speed;
            anim.SetFloat("speed", velocityNormalized); 
        }
        public virtual void AttackAnimation()
        {
            anim.SetTrigger("attack");
        }
        public virtual void DieAnimation()
        {
            anim.SetBool("dead", true);
        }
        public void FollowTheQueue()
        {
            if (MoveOrderQueue.Count <= 0) return;

            Action command = MoveOrderQueue.Dequeue();
            command?.Invoke();
        }
        public void ResetQueue()
        {
            MoveOrderQueue.Clear();
            agent.ResetPath();
        }
        public void NavigatePosition(Vector3 point)
        {
            if (isControlable)
            {
                agent.destination = point;
            }
            else
                charaSelections.DeSelect(this);
        }
        public virtual void Die()
        {
            DeSelect();
            ResetQueue();
            charaSelections.RemoveFromCharaList(this);
            agent.enabled = false;
            if (ragdollParts.Count > 0) 
            {
                TurnOnRagdoll();
            }
            else 
            {
                DieAnimation();
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount >= 1) 
            {
                charaSelections.LockTransform(transform);
            }
        }
        private void OnMouseExit()	
        {
            if (
                !charaSelections.CharaSelected.Contains(this)
            )
            {
                DeSelect();
            }
        }
        private void TurnOnRagdoll()
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            anim.enabled = false;
            anim.avatar = null;

            foreach (Collider c in ragdollParts)
            {
                c.isTrigger = false;
                c.attachedRigidbody.useGravity = true;
            }
        }
        private void GetRagdollParts()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                if (c.gameObject != gameObject)
                {
                    c.attachedRigidbody.useGravity = false;
                    c.isTrigger = true;
                    ragdollParts.Add(c);
                }
            }
        }
    }
}
