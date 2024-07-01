using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gloabal_EnumCalss
{

    #region 无界眼镜

    /// <summary>
    /// 视觉模式
    /// </summary>
    public enum Enum_VISION_MODE
    {
        /// <summary>
        /// 单目
        /// </summary>
        Monocular,
        /// <summary>
        /// 双目
        /// </summary>
        Binocular
    }
    /// <summary>
    /// 眼镜镜片模组类型
    /// </summary>
    public enum Enum_MODULE_TYPE
    {
        Taichi,
        Taichi_Pico,
        Pico,
        module_55070,
        BOE
    }
    #endregion
}
