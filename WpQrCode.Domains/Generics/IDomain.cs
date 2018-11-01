using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpQrCode.Domains.Generics
{
    public interface IDomain<T> where T : class
    {
        T Save(T entity);
        T Update(T entity);
        IEnumerable<T> GetAll(int idCliente);
        T GetById(int entityId, int idCliente);
        void Delete(T entity);
    }
}
