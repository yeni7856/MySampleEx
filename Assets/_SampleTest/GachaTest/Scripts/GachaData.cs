using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 가차 아이쳄 데이터를 관리하는 클래스
    /// </summary>
    public class GachaData : ScriptableObject
    {
        #region Variables
        public GachaItems gachaNameList;
        public GachaItems gachaItemList;
        #endregion

        //생성자
        public GachaData(){}

        //데이터 읽기
        public GachaItems LoadData(string dataPath)
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);
            if (asset == null || asset.text == null)
            {
                return null;
            }
            GachaItems itemList;
            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(GachaItems));
                itemList = (GachaItems)xs.Deserialize(reader);
            }
            return itemList;
        }
    }
}