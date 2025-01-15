using UnityEngine;

namespace MySampleEx
{
    public class TestManager : MonoBehaviour
    {

        public ItemDataBaseSO dataBase;
        public InventoryObject inventoryObject;


        void Start()
        {
            Item newItem = dataBase.itemObjects[0].CreateItme();        //아이템A 생성
            inventoryObject.AddItem(newItem, 1);
        }
    }
}