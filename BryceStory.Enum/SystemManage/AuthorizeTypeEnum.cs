using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BryceStory.Enum.SystemManage
{
    public enum AuthorizeTypeEnum
    {
        [Description("角色")]
        Role = 1,

        [Description("用户")]
        User = 2,
    }
}
