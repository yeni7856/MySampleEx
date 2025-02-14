using UnityEngine;
using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace MySampleEx
{
    /// <summary>
    /// 가챠 데이터 리스트
    /// </summary>
    [Serializable]
    public class GachaItems
    {
        public List<GachaItem> GachaItmes { get; set; }
    }

    /// <summary>
    /// 가챠 데이터 클래스
    /// </summary>
    [Serializable]
    public class GachaItem
    {
        public int number {get; set;}
        public string name { get; set; }
        public int rate { get; set; }
    }
}
