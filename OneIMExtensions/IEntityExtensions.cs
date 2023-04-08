using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.DB.Entities;

namespace OneIMExtensions
{
    public static class IEntityExtensions
    {
        /// <summary>
        /// Generates an event for an IEntity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="session"></param>
        /// <param name="eventname"></param>
        public static void GenerateEvent(this IEntity entity, ISession session, string eventname) => session.GenerateEvent(entity, eventname);
        
    }
}
