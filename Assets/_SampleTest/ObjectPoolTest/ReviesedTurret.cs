using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;


namespace MySampleEx
{
    public class ReviesedTurret : MonoBehaviour
    {
        #region Variables
        //풀에 들어가 오브젝트 프리팩
        [SerializeField] private ReviesedProjectTile projectPrefab;

        //fire
        [SerializeField] private float muzzleVelocity = 700f;           //발사 속도
        [SerializeField] private Transform muzzle;                         //발사 총구
        [SerializeField] private float cooldownTime = 0.1f;
        private float nextTimeToShoot;

        [SerializeField] private UnityEvent m_GunFire;                  //발사시 등록된 함수 호출

        //풀
        private IObjectPool<ReviesedProjectTile> objectPool;
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxSize = 100;
        [SerializeField] private bool collectionCheck = true;
        #endregion

        private void Awake()
        {
            //풀 셋팅 
            objectPool = new ObjectPool<ReviesedProjectTile>(CreateProjecttile,                    
                OnGetFromPool, OnReleaseToPool, OnDestoryPooledObject,
                collectionCheck, defaultCapacity, maxSize);
        }

        private ReviesedProjectTile CreateProjecttile()
        {
            ReviesedProjectTile projectTileInstance = Instantiate(projectPrefab);
            projectTileInstance.ObjectPool = objectPool;    
            return projectTileInstance;
        }

        private void OnGetFromPool(ReviesedProjectTile pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(ReviesedProjectTile pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }
        private void OnDestoryPooledObject(ReviesedProjectTile pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        private void FixedUpdate()
        {
            //마우스 좌클시 발사
            if(Input.GetMouseButton(0) && Time.time > nextTimeToShoot)
            {
                Shoot();
            }
        }
        void Shoot()
        {
            //풀에서 오브젝트 꺼내기
            ReviesedProjectTile bulletObject = objectPool.Get();
            if(bulletObject == null)
            {
                return;
            }
            bulletObject.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);
            Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(bulletObject.transform.forward * muzzleVelocity, ForceMode.Acceleration);
            }
            bulletObject.Deactivate();

            nextTimeToShoot = Time.time + cooldownTime; //연사 방지

            m_GunFire?.Invoke();
        }
    }
}