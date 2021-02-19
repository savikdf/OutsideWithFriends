using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public class DebugCol {
        //TODO add HEX style color coding
        public static void Log(Color c, string message){
            c.r = (c.r > 1) ? c.r/255 : c.r;
            c.g = (c.g > 1) ? c.g/255 : c.g;
            c.b = (c.b > 1) ? c.b/255 : c.b;
            
            if(GameManger.singleton.isDebug)
                Debug.Log (string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(c.r * 255f), (byte)(c.g * 255f), (byte)(c.b * 255f), message));
        }
    }
}
