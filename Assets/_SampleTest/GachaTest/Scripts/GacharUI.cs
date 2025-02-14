using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 가챠 뽑기 진행 상태
    /// </summary>
    public enum GachaState
    {
        Ready,          //시작 버튼을 기다리는 상태
        Scroll01,       //스타트 버튼을 누르면 애니1번 플레이, 아이템은 무작위, 스톱 버튼을 기다린다.
        Scroll02,      //스톱 버튼을 누르면 애니2번 플레이, 가챠 아이템이 뽑힌 상태, 공회전 (3번)
        Scroll03,      //애니2번 플레이, 일정 시간이 지나면 정답을 보여준다, 랜덤하게 1에서 3 스크롤을 해준다.
        Result          //정답을 보여준다
    }

    /// <summary>
    /// 가챠 데이터를 가져와서 뽑기를 진행
    /// </summary>
    public class GacharUI : MonoBehaviour
    {
        #region Variables
        public GachaItems gachaList;
        public string dataPath;

        //UI
        public TextMeshProUGUI realName;
        public TextMeshProUGUI fakeName;

        public TextMeshProUGUI sentence;

        public GameObject startButton;
        public GameObject stopButton;
        public GameObject nextButton;

        private Animator animator;
        private GachaState gachaState;

        //뽑기
        private int nowIndex = 0;
        private int dummyNumber = 3;        //3회전 공회전
        private int fakeNumber = 0;             //랜덤 스크롤 횟수
        private int scrollCount = 0;

        public int gachaIndex = 0;             //가차에 뽑힌 아이템 인덱스

        [SerializeField] private string nameText = "Who?";
        #endregion

        private void Start()
        {
            //참조
            animator = GetComponent<Animator>();
            gachaList = GachaManager.GetQuestData().LoadData(dataPath);

        }

        //가차 초기화
        public void SetGacha(bool isReady)
        {
            if (isReady)
            {
                realName.text = nameText;
                fakeName.text = nameText;
                sentence.text = "시작 버튼을 누르세요";

                startButton.SetActive(true);
            }
            else
            {
                realName.text = "";
                fakeName.text = "";
                sentence.text = "";
                startButton.SetActive(false);
            }
        }

        public void StartGacha()
        {
            animator.Play("NameScroll01");

            startButton.SetActive(false);
            stopButton.SetActive(true);

            sentence.text = "스톱 버튼을 누르세요";

            //뽑기 데이터 초기
            scrollCount = 0;
            nowIndex = 1;
            realName.text = gachaList.GachaItmes[nowIndex].name;
            fakeName.text = gachaList.GachaItmes[nowIndex - 1].name;
        }

        public void StopGacha()
        {
            //뽑기
            gachaIndex = GetGachaItem();
            //Debug.Log($"{ gachaList.GachaItmes[gachaIndex].name}");
            gachaList.GachaItmes[gachaIndex].rate = 0;

            fakeNumber = Random.Range(1, 4);
            nowIndex = gachaIndex - (dummyNumber + fakeNumber) + 1;
            GetGachaItem();

            animator.Play("NameScroll02");
            scrollCount = 0;
            stopButton.SetActive(false);
            sentence.text = "";
        }

        private int GetGachaItem()
        {
            int result = 0;

            //랜덤 범위 총합 구하기
            int total = 0;
            for(int i = 0; i <gachaList.GachaItmes.Count; i++)
            {
                total += gachaList.GachaItmes[i].rate;
            }

            int randNumber = Random.Range(0, total);
            int subTotal = 0;
            for (int i = 0; i < gachaList.GachaItmes.Count; i++)
            {
                if (gachaList.GachaItmes[i].rate == 0)
                    continue;
                    subTotal += gachaList.GachaItmes[i].rate;
                if(randNumber < subTotal)
                {
                    result = i; 
                    break;
                }
            }
            return result;
        }

        private void GotoScroll03()
        {
            animator.Play("NameScroll03");
            scrollCount = 0;
        }

        private void GotoResult()
        {
            animator.Play("Empty");
            nextButton.SetActive(true);
            sentence.text = gachaList.GachaItmes[gachaIndex].name + "뽑혔습니다";
        }

        private void SetGachaName()
        {
            if (nowIndex < 0)
                nowIndex += gachaList.GachaItmes.Count;
            if(nowIndex >= gachaList.GachaItmes.Count)
                nowIndex -= gachaList.GachaItmes.Count;

            if (nowIndex == 0)
            {
                realName.text = gachaList.GachaItmes[nowIndex].name;
                fakeName.text = gachaList.GachaItmes[gachaList.GachaItmes.Count - 1].name;        //맨마지막 이름은 fakeName
            }
            else
            {
                realName.text = gachaList.GachaItmes[nowIndex].name;
                fakeName.text = gachaList.GachaItmes[nowIndex - 1].name;
            }
        }

        private void PlayScroll01()
        {
            nowIndex++;
            SetGachaName();
            //Debug.Log($"{nowIndex}");
            //animator.Play("NameScroll01");
        }
        private void PlayScroll02()
        {
            scrollCount++;

            nowIndex++;
            SetGachaName();

            if(scrollCount >= dummyNumber)
            {
                GotoScroll03();
            }
        }
        private void PlayScroll03()
        {
            scrollCount++;
            nowIndex++;
            SetGachaName();
            if (scrollCount >= fakeNumber)
            {
                GotoResult();
            }
        }
    }
}