using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MySampleEx
{
    /// <summary>
    /// Card 데이터를 적용
    /// </summary>
    public class DrowCard : MonoBehaviour
    {
        //public GameObject card;
        public Card card;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI manaText;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI healthText;

        public Image artImage;

        private void Start()
        {
            //Card 데이터를 오브젝트에 적용
            UpdateCard();
        }
        void UpdateCard()
        {
            nameText.text = card.name;
            descriptionText.text = card.description;  

            manaText.text = card.mana.ToString();
            attackText.text = card.attack.ToString();
            healthText.text = card.health.ToString();

            artImage.sprite = card.argImage;
        }
    }
}
