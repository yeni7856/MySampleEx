using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MySampleEx
{
    public class PlayerInGameUI : MonoBehaviour
    {
        #region Variables
        public StatsObject statsObject;

        public Image healthBar;
        public Image manaBar;

        public TextMeshProUGUI levelText;
        public TextMeshProUGUI expText;
        #endregion

        private void OnEnable()
        {
            statsObject.OnChagnedStats += OnChangedStats;
        }

        private void OnDisable()
        {
            statsObject.OnChagnedStats -= OnChangedStats;
        }

        private void Start()
        {
            healthBar.fillAmount = statsObject.HealthPercentage;
            manaBar.fillAmount = statsObject.ManaPercentage;

            levelText.text = statsObject.level.ToString();
            expText.text = statsObject.exp.ToString();
        }

        private void Update()
        {
            levelText.text = statsObject.level.ToString();
            expText.text = statsObject.exp.ToString();
        }

        private void OnChangedStats(StatsObject statsObject)
        {
            healthBar.fillAmount = statsObject.HealthPercentage;
            manaBar.fillAmount = statsObject.ManaPercentage;
        }
    }
}
