using UnityEngine;

namespace MySampleEx
{
    public class AimAtMouse : MonoBehaviour
    {
        #region Variables
        [SerializeField] private MouseToWorldPosition m_MouseToWorldPosition;
        [SerializeField] private Transform panTransfrom;
        [SerializeField] private float PanSpeed = 5f;

        [SerializeField] private Vector3 m_Aimoffset;       //목표 위치 보정값
        private ScreenDeadZone m_ScreenDeadZone;
        #endregion

        private void Start()
        {
            //참조
            if(m_MouseToWorldPosition != null)
            {
                m_ScreenDeadZone = m_MouseToWorldPosition.ScreenDeadZone;
            }
        }
        private void Update()
        {
            if (m_MouseToWorldPosition == null)     //마우스 포지션 없거나
                return;
            if (m_ScreenDeadZone.IsMouseInDeadZone())   //데드존이 아니면
                return;

            RoatatePanTowards(m_MouseToWorldPosition.Position);
        }

        //타겟 위치를 바라보도록 회전
        private void RoatatePanTowards(Vector3 targetPosition)
        {
            Vector3 targetDirection = m_Aimoffset+ targetPosition - panTransfrom.position;
            targetDirection.y = 0f;

            //타겟direction을 바라보는 회전값 구하기
            Quaternion targetRotaion = Quaternion.LookRotation(targetDirection);
            panTransfrom.rotation = Quaternion.Slerp(panTransfrom.rotation, targetRotaion, PanSpeed * Time.deltaTime);
        }
    }
}