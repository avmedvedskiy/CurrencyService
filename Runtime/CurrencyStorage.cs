using System;
using System.Collections.Generic;
using UnityEngine;

namespace Currency
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Currency/Currency Storage", fileName = "CurrencyStorage",
        order = 0)]
    public class CurrencyStorage : ScriptableObject
    {
        [Serializable]
        public struct CurrencyData
        {
            public int id;
            public string name;
        }
        
        private const string DEFAULT_FOLDER_PATH = "Scripts/Generated";
        private const string DEFAULT_ENUM_FILENAME = "CurrencyEnum";

        public string codeGenFolderPath = DEFAULT_FOLDER_PATH;
        public string codeGenEnumName = DEFAULT_ENUM_FILENAME;
        
        public List<CurrencyData> currencylist;
    }
}