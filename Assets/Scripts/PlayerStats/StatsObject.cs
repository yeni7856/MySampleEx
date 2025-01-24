using System;
using UnityEngine;

namespace MySampleEx
{
    /// <summary>
    /// 캐릭터 스탯 데이터 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new Stats", menuName = "Stats System/new Character Stats")]
    public class StatsObject : ScriptableObject
    {
        #region Variables
        public Attribute[] attributes;

        [SerializeField] private int level;
        [SerializeField] private int exp;
        [SerializeField] private int gold;

        public Action<StatsObject> OnChagnedStats;

        public int Level => level;
        public int Exp => exp;
        public int Gold => gold;

        public int Health {  get;  set; }
        public int Mana { get;  set; }

        public float HealthPercentage
        {
            get 
            {
                int health = Health;
                int maxHealth = health;
                foreach(var attribute in attributes)
                {
                    if(attribute.type == CharacterAttribute.Health)
                    {
                        maxHealth = attribute.value.ModifedValue;
                    }
                }
                return (maxHealth > 0) ? ((float)health / (float)maxHealth) : 0f;
            }
        }
        public float ManaPercentage
        {
            get
            {
                int mana = Mana;
                int maxMana = mana;
                foreach (var attribute in attributes)
                {
                    if (attribute.type == CharacterAttribute.Mana)
                    {
                        maxMana = attribute.value.ModifedValue;
                    }
                }
                return (maxMana > 0) ? ((float)mana / (float)maxMana) : 0f;
            }
        }
        //최초 1회 초기화 체크
        [NonSerialized]
        private bool isInitailized = false;
        #endregion

        private void OnEnable()
        {
            initializeAttributes();
        }

        private void initializeAttributes()
        {
            if (isInitailized)
                return;
            isInitailized = true;
            Debug.Log("initialize Attributes");

            foreach (var attribute in attributes)
            {
                //attribute의 객체 생성
                attribute.value = new ModifiableInt(OnModifiedValue);
            }

            level = 1;
            exp = 0;
            gold = 100;

            SetBaseValue(CharacterAttribute.Agility, 100);
            SetBaseValue(CharacterAttribute.Intellect, 100);
            SetBaseValue(CharacterAttribute.Stamina, 100);
            SetBaseValue(CharacterAttribute.Strength, 100);
            SetBaseValue(CharacterAttribute.Health, 100);
            SetBaseValue(CharacterAttribute.Mana, 100);

            //Current Health, Mana 초기화
            Health = GetModifiredValue(CharacterAttribute.Health);
            Mana = GetModifiredValue(CharacterAttribute.Mana);
        }

        //속성값 초기화
        private void SetBaseValue(CharacterAttribute type, int value)
        {
            foreach(var attribute in attributes)
            {
                if (attribute.type == type)
                {
                    attribute.value.BaseValue = value;
                }
            }
        }

        //기본 속성값 가져오기 
       public int GetBaseValue(CharacterAttribute type)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == type)
                {
                    return attribute.value.BaseValue;
                }
            }
            return -1;
        }

        //최종 속성값 가져오기
        public int GetModifiredValue(CharacterAttribute type)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == type)
                {
                    return attribute.value.ModifedValue;
                }
            }
            return -1;
        }

        // attribute의 value값이 변경되면 호출 되는 함수
        private void OnModifiedValue(ModifiableInt value)
        {
            //스탯 변경시 등록된 함수 호출
            OnChagnedStats?.Invoke(this);

            return;
        }

        public void AddGold(int amount)
        {
            gold += amount;
            Debug.Log("AddGold : " + amount.ToString());
            //스탯 변경시 등록된 함수 호출
            OnChagnedStats?.Invoke(this);
        }

        public bool useGold(int amount)
        {
            if(gold < amount)
            {
                Debug.Log("소지금 부족");
                return false;
            }
            gold -= amount;
            //스탯 변경시 등록된 함수 호출
            OnChagnedStats?.Invoke(this);
            return true;
        }

        public bool Addexp(int amount)
        {
            bool isLevelup = false;

            exp += amount;

            //레벨업 체크
            while(exp >= GetExpForLevelup(level))
            {
                exp -= GetExpForLevelup(level);
                level++;
                //레벨업 보상
                //...
                isLevelup = true;
            }
            OnChagnedStats?.Invoke(this);
            return isLevelup;
        }

        //지정한 레벨에서 다음 레벨로 가는데 필요한 경험치값 반환
        public int GetExpForLevelup(int nowLevel)
        {
            return level * 100;
        }
    }

}
