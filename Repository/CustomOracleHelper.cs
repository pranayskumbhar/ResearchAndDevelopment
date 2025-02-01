using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class CustomOracleHelper

    {
        public string QurtyParam { get; set; }
        public string QueryString { get; set; }
        public OracleDbType DbType { get; set; }
        public object Value { get; set; }
        public string ConnectionString { get; set; }

        public List<CustomOracleHelper> Parameters { get; set; }


        public CustomOracleHelper()
        {

            Parameters = new List<CustomOracleHelper>();
        }




        public void Add(string parametrer, OracleDbType dbType, object value)
        {
            Parameters.Add(new CustomOracleHelper
            {
                //QurtyParam = parametrer,
                QurtyParam = parametrer.StartsWith(":") ? parametrer : ":" + parametrer,
                DbType = dbType,
                Value = value
            });
        }

        public int ExecuteSbilQuery()
        {
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (OracleCommand command = new OracleCommand(QueryString, connection))
                    {
                        foreach (var param in Parameters)
                        {

                            //string OracleParamString = param.QurtyParam.StartsWith(":") ? param.QurtyParam : ":" + param.QurtyParam;
                            //command.Parameters.Add(OracleParamString, param.DbType).Value = param.Value;


                            command.Parameters.Add(param.QurtyParam, param.DbType).Value = param.Value;
                        }

                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public DataTable GetDataTable()
        {
            DataTable dataTable = new DataTable();

            OracleConnection connection = new OracleConnection(ConnectionString);

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }



                OracleCommand command = new OracleCommand(QueryString, connection);
                if (Parameters.Count > 0)
                {
                    foreach (var param in Parameters)
                    {
                        command.Parameters.Add(param.QurtyParam, param.DbType).Value = param.Value;
                    }
                }


                OracleDataAdapter adapter = new OracleDataAdapter(command);
                adapter.Fill(dataTable);




                return dataTable;
            }
            catch (Exception ex)
            {
                return dataTable;
            }
            finally
            {
                connection.Close();
            }

        }
    }

}
