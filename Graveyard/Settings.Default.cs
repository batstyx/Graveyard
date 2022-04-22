using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.Graveyard
{
    public sealed partial class Settings
    {
        private int GetDefaultIntValue(string key) => int.Parse(Properties[key].DefaultValue.ToString());
        public int DefaultPlayerLeft => GetDefaultIntValue("PlayerLeft");
        public int DefaultPlayerTop => GetDefaultIntValue("PlayerTop");

        public void ResetPlayerPosition()
        {
            PlayerLeft = DefaultPlayerLeft;
            PlayerTop = DefaultPlayerTop;
        }

        public int DefaultOpponentLeft => GetDefaultIntValue("EnemyLeft");
        public int DefaultOpponentTop => GetDefaultIntValue("EnemyTop");

        public void ResetOpponentPosition()
        {
            EnemyLeft = DefaultOpponentLeft;
            EnemyTop = DefaultOpponentTop;
        }

        private double GetDefaultDoubleValue(string key) => double.Parse(Properties[key].DefaultValue.ToString());
        public double DefaultPlayerOpacity => GetDefaultDoubleValue("FriendlyOpacity");
        public double DefaultPlayerScale => GetDefaultDoubleValue("FriendlyScale");

        public void ResetPlayerDisplay()
        {
            FriendlyOpacity = DefaultPlayerOpacity;
            FriendlyScale = DefaultPlayerScale;
        }

        public double DefaultOpponentOpacity => GetDefaultDoubleValue("EnemyOpacity");
        public double DefaultOpponentScale => GetDefaultDoubleValue("EnemyScale");

        public void ResetOpponentDisplay()
        {
            EnemyOpacity = DefaultOpponentOpacity;
            EnemyScale = DefaultOpponentScale;
        }
    }
}
