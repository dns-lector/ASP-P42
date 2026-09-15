namespace ASP_P42.Data
{
    public class DataAccessor(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;

        public Guid GetDbIdentity() => Guid.NewGuid(); 

        public List<Entities.ProductGroup> GetAllProductGroups(bool isIncludeHidden = false) {
            IQueryable<Entities.ProductGroup> query = _dataContext.ProductGroups;
            if (!isIncludeHidden)
            {
                query = query.Where(g => g.IsHidden == 0);
            }
            return [..query.OrderBy(g => g.OrderInPrice)];
        }
    }
}
/* DAL - Data Access Layer
 * Шар доступу до даних - поєднання декількох DAO (Data Access Object)
 * або узагальнений інтерфейс одержання даних
 */