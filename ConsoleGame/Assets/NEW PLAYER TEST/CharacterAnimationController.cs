using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEWPLAYER
{
    [RequireComponent(typeof(Character))]
    public class CharacterAnimationController : MonoBehaviour
    {
        public Animator animator;
        private Character character;

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
        }

        void Update()
        {
            if (animator == null) 
            {
                Debug.Log("No valid animator");
                return;
            }

            animator.SetFloat("Velocity", character.GetVelocity());
        }
    }
}