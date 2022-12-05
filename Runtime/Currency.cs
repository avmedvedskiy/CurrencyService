using System;
using UnityEngine;

namespace Currency
{
    [Serializable]
    public struct Currency
    {
        [SerializeField] private int _id;

        [SerializeField] private int _amount;

        public int Id => _id;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public Currency(int id, int amount)
        {
            _id = id;
            _amount = amount;
        }
    }
}