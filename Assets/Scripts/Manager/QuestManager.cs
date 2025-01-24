using System;
using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 플레이어 퀘스트 관리 클래스
    /// </summary>
    public class QuestManager : Singleton<QuestManager>
    {
        #region Variables
        public List<QuestObject> playerQuests;      //플레이어가 현재 진행중인 퀘스트 리스트
        public QuestObject currentQuest;              //퀘스트 UI에 전달되는 퀘스트

        public Action<QuestObject> OnAcceptQuest;
        public Action<QuestObject> OnGiveUpQuest;
        public Action<QuestObject> OnCompletQuest;

        public StatsObject playerStats;
        #endregion

        private void Start()
        {
            playerQuests = new List<QuestObject>(); 
        }
        //퀘스트UI에 현재 선택된 퀘스트 셋팅
        public void SetCurrentQuest(QuestObject quest)
        {
            currentQuest = quest;
        }
        //
        public void AddPlayerQuest()
        {
            if(currentQuest == null)
                return;

            OnAcceptQuest?.Invoke(currentQuest);

            Quest quest = DataManager.GetQuestData().Quests.quests[currentQuest.number];
            QuestObject newQuest = new QuestObject(quest);
            newQuest.questState = QuestState.Accept;

            playerQuests.Add(newQuest);

        }

        public void GiveupPlayerQuest()
        {
            if (currentQuest == null)
                return;
            OnGiveUpQuest?.Invoke(currentQuest);
            playerQuests.Remove(currentQuest);
        }

        public void UpdateQuest(QuestType questType, int goalIndex)
        {
            switch (questType)
            {
                case QuestType.Kill:
                    foreach (var quest in playerQuests)
                    {
                        quest.EnemyKill(goalIndex);
                        if (quest.questGoal.IsReached)
                        {
                            quest.questState = QuestState.Complete;
                            OnCompletQuest?.Invoke(quest);
                        }
                    }
                    break;
                case QuestType.Collect:
                    foreach (var quest in playerQuests)
                    {
                        quest.ItemCollect(goalIndex);
                        if (quest.questGoal.IsReached)
                        {
                            quest.questState = QuestState.Complete;
                            OnCompletQuest?.Invoke(quest);
                        }
                    }
                    break;
            }
        }
        public void RewardQuest()
        {
            //
            Debug.Log("퀘스트 보상 받았습니다");
            Quest quest = DataManager.GetQuestData().Quests.quests[currentQuest.number];
            playerStats.AddGold(quest.rewardGold);

            if (playerStats.Addexp(quest.rewardExp))
            {
                //vfx, sfx
                Debug.Log("퀘스트 보상 받았습니다");
            }

            //플레이어 퀘스트 리스트에서도 삭제
            RemovePlayerQuest(currentQuest);
        }

        private void RemovePlayerQuest(QuestObject questObject)
        {
            foreach (var quest in playerQuests)
            {
                if (quest.number == questObject.number)
                {
                    playerQuests.Remove(quest);
                    break;
                }
            }
        }
    }

}
