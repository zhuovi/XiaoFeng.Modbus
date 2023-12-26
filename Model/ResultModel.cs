using System;
using System.Collections.Generic;
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
    /// ResultModel 类说明
    /// </summary>
    public class ResultModel
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ResultModel()
        {

        }
        public ResultModel(Boolean isSuccessed)
        {
            this.IsSuccessed = isSuccessed;
        }
        public ResultModel(string errorMessage)
        {
            this.IsSuccessed = false;
            this.ErrorMessage = errorMessage;
        }
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
        /// 响应报文
        /// </summary>
        public byte[] Response { get; set; }
        #endregion

        #region 方法
        public static ResultModel Error(string errorMessage)
        {
            return new ResultModel(errorMessage);
        }
        public static ResultModel Error(string errorMessage,ExceptionCodes errorCode)
        {
            return new ResultModel(errorMessage, errorCode);
        }
        #endregion
    }
    public class ResultModel<T> : ResultModel
    {
        public ResultModel() { }

        public ResultModel(Boolean isSuccessed) : base(isSuccessed) { }
        public ResultModel(string errorMessage) : base(errorMessage) { }
        public ResultModel(string errorMessage, ExceptionCodes errorCode) : base(errorMessage, errorCode) { }
        public ResultModel(T value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
        public static ResultModel<T> Error(string errorMessage)
        {
            return new ResultModel<T>(errorMessage);
        }
        public static ResultModel<T> Error(string errorMessage,ExceptionCodes errorCode)
        {
            return new ResultModel<T>(errorMessage, errorCode);
        }
    }
}