import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class ConfigService {

  public AppSettings: any;

  constructor(
    private http: HttpClient
  ) {

  }

  public async init(): Promise<void> {
    this.AppSettings = await this.http.get('/assets/configs/appsettings.json').toPromise();
  }

}
