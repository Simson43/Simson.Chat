import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AuthComponent } from './auth/auth.component';

import { MatSidenavModule } from '@angular/material/sidenav';
import { ChatComponent } from './chat/chat.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule, FlexModule } from '@angular/flex-layout';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthComponent,
    ChatComponent,

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,

    MatSidenavModule,

    ReactiveFormsModule,
    //MatDialogModule,
    //MatCardModule,
    MatInputModule,
    //MatSnackBarModule,

    //MatCheckboxModule,
    //MatSelectModule,
    //MatGridListModule,
    FlexLayoutModule,
    FlexModule,
    //MatProgressSpinnerModule,
    //MatIconModule,
    MatButtonModule,
    MatListModule,

    FormsModule,
    RouterModule.forRoot([
        { path: '', component: AuthComponent, pathMatch: 'full' }
      ],
      //{ relativeLinkResolution: 'legacy' }
    )
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
