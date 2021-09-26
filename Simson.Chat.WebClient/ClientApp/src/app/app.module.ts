import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';

import { ChatComponent } from './pages/chat/chat.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule, FlexModule } from '@angular/flex-layout';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { MessageInputComponent } from './ui/message-input/message-input.component';
import { MessagesStreamComponent } from './ui/messages-stream/messages-stream.component';
import { UsersBarComponent } from './ui/users-bar/users-bar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginDialog } from './dialogs/login/login.dialog';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ChatComponent,
    MessageInputComponent,
    MessagesStreamComponent,
    UsersBarComponent,

    LoginDialog,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,

    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatCardModule,
    MatInputModule,
    MatSnackBarModule,

    MatCheckboxModule,
    MatSelectModule,
    MatGridListModule,
    FlexLayoutModule,
    FlexModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatSidenavModule,
    MatButtonModule,
    MatListModule,

    RouterModule.forRoot([
      {
        path: '',
        component: ChatComponent,
      }
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
