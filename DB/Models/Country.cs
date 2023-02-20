using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.DB.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }

        public override bool Equals(object obj)
        {
            return string.Equals(this.Code, (obj as Country).Code, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
