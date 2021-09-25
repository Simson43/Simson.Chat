import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class LogService {

  public debug(message: any, ...args: any[]): void {
    console.debug(message, ...args);
  }
  
  public info(message: any, ...args: any[]): void {
    console.info(message, ...args);
  }

  public warn(message: any, ...args: any[]): void {
    console.warn(message, ...args);
  }

  public error(message: any, ...args: any[]): void {
    console.error(message, ...args);
  }
}
