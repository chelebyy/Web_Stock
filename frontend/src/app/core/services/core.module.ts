import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { BaseHttpService } from './base-http.service';
import { LogService } from './log.service';
import { ErrorService } from './error.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    BaseHttpService,
    LogService,
    ErrorService
  ],
  exports: []
})
export class CoreModule { } 