using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain
{
    /// <summary>
    /// IEntity：实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 获取实体ID
        /// 通常情况，一个实体仅有一个ID，但也不排除有多个ID的情况，这个接口方法是实现了有多个ID的情况。返回值为 object[]
        /// </summary>
        /// <returns></returns>
        object[] GetKeys();
    }

    /// <summary>
    /// 该接口表示实体仅有一个ID的情况
    /// ID的类型用TKey 一个泛型来表示
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}
