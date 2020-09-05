using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BryceStory.Enum.SystemManage
{
    public enum MenuTypeEnum
    {
        [Description("目录")]
        Directory = 1,

        [Description("页面")]
        Menu = 2,

        [Description("按钮")]
        Button = 3
    }
}
