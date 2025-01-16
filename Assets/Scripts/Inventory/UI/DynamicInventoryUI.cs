using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MySampleEx
{
    public class DynamicInventoryUI : InventoryUIManager
    {
        public GameObject slotPrefab;
        public Transform slotsParent;

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


                //
                inventoryObject.Slots[i].slotUI = go;
                slotUIs.Add(go, inventoryObject.Slots[i]);
                go.name += " : " + i.ToString();
            }
        }       
    }
}