using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables
{

    public class ProductMediaAccess
    {
        #region Default Methods
        public static Infrastructure.Data.Entities.Tables.ProductMediaEntity Get(int id)
        {
            var dataTable = new DataTable();
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [ProductMedia] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);

            }

            if (dataTable.Rows.Count > 0)
            {
                return new Infrastructure.Data.Entities.Tables.ProductMediaEntity(dataTable.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public static List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> Get()
        {  
            var dataTable = new DataTable();     
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM [ProductMedia]";
                var sqlCommand = new SqlCommand(query, sqlConnection); 

                new SqlDataAdapter(sqlCommand).Fill(dataTable);
            }

            if (dataTable.Rows.Count > 0)
            {
                return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductMediaEntity(x)).ToList();
            }
            else
            {
                return new List<Infrastructure.Data.Entities.Tables.ProductMediaEntity>();
            }
        }
        public static List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> Get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
                List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> results = null;
                if(ids.Count <= maxQueryNumber)
                {
                    results = get(ids);
                }else
                {
                    int batchNumber = ids.Count / maxQueryNumber;
                    results = new List<Infrastructure.Data.Entities.Tables.ProductMediaEntity>();
                    for(int i=0; i<batchNumber; i++)
                    {
                        results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
                    }
                    results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
                }
                return results;
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductMediaEntity>();
        }
        private static List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> get(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                var dataTable = new DataTable();
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    
                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    sqlCommand.CommandText = $"SELECT * FROM [ProductMedia] WHERE [Id] IN ({queryIds})";                    
                new SqlDataAdapter(sqlCommand).Fill(dataTable);
                }

                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.ProductMediaEntity(x)).ToList();
                }
                else
                {
                    return new List<Infrastructure.Data.Entities.Tables.ProductMediaEntity>();
                }
            }
            return new List<Infrastructure.Data.Entities.Tables.ProductMediaEntity>();
        }

        public static int Insert(Infrastructure.Data.Entities.Tables.ProductMediaEntity item)
        {
            int response = int.MinValue;
            using (var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                string query = "INSERT INTO [ProductMedia] ([Alt],[DisplayOrder],[ProductId],[Size],[Type],[Url])  VALUES (@Alt,@DisplayOrder,@ProductId,@Size,@Type,@Url); ";
                query += "SELECT SCOPE_IDENTITY();";

                using (var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Alt",item.Alt == null ? (object)DBNull.Value  : item.Alt);
					sqlCommand.Parameters.AddWithValue("DisplayOrder",item.DisplayOrder);
					sqlCommand.Parameters.AddWithValue("ProductId",item.ProductId);
					sqlCommand.Parameters.AddWithValue("Size",item.Size == null ? (object)DBNull.Value  : item.Size);
					sqlCommand.Parameters.AddWithValue("Type",item.Type == null ? (object)DBNull.Value  : item.Type);
					sqlCommand.Parameters.AddWithValue("Url",item.Url);

                    var result = sqlCommand.ExecuteScalar();
                    response = result == null? int.MinValue:  int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
                }
                sqlTransaction.Commit();

                return response;
            }
        }
        public static int Insert(List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
                int results=0;
                if(items.Count <= maxParamsNumber)
                {
                    results = insert(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }
                return results;
            }

            return -1;
        }
        private static int insert(List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " INSERT INTO [ProductMedia] ([Alt],[DisplayOrder],[ProductId],[Size],[Type],[Url]) VALUES ( "

							+ "@Alt"+ i +","
							+ "@DisplayOrder"+ i +","
							+ "@ProductId"+ i +","
							+ "@Size"+ i +","
							+ "@Type"+ i +","
							+ "@Url"+ i 
                            + "); ";

                            
							sqlCommand.Parameters.AddWithValue("Alt" + i, item.Alt == null ? (object)DBNull.Value  : item.Alt);
							sqlCommand.Parameters.AddWithValue("DisplayOrder" + i, item.DisplayOrder);
							sqlCommand.Parameters.AddWithValue("ProductId" + i, item.ProductId);
							sqlCommand.Parameters.AddWithValue("Size" + i, item.Size == null ? (object)DBNull.Value : item.Size);
							sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value  : item.Type);
							sqlCommand.Parameters.AddWithValue("Url" + i, item.Url);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Update(Infrastructure.Data.Entities.Tables.ProductMediaEntity item)
        {   
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "UPDATE [ProductMedia] SET [Alt]=@Alt, [DisplayOrder]=@DisplayOrder, [ProductId]=@ProductId, [Size]=@Size, [Type]=@Type, [Url]=@Url WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                    
                sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Alt",item.Alt == null ? (object)DBNull.Value  : item.Alt);
				sqlCommand.Parameters.AddWithValue("DisplayOrder",item.DisplayOrder);
				sqlCommand.Parameters.AddWithValue("ProductId",item.ProductId);
				sqlCommand.Parameters.AddWithValue("Size",item.Size == null ? (object)DBNull.Value  : item.Size);
				sqlCommand.Parameters.AddWithValue("Type",item.Type == null ? (object)DBNull.Value  : item.Type);
				sqlCommand.Parameters.AddWithValue("Url",item.Url);
                        
                results = sqlCommand.ExecuteNonQuery();
            }
                
            return results;
        }
        public static int Update(List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
                int results = 0;
                if(items.Count <= maxParamsNumber)
                {
                    results = update(items);
                }else
                {
                    int batchNumber = items.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
                }

                return results;
            }

            return -1;
        }
        private static int update(List<Infrastructure.Data.Entities.Tables.ProductMediaEntity> items)
        {
            if (items != null && items.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    string query = "";
                    var sqlCommand = new SqlCommand(query, sqlConnection);

                    int i = 0;
                    foreach (var item in items)
                    {
                        i++;
                        query += " UPDATE [ProductMedia] SET "

							+ "[Alt]=@Alt"+ i +","
							+ "[DisplayOrder]=@DisplayOrder"+ i +","
							+ "[ProductId]=@ProductId"+ i +","
							+ "[Size]=@Size"+ i +","
							+ "[Type]=@Type"+ i +","
							+ "[Url]=@Url"+ i +" WHERE [Id]=@Id" + i 
                            + "; ";

                            sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
							sqlCommand.Parameters.AddWithValue("Alt" + i, item.Alt == null ? (object)DBNull.Value  : item.Alt);
							sqlCommand.Parameters.AddWithValue("DisplayOrder" + i, item.DisplayOrder);
							sqlCommand.Parameters.AddWithValue("ProductId" + i, item.ProductId);
							sqlCommand.Parameters.AddWithValue("Size" + i, item.Size == null ? (object)DBNull.Value : item.Size);
							sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value  : item.Type);
							sqlCommand.Parameters.AddWithValue("Url" + i, item.Url);
                    }

                    sqlCommand.CommandText = query;

                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }

            return -1;
        }

        public static int Delete(int id)
        {
            int results = -1;
            using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [ProductMedia] WHERE [Id]=@Id";
                var sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);

                results = sqlCommand.ExecuteNonQuery();
            }

            return results;
        }
        public static int Delete(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int maxParamsNumber = Settings.MAX_BATCH_SIZE; 
                int results=0;
                if(ids.Count <= maxParamsNumber)
                {
                    results = delete(ids);
                } else
                {
                    int batchNumber = ids.Count / maxParamsNumber;
                    for(int i = 0; i < batchNumber; i++)
                    {
                        results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
                    }
                    results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
                }
            }
            return -1;
        }
        private static int delete(List<int> ids)
        {
            if(ids != null && ids.Count > 0)
            {
                int results = -1;
                using(var sqlConnection = new SqlConnection(Settings.GetConnectionString()))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;

                    string queryIds = string.Empty;
                    for(int i=0; i<ids.Count; i++)
                    {
                        queryIds += "@Id" + i + ",";
                        sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
                    }
                    queryIds = queryIds.TrimEnd(',');

                    string query = "DELETE FROM [ProductMedia] WHERE [Id] IN ("+ queryIds +")";                    
                    sqlCommand.CommandText = query;
                        
                    results = sqlCommand.ExecuteNonQuery();
                }

                return results;
            }
            return -1;
        }
        #endregion

        #region Custom Methods


        
        #endregion
    }
}
