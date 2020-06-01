using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain
{
    /// <summary>
    /// 聚合根接口，空接口：不实现任何方法
    /// 作用： 在实现仓储层的时候，一个仓储对应一个聚合根
    /// </summary>
    public interface IAggregateRoot
    {
    }
}
