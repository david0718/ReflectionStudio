﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ReflectionStudio.Core.Database.SQL2KProvider
{
	internal class SqlQuery : IDbSQLQuery
	{
		public DataTable ExecuteData(string connectionString, string commandText)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				SqlCommand command = new SqlCommand(commandText, connection);
				SqlDataAdapter adapter = new SqlDataAdapter();
				DataSet set = new DataSet();
				command.CommandType = CommandType.Text;
				adapter.SelectCommand = command;
				adapter.Fill(set);

				adapter.Dispose();
				command.Dispose();

				return set.Tables[0];
			}
		}

		public IDataReader ExecuteReader(string connectionString, string commandText)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			{
				connection.Open();

				SqlCommand command = new SqlCommand(commandText, connection);
				command.CommandType = CommandType.Text;

				SqlDataReader reader = command.ExecuteReader();
				command.Dispose();
			
				return reader;
			}
		}
	}
}
