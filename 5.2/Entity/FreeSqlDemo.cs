using FreeSql.DataAnnotations;

namespace _5._2.Entity
{
    public class FreeSqlDemo
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
