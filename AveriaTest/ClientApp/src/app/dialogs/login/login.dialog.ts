import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoginReason } from '../../models/login-reason.model';
import { ChatService } from '../../services/signalr/chat.service';

@Component({
  selector: 'login-dialog',
  templateUrl: './login.dialog.html',
  styleUrls: ['./login.dialog.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginDialog {
  errorMessage: string;

  constructor(
    public dialogRef: MatDialogRef<LoginDialog>,
    @Inject(MAT_DIALOG_DATA) public data,
    private chatService: ChatService,
    private cdr: ChangeDetectorRef
  ) {

  }

  async login(userName: string): Promise<void> {
    var result = await this.chatService.login(userName);
    if (this.validateResult(result.reason)) {
      this.dialogRef.close({ userName: userName, context: result });
    }
  }

  private validateResult(reason: LoginReason): boolean {
    switch (reason) {
      case LoginReason.Success:
        return true;
      case LoginReason.AlreadyExists:
        this.errorMessage = "The User name already exists";
        break;
      case LoginReason.IncorrectUserName:
        this.errorMessage = "The User name is incorrect";
        break;
      default:
        this.errorMessage = "Unknown error";
        break;
    }
    this.cdr.detectChanges();
    return false;
  }
}
