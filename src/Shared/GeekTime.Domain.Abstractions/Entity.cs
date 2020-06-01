using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain
{
    /*
     * 可在实体上定义一些共享的方法，比如重写Tostirng
     */

    public abstract class Entity : IEntity
    {
        public abstract object[] GetKeys();


        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Keys = {string.Join(",", GetKeys())}";
        }



        #region 
        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
        #endregion
    }


    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        int? _requestedHashCode;
        public virtual TKey Id { get; protected set; }
        public override object[] GetKeys()
        {
            return new object[] { Id };
        }

        /// <summary>
        /// 重写Equal，可判断两个实体对象是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity<TKey> item = (Entity<TKey>)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id.Equals(this.Id);
        }


        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }



        //表示对象是否为全新创建的，未持久化的
        //没有id说明未对象未持久化
        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default);
        }

        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Id = {Id}";
        }


        //通过下面两个操作符重载，使我们可以使用操作符来判断两个领域对象是否相等

        /// <summary>
        /// 操作符 == 重载
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        /// <summary>
        /// 操作符 != 重载
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }
    }
}
