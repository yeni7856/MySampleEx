using System.Collections.Generic;
using System;
using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 퀘스트를 가진 npc
    /// </summary>
    public class PickupQuestGiver : PickupNpc
    {
        #region Variables
        public List<Quest> quests;
        #endregion

        protected override void Start()
        {
            base.Start();   
            quests = GetNpcQuest(npc.number);
        }

        //npc 의 인덱스에 지정된 퀘스트 목록 가져오기
        public List<Quest> GetNpcQuest(int npcNumber)
        {
            List<Quest> questList = new List<Quest>();

            foreach(Quest quest in DataManager.GetQuestData().Quests.quests)
            {
                if(quest.npcNumber == npcNumber)
                {
                    Quest newQuest = new Quest();
                    newQuest.number = quest.number;
                    newQuest.npcNumber = quest.npcNumber;
                    newQuest.name = quest.name;
                    newQuest.description = quest.description;
                    newQuest.dialogIndex = quest.dialogIndex;
                    newQuest.level = quest.level;

                    newQuest.questGoal = new QuestGoal();
                    newQuest.questGoal.questType = quest.questType;
                    newQuest.questGoal.goalIndex = quest.goalIndex;
                    newQuest.questGoal.goalAmount = quest.goalAmount;
                    newQuest.questGoal.currentAmount = 0;

                    newQuest.rewardGold = quest.rewardGold;
                    newQuest.rewardExp = quest.rewardExp;
                    newQuest.rewardItem = quest.rewardItem;
                    
                    newQuest.questState = QuestState.Ready;

                    questList.Add(newQuest);
                }
            }

            return questList;
        }
        public override void DoAction()
        {
            if(quests.Count == 0)
            {
               Debug.Log("모든 퀘스트를 클리어");
                return;
            }
            QuestManager.Instance.currentQuest = quests[0];         //담았다가

            switch (quests[0].questState)
            {
                case QuestState.Ready:
                    UIManager.Instance.OpenDialogUI(quests[0].dialogIndex, npc.npcType);
                    break;
                case QuestState.Accept:
                    UIManager.Instance.OpenDialogUI(quests[0].dialogIndex + 1, npc.npcType);
                    break;
                case QuestState.Complete:
                    UIManager.Instance.OpenDialogUI(quests[0].dialogIndex + 2, npc.npcType);
                    break;
            }
        }
    }
}