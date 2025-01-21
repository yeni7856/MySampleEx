using UnityEngine;

namespace MySampleEx
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public ItemDataBaseSO database;

        public DynamicInventoryUI palyerInventoryUI;
        public StaticInventoryUI palyerEquipmentUI;
        public PlayerStatsUI playerStatsUI;
        public DialogUI dialogUI;

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

        public void OpenDialogUI(int dialogIndex)
        {
            Toggle(dialogUI.gameObject);
            dialogUI.StartDialog(dialogIndex);
        }

        public void CloseDialogUI()
        {
            Toggle(dialogUI.gameObject);
        }
    }
}