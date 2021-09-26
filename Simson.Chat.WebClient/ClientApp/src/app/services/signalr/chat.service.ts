import { EventEmitter, Injectable, Injector } from "@angular/core";
import { Context } from "../../models/context.model";
import { Message } from "../../models/message.model";
import { User } from "../../models/user.model";
import { ConfigService } from "../config.service";
import { SignalrService } from "./signalr.service";

@Injectable({
  providedIn: "root"
})
export class ChatService {

  public messageReceived: EventEmitter<Message> = new EventEmitter<Message>();
  public userStatusChanged: EventEmitter<User> = new EventEmitter<User>();

  private signalr: SignalrService;

  public get connected(): boolean {
    return this.signalr.connected;
  }

  constructor(
    private config: ConfigService,
    injector: Injector,
  ) {
    this.signalr = new SignalrService(injector, this.config.AppSettings.signalR.url);
    this.subscribe();
  }

  private subscribe(): void {
    this.signalr.on('OnMessageReceived', x => this.messageReceived.emit(x));
    this.signalr.on('OnUserStatusChanged', x => this.userStatusChanged.emit(x));
  }

  public connect(): Promise<void> {
    return this.signalr.start();
  }

  public close(): Promise<void> {
    return this.signalr.stop();
  }

  public login(username: string): Promise<Context> {
    return this.signalr.invoke("Login", username);
  }

  public logout(username: string): Promise<void> {
    return this.signalr.invoke("Logout", username);
  }

  public sendMessage(message: Message): Promise<void> {
    return this.signalr.invoke("SendMessage", message);
  }
}
