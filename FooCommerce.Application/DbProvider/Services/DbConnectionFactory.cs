﻿using System.Data;
using FooCommerce.Application.DbProvider.Interfaces;
using Microsoft.Data.SqlClient;

namespace FooCommerce.Application.DbProvider.Services;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}