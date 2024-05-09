using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPerson
{
    public class PlayerControls : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move; // Movement input values
        public Vector2 look; // Camera look input values
        public bool sprint; // Sprinting input state
        public bool aim; // Aiming input state
        public bool crouch; // Crouching input state
        public bool interact; // Interacting input state
        public bool shoot; // Shooting input state
        public bool pause; // Pausing input state

        [Header("Movement Settings")]
        public bool analogMovement; // Enable analog movement

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true; // Whether cursor is locked
        public bool cursorInputForLook = true; // Whether cursor input affects camera look

        // Handle movement input
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        // Handle look input
        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        // Handle sprint input
        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        // Handle aim input
        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

        // Handle crouch input
        public void OnCrouch(InputValue value)
        {
            CrouchInput(value.isPressed);
        }

        // Handle interact input
        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        // Handle shoot input
        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        // Handle pause input
        public void OnPause(InputValue value)
        {
            PauseInput(value.isPressed);
        }

        // Update movement input
        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        // Update look input
        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        // Update sprint input
        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        // Update aim input
        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        // Update crouch input
        public void CrouchInput(bool newCrouchState)
        {
            // Makes crouch a toggle input instead of a hold input
            if (newCrouchState)
            {
                crouch = !crouch;
            }
        }

        // Update interact input
        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        // Update shoot input
        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }

        // Update pause input
        public void PauseInput(bool newPauseState)
        {
            pause = newPauseState;
        }

        // Handle cursor state on application focus
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        // Set cursor state based on lock status
        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}