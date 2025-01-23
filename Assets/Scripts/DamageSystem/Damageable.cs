using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MySampleEx
{
    /// <summary>
    /// Damage 기능 구현 클래스 , partial 두개 클레스 로 나\눈다
    /// </summary>
    public partial class Damageable : MonoBehaviour
    {
        #region Variables
        public int maxHitPoint;             //MaxHeath
        public float invulnerablityTime = 0f;    //데미지 후 무적 타임

        //데미지 입는 방향
        [Range(0.0f, 360f)]     //렌덤
        public float hitAngle = 360.0f;     //
        [Range(0.0f, 360f)]
        public float hitForwardRotation = 360f; 

       public bool IsInvulnerable {  get;  set; }    //무적 여부
        public int CurrentHitPoints { get;  set; }   //Current Health

        public List<MonoBehaviour> onDamageMessageReceviers;

        protected float m_timeSinceLasHit = 0.0f;       //무적 타이머 카운트 다운

        public UnityAction OnDeath;
        public UnityAction OnReceiveDamage;
        public UnityAction OnHitWhileVulnerable;
        public UnityAction OnBecomeVulnerable;
        public UnityAction OnResetDamage;

        protected Collider m_Collider;
        public System.Action scheule;       //등록된 함수를 LateUpdate를 호출해서 
        #endregion

        private void Start()
        {
            //참조
            m_Collider = GetComponent<Collider>();

            //초기화
            ResetDamage();
        }

        private void Update()
        {
            //무적 타이머
            if(IsInvulnerable)
            {
                m_timeSinceLasHit += Time.deltaTime;
                if(m_timeSinceLasHit > invulnerablityTime)
                {
                    IsInvulnerable = false;
                    OnBecomeVulnerable?.Invoke(); 

                    m_timeSinceLasHit = 0;
                }
            }
        }

        public void LateUpdate()
        {
            if(scheule != null)
            {
                scheule();
                scheule = null;
            }
        }
        //충돌체 활성화 비활성
        public void SetColliderState(bool enabled)
        {
            m_Collider.enabled = enabled;   
        }
        //데미지 데이터 초기화
        public void ResetDamage()
        {
            CurrentHitPoints = maxHitPoint;
            IsInvulnerable = false;
            m_timeSinceLasHit = 0.0f;
            OnResetDamage?.Invoke();    //널이아니면 호출안함
        }
        //데미지 입기
        public void TakeDamage(DamageMessage data)
        {
            //이미 죽으면 더이상 데미지 입지 않는다.
            if(CurrentHitPoints <= 0)
                return;
            Debug.Log(CurrentHitPoints.ToString());
            //무적이면
            if(IsInvulnerable)
            {
                OnHitWhileVulnerable.Invoke();  //무적인상태표시
                return;
            }
            //Hit 방향 구하기
            Vector3 forward = transform.forward;
            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            Vector3 positionToDamager = data.dmgSource - transform.position; 
            positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

            if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.05f)
                return;

            //데이터 처리
            IsInvulnerable = true;
            CurrentHitPoints -= data.amount;
            if(CurrentHitPoints <= 0)
            {
                if(OnDeath != null)
                {
                    scheule += OnDeath.Invoke;      //조금 늦게 딜레이 Death,
                }
            }
            else
            {
                OnReceiveDamage?.Invoke();
            }
            //데미지 메세지 보내기
            var messageType = CurrentHitPoints <= 0 ? MessageType.Death : MessageType.Damaged;
            for(int i = 0; i < onDamageMessageReceviers.Count; i++)
            {
                var reciver = onDamageMessageReceviers[i] as IMessageReceiver;
                reciver.OnReceiveMessage(messageType, this, data);
            }
        }

#if  UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 forward = transform.forward;
            forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

            if(Event.current.type == EventType.Repaint)
            {
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.ArrowHandleCap(0, transform.position, 
                    Quaternion.LookRotation(forward), 1.0f, EventType.Repaint);
            }

            UnityEditor.Handles.color = new Color(1.0f, 0.0f, 0.5f);
            forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
        }
#endif
    }
}