using System;
using System.Collections.Generic;
using System.Text;
using XiaoFeng.IO;
using XiaoFeng.Modbus.Packets;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-21 16:58:27                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Protocols
{
    /// <summary>
    /// Modbus客户端协议包
    /// </summary>
    public abstract class ModbusRequestPacket: ModbusBasePacket
    {
        #region 构造器
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        public ModbusRequestPacket()
        {
            this.ModbusType = ModbusType.Request;
            this.Writer = new MemoryBufferWriter();
        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="requestPacket">请求包</param>
        public ModbusRequestPacket(byte[] data, ModbusRequestPacket requestPacket) : base(requestPacket)
        {
            this.Reader = new MemoryBufferReader(data);
            this.ModbusType = ModbusType.Response;
            this.UnPacket();
        }
        #endregion

        #region 属性
        
        #endregion

        #region 方法
        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray()
        {
            this.Writer = new MemoryBufferWriter();
            this.WriterStream();
            this.Writer.WriteByte(this.Host);
            this.Writer.WriteByte((byte)this.Code);
            this.Writer.WriteBytes(this.Address.GetBytes(false));

            if (this.RequestType == RequestType.WRITE)
            {
                if (this.Data == null || this.Data.Length == 0) return Array.Empty<byte>();
                if (this.Code == FunctionCodes.WriteCoil)
                {

                }
                else
                {
                    this.Writer.WriteBytes(this.Count.GetBytes(false));
                    var length = this.Data.Length;
                    this.Writer.WriteByte((byte)length);
                }
                this.Writer.WriteBytes(this.Data);
            }
            else
            {
                this.Writer.WriteBytes(this.Count.GetBytes(false));
            }
            return this.Writer.ToArray();
        }
        /// <summary>
        /// 解包
        /// </summary>
        public override void UnPacket()
        {
            if (this.Reader == null || this.Reader.Length == 0) return;
            if (!this.ReaderStream()) return;

            if (this.Reader.RemainingLength < 1) return;
            this.Host = (byte)this.Reader.ReadByte();

            if (this.Reader.RemainingLength < 1) return;
            var code = this.Reader.ReadByte();

            if (code > 0x80)
            {
                this.Code = (FunctionCodes)(code - 0x80);
                this.ErroCode = (ExceptionCodes)this.Reader.ReadByte();
            }
            else
            {
                this.Code = (FunctionCodes)code;
                this.ErroCode = 0;
                if (this.RequestType == RequestType.READ)
                {
                    if (this.Reader.RemainingLength < 1) return;
                    //数据长度
                    var length = this.Reader.ReadByte();
                    /*var RealLength =(int) RequestPacket.Count;
                    if (this.Code == FunctionCodes.ReadCoils || this.Code == FunctionCodes.ReadInputDiscreteQuantity)
                    {
                        length = length / 8 + (length % 8 == 0 ? 0 : 1);
                    }
                    else if (this.Code == FunctionCodes.ReadHoldingRegisters || this.Code == FunctionCodes.ReadInputRegister)
                    {
                        length = 2 * length;
                    }*/
                    if (this.Reader.RemainingLength >= length)
                        this.Data = this.Reader.ReadBytes(length);
                }
                else
                {
                    if (this.Reader.RemainingLength < 2) return;
                    this.Address = this.Reader.ReadBytes(2).ToUInt16();

                    if (this.Reader.RemainingLength < 2) return;
                    this.Count = this.Reader.ReadBytes(2).ToUInt16();
                }
                if (!this.Reader.EndOfStream)
                    this.VerificationCode = this.Reader.ReadBytes();
            }
        }
        #endregion
    }
}