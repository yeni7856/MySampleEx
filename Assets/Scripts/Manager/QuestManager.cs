using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 플레이어 퀘스트 관리 클래스
    /// </summary>
    public class QuestManager : Singleton<QuestManager>
    {
        #region Variables
        public Quest currentQuest;              //퀘스트 UI에 전달되는 퀘스트
        #endregion
    }

}
