using Microsoft.EntityFrameworkCore;

namespace HangfireSulotion.Data
{
    public class HangFireContext : DbContext
    {
        public HangFireContext(DbContextOptions<HangFireContext> context) : base(context) 
        {

        }
    }
}
