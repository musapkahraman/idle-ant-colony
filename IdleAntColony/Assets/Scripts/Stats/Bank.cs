using UnityEngine;

namespace IdleAnt.Stats
{
    [CreateAssetMenu]
    public class Bank : Stat
    {
        [Tooltip("The amount of gold that earned for each piece of food.")] [SerializeField]
        private int foodPrice = 5;

        [SerializeField] private int goldAccumulated;

        public override int GetStat()
        {
            return goldAccumulated;
        }

        public void ExchangeFoodPiecesWithGold(int amount)
        {
            goldAccumulated += foodPrice * amount;
            OnStatChanged(goldAccumulated);
        }

        public bool Spend(int amount)
        {
            if (goldAccumulated < amount) return false;
            goldAccumulated -= amount;
            OnStatChanged(goldAccumulated);
            return true;
        }
    }
}