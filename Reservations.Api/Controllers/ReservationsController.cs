using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservations.Application.Interfaces;
using Reservations.Data.Models;

namespace Reservations.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService _service;

        public ReservationsController(IReservationsService service)
        {
            _service = service;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationItem>>> Get()
        {
            return Ok(await _service.GetReservations());
        }

        [HttpGet]
        [Route("lecturers")]
        public async Task<ActionResult<IEnumerable<LecturerItem>>> GetLecturerItems()
        {
            return Ok(await _service.GetLecturerItems());
        }

        [HttpGet]
        [Route("lecturerHalls")]
        public async Task<ActionResult<IEnumerable<LectureHallItem>>> GetLectureHallItems()
        {
            return Ok(await _service.GetLectureHallItems());
        }

        [HttpPut]
        public Task Put(NewReservationItem newReservationItem)
        {
            return _service.SaveReservation(newReservationItem);
        }
    }
}