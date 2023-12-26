using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-13 17:25:33                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// ModbusLog 接口
    /// </summary>
    public interface IModbusLog
    {
        #region 属性
        /// <summary>
        /// 是否启用调试
        /// </summary>
        Boolean IsDebug { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 输出控制台信息
        /// </summary>
        /// <param name="message">信息</param>
        void Debug(string message);
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message">消息</param>
        void Information(string message);
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="message">消息</param>
        void Error(Exception ex, string message = "");
        #endregion
    }
}