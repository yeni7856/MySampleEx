using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 아이템 기본 정보를 저장하는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item/New Item")]
    public class ItemObject : ScriptableObject
    {
        #region Variables
        public Item data = new Item();

        public ItemType type;
        public bool stackable;                  //슬롯에 누적 여부 //true면 누적
        public Sprite icon;
        public GameObject modelPrefab;

        [TextArea(15,20)]
        public string description;
        #endregion

        public Item CreateItme()
        {
            Item newItem = new Item(this);      //this 오브젝트 정보 넘겨줘야함
            return newItem;                 //아이템 반환
        }
    }
}