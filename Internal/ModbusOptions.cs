using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-22 11:07:03                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Internal
{
    /// <summary>
    /// Modbus配置
    /// </summary>
    public class ModbusOptions
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusOptions()
        {

        }
        #endregion

        #region 属性
        /// <summary>
        /// 终节点
        /// </summary>
        public IPEndPoint EndPoint { get; set; }
        /// <summary>
        /// 读取数据超时
        /// </summary>
        public int ReadTimeout { get; set; }
        /// <summary>
        /// 写入数据超时
        /// </summary>
        public int WriteTimeout { get; set; }
        /// <summary>
        /// 连接超时
        /// </summary>
        public int ConnectTimeout { get; set; }
        /// <summary>
        /// 读取数据缓存大小
        /// </summary>
        public int ReadBufferSize { get; set; }
        /// <summary>
        /// 写入数据缓存大小
        /// </summary>
        public int WriteBufferSize { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        #endregion

        #region 方法

        #endregion
    }
}