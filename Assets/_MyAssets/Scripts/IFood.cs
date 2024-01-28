using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    /// <summary>
    /// �u�����v�u�ǂꂭ�炢�̗ʁv��\�����邽�߂̍\����
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
    /// ��ނƂǂ�Ԃ肪�ڐG�����ۂɁA���[����������ނ��ǂ�Ԃ�ɓ��������𔻒肷��B
    /// </summary>
    public interface IFood
    {
        /// <summary>
        /// ��ނ����[�����ɒǉ��\�ȏ�ԂȂ�΁u�ǂ̋�ނ��v�u1��Œǉ������ʁv��Ԃ�
        /// </summary>
        public bool TryAddRamen(out Per<FoodType> food);
    }
}
