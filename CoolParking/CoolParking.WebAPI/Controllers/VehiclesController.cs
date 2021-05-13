using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolParking.BL.Services;
using CoolParking.BL.Models;
using CoolParking.BL.Interfaces;
using System.Text.RegularExpressions;

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private IParkingService Service { get; set; }
        public VehiclesController(IParkingService service)
        {
            Service = service;
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<Vehicle>> GetVehiclesCollection() => Ok(Service.GetVehicles());


        [HttpGet("{id}")]
        public ActionResult<Vehicle> GetVehiclesById(string id)
        {
            Regex regex = new Regex(@"[A-Z]{2}-\d{4}-[A-Z]{2}");
            if (!regex.IsMatch(id))
                return BadRequest("Id is incorrect");
            Vehicle vehicle = Vehicle.GetVehicleById(id);
            if (vehicle == null)
                return NotFound("Vehicle with this id is not found");
            return vehicle;
        }
        
        [HttpPost]
        public ActionResult<Vehicle> AddVehicle(string id, VehicleType vehicleType, decimal balance)
        {
            if (id == null || vehicleType == 0 || balance == 0)
                return BadRequest("No one or several needed parameters");
            Vehicle vehicle;
            try
            {
                vehicle = new Vehicle(id, vehicleType, balance);
                Service.AddVehicle(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Created("./CoolParking/Moderls/Parking.Vehicles", vehicle);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteVehicles(string id)
        {
            Regex regex = new Regex(@"[A-Z]{2}-\d{4}-[A-Z]{2}");
            if (!regex.IsMatch(id))
                return BadRequest("This id is incorrect");
            Vehicle vehicle = Vehicle.GetVehicleById(id);
            if (vehicle == null)
                return NotFound("Vehicle with this id is not found");
            Service.RemoveVehicle(id);
            return NoContent();
        }
    }
}
