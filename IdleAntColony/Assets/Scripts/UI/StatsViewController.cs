using IdleAnt.Stats;
using TMPro;
using UnityEngine;

namespace IdleAnt.UI
{
    public class StatsViewController : MonoBehaviour
    {
        [SerializeField] private Stat stat;
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            text.text = stat.GetStat().ToString();
        }

        private void OnEnable()
        {
            stat.StatChanged += OnStatChanged;
        }

        private void OnDisable()
        {
            stat.StatChanged -= OnStatChanged;
        }

        private void OnStatChanged(int value)
        {
            text.text = value.ToString();
        }
    }
}