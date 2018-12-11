import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { TriangleComponent } from './triangle/triangle.component';
import { TriangleService } from './triangle/triangle.service';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,            
    TriangleComponent
  ],
  providers: [
    TriangleService
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: TriangleComponent, pathMatch: 'full' }      
    ]),
    ReactiveFormsModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
