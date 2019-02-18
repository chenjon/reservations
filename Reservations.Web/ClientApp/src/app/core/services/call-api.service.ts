import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Reservation } from '../models/reservation';
import { Observable, iif, throwError, of } from 'rxjs';
import { catchError, map, retryWhen, concatMap, delay } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { LectureHall } from '../models/lectureHall';
import { Lecturer } from '../models/lecturer';
import { NewReservation } from '../models/NewReservation';

const APIBASE_URL = environment.apiBaseUrl;
const RETRY_COUNT = environment.retryCount;
const RETRY_INTERVAL = environment.retryInterval;
const retryPipeline = 
    // using retryWhen to handle errors
    retryWhen(errors => errors.pipe(
      // Use concat map to keep the errors in order and make sure they aren't executed in parallel
      concatMap((e, i) => 
        // Executes a conditional Observable depending on the result of the first argument
        iif(
          () => i > RETRY_COUNT,
          // If the condition is true we throw the error (the last error)
          throwError(e),
          // Otherwise we pipe this back into our stream and delay the retry
          of(e).pipe(delay(RETRY_INTERVAL)) 
      )
    )
  )
);
@Injectable({
  providedIn: 'root'
})
export class CallApiService {

  constructor(private http: HttpClient) { }

  public getReservations(action: string): Observable<Reservation[]> {
    let apiUrl = APIBASE_URL + action;
    return this.http.get<Reservation[]>(apiUrl)
      .pipe(
        map(resp => <Reservation[]>resp),
        retryPipeline,
        catchError(this.formatErrors)
      )
  }

  public getLectureHalls(action: string): Observable<LectureHall[]> {
    let apiUrl = APIBASE_URL + action;
    return this.http.get<LectureHall[]>(apiUrl)
      .pipe(
      map(resp => <LectureHall[]>resp),
        retryPipeline,
        catchError(this.formatErrors)
      )
  }

  public getLecturers(action: string): Observable<Lecturer[]> {
    let apiUrl = APIBASE_URL + action;
    return this.http.get<Lecturer[]>(apiUrl)
      .pipe(
      map(resp => <Lecturer[]>resp),
        retryPipeline,
        catchError(this.formatErrors)
      )
  }

  public SaveReservation(newReservation: NewReservation) {
    let apiUrl = APIBASE_URL + "Reservations";
    return this.http.put(apiUrl, newReservation)
      .pipe(
        retryPipeline,
        catchError(this.formatErrors)
      )
  }

  public formatErrors(error: any): Observable<any> {
    return throwError(error);
  }
}
