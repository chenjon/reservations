import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/api';
import { CallApiService } from 'src/app/core/services/call-api.service';
import { LectureHall } from 'src/app/core/models/lectureHall';
import { Lecturer } from 'src/app/core/models/lecturer';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-reservation',
  templateUrl: './new-reservation.component.html',
  styleUrls: ['./new-reservation.component.css']
})
export class NewReservationComponent implements OnInit {
  reservationform: FormGroup;
  submitted: boolean;
  lectureHalls: SelectItem[];
  lecturers: SelectItem[];
  
  constructor(private fb: FormBuilder,
    private apiService: CallApiService,
    private router: Router) { }

  ngOnInit() {
    this.reservationform = this.fb.group({
      'fromDate': new FormControl('', Validators.required),
      'toDate': new FormControl(''),
      'lecturerHall': new FormControl('', Validators.required),
      'lecturer': new FormControl('', Validators.required)
    });

    this.apiService.getLectureHalls("Reservations/lecturerHalls")
      .subscribe(
        res => {
            this.pushlectureHalls(res)
      });

    this.apiService.getLecturers("Reservations/lecturers")
      .subscribe(
        res => {
          this.pushlectures(res)
      });
  }

  pushlectureHalls(lectureHalls: LectureHall[]) {
    this.lectureHalls = [];
    this.lectureHalls.push({ label: `Select a lecture hall`, value: '' });
    let count = lectureHalls.length;
    for (var i = 0; i < count; i++) {
      this.lectureHalls.push({ label: lectureHalls[i].number.toString(), value: lectureHalls[i].number });
    }
  }

  pushlectures(lectureHalls: Lecturer[]) {
    this.lecturers = [];
    this.lecturers.push({ label: `Select a lecturer`, value: '' });
    let count = lectureHalls.length;
    for (var i = 0; i < count; i++) {
      this.lecturers.push({ label: `${lectureHalls[i].title} ${lectureHalls[i].name} ${lectureHalls[i].surname}`, value: lectureHalls[i].id });
    }
  }

  onSubmit(value: string) {
    this.submitted = true;

    let newReservation = {
      from: this.reservationform.value.fromDate,
      to: this.reservationform.value.toDate,
      lectureHallNumber: this.reservationform.value.lecturerHall,
      lecturerId: this.reservationform.value.lecturer
    };

    this.apiService.SaveReservation(newReservation)
      .subscribe(
        res => {
          this.router.navigate(['/reservations'])
      });

  }

  get diagnostic() { return JSON.stringify(this.reservationform.value); }

}
