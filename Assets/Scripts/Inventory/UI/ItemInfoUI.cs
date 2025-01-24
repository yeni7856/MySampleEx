using UnityEngine;
using TMPro;

namespace MySampleEx
{
    /// <summary>
    /// 아이템 정보창 기능
    /// </summary>
    public class ItemInfoUI : MonoBehaviour
    {
        #region Variables
        //아이템 속성 텍스트
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;
        #endregion
        
        public void SetItemInfoUI(ItemSlot itemSlot) 
        { 
            ItemObject itemObject = itemSlot.ItemObject;

            itemName.text = itemObject.name;
            itemDescription.text = itemObject.description;
        }
    }
}
