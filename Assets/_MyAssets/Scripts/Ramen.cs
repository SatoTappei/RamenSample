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
        #region 操作用の型
        // 食材の3Dモデルを列挙型に対応させて操作する。
        [System.Serializable]
        class FoodModelData
        {
            [SerializeField] GameObject _model;
            [SerializeField] FoodType _type;

            public GameObject Model => _model;
            public FoodType Type => _type;
        }

        // 現在の量と必要な量
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

        [Header("各種パラメータの設定")]
        [SerializeField] RamenParameterSettings _settings;
        [Header("具材の追加順番は自由か")]
        [SerializeField] bool _isOrderFree;
        [Header("食材の当たり判定")]
        [SerializeField] Collider _foodCollider;
        [Header("食材の3Dモデル")]
        [SerializeField] FoodModelData[] _foodModelData;

        Dictionary<FoodType, FoodStatus> _contentFoods;
        Dictionary<FoodType, List<GameObject>> _foodModels;

        void Awake()
        {
            // 具材が食材の当たり判定に接触した場合、追加可能な状態ならばラーメンに追加する
            IFood food = default;
            Per<FoodType> per = default;
            _foodCollider.OnTriggerEnterAsObservable()
                .Where(c => c.TryGetComponent(out food))
                .Where(_ => food.TryAddRamen(out per))
                .Subscribe(_ => AddFood(per.Entity, per.Volume));

            // 具材のモデルを全部非表示にしておく
            foreach (FoodModelData m in _foodModelData) m.Model.SetActive(false);

            // 具材の必要量を辞書に登録
            _contentFoods = new();
            foreach (FoodCondition c in _settings.Foods) _contentFoods.Add(c.Food, new(c.Volume));

            // 具材の3Dモデルを辞書に登録
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

        #region 未使用
        void SendMessage(IReadOnlyDictionary<FoodType, int> contentFoods)
        {
            int foodsCount = _settings.Foods.Count;

            FoodInfo[] buffer = ArrayPool<FoodInfo>.Shared.Rent(foodsCount);
            Span<FoodInfo> span = new Span<FoodInfo>(buffer, 0, foodsCount);
            Debug.Log("すぱん"+span.Length);

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

        // 具材を追加し、量に応じたモデルを表示
        void AddFood(FoodType food, int volume)
        {
            if (!_contentFoods.ContainsKey(food)) return;

            _contentFoods[food].Current += volume;

            // 分量とモデルの数が違う場合を考慮してリマップする
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