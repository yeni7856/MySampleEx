using System.Collections.Generic;
using UnityEngine;


namespace MySampleEx
{
    public class GachaManager : MonoBehaviour
    {
        #region Variables
        private static GachaData gachaData = null;

        public GacharUI nameGacha;
        public GacharUI itemGacha;

        //수상자 목록
        public Transform prizeParent;
        public GameObject prizePrefab;
        public List<PrizeSlotUI> prizeSlots = new List<PrizeSlotUI>();
        private int winnerIndex = -1;

        #endregion

        private void Awake()
        {
            //가챠 데이터 가져오기
            if (gachaData == null)
            {
                gachaData = ScriptableObject.CreateInstance<GachaData>();
                //gachaData.gachaItemList = gachaData.LoadData("Gacha/GachaItem");
                //gachaData.gachaNameList = gachaData.LoadData("Gacha/GachaName");
            }
            Debug.Log("가챠 데이터 가져오기");
        }

        //가챠 데이터 가져오기
        public static GachaData GetQuestData()
        {
            if (gachaData == null)
            {
                gachaData = ScriptableObject.CreateInstance<GachaData>();
                //gachaData.gachaItemList = gachaData.LoadData("Gacha/GachaItem");
                //gachaData.gachaNameList = gachaData.LoadData("Gacha/GachaName");
            }
            return gachaData;
        }

        private void Start()
        {
            //초기화
            nameGacha.SetGacha(true);
            itemGacha.SetGacha(false);
        }

        public void NextGacha()
        {
            nameGacha.nextButton.SetActive(false);
            itemGacha.SetGacha(true);

            CreatePrizeSlot();
            prizeSlots[winnerIndex].SetNameText(nameGacha.gachaList.GachaItmes[nameGacha.gachaIndex].name);
        }

        public void ItemNextGacha()
        {
            itemGacha.nextButton.SetActive(false);

            nameGacha.SetGacha(true);
            itemGacha.SetGacha(false) ;

            prizeSlots[winnerIndex].SetItemtex(itemGacha.gachaList.GachaItmes[itemGacha.gachaIndex].name);
        }

        private void CreatePrizeSlot()
        {
            GameObject slotGo = Instantiate(prizePrefab, prizeParent);
            PrizeSlotUI prizeSlotUI = slotGo.GetComponent<PrizeSlotUI>();

            prizeSlots.Add(prizeSlotUI);
            winnerIndex++;
        }
    }
}