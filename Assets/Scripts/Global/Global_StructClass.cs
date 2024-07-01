using Gloabal_EnumCalss;
using MRLessonrender;
using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Global_StructClass
{

    #region HTTP数据库

    #region 作为参数


    /// <summary>
    /// Http请求参数基类
    /// </summary>
    public class HtReqParam
    {
    }
    public class HtP_LoginOut: HtReqParam
    {
        public string Authorization;
    }
    /// <summary>
    /// 登录发送消息
    /// </summary>
    public class HtP_Login : HtReqParam
    {
        public string password;
        public string userName;
        public string verifyCode;
    }
    public class HtP_UpdatePW:HtReqParam
    {
        public string curpwd;
        public string pwd;
    }
    public class Htp_Regis:HtReqParam
    {
        public string uname;//用户名
        public string mobile;//电话号码
        public string passwd;//
        public string hospital;
    }
    #endregion

    #region 作为返回结果


    public class HtR_BaseList
    {
        public int total;
        public int page;
        public int pageSize;
    }


    public class HtR_Regis
    {
        public int register; //1 成功，0失败
    }
    /// <summary>
    /// 返回的用户登录信息
    /// </summary>
   // [Serializable]
    public class HtR_Login
    {
        public string authorization;
        public UserSetInfo userSetInfo;
    }

    public class HtR_PModifyPW
    {

    }

    /// <summary>
    /// Http响应结果基类,T类型的都是对应web服务器的类都以“HtR_”开头命名
    /// HtR_XXX，其中xxx为对应访问Htt的url名，如api/v1/role中的role对应的类为HtR_Role
    /// 如果除了"HtR_"后面还有“_”分隔符的都统一替换成“/”，如“HtR_Book_ChapterList”
    /// 最后hi转成“bool/chapter/list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class HtResResult<T>
    {
        /// <summary>
        /// OK:200,201:Create,401"Unauthorized，403:Forbidden，404：Not Found
        /// </summary>
        public int code;
        public T data;
        public string msg;
     //   public bool success;

        /// <summary>
        /// Http响应结果基类,T类型的都是对应web服务器的类都以“HtR_”开头命名
        /// HtR_XXX，其中xxx为对应访问Htt的url名，如api/v1/role中的role对应的类为HtR_Role
        /// 如果除了"HtR_"后面还有“_”分隔符的都统一替换成“/”，如“HtR_Book_ChapterList”
        /// 最后hi转成“bool/chapter/list
        /// </summary>
        /// <returns></returns>
        public string GetHttpURLSuffixName()
        {
            const string tempHeader = "HtR_";
            const string tempEndStr = "list";
            const string tempSplitOld = "_";
            const string tempSplitNew = "/";
            string tempStr = string.Empty;
            tempStr = typeof(T).Name;
            if (!tempStr.Contains(tempHeader))
            {
                string tempErro = "该类不包含" + tempHeader + "的头";
                //  Debug.LogError(tempErro);
                throw new Exception(tempErro);
            }
            int tempLength = tempStr.Length - tempHeader.Length;
            tempStr = tempStr.Substring(tempHeader.Length, tempLength).ToLower();
            //判断中间的是否有分隔符，并进行替换
            tempStr = tempStr.Replace(tempSplitOld, tempSplitNew);
            //判断是否有List作为结尾
            if (tempStr.EndsWith(tempEndStr))
            {
                //在含list的位置开始插入'/';
                tempStr = tempStr.Insert(tempStr.Length - tempEndStr.Length, "/");
            }
            return tempStr;

        }
    }

    #endregion

    #region 用户基础数据

    /// <summary>
    /// 游戏难度
    /// </summary>
    public class Data_GameLevel
    {
        public int difficult;
        public string gameCode;
    }

    public class User_Patient
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int versionId { get; set; }//版本号
        public string userName { get; set; }//用户名
        public string password { get; set; }//密码
        public int age { get; set; }//年龄
        public float nakedVisionLeft { get; set; }//裸眼视力
        public float nakedVisionRight { get; set; }
        public float correctedVisionLeft { get; set; }//矫正视力
        public float correctedVisionRight { get; set; }
        public float nearStereoVision { get; set; }//近立体视
        public float farStereoVision { get; set; }//远立体视
        public override string ToString()
        {
            return string.Format("[User_Patient: Id={0}, Age={1},  NakedVision_left={2}, " +
                "NakedVision_right={3}],CorrectedVision_left={4},CorrectedVision_right={5},NearStereoVision={6},FarStereoVision={7}",
                id, age, nakedVisionLeft, nakedVisionRight, correctedVisionLeft, correctedVisionRight, nearStereoVision, farStereoVision);
        }
    }
    /// <summary>
    /// 用户设置的参数
    /// </summary>
    public class UserSetInfo
    {
        public float brightLeft { get; set; }//左眼亮度
        public float brightRight { get; set; }//右眼亮度
        public float contrastLeft { get; set; }//左眼对比度
        public float contrastRight { get; set; }//右眼对比度
        public float saturationLeft { get; set; }//左眼饱和度
        public float saturationRight { get; set; }//右眼饱和度
    }
    /// <summary>
    /// 上传到后台的的用户参数
    /// </summary>
    public class UpLoadUserInfo
    {
        public string account;
        public string deviceCode;
        public string requestId;
        public UserSetInfo userSetInfo;
    }
    #endregion

    #region 水果射击游戏

    /// <summary>
    /// 一次训练的数据
    /// </summary>
    public class Data_Fruits
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int userId { get; set; }
        public string timeStart { get; set; }//训练开始时间
        public float bulletSize { get; set; }//子弹大小
        public float bulletSpeed { get; set; }//子弹速度
        public string gameCode { get; set; }//游戏编码
        public string deviceCode { get; set; }//设备的唯一编码
        public int difficult { get; set; }//本次训练的难度
        public int interesting { get; set; }//兴趣
        public string requestId { get; set; }//时间戳+6位随机数
        public int experience { get; set; }//体验
        public int expect { get; set; }//期待
        public string timeLen { get; set; }//本次训练的时长
        public float hitRatio { get; set; }//命中率（命中的子弹数/子弹总数）
        public string[] subHitRatio { get; set; }//9个视觉区域的分命中率
        //public override string ToString()
        //{
        //    return string.Format("[User_Patient: Id={0}, Difficult={1},  Interest={2}, " +
        //        "NakedVision_right={3}],CorrectedVision_left={4},CorrectedVision_right={5},NearStereoVision={6},FarStereoVision={7}",
        //        Id, Age, NakedVision_left, NakedVision_right, CorrectedVision_left, CorrectedVision_right, NearStereoVision, FarStereoVision);
        //}
        public List<BulletInfo> bulletInfo;
        public List<EyeInfo> eyeInfo;
        public UserSetInfo userSetInfo;
        public Data_Fruits()
        {
            bulletInfo = new List<BulletInfo>();
            eyeInfo = new List<EyeInfo>();
            subHitRatio = new string[9];
        }
    }

    /// <summary>
    /// 子弹信息
    /// </summary>
    public class BulletInfo
    {
        //[PrimaryKey, AutoIncrement]
        //public int id { get; set; }
        public int userId { get; set; }
        public int trainId { get; set; }
        public int indexOrder { get; set; }//生成的子弹的序号
        public string timeSpawn { get; set; }//出生的时间
        public string posSpawn { get; set; }//出生的位置格式：1，2，3
        public string directSpawn { get; set; }//出生的方向
        public string posDisappear { get; set; }//消失的位置
        public string timeDisappear { get; set; }//消失的时间
        public int isHit { get; set; }//是否击中
        public string duration { get; set; }//子弹存在的时长
    }

    /// <summary>
    /// 眼动信息
    /// </summary>
    public class EyeInfo
    {
        //[PrimaryKey, AutoIncrement]
        //public int id { get; set; }
        public int indexNums { get; set; }//第几次检测
        public int userId { get; set; }
        public int trainId { get; set; }
        public string timeCur { get; set; }//当前时间
        public bool blinkLeft { get; set; }//左眼是否眨眼
        public bool blinkRight { get; set; }//右眼是否眨眼
        public string direction { get; set; }//眼睛看向的方向
        public string posHead { get; set; }//头部位置
        public string rotHead { get; set; }//头部朝向
    }
    /// <summary>
    /// 视觉区域
    /// </summary>
    [Serializable]
    public class VisionRegion
    {
        public int posIndex;//位置编号
        /// <summary>
        /// 该区域当前生成的总个数
        /// </summary>
        public int Num;
        /// <summary>
        /// 当前命中的个数
        /// </summary>
        public int hitNum;
        /// <summary>
        /// 命中率0~1,默认为1
        /// </summary>
        public float hitRate;
        /// <summary>
        /// 间隔时长
        /// </summary>
        public float timeInterval;
    }
    //测试数据
    public class Person
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: Id={0}, Name={1},  Surname={2}, Age={3}]", Id, Name, Surname, Age);
        }
    }

    #endregion

    #region 气球挑战

    public class Data_Ballon
    {
        public int amblyopiaInde;//哪个眼睛是弱视眼，0为左眼，1为右眼
        public int difficult;//0~2分别为简单、中等和困难	
        public string gameCode;//游戏编码
        public string deviceCode;//设备的唯一编码
        public int numHitAll;//气球击破总数量
        //public int numHitBlue;//蓝色气球（双眼目标）击破数	
        public int numHitRed;//红气球（单眼目标）击破数
        public string requestId;//	请求id,每次唯一
        public UserSetInfo userSetInfo;//用户当前设置参数
    }

    #endregion

    #region 划船

    public class Data_Forest
    {
        public int amblyopiaInde;//哪个眼睛是弱视眼，0为左眼，1为右眼
        public int difficult;//0~2分别为简单、中等和困难	
        public string gameCode;//游戏编码
        public string deviceCode;//设备的唯一编码
        public int numAllBlue;//
        public int numAllRed;
        public int numCollectBlue;
        public int numCollectRed;
        public string requestId;//	请求id,每次唯一
        public UserSetInfo userSetInfo;//用户当前设置参数
    }

    #endregion

    #endregion
}
