using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MySampleEx
{
    public class ShopUI : InventoryUIManager
    {
        public GameObject[] staticSlots;            //아이템 슬롯 오브젝트
        public ItemObject[] ItemObject;         //아이템 목록

        public GameObject BuyButton;        //구매버튼

        private UIManager uiManager;

        public GameObject adButton;

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, ItemSlot>();
            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                GameObject slotgo = staticSlots[i];

                //생성된 슬롯 오브젝트의 이벤트 트리거에 이벤트 등록
                AddEvent(slotgo, EventTriggerType.PointerClick, delegate { OnClick(slotgo); });

                inventoryObject.Slots[i].slotUI = slotgo;
                slotUIs.Add(slotgo, inventoryObject.Slots[i]);

                //npc가 판매하는 아이템 목록 슬롯에 셋팅
                Item shopItem = ItemObject[i].CreateItem();
                slotUIs[slotgo].UpdateSlot(shopItem, 1);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(uiManager == null)
            {
                uiManager = UIManager.Instance;
            }
#if AD_MODE
            adButton.SetActive(true);   
#endif
        }

        public override void OpenInventoryUI(Action closeMethod)
        {
            if (closeMethod != null)
                OnCloseInventoryUI += closeMethod;
            //shop 셋팅
        }

        public override void UpdateSelectSlot(GameObject go)
        {
            base.UpdateSelectSlot(go);
            if (selectSlotObject == null)
            {
                BuyButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                BuyButton.GetComponent<Button>().interactable = true;
            }
        }

        public void BuyItem()
        {
            if (selectSlotObject == null)
                return;
            int price = slotUIs[selectSlotObject].ItemObject.shopPrice;

            //인벤토리에 비어있어야함 + 주머니에 돈이잇어야한다.

            if (uiManager.EnoughGold(price))
            {
                Item newItem = slotUIs[selectSlotObject].ItemObject.CreateItem();
                if (uiManager.AddItemInventory(newItem, 1))
                {
                    uiManager.UseGold(price);
                    UpdateSelectSlot(null);
                }
            }
            else
            {
                Debug.Log("돈이 부족합니다");
            }
        }

        public void ShowAd()
        {
            //타이머 - 일정시간동안 광고를 보지 못하도록 한다

#if AD_MODE
            AdManager.Instance.ShowRewardAd();
#endif
        }
    }
}
