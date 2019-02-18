using Reservations.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reservations.Application.Interfaces
{
    public interface IReservationsService
    {
        Task<IList<ReservationItem>> GetReservations();
        Task SaveReservation(NewReservationItem reservation);

        Task<IList<LecturerItem>> GetLecturerItems();
        Task<IList<LectureHallItem>> GetLectureHallItems();
    }
}
