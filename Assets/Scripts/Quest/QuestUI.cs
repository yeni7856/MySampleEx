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
        private Quest quest;                //퀘스트 정보창에 보이는 퀘스트

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
        public Action OnCloseQuest;

        void SetQuestUI(Quest _quest)
        {
            quest = _quest;
            nameText.text = quest.name;
            descriptionText.text = quest.description;

            goalText.text = quest.questGoal.currentAmount.ToString() + " / " + quest.questGoal.goalAmount.ToString();
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
            switch (quest.questState)
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

        public void OpenQuestUI()
        {
           if(QuestManager.Instance.currentQuest == null)
            {
                //
                return;
            }
            SetQuestUI(QuestManager.Instance.currentQuest);         //정보창 세팅
        }
        public void CloseQuestUI()
        {
            UIManager.Instance.CloseQuestUI();
        }
    }
}