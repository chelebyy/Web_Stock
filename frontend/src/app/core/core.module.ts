import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { BaseHttpService } from './services/base-http.service';
import { ErrorService } from './services/error.service';
import { LogService } from './services/log.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    BaseHttpService,
    ErrorService,
    LogService
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule zaten AppModule tarafından import edilmiş. CoreModule sadece AppModule tarafından import edilmelidir.'
      );
    }
  }
} 