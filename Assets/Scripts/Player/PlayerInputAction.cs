using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
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

        public bool Attack { get; private set; }         //공격 입력값

        public Coroutine m_AttackCoroutine;    //어텍 코루틴
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
        public void OnAttack(InputValue value)
        {
            AttackInput(value.isPressed);
        }
        public void MoveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }
        public void JumpInput(bool newJumpState)
        {
            Jump = newJumpState;
        }

         public void AttackInput(bool newAttackState)
        {
            Attack = newAttackState;

            if(m_AttackCoroutine != null)
            {
                StopCoroutine(m_AttackCoroutine);
            }
            m_AttackCoroutine = StartCoroutine(AttackWait());
        }

        IEnumerator AttackWait()
        {
            yield return new WaitForSeconds(0.03f);
            Attack = false; 
        }
        #endregion
    }
}
