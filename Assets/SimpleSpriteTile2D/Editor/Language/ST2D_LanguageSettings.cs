using System.Collections.Generic;
using UnityEngine;

namespace moogle.SmartTile2D
{
    public static class Language
    {
        static Dictionary<string, string> cnlangugeDict = new Dictionary<string, string>(){
                {"Create Prefab", "创建预设"},
                {"Prefab Type", "预设类型"},
                {"Select Tile Type", "选择Tile类型"},
                {"Output Prefab Name:","预设保存为:"},
                {"Select Sprite:","选择图片素材:"},
                {"Add Collider:","为预设添加碰撞器/触发器:"},
                {"Custom Material","自定义材质(Material)"},
                {"Select Multi Texture2D:","选择Texture2D素材图集"},
                {"Prefab Output Path:","预设输出路径"},
                {"Update Order","更新渲染层级" },
                {"Recover Order","还原渲染层级"},
                {"Select Prefab Type","选择预设类型"},
                {"All sorting orders in this panel will be rewritten by Y Axis,When you press [Update Order].\n",
                    "点击[更新渲染层级]后，所有子项的渲染层级都会依据Y轴重写。\n\n" },
                {"If you still want to use the original sorting orders defined in prefab,\n Please Press [Recover Order]",
                    "如果你需要使用原始prefab中预设的渲染层级,\n请点击[还原渲染层级]" },

                {"1). First Set up your sprite with Sprite Editor in [Project Window] ","1). 首先在Project界面中设定sprite的pixelperunit和pivot"},
                {"    Make sure the pivot & sprite are correct in scene display.","    确认sprite的大小在场景中正合适。"},
                {"2). Drag Sprite on this window and Set pixelPerUnit of your sprite. ","把sprite拖拽到本界面，把pixelperunit填入"},
                {"    Set width of this prefab","    并设定预设的宽度"},
                {"3). Create Prefab :XD","3). 点击Create Prefab"}};

        public static string GetLanguage(string str)
        {
            string dst = str;
            if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                if (cnlangugeDict.ContainsKey(str))
                {
                    return cnlangugeDict[str];
                }

            }
            return dst;

        }
    }
}


