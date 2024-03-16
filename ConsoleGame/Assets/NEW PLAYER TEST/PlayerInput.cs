using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEWPLAYER
{
    [RequireComponent(typeof(Character))]
    public class PlayerInput : MonoBehaviour
    {
        private Character character;

        void Start()
        {
            character = GetComponent<Character>();
        }

        void Update()
        {
            character.AddMovementInput(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        }
    }
}