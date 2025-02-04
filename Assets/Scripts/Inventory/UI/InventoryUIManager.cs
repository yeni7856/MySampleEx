using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MySampleEx
{
    /// <summary>
    /// 인벤토리 UI 부모 (추상)클래스
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUIManager : MonoBehaviour
    {
        #region Variables
        public InventoryObject inventoryObject;
        public Dictionary<GameObject, ItemSlot> slotUIs = new Dictionary<GameObject, ItemSlot>();
        
        //슬롯 선택기능
        protected GameObject selectSlotObject = null;   //현재 선택된 슬롯 오브젝트
        public Action<GameObject> OnUpdateSelectSlot;

        public Action OnCloseInventoryUI;
        #endregion

        private void Awake()
        {
            CreateSlots();

            //인벤토리 슬롯 설정
            for (int i = 0; i < inventoryObject.Slots.Length; i++)
            {
                inventoryObject.Slots[i].parent = inventoryObject;
                //아이템 내용 변경시 호출되는 UI 함수 등록 
                inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
            }

            //이벤트 트리거 이벤트 등록
            AddEvent(this.gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(this.gameObject); } );
            AddEvent(this.gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(this.gameObject); } );
        }

        protected virtual void OnEnable()
        {
            OnCloseInventoryUI = null;
        }

        protected virtual void Start()
        {
            //UI 슬롯 갱신
            for(int i = 0;i < inventoryObject.Slots.Length; i++)
            {
                inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
            }
        }

        public abstract void CreateSlots();

        public void OnPostUpdate(ItemSlot itemSlot)
        {
            //아이템 슬롯 체크
            if (itemSlot == null || itemSlot.slotUI == null)
            {
                return;
            }
            itemSlot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite
                = itemSlot.item.id < 0 ? null : itemSlot.ItemObject.icon;
            itemSlot.slotUI.transform.GetChild(0).GetComponent<Image>().color
                = itemSlot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
            itemSlot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text
                = itemSlot.item.id < 0 ? string.Empty :
                (itemSlot.amount == 1 ? string.Empty : itemSlot.amount.ToString());
        }

        //이벤트 트리거 이벤트 등록
        protected  void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                Debug.LogWarning("No EventTrigger componet found!");
                return;
            }
            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);

        }

        //게임오브젝트에 마우스 들어갈때 이벤트
        public void OnEnterInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUIManager>();
        }

        public void OnExitInterface(GameObject go)
        {
            MouseData.interfaceMouseIsOver = null;
        }

        //슬롯오브젝트에 마우스가 들어갈때 호출
        public void OnEnter(GameObject go)
        {
            MouseData.slotHoveredOver = go;
            MouseData.interfaceMouseIsOver = GetComponentInParent<InventoryUIManager>();
        }

        public void OnExit(GameObject go)
        {
            MouseData.slotHoveredOver = null;
        }

        //슬롯오브젝트에 마우스를 드래그 할때 호출
        public void OnStartDrag(GameObject go)
        {
            MouseData.tempItemBeginDragged = CreateDragImage(go);
            OnUpdateSelectSlot?.Invoke(null);
            UpdateSelectSlot(null);
        }

        //마우스 드래그시 마우스 포인터이미지 오브젝트
       GameObject CreateDragImage(GameObject go)
        {
            if (slotUIs[go].item.id <= -1)
            {
                return null;            //빈슬롯 아이콘아이템이없는 부분 
            }

            GameObject dragImage = new GameObject();

            RectTransform rectTransform = dragImage.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            dragImage.transform.SetParent(transform.parent);
            Image image = dragImage.AddComponent<Image>();
            image.sprite = slotUIs[go].ItemObject.icon;
            image.raycastTarget = false;        //클릭이벤트 안먹도록

            dragImage.name = "Drag Image";

            return dragImage;
        }

        //마우스 드래그시 마우스 포인터에 달고 다니는 이미지 위치 설정
        public void OnDrag(GameObject go)
        {
            if(MouseData.tempItemBeginDragged == null)
            {
                return;
            }
            MouseData.tempItemBeginDragged.GetComponent<RectTransform>().position = Input.mousePosition;

        }

        //슬롯 오브젝트에 마우스를 드래그 끝날때 호출
        public void OnEndDrag(GameObject go)
        {
            Destroy(MouseData.tempItemBeginDragged);

            //마우스의 위치가 인벤토리 UI 밖에 있을 경우
            if(MouseData.interfaceMouseIsOver == null)
            {
                //아이템 버리기
                slotUIs[go].Remove();
                Debug.Log("아이템 버리기");
            }
            else if(MouseData.slotHoveredOver != null)      //a마우스의 위치가 슬롯 게임 오브젝트 위에 잇을경우
            {
                //아이템 바꾸기
                //마우스가 위치한 게임 오브젝트의 슬롯
                ItemSlot mouseHoverSlot = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
                inventoryObject.SwapItems(slotUIs[go], mouseHoverSlot);
                Debug.Log("아이템 교환하기");
            }
            else
            {
                Debug.Log("슬롯없음");
            }
        }
        
        //슬롯 오브젝트를 마우스로 클릭할때 호출
        public void OnClick(GameObject go)
        {
            OnUpdateSelectSlot?.Invoke(null);   //업데이트를 null해야 디스토리 됨

            ItemSlot slot = slotUIs[go];

            //아이템 존재 여부 체크
            if(slot.item.id >= 0)
            {
                //현재 선택된 슬롯 오브젝트 와 같으면 다시선택
                if (selectSlotObject == go)
                {
                    UpdateSelectSlot(null);
                }
                else
                {
                    UpdateSelectSlot(go);
                }
            }
        }

        public virtual void UpdateSelectSlot(GameObject go)
        {
            selectSlotObject = go;
            foreach(KeyValuePair<GameObject, ItemSlot> slot in slotUIs)      //2개의 값이 들어옴
            {
                //slot.Key
                //slot.Value
                if(slot.Key == go)
                {
                    slot.Value.slotUI.transform.GetChild(1).GetComponent<Image>().enabled = true;
                }
                else
                {
                    slot.Value.slotUI.transform.GetChild(1).GetComponent<Image>().enabled = false;
                }
            }
        }

        public virtual void OpenInventoryUI(Action closeMethod)
        {
            if (closeMethod != null)
                OnCloseInventoryUI += closeMethod;
            //shop 셋팅
        }

        public virtual void CloseInventoryUI()
        {
            UpdateSelectSlot(null);
            OnCloseInventoryUI?.Invoke();
        }

    }
}