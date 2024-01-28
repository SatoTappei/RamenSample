using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UniRx;

namespace PSB.Ramen
{
    /// <summary>
    /// �u�܂܂��H�ނ̌��݋y�ѕK�v�ȗʁv�̊e�l���܂Ƃ߂��\����
    /// ���b�Z�[�W���O�p�ɒl����XTuple�ŏ����Ə璷�ɂȂ�̂�h��
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
    /// ���[�����̏�����M���AUI�ɕ\������
    /// </summary>
    public class RamenInfoPrinter : MonoBehaviour
    {
        /// <summary>
        /// �u���i���v�Ɓu�܂܂��H�ނ̌��݋y�ѕK�v�ȗʁv�����b�Z�[�W���O����ۂ̍\����
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

        [Header("���[�����̏���\������")]
        [SerializeField] Text _nameText;
        [SerializeField] Text _foodsText;

        void Awake()
        {
            if (_nameText != null) _nameText.text = string.Empty;
            if (_foodsText != null) _foodsText.text = string.Empty;

            // �e�l�ɑ΂��ĕ����񑀍������̂�StringBuilder���g��
            StringBuilder builder = new();
            MessageBroker.Default.Receive<Message>()
                .Subscribe(msg => Print(builder, msg)).AddTo(gameObject);
        }

        void Print(StringBuilder builder, Message msg)
        {
            if (_nameText != null) _nameText.text = msg.Name;
            if (_foodsText == null) return;

            // ���b�Z�[�W���g�𕡐��s�̕�����ɕϊ�������ɕ\��
            builder.Clear();
            foreach (FoodInfo value in msg.Values)
            {
                builder.AppendLine($"{EnumExtensions.ToString(value.Food)} {value.Current}/{value.Require}");
            }

            _foodsText.text = builder.ToString();
        }

        /// <summary>
        /// UI�ɕ\��
        /// </summary>
        public static void Print(string productName, IEnumerable<FoodInfo> values)
        {
            MessageBroker.Default.Publish(new Message(productName, values));
        }
    }
}