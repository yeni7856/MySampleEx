using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace MySampleEx
{
    public class QuestData : ScriptableObject
    {
        #region Variables
        public Quests Quests;         //퀘스트 데이터베이스

        private string dataPath = "Data/QuestData";
        #endregion

        //생성자
        public QuestData() { }

        //데이터 읽기
        public void LoadData()
        {
            TextAsset asset = (TextAsset)ResourcesManager.Load(dataPath);
            if (asset == null || asset.text == null)
            {
                return;
            }

            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(Quests));
                Quests = (Quests)xs.Deserialize(reader);

                foreach (var quest in Quests.quests)
                {
                    quest.questGoal = new QuestGoal();
                    quest.questGoal.questType = quest.questType;
                    quest.questGoal.goalIndex = quest.goalIndex;
                    quest.questGoal.goalAmount = quest.goalAmount;
                    quest.questGoal.currentAmount = 0;
                }
            }
        }
    }
}
