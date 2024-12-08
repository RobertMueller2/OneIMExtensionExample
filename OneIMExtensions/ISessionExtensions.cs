using VI.DB.DataAccess;
using VI.DB.Entities;
using VI.DB.Sync;

namespace OneIMExtensions
{
    public static class ISessionExtensions
    {

        /// <summary>
        /// Gets an IStatementRunner from an ISession object
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static IStatementRunner GetStatementRunner(this ISession session) => session.Resolve<IStatementRunner>();

        /// <summary>
        /// Generates an event for an IEntity.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="entity"></param>
        /// <param name="eventname"></param>
        public static void GenerateEvent(this ISession session, IEntity entity, string eventname)
        {
            using (var myUoW = session.StartUnitOfWork())
            {
                myUoW.Generate(entity, eventname);
                myUoW.Commit();
            }
        }
        

    }
}
