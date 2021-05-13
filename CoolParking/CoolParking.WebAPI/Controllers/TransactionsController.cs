using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolParking.BL.Services;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System.Text.RegularExpressions;

namespace CoolParking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private IParkingService Service;
        public TransactionsController(IParkingService service)
        {
            Service = service;
        }

        [HttpGet]
        [Route("last")]
        public ActionResult<IReadOnlyCollection<TransactionInfo>> GetLatsTransactions()
        {
            return Ok(Service.GetLastParkingTransactions());
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<string> AllTransactions()
        {
            if (!System.IO.File.Exists(Settings.LogPath))
                return NotFound("Log file not found");
            return Ok(Service.ReadFromLog());
        }

        [HttpPut]
        [Route("topUpVehicle")]
        public ActionResult<Vehicle> TopUpVehicle(string id, decimal sum)
        {
            if (sum <= 0)
                return BadRequest("The sum can not be less than zero");
            Vehicle vehicle = Vehicle.GetVehicleById(id);
            if (vehicle == null)
                return NotFound("Vehicle with this id did not found");
            Service.TopUpVehicle(id, sum);
            return Ok(vehicle);
        }
    }
}
