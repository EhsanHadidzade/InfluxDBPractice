﻿using System;
using System.Threading.Tasks;
using InfluxDB.Client;
using Microsoft.Extensions.Configuration;

namespace app.Services
{
    public class InfluxDBService : IInfluxDBService
    {
        private readonly string _token;

        public InfluxDBService(IConfiguration configuration)
        {
            _token = configuration.GetValue<string>("InfluxDB:Token");
        }

        public void Write(Action<WriteApi> action)
        {
            var client = InfluxDBClientFactory.Create("http://localhost:8086", _token);
            var write = client.GetWriteApi();
            action(write);
        }

        public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
        {
            using var client = InfluxDBClientFactory.Create("http://localhost:8086", _token);
            var query = client.GetQueryApi();
            return await action(query);
        }
    }


    public interface IInfluxDBService
    {
        void Write(Action<WriteApi> action);
        Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action);
    }
}