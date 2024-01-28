using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    [CreateAssetMenu(fileName = "ParameterSettings_Ramen_", menuName = "ParameterSettings/Ramen")]
    public class RamenParameterSettings : ScriptableObject
    {
        [Header("�K�v�ȋ��")]
        [SerializeField] List<FoodCondition> _foods;
        [Header("���i��")]
        [SerializeField] string _productName;

        public IReadOnlyList<FoodCondition> Foods => _foods ??= new();
        public string ProductName => _productName;
    }
}

