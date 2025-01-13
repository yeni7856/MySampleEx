using UnityEngine;
using System.Collections;

namespace MySampleEx
{
    /// <summary>
    /// 플레이어 공격 이펙트 애니메이션 플레이
    /// </summary>
    public class TimeEffect : MonoBehaviour
    {
        #region Variables
        public Light staffLight;
        private Animation m_Animation;
        #endregion

        private void Awake()
        {
            m_Animation = GetComponent<Animation>();    

            //초기화
            gameObject.SetActive(false); 
        }
        //이펙트 연출
        public void Activate()
        {
            gameObject.SetActive(true);
            staffLight.enabled = true;
            if(m_Animation)
            {
                m_Animation.Play(); 
            }
            //이펙트 초기화
        }
        IEnumerator DisableAtEndOfAnimation()
        {
            yield return new WaitForSeconds(m_Animation.clip.length);
        }
    }

}
