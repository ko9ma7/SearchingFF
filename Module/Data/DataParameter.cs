using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MCF.Classes.Data
{
    /// <summary>
    /// 데이터 파라미터
    /// </summary>
    [Serializable]
    public class DataParameter : IDataParameter
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="name">파라미터 명</param>
        /// <param name="value">파라미터 값</param>
        public DataParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
            this.Direction = ParameterDirection.Input;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="name">파라미터 명</param>
        /// <param name="value">파라미터 값</param>
        /// <param name="dbType">타입</param>
        public DataParameter(string name, DbType dbType, object value)
        {
            this.Name = name;
            this.Value = value;
            this.DbType = dbType;
            this.Direction = ParameterDirection.Input;
        }

        public DataParameter(string name, DbType dbType, ParameterDirection direction, int size)
        {
            this.Name = name;
            this.Value = DBNull.Value;
            this.DbType = dbType;
            this.Size = size;
            this.Direction = direction;
            this.SourceVersion = (direction == ParameterDirection.Output) ? DataRowVersion.Current : DataRowVersion.Default;
        }

        #region IDataParameter 멤버

        private DbType _dbType;

        /// <summary>
        /// 타입
        /// </summary>
        public DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        private ParameterDirection _direction;

        /// <summary>
        /// 파라미터구분
        /// </summary>
        public ParameterDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private bool _isNullable;

        /// <summary>
        /// Null 여부
        /// </summary>
        public bool IsNullable
        {
            get { return _isNullable; }
            set { _isNullable = value; }
        }

        private string _name;

        /// <summary>
        /// 파라미터명
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 파라미터명
        /// </summary>
        public string ParameterName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        private string _sourceColumn;

        /// <summary>
        /// 매핑컬럼
        /// </summary>
        public string SourceColumn
        {
            get { return _sourceColumn; }
            set { _sourceColumn = value; }
        }

        private DataRowVersion _sourceVersion;

        /// <summary>
        /// 데이터로우 버전
        /// </summary>
        public DataRowVersion SourceVersion
        {
            get { return _sourceVersion; }
            set { _sourceVersion = value; }
        }

        private object _value;

        /// <summary>
        /// 값
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// 값
        /// </summary>
        object IDataParameter.Value
        {
            get { return this.Value; }
            set { this.Value = value; }
        }

        private int _size;

        /// <summary>
        /// 사이즈
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        #endregion
    }
}
