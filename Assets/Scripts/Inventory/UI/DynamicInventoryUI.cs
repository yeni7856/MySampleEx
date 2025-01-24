using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MySampleEx
{
    public class DynamicInventoryUI : InventoryUIManager
    {
        public GameObject slotPrefab;
        public Transform slotsParent;

        public ItemInfoUI itemInfoUI;

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, ItemSlot>();
            //인벤토리 오브젝트안에잇는 슬롯에 갯수만큼
            for(int i = 0;i<inventoryObject.Slots.Length;i++)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero,Quaternion.identity, slotsParent);

                //생성된 슬롯 오브젝트의 이벤트 트리거에 이벤트 등록
                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnter(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExit(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
                AddEvent(go, EventTriggerType.PointerClick, delegate { OnClick(go); });

                //
                inventoryObject.Slots[i].slotUI = go;
                slotUIs.Add(go, inventoryObject.Slots[i]);
                go.name += " : " + i.ToString();
            }
        }
        
        public override void UpdateSelectSlot(GameObject go)
        {
            base.UpdateSelectSlot(go);

            if (selectSlotObject == null)
            {
                itemInfoUI.gameObject.SetActive(false);
            }
            else
            {
                itemInfoUI.gameObject.SetActive(true);
                itemInfoUI.SetItemInfoUI(slotUIs[selectSlotObject]);
            }
        }
        public void UseItem()
        {
            if (selectSlotObject == null)
                return;
            //소모품, (장비아이템 장착) 
            inventoryObject.UseItem(slotUIs[selectSlotObject]);
            UpdateSelectSlot(null);
        }
        public void SellItem()
        {
            if (selectSlotObject == null)
                return;
            //아이템 판매대금(구매대금의 반값) 받고
            int sellPrice = slotUIs[selectSlotObject].ItemObject.shopPrice / 2;
            UIManager.Instance.AddGold(sellPrice);
            slotUIs[selectSlotObject].Remove();
            UpdateSelectSlot(null);
        }
    }
}