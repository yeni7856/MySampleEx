using System.Collections.Generic;
using UnityEngine;


namespace MySampleEx
{
    /// <summary>
    /// 오브젝트 풀 : 풀셋팅, 오브젝트 꺼내오기, 오브젝트 다시 넣기
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        #region Variables
        [SerializeField] private PooledObject objectToPool;
        [SerializeField] private Stack<PooledObject> stack = new Stack<PooledObject>();
        [SerializeField] private int initPoolSize;
        #endregion

        void Start ()
        {
            //풀셋팅
            SetupPool();
        }

        //풀 셋팅
        void SetupPool()
        {
            if (objectToPool == null)
            {
                return;         //셋팅할거없음
            }

            stack = new Stack<PooledObject>();
            //stack 넣을 오브젝트 객체 변수
            PooledObject instance = null;
            for(int i = 0; i < initPoolSize; i++)
            {
                instance = Instantiate(objectToPool);            //생성한 풀 오브젝트 
                instance.Pool = this;                                       //생성한 풀 오브젝트에 풀 등록
                instance.gameObject.SetActive(false);           //생성한 풀 오브젝트 비활성
                stack.Push(instance);
            }
        }
        //오브젝트 꺼내오기
        public PooledObject GetPooledObject()
        {
            if(objectToPool == null)
            {
                return null;
            }

            //풀(스택)에 더이상 오브젝트가 없으면 새로 생성한다
            if(stack.Count == 0)
            {
                PooledObject newInstance = Instantiate(objectToPool);
                newInstance.Pool = this;
                newInstance.gameObject.SetActive(true);
                return newInstance;
            }

            PooledObject nextInstance = stack.Pop(); //스텍이라 우물
            nextInstance.gameObject.SetActive(true);
            return nextInstance;
        }
        //풀에 오브젝트 다시 넣기
        public void ReturnToPool(PooledObject pooledObject)
        {
            stack.Push(pooledObject); 
            pooledObject.gameObject.SetActive(false);
        }
    }
}