using UnityEngine;
using UnityEngine.InputSystem;

namespace MySampleEx
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerInputAction : MonoBehaviour
    {
        #region Variables
        public Vector3 Move {  get; private set; }
        public bool Jump {  get; set; }         //점프 입력값
        #endregion

        #region NewInput SendMessage
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }
        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }
        public void MoveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }
        public void JumpInput(bool newJumpState)
        {
            Jump = newJumpState;
        }
        #endregion
    }
}
