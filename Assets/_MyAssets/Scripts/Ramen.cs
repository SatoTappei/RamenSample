using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UniRx;
using UniRx.Triggers;
using System.Buffers;
using System;

namespace PSB.Ramen
{
    public class Ramen : MonoBehaviour
    {
        #region ����p�̌^
        // �H�ނ�3D���f����񋓌^�ɑΉ������đ��삷��B
        [System.Serializable]
        class FoodModelData
        {
            [SerializeField] GameObject _model;
            [SerializeField] FoodType _type;

            public GameObject Model => _model;
            public FoodType Type => _type;
        }

        // ���݂̗ʂƕK�v�ȗ�
        class FoodStatus
        {
            int _current;

            public FoodStatus(int required)
            {
                _current = 0;
                Required = required;
            }

            public int Current 
            {
                get => _current;
                set
                {
                    _current = value;
                    _current = Mathf.Clamp(_current, 0, Required);
                }
            }

            public int Required { get; private set; }

            public bool IsFill => Current >= Required;
        }
        #endregion

        [Header("�e��p�����[�^�̐ݒ�")]
        [SerializeField] RamenParameterSettings _settings;
        [Header("��ނ̒ǉ����Ԃ͎��R��")]
        [SerializeField] bool _isOrderFree;
        [Header("�H�ނ̓����蔻��")]
        [SerializeField] Collider _foodCollider;
        [Header("�H�ނ�3D���f��")]
        [SerializeField] FoodModelData[] _foodModelData;

        Dictionary<FoodType, FoodStatus> _contentFoods;
        Dictionary<FoodType, List<GameObject>> _foodModels;

        void Awake()
        {
            // ��ނ��H�ނ̓����蔻��ɐڐG�����ꍇ�A�ǉ��\�ȏ�ԂȂ�΃��[�����ɒǉ�����
            IFood food = default;
            Per<FoodType> per = default;
            _foodCollider.OnTriggerEnterAsObservable()
                .Where(c => c.TryGetComponent(out food))
                .Where(_ => food.TryAddRamen(out per))
                .Subscribe(_ => AddFood(per.Entity, per.Volume));

            // ��ނ̃��f����S����\���ɂ��Ă���
            foreach (FoodModelData m in _foodModelData) m.Model.SetActive(false);

            // ��ނ̕K�v�ʂ������ɓo�^
            _contentFoods = new();
            foreach (FoodCondition c in _settings.Foods) _contentFoods.Add(c.Food, new(c.Volume));

            // ��ނ�3D���f���������ɓo�^
            _foodModels = new();
            foreach (FoodModelData m in _foodModelData)
            {
                if (!_foodModels.ContainsKey(m.Type)) _foodModels.Add(m.Type, new());
                _foodModels[m.Type].Add(m.Model);
            }
        }

        void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {

            }
        }

        #region ���g�p
        void SendMessage(IReadOnlyDictionary<FoodType, int> contentFoods)
        {
            int foodsCount = _settings.Foods.Count;

            FoodInfo[] buffer = ArrayPool<FoodInfo>.Shared.Rent(foodsCount);
            Span<FoodInfo> span = new Span<FoodInfo>(buffer, 0, foodsCount);
            Debug.Log("���ς�"+span.Length);

            for (int i = 0; i < foodsCount; i++)
            {
                int current = contentFoods.TryGetValue(_settings.Foods[i].Food, out int value) ? value : 0;
                int require = _settings.Foods[i].Volume;
                span[i] = new(_settings.Foods[i].Food, current, require);
            }

            RamenInfoPrinter.Print(_settings.ProductName, buffer);

            ArrayPool<FoodInfo>.Shared.Return(buffer);
        }
        #endregion

        // ��ނ�ǉ����A�ʂɉ��������f����\��
        void AddFood(FoodType food, int volume)
        {
            if (!_contentFoods.ContainsKey(food)) return;

            _contentFoods[food].Current += volume;

            // ���ʂƃ��f���̐����Ⴄ�ꍇ���l�����ă��}�b�v����
            float remap = math.remap(0, _contentFoods[food].Required, 
                0, _foodModels[food].Count, _contentFoods[food].Current);
            int floor = Mathf.FloorToInt(remap);
            for (int i = 0; i < _foodModels[food].Count; i++)
            {
                _foodModels[food][i].SetActive(i < floor);
            }
        }
    }
}