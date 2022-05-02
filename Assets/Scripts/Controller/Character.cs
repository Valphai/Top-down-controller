using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace TopDownController.Controller
{
    public abstract class Character : MonoBehaviour, IPointerClickHandler
    {
        public Queue<Action> MoveOrderQueue;
        public bool IsControlable;
        public float InteractionRange = .7f;
        private Animator anim;
        private Outline outline;
        private CharacterSelections charaSelections;
        private List<Collider> ragdollParts;
        protected NavMeshAgent agent;
        public bool PathCompleted 
        { 
            get 
            { return agent.pathStatus == NavMeshPathStatus.PathComplete && 
                        agent.remainingDistance == 0; }
            set { return; }
        }
        public abstract void InteractWith(Character chara);
        private void Awake()
        {
            outline = GetComponent<Outline>();
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();
            MoveOrderQueue = new Queue<Action>();
            GetRagdollParts();
        }

        private void OnEnable()
        {
            charaSelections = CharacterSelections.Instance;
            charaSelections.CharaList.Add(this);
        }
        private void OnDisable()	
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

            Action action = MoveOrderQueue.Dequeue();
            action?.Invoke();
        }
        public void NavigatePosition(Vector3 point)
        {
            agent.destination = point;
        }
        public virtual void Die()
        {
            charaSelections.RemoveFromCharaList(this);
            MoveOrderQueue.Clear();
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
                charaSelections.LockCharacter(this);
            }
        }
        public void Deselect()
        {
            if (outline)
            {
                outline.Remove();
            }
        }
        public void Select()
        {
            outline.Activate();
        }
        private void OnMouseExit()	
        {
            if (
                !charaSelections.CharaSelected.Contains(this)
            )
            {
                outline.Remove();
            }
        }
        private void TurnOnRagdoll()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            anim.enabled = false;
            anim.avatar = null;

            foreach (Collider c in ragdollParts)
            {
                c.isTrigger = false;
                // c.attachedRigidbody.velocity = Vector3.zero;
            }
        }
        private void GetRagdollParts()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                if (c.gameObject != gameObject)
                {
                    c.isTrigger = true;
                    ragdollParts.Add(c);
                }
            }
        }
    }
}
