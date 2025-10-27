using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ESOA.Common
{
    public static class Data
    {
        public static string BuildNumber;

        private static bool _isProduction;
        private static string _connection = string.Empty;
        private static string _identityConnection = string.Empty;
        private static readonly IConfiguration _config;

        public static IHttpContextAccessor HttpContextAccessor;

        static Data()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version is { })
            {
                DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
                BuildNumber = $"{version} ({buildDate})";
            }
        }

        public static bool HasConnectionString => !string.IsNullOrEmpty(_connection);
        public static bool HasIdentityConnectionString => !string.IsNullOrEmpty(_identityConnection);
        public static bool IsProduction => _isProduction;

        public static void Initialize(bool isProduction, string connection, string identityConnection, IHttpContextAccessor httpContextAccessor)
        {
            _isProduction = isProduction;
            _connection = connection;
            _identityConnection = identityConnection;
            HttpContextAccessor = httpContextAccessor;
        }

        public static async Task<SqlConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
        {
            //Added

            //dev local db
            //var connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=ESOA;Trusted_Connection=True;");

            //uat db
            //var connection = new SqlConnection("Server=172.25.65.12;Database=ESOA;user id=lbcdbsa;password=B7xqz56*;");
            //var connection = new SqlConnection("Server=172.25.65.12;Database=ESOA;user id=lbcdbsa;password=B7xqz56!;");

            var connection = new SqlConnection("Server=172.25.65.12;Database=ESOA;user id=lbcdbsa;password=D3vdb92(dz;");

            //prod db
            //

            //var connection = new SqlConnection(_connection);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }

        public static SqlConnection CreateConnection(CancellationToken cancellationToken = default)
        {
            //Added  
            var connection = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=EPDV;Trusted_Connection=True;");

            //var connection = new SqlConnection(_connection);
            connection.Open();
            return connection;
        }

        public static async Task<SqlConnection> CreateIdentityConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = new SqlConnection(_identityConnection);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }

        public static string GetString(object input)
        {
            if (input == null) return string.Empty;
            return input == DBNull.Value ? string.Empty : input.ToString();
        }

        public static string GetNullableString(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return input.ToString();
        }

        public static bool GetBool(object input)
        {
            if (input == null) return false;
            return input != DBNull.Value && Convert.ToBoolean(input);
        }

        public static bool? GetNullableBool(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return Convert.ToBoolean(input);
        }

        public static double GetDouble(object input)
        {
            if (input == null || input == DBNull.Value) return double.NaN;
            return Convert.ToDouble(input);
        }

        public static double? GetNullableDouble(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return Convert.ToDouble(input);
        }

        public static float GetFloat(object input)
        {
            if (input == null || input == DBNull.Value) return 0;
            return Convert.ToSingle(input);
        }

        public static float? GetNullableFloat(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return Convert.ToSingle(input);
        }

        public static Guid GetGuid(object input)
        {
            if (input == null || input == DBNull.Value) return Guid.Empty;
            return (Guid)input;
        }

        public static Guid? GetNullableGuid(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return (Guid)input;
        }

        public static DateTime GetDateTime(object input)
        {
            if (input == null || input == DBNull.Value) return DateTime.MinValue;
            return Convert.ToDateTime(input);
        }

        public static DateTime? GetNullableDateTime(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return Convert.ToDateTime(input);
        }

        public static int GetInt(object input)
        {
            if (input == null || input == DBNull.Value) return 0;
            return Convert.ToInt32(input);
        }

        public static long GetLong(object input)
        {
            if (input == null || input == DBNull.Value) return 0;
            return Convert.ToInt64(input);
        }

        public static int? GetNullableInt(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return Convert.ToInt32(input);
        }

        public static long GetInt64(object input)
        {
            if (input == null || input == DBNull.Value) return 0;
            return Convert.ToInt64(input);
        }

        public static decimal GetDecimal(object input)
        {
            if (input == null || input == DBNull.Value) return 0;
            string decimalString = GetString(input);
            return Convert.ToDecimal(decimalString);
        }

        public static byte[] GetByteArray(object input)
        {
            if (input == null || input == DBNull.Value) return null;
            return (byte[])input;
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, bool value)
        {
            try
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, bool? value)
        {
            try
            {
                if (!value.HasValue)
                {
                    cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameterName, value);
                }

            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static object DbParameter(Guid value)
        {
            if (value == Guid.Empty) return DBNull.Value;
            return value;
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, Guid value)
        {
            if (value == Guid.Empty)
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, Guid? value)
        {
            if (!value.HasValue || value.Value == Guid.Empty)
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(parameterName, value.Value);
            }
        }

        public static object DbParameter(DateTime value)
        {
            if (value < DateTime.Parse("1/1/1950") || (value > DateTime.Parse("12/31/2100")))
            {
                return DBNull.Value;
            }
            return value;
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, DateTime value)
        {
            string format = "MM/dd/yyyy";
            if (value < DateTime.ParseExact("01/01/1950", format, CultureInfo.InvariantCulture)
              || (value > DateTime.ParseExact("12/31/2100", format, CultureInfo.InvariantCulture)))
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, DateTime? value)
        {
            string format = "MM/dd/yyyy";
            if (value == null
                || (value < DateTime.ParseExact("01/01/1950", format, CultureInfo.InvariantCulture))
                || (value > DateTime.ParseExact("12/31/2100", format, CultureInfo.InvariantCulture)))
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
        }

        public static object DbParameter(int value)
        {
            try
            {
                return value;
            }
            catch
            {
                return DBNull.Value;
            }

        }
        public static object DbParameter(int? value)
        {
            try
            {
                if (value == null) return DBNull.Value;
                return value;
            }
            catch
            {
                return DBNull.Value;
            }

        }
        public static void AddParameter(SqlCommand cmd, string parameterName, byte[] value)
        {
            SqlParameter parameter = new SqlParameter(parameterName, SqlDbType.VarBinary) { Size = -1 };

            try
            {
                if (value == null || (value.Length < 1))
                {
                    parameter.Value = DBNull.Value;
                }
                else
                {
                    parameter.Value = value;
                }
            }
            catch (Exception e)
            {
                parameter.Value = DBNull.Value;
            }
            cmd.Parameters.Add(parameter);
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, long value)
        {
            try
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, long? value)
        {
            try
            {
                if (!value.HasValue)
                {
                    cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameterName, value.Value);
                }
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, int value)
        {
            try
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, int? value)
        {
            try
            {
                if (!value.HasValue)
                {
                    cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameterName, value.Value);
                }
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static object DbParameter(float value)
        {
            try
            {
                if ((float.IsNaN(value)) || (value < float.MinValue) || (value > float.MaxValue))
                {
                    return DBNull.Value;
                }
                return value;
            }
            catch
            {
                return DBNull.Value;
            }

        }

        public static void AddParameter(SqlCommand cmd, string parameterName, float value)
        {
            try
            {
                if ((float.IsNaN(value)) || (value < float.MinValue) || (value > float.MaxValue))
                {
                    cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameterName, value);
                }
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, float? value)
        {
            try
            {
                if ((!value.HasValue) || (float.IsNaN(value.Value)) || (value.Value < float.MinValue) || (value.Value > float.MaxValue))
                {
                    cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameterName, value.Value);
                }
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static object DbParameter(double value)
        {
            try
            {
                if ((double.IsNaN(value)) || (value < double.MinValue) || (value > double.MaxValue))
                {
                    return DBNull.Value;
                }
                return value;
            }
            catch
            {
                return DBNull.Value;
            }

        }

        public static void AddParameter(SqlCommand cmd, string parameterName, double value)
        {
            try
            {
                if ((double.IsNaN(value)) || (value < double.MinValue) || (value > double.MaxValue))
                {
                    cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameterName, value);
                }
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, decimal value)
        {
            try
            {
                if ((value < decimal.MinValue) || (value > decimal.MaxValue))
                {
                    cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue(parameterName, value);
                }
            }
            catch
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
        }

        public static object DbParameter(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return DBNull.Value;
            return value;
        }

        public static object DbParameter(byte[] value)
        {
            if (value == null || value.Length < 1) return DBNull.Value;
            return value;
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(parameterName, value);
            }
        }

        public static void AddParameter(SqlCommand cmd, string parameterName, DBNull value)
        {
            cmd.Parameters.AddWithValue(parameterName, value);
        }

        /// <summary>
        /// Add Output Parameter 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public static void AddOutputParameter(SqlCommand cmd, string parameterName, DbType dbType, int size = 0)
        {
            SqlParameter sqlParam = new SqlParameter
            {
                ParameterName = parameterName,
                DbType = dbType,
                Direction = ParameterDirection.Output,
                Size = size
            };

            cmd.Parameters.Add(sqlParam);
        }

        public static DataTable CreateDataTable(List<string> values)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("string", typeof(string));
                if (values == null) return table;
                foreach (string item in values)
                {
                    table.Rows.Add(item);
                }

                return table;

            }
            catch (Exception ex)
            {
                ////AppLogger.Error("CreateDataTable.string", ex);
                return null;
            }
        }

        public static DataTable CreateDataTable(List<Guid> values)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("guid", typeof(Guid));
                if (values != null)
                {
                    foreach (Guid item in values)
                    {
                        table.Rows.Add(item);
                    }
                }

                return table;

            }
            catch (Exception ex)
            {
                //AppLogger.Error("CreateDataTable.Guid", ex);
                return null;
            }
        }

        public static void AddTableValuedParameter<T>(SqlCommand cmd, string name, IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            SqlParameter parameter = new SqlParameter
            {
                SqlDbType = SqlDbType.Structured,
                Value = table,
                ParameterName = name
            };

            cmd.Parameters.Add(parameter);
        }

        public static void AddTableValuedParameter(SqlCommand cmd, string parameterName, DataTable value)
        {
            SqlParameter sqlParam = new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = SqlDbType.Structured,
                Value = value
            };

            cmd.Parameters.Add(sqlParam);
        }
    }
}
