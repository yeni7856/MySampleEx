using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MySampleEx
{

    /// <summary>
    /// 퀘스트 목록, 퀘스트 진행창, 퀘스트 정보창
    /// </summary>
    public class QuestUI : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;

        public TextMeshProUGUI goalText;
        public TextMeshProUGUI rewardgoldText;
        public TextMeshProUGUI rewardExpText;
        public TextMeshProUGUI rewardItemText;
        public Image itemImage;

        public GameObject acceptButton;
        public GameObject giveupButton;
        public GameObject okButton;

        //퀘스트창 진행 종료시 실행된 이벤트
        public Action OnCloseQuestUI;
        private QuestManager questManager;

        private void OnEnable()
        {
            if (questManager == null)
            {
                questManager = QuestManager.Instance;
            }
            OnCloseQuestUI = null;
        }

        void SetQuestUI(QuestObject _questobject)
        {
            Quest quest = DataManager.GetQuestData().Quests.quests[_questobject.number];
            nameText.text = quest.name;

            if (_questobject.questState == QuestState.Complete)
            {
                descriptionText.text = "Quest Completed";
            }
            else
            {
                descriptionText.text = quest.description;
            }

            goalText.text = _questobject.questGoal.currentAmount.ToString() + " / " + _questobject.questGoal.goalAmount.ToString();
            rewardgoldText.text = quest.rewardGold.ToString();
            rewardExpText.text = quest.rewardExp.ToString();
            
            if(quest.rewardItem >= 0)
            {
                rewardExpText.text = UIManager.Instance.database.itemObjects[quest.rewardItem].name;
                itemImage.sprite = UIManager.Instance.database.itemObjects[quest.rewardItem].icon;
                itemImage.enabled = true;
            }
            else
            {
                rewardItemText.text = string.Empty;
                itemImage.sprite = null;
                itemImage.enabled = false;
            }

            //버튼 세팅
            switch (_questobject.questState)
            {
                case QuestState.Ready:
                    acceptButton.SetActive(true);
                    break;
                case QuestState.Accept:
                    giveupButton.SetActive(true);
                    break;
                case QuestState.Complete:
                    okButton.SetActive(true);
                    break;
            }
        }

        void ResetButtons()
        {
            acceptButton.SetActive(false);
            giveupButton.SetActive(false);
            okButton.SetActive(false);
        }

        //플레이어 퀘스트 보기
        public void OpenPlayerQuestUI(Action closeMethod)
        {
            if (closeMethod != null)
                OnCloseQuestUI += closeMethod;
            if (questManager.playerQuests.Count <= 0)
            {
                CloseQuestUI();
                return;
            }
            questManager.SetCurrentQuest(questManager.playerQuests[0]);
            SetQuestUI(questManager.currentQuest);
        }

        //NPC 퀘스트 보기
        public void OpenQuestUI(Action closeMethod)
        {
            if(closeMethod != null)
            {
                OnCloseQuestUI += closeMethod;
            }

           if (questManager.currentQuest == null)
            {
                CloseQuestUI();
                return;
            }

            SetQuestUI(questManager.currentQuest);       //정보창 세팅
        }

        public void CloseQuestUI()
        {
            ResetButtons();
            OnCloseQuestUI?.Invoke();
        }

        public void AcceptQuest()
        {
            //플레이어에게 퀘스트리스트에 currentQuest 추가
            questManager.AddPlayerQuest();

            CloseQuestUI();
        }

        public void GiveupQuest()
        {
            //플레이어에게 퀘스트리스트에 currentQuest 제거
            questManager.GiveupPlayerQuest();

            CloseQuestUI();
        }

        
    }
}