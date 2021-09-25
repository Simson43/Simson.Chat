import { Component, OnDestroy, OnInit } from '@angular/core';
import { ChatService } from '../services/signalr/chat.service';

@Component({
  selector: 'chat-component',
  templateUrl: './chat.component.html'
})
export class ChatComponent implements OnInit, OnDestroy {



  constructor(
    private chatService: ChatService
  ) {

  }

  ngOnInit(): void {
    //this.chatService.
  }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }
}
