import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';

@Component({
  selector: 'message-input-component',
  templateUrl: './message-input.component.html',
  styleUrls: ['./message-input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessageInputComponent {

  @Output() onSubmit: EventEmitter<string> = new EventEmitter<string>();

  public async submit(text: string): Promise<void> {
    this.onSubmit.emit(text)
  }
}
