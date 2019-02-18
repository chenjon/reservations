using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reservations.Application.Interfaces;
using Reservations.Application.Reservations;
using Reservations.Data.Db;
using Reservations.Data.Entities;
using Reservations.Data.Models;
using System;
using System.Threading.Tasks;

namespace Reservations.Tests
{
    [TestClass]
    public class ReservationsTests
    {
        IDatabase _database;

        [TestInitialize()]
        public void Initialize()
        {
            _database = new InMemoryDatabase();
            var lh101 = new LectureHall
            {
                Number = 101,
                Capacity = 100
            };
            _database.LectureHalls.Add(101, lh101);
            var l1 = new Lecturer()
            {
                Id = 1,
                Name = "John",
                Surname = "Nash",
                Title = "prof. ",
            };
            _database.Lecturers.Add(1, l1);
            var r1 = new Reservation
            {
                Id = 1,
                From = new DateTime(2015, 1, 2, 8, 0, 0),
                To = new DateTime(2015, 1, 2, 8, 0, 0).AddHours(2),
                Hall = lh101,
                Lecturer = l1
            };
            _database.Reservations.Add(1, r1);
        }

        [TestMethod]
        public void can_add_reservation()
        {
            //arrange
            NewReservationItem newRes = new NewReservationItem
            {
                From = DateTime.Now,
                To = DateTime.Now.AddDays(10),
                LectureHallNumber = 101,
                LecturerId = 1
            };
            IReservationsService service = new ReservationsService(_database);

            //add a new reservation
            service.SaveReservation(newRes);
            var reservations = _database.Reservations;

            //assert
            Assert.IsTrue(reservations.Count == 2);
        }

        [TestMethod]
        public void can_list_reservations()
        {
            //arrange
            IReservationsService service = new ReservationsService(_database);

            //get all reservations
            var reservations = Task.Run(async() => await service.GetReservations());
            var results = reservations.Result;

            //assert
            Assert.IsNotNull(results);
            Assert.IsTrue(reservations.Result.Count == 1);

        }
    }
}
