using VI.DB.Model;
using VI.DB.Entities;
using VI.DB.Sync;

namespace TypedWrapperExensions
{
    public static class PersonExtensions
    {
        /// <summary>
        /// Returns a Department object from a Person, if present
        /// </summary>
        /// <param name="p"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static Department? GetDepartment(this Person p, ISession session)
        {
            if (String.IsNullOrEmpty(p.UID_Department)) { return null; }

            return session.Source().Get<Department>(p.UID_Department);
        }

    }
}
