import { Injectable, Injector } from "@angular/core";
import * as signalR from "@aspnet/signalr";
import { ConfigService } from "../config.service";
import { LogService } from "../log.service";

@Injectable({
  providedIn: "root"
})
export class SignalrService {
  private connection: signalR.HubConnection;

  public get connected(): boolean {
    return this.connection.state == signalR.HubConnectionState.Connected;
  }
  private logger: LogService;

  constructor(
    injector: Injector,
    url: string
  ) {
    this.logger = injector.get(LogService);

    this.logger.info('SignalrService init');

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(url, signalR.HttpTransportType.WebSockets)
      .withAutomaticReconnect()
      .build();

    this.connection.onreconnected(() => console.info('SignalrService reconnected'));

    this.logger.info('SignalrService inited: ' + this.connection.state);
  }
  
  public async start(): Promise<void> {
    this.logger.info('SignalrService start');
    await this.connection.start();
    this.logger.info('SignalrService started ' + this.connection.state);
  }

  public async stop(): Promise<void> {
    this.logger.info('SignalrService stop');
    await this.connection.stop();
    this.logger.info('SignalrService stoped ' + this.connection.state);
  }

  public invoke<T>(methodName: string, ...args: any[]): Promise<T> {
    this.logger.debug('SignalrService invoke ' + methodName);
    return this.connection.invoke<T>(methodName, ...args);
  }

  public send(methodName: string, ...args: any[]): Promise<void> {
    this.logger.debug('SignalrService send ' + methodName);
    return this.connection.send(methodName, args);
  }

  public on(methodName: string, method: (...args: any[]) => void): void {
    this.connection.on(methodName, method);
  }

  public off(methodName: string, method: (...args: any[]) => void): void {
    this.connection.off(methodName, method);
  }
}
