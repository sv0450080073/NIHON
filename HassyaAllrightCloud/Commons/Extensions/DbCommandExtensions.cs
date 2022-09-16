using Microsoft.Data.SqlClient;
using System;
using System.Data.Common;

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class DbCommandExtensions
    {
        public static void AddParam(this DbCommand dBCommand, string propName, object propVal)
        {
            var param = dBCommand.CreateParameter();
            param.ParameterName = propName;
            param.Value = propVal;
            dBCommand.Parameters.Add(param);
        }

        public static void AddOutputParam(this DbCommand dBCommand, string propName, System.Data.DbType type = System.Data.DbType.Int32)
        {
            var param = dBCommand.CreateParameter();
            param.ParameterName = propName;
            param.Direction = System.Data.ParameterDirection.Output;
            param.DbType = type;
            dBCommand.Parameters.Add(param);
        }

        public static SqlParameter AddParameter(SqlParameterCollection parameters, string paramName, dynamic value)
        {
            return value == null ? parameters.AddWithValue(paramName, DBNull.Value) : parameters.AddWithValue(paramName, value);
        }
    }

}
