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
        [Header("��ɓ����K�v������H��")]
        [SerializeField] List<FoodType> _prerequisites;

        public FoodType Food => _food;
        public int Volume => _volume;

        /// <summary>
        /// ��ɓ����K�v������H�ނ��S�Ċ�ɓ����Ă��邩�`�F�b�N
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
