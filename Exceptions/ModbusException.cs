using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-13 15:13:11                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Exceptions
{
    /// <summary>
    /// 异常类
    /// </summary>
    public class ModbusException : Exception
    {
        #region 构造器
        /// <summary>
        /// 初始化 <see cref="ModbusException"/> 类的新实例。
        /// </summary>
        public ModbusException() : base() { }
        /// <summary>
        /// 用指定的错误消息初始化 <see cref="ModbusException"/> 类的新实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误消息</param>
        public ModbusException(string message) : base(message) { }
        /// <summary>
        /// 使用指定的错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="ModbusException"/> 类的新实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误消息</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用</param>
        public ModbusException(string message, Exception innerException) : base(message, innerException) { }
        /// <summary>
        /// 用序列化数据初始化 <see cref="ModbusException"/> 类的新实例。
        /// </summary>
        /// <param name="info">包含有关所引发异常的序列化对象数据的 <see cref="System.Runtime.Serialization.SerializationInfo"/> 。</param>
        /// <param name="context"><see cref="System.Runtime.Serialization.StreamingContext"/> ，它包含关于源或目标的上下文信息。</param>
        protected ModbusException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion

        #region 属性

        #endregion

        #region 方法

        #endregion
    }
}