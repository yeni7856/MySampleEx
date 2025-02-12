using System.Collections;
using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 발사체를 관리하는 클래스 : 풀 해제 기능
    /// </summary>
    [RequireComponent(typeof(PooledObject))]
    public class ProjectTile : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float timeoutDelay = 3f;           //지연 시간 이후 해제

        private PooledObject pooledObject;
        #endregion

        private void Awake()
        {
            //참조
            pooledObject = GetComponent<PooledObject>();
        }

        //오브젝트 생성 후 바로 호출되는 함수
        public void Deactivate()
        {
            StartCoroutine(DeactivateRoutin(timeoutDelay));
        }
        IEnumerator DeactivateRoutin(float dealy)
        {
            yield return new WaitForSeconds(dealy);

            //rigidbody 이동 값 리셋
            Rigidbody rigidbody = GetComponent<Rigidbody>();   
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;   
            
            pooledObject.Release();
            this.gameObject.SetActive(false); 

        }
    }
}