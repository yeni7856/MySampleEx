using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MySampleEx
{
    public class StaticInventoryUI : InventoryUIManager
    {
        #region Variables
        public GameObject[] staticSlots;
        #endregion

        public override void CreateSlots()
        {
            slotUIs = new Dictionary<GameObject, ItemSlot>();
            for(int i = 0; i< inventoryObject.Slots.Length; i++)
            {
                GameObject slotgo = staticSlots[i];

                //생성된 슬롯 오브젝트의 이벤트 트리거에 이벤트 등록
                AddEvent(slotgo, EventTriggerType.PointerEnter, delegate { OnEnter(slotgo); });
                AddEvent(slotgo, EventTriggerType.PointerExit, delegate { OnExit(slotgo); });
                AddEvent(slotgo, EventTriggerType.BeginDrag, delegate { OnStartDrag(slotgo); });
                AddEvent(slotgo, EventTriggerType.Drag, delegate { OnDrag(slotgo); });
                AddEvent(slotgo, EventTriggerType.EndDrag, delegate { OnEndDrag(slotgo); });

                inventoryObject.Slots[i].slotUI = slotgo;
                slotUIs.Add(slotgo, inventoryObject.Slots[i] );
            }
        }
    }

}
