using UnityEngine;

namespace MySampleEx
{
    public class DataManager : MonoBehaviour
    {
        private static EffectData effectData = null;

        private void Start()
        {
            if(effectData ==  null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }
        }
        //이펙트 데이터 가져오기
        public static EffectData GetEffectData()
        {
            if (effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }
            return effectData;
        }
    }

}
