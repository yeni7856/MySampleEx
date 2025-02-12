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
        public ShopUI shopUI;

        public InventoryObject inventory;
        public StatsObject playerStats;

        public int itemId = 0;
        #endregion

        private void OnEnable()
        {
            palyerInventoryUI.OnUpdateSelectSlot += palyerEquipmentUI.UpdateSelectSlot;

            palyerEquipmentUI.OnUpdateSelectSlot += palyerInventoryUI.UpdateSelectSlot;
        }

        private void Update()
        {
#if !TOUCH_MODE
            if (Input.GetKeyDown(KeyCode.I))
            {
                OpenInventoryUI();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                OpenEquipMentUI();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                OpenPlayerStatsUI();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OpenPlayerQuestUI();
            }
            if(Input.GetKeyDown(KeyCode.Y))
            {
                OpenShopUI();
            }
#endif
        }

        public void Toggle(GameObject go)
        {
            go.SetActive(!go.activeSelf);
        }
        public void OpenInventoryUI()
        {
            palyerInventoryUI.UpdateSelectSlot(null);    
            Toggle(palyerInventoryUI.gameObject);
            if (palyerInventoryUI.gameObject.activeSelf)
            {
                palyerInventoryUI.OpenInventoryUI(CloseInventoryUI);
            }
        }

        public void CloseInventoryUI()
        {
            Toggle(palyerInventoryUI.gameObject);
        }

        public void OpenPlayerStatsUI()
        {
            Toggle(playerStatsUI.gameObject);
        }

        public void OpenEquipMentUI()
        {
            palyerEquipmentUI.UpdateSelectSlot(null);
            Toggle(palyerEquipmentUI.gameObject);
            if (palyerEquipmentUI.gameObject.activeSelf)
            {
                palyerEquipmentUI.OpenInventoryUI(CloseEquipMentUI);
            }
        }

        public void CloseEquipMentUI()
        {
            Toggle(palyerEquipmentUI.gameObject);
        }

        public void AddNewItem()
        {
            ItemObject itemObject = database.itemObjects[itemId];
            Item newItem = itemObject.CreateItem();

            palyerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }

        public void OpenDialogUI(int dialogIndex, NpcType npcType = NpcType.None)
        {
            Toggle(dialogUI.gameObject);
            dialogUI.OnCloseDialog += CloseDialogUI;        //다이알로그 종료시 
            if (npcType == NpcType.QuestGiver)
            {
                //quest UI 열기
                dialogUI.OnCloseDialog += OpenQuestUI;
            }
            else if(npcType == NpcType.Merchant)
            {
                //shop UI 열기
                dialogUI.OnCloseDialog += OpenShopUI;
            }
            dialogUI.StartDialog(dialogIndex);
        }

        public void CloseDialogUI()
        {
            Toggle(dialogUI.gameObject);
        }

        //플레이어 퀘스트 보기 (퀘스트... 리스트...)
        public void OpenPlayerQuestUI()
        {
            Toggle(questUI.gameObject);
            if (questUI.gameObject.activeSelf)
            {
                questUI.OpenPlayerQuestUI(CloseQuestUI);
            }
        }

        //NPC 퀘스트 보기 
        public void OpenQuestUI()           //퀘스트창 열기
        {
            Toggle(questUI.gameObject);
            questUI.OpenQuestUI(CloseQuestUI);
        }

        public void CloseQuestUI()           //퀘스트창 닫기
        {
            Toggle(questUI.gameObject);
        }
        public bool AddItemInventory(Item item, int amount)
        {
            return inventory.AddItem(item, amount); 
        }
        public void AddGold(int amout)
        {
            playerStats.AddGold(amout);
        }

        public void OpenShopUI()
        {
            shopUI.UpdateSelectSlot(null);
            Toggle(shopUI.gameObject);
            if(shopUI.gameObject.activeSelf)
            {
                shopUI.OpenInventoryUI(CloseShopUI);
            }
        }
        public void CloseShopUI()
        {
            Toggle(shopUI.gameObject);
        }
        public bool UseGold(int amount)
        {
            return playerStats.useGold(amount);
        }

        public bool EnoughGold(int amount)
        {
            return playerStats.EnoughGold(amount);
        }
    }
}