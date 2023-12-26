using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using XiaoFeng.Net;
using XiaoFeng.Modbus.Internal;
using System.Net;
using System.Threading.Tasks;
using XiaoFeng.Modbus.Protocols;
/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-13 15:27:39                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Server
{
    /// <summary>
    /// ModbusServer 服务端
    /// </summary>
    public class ModbusTcpServer : IModbusServer
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusTcpServer()
        {
            Log = new ModbusLog();
        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="port">监听端口</param>
        public ModbusTcpServer(int port)
        {
            if (Options == null) Options = new ModbusOptions();
            Options.EndPoint = new IPEndPoint(IPAddress.Any, port);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 服务端
        /// </summary>
        private ISocketServer Server { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public ModbusOptions Options { get; set; }
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
        #endregion

        #region 方法
        public void Init()
        {
            if (Server == null)
            {
                Server = new SocketServer(Options.EndPoint)
                {
                    Encoding = Options.Encoding,
                    ConnectTimeout = Options.ConnectTimeout,
                    ReceiveTimeout = Options.ReadTimeout,
                    SendTimeout = Options.WriteTimeout,
                    ReceiveBufferSize = Options.ReadBufferSize,
                    SendBufferSize = Options.WriteBufferSize
                };
                Server.OnStart += (s, e) =>
                {
                    Log.Debug($"服务器已启动.\r\n监听地址:{Options.EndPoint.Address.ToString()}\r\n监听端口:{Options.EndPoint.Port}");
                };
                Server.OnStop += (s, e) =>
                {
                    Log.Debug("服务器已停止.");
                };
                Server.OnNewConnection += (c, e) =>
                {
                    Log.Debug($"客户端已连接:{c.EndPoint}");
                };
                Server.OnMessageByte += (c, m, e) =>
                {
                    ReceviceMessageAsync(c, m).ConfigureAwait(false);
                };
                Server.OnDisconnected += (c, e) =>
                {
                    Log.Debug($"客户端已断开:{c.EndPoint}");
                };
                Server.OnClientError += (c, e, ex) =>
                {
                    Log.Debug($"客户端[{e}]出错:{ex.Message}");
                };
                Server.OnError += (s, e) =>
                {
                    Log.Debug($"服务器出错:{e.Message}");
                };
            }
            if (!Server.Active)
                Server.Start();
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="bytes">数据</param>
        /// <returns></returns>
        private async Task ReceviceMessageAsync(ISocketClient client, byte[] bytes)
        {
            var packet = new ModbusTcpRequestPacket(bytes,null);
            await Task.CompletedTask;
        }
        #endregion
    }
}