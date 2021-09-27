import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { Message } from '../../models/message.model';
import { User } from '../../models/user.model';
import { ChatService } from '../../services/signalr/chat.service';

interface MessageGroup {
  user: User;
  messages: Message[];
}

@Component({
  selector: 'messages-stream-component',
  templateUrl: './messages-stream.component.html',
  styleUrls: ['./messages-stream.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessagesStreamComponent implements OnChanges, OnInit, OnDestroy {

  @Input() currentUser: User;
  @Input() messages: Message[];

  messageGroups: MessageGroup[];
  width = 25;

  private subscription: Subscription;
  private containerKey = 'container';

  constructor(
    private chatService: ChatService,
    private cdr: ChangeDetectorRef
  ) {

  }

  private scroll(): void {
    let elements = document.getElementsByClassName(this.containerKey);
    if (elements.length > 0) {
      let container = elements[0];
      container.parentElement.scrollTo(0, container.parentElement.scrollHeight)
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.messageGroups = this.toMessageGroups(this.messages);
    this.updateStreamMessages();
  }

  ngOnInit(): void {
    this.subscription = this.chatService.messageReceived.subscribe(x => this.pushMesasge(x));
  }

  ngOnDestroy(): void {
    if (this.subscription)
      this.subscription.unsubscribe();
  }

  private toMessageGroups(mesasges: Message[]): MessageGroup[] {
    let result: MessageGroup[] = [];

    if (this.messages && this.messages.length > 0) {
      for (let msg of this.messages) {
        let lastGroup = this.getLastGroup(result, msg.user);
        if (lastGroup) {
          lastGroup.messages.push(msg);
        }
        else {
          let newGroup = {
            user: msg.user,
            messages: [msg]
          };
          result.push(newGroup);
        }
      }
    }

    return result;
  }

  private pushMesasge(message: Message): void {
    var group = this.getLastGroup(this.messageGroups, message.user, true);
    group.messages.push(message);
    this.updateStreamMessages();
  }

  private getLastGroup(groups: MessageGroup[], user: User, create: boolean = false): MessageGroup {
    if (groups.length > 0) {
      let lastGroup = groups[groups.length - 1];
      if (lastGroup.user.name == user.name) {
        return lastGroup;
      }
    }
    if (create) {
      let result = {
        user: user,
        messages: []
      };
      groups.push(result);
      return result;
    }
    return undefined;
  }

  private updateStreamMessages(): void {
    this.cdr.detectChanges();
    this.scroll();
  }
}
