import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserStatus } from '../../models/user-status.model';
import { User } from '../../models/user.model';
import { ChatService } from '../../services/signalr/chat.service';

@Component({
  selector: 'users-bar-component',
  templateUrl: './users-bar.component.html',
  styleUrls: ['./users-bar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UsersBarComponent implements OnInit, OnDestroy {

  @Input() currentUser: User;
  @Input() users: User[];

  private subscription: Subscription;

  constructor(
    private chatService: ChatService,
    private cdr: ChangeDetectorRef
  ) {

  }

  ngOnInit(): void {
    this.subscription = this.chatService.userStatusChanged.subscribe(x => this.updateUsers(x));
  }

  ngOnDestroy(): void {
    if (this.subscription)
      this.subscription.unsubscribe();
  }

  private updateUsers(user: User): void {
    if (!this.users)
      this.users = [];
    if (user.status == UserStatus.Offline)
      this.users = this.users.filter(x => x.name != user.name);
    else
      this.users.push(user);
    this.cdr.detectChanges();
  }
}
