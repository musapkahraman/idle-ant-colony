using System.Text;
using IdleAnt.Stats;
using TMPro;
using UnityEngine;

namespace IdleAnt.UI
{
    public class UpgradeButtonController : MonoBehaviour
    {
        [SerializeField] private Upgrade upgrade;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text costText;

        private void Start()
        {
            SetLevelText(upgrade.Level);
            costText.text = upgrade.Cost.ToString();
        }

        private void OnEnable()
        {
            upgrade.StatChanged += OnLevelChanged;
            upgrade.CostChanged += OnCostChanged;
        }

        private void OnDisable()
        {
            upgrade.StatChanged -= OnLevelChanged;
            upgrade.CostChanged -= OnCostChanged;
        }

        public void IncreaseLevel()
        {
            upgrade.IncreaseLevel();
        }

        private void OnLevelChanged(int level)
        {
            SetLevelText(level);
        }

        private void OnCostChanged(int cost)
        {
            costText.text = cost.ToString();
        }

        private void SetLevelText(int level)
        {
            var sb = new StringBuilder("Lv. ");
            sb.Append(level);
            levelText.text = sb.ToString();
        }
    }
}