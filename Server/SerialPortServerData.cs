using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XiaoFeng.Data.SQL;
using XiaoFeng.Modbus.Internal;
using XiaoFeng.Modbus.Model;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-25 11:33:56                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Server
{
    /// <summary>
    /// 线圈寄存器数据
    /// </summary>
    public class SerialPortServerData
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public SerialPortServerData()
        {
            this.Coils = new List<CoilModel>();
            this.Registers = new List<RegisterModel>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 单线圈数据
        /// </summary>
        public List<CoilModel> Coils { get; set; }
        /// <summary>
        /// 寄存器数据
        /// </summary>
        public List<RegisterModel> Registers { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 读线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取线圈数量</param>
        /// <returns></returns>
        public byte[] ReadCoil(ushort address, ushort count)
        {
            if (address < ModbusHelper.ADDRESS_MIN || address > ModbusHelper.ADDRESS_MAX || this.Coils == null || !this.Coils.Any()) return Array.Empty<byte>();

            if (address + count > Math.Min(ModbusHelper.ADDRESS_MAX, this.Coils.Count)) return Array.Empty<byte>();

            var cs = this.Coils.Skip(address).Take(count).ToList();
            var length = (int)Math.Ceiling(count / 8f);
            var bytes = new byte[length];
            for (var i = 0; i < length; i++)
            {
                var bs = "";
                for (var j = 0; j < 8; j++)
                {
                    var _index = i * 8 + j;
                    if (_index < count)
                    {
                        var a = cs[_index];
                        bs += a.Value == 0 ? '0' : '1';
                    }
                }
                bs = bs.PadRight(8, '0');
                bytes[i] = Convert.ToByte(bs, 2);
            }
            return bytes;
        }
        /// <summary>
        /// 写线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Boolean WriteCoil(ushort address, byte value)
        {
            if (address < ModbusHelper.ADDRESS_MIN || address > ModbusHelper.ADDRESS_MAX) return false;
            if (this.Coils == null) this.Coils = new List<CoilModel>();
            if (!this.Coils.Any())
            {
                for (ushort i = 0; i < address; i++)
                {
                    this.Coils.Add(new CoilModel(i, 0));
                }
                this.Coils.Add(new CoilModel(address, value));
                return true;
            }
            if (address < this.Coils.Count)
            {
                this.Coils[address].Value = value;
            }
            else
            {
                for (ushort i = (ushort)this.Coils.Count; i < address; i++)
                    this.Coils.Add(new CoilModel(i, 0));
                this.Coils.Add(new CoilModel(address, value));
            }
            return true;
        }
        /// <summary>
        /// 读取寄存器数据
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取数量</param>
        /// <returns></returns>
        public byte[] ReadRegister(ushort address, ushort count)
        {
            if (address < ModbusHelper.ADDRESS_MIN || address > ModbusHelper.ADDRESS_MAX || this.Registers == null || !this.Registers.Any()) return Array.Empty<byte>();

            if (address + count > Math.Min(ModbusHelper.ADDRESS_MAX, this.Coils.Count)) return Array.Empty<byte>();

            var cs = this.Registers.Skip(address).Take(count).ToList();
            var length = count * 2;
            var bytes = new byte[length];
            for (var i = 0; i < count; i++)
            {
                var bs = cs[i].Data;
                bytes[2 * i + 0] = bs[0];
                bytes[2 * i + 1] = bs[1];
            }
            return bytes;
        }
        /// <summary>
        /// 写寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Boolean WriteRegister(ushort address, ushort value)
        {
            if (address < ModbusHelper.ADDRESS_MIN || address > ModbusHelper.ADDRESS_MAX) return false;
            if (this.Registers == null) this.Registers = new List<RegisterModel>();

            if (!this.Registers.Any())
            {
                for (ushort i = 0; i < address; i++)
                {
                    this.Registers.Add(new RegisterModel(i, 0));
                }
                this.Registers.Add(new RegisterModel(address, value));
                return true;
            }
            if (address < this.Registers.Count)
            {
                this.Registers[address].Value = value;
            }
            else
            {
                for (ushort i = (ushort)this.Registers.Count; i < address; i++)
                    this.Registers.Add(new RegisterModel(i, 0));
                this.Registers.Add(new RegisterModel(address, value));
            }
            return true;
        }
        #endregion
    }
}