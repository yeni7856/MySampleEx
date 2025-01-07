using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using TMPro;

namespace MySampleEx
{
    /// <summary>
    /// 대화창 UI 구현 클래스
    /// 대화 데이터 파일 읽기
    /// 대화 데이터 UI 적용
    /// </summary>
    public class DialogUI : MonoBehaviour
    {
        #region Variables
        //xml
        public string xmlFile = "Dialog/Dialog";        //파일 경로 path
        private XmlNodeList allNodes;

        private Queue<Dialog> dialogs;

        //UI
        public TextMeshProUGUI nameTxt;
        public TextMeshProUGUI sentenceTxt;
        public GameObject npcImg;
        public GameObject nextButton;

        #endregion
        private void Start()
        {
            //xml 데이터 파일 읽기
            LoadDialogXml(xmlFile);

            dialogs = new Queue<Dialog>();
            InitDialog();

            //시작
            StartDialog(0); //int dialogIndex  =>  Dialog/number
        }
        //xml 데이터 읽어 들이기
        void LoadDialogXml(string path)
        {
           TextAsset xmlFile = Resources.Load<TextAsset>(path); 
            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile.text);
            allNodes = xmlDoc.SelectNodes("root/Dialog");       //스키마에 만든
        }

        //초기화
        void InitDialog()
        {
            dialogs.Clear();

            npcImg.SetActive(false);
            nameTxt.text = "";
            sentenceTxt.text = "";

            nextButton.SetActive(false);
        }


        //대화 시작하기
        public void StartDialog(int dialogIndex)
        {
            //현재 대화씬
            foreach (XmlNode node in allNodes)
            {
                int num = int.Parse(node["number"].InnerText);
                if(num == dialogIndex)
                {
                    Dialog dialog = new Dialog();
                    dialog.number = num;
                    dialog.character = int.Parse(node["character"].InnerText);
                    dialog.name = node["name"].InnerText;
                    dialog.sentence = node["sentence"].InnerText;

                    dialogs.Enqueue(dialog);        //Queue
                }
            }
            //첫번째 대화를 보여준다
            DrowNextDialog();
        }

        //두번재 대화를 보여준다 - (큐)dialogs에서 하나씩 꺼내서 보여주기
        public void DrowNextDialog()
        {
            //dialogs 체크
            if(dialogs.Count == 0)
            {
                EndDialog();
                return;
            }
            //dialogs 에서 꺼내오기
            Dialog dialog = dialogs.Dequeue();
            if(dialog.character >0)
            {
                npcImg.SetActive(true);
                npcImg.GetComponent<Image>().sprite = 
                    Resources.Load<Sprite>("Dialog/Npc/npc0" + dialog.character.ToString());
            }
            else //dialog.character <= 0
            {
                npcImg.SetActive(false);
            }
            nextButton.SetActive(false);

            nameTxt.text = dialog.name;
            //sentenceTxt.text = dialog.sentence;
            StartCoroutine(typingSentence(dialog.sentence));
        }

        //텍스트 타이핑 연출
       IEnumerator typingSentence(string typingText)
        {
            sentenceTxt.text = "";
            foreach(char latter in typingText)
            {
                sentenceTxt.text += latter;
                yield return new WaitForSeconds(0.03f);
            }
            nextButton.SetActive(true);
        }

        //대화 종료
        void EndDialog()
        {
            InitDialog();

            //대화 종료시 이벤트 처리 
            //...
        }
    }
}