using UnityEngine;
using TMPro;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace MySampleEx
{
    /// <summary>
    /// NPC를 관리하는 클래스, 인터랙티브 기능 추가
    /// </summary>
    public class PickupNpc : MonoBehaviour
    {
        #region Variables
        public Npc npc;

        //인터랙티브 기능
        protected PlayerController playerController;
        protected float distance;

        public TextMeshProUGUI actionTextUI;
        public string actionText = "Pickup ";

        public float interactDistance = 2f;  // 상호작용 가능한 거리

        #endregion

        protected virtual void Start()
        {
            //참조
            playerController = GameObject.FindAnyObjectByType<PlayerController>();
        }

#if TOUCH_MODE
        private void Update()
        {
            //터치 입력있을때
            if(Touch.activeTouches.Count > 0)
            {
                var touch = Touch.activeTouches[0];  // 첫 번째 터치 가져오기
                switch (touch.phase)
                {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                        // 터치한 지점에 레이를 쏘아 충돌체를 찾는다
                        Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            Debug.Log("Raycast Hit!");
                            if (hit.transform.tag == "Npc")  // 이 객체가 터치된 경우
                            {
                                Debug.Log("Hit NPC: " + hit.transform.name); // 어떤 NPC가 맞았는지 출력
                                ShowActionUI();  // 가까운 경우 UI 표시
                                // 터치로 상호작용
                                DoAction();
                            }
                            else
                            {
                                Debug.Log("Hit non-NPC object: " + hit.transform.name); // NPC가 아닌 객체일 경우
                                HiddenActionUI();  // 멀리 있을 경우 UI 숨기기
                            }
                        }
                        break;

                    case UnityEngine.InputSystem.TouchPhase.Ended:
                        HiddenActionUI();  // 터치가 끝났을 때 UI 숨기기
                        break;
                }
            }
        }
#endif

#if !TOUCH_MODE
        protected virtual void OnMouseOver()
        {
            distance = Vector3.Distance(transform.position, playerController.transform.position);

            if (distance < 2f)
            {
                ShowActionUI();
            }
            else
            {
                HiddenActionUI();
            }
            if (Input.GetKeyUp(KeyCode.E) && distance < 2f)
            {
                //transform.GetComponent<BoxCollider>().enabled = false;
                DoAction();
            }
        }
        private void OnMouseExit()
        {
            HiddenActionUI();
        }
#endif

        protected virtual void ShowActionUI()
        {
            actionTextUI.gameObject.SetActive(true);
            actionTextUI.text = actionText + npc.name;
        }

        protected virtual void HiddenActionUI()
        {
            actionTextUI.gameObject.SetActive(false);
            actionTextUI.text = "";
        }

        public virtual void DoAction()
        {
            UIManager.Instance.OpenDialogUI(0, npc.npcType);
#if TOUCH_MODE
            //TODO : something touch
#endif
        }

    }
}