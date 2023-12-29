using System;
using System.Collections.Generic;
using System.Text;
using XiaoFeng.IO;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-25 11:02:57                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Packets
{
    /// <summary>
    /// Modbus基础包
    /// </summary>
    public abstract class ModbusBasePacket
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusBasePacket()
        {

        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="requestPacket">请求包</param>
        public ModbusBasePacket(ModbusBasePacket requestPacket)
        {
            this.RequestPacket = requestPacket;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 是请求还是响应
        /// </summary>
        public ModbusType ModbusType { get; set; } = ModbusType.Request;
        /// <summary>
        /// 请求类型
        /// </summary>
        private RequestType _RequestType = RequestType.READ;
        /// <summary>
        /// 请求类型
        /// </summary>
        public RequestType RequestType { get => this._RequestType; }
        /// <summary>
        /// 字节序类型
        /// </summary>
        public EndianType EndianType { get; set; } = EndianType.BIG;
        /// <summary>
        /// 主机
        /// </summary>
        public byte Host { get; set; } = 0x01;
        /// <summary>
        /// 功能码
        /// </summary>
        private FunctionCodes _Code;
        /// <summary>
        /// 功能码
        /// </summary>
        public FunctionCodes Code
        {
            get => this._Code;
            set
            {
                this._Code = value;
                if (value > 0)
                {
                    this._RequestType = value.IsDefined<ReadAttribute>() ? RequestType.READ : RequestType.WRITE;
                }
            }
        }
        /// <summary>
        /// 错误码
        /// </summary>
        public ExceptionCodes ErroCode { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public ushort Address { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public ushort Count { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 内存写入器
        /// </summary>
        internal MemoryBufferWriter Writer { get; set; }
        /// <summary>
        /// 内存读取器
        /// </summary>
        internal MemoryBufferReader Reader { get; set; }
        /// <summary>
        /// 校验码
        /// </summary>
        public byte[] VerificationCode { get; set; }
        /// <summary>
        /// 协议类型
        /// </summary>
        public ProtocolType ProtocolType { get; set; }
        /// <summary>
        /// 请求包
        /// </summary>
        public ModbusBasePacket RequestPacket { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToArray();
        /// <summary>
        /// 解包
        /// </summary>
        public abstract void UnPacket();
        /// <summary>
        /// 读流
        /// </summary>
        public abstract Boolean ReaderStream();
        /// <summary>
        /// 写流
        /// </summary>
        public abstract void WriterStream();
        #endregion
    }
}