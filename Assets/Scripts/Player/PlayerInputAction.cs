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
        #endregion

        #region NewInput SendMessage
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }
        #endregion
        public void MoveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }
    }
}
