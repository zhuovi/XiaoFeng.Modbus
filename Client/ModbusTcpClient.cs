using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XiaoFeng.IO;
using XiaoFeng.Modbus.Internal;
using XiaoFeng.Modbus.Model;
using XiaoFeng.Modbus.Protocols;
using XiaoFeng.Net;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-13 15:27:27                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Client
{
    /// <summary>
    /// Modbus客户端
    /// </summary>
    public class ModbusTcpClient : Disposable, IModbusClient
    {
        #region 构造器
        /// <summary>
        /// 初始化 <see cref="ModbusTcpClient"/> 类的新实例
        /// </summary>
        public ModbusTcpClient()
        {
            Log = new ModbusLog();
        }
        /// <summary>
        /// 初始始化一个实例
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        public ModbusTcpClient(string host, int port) : this()
        {
            if (host.IsNullOrEmpty()) return;
            EndPoint = new IPEndPoint(Dns.GetHostAddresses(host).First(), port);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 是否启用调试
        /// </summary>
        private bool _Debug = true;
        /// <summary>
        /// 是否启用调试
        /// </summary>
        public bool Debug
        {
            get => _Debug;
            set
            {
                if (Log == null) Log = new ModbusLog(value);
                else Log.IsDebug = value;
                _Debug = value;
            }
        }
        /// <summary>
        /// 日志类
        /// </summary>
        private IModbusLog Log { get; set; }
        /// <summary>
        /// 协议类型
        /// </summary>
        public ProtocolType ProtocolType { get; set; }
        /// <summary>
        /// 服务端终节点
        /// </summary>
        public IPEndPoint EndPoint { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        protected ISocketClient Client { get; set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        private bool _IsConnected = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected => _IsConnected;
        /// <summary>
        /// 校验算法
        /// </summary>
        public Func<byte[], byte[]> VerificationAlgorithm { get; set; }
        /// <summary>
        /// 字节序类型
        /// </summary>
        public EndianType EndianType { get; set; } = EndianType.LITTLE;
        /// <summary>
        /// 校验类型
        /// </summary>
        public VerificationType VerificationType { get; set; } = VerificationType.None;
        #endregion

        #region 方法
        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            if (EndPoint == null) return await Task.FromResult(false);
            if (Client == null)
                Client = new SocketClient(EndPoint);
            else
                Client.EndPoint = EndPoint;
            if (IsConnected && Client.Connected) return await Task.FromResult(true);
            var result = await Client.ConnectAsync(EndPoint).ConfigureAwait(false);
            if (result == System.Net.Sockets.SocketError.Success)
            {
                _IsConnected = true;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="packet">请求包</param>
        /// <returns></returns>
        public async Task<ResultModel<byte[]>> SendCommandAsync(ModbusTcpRequestPacket packet)
        {
            if (packet == null) return ResultModel<byte[]>.Error("请求数据为空.");
            if (packet.Address < ModbusHelper.ADDRESS_MIN || packet.Address > ModbusHelper.ADDRESS_MAX) return ResultModel<byte[]>.Error("地址不合法.");
            var command = packet.ToArray();
            command = command.Encode(0, command.Length, this.VerificationType, this.EndianType, this.VerificationAlgorithm);
            var result = new ResultModel<byte[]>
            {
                IsSuccessed = true,
                Request = command
            };
            if (await ConnectAsync().ConfigureAwait(false))
            {                
                await Client.SendAsync(command).ConfigureAwait(false);
                var bytes = await this.Client.ReceviceMessageAsync(new byte[8]).ConfigureAwait(false);
                if (bytes == null)
                {
                    result.IsSuccessed = false;
                    result.ErrorMessage = "网络错误";
                    return result;
                }
                var length = bytes.ReadBytes(4, 5).ToUInt32().ToCast<int>();
                if (packet.Code == FunctionCodes.ReadCoils || packet.Code == FunctionCodes.ReadInputDiscreteQuantity || packet.Code == FunctionCodes.WriteCoils)
                {
                    length = length / 8 + (length % 8 == 0 ? 0 : 1);
                }
                else if (packet.Code == FunctionCodes.ReadHoldingRegisters || packet.Code == FunctionCodes.ReadInputRegister || packet.Code == FunctionCodes.WriteRegisters || packet.Code == FunctionCodes.ReadFileRecord || packet.Code == FunctionCodes.WriteFileRecord || packet.Code == FunctionCodes.ReadWriteMultipleRegisters)
                {
                    length = 2 * length;
                }
                var bytes2 = await this.Client.ReceviceMessageAsync(new byte[length - 2]).ConfigureAwait(false);
                result.Response = bytes.Concat(bytes2).ToArray();
                var ResPacket = new ModbusTcpRequestPacket(result.Response, packet);
                if (packet.TransactionFlags != ResPacket.TransactionFlags || packet.ProtocolFlags != ResPacket.ProtocolFlags || packet.Host != ResPacket.Host)
                {
                    result.IsSuccessed = false;
                    result.ErrorMessage = "响应数据不正确";
                    return result;
                }
                if (ResPacket.Code == 0 && ResPacket.ErroCode > 0)
                {
                    result.IsSuccessed = false;
                    result.ErrorCode = ResPacket.ErroCode;
                    result.ErrorMessage = ResPacket.ErroCode.GetDescription();
                    return result;
                }
                if (result.IsSuccessed)
                {
                    result.Value = ResPacket.Data;
                }
                return await Task.FromResult(result);
            }
            return await Task.FromResult(result);
        }

        #region 读取

        #region 读线圈
        /// <summary>
        /// 读线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">数量</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> ReadCoilAsync(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.COIL_MIN_COUNT || count > ModbusHelper.COIL_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.ReadCoils,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Count = count,
                Host = host
            };
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #region 读离散量输入
        /// <summary>
        /// 读离散量输入
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">数量</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> ReadInputDiscreteQuantityAsync(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.COIL_MIN_COUNT || count > ModbusHelper.COIL_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.ReadInputDiscreteQuantity,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Count = count,
                Host = host
            };
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #region 读保持寄存器
        /// <summary>
        /// 读保持寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取长度</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> ReadHoldingRegistersAsync(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.REGISTER_MIN_COUNT || count > ModbusHelper.REGISTER_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.ReadHoldingRegisters,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Count = count,
                Host = host
            };
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #region 读输入寄存器
        /// <summary>
        /// 读输入寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取长度</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> ReadInputRegisterAsync(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.REGISTER_MIN_COUNT || count > ModbusHelper.REGISTER_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.ReadInputRegister,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Count = count,
                Host = host
            };
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #endregion

        #region 写入

        #region 写单个线圈
        /// <summary>
        /// 写单个线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> WriteCoilAsync(ushort address, bool value, byte host = 1)
        {
            var bytes = new byte[2] { 0x00, 0x00 };
            bytes[0] = (byte)(value ? 0xFF : 0x00);
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.WriteCoil,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Data = bytes,
                Host = host
            };
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #region 写单个保持寄存器
        /// <summary>
        /// 写单个保持寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> WriteRegisterAsync(ushort address, ushort value, byte host = 1)
        {
            if (value < ModbusHelper.REGISTER_MIN_VALUE || value > ModbusHelper.REGISTER_MAX_VALUE) return Array.Empty<byte>();
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.WriteRegister,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Data = value.GetBytes(),
                Host = host
            };
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #region 写多个线圈
        /// <summary>
        /// 写多个线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> WriteCoilsAsync(ushort address, ushort value, byte host = 1)
        {
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.WriteCoils,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Data = value.GetBytes(),
                Host = host
            };
            packet.Count = (ushort)(packet.Data.Length * 8);
            if (packet.Count < ModbusHelper.COILS_MIN_COUNT || packet.Count > ModbusHelper.COILS_MAX_COUNT) return Array.Empty<byte>();
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #region 写多个保持寄存器
        /// <summary>
        /// 写多个保持寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public async Task<byte[]> WriteRegistersAsync(ushort address, ushort value, byte host = 1)
        {
            var packet = new ModbusTcpRequestPacket()
            {
                Code = FunctionCodes.WriteRegisters,
                TransactionFlags = ModbusHelper.GetCheckHeader().ToUInt16(),
                Address = address,
                Data = value.GetBytes(),
                Host = host
            };
            packet.Count = (ushort)(Math.Ceiling(packet.Data.Length / 2f));
            if (packet.Count > ModbusHelper.REGISTER_MIN_COUNT || packet.Count < ModbusHelper.REGISTER_MAX_COUNT) return Array.Empty<byte>();
            var result = await SendCommandAsync(packet).ConfigureAwait(false);
            if (!result.IsSuccessed) return await Task.FromResult(Array.Empty<byte>());
            return await Task.FromResult(result.Value);
        }
        #endregion

        #endregion

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (IsConnected)
            {
                Client.Stop();
                _IsConnected = false;
            }
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing">标识</param>
        protected override void Dispose(bool disposing)
        {
            DisConnect();
            Client.Dispose();
            base.Dispose(disposing);
        }
        /// <summary>
        /// 析构器
        /// </summary>
        ~ModbusTcpClient()
        {
            Dispose(true);
        }
        #endregion
    }
}