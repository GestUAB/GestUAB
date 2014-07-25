//
// CustomMapper.cs
//
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
//
// Copyright (c) 2013 Tony Alexander Hild
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Dapper;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using GestUAB.Models;

namespace GestUAB.DataAccess.Mapping
{
    /// <summary>
    /// Uses the Name value of the ColumnAttribute specified, otherwise maps as usual.
    /// </summary>
    /// <typeparam name="T">The type of the object that this mapper applies to.</typeparam>
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {

        static ColumnAttributeTypeMapper()
        {
//            Dapper.SqlMapper.SetTypeMap(
//                typeof(MyModel),
//                new ColumnAttributeTypeMapper<MyModel>());
        }

        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
                {
                    new CustomPropertyTypeMap(
                        typeof(T),
                        (type, columnName) =>
                        type.GetProperties().FirstOrDefault(prop =>
                            prop.GetCustomAttributes(false)
                                .OfType<ColumnMappingAttribute>()
                                .Any(attr => attr.Name == columnName)
                            )
                        ),
                    new DefaultTypeMap(typeof(T))
            })
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnMappingAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }
    }

    public class CustomTypeMap : SqlMapper.ITypeMap
    {
        private readonly Dictionary<string, SqlMapper.IMemberMap> members
            = new Dictionary<string, SqlMapper.IMemberMap>(StringComparer.OrdinalIgnoreCase);
        public Type Type { get { return type; } }
        private readonly Type type;
        private readonly SqlMapper.ITypeMap tail;
        public void Map(string columnName, string memberName)
        {
            MemberInfo mi = type.GetMember(memberName).Single();
            if (mi.DeclaringType.IsAssignableFrom(typeof(IModel)))
            {
                mi = mi.DeclaringType.GetProperty("Id" );
            }
            members[columnName] = new MemberMap(mi, columnName);
        }
        public CustomTypeMap(Type type, SqlMapper.ITypeMap tail)
        {
            this.type = type;
            this.tail = tail;
        }
        public System.Reflection.ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return tail.FindConstructor(names, types);
        }

        public SqlMapper.IMemberMap GetConstructorParameter(
            System.Reflection.ConstructorInfo constructor, string columnName)
        {
            return tail.GetConstructorParameter(constructor, columnName);
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            SqlMapper.IMemberMap map;
            if (!members.TryGetValue(columnName, out map))
            { // you might want to return null if you prefer not to fallback to the
                // default implementation
                map = tail.GetMember(columnName);
            }
            return map;
        }
    }

    public class MemberMap : SqlMapper.IMemberMap
    {
        private readonly MemberInfo member;
        private readonly string columnName;
        public MemberMap(MemberInfo member, string columnName)
        {
            this.member = member;
            this.columnName = columnName;
        }
        public string ColumnName { get { return columnName; } }
        public System.Reflection.FieldInfo Field { get { return member as FieldInfo; } }
        public Type MemberType { get {
                switch (member.MemberType)
                {
                    case MemberTypes.Field: return ((FieldInfo)member).FieldType;
                    case MemberTypes.Property: return ((PropertyInfo)member).PropertyType;
                    default: throw new NotSupportedException();
                }
            } }
        public System.Reflection.ParameterInfo Parameter { get { return null; } }
        public System.Reflection.PropertyInfo Property { get { return member as PropertyInfo; } }
    }
}

