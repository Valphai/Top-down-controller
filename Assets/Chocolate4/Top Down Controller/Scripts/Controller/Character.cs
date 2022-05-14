using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TopDownController.Controller
{
    public abstract class Character : CanInteract
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
        }
        public virtual void Start()
        {
            charaSelections = CharacterSelections.Instance;
            charaSelections.CharaList.Add(this);
        }

        public virtual void OnEnable()
        {
            MoveOrderQueue = new Queue<Action>();
            ResetQueue();
            ragdollParts = new List<Collider>();
            GetRagdollParts();

            charaSelections = CharacterSelections.Instance;
            if (charaSelections)
            {
                charaSelections.CharaList.Add(this);
            }
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
