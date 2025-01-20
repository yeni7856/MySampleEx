using UnityEngine;

namespace MySampleEx
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public ItemDataBaseSO dataBase;

        public DynamicInventoryUI playerInventoryUI;
        public StaticInventoryUI playerIEquipmentUI;

        public int itemId = 0;
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                playerInventoryUI.gameObject.SetActive(!playerInventoryUI.gameObject.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerIEquipmentUI.gameObject.SetActive(!playerIEquipmentUI.gameObject.activeSelf);
                //커서

            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddNewItem();
            }
        }

        public void AddNewItem()
        {
            ItemObject itemObject = dataBase.itemObjects[itemId];
            Item newItem = itemObject.CreateItme();

            playerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }
    }
}