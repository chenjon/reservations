import { Injectable, ErrorHandler, Injector } from '@angular/core';
import { LoggerService } from './logger.service';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {
  
  constructor(private injector: Injector) { }

  handleError(error: any): void {
    const logger = this.injector.get(LoggerService);
    logger.logError(error);
  }
}
