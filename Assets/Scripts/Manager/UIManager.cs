using UnityEngine;

namespace MySampleEx
{
    public class UIManager : Singleton<UIManager>
    {
        #region Variables
        public ItemDataBaseSO database;

        public DynamicInventoryUI palyerInventoryUI;
        public StaticInventoryUI palyerEquipmentUI;
        public PlayerStatsUI playerStatsUI;
        public DialogUI dialogUI;
        public QuestUI questUI;

        public int itemId = 0;
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Toggle(palyerInventoryUI.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Toggle(palyerEquipmentUI.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                Toggle(playerStatsUI.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                AddNewItem();
            }
        }

        public void Toggle(GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }

        public void AddNewItem()
        {
            ItemObject itemObject = database.itemObjects[itemId];
            Item newItem = itemObject.CreateItme();

            palyerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }

        public void OpenDialogUI(int dialogIndex, NpcType npcType)
        {
            Toggle(dialogUI.gameObject);
            dialogUI.OnCloseDialog += CloseDialogUI;        //다이알로그 종료시 
            if (npcType == NpcType.QuestGiver)
            {
                //quest UI 열기
                dialogUI.OnCloseDialog += OpenQuestUI;
            }
            dialogUI.StartDialog(dialogIndex);
        }

        public void CloseDialogUI()
        {
            Toggle(dialogUI.gameObject);
        }
        public void OpenQuestUI()           //퀘스트창 열기
        {
            Toggle(questUI.gameObject);
            questUI.OnCloseQuest += CloseQuestUI;
            questUI.OpenQuestUI();
        }
        public void CloseQuestUI()           //퀘스트창 닫기
        {
            Toggle(questUI.gameObject);
        }
    }
}