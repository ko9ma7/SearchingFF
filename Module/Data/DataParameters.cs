using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;

namespace MCF.Classes.Data
{
    /// <summary>
    /// 데이터 파라미터 컬렉션
    /// </summary>
    [Serializable]
    public class DataParameters : IEnumerable<DataParameter>
    {
        private List<DataParameter> _parameters;

        #region Properties

        /// <summary>
        /// SQL 파라미터 배열
        /// </summary>
        public MySqlParameter[] MySqlParameters
        {
            get 
            {
                MySqlParameter[] reparams = new MySqlParameter[_parameters.Count];

                for (int i = 0; i < reparams.Length; i++)
                {
                    reparams[i] = ToMySqlParameter(_parameters[i]);
                }
                return reparams;
               
            }
        }

        /// <summary>
        /// 파라미터의 카운트 수
        /// </summary>
        public int Count
        {
            get { return _parameters.Count; }
        }

        #endregion

        #region Indexer

        /// <summary>
        /// 인덱스
        /// </summary>
        /// <param name="index">인덱스번호</param>
        /// <returns>데이터 파라미터</returns>
        public DataParameter this[int index]
        {
            get { return _parameters[index]; }
        }

        /// <summary>
        /// 인덱스
        /// </summary>
        /// <param name="name">파라미터명</param>
        /// <returns>데이터 파라미터</returns>
        public DataParameter this[string name]
        {
            get
            {
                foreach (DataParameter parameter in _parameters)
                    if (parameter.Name.Equals(name))
                        return parameter;

                throw new Exception(string.Format("[{0}] 파라미터를 찾을 수 없습니다", name));
            }
        }

        /// <summary>
        /// 해당 파라미터가 있는지 여부를 판단합니다.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            foreach (DataParameter parameter in _parameters)
                if (parameter.Name.Equals(name))
                    return true;
            return false;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 생성자
        /// </summary>
        public DataParameters()
        {
            _parameters = new List<DataParameter>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 데이터 파라미터 추가
        /// </summary>
        /// <param name="parameter">데이터 파라미터</param>
        public void Add(DataParameter parameter)
        {
            _parameters.Add(parameter);
        }

        /// <summary>
        /// Oracle 파리미터로 변환
        /// </summary>
        /// <param name="parameter">데이터파라미터</param>
        /// <returns>Oracle 파리미터</returns>
        public MySqlParameter ToMySqlParameter(DataParameter parameter)
        {
            MySqlParameter sqlParameter = new MySqlParameter();
            sqlParameter.ParameterName = parameter.ParameterName;

            if (parameter.Direction == ParameterDirection.Output)
                sqlParameter.Size = parameter.Size;

            sqlParameter.Direction = parameter.Direction;

            // 값이 없으면 NULL로 전달
            //if (string.IsNullOrEmpty(parameter.Value.ToString()))
            //{
            //    sqlParameter.Value = DBNull.Value;
            //    //sqlParameter.OracleValue = DBNull.Value;
            //}
            //else
            //{
                sqlParameter.Value = parameter.Value;
                //sqlParameter.OracleValue = parameter.Value;
            //}

            return sqlParameter;
        }

        /// <summary>
        /// Oracle 파리미터들
        /// </summary>
        /// <returns>Oracle 파라미터 배열</returns>
        public MySqlParameter[] ToMySqlParameter()
        {
            MySqlParameter[] parameters = new MySqlParameter[_parameters.Count];

            int index = 0;

            foreach (DataParameter parameter in _parameters)
            {
                parameters[index] = ToMySqlParameter(parameter);
                index++;
            }

            return parameters;
        }

        private MySqlDbType ToMySqlType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                    return MySqlDbType.Int64;
                default:
                    return MySqlDbType.VarChar;
            }
        }

        /// <summary>
        /// 데이터 파라미터 추가
        /// </summary>
        /// <param name="name">파라미터명</param>
        /// <param name="value">파라미터값</param>
        public void Add(string name, object value)
        {
            Add(new DataParameter(name, value));
        }

        /// <summary>
        /// 데이터 파라미터 추가
        /// </summary>
        /// <param name="name">파라미터명</param>
        /// <param name="value">파라미터값</param>
        /// <param name="dbType">디비타입</param>
        public void Add(string name, DbType dbType, object value)
        {
            Add(new DataParameter(name, dbType, value));
        }

        /// <summary>
        /// 데이터 파라미터 추가
        /// </summary>
        /// <param name="name">파라미터명</param>
        /// <param name="direction">다이렉션</param>
        /// <param name="dbType">디비타입</param>
        public void Add(string name, DbType dbType, ParameterDirection direction, int size)
        {
            Add(new DataParameter(name, dbType, direction, size));
        }

        /// <summary>
        /// 아웃풋 파라미터 추가
        /// </summary>
        /// <param name="name">파라미터명</param>
        /// <param name="dbType">디비타입</param>
        public void Out(string name, DbType dbType)
        {
            Add(new DataParameter(name, dbType, ParameterDirection.Output, int.MaxValue));
        }

        /// <summary>
        /// 아웃풋 파라미터 값
        /// </summary>
        /// <param name="name">아웃풋 파라미터명</param>
        /// <returns>아웃풋 값</returns>
        public object OutValue(string name)
        {
            foreach (MySqlParameter parameter in this.MySqlParameters)
                if (parameter.ParameterName.Equals(name) && parameter.Direction == ParameterDirection.Output)
                    return parameter.Value;

            throw new Exception(string.Format("[{0}] 파라미터를 찾을 수 없습니다", name));
        }

        /// <summary>
        /// 아웃풋값 비교
        /// </summary>
        /// <param name="name">아웃풋 파라미터명</param>
        /// <param name="value">비교값</param>
        /// <returns>동일한지</returns>
        public bool OutValueCompare(string name, object value)
        {
            return OutValue(name).ToString().Equals(value.ToString());
        }

        /// <summary>
        /// 데이터 파라미터 클리어
        /// </summary>
        public void Clear()
        {
            if (_parameters != null)
                _parameters.Clear();            
        }

        #endregion

        #region IEnumerable<DataParameter> 멤버

        IEnumerator<DataParameter> IEnumerable<DataParameter>.GetEnumerator()
        {
            for (int i = 0; i < _parameters.Count; i++)
                yield return _parameters[i];
        }

        #endregion

        #region IEnumerable 멤버

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<DataParameter>)this).GetEnumerator();
        }

        #endregion
    }
}
