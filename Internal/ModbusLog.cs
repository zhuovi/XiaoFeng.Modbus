using System;
using System.Collections.Generic;
using System.Text;
using XiaoFeng.Log;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-13 16:32:31                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Internal
{
    /// <summary>
    /// Modbus 日志类
    /// </summary>
    public class ModbusLog : IModbusLog
    {
        #region 构造器
        /// <summary>
        /// 初始化 <see cref="ModbusLog"/> 类的实例
        /// </summary>
        public ModbusLog() : this(true) { }
        /// <summary>
        /// 初始化 <see cref="ModbusLog"/> 类的实例
        /// </summary>
        /// <param name="isDebug">是否启用调试</param>
        public ModbusLog(Boolean isDebug) { this.IsDebug = isDebug; }
        #endregion

        #region 属性
        /// <summary>
        /// 是否启用调试
        /// </summary>
        public Boolean IsDebug { get; set; } = true;
        #endregion

        #region 方法
        private void Info(LogType logType, string message)
        {
            if (this.IsDebug)
                Console.WriteLine($"[{logType.GetDescription()}]  {message} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff}");
        }
        /// <summary>
        /// 输出控制台信息
        /// </summary>
        /// <param name="message">信息</param>
        public void Debug(string message)
        {
            this.Info(LogType.Debug, message);
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message">消息</param>
        public void Information(string message)
        {
            this.Info(LogType.Info, message);
            LogHelper.Info(message);
        }
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="message">消息</param>
        public void Error(Exception ex, string message = "")
        {
            this.Info(LogType.Error, (message.IsNullOrEmpty() ? "" : (message + "[")) + ex.Message);
            LogHelper.Error(ex, message);
        }
        #endregion
    }
}