using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

/// <summary>
/// ���ݿ�Ĺ�ͨ����
/// </summary>
public class DBFunction : IDisposable
{
    private DbConnection _connection;
    private DataAdapter _dataAdapter;
    private DbCommand _dbcmd;

    #region ��ʼ��
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public DBFunction()
    {
        _dataAdapter = DBFactory.GetDataAdapter();
        _connection = DBFactory.Connection;
        _dbcmd = DBFactory.GetCommand();
        _dbcmd.Connection = _connection;
    }

    public DBFunction(DbTransaction trans)
    {
        _dataAdapter = DBFactory.GetDataAdapter();
        _connection = trans.Connection as DbConnection;
        _dbcmd = DBFactory.GetCommand();
        _dbcmd.Connection = _connection;
        _dbcmd.Transaction = trans;
    }

    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(true);
    }

    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        if (_dataAdapter != null)
        {
            _dataAdapter.Dispose();
            _dataAdapter = null;
        }

        if (_dbcmd != null)
        {
            _dbcmd.Dispose();
            _dbcmd = null;
        }

        //WinForm
        if (_connection != null) //WinForm
        {
            if (_connection.State == ConnectionState.Broken
                || _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
    }
    #endregion

    #region ִ��һ��sql���
    /// <summary>
    /// ִ��һ��sql��䷵��
    /// </summary>
    public bool ExecCommandForBool(String strCommandText)
    {
        try
        { 
            _dbcmd.CommandText = strCommandText;
            _dbcmd.CommandType = CommandType.Text;
            _dbcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ex.ToString();
            return false;
        }
        return true;
    }


    /// <summary>
    /// ִ��һ��sql��䷵����Ӱ������
    /// </summary>
    public int ExecCommandForCount(String strCommandText)
    {
        int ReturnCount = 0;
        try
        { 
            _dbcmd.CommandText = strCommandText;
            _dbcmd.CommandType = CommandType.Text;
            ReturnCount = _dbcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
        return ReturnCount;
    }

    /// <summary>
    /// ִ��һ��sql��䷵��List<Dictionary<string, object>> ��ֵ���б� ����Ҫӳ�������ݱ�ʵ���ࡣ
    /// ע�⣺����ֶ�Ҫ����������
    /// ���飺ͨ��JSON������ConvertToDynamicObject����ת����Dynamic����ʹ�á�
    /// </summary>
    /// <param name="strCommandText"></param>
    /// <returns></returns>
    public List<Dictionary<string, object>> ExecCommandForDictionaryData(String strCommandText)
    {
        try
        {
            _dbcmd.CommandText = strCommandText;
            _dbcmd.CommandType = CommandType.Text;
            var reader = _dbcmd.ExecuteReader();
            List<Dictionary<string, object>> lstData = new List<Dictionary<string, object>>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Dictionary<string, object> lstFieldData = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        lstFieldData.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    lstData.Add(lstFieldData);
                }
            }
            return lstData;
        }
        catch (Exception ex)
        {
            ex.ToString();
            return null;
        }
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Sql"></param>
    /// <param name="cmdType"></param>
    /// <param name="commandParameters"></param>
    /// <returns></returns>
    public List<T> GetData<T>(string Sql, CommandType cmdType = CommandType.Text, params DbParameter[] commandParameters) where T : class, new()
    {
        _dbcmd.CommandText = Sql;
        _dbcmd.CommandType = cmdType;
        if (commandParameters != null)
        {
            foreach (DbParameter parm in commandParameters)
            {
                _dbcmd.Parameters.Add(parm);
            }
        }
        var dataReader = _dbcmd.ExecuteReader();
        List<T> lstData = new List<T>();
        Type type = typeof(T);
        var properties = type.GetProperties();
        if (dataReader.HasRows)
        {
            while (dataReader.Read())
            {
                T objValue = Activator.CreateInstance<T>();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    var propertie = properties.FirstOrDefault(o => o.Name.ToLower() == dataReader.GetName(i).ToLower());
                    if (propertie != null && dataReader.GetValue(i) != DBNull.Value)
                    {
                        propertie.SetValue(objValue, dataReader.GetValue(i), null);
                    }
                }
                lstData.Add(objValue);
            }
        }
        return lstData;
    }


    /// <summary>
    /// ִ��һ��sql��䷵��DataTable
    /// </summary>
    /// <param name="strCommandText"></param>
    /// <returns></returns>
    public DataTable ExecCommandForTable(String strCommandText)
    {
        try
        {
            DataTable data = new DataTable();

            _dbcmd.CommandText = strCommandText;
            _dbcmd.CommandType = CommandType.Text;

            _dataAdapter.SelectCommand = _dbcmd;
            _dataAdapter.Fill(data);
            return data;
        }
        //catch (System.Data.Common.DbException dbex)
        //{
        //    if (ex.Number == 8152)
        //    {
        //        return new DataTable();
        //    }
        //    else
        //    {
        //        throw dbex;
        //    }
        //}
        catch (Exception ex)
        {
            ex.ToString();
            return new DataTable();
        }
    }

    /// <summary>
    /// Ҫ��������dt.TableName Ϊ��Ҫ������߸��µı�����
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public bool UpdateDB(DataTable dt)
    {
        if (_dataAdapter == null)
        {
            throw new System.ObjectDisposedException(GetType().FullName);
        }
        var commandBuilder = DBFactory.CreateCommandBuilder();
        commandBuilder.DataAdapter = _dataAdapter;
        _dbcmd.CommandType = CommandType.Text;
        _dbcmd.CommandText = " select * from " + dt.TableName + " where 1=2 ";
        _dataAdapter.SelectCommand = _dbcmd;
        _dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
        _dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
        _dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
        _dataAdapter.Update(dt);
        if (dt.HasErrors)
        {
            dt.GetErrors()[0].ClearErrors();
            return false;
        }
        else
        {
            dt.AcceptChanges();
            return true;
        }
    }

    #endregion

}
