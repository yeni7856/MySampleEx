using UnityEngine;
using TMPro;

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
        #endregion

        protected virtual void Start()
        {
            //참조
            playerController = GameObject.FindAnyObjectByType<PlayerController>();
        }

        protected virtual void OnMouseOver()
        {
            distance = Vector3.Distance(transform.position, playerController.transform.position);

            if(distance < 2f)
            {
                ShowActionUI();
            }
            else
            {
                HiddenActionUI();
            }
        }



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
    }
}