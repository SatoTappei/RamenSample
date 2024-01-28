using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UniRx;

namespace PSB.Ramen
{
    /// <summary>
    /// 「含まれる食材の現在及び必要な量」の各値をまとめた構造体
    /// メッセージング用に値を一々Tupleで書くと冗長になるのを防ぐ
    /// </summary>
    public struct FoodInfo
    {
        public FoodInfo(FoodType food, int current, int require)
        {
            Food = food;
            Current = current;
            Require = require;
        }

        public FoodType Food { get; private set; }
        public int Current { get; private set; }
        public int Require { get; private set; }
    }

    /// <summary>
    /// ラーメンの情報を受信し、UIに表示する
    /// </summary>
    public class RamenInfoPrinter : MonoBehaviour
    {
        /// <summary>
        /// 「商品名」と「含まれる食材の現在及び必要な量」をメッセージングする際の構造体
        /// </summary>
        public struct Message
        {
            public Message(string name, IEnumerable<FoodInfo> values)
            {
                Name = name;
                Values = values;
            }

            public string Name { get; private set; }
            public IEnumerable<FoodInfo> Values { get; private set; }
        }

        [Header("ラーメンの情報を表示する")]
        [SerializeField] Text _nameText;
        [SerializeField] Text _foodsText;

        void Awake()
        {
            if (_nameText != null) _nameText.text = string.Empty;
            if (_foodsText != null) _foodsText.text = string.Empty;

            // 各値に対して文字列操作をするのでStringBuilderを使う
            StringBuilder builder = new();
            MessageBroker.Default.Receive<Message>()
                .Subscribe(msg => Print(builder, msg)).AddTo(gameObject);
        }

        void Print(StringBuilder builder, Message msg)
        {
            if (_nameText != null) _nameText.text = msg.Name;
            if (_foodsText == null) return;

            // メッセージ中身を複数行の文字列に変換した後に表示
            builder.Clear();
            foreach (FoodInfo value in msg.Values)
            {
                builder.AppendLine($"{EnumExtensions.ToString(value.Food)} {value.Current}/{value.Require}");
            }

            _foodsText.text = builder.ToString();
        }

        /// <summary>
        /// UIに表示
        /// </summary>
        public static void Print(string productName, IEnumerable<FoodInfo> values)
        {
            MessageBroker.Default.Publish(new Message(productName, values));
        }
    }
}