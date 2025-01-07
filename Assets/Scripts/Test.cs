using UnityEngine;

namespace MySampleEx
{
    public class Test : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            EffectManager.Instance.EffectOneShot((int)EffectList.NewData0,Vector3.zero);
        }
    }
}
