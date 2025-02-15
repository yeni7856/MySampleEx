using System;
using System.Linq;
using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 아이템 슬롯들을 가지고 있는 데이터 컨테이너 클래스
    /// </summary>
    [Serializable]
    public class Inventory
    {
        #region Varables
        public ItemSlot[] slots = new ItemSlot[16];
        #endregion

        //인벤토리 슬롯 초기화
        public void Clear()
        {
            foreach(var slot in slots)
            {
                //빈슬롯 만들기
                slot.UpdateSlot(new Item(), 0); 
            }
        }
        //슬롯 아이템 찾기 : ItemObeject
        public bool IsContain(ItemObject itemObject)
        {
            return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
        }
        //아이템 찾기 : id
        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }
    }
}