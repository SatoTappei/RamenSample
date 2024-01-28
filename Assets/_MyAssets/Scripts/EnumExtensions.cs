using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    public static class EnumExtensions
    {
        public static string ToString(FoodType food)
        {
            if (food == FoodType.Soup) return "�X�[�v";
            if (food == FoodType.Noodle) return "��";
            if (food == FoodType.CharSiu) return "�`���[�V���[";

            Debug.LogWarning($"{nameof(FoodType)}�Ή�����l������: {food}");
            return $"�Ή����镶���񂪖���{nameof(FoodType)}";
        }
    }
}
