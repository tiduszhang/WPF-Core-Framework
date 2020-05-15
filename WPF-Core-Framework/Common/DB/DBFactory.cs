
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;

namespace Common
{
    /// <summary>
    /// ���ݿ⹤��
    /// </summary>
    public static class DBFactory
    {
        /// <summary>
        /// DB�๤��
        /// </summary>
        private static DbProviderFactory DbProviderFactory = null;

        //���ӳ�
        private static List<DbConnection> _lstconnection;

        /// <summary>
        /// �����ӳ��л�ȡ���Ӷ���
        /// </summary>
        internal static DbConnection Connection
        {
            get
            {

                if (_lstconnection == null)
                {
                    _lstconnection = new List<DbConnection>();
                }
                if (_lstconnection.Count > 0)
                {
                    int i = _lstconnection.Count - 1;
                    for (i = _lstconnection.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            if (_lstconnection[i].State == ConnectionState.Closed
                                || _lstconnection[i].State == ConnectionState.Broken)
                            {
                                if (_lstconnection[i].State == ConnectionState.Broken)
                                {
                                    _lstconnection[i].Close();
                                }
                                _lstconnection[i].Dispose();
                                _lstconnection.RemoveAt(i);
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                DbConnection _connection = null;
                _connection = CreateConnection();
                _lstconnection.Add(_connection);
                return _connection;
            }
        }


        /// <summary>
        /// GetDataAdapter
        /// </summary>
        /// <returns></returns>
        internal static DbDataAdapter GetDataAdapter()
        {
            return DbProviderFactory.CreateDataAdapter();
        }

        /// <summary>
        /// CreateCommand
        /// </summary>
        /// <returns></returns>
        internal static DbCommand GetCommand()
        {
            return DbProviderFactory.CreateCommand();
        }

        /// <summary>
        /// CreateConnection
        /// </summary>
        /// <returns></returns>
        internal static DbConnection CreateConnection()
        {
            var conn = DbProviderFactory.CreateConnection();
            conn.ConnectionString = ConnectionString;
            return conn;
        }

        /// <summary>
        /// CreateTransaction
        /// </summary>
        /// <returns></returns>
        public static DbTransaction CreateTransaction()
        {
            var conn = Connection;
            conn.ConnectionString = ConnectionString;
            return conn.BeginTransaction();
        }

        /// <summary>
        /// CreateCommandBuilder
        /// </summary>
        /// <returns></returns>
        internal static DbCommandBuilder CreateCommandBuilder()
        {
            return DbProviderFactory.CreateCommandBuilder();
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        internal static string ConnectionString { get; set; }

        /// <summary>
        /// ��ʼ��DBFactory
        /// </summary> 
        /// <param name="dbProviderFactory"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool Initialization(DbProviderFactory dbProviderFactory, string connectionString)
        {
            try
            {
                using (var conn = dbProviderFactory.CreateConnection())
                {
                    conn.ConnectionString = connectionString;
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
            DbProviderFactory = dbProviderFactory;
            ConnectionString = connectionString;
            return true;
        }


        /// <summary>
        /// ��ȡDBFunction
        /// </summary>
        /// <returns></returns>
        public static DBFunction GetDbFunction()
        {
            return new DBFunction();
        }

        /// <summary>
        /// ��ȡDBFunction
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static DBFunction GetDbFunction(DbTransaction trans)
        {
            return new DBFunction(trans);
        }

        /// <summary>
        /// Ҫ��������dt.TableName Ϊ��Ҫ������߸��µı�����
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool UpdateDB(this DataTable dt)
        {
            using (var dBFunction = GetDbFunction())
            {
                return dBFunction.UpdateDB(dt);
            }
        }

        /// <summary>
        /// Ҫ��������dt.TableName Ϊ��Ҫ������߸��µı�����
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static bool UpdateDB(this DataTable dt, DbTransaction trans)
        {
            var dBFunction = GetDbFunction(trans);
            return dBFunction.UpdateDB(dt);
        }

        public static bool ExecCommandForBool(this string strCommandText)
        {
            using (var dBFunction = GetDbFunction())
            {
                return dBFunction.ExecCommandForBool(strCommandText);
            }
        }

        public static bool ExecCommandForBool(this string strCommandText, DbTransaction trans)
        {
            var dBFunction = GetDbFunction(trans);
            return dBFunction.ExecCommandForBool(strCommandText);
        }

        public static int ExecCommandForCount(this string strCommandText)
        {
            using (var dBFunction = GetDbFunction())
            {
                return dBFunction.ExecCommandForCount(strCommandText);
            }
        }

        public static int ExecCommandForCount(this string strCommandText, DbTransaction trans)
        {
            var dBFunction = GetDbFunction(trans);
            return dBFunction.ExecCommandForCount(strCommandText);
        }

        public static DataTable ExecCommandForTable(this string strCommandText)
        {
            using (var dBFunction = GetDbFunction())
            {
                return dBFunction.ExecCommandForTable(strCommandText);
            }
        }

        public static DataTable ExecCommandForTable(this string strCommandText, DbTransaction trans)
        {
            var dBFunction = GetDbFunction(trans);
            return dBFunction.ExecCommandForTable(strCommandText);
        }



        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static List<T> GetData<T>(this string Sql, CommandType cmdType = CommandType.Text, params DbParameter[] commandParameters) where T : class, new()
        {
            using (var dBFunction = GetDbFunction())
            {
                return dBFunction.GetData<T>(Sql, cmdType, commandParameters);
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sql"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static List<T> GetData<T>(this string Sql, DbTransaction trans, CommandType cmdType = CommandType.Text, params DbParameter[] commandParameters) where T : class, new()
        {
            var dBFunction = GetDbFunction(trans);
            return dBFunction.GetData<T>(Sql, cmdType, commandParameters);
        }


    }
}