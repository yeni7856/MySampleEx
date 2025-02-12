using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;

namespace MySampleEx
{
    public class TouchManager : MonoBehaviour
    {
        #region Variabels
        public GameObject touchCanvas;
        public CameraSetting cameraSetting;
        #endregion

        private void Awake()
        {
            EnhancedTouchSupport.Enable();
        }
        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();

        }

        private void Start()
        {
#if TOUCH_MODE
            touchCanvas.SetActive(true);
#else
            touchCanvas.SetActive(false);
#endif
        }
        private void Update()
        {
#if TOUCH_MODE
            if(Touch.activeTouches.Count > 0)   //터치가 들어왔을때
            {
                var touch = Touch.activeTouches[0]; //터치가 시작했는지 이동하고있는지 
                switch (touch.phase)
                { 
                    case UnityEngine.InputSystem.TouchPhase.Began:

                        //터치한 지점에 레이를 쏘아 충돌체를 찾는다.
                        Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);
                        RaycastHit hit;
                        if(Physics.Raycast(ray, out hit))
                        { 
                            if(hit.transform.tag == "Npc")
                            {
                                //TODo : 플레이어와 Npc 간의 거리
                                hit.transform.SendMessage("DoAction");
                            }
                        }
                        //
                        if (EventSystem.current.IsPointerOverGameObject())//UI클릭시
                        {
                            cameraSetting.SetchineInputAxisController(false);
                        }
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Moved:
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Ended:
                        cameraSetting.SetchineInputAxisController(true);
                        break;
                }
            }
#endif
        }
    }
}