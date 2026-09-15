using ASP_P42.Data.Entities;

namespace ASP_P42.Data
{
    public class DataAccessor(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;

        public Guid GetDbIdentity() => Guid.NewGuid();
        /* Д.З. - реалізувати метод отримання унікального ідентифікатора 
         * з бази даних. SQL Server має функцію NEWID(), яка генерує 
         * унікальний ідентифікатор.
         */

        public List<Entities.ProductGroup> GetAllProductGroups(bool isIncludeHidden = false) {
            IQueryable<Entities.ProductGroup> query = _dataContext.ProductGroups;
            if (!isIncludeHidden)
            {
                query = query.Where(g => g.IsHidden == 0);
            }
            return [..query.OrderBy(g => g.OrderInPrice)];
        }

        public async Task<Guid> AddNewProductGroup(Entities.ProductGroup productGroup)
        {
            Guid id = GetDbIdentity();
            productGroup.Id = id;
            _dataContext.ProductGroups.Add(productGroup);
            await _dataContext.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> AddNewProduct()
        {
            Guid id = GetDbIdentity();

            return id;
        }
    }
}
/* DAL - Data Access Layer
 * Шар доступу до даних - поєднання декількох DAO (Data Access Object)
 * або узагальнений інтерфейс одержання даних
 */