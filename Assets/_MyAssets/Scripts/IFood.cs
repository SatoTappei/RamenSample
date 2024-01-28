using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    /// <summary>
    /// 「何が」「どれくらいの量」を表現するための構造体
    /// </summary>
    public struct Per<T>
    {
        public Per(T entity, int volume)
        {
            Entity = entity;
            Volume = volume;
        }

        public T Entity { get; private set; }
        public int Volume { get; private set; }
    }

    /// <summary>
    /// 具材とどんぶりが接触した際に、ラーメン側が具材がどんぶりに入ったかを判定する。
    /// </summary>
    public interface IFood
    {
        /// <summary>
        /// 具材がラーメンに追加可能な状態ならば「どの具材が」「1回で追加される量」を返す
        /// </summary>
        public bool TryAddRamen(out Per<FoodType> food);
    }
}
