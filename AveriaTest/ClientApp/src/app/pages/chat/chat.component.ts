import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginDialog } from '../../dialogs/login/login.dialog';
import { Context } from '../../models/context.model';
import { Message } from '../../models/message.model';
import { User } from '../../models/user.model';
import { ChatService } from '../../services/signalr/chat.service';

@Component({
  selector: 'chat-component',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatComponent implements OnInit {

  currentUser: User;
  users: User[];
  messages: Message[];

  constructor(
    private chatService: ChatService,
    private dialogs: MatDialog,
    private cdr: ChangeDetectorRef
  ) {

  }

  async ngOnInit(): Promise<void> {
    await this.login();
  }

  public async login(): Promise<void> {
    let result: { userName: string, context: Context } = await this.dialogs.open(LoginDialog, {
      disableClose: true
    }).afterClosed().toPromise();

    if (result) {
      let context = result.context;
      this.currentUser = context.users.find(x => x.name == result.userName);
      this.users = context.users;
      this.messages = context.messages;
      this.cdr.detectChanges();
    }
  }

  public async sendMessage(text: string): Promise<void> {
    var message: Message = {
      user: this.currentUser,
      text: text
    }
    await this.chatService.sendMessage(message);
  }
}
