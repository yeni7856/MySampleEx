using UnityEngine;


namespace MySampleEx
{
    /// <summary>
    /// 플레이어 캐릭터 제어(이동, 애니메이션, 액션)
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        public float maxForwardSpeed = 8f;              //플레이어 최고 이동 속도
        public float minTurnSpeed = 400f;                //플레이어 최저 회전 속도
        public float maxTurnSpeed = 1200f;              //플레이어 최고 회전 속도

        protected PlayerInputAction m_Input;
        protected CharacterController m_CharCtr; 
        protected Animator m_Animator;

        //이동,회전
        protected bool m_IsGrounded = true;
        protected float m_DesiredForwardSpeed;      //플레이어 입력값에 따라 낼수 있는 최고 이동 속도
        protected float m_ForwardSpeed;                 //플레이어 현재 이동 속도
        protected Quaternion m_TragetRotation;      //타겟을 향한 회전값
        protected float m_AngleDiff;                           //플레이어의 회전값과 타겟의 회전값의 차이 각도

        //이동 입력값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Input.Move.sqrMagnitude, 0f); }
        }

        //상수
        const float k_GroundAccelation = 20f;   //이동 가속도 값
        const float k_GroundDeceleration = 25f; //이동 감속도 값
        const float k_GroundRayDistacne = 1f;   //

        //애니메이션 상태
        protected bool m_IsAnimatorTransitioning;           //상태 전환 중이냐?
        protected AnimatorStateInfo m_CurrentStateInfo; //현재 애니 상태
        protected AnimatorStateInfo m_NextStateInfo;       //다음 애니 상태 정보
        protected AnimatorStateInfo m_PreviousStateInfo;    //현재 애니 상태 정보 저장
        protected AnimatorStateInfo m_PreviousNxetStateInfo;    //다음 애니 상태 정보 저장
        protected bool m_PreviousIsAnimatorTransitioning;               //상태전환중이냐?

        //애니메이션 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAngleDeltaRad = Animator.StringToHash("AngleDeltaRad");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");

        //애니메이션 상태 해시값
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        #endregion

        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInputAction>();
            m_CharCtr = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();
        }
        private void FixedUpdate()
        {
            CacheAnimatorState();
            CalculateForwardMovement();
            SetTargetRotation();
            if(IsOrientationUpdate() && IsMoveInput)
            {
                UpdateOrientation();
            }

            TimeoutToIdle();
        }
        //애니메이션 상태값 구하기
        void CacheAnimatorState()
        {
            m_PreviousStateInfo = m_CurrentStateInfo;
            m_PreviousNxetStateInfo = m_NextStateInfo;
            m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

            m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_IsAnimatorTransitioning = m_Animator.IsInTransition(0);
        }

        //업데이트
        bool IsOrientationUpdate()
        {
            bool updateOrientationForLocomotion = (!m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLocomotion
                || m_NextStateInfo.shortNameHash == m_HashLocomotion);

            return updateOrientationForLocomotion;
        }

        //(Forward)이동속도 계산
        void CalculateForwardMovement()
        {
            Vector2 moveInput = m_Input.Move;
            if (moveInput.sqrMagnitude > 1f)        //sqrMagnitude 크기비교
            {
                moveInput.Normalize();      //단위 백터로 
            }
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            //입력에 따라서 가속, 감속, 결정 //입력이 들어오지않으면 스피드 멈추기
            float accceleration = IsMoveInput ? k_GroundAccelation : k_GroundDeceleration;

            //현재 이동속도를 구한다  //가고싶은 속도?
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed,
                accceleration * Time.deltaTime);

            //애니 입력값 설정 
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        //이동 방향 계산 
        void SetTargetRotation()
        {
            Vector2 moveInput = m_Input.Move;
            Vector3 localMovementDirection = new Vector3(moveInput.x,0f,moveInput.y).normalized;    //인풋에 따른 3d 방향 계산

            //Camera forward 구하기
            Vector3 forward = Vector3.forward;      //글로벌에 forward 방향 고정
            Quaternion targetRotation;
            if(Mathf.Approximately(Vector3.Dot(localMovementDirection,forward), -1.0f))   //반대방향
            {
                targetRotation = Quaternion.LookRotation(-forward); //앞방향에 반대방향 
            }
            else
            {
                Quaternion cameraToInputOffest = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
                targetRotation = Quaternion.LookRotation(cameraToInputOffest * forward);
            }
            //바라볼 앞방향
            Vector3 resultingForward = targetRotation * Vector3.forward;

            //회전하기위해 각도값 //탄젠트값
            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            
            //타겟에 대한 앵글
            float angleTarget = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, angleTarget);

            m_TragetRotation = targetRotation;
        }

        void UpdateOrientation()
        {
            //애니 입력값 설정 
            m_Animator.SetFloat(m_HashAngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);

            //회전 구현
            Vector3 localInput = new Vector3(m_Input.Move.x, 0f, m_Input.Move.y);
            float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed/m_DesiredForwardSpeed);
            
            //공중회전속도, 그라운드회전 속도
            float acutalTurnSpeed = groundedTurnSpeed;
            m_TragetRotation = Quaternion.RotateTowards(transform.rotation,m_TragetRotation, acutalTurnSpeed * Time.deltaTime);
            
            transform.rotation = m_TragetRotation;  
        }

        //시간이 경과되면 아이들 상태로 보낸다
        void TimeoutToIdle()
        {
            //입력값 (이동)
            bool inputDetected = IsMoveInput;

            //애니 입력값 설정
            m_Animator.SetBool(m_HashInputDetected,inputDetected);
        }

        private void OnAnimatorMove()
        {
            Vector3 movement;

            if(m_IsGrounded)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position + Vector3.up * k_GroundRayDistacne * 0.5f, -Vector3.up);
                if(Physics.Raycast(ray,out hit, k_GroundRayDistacne, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    movement = Vector3.ProjectOnPlane(m_Animator.deltaPosition, hit.normal);
                }
                else
                {
                    movement = m_Animator.deltaPosition;
                }
            }
            else
            {
                movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
            }

            //애니메이션에 의한 회전값 설정
            m_CharCtr.transform.rotation *= m_Animator.deltaRotation;
            //애니메이션에 의한 이동
            m_CharCtr.Move(movement);
        }
    }
}
