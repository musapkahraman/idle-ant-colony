using System.Text;
using IdleAnt.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleAnt.UI
{
    public class UpgradeButtonController : MonoBehaviour
    {
        [SerializeField] private Bank bank;
        [SerializeField] private Upgrade upgrade;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite passiveSprite;
        private Button _button;
        private Image _image;
        private int _cost;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            SetLevelText(upgrade.Level);
            OnCostChanged(upgrade.Cost);
            OnBankBalanceChanged(bank.GetStat());
        }

        private void OnEnable()
        {
            upgrade.StatChanged += OnLevelChanged;
            upgrade.CostChanged += OnCostChanged;
            bank.StatChanged += OnBankBalanceChanged;
        }

        private void OnDisable()
        {
            upgrade.StatChanged -= OnLevelChanged;
            upgrade.CostChanged -= OnCostChanged;
            bank.StatChanged -= OnBankBalanceChanged;
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
            _cost = cost;
        }

        private void OnBankBalanceChanged(int bankBalance)
        {
            if (bankBalance < _cost)
            {
                _button.interactable = false;
                _image.sprite = passiveSprite;
            }
            else
            {
                _button.interactable = true;
                _image.sprite = activeSprite;
            }
        }

        private void SetLevelText(int level)
        {
            var sb = new StringBuilder("Lv. ");
            sb.Append(level);
            levelText.text = sb.ToString();
        }
    }
}