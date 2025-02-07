using System;
using UnityEngine;

namespace MySampleEx
{
    [Serializable]
    public class UserData
    {
        public int level;
        public int exp;
        public int gold;
        public int mana;
        public int health;
    }
    /// <summary>
    /// 캐릭터 스탯 데이터 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new Stats", menuName = "Stats System/new Character Stats")]
    public class StatsObject : ScriptableObject
    {
        #region Variables
        public Attribute[] attributes;

        [SerializeField] private UserData userData;

        public Action<StatsObject> OnChagnedStats;

        public int Level
        {
            get => userData.level;
            set => userData.level = value;
        }

        public int Exp
        {
            get => userData.exp;
            set => userData.exp = value;
        }

        public int Gold
        {
            get => userData.gold;
            set => userData.gold = value;
        }

        public int Health
        {
            get => userData.health;
            set => userData.health = value;
        }
        public int Mana
        {
            get => userData.mana;
            set => userData.mana = value;
        }

        public int MaxHealth
        {
            get
            {
                int maxHealth = 0;
                foreach (var attribute in attributes)
                {
                    if (attribute.type == CharacterAttribute.Health)
                    {
                        maxHealth = attribute.value.ModifedValue;
                    }
                }
                return maxHealth;
            }
        }

        public float HealthPercentage
        {
            get 
            {
                return (MaxHealth > 0) ? ((float)Health / (float)MaxHealth) : 0f;
            }
        }

        public int MaxMana
        {
            get
            {
                int maxMana = 0;
                foreach (var attribute in attributes)
                {
                    if (attribute.type == CharacterAttribute.Mana)
                    {
                        maxMana = attribute.value.ModifedValue;
                    }
                }
                return maxMana;
            }
        }

        public float ManaPercentage
        {
            get
            {
                return (MaxMana > 0) ? ((float)Mana / (float)MaxMana) : 0f;
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


            SetBaseValue(CharacterAttribute.Agility, 100);
            SetBaseValue(CharacterAttribute.Intellect, 100);
            SetBaseValue(CharacterAttribute.Stamina, 100);
            SetBaseValue(CharacterAttribute.Strength, 100);
            SetBaseValue(CharacterAttribute.Health, 100);
            SetBaseValue(CharacterAttribute.Mana, 100);
            Level = 1;
            Exp = 0;
            Gold = 1000;

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
            Gold += amount;
            Debug.Log("AddGold : " + amount.ToString());
            //스탯 변경시 등록된 함수 호출
            OnChagnedStats?.Invoke(this);
        }

        public bool useGold(int amount)
        {
            if(Gold < amount)
            {
                Debug.Log("소지금 부족");
                return false;
            }
            Gold -= amount;
            //스탯 변경시 등록된 함수 호출
            OnChagnedStats?.Invoke(this);
            return true;
        }

        public bool EnoughGold(int amount)
        {
            return Gold >= amount;
        }

        public bool Addexp(int amount)
        {
            bool isLevelup = false;

            Exp += amount;

            int nowLevel = Level;

            //레벨업 체크
            while(Exp >= GetExpForLevelup(nowLevel))
            {
                Exp -= GetExpForLevelup(nowLevel);
#if NET_MODE
                nowLevel++;
                NetManager.Instance.NetsendUserLevelup();
#else
                Level++;
                nowLevel++;
#endif
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
            return Level * 100;
        }

        #region Save/Load Methods
        public string ToJson()
        {
            return JsonUtility.ToJson(userData);
        }
        public void FromJson(string jsonString)
        {
            userData = JsonUtility.FromJson<UserData>(jsonString);
        }
        #endregion
    }

}
