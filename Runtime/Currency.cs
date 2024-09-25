using System;
using UnityEngine;

namespace Currency
{
    [Serializable]
    public struct Currency
    {
        [SerializeField] private int _id;

        [SerializeField] private int _a;

        public int Id => _id;

        public int Amount
        {
            get => _a;
            set => _a = value;
        }

        public Currency(int id, int amount)
        {
            _id = id;
            _a = amount;
        }
    }
}