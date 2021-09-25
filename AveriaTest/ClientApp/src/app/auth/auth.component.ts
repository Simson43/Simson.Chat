import { Component } from '@angular/core';
import { ChatService } from '../services/signalr/chat.service';

@Component({
  selector: 'auth-component',
  templateUrl: './auth.component.html'
})
export class AuthComponent {

  constructor(
    private chatService: ChatService
  ) {

  }

  public async login(userName: string): Promise<void> {
    let result = await this.chatService.login(userName);
    console.warn(result)
  }
}
