using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Cinemachine.CinemachineFreeLookModifier;

namespace MySampleEx
{
    /// <summary>
    /// 캐릭터 속성 Value값을 관리하는 클래스
    /// </summary>
    [Serializable]
    public class ModifiableInt
    {
        #region Variables
        [NonSerialized]
        private int baseValue;      //기본값
        [SerializeField]
        private int modifedValue;   //수정된 값, 최종값

        public int BaseValue
        {
            get { return baseValue; }
            set { 
                baseValue = value;
                UpdateModifedValue();
            }
        }
        public int ModifedValue
        {
            get { return modifedValue; }
            set { modifedValue = value;}
        }
        //modifedValue값 변경시 등록된 함수 실행
        private event Action<ModifiableInt> OnModifedValue; 

        private List<IModfier> modifiers = new List<IModfier>();

        #endregion

        //생성자 - 값 변경시 호출할 함수를 매개변수로 받아 등록
        public ModifiableInt(Action<ModifiableInt> method)
        {
            ModifedValue = baseValue;
            ResisterModEvent(method);
        }

        public void ResisterModEvent(Action<ModifiableInt> method)
        {
            if(method != null)
            {
                OnModifedValue += method;
            }
        }
        public void UnRegisterModEvent(Action<ModifiableInt> method)
        {
            if (method != null)
            {
                OnModifedValue -= method;
            }
        }


        //modifedValue값 구하기,
        private void UpdateModifedValue()
        {
            int valueToAdd = 0;
            foreach (var modifier in modifiers)
            {
                modifier.AddValue(ref valueToAdd);
            }
            ModifedValue = baseValue + valueToAdd;

            //modifedValue값이 변경시 등록된 함수 호출
            OnModifedValue?.Invoke(this);
        }
        public void AddModifier(IModfier modifier)
        {
            modifiers.Add(modifier);
            UpdateModifedValue();
        }
        public void RemoveModifier(IModfier modifier)
        {
            modifiers.Remove(modifier);
            UpdateModifedValue();
        }
    }

}
