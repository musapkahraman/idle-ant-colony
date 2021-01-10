using TMPro;
using UnityEngine;

public class UpgradeButtonController : MonoBehaviour
{
    [SerializeField] private Upgrade upgrade;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text priceText;

    private void OnEnable()
    {
        upgrade.LevelChanged += OnLevelChanged;
        upgrade.PriceChanged += OnPriceChanged;
    }

    private void OnDisable()
    {
        upgrade.LevelChanged -= OnLevelChanged;
        upgrade.PriceChanged -= OnPriceChanged;
    }

    private void OnLevelChanged(int level)
    {
        levelText.text = level.ToString();
    }

    private void OnPriceChanged(int price)
    {
        priceText.text = price.ToString();
    }
}