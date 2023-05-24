using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Domain.Entities
{
    /// <summary>
    /// 定义一个聚合根
    /// </summary>
    public interface IAggregateRoot : IEntity
    {

    }

    /// <summary>
    /// 定义一个聚合根，具有一个Id属性作为其唯一标识。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
        where TKey : IEquatable<TKey>
    {

    }
}
