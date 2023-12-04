using app.Services;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Writes;
using InfuxDBPractice.DTOs;
using LibInfluxDB.Net;
using LibInfluxDB.Net.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace InfuxDBPractice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IInfluxDBService _influxDBService;
        private static readonly Random _random = new Random();


        public ProductController(IInfluxDBService influxDBService)
        {
            _influxDBService = influxDBService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(long postId)
        {



            var token = "3NzV3eeeznGgpbWv7nXGDjIdb8209kyAOcriKgAZBjReIBfHQt6lQzmeCFOqnryphpU4lH2JCFR-GCgWe-C7mA==";
            string bucket = "shoplonDev";
            string org = "influxdb";

            using var client = new InfluxDBClient("http://localhost:8086", token);


            _ = Task.Run(async () =>
            {

                using var clientNew = new InfluxDBClient("http://localhost:8086", token);
                for (int i = 1; i < 100; i++)
                {
                    var view = new PageView(i);
                    using (var writeApi = clientNew.GetWriteApi())
                    {
                        writeApi.WriteMeasurement(view, WritePrecision.Ms, bucket, org);
                    }
                }

            });
            //await SetView(25, client, bucket, org);
            //var query = "from(bucket: \"shoplonDev\") |> range(start: -1h)";
            var query = "from(bucket: \"shoplonDev\") |> range(start: -1h) |> filter(fn: (r) => r[\"_measurement\"] == \"PostView\")";
            var queryPage = "from(bucket: \"shoplonDev\") |> range(start: -1d) |> filter(fn: (r) => r[\"_measurement\"] == \"PageView\")";

            var tables = await client.GetQueryApi().QueryAsync(queryPage, org);



            return Ok(tables.SelectMany(x => x.Records).Count());

        }


        private async Task SetView(int amount, InfluxDBClient client, string bucket, string org)
        {
            for (int i = 1; i < amount; i++)
            {
                var view = new PageView(i);
                using (var writeApi = client.GetWriteApi())
                {
                    writeApi.WriteMeasurement(view, WritePrecision.Ms, bucket, org);
                }
            }
        }
    }


    [Measurement("PostView")]
    public class PostView
    {


        [Column("PostId")]
        public long PostId { get; set; }


        [Column(IsTimestamp = true)]
        public DateTime Time { get; set; }

        public PostView(long postId)
        {
            PostId = postId;
            Time = DateTime.UtcNow;
        }
    }

    [Measurement("PageView")]
    public class PageView
    {


        [Column("PageId")]
        public long PageId { get; set; }


        [Column(IsTimestamp = true)]
        public DateTime Time { get; set; }

        public PageView(long pageId)
        {
            PageId = pageId;
            Time = DateTime.UtcNow;
        }
    }
}
