using UnityEngine;
using System.Collections.Generic;

using Players;


namespace Games.Conditions
{
    public abstract class GCBase : ScriptableObject
    {
		public abstract Player Pass(IEnumerable<Player> players);

        protected bool CompareByCondition(int count1, int count2, string condition)
        {
            switch (condition)
            {
                case ">":
                    return count1 > count2;

                case "<":
                    return count1 < count2;

                case ">=":
                    return count1 >= count2;

                case "<=":
                    return count1 <= count2;

                case "==":
                    return count1 == count2;

                case "!=":
                    return count1 != count2;
            }

            return false;
        }
    }
}
