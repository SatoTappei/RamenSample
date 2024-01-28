using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    public static class EnumExtensions
    {
        public static string ToString(FoodType food)
        {
            if (food == FoodType.Soup) return "スープ";
            if (food == FoodType.Noodle) return "麺";
            if (food == FoodType.CharSiu) return "チャーシュー";

            Debug.LogWarning($"{nameof(FoodType)}対応する値が無い: {food}");
            return $"対応する文字列が無い{nameof(FoodType)}";
        }
    }
}
