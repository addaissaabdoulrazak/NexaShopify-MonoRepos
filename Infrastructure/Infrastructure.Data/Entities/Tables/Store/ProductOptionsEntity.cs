using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductOptionsEntity
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int Position { get; set; }
		public int ProductId { get; set; }

        public ProductOptionsEntity() { }

        public ProductOptionsEntity(DataRow dataRow)
        {
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = Convert.ToString(dataRow["Name"]);
			Position = Convert.ToInt32(dataRow["Position"]);
			ProductId = Convert.ToInt32(dataRow["ProductId"]);
        }
    }
}

