using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using XiaoFeng.Modbus.Internal;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-23 18:49:42                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Server
{
    /// <summary>
    /// ModbusRtu服务端
    /// </summary>
    public class ModbusRtuServer:ModbusRtu
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusRtuServer()
        {

        }
        #endregion

        #region 属性

        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serialPort">SerialPort</param>
        public override void Init(SerialPort serialPort)
        {
            this.SerialDataReceived += (sender, e) =>
            {
                var requestData = new byte[this.Serial.BytesToRead];
                this.Serial.Read(requestData, 0, requestData.Length);

            };
            this.SerialErrorReceived += (sender, e) =>
            {

            };
            this.SerialPinChanged += (sender, e) =>
            {

            };
        }
        #endregion
    }
}