using System;
using System.Collections;
using IdleAnt.Food;
using UnityEngine;

namespace IdleAnt.Spawn
{
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private Target[] targets;
        private Target _activeTarget;
        private int _index;

        public Target GetActiveTarget()
        {
            return _activeTarget;
        }

        public void BringNextTarget(Action foodReady = null)
        {
            if (_activeTarget) Destroy(_activeTarget.gameObject);

            if (_index < targets.Length)
            {
                _activeTarget = Instantiate(targets[_index++]);
                StartCoroutine(FoodSlideInCoroutine(foodReady, _activeTarget.transform));
                return;
            }

            Debug.Log("Game is finished!");
        }

        private static IEnumerator FoodSlideInCoroutine(Action foodReady, Transform food)
        {
            var foodPosition = food.position;
            var targetPosition = foodPosition;
            foodPosition += 20 * Vector3.right;
            food.position = foodPosition;
            while (Vector3.Distance(food.position, targetPosition) > 1)
            {
                yield return null;
                food.position = Vector3.Lerp(food.position, targetPosition, 2 * Time.deltaTime);
            }

            foodReady?.Invoke();
        }
    }
}