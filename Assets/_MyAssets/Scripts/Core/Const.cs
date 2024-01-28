using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    public class Const
    {
        /// <summary>
        /// 左右のコントローラーのタグ
        /// </summary>
        public const string ControllerTag = "GameController";

        /// <summary>
        /// コントローラで操作して間接的に物をつかむ際の掴む位置のタグ
        /// 具材を生成する判定に触れた場合にコントローラ同様生成される
        /// </summary>
        public const string GrabIndirectlyTag = "GrabIndirectly";

        /// <summary>
        /// 湯切りざるを取り出した際に水が流れ落ちる判定のタグ
        /// </summary>
        public const string WaterTag = "Water";

        /// <summary>
        /// お玉と接触した際にスープが補充される判定のタグ
        /// </summary>
        public const string SoopTag = "Soop";

        /// <summary>
        /// 掴むアイテムのレイヤー
        /// </summary>
        public const int GrabItemtLayer = 0;
    }
}