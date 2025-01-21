using UnityEngine;

namespace MySampleEx
{
    public class DataManager : MonoBehaviour
    {
        private static EffectData effectData = null;
        private static DialogData dialogData = null;
        private static QuestData questData = null;

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

        //다이알로그 데이터 가져오기
        public static DialogData GetDialogData()
        {
            if (dialogData == null)
            {
                dialogData = ScriptableObject.CreateInstance<DialogData>();
                dialogData.LoadData();
            }
            return dialogData;
        }


        //퀘스트 데이터 가져오기
        public static QuestData GetQuestData()
        {
            if (questData == null)
            {
                questData = ScriptableObject.CreateInstance<QuestData>();
                questData.LoadData();
            }
            return questData;
        }
    }

}
