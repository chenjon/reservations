using Reservations.Application.Interfaces;
using Reservations.Data.Db;
using Reservations.Data.Entities;
using Reservations.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservations.Application.Reservations
{
    public class ReservationsService : IReservationsService
    {
        private readonly IDatabase _database;
        public ReservationsService(IDatabase database)
        {
            _database = database;
        }

        #region public
        public async Task<IList<ReservationItem>> GetReservations()
        {
            return await Task.Run(() => 
                ConvertToReservationItem(_database.Reservations)
            );
        }

        public Task SaveReservation(NewReservationItem reservation)
        {
            var latestId = _database.Reservations.Max(r => r.Key);
            var lectureHall = _database.LectureHalls.Where(h => h.Key == reservation.LectureHallNumber);
            var lecturer = _database.Lecturers.Where(l => l.Key == reservation.LecturerId);
            latestId++;
            var res = new Reservation()
            {
                 Id = latestId,
                 From = reservation.From,
                 To = reservation.To,
                 Hall = lectureHall?.FirstOrDefault().Value,
                 Lecturer = lecturer?.FirstOrDefault().Value
            };

            _database.Reservations.Add(latestId, res);

            return Task.FromResult(0);
        }

        public async Task<IList<LectureHallItem>> GetLectureHallItems()
        {
            return await Task.Run(() =>
                ConvertToLectureHallItem(_database.LectureHalls)
            );
        }

        public async Task<IList<LecturerItem>> GetLecturerItems()
        {
            return await Task.Run(() =>
                ConvertToLecturerItem(_database.Lecturers)
            );
        }
        #endregion

        #region private
        private IList<ReservationItem> ConvertToReservationItem(IDictionary<int, Reservation> reservations)
        {
            IList<ReservationItem> reservationItems = new List<ReservationItem>();
            foreach(Reservation reservation in reservations.Values)
            {
                ReservationItem reservationItem = new ReservationItem()
                {
                    From = reservation.From,
                    To = reservation.To,
                    Id = reservation.Id,
                    LectureHallNumber = reservation.Hall.Number,
                    Lecturer = $"{reservation.Lecturer.Title} {reservation.Lecturer.Name} {reservation.Lecturer.Surname}"
                };

                reservationItems.Add(reservationItem);
            }

            return reservationItems;
        }

        private IList<LectureHallItem> ConvertToLectureHallItem(IDictionary<int, LectureHall> lectureHalls)
        {
            IList<LectureHallItem> lectureHallItems = new List<LectureHallItem>();
            foreach (LectureHall hall in lectureHalls.Values)
            {
                LectureHallItem lectureHallItem = new LectureHallItem()
                {
                    Number = hall.Number,
                    Capacity = hall.Capacity
                };

                lectureHallItems.Add(lectureHallItem);
            }

            return lectureHallItems;
        }

        private IList<LecturerItem> ConvertToLecturerItem(IDictionary<int, Lecturer> lecturers)
        {
            IList<LecturerItem> lectureHallItems = new List<LecturerItem>();
            foreach (Lecturer lecturer in lecturers.Values)
            {
                LecturerItem lecturerItem = new LecturerItem()
                {
                    Id = lecturer.Id,
                    Title = lecturer.Title,
                    Name = lecturer.Name,
                    Surname = lecturer.Surname
                };

                lectureHallItems.Add(lecturerItem);
            }

            return lectureHallItems;
        }
        #endregion
    }
}
