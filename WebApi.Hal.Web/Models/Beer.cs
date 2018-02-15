using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Hal.Web.Models
{
    public class Beer
    {
        protected Beer()
        {
        }

        public Beer(string name)
        {
            Name = name;
        }

        public int Id { get; protected set; }
        public string Name { get; set; }
        [ForeignKey("Style_Id")]
        public BeerStyle Style { get; set; }
        [ForeignKey("Brewery_Id")]
        public Brewery Brewery { get; set; }
    }
}