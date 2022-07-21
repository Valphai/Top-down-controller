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
        protected event Action queueReset;
        private bool DoOnce;

        public bool PathCompleted 
        {
            get 
            {
                if (!DoOnce)
                {
                    // fires twice on update
                    if (agent.hasPath && 
                    agent.remainingDistance < InteractionRange)
                    {
                        DoOnce = true;
                        return true;
                    }
                }
                DoOnce = false;
                return false;
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
            queueReset?.Invoke();
            MoveOrderQueue.Clear();
            agent.ResetPath();
        }
        public void AddCommand(Action command)
        {
            MoveOrderQueue.Enqueue(command);
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
        public void RotateTowards(Vector3 target)
        {
            Vector3 charaPos = transform.position;
            Vector3 direction = target - charaPos;

            // to prevent message in unity saying "Look rotation viewing vector is zero"
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
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
        public override void OnMouseExit()	
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
