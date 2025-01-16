using UnityEngine;

namespace MySampleEx
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public ItemDataBaseSO dataBase;
        public DynamicInventoryUI playerInventoryUI;
        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                playerInventoryUI.gameObject.SetActive(!playerInventoryUI.gameObject.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                AddNewItem();
            }
        }

        public void AddNewItem()
        {
            ItemObject itemObject = dataBase.itemObjects[1];
            Item newItem = itemObject.CreateItme();

            playerInventoryUI.inventoryObject.AddItem(newItem, 1);
        }
    }
}