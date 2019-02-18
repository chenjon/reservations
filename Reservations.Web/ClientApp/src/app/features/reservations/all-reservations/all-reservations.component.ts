import { Component, OnInit } from '@angular/core';
import { Reservation } from 'src/app/core/models/reservation';
import { CallApiService } from 'src/app/core/services/call-api.service';

@Component({
  selector: 'app-all-reservations',
  templateUrl: './all-reservations.component.html',
  styleUrls: ['./all-reservations.component.css']
})
export class AllReservationsComponent implements OnInit {
  reservations: Reservation[];

  constructor(private apiService: CallApiService) { }

  ngOnInit() {
    //this.reservations = Resevations;
    this.getReservations();
  }

  getReservations() {
    this.apiService.getReservations("Reservations")
      .subscribe(
        res => {
          this.reservations = res
      });
  }
}
