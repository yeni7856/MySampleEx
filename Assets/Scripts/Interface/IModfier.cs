using UnityEngine;

namespace MySampleEx
{
    public interface IModfier
    {
        //매개변수로 입력받은 변수에 누적한다.
        void AddValue(ref int baseValue);
    }
}
