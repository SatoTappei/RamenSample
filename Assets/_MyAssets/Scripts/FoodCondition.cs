using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PSB.Ramen
{
    [System.Serializable]
    public class FoodCondition
    {
        [SerializeField] FoodType _food;
        [Range(1, 10)]
        [SerializeField] int _volume;
        [Header("先に入れる必要がある食材")]
        [SerializeField] List<FoodType> _prerequisites;

        public FoodType Food => _food;
        public int Volume => _volume;

        /// <summary>
        /// 先に入れる必要がある食材が全て器に入っているかチェック
        /// </summary>
        public bool IsCompletedPrerequisites(IEnumerable<FoodType> foods)
        {
            foreach (FoodType f in _prerequisites)
            {
                if (!foods.Contains(f)) return false;
            }

            return true;
        }
    }
}
