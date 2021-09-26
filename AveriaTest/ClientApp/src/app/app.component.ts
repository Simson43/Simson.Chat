import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { ConfigService } from './services/config.service';
import { ChatService } from './services/signalr/chat.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent implements OnInit {
  ready: boolean;

  constructor(
    private configService: ConfigService,
    private injector: Injector,
    private cdr: ChangeDetectorRef
  ) {

  }

  async ngOnInit(): Promise<void> {
    await this.configService.init();
    let chatService = this.injector.get(ChatService);
    await chatService.connect();

    this.ready = true;
    this.cdr.detectChanges();
  }
}
