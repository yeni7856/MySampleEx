using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 아이템 정보를 가지고있는 오브젝트들을 모아놓은 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new ItemDataBase", menuName = "Inventory System/ItemDataBase/New ItemDataBase")]
    public class ItemDataBaseSO : ScriptableObject
    {
        #region Variables
        public ItemObject[] itemObjects;        //배열의순서대로 아이디값 결정됨
        #endregion

        //인스펙터창에서 값을 조정할때 호출되는 함수 
        //아이템오브젝트에 있는 아이템의 id값을 설정 
        private void OnValidate()           
        {
            for(int i = 0; i < itemObjects.Length; i++)
            {
                if(itemObjects[i] == null)
                    continue;
                itemObjects[i].data.id = i; 
            }
        }
    }
}