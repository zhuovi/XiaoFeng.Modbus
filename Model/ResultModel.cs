using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-22 14:41:47                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Model
{
    /// <summary>
    /// 结果模型
    /// </summary>
    public class ResultModel
    {
        #region 构造器
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        public ResultModel()
        {

        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="isSuccessed">成功状态</param>
        public ResultModel(Boolean isSuccessed)
        {
            this.IsSuccessed = isSuccessed;
        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        public ResultModel(string errorMessage)
        {
            this.IsSuccessed = false;
            this.ErrorMessage = errorMessage;
        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误码</param>
        public ResultModel(string errorMessage, ExceptionCodes errorCode) : this(errorMessage)
        {
            this.ErrorCode = errorCode;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 错误码
        /// </summary>
        public ExceptionCodes ErrorCode { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public Boolean IsSuccessed { get; set; } = true;
        /// <summary>
        /// 请求报文
        /// </summary>
        public byte[] Request { get; set; }
        /// <summary>
        /// 请求报文16进制
        /// </summary>
        public string[] RequestHex => Request == null ? Array.Empty<string>() : Request.Select(a => a.ToString("X2")).ToArray();
        /// <summary>
        /// 响应报文
        /// </summary>
        public byte[] Response { get; set; }
        /// <summary>
        /// 响应报文16进制
        /// </summary>
        public string[] ResponseHex => Response == null ? Array.Empty<string>() : Response.Select(a => a.ToString("X2")).ToArray();
        #endregion

        #region 方法
        /// <summary>
        /// 错误消息
        /// </summary>
        /// <param name="errorMessage">消息</param>
        /// <returns></returns>
        public static ResultModel Error(string errorMessage)
        {
            return new ResultModel(errorMessage);
        }
        /// <summary>
        /// 错误消息
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static ResultModel Error(string errorMessage,ExceptionCodes errorCode)
        {
            return new ResultModel(errorMessage, errorCode);
        }
        #endregion
    }
    /// <summary>
    /// 结果模型
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class ResultModel<T> : ResultModel
    {
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        public ResultModel() { }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="isSuccessed">成功状态</param>
        public ResultModel(Boolean isSuccessed) : base(isSuccessed) { }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        public ResultModel(string errorMessage) : base(errorMessage) { }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误码</param>
        public ResultModel(string errorMessage, ExceptionCodes errorCode) : base(errorMessage, errorCode) { }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="value">值</param>
        public ResultModel(T value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns></returns>
        public static new ResultModel<T> Error(string errorMessage)
        {
            return new ResultModel<T>(errorMessage);
        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="errorCode">错误码</param>
        /// <returns></returns>
        public static new ResultModel<T> Error(string errorMessage,ExceptionCodes errorCode)
        {
            return new ResultModel<T>(errorMessage, errorCode);
        }
    }
}