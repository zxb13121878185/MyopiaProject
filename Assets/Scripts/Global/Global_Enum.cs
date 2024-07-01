using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gloabal_EnumCalss
{
    public enum EnemyType
    {
        Apple = 0,
        Orange = 1,
        Grape = 2,
        Lemon = 3,
        Carrot,
        Cherry,
        Kiwi,
        Pumpkin,
        Star,
    }
    public enum EyeType
    {
        None,
        Left,
        Right,
    }
    /// <summary>
    /// http的请求类型
    /// </summary>
    public enum HttpRequestType
    {
        NONE = -1,
        /// <summary>
        /// 修改
        /// </summary>
        MODIFY = 0,
        /// <summary>
        /// 获取
        /// </summary>
        GET = 1,
        DELETE = 2,
        ADD,
        POST,
    }
}
